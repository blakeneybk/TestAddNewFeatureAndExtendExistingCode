namespace Sakila.Models
{
    public class CustomerOutstandingRentals
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int OutstandingRentals { get; set; }
    }
}
