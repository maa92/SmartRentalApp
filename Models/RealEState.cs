//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmartRentalApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class RealEState
    {
        public RealEState()
        {
            this.RealEstateComments = new HashSet<RealEstateComment>();
            this.RealEStateImages = new HashSet<RealEStateImage>();
        }
    
        public long RealEStateID { get; set; }
        public byte TypeID { get; set; }
        public string Description { get; set; }
        public double Area { get; set; }
        public string Address { get; set; }
        public byte PurposeID { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public int DistrictID { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<bool> RealEstateStatus { get; set; }
    
        public virtual ICollection<RealEstateComment> RealEstateComments { get; set; }
        public virtual ICollection<RealEStateImage> RealEStateImages { get; set; }
        public virtual RealEStatePurpos RealEStatePurpos { get; set; }
        public virtual RealEstatesDistrict RealEstatesDistrict { get; set; }
        public virtual RealEStateType RealEStateType { get; set; }
        public virtual ResidentialRealEstate ResidentialRealEstate { get; set; }
    }
}
