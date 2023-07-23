

Public Class WeatherForecast
    Implements IWeatherForecast

    Public ReadOnly Summaries As String() = {"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"}
    Public Property [Date] As DateTime Implements IWeatherForecast.Date
    Public Property TemperatureC As Integer Implements IWeatherForecast.TemperatureC
    Public Property Summary As String Implements IWeatherForecast.Summary

    Public ReadOnly Property TemperatureF As Integer Implements IWeatherForecast.TemperatureF
        Get
            Return 32 + CInt((TemperatureC / 0.5556))
        End Get
    End Property
    Public Function [Get](Optional Num As Integer = 1) As IEnumerable(Of WeatherForecast) Implements IWeatherForecast.Get
        Dim rng = New Random()
        Return Enumerable.Range(1, Num).[Select](Function(index) New WeatherForecast With {
            .Date = DateTime.Now.AddDays(index),
            .TemperatureC = rng.[Next](-20, 55),
            .Summary = Summaries(rng.[Next](Summaries.Length))
        }).ToArray()
    End Function
End Class



