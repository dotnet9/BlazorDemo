using BlazorServer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorServer.Pages;

public class PostBase : ComponentBase
{
	[Parameter]
	public PostModel? Post { get; set; }

	protected EditContext? EditContext;

	protected override Task OnInitializedAsync()
	{
		EditContext = new EditContext(Post);
		EditContext.SetFieldCssClassProvider(new CustomFieldClassProvider());

		return base.OnInitializedAsync();
	}
}