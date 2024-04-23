using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace Labb2Theres.Models
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        [ForeignKey ("StudentYear")]
        public int FkStudentYearId { get; set; }
/*        [ForeignKey("Course")]
        public int FkCourseId { get; set; }*/
        public StudentYear? StudentYear { get; set; } 
        public ICollection<Course>? Courses { get; set; }
    }
}
