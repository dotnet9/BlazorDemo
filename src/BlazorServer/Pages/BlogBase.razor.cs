using BlazorServer.Repository;
using BlazorServer.Shared;
using BlazorServer.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorServer.Pages;

public class BlogBase : ComponentBase
{
	private JsInteropClasses? _jsClass;
	[Inject] protected IJSRuntime? Js { get; set; }
	[Inject] protected IBlogRepository? BlogRepository { get; set; }

	public BlogViewModel? Blog { get; set; }
	public string? ColorStyle { get; set; } = "color: goldenrod";

	protected void Add()
	{
		Blog!.Posts!.Add(new PostViewModel
		{
			BlogId = Blog.Id,
			CreateDateTime = DateTime.Now,
			UpdateDateTime = DateTime.Now
		});
	}

	protected override async Task OnInitializedAsync()
	{
		_jsClass = new JsInteropClasses(Js!);
		await LoadData();
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await _jsClass!.ConsoleLog("这是Blazor Server的console.log信息");
	}

	private async Task LoadData()
	{
		Blog = await BlogRepository!.GetBlog();
	}

	protected void GetPostId(int id)
	{
		var post = Blog?.Posts?.FirstOrDefault(p => p.Id == id);
		if (post != null)
			Blog!.Posts!.Remove(post);
	}

	protected async Task CreateBlog()
	{
		var result = await BlogRepository!.CreateBlog(Blog!);
		if (result.IsSuccess)
			await LoadData();
		else
			await _jsClass!.Alert(result.Message!);
	}

	protected async Task PostCreated()
	{
		await LoadData();
	}
}