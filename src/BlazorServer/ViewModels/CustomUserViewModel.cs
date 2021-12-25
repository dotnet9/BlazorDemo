namespace BlazorServer.ViewModels;

public class CustomUserViewModel
{
	public CustomUserViewModel()
	{
		Claims = new List<string>();
	}

	public string? UserId { get; set; }

	public string? UserName { get; set; }

	public string? Email { get; set; }

	public List<string> Claims { get; set; }
}