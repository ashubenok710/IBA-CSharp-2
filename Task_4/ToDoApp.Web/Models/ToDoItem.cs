using System.ComponentModel.DataAnnotations;

namespace ToDo.Web.Models
{
    public partial class ToDoItem
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Priority { get; set; }        
        public string? DueDate { get; set; }
        public byte? IsCompleted { get; set; }
        public int EmployeeID { get; set; }
    }
}
