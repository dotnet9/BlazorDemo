using System.Security.Claims;
using BlazorServer.Models;
using BlazorServer.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace BlazorServer.Repository.Implement;

public class UserRepository : IUserRepository
{
	private readonly UserManager<IdentityUser> _userManager;

	public UserRepository(UserManager<IdentityUser> userManager)
	{
		_userManager = userManager;
	}

	public async Task<List<CustomUserViewModel>> GetUsersAsync()
	{
		var customUsers = _userManager.Users.Select(user => new CustomUserViewModel
			{ UserId = user.Id, UserName = user.UserName, Email = user.Email }).ToList();

		return await Task.Run(() => customUsers);
	}

	public async Task<CustomUserViewModel> GetUserAsync(string userId)
	{
		var user = await _userManager.FindByIdAsync(userId);
		var userClaims = await _userManager.GetClaimsAsync(user);
		var result = new CustomUserViewModel
		{
			UserId = user.Id,
			UserName = user.UserName,
			Email = user.Email,
			Claims = userClaims.Select(x => $"{x.Type} : {x.Value}").ToList()
		};
		return result;
	}

	public async Task<ResultViewModel> EditUserAsync(CustomUserViewModel model)
	{
		var user = await _userManager.FindByIdAsync(model.UserId);

		if (user == null)
		{
			return new ResultViewModel
			{
				Message = $"找不到 Id 为{model.UserId} 的用户",
				IsSuccess = false
			};
		}

		user.UserName = model.UserName;
		user.Email = model.Email;
		var result = await _userManager.UpdateAsync(user);
		if (result.Succeeded)
		{
			return new ResultViewModel
			{
				Message = "用户更新成功！",
				IsSuccess = true
			};
		}

		return new ResultViewModel
		{
			Message = "用户更新失败！",
			IsSuccess = false
		};
	}

	public async Task<ResultViewModel> DeleteUserAsync(string userId)
	{
		var user = await _userManager.FindByIdAsync(userId);

		if (user == null)
		{
			return new ResultViewModel
			{
				Message = $"找不到 Id 为 {userId} 的用户",
				IsSuccess = false
			};
		}

		var result = await _userManager.DeleteAsync(user);
		if (result.Succeeded)
		{
			return new ResultViewModel
			{
				Message = "用户刪除成功！",
				IsSuccess = true
			};
		}

		return new ResultViewModel
		{
			Message = "用户刪除失败！",
			IsSuccess = false
		};
	}

	public async Task<CustomUserClaimsViewModel> EditClaimsInUserAsync(string userId)
	{
		var user = await _userManager.FindByIdAsync(userId);
		var claims = await _userManager.GetClaimsAsync(user);
		var model = new CustomUserClaimsViewModel
		{
			UserId = userId
		};

		foreach (var claim in ClaimsStore.AllClaims)
		{
			var userClaim = new CustomUserClaimViewModel
			{
				ClaimType = claim.Type
			};

			if (claims.Any(c => c.Type == claim.Type && c.Value == "true"))
			{
				userClaim.IsSelected = true;
			}

			model.Claims.Add(userClaim);
		}

		return model;
	}

	public async Task<ResultViewModel> EditClaimsInUserAsync(CustomUserClaimsViewModel model)
	{
		var user = await _userManager.FindByIdAsync(model.UserId);
		var claims = await _userManager.GetClaimsAsync(user);
		var result = await _userManager.RemoveClaimsAsync(user, claims);

		if (!result.Succeeded)
		{
			return new ResultViewModel
			{
				Message = "无法移除用户的 Claim！",
				IsSuccess = false
			};
		}

		result = await _userManager.AddClaimsAsync(user,
			model.Claims.Select(c => new Claim(c.ClaimType!, c.IsSelected ? "true" : "false")));

		if (!result.Succeeded)
		{
			return new ResultViewModel
			{
				Message = "无法將指定的 Claim 分配给用户！",
				IsSuccess = false
			};
		}

		return new ResultViewModel
		{
			Message = "分配 Claim 成功",
			IsSuccess = true
		};
	}
}