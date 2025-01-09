using System.ComponentModel.DataAnnotations;

namespace ToDo.Web.Models
{
    public partial class Employee
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Age { get; set; }
        public string Speciality { get; set; } = null!;        
        public DateTime EmployeeDate { get; set; }

        List<ToDoItem> tasks { get; set; }  
    }
}
