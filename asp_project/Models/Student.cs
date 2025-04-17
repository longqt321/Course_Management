using System.ComponentModel.DataAnnotations;

namespace asp_project.Models
{
    public class Student
    {
        public Student()
        {
        }

        public Student(int id, string username, string password, string firstName, string lastName, string email, string phoneNumber)
        {
            Id = id;
            Username = username;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public  String Username { get; set; }
        [Required]
        public  String Password { get; set; }
        [Required]
        public  String FirstName { get; set; }
        [Required]
        public  String LastName { get; set; }
        [EmailAddress]
        public String Email { get; set; }
        [Phone]
        public String PhoneNumber { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Student student &&
                   Id == student.Id &&
                   Username == student.Username &&
                   Password == student.Password &&
                   FirstName == student.FirstName &&
                   LastName == student.LastName &&
                   Email == student.Email &&
                   PhoneNumber == student.PhoneNumber;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Username, Password, FirstName, LastName, Email, PhoneNumber);
        }
    }
}
