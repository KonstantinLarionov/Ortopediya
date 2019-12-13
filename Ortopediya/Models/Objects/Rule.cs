using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ortopediya.Models.Objects
{
    public class Rule
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime LastDateTimeEdit { get; set; }
    }
}
