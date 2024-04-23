using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Labb2Theres.Models
{
    public class StudentYear
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentYearId { get; set; }
        public string StudentYearName { get; set; }
        public ICollection<Student>? Students { get; set; }

    }


}
