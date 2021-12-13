﻿using System.ComponentModel.DataAnnotations;

namespace BlazorServer.Models;

public class PostModel
{
	public int Id { get; set; }

	[Required]
	[MaxLength(10, ErrorMessage = "标题太长")]
	public string? Title { get; set; }

	[Required]
	[MinLength(100, ErrorMessage = "内容太短")]
	public string? Content { get; set; }

	public DateTime CreateDateTime { get; set; } = new(2021, 12, 13, 23, 40, 23);
}