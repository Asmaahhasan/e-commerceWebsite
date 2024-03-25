using FixIt.Models;
using FixIt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FixIt.Models;

public partial class Users
{
    [Display(Name = "UId")]
    public int UId { get; set; }

    [Required(ErrorMessage = "Please Enter Your First Name")]
    [Display(Name = "First Name")]
    [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Use Characters only")]

    public string FName { get; set; } = null!;

    [Required(ErrorMessage = "Please Enter Your Last Name")]
    [Display(Name = "Last Name")]
    [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Use Characters only")]

    public string LName { get; set; } = null!;
    [Required(ErrorMessage = "Please Enter Your Email ")]
    [RegularExpression(".+@.+\\..+", ErrorMessage = "please enter correct email address")]
    [Display(Name = "EMail")]
    public string? EMail { get; set; }

    [Required(ErrorMessage = "Please Enter Your Password ")]
    [Display(Name = "Pass")]
    [StringLength(50, ErrorMessage = "The Password must be at least 8 characters long.", MinimumLength = 8)]

    public string Pass { get; set; } = null!;

    [Required(ErrorMessage = "Please Enter Your Address ")]
    [Display(Name = "Address")]
    public string Address { get; set; } = null!;
    [Required(ErrorMessage = "Please Enter Your Phone ")]
    [RegularExpression(@"^\(?([0-9]{11})$",
   ErrorMessage = "Entered phone format is not valid.")]
    [Display(Name = " Phone")]
    public string Phone { get; set; } = null!;

    [NotMapped]
    public  IFormFile? ClientFile { get; set; }
    public byte[]? Photo { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
