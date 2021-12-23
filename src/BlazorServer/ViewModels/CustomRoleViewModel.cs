using System.ComponentModel.DataAnnotations;

namespace BlazorServer.ViewModels;

public class CustomRoleViewModel
{
	public string? Id { get; set; }

	[Required(ErrorMessage = "角色名称为必填")] public string? Name { get; set; }

	public List<string>? Users { get; set; }
}