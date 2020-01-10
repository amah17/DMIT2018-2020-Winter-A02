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

    [Table("Albums")]
    public class Album
    {
        private string _ReleaseLabel;

        [Key]
        public int AlbumId { get; set; }

        [Required(ErrorMessage = "Album Title required")]
        [StringLength(160, ErrorMessage ="Album Title limited to 160 characters")]
        public string AlbumTitle { get; set; }

        [Range(1, int.MaxValue, ErrorMessage ="Artist ID out of range")]
        public int ArtistId { get; set; }

        public int ReleaseYear { get; set; }

        [StringLength(50, ErrorMessage ="Release Label is limited to 50 characters")]
        public string ReleaseLabel
        {
            get
            {
                return _ReleaseLabel;
            }
            set
            {
                _ReleaseLabel = string.IsNullOrEmpty(value) ? null : value;
            }
        }
    }
}
