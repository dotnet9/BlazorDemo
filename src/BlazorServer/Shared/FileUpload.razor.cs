using BlazorServer.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Text.Json;

namespace BlazorServer.Shared;

public partial class FileUpload
{
	private JsInteropClasses? _jsClass;

	// 取得`<InputFile>`的文件内容
	public IReadOnlyList<IBrowserFile>? ImageFiles;

	public List<string> ImageList = new();
	public string? ImageSrc;
	[Inject] protected IJSRuntime Js { get; set; }

	/// <summary>
	///     用以判断runtime期间在什么环境执行
	/// </summary>
	[Inject]
	protected IWebHostEnvironment? Env { get; set; }

	protected override Task OnInitializedAsync()
	{
		_jsClass = new JsInteropClasses(Js);
		return base.OnInitializedAsync();
	}

	public async Task OnChange(InputFileChangeEventArgs e)
	{
		ImageList = new List<string>();
		const string format = "image/jpeg";

		// 取得文件
		ImageFiles = e.GetMultipleFiles();
		foreach (var file in ImageFiles)
		{
			// 将图片内容转换成指定类型及最大尺寸
			var imageFile = await file.RequestImageFileAsync(format, 1200, 675);

			// 利用Stream读取图片内容
			await using var fileStream = imageFile.OpenReadStream();

			// 将 Stream读取到内存中，如果没有确定要上传不要这么做，以免浪费内存
			await using var memoryStream = new MemoryStream();
			await fileStream.CopyToAsync(memoryStream);
			ImageSrc = $"data:{format};base64,{Convert.ToBase64String(memoryStream.ToArray())}";

			// 以Data URI的方式将图片呈现
			ImageList.Add(ImageSrc);
		}
	}

	public async Task OnSubmit()
	{
		// 将提示信息变成ViewModel
		var sweetConfirm = new SweetConfirmViewModel
		{
			RequestTitle = "是否确定上传图片？",
			ResponseTitle = "上传成功"
		};
		var jsonString = JsonSerializer.Serialize(sweetConfirm);
		var result = await _jsClass!.Confirm(jsonString);
		if (result && ImageFiles != null && ImageFiles.Any())
		{
			const long maxFileSize = 1024 * 1024 * 15;

			// 指定图片存储路径
			var folder = $@"{Env!.WebRootPath}\images";
			foreach (var file in ImageFiles)
			{
				// 使用Stream 将文件存储到指定路径
				await using var stream = file.OpenReadStream(maxFileSize);

				//如果文件夹不存在先创建
				Directory.CreateDirectory(folder);

				var path = $@"{Env.WebRootPath}\images\{file.Name}";

				//创建文件
				var fs = File.Create(path);

				// 将图片Stream复制到文件中
				await stream.CopyToAsync(fs);

				// Stream用完记得关闭
				stream.Close();
				fs.Close();
			}
		}
	}
}