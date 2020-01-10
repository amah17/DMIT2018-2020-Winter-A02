using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
#endregion

namespace ChinookSystem.Data.Entities
{
    [Table("Artist")]
    public class Artist
    {
        private string _Name;
        // If using the same names as table, then the properties can be in any order.
        [Key]
        public int ArtistId { get; set; }

        // Fully implemented property. MUST DO FOR STRINGS. CLASS STANDARD
        // Check if the SQL entity attribute has any constraints.
        [StringLength(120, ErrorMessage ="Artist Name is limited to 120 characters.")]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                //if (string.IsNullOrEmpty(value))
                //{
                //    _Name = null;
                //}
                //else
                //{
                //    _Name = value;
                //}
                _Name = string.IsNullOrEmpty(value) ? null : value;
            }
        }

        //[NotMapped] Properties

        //navigational Properties
        public virtual ICollection<Album> Albums { get; set; }
    }
}
