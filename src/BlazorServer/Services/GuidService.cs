namespace BlazorServer.Services;

public class GuidService : IGuidService
{
	public GuidService()
	{
		UId = Guid.NewGuid().ToString();
	}

	public string? UId { get; set; }
}