using BlazorServer.Models;
using BlazorServer.ViewModels;

namespace BlazorServer.Repository;

public interface IBlogRepository
{
	Task<ResultViewModel> CreateBlog(BlogViewModel blog);

	Task<BlogViewModel> GetBlog();
}