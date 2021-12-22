using BlazorServer.Models;
using BlazorServer.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BlazorServer.Repository.Implement;

public class BlogRepository : IBlogRepository
{
	private readonly AppDbContext _appDbContext;

	public BlogRepository(AppDbContext appDbContext)
	{
		_appDbContext = appDbContext;
	}

	public async Task<ResultViewModel> CreateBlog(BlogViewModel blog)
	{
		var data = await _appDbContext.Blogs!.FirstOrDefaultAsync(x => x.Id == blog.Id);
		if (data != null) return new ResultViewModel { IsSuccess = false, Message = $"{blog.Name}已存在！" };

		data = new BlogModel();
		data.Name = blog.Name;
		data.CreateDateTime = DateTime.Now;

		_appDbContext.Blogs!.Add(data);
		await _appDbContext.SaveChangesAsync();
		return new ResultViewModel { IsSuccess = true, Message = $"{blog.Name}创建成功！" };
	}

	public async Task<BlogViewModel> GetBlog()
	{
		var blog = await _appDbContext.Blogs!.Include(b => b.Posts).FirstOrDefaultAsync();
		if (blog == null) return new BlogViewModel();

		var blogViewModel = new BlogViewModel
		{
			Id = blog.Id,
			Name = blog.Name,
			CreateDateTime = blog.CreateDateTime,
			Posts = blog.Posts!.Select(p => new PostViewModel
			{
				Id = p.Id,
				BlogId = p.BlogId,
				Blog = new BlogViewModel
				{
					Id = p.BlogId,
					Name = p.Blog?.Name,
					CreateDateTime = p.CreateDateTime
				},
				Title = p.Title,
				Content = p.Content,
				CreateDateTime = p.CreateDateTime,
				UpdateDateTime = p.UpdateDateTime
			}).ToList()
		};
		return blogViewModel;
	}
}