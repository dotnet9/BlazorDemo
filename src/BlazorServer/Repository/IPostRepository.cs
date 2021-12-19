using BlazorServer.Models;

namespace BlazorServer.Repository;

public interface IPostRepository
{
	Task<ResultViewModel> CreatePost(PostModel post);

	Task<ResultViewModel> DeletePost(int id);
}