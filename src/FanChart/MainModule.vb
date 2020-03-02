Module MainModule

    Sub Main(cmdArgs() As String)

        If cmdArgs.Length = 1 AndAlso cmdArgs(0) = "--run-now" Then

            Dim process As New EngineProcess
            process.ProcessAllAsync().Wait()

        Else
            Dim f As New Main()
            f.Show()
            AddHandler f.FormClosed, Sub() Application.Exit()
            Application.Run()
        End If

    End Sub

End Module
