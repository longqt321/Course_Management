using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace asp_project.Models
{
    public class Course
    {
        public Course()
        {
            Enrollments = new HashSet<CourseEnrollment>();
        }

        public Course(int id, string name, DateTime startDate, int fee, int maxStudents, int teacherId, string? image)
        {
            Id = id;
            Name = name;
            StartDate = startDate;
            Fee = fee;
            MaxStudents = maxStudents;
            TeacherId = teacherId;
            Image = image;
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên khóa học")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập học phí")]
        public int Fee { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng học viên tối đa")]
        public int MaxStudents { get; set; }

        [ForeignKey("Teacher")]
        [Required(ErrorMessage = "Vui lòng chọn giảng viên")]
        public int TeacherId { get; set; }

        [ValidateNever]
        public virtual User Teacher { get; set; }

        [Url(ErrorMessage = "Vui lòng nhập đường dẫn hình ảnh hợp lệ")]
        public string? Image { get; set; }

        public virtual ICollection<CourseEnrollment> Enrollments { get; set; }
    }
}
