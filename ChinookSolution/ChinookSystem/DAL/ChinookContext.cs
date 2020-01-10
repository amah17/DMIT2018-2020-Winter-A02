using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Add Namespaces
using System.Data.Entity;
using ChinookSystem.Data.Entities;
#endregion
namespace ChinookSystem.DAL
{
    //restrict access to the database interface to within the project
    //inherit EntityFramework interface DbContext
    internal class ChinookContext:DbContext
    {
        //Constructer will pass the connection string name to the database to EntityFramework via DbContext
        public ChinookContext() : base("ChinookDB")
        {
        }
        //Create a DbSet<T> for each Entity
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Album> Albums { get; set; }
    }
}
