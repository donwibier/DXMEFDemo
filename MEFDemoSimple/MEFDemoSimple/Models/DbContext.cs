using Contracts;
using MEFDemoSimple.Code;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MEFDemoSimple.Models
{
    public class AppDBContext : DbContext, IDBRepository
    {

        public AppDBContext() : base("ApplicationServices")
        {
        }

        public DbSet<Person> Persons { get; set; }

        public bool Add<T>(T item) where T : class
        {
            try
            {
                Set<T>().Add(item);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Save()
        {
            try
            {
                SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().ToTable("Persons", "dbo");
            modelBuilder.Entity<Person>().HasKey(_a => _a.Id);

            Manager.ForEachExport<IDBPluginContext>(p => p.Setup(modelBuilder));
        }
    }
}