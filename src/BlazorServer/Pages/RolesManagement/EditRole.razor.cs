using BlazorServer.Repository;
using BlazorServer.ViewModels;
using Microsoft.AspNetCore.Components;

namespace BlazorServer.Pages.RolesManagement;

public partial class EditRole
{
	[Inject] protected IRolesRepository? RolesRepository { get; set; }
	[Inject] protected NavigationManager? NavigationManager { get; set; }
	public CustomRoleViewModel Role { get; set; } = new();
	[Parameter] public string? Id { get; set; }

	protected override async Task OnInitializedAsync()
	{
		var result = await RolesRepository!.GetRoleAsync(Id!);
		Role = new CustomRoleViewModel
		{
			Id = result.Id,
			Name = result.Name,
			Users = result.Users
		};
	}

	private async Task EditRoleInfo()
	{
		await RolesRepository!.EditRoleAsync(Role);
		NavigationManager!.NavigateTo("/RolesManagement/RolesList");
	}

	public void Cancel()
	{
		NavigationManager!.NavigateTo("/RolesManagement/RolesList");
	}

	public void EditUsersInRole()
	{
		NavigationManager!.NavigateTo($"/RolesManagement/EditUsersInRole/{Id}");
	}
}