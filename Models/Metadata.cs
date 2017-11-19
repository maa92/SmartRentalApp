using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartRentalApp.Models
{
    public class RealEStateMetadata
    {
        [Required]
        [Display(Name = "Property Type")]
        public byte TypeID { get; set; }

        [Required]
        [StringLength(2000)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [Range(60, 1500, ErrorMessage = "Invalid Size for Real Estate. Allowed Size: 60 - 1000 SQ.M")]
        [DisplayFormat(DataFormatString = "{0} SQ.M")]
        [Display(Name = "Size")]
        public double Area { get; set; }

        [Required]
        [StringLength(200)]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Purpose")]
        public byte PurposeID { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "SR {0:F}")]
        [Display(Name = "Min Price")]
        public decimal MinPrice { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "SR {0:F}")]
        [Display(Name = "Max Price")]
        //[GreaterThan("MinPrice", ErrorMessage = "Max Price must be greater than Min Price!")]
        public decimal MaxPrice { get; set; }

        [Required]
        [Display(Name = "District")]
        public int DistrictID { get; set; }

        [Required]
        public string Latitude { get; set; }

        [Required]
        public string Longitude { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Date Added")]
        public System.DateTime CreatedOn { get; set; }

        [Display(Name = "Added By")]
        public string CreatedBy { get; set; }
    }

    public class ResidentialRealEstateMetadata
    {
        [Range(1, 10, ErrorMessage = "Invalid Rooms Number. Allowed number between 1 and 10.")]
        [Display(Name = "Rooms#")]
        public int RoomsNo { get; set; }

        [Range(1, 10, ErrorMessage = "Invalid Baths Number. Allowed number between 1 and 10.")]
        [Display(Name = "Baths#")]
        public int BathsNo { get; set; }

        [Display(Name = "With Garden")]
        public bool WithGarden { get; set; }

        [Display(Name = "With Roof")]
        public bool WithRoof { get; set; }

        [Range(1, 10, ErrorMessage = "Invalid Level Number. Allowed number between 1 and 10.")]
        [Display(Name = "Level#")]
        public Nullable<int> LevelNo { get; set; }

        [Range(1, 20, ErrorMessage = "Invalid Level Number. Allowed number between 1 and 20.")]
        [Display(Name = "House#")]
        public Nullable<int> HouseNo { get; set; }
    }

    public class RealEstatesDistrictMetadata
    {
        [Display(Name = "ID")]
        public int DistrictID { get; set; }

        [Display(Name = " District")]
        public string DistrictName { get; set; }

        [Display(Name = "City ID")]
        public byte CityID { get; set; }
    }

    public class RealEStatesCityMetadata
    {
        [Display(Name = "City ID")]
        public byte CityID { get; set; }

        [Display(Name = "City")]
        public string CityName { get; set; }
    }

    public class RealEStateTypeMetadata
    {
        [Display(Name = "Type ID")]
        public byte TypeID { get; set; }

        [Display(Name = "Type")]
        public string TypeName { get; set; }
    }

    public class RealEStatePurposMetadata
    {
        [Display(Name = "Purpose ID")]
        public byte PurposeID { get; set; }

        [Display(Name = "Purpose")]
        public string PurposeName { get; set; }
    }
}