using SmartRentalApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SmartRentalApp.Controllers
{
    public class HomeController : Controller
    {
        private SmartRentalDBEntities db = new SmartRentalDBEntities();
        private ApplicationDbContext identityDB = new ApplicationDbContext();

        public ActionResult Index()
        {
            var realEstates = db.RealEStates.Where(s => s.RealEstateStatus.Value).OrderByDescending(r => r.CreatedOn).Take(3);
            return View(realEstates.ToList());
        }

        public ActionResult AdvancedSearch(byte? typeID, float? areaFrom, float? areaTo, int? districtID, int? roomsNo, int? bathsNo)
        {
            var realEstates = from r in db.RealEStates
                              where r.RealEstateStatus.Value
                              select r;
            if (typeID.HasValue)
            {
                realEstates = realEstates.Where(r => r.TypeID == typeID.Value);
                ViewBag.SelectedType = typeID;
            }
            if (areaFrom.HasValue && areaTo.HasValue)
            {
                realEstates = realEstates.Where(r => r.Area >= areaFrom.Value && r.Area <= areaTo.Value);
                ViewBag.AreaFrom = areaFrom;
                ViewBag.AreaTo = areaTo;
            }
            else if (areaFrom.HasValue && !areaTo.HasValue)
            {
                realEstates = realEstates.Where(r => r.Area >= areaFrom.Value);
                ViewBag.AreaFrom = areaFrom;
            }
            else if (!areaFrom.HasValue && areaTo.HasValue)
            {
                realEstates = realEstates.Where(r => r.Area <= areaTo.Value);
                ViewBag.AreaTo = areaTo;
            }
            if (districtID.HasValue)
            {
                realEstates = realEstates.Where(r => r.DistrictID == districtID.Value);
                ViewBag.SelectedDistrict = districtID;
            }
            if (roomsNo.HasValue)
                realEstates = realEstates.Where(r => r.ResidentialRealEstate.RoomsNo == roomsNo.Value);
            if (bathsNo.HasValue)
                realEstates = realEstates.Where(r => r.ResidentialRealEstate.BathsNo == bathsNo.Value);
            if (districtID.HasValue)
                ViewBag.DistrictID = new SelectList(db.RealEstatesDistricts, "DistrictID", "DistrictName", new { DistrictID = districtID.Value });
            else
                ViewBag.DistrictID = new SelectList(db.RealEstatesDistricts, "DistrictID", "DistrictName");
            if (typeID.HasValue)
                ViewBag.TypeID = new SelectList(db.RealEStateTypes, "TypeID", "TypeName", new { TypeID = typeID.Value });
            else
                ViewBag.TypeID = new SelectList(db.RealEStateTypes, "TypeID", "TypeName");

            return View(realEstates.ToList());
        }

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
            ViewBag.AgentPhone = identityDB.Users.Where(u => u.Id == realEState.CreatedBy).FirstOrDefault().PhoneNumber;
            return View(realEState);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}