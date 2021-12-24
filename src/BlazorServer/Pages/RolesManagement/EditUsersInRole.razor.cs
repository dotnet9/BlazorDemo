using BlazorServer.Repository;
using BlazorServer.Shared;
using BlazorServer.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorServer.Pages.RolesManagement;

public partial class EditUsersInRole
{
	[Inject] protected IRolesRepository? RolesRepository { get; set; }
	[Inject] protected NavigationManager? NavigationManager { get; set; }
	[Inject] protected IJSRuntime? Js { get; set; }
	private JsInteropClasses? _jsClass;
	[Parameter] public string? Id { get; set; }
	public List<CustomUserRoleViewModel> UserRoleViewModel { get; set; } = new List<CustomUserRoleViewModel>();

	protected override async Task OnInitializedAsync()
	{
		await LoadData();
		_jsClass = new JsInteropClasses(Js!);
	}

	private async Task LoadData()
	{
		UserRoleViewModel = (await RolesRepository!.EditUsersInRoleAsync(Id!)).ToList();
	}


	public async Task HandleValidSubmit()
	{
		var result = await RolesRepository!.EditUsersInRoleAsync(UserRoleViewModel, Id!);

		if (result.IsSuccess)
		{
			NavigationManager!.NavigateTo($"/RolesManagement/EditRole/{Id}");
		}
		else
		{
			await _jsClass!.Alert(result.Message!);
		}
	}

	public void Cancel()
	{
		NavigationManager!.NavigateTo($"/RolesManagement/EditRole/{Id}");
	}
}