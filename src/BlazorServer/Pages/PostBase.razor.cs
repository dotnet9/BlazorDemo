using BlazorServer.Models;
using BlazorServer.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace BlazorServer.Pages;

public class PostBase : ComponentBase, IDisposable
{
	private JsInteropClasses _jsClass;

	protected EditContext? EditContext;
	[Inject] protected IJSRuntime? Js { get; set; }

	[Parameter] public PostModel? Post { get; set; }

	[CascadingParameter(Name = "ColorStyle")]
	public string? ColorStyle { get; set; }

	[CascadingParameter(Name = "FontSizeStyle")]
	public string? FontSizeStyle { get; set; }

	[Parameter] public EventCallback<int> GetPostId { get; set; }

	public void Dispose()
	{
		_jsClass?.Dispose();
	}

	protected override Task OnInitializedAsync()
	{
		_jsClass = new JsInteropClasses(Js);
		EditContext = new EditContext(Post);
		EditContext.SetFieldCssClassProvider(new CustomFieldClassProvider());

		return base.OnInitializedAsync();
	}

	protected async Task DeletePost()
	{
		//var confirm = await Js!.InvokeAsync<bool>("confirm", $"是否删除{Post?.Title}?");
		var confirm = await _jsClass.Confirm(Post!.Title!);
		if (confirm) await GetPostId.InvokeAsync(Post!.Id);
	}
}