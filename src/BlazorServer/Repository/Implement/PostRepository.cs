using BlazorServer.Models;
using BlazorServer.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BlazorServer.Repository.Implement;

public class PostRepository : IPostRepository
{
	private readonly AppDbContext _appDbContext;

	public PostRepository(AppDbContext appDbContext)
	{
		_appDbContext = appDbContext;
	}

	public async Task<ResultViewModel> CreatePost(PostViewModel post)
	{
		var data = await _appDbContext.Posts!.FirstOrDefaultAsync(x => x.Id == post.Id);
		if (data != null)
		{
			data.Title = post.Title;
			data.Content = post.Content;
			data.UpdateDateTime = DateTime.Now;
			await _appDbContext.SaveChangesAsync();
			return new ResultViewModel { IsSuccess = false, Message = $"{post.Title}修改成功！" };
		}

		data = new PostModel
		{
			Title = post.Title,
			Content = post.Content,
			CreateDateTime = DateTime.Now,
			UpdateDateTime = DateTime.Now,
			BlogId = post.BlogId
		};
		_appDbContext.Posts!.Add(data);
		await _appDbContext.SaveChangesAsync();
		return new ResultViewModel { IsSuccess = true, Message = $"{post.Title}创建成功！" };
	}

	public async Task<ResultViewModel> DeletePost(int id)
	{
		var data = await _appDbContext.Posts!.FirstOrDefaultAsync(x => x.Id == id);
		if (data == null) return new ResultViewModel { IsSuccess = false, Message = $"id为{id}的Post不存在！" };

		_appDbContext.Posts!.Remove(data);
		await _appDbContext.SaveChangesAsync();
		return new ResultViewModel { IsSuccess = true, Message = $"{data.Title}删除成功！" };
	}
}