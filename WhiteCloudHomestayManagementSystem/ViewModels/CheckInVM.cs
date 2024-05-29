using System;
using System.ComponentModel.DataAnnotations;
using WhiteCloudHomestayManagementSystem.Models;

namespace WhiteCloudHomestayManagementSystem.ViewModels
{
    public class CheckInViewModel
    {
        [Required(ErrorMessage = "Please select a customer")]
        public Guid CustomerId { get; set; }

        [Required(ErrorMessage = "Please select a homestay")]
        public Guid HomestayId { get; set; }

        [Required(ErrorMessage = "Please select an employee")]
        public Guid EmployeeId { get; set; }

        [Required(ErrorMessage = "Please enter check-in date")]
        [DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; }

        [Required(ErrorMessage = "Please enter check-out date")]
        [DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; }

        [Required(ErrorMessage = "Please enter number of adults")]
        public int NumberOfAdults { get; set; }

        public int? NumberOfChildren { get; set; }

        public string SpecialRequests { get; set; }

        public string Notes { get; set; }

        [Required(ErrorMessage = "Please select a payment type")]
        public int PaymentTypeId { get; set; }

        [Required(ErrorMessage = "Please enter the price")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public decimal Price { get; set; }
        public Customer Customer { get; set; }
        public Customer NewCustomer { get; set; }


    }
}
