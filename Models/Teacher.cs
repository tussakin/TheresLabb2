using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Labb2Theres.Models
{
    public class Teacher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
/*        [ForeignKey ("Course")]
        public int FkCourseId { get; set; }*/
        public ICollection<Course>? Courses { get; set; }

    }
}


