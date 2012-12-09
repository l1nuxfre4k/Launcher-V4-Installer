Imports System
Imports System.Threading
Imports System.IO
Imports System.Net

Public Class Main

    Dim CommandLineArgs As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Application.CommandLineArgs
    Dim update As BootMode = False

    Dim WithEvents WC1 As New WebClient
    Dim WithEvents WC2 As New WebClient
    Dim WithEvents WC3 As New WebClient

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            If CommandLineArgs(0) = "update" Then
                update = True
            End If
        Catch ex As Exception
        End Try
        If update = True Then
            Text = Text & "Updater"
            CheckBox1.Hide()
            Button1.Hide()
            Label1.Text = "Installing The MineUK Launcher..."
            Label2.Show()
            ProgressBar1.Show()
            BackgroundWorker1.RunWorkerAsync()
        Else
            Text = Text & "Installer"
            CheckBox1.Show()
            Button1.Show()
            Label2.Hide()
            ProgressBar1.Hide()
        End If
        Button1.Enabled = CheckBox1.Checked
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        CheckBox1.Hide()
        Button1.Hide()
        Label1.Text = "Installing The MineUK Launcher..."
        Label2.Show()
        ProgressBar1.Show()
        BackgroundWorker1.RunWorkerAsync()
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Thread.Sleep(500)
        Dim proc1 = Process.GetProcessesByName("MineUK Launcher")
        For i As Integer = 0 To proc1.Count - 1
            Try
                proc1(i).CloseMainWindow()
                Thread.Sleep(500)
                proc1(i).Kill()
            Catch ex As Exception
            End Try
        Next
        Dim proc2 = Process.GetProcessesByName("java")
        For i As Integer = 0 To proc2.Count - 1
            Try
                proc2(i).CloseMainWindow()
                Thread.Sleep(500)
                proc2(i).Kill()
            Catch ex As Exception
            End Try
        Next
        Dim proc3 = Process.GetProcessesByName("javaw")
        For i As Integer = 0 To proc3.Count - 1
            Try
                proc3(i).CloseMainWindow()
                Thread.Sleep(500)
                proc3(i).Kill()
            Catch ex As Exception
            End Try
        Next
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Label2.Text = ("Removing Old Files...")
        BackgroundWorker2.RunWorkerAsync()
    End Sub

    Private Sub BackgroundWorker2_DoWork(sender As Object, e As ComponentModel.DoWorkEventArgs) Handles BackgroundWorker2.DoWork
        Try
            For Each i As String In Directory.GetDirectories(Environ("appdata") & "\MineUK Launcher")
                If Path.GetFileName(i) = "Tekkit" Then
                Else
                    If Path.GetFileName(i) = "FTB" Then
                    Else : My.Computer.FileSystem.DeleteDirectory(Path.GetFullPath(i), FileIO.DeleteDirectoryOption.DeleteAllContents)
                    End If
                End If
            Next
        Catch ex As Exception
        End Try
        Try
            For Each i As String In Directory.GetFiles(Environ("appdata") & "\MineUK Launcher")
                My.Computer.FileSystem.DeleteFile(Path.GetFullPath(i))
            Next
        Catch ex As Exception
        End Try
        If My.Computer.FileSystem.DirectoryExists(Environ("appdata") & "\MineUK Launcher") Then
        Else : My.Computer.FileSystem.CreateDirectory(Environ("appdata") & "\MineUK Launcher")
        End If
        If My.Computer.FileSystem.FileExists(Environ("appdata") & "\MineUK Launcher\script.bat") Then
            My.Computer.FileSystem.DeleteFile(Environ("appdata") & "\MineUK Launcher\script.bat")
        End If
        If My.Computer.FileSystem.FileExists(Environ("appdata") & "\MineUK Launcher\7za.exe") Then
            My.Computer.FileSystem.DeleteFile(Environ("appdata") & "\MineUK Launcher\7za.exe")
        End If
        If My.Computer.FileSystem.FileExists(Environ("appdata") & "\MineUK Launcher\files.7z") Then
            My.Computer.FileSystem.DeleteFile(Environ("appdata") & "\MineUK Launcher\files.7z")
        End If
    End Sub

    Private Sub BackgroundWorker2_RunWorkerCompleted(sender As Object, e As ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker2.RunWorkerCompleted
        Label2.Text = ("Downloading 7zip...")
        ProgressBar1.Style = ProgressBarStyle.Blocks
        WC1.DownloadFileAsync(New Uri("http://launcher.mineuk.com/v3/7za.exe"), Environ("appdata") & "\MineUK Launcher\7za.exe")
    End Sub

    Private Sub WC1_DownloadProgressChanged(ByVal sender As Object, ByVal e As DownloadProgressChangedEventArgs) Handles WC1.DownloadProgressChanged
        ProgressBar1.Value = e.ProgressPercentage
    End Sub

    Private Sub WC1_DownloadFileCompleted(sender As Object, e As ComponentModel.AsyncCompletedEventArgs) Handles WC1.DownloadFileCompleted
        Label2.Text = ("Downloading Core Files...")
        WC2.DownloadFileAsync(New Uri("http://launcher.mineuk.com/v3/files.7z"), Environ("appdata") & "\MineUK Launcher\files.7z")
    End Sub

    Private Sub WC2_DownloadProgressChanged(ByVal sender As Object, ByVal e As DownloadProgressChangedEventArgs) Handles WC2.DownloadProgressChanged
        ProgressBar1.Value = e.ProgressPercentage
    End Sub

    Private Sub WC2_DownloadFileCompleted(sender As Object, e As ComponentModel.AsyncCompletedEventArgs) Handles WC2.DownloadFileCompleted
        Label2.Text = ("Installing...")
        ProgressBar1.Style = ProgressBarStyle.Marquee
        BackgroundWorker3.RunWorkerAsync()
    End Sub

    Private Sub BackgroundWorker3_DoWork(sender As Object, e As ComponentModel.DoWorkEventArgs) Handles BackgroundWorker3.DoWork
        Dim writer As StreamWriter
        writer = My.Computer.FileSystem.OpenTextFileWriter(Environ("appdata") & "\MineUK Launcher\Script.bat", True)
        Writer.WriteLine("@ECHO OFF")
        writer.WriteLine("@ECHO OFF")
        writer.WriteLine("cd " & """%appdata%\MineUK Launcher""")
        writer.WriteLine("7za  x files.7z * -y")
        writer.Close()
        Thread.Sleep(200)
        writer.Dispose()
        Dim objProcesss As Process
        objProcesss = New Process()
        objProcesss.StartInfo.WorkingDirectory = Environ("appdata") & "\MineUK Launcher\"
        objProcesss.StartInfo.FileName = "Script.bat"
        objProcesss.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        objProcesss.Start()
        objProcesss.WaitForExit()
        objProcesss.Close()
    End Sub

    Private Sub BackgroundWorker3_RunWorkerCompleted(sender As Object, e As ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker3.RunWorkerCompleted
        Label2.Text = ("Finishing...")
        BackgroundWorker4.RunWorkerAsync()
    End Sub

    Private Sub BackgroundWorker4_DoWork(sender As Object, e As ComponentModel.DoWorkEventArgs) Handles BackgroundWorker4.DoWork
        Thread.Sleep(200)
        Dim wsh1 As Object = CreateObject("WScript.Shell")
        wsh1 = CreateObject("WScript.Shell")
        Dim MyShortcut1, DesktopPath1
        DesktopPath1 = wsh1.SpecialFolders("Desktop")
        MyShortcut1 = wsh1.CreateShortcut(DesktopPath1 & "\MineUK Launcher.lnk")
        MyShortcut1.TargetPath = wsh1.ExpandEnvironmentStrings(Environ("appdata") & "\MineUK Launcher\MineUK Launcher.exe")
        MyShortcut1.WorkingDirectory = wsh1.ExpandEnvironmentStrings(Environ("appdata") & "\MineUK Launcher")
        MyShortcut1.WindowStyle = 4
        MyShortcut1.Save()
        Dim wsh2 As Object = CreateObject("WScript.Shell")
        wsh2 = CreateObject("WScript.Shell")
        Dim MyShortcut2, DesktopPath2
        DesktopPath2 = wsh2.SpecialFolders("StartMenu")
        MyShortcut2 = wsh2.CreateShortcut(DesktopPath2 & "\MineUK Launcher.lnk")
        MyShortcut2.TargetPath = wsh2.ExpandEnvironmentStrings(Environ("appdata") & "\MineUK Launcher\MineUK Launcher.exe")
        MyShortcut2.WorkingDirectory = wsh2.ExpandEnvironmentStrings(Environ("appdata") & "\MineUK Launcher")
        MyShortcut2.WindowStyle = 4
        MyShortcut2.Save()
        Thread.Sleep(200)
        Dim objProcesss As Process
        objProcesss = New Process()
        objProcesss.StartInfo.WorkingDirectory = Environ("appdata") & "\MineUK Launcher\"
        objProcesss.StartInfo.FileName = "MineUK Launcher.exe"
        objProcesss.Start()
        objProcesss.Close()
    End Sub

    Private Sub BackgroundWorker4_RunWorkerCompleted(sender As Object, e As ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker4.RunWorkerCompleted
        Me.Close()
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        Button1.Enabled = CheckBox1.Checked
    End Sub
End Class
