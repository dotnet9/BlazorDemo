using Microsoft.EntityFrameworkCore;

namespace BlazorServer.Models;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{
	}

	public DbSet<BlogModel>? Blogs { get; set; }

	public DbSet<PostModel>? Posts { get; set; }
}