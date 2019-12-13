using Microsoft.EntityFrameworkCore;
using Ortopediya.Models.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ortopediya.Models.Entitys
{
    public class FrontContext : DbContext
    {
        DbSet<About> Abouts { get; set; }
        DbSet<Baner> Baners { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Click> Clicks { get; set; }
        DbSet<Coming> Comings { get; set; }
        DbSet<Contact> Contacts { get; set; }
        DbSet<Expend> Expends { get; set; }
        DbSet<Partner> Partners { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<Request> Requests { get; set; }
        DbSet<Rule> Rule { get; set; }
        DbSet<Rules> Rules { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<IP> IPs { get; set; }
        public FrontContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("Server=localhost;Database=BUData;User=root;Password=Thehorde;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
