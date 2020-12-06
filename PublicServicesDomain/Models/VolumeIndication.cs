using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicServicesDomain.Models
{
    public class VolumeIndication
    {
        public int Id { get; set; }
        public Rate Rate { get; set; }
        public Address Address { get; set; }
        public Service Service { get; set; }
        public int PrevIndication { get; set; }
        public int CurIndication { get; set; }
        public decimal Total { get; set; }
        public DateTime DatePaid { get; set; }
        public User User { get; set; }
    }
}
