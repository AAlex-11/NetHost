Public Interface IWeatherForecast
    Property [Date] As Date
    Property Summary As String
    Property TemperatureC As Integer
    ReadOnly Property TemperatureF As Integer
    Function [Get](Optional Num As Integer = 1) As IEnumerable(Of WeatherForecast)
End Interface
