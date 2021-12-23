using System.Text.Json;
using BlazorServer.Repository;
using BlazorServer.Shared;
using BlazorServer.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorServer.Pages.RolesManagement;

public partial class RolesManagement
{
	private JsInteropClasses? _jsClass;
	[Inject] protected IRolesRepository? RolesRepository { get; set; }
	[Inject] protected IJSRuntime? Js { get; set; }
	[Inject] private NavigationManager? NavigationManager { get; set; }
	public List<CustomRoleViewModel> Roles { get; set; } = new();

	protected override async Task OnInitializedAsync()
	{
		await LoadData();
		_jsClass = new JsInteropClasses(Js!);
	}

	private async Task LoadData()
	{
		Roles = await RolesRepository!.GetRolesAsync();
	}

	private void EditRole(string id)
	{
		NavigationManager!.NavigateTo($"RolesManagement/EditRole/{id}");
	}

	private async Task DeleteRole(string id)
	{
		var sweetConfirm = new SweetConfirmViewModel
		{
			RequestTitle = $"是否确定刪除角色{id}？",
			RequestText = "这个动作不可恢复",
			ResponseTitle = "刪除成功",
			ResponseText = "角色被刪除了"
		};
		var jsonString = JsonSerializer.Serialize(sweetConfirm);
		var result = await _jsClass!.Confirm(jsonString);
		if (result)
		{
			var deleted = await RolesRepository!.DeleteRoleAsync(id);
			if (deleted.IsSuccess)
				await LoadData();
			else
				await _jsClass.Alert(deleted.Message!);
		}
	}
}