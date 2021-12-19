using BlazorServer.Models;

namespace BlazorServer.Repository;

public interface IBlogRepository
{
	Task<ResultViewModel> CreateBlog(BlogModel blog);

	Task<BlogModel> GetBlog();
}