using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WhiteCloudHomestayManagementSystem.ViewModels
{
    public class NewCustomerVM

    {
        [Required(ErrorMessage = "Customer Name is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        public string Phone { get; set; }
        [Display(Name = "ID Card Image")]
        public string IdCardImg { get; set; }

        [Display(Name = "ID Card Number")]
        public string IdCardNum { get; set; }
    }
}