using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WpfApp1.DataModel
{
    public partial class Person
    {
        private DateTime date;

        [Key]
        public int PersonId { get; set; }

        [Column(TypeName = "Date")]
        public DateTime Date { get => date; set => date = value; }
        public string FirstName { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Country { get; set; } = null!;


        public int getAge()
        {
            return DateTime.Now.Year - Date.Year;
        }
               
    }

}