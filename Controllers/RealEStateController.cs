using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SmartRentalApp.Models;
using System.IO;

namespace SmartRentalApp.Controllers
{
    public class RealEStateController : Controller
    {
        private SmartRentalDBEntities db = new SmartRentalDBEntities();

        [Authorize(Roles = "Admin, Agent")]
        // GET: RealEState
        public ActionResult Index()
        {
            var realEStates = db.RealEStates
                .Include(r => r.RealEStatePurpos)
                .Include(r => r.RealEstatesDistrict)
                .Include(r => r.RealEStateType)
                .Include(r => r.ResidentialRealEstate);
            //.Where(r => r.CreatedBy == Utilities.GetLoggedInUser.Id);

            if (User.Identity.IsAuthenticated && User.IsInRole("Agent"))
                realEStates = realEStates.Where(r => r.CreatedBy == Utilities.GetLoggedInUser.Id);

            return View(realEStates.ToList());
        }

        public ActionResult SendReportToAdmin()
        {
            return View();
        }

        // GET: RealEState/Details/5
        [Authorize(Roles = "Admin, Agent")]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RealEState realEState = db.RealEStates.Find(id);
            if (realEState == null)
            {
                return HttpNotFound();
            }
            return View(realEState);
        }


        // GET: RealEState/Create
        [Authorize(Roles = "Agent")]
        public ActionResult Create()
        {
            ViewBag.PurposeID = new SelectList(db.RealEStatePurposes, "PurposeID", "PurposeName");
            ViewBag.DistrictID = new SelectList(db.RealEstatesDistricts, "DistrictID", "DistrictName");
            ViewBag.TypeID = new SelectList(db.RealEStateTypes, "TypeID", "TypeName");
            ViewBag.RealEStateID = new SelectList(db.ResidentialRealEstates, "RealEstateID", "RealEstateID");
            return View();
        }

        // POST: RealEState/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Agent")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RealEState realEState, FormCollection residentialData)
        {
            if (ModelState.IsValid)
            {
                realEState.CreatedOn = DateTime.Now;
                realEState.CreatedBy = Utilities.GetLoggedInUser.Id;
                realEState.RealEstateStatus = true;
                db.RealEStates.Add(realEState);
                db.SaveChanges();

                if (realEState.TypeID == 1
                    || realEState.TypeID == 2
                    || realEState.TypeID == 5)
                {
                    ResidentialRealEstate residential = new ResidentialRealEstate()
                    {
                        RealEstateID = realEState.RealEStateID,
                        BathsNo = Convert.ToInt32(residentialData["bathsNo"]),
                        RoomsNo = Convert.ToInt32(residentialData["roomsNo"]),
                        HouseNo = Convert.ToInt32(residentialData["houseNo"]),
                        LevelNo = Convert.ToInt32(residentialData["levelNo"]),
                        WithGarden = residentialData["withGarden"] != null ? true : false,
                        WithRoof = residentialData["withRoof"] != null ? true : false
                    };

                    db.ResidentialRealEstates.Add(residential);
                    db.SaveChanges();
                }

                Utilities.SendSubscriptionEmail(realEState.DistrictID, realEState.RealEStateID);

                return RedirectToAction("Index");
            }

            ViewBag.PurposeID = new SelectList(db.RealEStatePurposes, "PurposeID", "PurposeName", realEState.PurposeID);
            ViewBag.DistrictID = new SelectList(db.RealEstatesDistricts, "DistrictID", "DistrictName", realEState.DistrictID);
            ViewBag.TypeID = new SelectList(db.RealEStateTypes, "TypeID", "TypeName", realEState.TypeID);
            ViewBag.RealEStateID = new SelectList(db.ResidentialRealEstates, "RealEstateID", "RealEstateID", realEState.RealEStateID);
            return View(realEState);
        }

