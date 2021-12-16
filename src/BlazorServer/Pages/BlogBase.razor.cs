using BlazorServer.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorServer.Pages;

public class BlogBase : ComponentBase
{
	private int _postId;
	public BlogModel? Blog { get; set; }

	public string? ColorStyle { get; set; } = "color: goldenrod";

	protected void Add()
	{
		_postId++;
		Blog?.Posts?.Add(new PostModel { Id = _postId });
	}

	protected override Task OnInitializedAsync()
	{
		LoadData();
		return base.OnInitializedAsync();
	}

	private void LoadData()
	{
		Blog = new BlogModel
		{
			Id = 1,
			Name = "我的博客",
			Posts = new List<PostModel>(),
			CreateDateTime = new DateTime(2021, 12, 14, 23, 46, 59)
		};
	}

	protected void GetPostId(int id)
	{
		var post = Blog?.Posts?.FirstOrDefault(p => p.Id == id);
		if (post != null)
			Blog!.Posts!.Remove(post);
		//StateHasChanged();
	}
}