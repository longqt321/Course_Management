using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace asp_project.Models
{
    public class CourseEnrollment
    {
        public CourseEnrollment(int id, int userId, int courseId, DateTime enrolledAt)
        {
            Id = id;
            UserId = userId;
            CourseId = courseId;
            EnrolledAt = enrolledAt;
        }

        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; } // Học viên

        [ForeignKey("Course")]
        public int CourseId { get; set; }

        public DateTime EnrolledAt { get; set; } = DateTime.Now;

        public virtual User User { get; set; }
        public virtual Course Course { get; set; }
    }
}
