using System;
using System.Collections.Generic;
using System.Text;

namespace Sakila.Models
{
    public class Rental
    {
        public int RentalId { get; set; }
        public string Title { get; set; }
        public string StoreName { get; set; }
        public int StoreId { get; set; }
        public int CustomerId { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public uint StaffId { get; set; }
    }
}
 