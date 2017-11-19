using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartRentalApp.Models
{
    [MetadataType(typeof(RealEStateMetadata))]
    public partial class RealEState
    {
    }

    [MetadataType(typeof(ResidentialRealEstateMetadata))]
    public partial class ResidentialRealEstate
    {
    }

    [MetadataType(typeof(RealEstatesDistrictMetadata))]
    public partial class RealEstatesDistrict
    {
    }

    [MetadataType(typeof(RealEStatesCityMetadata))]
    public partial class RealEStatesCity
    {
    }

    [MetadataType(typeof(RealEStateTypeMetadata))]
    public partial class RealEStateType
    {
    }

    [MetadataType(typeof(RealEStatePurposMetadata))]
    public partial class RealEStatePurpos
    {
    }
}