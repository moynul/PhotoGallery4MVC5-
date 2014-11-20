using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace photoGallery.Models
{
    public class Album
    {
        [Key]
        public int ID { get; set; }

        [DisplayName("Albam Name ")]
        [Required(ErrorMessage = "An Album Title is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Albam Name must be between 3 and 50 characters!")]
        public string AlbumName { get; set; }

        [ScaffoldColumn(false)]
        public string ThumbnailImage { get; set; }
        [NotMapped]
        public HttpPostedFileBase File { get; set; }

        [ScaffoldColumn(false)]
        [DataType(DataType.Date)]
        [DisplayName("Inserted Date ")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? InsertedDateTime { get; set; }

        [ScaffoldColumn(false)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? LastUpdatedDatetime { get; set; }

        [ScaffoldColumn(false)]
        public string InsertedBy { get; set; }
        [ScaffoldColumn(false)]
        public string LastUpdatedBy { get; set; }

        public virtual List<Photo> Photos { get; set; }
    }
}