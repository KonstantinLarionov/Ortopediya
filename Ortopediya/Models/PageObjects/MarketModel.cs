using Ortopediya.Models.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ortopediya.Models.PageObjects
{
    public class MarketModel
    {
        public List<Product> Products { get; set; }
        public User User { get; set; }
    }
}
