using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PRN221_BlogWeb.Models;

public partial class User
{
    public int UserId { get; set; }
    [Display(Name = "Fullname")]
    public string Fullname { get; set; } = ""!;
    [Display(Name = "Gender")]
    [Required]
    public bool Gender { get; set; }
    [Required]    
    [Display(Name = "Username")]
    public string Username { get; set; } = null!;
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
