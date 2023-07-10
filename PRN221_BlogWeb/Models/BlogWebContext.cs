using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PRN221_BlogWeb.Models;

public partial class BlogWebContext : DbContext
{
    private static object syncRoot = new object();    
    public BlogWebContext()
    {
       
    }

    public BlogWebContext(DbContextOptions<BlogWebContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=DESKTOP-LGFID7C;database=BlogWeb;User ID=sa;Password=12345678;TrustServerCertificate=True;Trusted_Connection=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Admin");

            entity.Property(e => e.AdminId).HasColumnName("AdminID");
            entity.Property(e => e.Fullname).HasMaxLength(50);
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.ToTable("Blog");

            entity.Property(e => e.BlogId).HasColumnName("BlogID");
            entity.Property(e => e.Content).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnType("date");
            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Blog_User");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => new { e.CommentId, e.BlogId, e.UserId });

            entity.ToTable("Comment");

            entity.Property(e => e.CommentId)
                .ValueGeneratedOnAdd()
                .HasColumnName("CommentID");
            entity.Property(e => e.BlogId).HasColumnName("BlogID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Content).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnType("date");

            entity.HasOne(d => d.Blog).WithMany(p => p.Comments)
                .HasForeignKey(d => d.BlogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comment_Blog");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comment_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Fullname).HasMaxLength(50);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
