using System;
using System.Collections.Generic;


    public partial class Category
    {
        public Category()
        {
            Blogs = new HashSet<Blog>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;

        public virtual ICollection<Blog> Blogs { get; set; }
    }

