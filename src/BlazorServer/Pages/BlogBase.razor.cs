using BlazorServer.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorServer.Pages;

public class BlogBase : ComponentBase
{
	public BlogModel? Blog { get; set; }

	public string? ColorStyle { get; set; } = "color: goldenrod";
	public string? FontSizeStyle { get; set; } = "color: goldenrod";

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
			Posts = new List<PostModel>
			{
				new()
				{
					Id = 1, Title = "标题1", Content = "内容1", CreateDateTime = new DateTime(2021, 12, 11, 10, 20, 50)
				},
				new()
				{
					Id = 1, Title = "标题2", Content = "内容2", CreateDateTime = new DateTime(2021, 12, 12, 9, 13, 15)
				},
				new()
				{
					Id = 1, Title = "标题3", Content = "内容3", CreateDateTime = new DateTime(2021, 12, 13, 20, 31, 26)
				},
				new()
				{
					Id = 1, Title = "标题4", Content = "内容4", CreateDateTime = new DateTime(2021, 12, 14, 22, 15, 27)
				}
			},
			CreateDateTime = new DateTime(2021, 12, 14, 23, 46, 59)
		};
	}
}