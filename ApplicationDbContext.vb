Imports System.Data.Entity
Imports Microsoft.EntityFrameworkCore
Public Class ApplicationDbContext
    Inherits Microsoft.EntityFrameworkCore.DbContext
    Public Sub New(ByVal options As DbContextOptions)
        MyBase.New(options)
    End Sub

    Public Test As Microsoft.EntityFrameworkCore.DbSet(Of OneTest)
End Class

Public Class OneTest
    Public DT As DateTime
    Public T As Integer
    Public S As String
    Public JS As String
End Class