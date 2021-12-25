namespace BlazorServer.ViewModels;

public class CustomUserClaimsViewModel
{
	public CustomUserClaimsViewModel()
	{
		Claims = new List<CustomUserClaimViewModel>();
	}

	public string? UserId { get; set; }

	public List<CustomUserClaimViewModel> Claims { get; set; }
}