using System.ComponentModel.DataAnnotations;

namespace asp_project.Models
{
    public class User
    {
        public User()
        {
        }

        public User(int id, string username, string password, string firstName, string lastName, string email, string phoneNumber, DateTime birthDay, Role role)
        {
            Id = id;
            Username = username;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            BirthDay = birthDay;
            Role = role;
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        public  String Username { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [MaxLength(64)] 
        public  String Password { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên của bạn")]
        public  String FirstName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập họ của bạn")]
        public  String LastName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập email của bạn")]
        [EmailAddress(ErrorMessage ="Email không hợp lệ")]
        public String Email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression(@"^(0|\+84)(3[2-9]|5[6|8|9]|7[0|6-9]|8[1-5]|9[0-9])[0-9]{7}$",
        ErrorMessage = "Số điện thoại không hợp lệ")]
        public String PhoneNumber { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime BirthDay { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Vui lòng chọn quyền hạn")]
        public Role Role { get; set; }
    }
    public enum Role
    {
        Admin,
        Student,
        Teacher
    }
}
