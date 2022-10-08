using System.ComponentModel.DataAnnotations;

namespace Auth.Web.ViewModel
{
	public class CreateUserViewModel
	{
		[Required]
		[EmailAddress]
		[Display(Name ="User Email")]
		public string  Email { get; set; }

        [Required]
		[Phone]
        [Display(Name = "User Phone Number")]

        public string Phone { get; set; }

        [Required] 
		[DataType(DataType.Password)]
        [Display(Name = "User PassWord")]

        public string  PassWord { get; set; }
	
	}
}
