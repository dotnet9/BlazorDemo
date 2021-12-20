using BlazorServer.Models;
using BlazorServer.ViewModels;

namespace BlazorServer.Repository;

public interface IPostRepository
{
	Task<ResultViewModel> CreatePost(PostViewModel post);

	Task<ResultViewModel> DeletePost(int id);
}