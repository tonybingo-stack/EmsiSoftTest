using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmsisoftTest.Models
{
    [Table("hashes")]
    public class HashData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public DateTime date { get; set; }
        public string sha1 { get; set; }

    }
}