using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("owner")] 
    public class Owner 
    {
        //private string _name;

        [Column("OwnerId")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(60, ErrorMessage = "Name can't be longer than 60 characters")]
        public string? Name { get; set; }
        //public string? Name
        //{
        //    get { return _name; }
        //    set { _name = value; }
        //}

        [Required(ErrorMessage = "Date of birth is required")] 
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Address is required")] 
        [StringLength(100, ErrorMessage = "Address cannot be longer then 100 characters")] 
        public string? Address { get; set; }

        //public string Name1 = "Kurt";

        public virtual ICollection<Account>? Accounts { get; set; }
    }
}
