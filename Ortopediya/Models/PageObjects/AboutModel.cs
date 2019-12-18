using Ortopediya.Models.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ortopediya.Models.PageObjects
{
    public class AboutModel
    {
        public List<Category> Categories { get; set; }
        public About Abouts { get; set; }
        public Contact Contacts { get; set; }
    }
}
