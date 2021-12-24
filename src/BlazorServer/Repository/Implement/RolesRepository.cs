using BlazorServer.Models;
using BlazorServer.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace BlazorServer.Repository.Implement;

public class RolesRepository : IRolesRepository
{
	private readonly RoleManager<IdentityRole> _roleManager;
	private readonly UserManager<IdentityUser> _userManager;

	public RolesRepository(
		RoleManager<IdentityRole> roleManager,
		UserManager<IdentityUser> userManager)
	{
		_roleManager = roleManager;
		_userManager = userManager;
	}

	#region Roles

	public async Task<CustomRoleViewModel> GetRoleAsync(string roleId)
	{
		var role = await _roleManager.FindByIdAsync(roleId);
		var users = await _userManager.GetUsersInRoleAsync(role.Name);
		var result = new CustomRoleViewModel
		{
			Id = role.Id,
			Name = role.Name,
			Users = users.Select(u => u.UserName).ToList()
		};
		return result;
	}

	public async Task<List<CustomRoleViewModel>> GetRolesAsync()
	{
		var roles = _roleManager.Roles;
		var customRoles = new List<CustomRoleViewModel>();
		foreach (var role in roles) customRoles.Add(new CustomRoleViewModel { Id = role.Id, Name = role.Name });

		return await Task.Run(() => customRoles);
	}

	public async Task<ResultViewModel> CreateRoleAsync(CustomRoleViewModel model)
	{
		var identityRole = new IdentityRole
		{
			Name = model.Name
		};
		var result = await _roleManager.CreateAsync(identityRole);
		if (result.Succeeded)
			return new ResultViewModel
			{
				Message = "角色创建成功！",
				IsSuccess = true
			};

		return new ResultViewModel
		{
			Message = "角色创建失敗！",
			IsSuccess = false
		};
	}

	public async Task<ResultViewModel> EditRoleAsync(CustomRoleViewModel model)
	{
		var role = await _roleManager.FindByIdAsync(model.Id);

		if (role == null)
			return new ResultViewModel
			{
				Message = $"找不到 Id 为 {model.Id} 的角色",
				IsSuccess = false
			};

		role.Name = model.Name;
		var result = await _roleManager.UpdateAsync(role);
		if (result.Succeeded)
			return new ResultViewModel
			{
				Message = "角色更新成功！",
				IsSuccess = true
			};

		return new ResultViewModel
		{
			Message = "角色更新失败！",
			IsSuccess = false
		};
	}

	public async Task<ResultViewModel> DeleteRoleAsync(string roleId)
	{
		var role = await _roleManager.FindByIdAsync(roleId);

		if (role == null)
			return new ResultViewModel
			{
				Message = $"找不到 Id 为 {roleId} 的角色",
				IsSuccess = false
			};

		var result = await _roleManager.DeleteAsync(role);
		if (result.Succeeded)
			return new ResultViewModel
			{
				Message = "角色刪除成功！",
				IsSuccess = true
			};

		return new ResultViewModel
		{
			Message = "角色刪除失败！",
			IsSuccess = false
		};
	}

	public async Task<List<CustomUserRoleViewModel>> EditUsersInRoleAsync(string roleId)
	{
		var role = await _roleManager.FindByIdAsync(roleId);
		var model = new List<CustomUserRoleViewModel>();

		// 这里注意，_userManager.Users.ToList()后面一定要加.ToList()，否则会抛出异常：https://stackoverflow.com/questions/60727080/helping-solving-there-is-already-an-open-datareader-associated-with-this-comman
		foreach (var user in _userManager.Users.ToList())
		{
			var userRoleViewModel = new CustomUserRoleViewModel
			{
				UserId = user.Id,
				UserName = user.UserName
			};

			if (await _userManager.IsInRoleAsync(user, role.Name))
				userRoleViewModel.IsSelected = true;
			else
				userRoleViewModel.IsSelected = false;
			model.Add(userRoleViewModel);
		}

		return model;
	}

	public async Task<ResultViewModel> EditUsersInRoleAsync(List<CustomUserRoleViewModel> model, string roleId)
	{
		var role = await _roleManager.FindByIdAsync(roleId);
		foreach (var m in model)
		{
			var user = await _userManager.FindByIdAsync(m.UserId);
			IdentityResult result;
			if (m.IsSelected && !await _userManager.IsInRoleAsync(user, role.Name))
				result = await _userManager.AddToRoleAsync(user, role.Name);
			else if (!m.IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
				result = await _userManager.RemoveFromRoleAsync(user, role.Name);
			else
				continue;
			if (result.Succeeded)
			{
				if (model.Count > 0)
					continue;
				return new ResultViewModel
				{
					Message = roleId,
					IsSuccess = true
				};
			}

			return new ResultViewModel
			{
				Message = roleId,
				IsSuccess = false
			};
		}

		return new ResultViewModel
		{
			Message = roleId,
			IsSuccess = true
		};
	}

	#endregion
}