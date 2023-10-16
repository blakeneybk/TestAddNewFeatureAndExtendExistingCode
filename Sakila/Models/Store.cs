using System;
using System.Collections.Generic;
using System.Text;

namespace Sakila.Models
{
    public class Store
    {
        public int StoreId { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string District { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
