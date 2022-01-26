using Microsoft.EntityFrameworkCore;

namespace Order.API.Models
{
    [Owned]
    public class Address
    {
        public string Line { get; set; }
        public string Province { get; set; }
        public string Distrcit { get; set; }
    }
}
