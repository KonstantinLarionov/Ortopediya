using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ortopediya.Models.Objects
{
    public class Partner : User
    {
        public int Id { get; set; } // Возможно не нужно
        public string Code { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        
    }
}
