using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using paf.api.Models;

namespace paf.api.Infrastructure;

public class AppDbContext:IdentityDbContext<IdentityUser,IdentityRole,string>
{
    public DbSet<User> Users { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Comment> Comments { get; set; }


    public AppDbContext(DbContextOptions options):base(options)
    {

    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Filename=problemandfix.db");
        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>().HasKey(x => x.Id);
        builder.Entity<Blog>().HasKey(x => x.Id);
        builder.Entity<Comment>().HasKey(x => x.Id);

        builder.Entity<User>().HasMany(e => e.UserBlogs).WithOne(e => e.User);
        builder.Entity<Comment>().HasOne(e => e.User);
        builder.Entity<Blog>().HasMany(e => e.Comments);
        base.OnModelCreating(builder);
    }

}
