namespace BlazorServer.Data;

public interface IWeatherForecastService
{
	Task<WeatherForecast[]> GetForecastAsync(DateTime startDate);
}