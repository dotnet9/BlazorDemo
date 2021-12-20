using System.ComponentModel.DataAnnotations;

namespace BlazorServer.ViewModels;

public class BlogViewModel
{
	public int Id { get; set; }

	[Required(ErrorMessage = "博客名称为必填！")]
	[MaxLength(10, ErrorMessage = "博客名称太长")]
	public string? Name { get; set; }

	public List<PostViewModel>? Posts { get; set; }

	public DateTime CreateDateTime { get; set; }
}