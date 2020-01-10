using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Add namespaces
using ChinookSystem.Data.Entities;
using ChinookSystem.DAL;
#endregion

namespace ChinookSystem.BLL
{
    public class AlbumController
    {
        //basic query: complete list of DbSet
        public List<Album> Album_List()
        {
            //setup the code block to ensure the release of the sql connection
            using (var context = new ChinookContext())
            {
                // .ToList<t> is used to convert the DbSet<t> into a List<t> collection
                return context.Albums.ToList();
            }
        }

        //basic query: return a recorded based on pkey
        public Album Album_FindByID(int albumid)
        {
            using (var context = new ChinookContext())
            {
                return context.Albums.Find(albumid);
            }
        }
    }
}
