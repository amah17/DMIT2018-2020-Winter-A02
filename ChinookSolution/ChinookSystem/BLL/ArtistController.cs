using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Add Namespaces
using System.Collections.Generic;
using ChinookSystem.Data.Entities;
using ChinookSystem.DAL;
#endregion

namespace ChinookSystem.BLL
{
    //ask yourself, what is going to be using this. This case, its another project. The WEBAPP
    public class ArtistController
    {
        //basic query: complete list of DbSet
        public List<Artist> Artist_List()
        {
            //setup the code block to ensure the release of the sql connection
            using (var context = new ChinookContext())
            {
                // .ToList<t> is used to convert the DbSet<t> into a List<t> collection
                return context.Artists.ToList();
            }
        }

        //basic query: return a recorded based on pkey
        public Artist Artist_FindByID(int artistid)
        {
            using (var context = new ChinookContext())
            {
                return context.Artists.Find(artistid);
            }
        }
    }
}
