namespace HappyPath.Services.Migrations
{
    using HappyPath.Services.Data;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<HappyPathDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(HappyPathDbContext context)
        {
            //  This method will be called after migrating to the latest version.
        }
    }
}
