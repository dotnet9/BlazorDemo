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
		var confirm = await _jsClass.Confirm(Post!.Title!);
		if (confirm)
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