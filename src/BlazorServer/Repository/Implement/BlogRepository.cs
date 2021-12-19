using BlazorServer.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorServer.Repository.Implement;

public class BlogRepository : IBlogRepository
{
	private readonly AppDbContext _appDbContext;

	public BlogRepository(AppDbContext appDbContext)
	{
		_appDbContext = appDbContext;
	}

	public async Task<ResultViewModel> CreateBlog(BlogModel blog)
	{
		var data = await _appDbContext.Blogs!.FirstOrDefaultAsync(x => x.Id == blog.Id);
		if (data != null) return new ResultViewModel { IsSuccess = false, Message = $"{blog.Name}已存在！" };

		blog.CreateDateTime = DateTime.Now;
		_appDbContext.Blogs!.Add(blog);
		await _appDbContext.SaveChangesAsync();
		return new ResultViewModel { IsSuccess = true, Message = $"{blog.Name}创建成功！" };
	}

	public async Task<BlogModel> GetBlog()
	{
		var blog = await _appDbContext.Blogs!.Include(b => b.Posts).FirstOrDefaultAsync() ?? new BlogModel();

		return blog;
	}
}