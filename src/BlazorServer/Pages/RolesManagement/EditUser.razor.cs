using BlazorServer.Repository;
using BlazorServer.ViewModels;
using Microsoft.AspNetCore.Components;

namespace BlazorServer.Pages.RolesManagement;

public partial class EditUser
{
	[Inject] protected IUserRepository? UserRepository { get; set; }
	[Inject] protected NavigationManager? NavigationManager { get; set; }
	public CustomUserViewModel User { get; set; } = new();
	[Parameter] public string? UserId { get; set; }

	protected override async Task OnInitializedAsync()
	{
		var result = await UserRepository!.GetUserAsync(UserId!);
		User = new CustomUserViewModel
		{
			UserId = result.UserId,
			UserName = result.UserName,
			Claims = result.Claims
		};
	}

	private async Task EditRole()
	{
		await UserRepository!.EditUserAsync(User);
		NavigationManager!.NavigateTo("/UserManagement/UserList");
	}

	public void EditUsersInRole()
	{
		NavigationManager!.NavigateTo($"/UserManagement/EditClaimsInUser/{UserId}");
	}

	public void Cancel()
	{
		NavigationManager!.NavigateTo($"/UserManagement/UserList");
	}
}