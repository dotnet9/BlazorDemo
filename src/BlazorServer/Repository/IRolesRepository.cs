using BlazorServer.Models;
using BlazorServer.ViewModels;

namespace BlazorServer.Repository;

public interface IRolesRepository
{
	Task<CustomRoleViewModel> GetRoleAsync(string roleId);
	Task<List<CustomRoleViewModel>> GetRolesAsync();
	Task<ResultViewModel> CreateRoleAsync(CustomRoleViewModel model);
	Task<ResultViewModel> EditRoleAsync(CustomRoleViewModel model);
	Task<ResultViewModel> DeleteRoleAsync(string roleId);
	Task<List<CustomUserRoleViewModel>> EditUsersInRoleAsync(string roleId);
	Task<ResultViewModel> EditUsersInRoleAsync(List<CustomUserRoleViewModel> model, string roleId);
}