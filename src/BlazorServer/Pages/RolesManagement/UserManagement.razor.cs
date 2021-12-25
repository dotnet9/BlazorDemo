using BlazorServer.Repository;
using BlazorServer.Shared;
using BlazorServer.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BlazorServer.Pages.RolesManagement;

public partial class UserManagement
{
	[Inject] protected IUserRepository? UserRepository { get; set; }
	[Inject] protected NavigationManager? NavigationManager { get; set; }
	[Inject] protected IJSRuntime? Js { get; set; }
	private JsInteropClasses? _jsClass;
	public List<CustomUserViewModel> Users { get; set; } = new();

	protected override async Task OnInitializedAsync()
	{
		await LoadData();
		_jsClass = new JsInteropClasses(Js!);
	}

	private async Task LoadData()
	{
		Users = await UserRepository!.GetUsersAsync();
	}

	private async Task EditUser(string userId)
	{
		NavigationManager!.NavigateTo($"UserManagement/EditUser/{userId}");
		await Task.CompletedTask;
	}

	private async Task DeleteUser(string userId)
	{
		var sweetConfirm = new SweetConfirmViewModel()
		{
			RequestTitle = $"是否确定删除用户{userId}？",
			RequestText = "这个操作不可逆",
			ResponseTitle = "刪除成功",
			ResponseText = "用户被刪除了",
		};
		var jsonString = JsonSerializer.Serialize(sweetConfirm);
		var result = await _jsClass!.Confirm(jsonString);
		if (result)
		{
			var deleted = await UserRepository!.DeleteUserAsync(userId);
			if (deleted.IsSuccess)
			{
				await LoadData();
			}
			else
			{
				await _jsClass!.Alert(deleted.Message!);
			}
		}
	}
}