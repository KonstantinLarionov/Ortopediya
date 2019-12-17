using Ortopediya.Models.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ortopediya.Models.PageObjects
{
    public class PartnerModel
    {
        public List<Category> Categories { get; set; }

        public Partner Partner { get; set; }
        public Contact Contacts { get; set; }
    }
}
