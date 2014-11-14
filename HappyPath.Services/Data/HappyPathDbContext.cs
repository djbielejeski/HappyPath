using HappyPath.Services.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyPath.Services.Data
{
    public class HappyPathDbContext : DbContext
    {
        public HappyPathDbContext() : base()
        {
            Database.SetInitializer<HappyPathDbContext>(null);
            Database.CommandTimeout = 300;
        }

        private DbSet<TestItem> Items { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            this.Configuration.LazyLoadingEnabled = false;
        }
    }
}