        // GET: RealEState/Edit/5
        [Authorize(Roles = "Agent")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RealEState realEState = db.RealEStates.Find(id);
            if (realEState == null)
            {
                return HttpNotFound();
            }
            ViewBag.PurposeID = new SelectList(db.RealEStatePurposes, "PurposeID", "PurposeName", realEState.PurposeID);
            ViewBag.DistrictID = new SelectList(db.RealEstatesDistricts, "DistrictID", "DistrictName", realEState.DistrictID);
            ViewBag.TypeID = new SelectList(db.RealEStateTypes, "TypeID", "TypeName", realEState.TypeID);
            ViewBag.RealEStateID = new SelectList(db.ResidentialRealEstates, "RealEstateID", "RealEstateID", realEState.RealEStateID);
            return View(realEState);
        }

        // POST: RealEState/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Agent")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RealEStateID,TypeID,Description,Area,Address,PurposeID,MinPrice,MaxPrice,DistrictID,Latitude,Longitude,CreatedOn,CreatedBy")] RealEState realEState,
            FormCollection residentialData)
        {
            if (ModelState.IsValid)
            {
                db.Entry(realEState).State = EntityState.Modified;

                if (realEState.TypeID == 1
                    || realEState.TypeID == 2
                    || realEState.TypeID == 5)
                {
                    var residential = db.ResidentialRealEstates.Find(realEState.RealEStateID);
                    if (residential != null)
                    {
                        residential.BathsNo = Convert.ToInt32(residentialData["bathsNo"]);
                        residential.RoomsNo = Convert.ToInt32(residentialData["roomsNo"]);
                        residential.HouseNo = Convert.ToInt32(residentialData["houseNo"]);
                        residential.LevelNo = Convert.ToInt32(residentialData["levelNo"]);
                        residential.WithGarden = residentialData["withGarden"] != null ? true : false;
                        residential.WithRoof = residentialData["withRoof"] != null ? true : false;

                        db.Entry(residential).State = EntityState.Modified;
                    }
                }
                else
                {
                    var residential = db.ResidentialRealEstates.Find(realEState.RealEStateID);
                    if (residential != null)
                        db.ResidentialRealEstates.Remove(residential);
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PurposeID = new SelectList(db.RealEStatePurposes, "PurposeID", "PurposeName", realEState.PurposeID);
            ViewBag.DistrictID = new SelectList(db.RealEstatesDistricts, "DistrictID", "DistrictName", realEState.DistrictID);
            ViewBag.TypeID = new SelectList(db.RealEStateTypes, "TypeID", "TypeName", realEState.TypeID);
            ViewBag.RealEStateID = new SelectList(db.ResidentialRealEstates, "RealEstateID", "RealEstateID", realEState.RealEStateID);
            return View(realEState);
        }

        // GET: RealEState/Delete/5
        [Authorize(Roles = "Agent")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RealEState realEState = db.RealEStates.Find(id);
            if (realEState == null)
            {
                return HttpNotFound();
            }
            return View(realEState);
        }

        // POST: RealEState/Delete/5
        [Authorize(Roles = "Agent")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ResidentialRealEstate residential = db.ResidentialRealEstates.Find(id);
            if (residential != null)
                db.ResidentialRealEstates.Remove(residential);

            RealEState realEState = db.RealEStates.Find(id);
            db.RealEStates.Remove(realEState);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpGet]
        [Authorize(Roles = "Agent")]
        public ActionResult ListImages(long id)
        {
            var images = db.RealEStateImages.Where(i => i.RealEStateID == id);
            ViewBag.RealEstateID = id;
            return View(images.ToList());
        }

        [Authorize(Roles = "Agent")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadImage(long? id)
        {
            try
            {
                var allowedExtensions = new[] { ".jpg", ".png", ".jpeg", "bmp" };

                HttpPostedFileBase file = Request.Files["realEsateImage"];

                if (file.ContentLength > (2048 * 1000))
                {
                    TempData["Message"] = "Maximum allowed size is 2MB.";
                    return RedirectToAction("ListImages", new { id = id });
                }

                var extension = Path.GetExtension(file.FileName);
                if (!allowedExtensions.Contains(extension))
                {
                    TempData["Message"] = "Selected file not allowed. Allowed types are: jpg, png, jpeg and bmp.";
                }
                else
                {
                    int fileLength = file.ContentLength;
                    byte[] fileData = new byte[fileLength];
                    file.InputStream.Read(fileData, 0, fileLength);

                    RealEStateImage image = new RealEStateImage()
                    {
                        ImageTitle = "Image",
                        RealEStateID = id.Value,
                        ImageDate = DateTime.Now,
                        ImageData = fileData
                    };

                    db.RealEStateImages.Add(image);
                    db.SaveChanges();
                }

                return RedirectToAction("ListImages", new { id = id });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
                return View("Error");
            }
        }

        public FileContentResult ShowImage(long id)
        {
            var image = (from i in db.RealEStateImages
                         where i.ImageID == id
                         select i.ImageData).FirstOrDefault();

            return new FileContentResult(image, "image/jpeg");
        }

        [HttpPost]
        public ActionResult WriteComment(long RealEstateID, string comment)
        {
            try
            {
                RealEstateComment aComment = new RealEstateComment()
                {
                    CommentContent = comment,
                    CommentDate = DateTime.Now,
                    CommentTitle = "Comment",
                    Username = Utilities.GetLoggedInUser.UserName,
                    RealEstateID = RealEstateID,
                    IsSpam = false
                };

                db.RealEstateComments.Add(aComment);
                db.SaveChanges();

                return RedirectToAction("Details", "Home", new { id = RealEstateID });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
                return View("Error");
            }
        }

        public ActionResult AddFavorite(int id)
        {
            try
            {
                var realEstate = db.RealEStates.Find(id);
                CustomersFavorite favorite = new CustomersFavorite()
                {
                    DistrictID = realEstate.DistrictID,
                    Username = Utilities.GetLoggedInUser.UserName,
                };

                db.CustomersFavorites.Add(favorite);
                db.SaveChanges();

                return RedirectToAction("Details", "Home", new { id = realEstate.RealEStateID });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
                return View("Error");
            }
        }

        [Authorize(Roles = "Agent")]
        public ActionResult ReportSpam(long id)
        {
            try
            {
                var comment = db.RealEstateComments.Find(id);
                comment.IsSpam = true;
                if (ModelState.IsValid)
                {
                    db.Entry(comment).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Details", new { id = comment.RealEstateID });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
                return View("Error");
            }
        }

        public ActionResult SearchRealEstate(FormCollection searchCriteria)
        {
            return View();
        }

        public ActionResult ReplyToComment(long commentID, string agentReply)
        {
            return View();
        }

        public ActionResult QuickSearch(string searchKey)
        {
            var realEstates = db.RealEStates.Where(r => (r.Description.Contains(searchKey) || r.Address.Contains(searchKey)) && r.RealEstateStatus.Value);

            return View("SearchResult", realEstates.ToList());
        }

        [Authorize(Roles = "Agent")]
        public ActionResult DeleteImage(long? id)
        {
            try
            {
                long realEstateID = 0;

                if (id.HasValue)
                {
                    RealEStateImage image = db.RealEStateImages.Find(id.Value);
                    realEstateID = image.RealEStateID;
                    db.RealEStateImages.Remove(image);
                    db.SaveChanges();
                }
                else
                {
                    TempData["Message"] = "Please select an image to delete!";
                }
                return RedirectToAction("ListImages", new { id = realEstateID });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }

        }

        [Authorize(Roles = "Agent")]
        public ActionResult UpdateStatus(long id)
        {
            var realEstate = db.RealEStates.Find(id);
            realEstate.RealEstateStatus = !realEstate.RealEstateStatus;
            db.Entry(realEstate).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult SpamComment()
        {
            var spam = db.RealEstateComments.Where(c => c.IsSpam);
            return View(spam.ToList());
        }

        public ActionResult DeleteComment(long id)
        {
            RealEstateComment comment = db.RealEstateComments.Find(id);
            db.RealEstateComments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("SpamComment");
        }

        public ActionResult ShowSubscriptions()
        {
            var subscriptions = db.CustomersFavorites.Where(c => c.Username == Utilities.GetLoggedInUser.UserName);
            return View(subscriptions.ToList());
        }

        public ActionResult DeleteSubscription(int id)
        {
            var subscription = db.CustomersFavorites.Find(id);
            db.CustomersFavorites.Remove(subscription);
            db.SaveChanges();
            return RedirectToAction("ShowSubscriptions");
        }
    }
}
