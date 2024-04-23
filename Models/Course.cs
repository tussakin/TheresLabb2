using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Labb2Theres.Models
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string CourseDescription { get; set; }
        public ICollection<Teacher> Teachers { get; set; } 
        public ICollection<Student> Students { get; set; }
    }
}
