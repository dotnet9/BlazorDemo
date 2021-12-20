using System.ComponentModel.DataAnnotations;

namespace BlazorServer.ViewModels;

public class PostViewModel
{
	public int Id { get; set; }

	[Required]
	[MaxLength(10, ErrorMessage = "标题太长")]
	public string? Title { get; set; }

	[Required]
	[MinLength(100, ErrorMessage = "内容太短")]
	public string? Content { get; set; }

	public int BlogId { get; set; }

	public BlogViewModel? Blog { get; set; }

	public DateTime CreateDateTime { get; set; }

	public DateTime UpdateDateTime { get; set; }
}