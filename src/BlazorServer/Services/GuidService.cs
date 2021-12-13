namespace BlazorServer.Services;

public class GuidService : IGuidService
{
	public string? UId { get; set; }

	public GuidService()
	{
		UId = Guid.NewGuid().ToString();
	}
}