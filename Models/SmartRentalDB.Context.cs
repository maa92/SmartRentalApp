﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SmartRentalDBEntities : DbContext
    {
        public SmartRentalDBEntities()
            : base("name=SmartRentalDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CustomersFavorite> CustomersFavorites { get; set; }
        public virtual DbSet<RealEstateComment> RealEstateComments { get; set; }
        public virtual DbSet<RealEStateImage> RealEStateImages { get; set; }
        public virtual DbSet<RealEStatePurpos> RealEStatePurposes { get; set; }
        public virtual DbSet<RealEState> RealEStates { get; set; }
        public virtual DbSet<RealEStatesCity> RealEStatesCities { get; set; }
        public virtual DbSet<RealEstatesDistrict> RealEstatesDistricts { get; set; }
        public virtual DbSet<RealEStateType> RealEStateTypes { get; set; }
        public virtual DbSet<ResidentialRealEstate> ResidentialRealEstates { get; set; }
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
    }
}
