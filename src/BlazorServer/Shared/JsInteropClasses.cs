using Microsoft.JSInterop;

namespace BlazorServer.Shared;

public class JsInteropClasses : IDisposable
{
	private readonly IJSRuntime _js;

	public JsInteropClasses(IJSRuntime js)
	{
		_js = js;
	}

	public void Dispose()
	{
	}

	public async ValueTask<bool> Confirm(string title)
	{
		var confirm = await _js.InvokeAsync<object?>("SweetConfirm", $"是否确定删除日志{title}?");
		if (confirm == null)
			return false;
		return bool.TryParse(confirm.ToString(), out var result) && result;
	}

	public async Task Alert(string title)
	{
		await _js.InvokeAsync<object?>("Alert", title);
	}
}