using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PRN221_BlogWeb.Models;

public partial class Blog
{
    public int? BlogId { get; set; }

    public int? UserId { get; set; }
    [Required]
    public string Title { get; set; } = ""!;
    [Required]
    public string Content { get; set; } = ""!;

    [DataType(DataType.Date)]
    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Comment>? Comments { get; set; } = new List<Comment>();

    public virtual User? User { get; set; } = null!;
}
