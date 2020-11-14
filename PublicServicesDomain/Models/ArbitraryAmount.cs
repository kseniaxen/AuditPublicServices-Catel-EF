using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicServicesDomain.Models
{
    public class ArbitraryAmount
    {
        public int Id { get; set; }
        public Rate Rate { get; set; }
        public Address Address { get; set; }
        public Service Service { get; set; }
        public double Total { get; set; }
        public string DatePaid { get; set; }
    }
}
