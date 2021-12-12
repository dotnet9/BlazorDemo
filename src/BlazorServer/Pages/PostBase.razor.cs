using BlazorServer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorServer.Pages;

public class PostBase : ComponentBase
{
	public PostModel? Post { get; set; }

	protected EditContext? EditContext;

	protected override Task OnInitializedAsync()
	{
		Post = new PostModel
		{
			Id = 1,
			Title = "这是标题",
			Content = "这是内容"
		};

		EditContext = new EditContext(Post);
		EditContext.SetFieldCssClassProvider(new CustomFieldClassProvider());

		return base.OnInitializedAsync();
	}
}