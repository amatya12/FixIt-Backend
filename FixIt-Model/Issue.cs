using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FixIt_Model
{
    public class Issue
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName ="varchar(500)")]
        public string Title { get; set; }

        [Column(TypeName = "varchar(5000)")]
        public string Description { get; set; }

        public double latitude { get; set; }

        public double longitude { get; set; }

        public string Severity { get; set; }

        public bool Status { get; set; }

        public virtual Category Category { get; set; }
    }
}
