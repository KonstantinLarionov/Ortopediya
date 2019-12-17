using Ortopediya.Models.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ortopediya.Models.PageObjects
{
    public class RulesModel
    {
        public List<Category> Categories { get; set; }

        public Contact Contact { get; set; }
        public Rules Rules { get; set; }
    }
}
