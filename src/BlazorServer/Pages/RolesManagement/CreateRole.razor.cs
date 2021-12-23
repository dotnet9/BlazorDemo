using BlazorServer.Repository;
using BlazorServer.ViewModels;
using Microsoft.AspNetCore.Components;

namespace BlazorServer.Pages.RolesManagement;

public partial class CreateRole
{
	[Inject] protected IRolesRepository? RolesRepository { get; set; }
	[Inject] protected NavigationManager? NavigationManager { get; set; }
	public CustomRoleViewModel Role { get; set; } = new();

	private async Task CreateRoleInfo()
	{
		await RolesRepository!.CreateRoleAsync(Role);
		NavigationManager!.NavigateTo("/RolesManagement/RolesList");
	}
}