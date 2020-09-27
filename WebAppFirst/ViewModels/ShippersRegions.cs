namespace WebAppFirst.ViewModels
{
    using System;
    public class ShippersRegions
    {
        public int ShipperID { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public Nullable<int> RegionID { get; set; }
        public string RegionDescription { get; set; }
    }
}