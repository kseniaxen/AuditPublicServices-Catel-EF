using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicServicesDomain.Models
{
    public class Rate
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string MeasureTitle { get; set; }
        public double Price { get; set; }
        public User User { get; set; }
    }
}
