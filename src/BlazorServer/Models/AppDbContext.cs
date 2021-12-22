using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorServer.Models;

public class AppDbContext : IdentityDbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{
	}

	public DbSet<BlogModel>? Blogs { get; set; }

	public DbSet<PostModel>? Posts { get; set; }
}