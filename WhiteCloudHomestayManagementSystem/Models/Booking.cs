//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WhiteCloudHomestayManagementSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Booking
    {
        public int BookingID { get; set; }
        public Nullable<int> HomestayID { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public System.DateTime CheckInDate { get; set; }
        public System.DateTime CheckOutDate { get; set; }
        public string Status { get; set; }
        public bool Paid { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual Homestay Homestay { get; set; }
    }
}