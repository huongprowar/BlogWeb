using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public partial class Blog
{
    public Blog()
    {
        Comments = new HashSet<Comment>();
    }

    public int BlogId { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
    public DateTime CreatedAt { get; set; }
    public int? CategoryId { get; set; }

    public virtual Category? Category { get; set; } = null!;
    public virtual User? User { get; set; } = null;
    public virtual ICollection<Comment> Comments { get; set; }
}

