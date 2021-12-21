using System.Text.Json;
using BlazorServer.Repository;
using BlazorServer.Shared;
using BlazorServer.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace BlazorServer.Pages;

public class PostBase : ComponentBase, IDisposable
{
	private JsInteropClasses _jsClass;
	protected EditContext? EditContext;
	[Inject] protected IJSRuntime? Js { get; set; }
	[Inject] protected IPostRepository? PostRepository { get; set; }
	[Parameter] public PostViewModel? Post { get; set; }

	[CascadingParameter(Name = "ColorStyle")]
	public string? ColorStyle { get; set; }

	[CascadingParameter(Name = "FontSizeStyle")]
	public string? FontSizeStyle { get; set; }

	[Parameter] public EventCallback<int> GetPostId { get; set; }
	[Parameter] public EventCallback PostCreated { get; set; }

	public void Dispose()
	{
		_jsClass?.Dispose();
	}

	protected override Task OnInitializedAsync()
	{
		_jsClass = new JsInteropClasses(Js!);
		EditContext = new EditContext(Post!);
		EditContext.SetFieldCssClassProvider(new CustomFieldClassProvider());

		return base.OnInitializedAsync();
	}

	protected async Task DeletePost()
	{
		// 改成ViewModel
		var sweetConfirm = new SweetConfirmViewModel
		{
			RequestTitle = $"是否确定删除日志 {Post!.Title}?",
			RequestText = "这个操作不可恢复",
			ResponseTitle = "删除成功",
			ResponseText = "日志被删除了"
		};
		var jsonString = JsonSerializer.Serialize(sweetConfirm);
		var result = await _jsClass.Confirm(jsonString);
		if (result)
		{
			var deleted = await PostRepository!.DeletePost(Post.Id);
			if (deleted.IsSuccess)
				await GetPostId.InvokeAsync(Post!.Id);
			else
				await _jsClass.Alert(deleted.Message!);
		}
	}

	protected async Task CreatePost()
	{
		var result = await PostRepository!.CreatePost(Post!);
		if (result.IsSuccess)
			await PostCreated.InvokeAsync();

		await _jsClass!.Alert(result.Message!);
	}
}