Imports Microsoft.Extensions.Logging
Imports Microsoft.EntityFrameworkCore.NpgsqlDatabaseFacadeExtensions
Imports Microsoft.EntityFrameworkCore
Imports System.Data.SqlClient
Imports Newtonsoft.Json
Imports Npgsql

Public Class WriteToPostgres
    Implements IWriteToPostgres

    Dim _DB As ApplicationDbContext
    Dim _WS As IWeatherForecast
    Dim _Log As ILogger(Of WriteToPostgres)
    Public Sub New(DB As ApplicationDbContext, WS As WeatherForecast, Log As ILogger(Of WriteToPostgres))
        _DB = DB
        _WS = WS
        _Log = Log
    End Sub

    Public Sub Write(Optional Num As Integer = 1) Implements IWriteToPostgres.Write
        Try
            _DB.Database.OpenConnection()
        Catch ex As Exception
            _Log.LogError(ex.Message)
        End Try
        Try
            Dim I As Integer
            For Each One As IWeatherForecast In _WS.Get(Num).ToList
                Dim JS = New With {.DT = One.Date,
                                   .S = One.Summary,
                                   .T = One.TemperatureC}
                Dim DT = String.Format("{0:yyyy-MM-dd}", One.Date)
                Dim CMD1 = New NpgsqlCommand()
                CMD1.CommandText = $"INSERT INTO public.""Test""(""DT"", ""S"", ""T"",""JS"") VALUES('{DT}','{One.Summary}','{One.TemperatureC}', '{{{JsonConvert.SerializeObject(JS)}}}' );"
                _Log.LogInformation($"{I}: {CMD1.CommandText}")
                _DB.Database.ExecuteSqlRaw(CMD1.CommandText)
                I = I + 1
            Next
        Catch ex As Exception
            _Log.LogError(ex.Message)
        End Try
    End Sub
End Class
