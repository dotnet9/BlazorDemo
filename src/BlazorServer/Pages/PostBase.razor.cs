using BlazorServer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorServer.Pages;

public class PostBase : ComponentBase
{
	protected EditContext? EditContext;

	[Parameter] public PostModel? Post { get; set; }

	[CascadingParameter(Name = "ColorStyle")]
	public string? ColorStyle { get; set; }

	[CascadingParameter(Name = "FontSizeStyle")]
	public string? FontSizeStyle { get; set; }

	protected override Task OnInitializedAsync()
	{
		EditContext = new EditContext(Post);
		EditContext.SetFieldCssClassProvider(new CustomFieldClassProvider());

		return base.OnInitializedAsync();
	}
}