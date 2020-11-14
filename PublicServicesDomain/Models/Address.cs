using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicServicesDomain.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public User User { get; set; }
    }
}
