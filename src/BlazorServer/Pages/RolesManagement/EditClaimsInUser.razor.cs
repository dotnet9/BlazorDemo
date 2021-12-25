using BlazorServer.Repository;
using BlazorServer.Shared;
using BlazorServer.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorServer.Pages.RolesManagement;

public partial class EditClaimsInUser
{
	[Inject] protected IUserRepository? UserRepository { get; set; }
	[Inject] protected NavigationManager? NavigationManager { get; set; }
	[Inject] protected IJSRuntime? Js { get; set; }
	private JsInteropClasses? _jsClass;
	[Parameter] public string? UserId { get; set; }
	public CustomUserClaimsViewModel UserClaimViewModel { get; set; } = new CustomUserClaimsViewModel();

	protected override async Task OnInitializedAsync()
	{
		await LoadData();
		_jsClass = new JsInteropClasses(Js!);
	}

	private async Task LoadData()
	{
		UserClaimViewModel = (await UserRepository!.EditClaimsInUserAsync(UserId!));
	}


	public async Task HandleValidSubmit()
	{
		var result = await UserRepository!.EditClaimsInUserAsync(UserClaimViewModel);

		if (result.IsSuccess)
		{
			NavigationManager!.NavigateTo($"/UserManagement/EditUser/{UserId}");
		}
		else
		{
			await _jsClass!.Alert(result.Message!);
		}
	}

	public void Cancel()
	{
		NavigationManager!.NavigateTo($"/UserManagement/EditUser/{UserId}");
	}
}