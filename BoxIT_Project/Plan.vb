Public Enum BackupPlan
    FullBackup
    IncrementalBackup
    DifferentialBackup
    None
End Enum

Public Class Plan
    Public Property Name As String = String.Empty
    Public Property Nxt_backup As String = String.Empty
    Public Property Src As String = String.Empty
    Public Property Dst As String = String.Empty
    Public Property backupPlan As BackupPlan = BackupPlan.None
End Class
