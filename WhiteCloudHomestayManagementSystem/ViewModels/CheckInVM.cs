using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhiteCloudHomestayManagementSystem.Models;

namespace WhiteCloudHomestayManagementSystem.ViewModels
{
    public class CheckInVM
    {
        public List<Homestay> Homestays { get; set; }
        public Booking Booking { get; set; }
        public Customer Customer { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}