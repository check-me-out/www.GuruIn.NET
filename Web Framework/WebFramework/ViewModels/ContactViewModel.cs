namespace WebFramework.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class ContactViewModel
    {
        [Required]
        public string Name { get; set; }

        [EmailAddress]
        [RegularExpression(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
+ "@"
+ @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$", ErrorMessage = "Please enter a valid email address!")]
        public string Email { get; set; }

        [Required,
        StringLength(3000, ErrorMessage = "Message too long, consider breaking it up.")]
        public string Message { get; set; }

        public bool CopyUser { get; set; }
    }
}