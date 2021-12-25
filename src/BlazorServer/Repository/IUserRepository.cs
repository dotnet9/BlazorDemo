﻿using BlazorServer.Models;
using BlazorServer.ViewModels;

namespace BlazorServer.Repository;

public interface IUserRepository
{
	Task<ResultViewModel> DeleteUserAsync(string userId);
	Task<ResultViewModel> EditUserAsync(CustomUserViewModel model);
	Task<CustomUserViewModel> GetUserAsync(string userId);
	Task<List<CustomUserViewModel>> GetUsersAsync();
	Task<CustomUserClaimsViewModel> EditClaimsInUserAsync(string userId);
	Task<ResultViewModel> EditClaimsInUserAsync(CustomUserClaimsViewModel model);
}