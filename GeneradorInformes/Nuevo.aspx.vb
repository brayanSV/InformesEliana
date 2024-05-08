Imports Microsoft.VisualBasic.FileIO
Imports System.IO

Public Class Nuevo
    Inherits System.Web.UI.Page
    Dim delimitador As String = ";.;"
    Dim bandexito As Boolean = True
    Dim mensaje As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        contentError.Visible = False
        EliminarArchivosDeCarpeta(Server.MapPath("~/Archivos/"))
        EliminarArchivosDeCarpeta(Server.MapPath("~/Archivos/Informes"))
    End Sub

    Protected Sub EliminarArchivosDeCarpeta(ByVal rutaCarpeta As String)
        Try
            ' Verificar si la carpeta existe
            If Directory.Exists(rutaCarpeta) Then
                ' Obtener la lista de archivos en la carpeta
                Dim archivos As String() = Directory.GetFiles(rutaCarpeta)

                ' Iterar sobre cada archivo y eliminarlo
                For Each archivo As String In archivos
                    File.Delete(archivo)
                Next
            Else
            End If
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub BtnEnviar_Click(sender As Object, e As EventArgs)
        contentError.Visible = False
        Dim ruta As String = Server.MapPath("~/Archivos/")

        If chkInforme1.Checked Then
            If fileInforme1.HasFile Then
                fileInforme1.SaveAs(Path.Combine(ruta, fileInforme1.FileName))
                EliminarFIlas(fileInforme1.FileName, 1)
            End If
        End If

        If bandexito Then
            If chkInforme2.Checked Then
                If fileInforme2.HasFile Then
                    fileInforme2.SaveAs(Path.Combine(ruta, fileInforme2.FileName))
                    EliminarFIlas(fileInforme2.FileName, 2)
                End If
            End If
        End If

        If bandexito Then
            If chkInforme3.Checked Then
                If fileInforme3.HasFile Then
                    fileInforme3.SaveAs(Path.Combine(ruta, fileInforme3.FileName))
                    EliminarFIlas(fileInforme3.FileName, 3)
                End If
            End If
        End If

        If bandexito = True Then
            Response.Redirect("Default.aspx")
        Else
            contentError.Visible = True
        End If
    End Sub

    Protected Sub ShowAlert(ByVal mensaje As String)
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alert", "alert('" & mensaje & "')", True)
    End Sub

    Protected Sub EliminarFIlas(filename As String, numinforme As Integer)
        Dim rutaArchivoOriginal As String = "C:/inetpub/wwwroot/Informes/Archivos/" & filename
        Dim rutaNuevoArchivo As String = "C:/inetpub/wwwroot/Informes/Archivos/Informes/" & filename

        ' Llama a la función para realizar la eliminación de filas
        EliminarFilasCSV(rutaArchivoOriginal, rutaNuevoArchivo, numinforme)
    End Sub

    Sub EliminarFilasCSV(rutaArchivoOriginal As String, rutaNuevoArchivo As String, numinforme As Integer)
        ' Lee todas las líneas del archivo original
        Dim lineas As List(Of String) = File.ReadAllLines(rutaArchivoOriginal).ToList()

        ' Verifica si hay al menos 5 filas en el archivo original (4 primeras + 1 última)
        If lineas.Count >= 5 Then
            lineas.RemoveRange(0, 3) ' Elimina las primeras 4 filas
            lineas.RemoveAt(lineas.Count - 1) ' Elimina la última fila

            ' Cambia el delimitador en cada línea, pero solo fuera de las comillas
            For i As Integer = 0 To lineas.Count - 1
                lineas(i) = CambiarDelimitadorEnLinea(lineas(i), "|")

                ' Cambia el delimitador "," por el nuevo, luego reemplzamos las | por comas y eliminamos las comillas
                lineas(i) = lineas(i).Replace(",", delimitador).Replace("|", ",").Replace("""", "")
            Next

            ' Guarda las líneas restantes en el nuevo archivo
            File.WriteAllLines(rutaNuevoArchivo, lineas, Encoding.UTF8) ' Puedes ajustar la codificación según tus necesidades         

            ' Llamada a GenerarInformes después de crear el nuevo archivo
            Select Case numinforme
                Case 1
                    GenerarInformes("informe_uno", rutaNuevoArchivo, numinforme)
                Case 2
                    GenerarInformes("informe_dos", rutaNuevoArchivo, numinforme)
                Case 3
                    GenerarInformes("informe_tres", rutaNuevoArchivo, numinforme)
            End Select
        End If
    End Sub

    Function CambiarDelimitadorEnLinea(linea As String, nuevoDelimitador As Char) As String
        ' Utiliza expresiones regulares para encontrar texto entre comillas
        Dim patron As String = """(.*?)"""
        Dim coincidencias As MatchCollection = Regex.Matches(linea, patron)

        ' Reemplaza comas por el nuevo delimitador fuera de las comillas
        For Each coincidencia As Match In coincidencias
            Dim textoEntreComillas As String = coincidencia.Groups(1).Value
            Dim textoReemplazado As String = textoEntreComillas.Replace(",", nuevoDelimitador)
            Dim textoFinal As String = """" & textoReemplazado & """"
            linea = linea.Replace(coincidencia.Value, textoFinal)
        Next

        Return linea
    End Function

    Protected Sub GenerarInformes(tableName As String, filePath As String, numinforme As Integer)
        ' Eliminar la tabla existente
        If CreateInsertDeleteTable(GenerateScriptDeleteTable($"{tableName}_data")) Then
            ' Configuración del DataTable para el informe
            Dim dtInforme As New DataTable("informe")

            ' Configuración del lector CSV
            Using reader As New TextFieldParser(filePath)
                reader.TextFieldType = FieldType.Delimited
                reader.SetDelimiters(delimitador)

                ' Procesamiento del archivo CSV
                While Not reader.EndOfData
                    Dim fields() As String = reader.ReadFields()

                    ' Contadores de columnas
                    Dim counterColumns As Integer = 0

                    ' Procesar cabeceras y crear columnas en el DataTable
                    For Each field In fields
                        counterColumns += 1
                        Dim columnName As String = SanitizeColumnName(field)

                        If ColumExist(dtInforme, columnName) Then
                            columnName &= counterColumns
                        End If

                        Dim newColumn As New DataColumn(columnName)
                        dtInforme.Columns.Add(newColumn)
                    Next

                    Exit While
                End While
            End Using

            ' Crear la nueva tabla            
            If CreateInsertDeleteTable(GenerateScriptCreateTable(dtInforme, $"{tableName}_data")) Then
                If bulkInsertTable(filePath, $"{tableName}_data") Then
                    Select Case numinforme
                        Case 1
                            If CreateInsertDeleteTable(InsertInformeUno()) = False Then
                                bandexito = False
                                lblmensaje.Text = "No se pudo generar el informe Detailed Job Status"
                            End If
                        Case 2
                            If CreateInsertDeleteTable(InsertInformeDos(dtInforme)) = False Then
                                bandexito = False
                                lblmensaje.Text = "No se pudo generar el informe Data respaldada por cliente"
                            End If
                        Case 3
                            If CreateInsertDeleteTable(InsertInformeTres()) = False Then
                                bandexito = False
                                lblmensaje.Text = "No se pudo generar el informe Restauraciones"
                            End If
                    End Select
                End If
            End If
        End If
    End Sub

    Protected Function GenerateScriptCreateTable(dt As DataTable, name As String)
        Dim script As String = "CREATE TABLE " & name & " ("
        For Each col As DataColumn In dt.Columns
            script &= col.ColumnName & " NVARCHAR(255),"
        Next
        script &= ");"

        Return script.Replace(",);", ");")
    End Function

    Protected Function GenerateScriptDeleteTable(name As String)
        Dim script As String = "IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '" & name & "' AND TABLE_SCHEMA = 'dbo')" &
                               "BEGIN" &
                               "    DROP TABLE " & name & ";" &
                               "END"
        Return script
    End Function

    Protected Function CreateInsertDeleteTable(script As String) As Boolean
        Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("conexion").ToString

        Using MyConnection As New SqlClient.SqlConnection(connectionString)
            Try
                MyConnection.Open()

                Using myTrans As SqlClient.SqlTransaction = MyConnection.BeginTransaction()
                    Try
                        Using MyCommand As New SqlClient.SqlCommand(script, MyConnection, myTrans)
                            MyCommand.CommandTimeout = 5000
                            MyCommand.ExecuteNonQuery()
                        End Using

                        ' Confirmar la transacción si todo está bien
                        myTrans.Commit()
                        Return True
                    Catch ex As SqlClient.SqlException
                        ' Manejar errores específicos de SQL
                        myTrans.Rollback()
                        'lblprueba.Text = "Error SQL: " & ex.Message
                        Return False
                    End Try
                End Using
            Catch ex As Exception
                ' Manejar otros errores
                'lblprueba.Text = "Error: " & ex.Message
                Return False
            End Try
        End Using
    End Function

    Private Function SanitizeColumnName(columnName As String) As String
        Return columnName.ToLower().Replace(" -", "_").Replace("-", "_").Replace("/", "_").
        Replace(" ", "_").Replace("(", "").Replace(")", "").Replace(".", "_").Replace(":", "").Replace("__", "_")
    End Function

    Private Function ColumExist(dt As DataTable, columName As String) As String
        Return dt.Columns.Contains(columName)
    End Function

    Protected Function bulkInsertTable(fileName As String, tableName As String) As Boolean
        Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("conexion").ToString

        Using MyConnection As New SqlClient.SqlConnection(connectionString)
            Try
                MyConnection.Open()

                Using myTrans As SqlClient.SqlTransaction = MyConnection.BeginTransaction()
                    Try
                        Dim query As String =
                            $"BULK INSERT {tableName} " &
                            $"FROM '{fileName}' " &
                            "WITH ( " &
                            "    FIELDTERMINATOR = '" & delimitador & "', " &
                            "    ROWTERMINATOR = '\n', " &
                            "    FIRSTROW = 2 " &
                            "); "

                        Using MyCommand As New SqlClient.SqlCommand(query, MyConnection, myTrans)
                            MyCommand.CommandTimeout = 5000
                            MyCommand.ExecuteNonQuery()
                        End Using

                        myTrans.Commit()

                        Return True
                    Catch ex As SqlClient.SqlException
                        myTrans.Rollback()
                        'lblprueba.Text = "Error SQL: " & ex.Message
                        Return False
                    End Try
                End Using
            Catch ex As Exception
                ' Manejar otros errores
                'lblprueba.Text = "Error: " & ex.Message
                Return False
            End Try
        End Using
    End Function

    Protected Function InsertInformeUno() As String
        Dim script As New StringBuilder()
        script.AppendLine("DELETE FROM informe_uno;")
        script.AppendLine(
            "	WITH RankedResults AS ( " &
            "		SELECT " &
            "			[job_type], " &
            "			[status_code], " &
            "			COUNT([job_type]) AS cuenta_job_type, " &
            "			ROW_NUMBER() OVER (PARTITION BY [job_type] ORDER BY COUNT([job_type]) DESC) AS RowNum " &
            "		FROM " &
            "			[Informes].[dbo].[informe_uno_data] " &
            "		WHERE " &
            "			[status_code] > 0 " &
            "		GROUP BY " &
            "			[job_type], [status_code] " &
            "	) " &
            "	INSERT INTO [Informes].[dbo].[informe_uno] ([job_type], [status_code], [cuantos_job_type]) " &
            "	SELECT " &
            "		[job_type], " &
            "		[status_code], " &
            "		cuenta_job_type " &
            "	FROM " &
            "		RankedResults " &
            "	WHERE " &
            "		RowNum <= 3; "
        )

        Return script.ToString()
    End Function

    Protected Function InsertInformeDos(dt As DataTable) As String
        Dim script As New StringBuilder()
        script.AppendLine("DELETE FROM informe_dos;")
        script.AppendLine("INSERT INTO informe_dos (date_time_text, date_time, total_tb)")

        Dim dateColumns As New List(Of String)()
        Dim jobColumns As New List(Of String)()

        For Each col As DataColumn In dt.Columns
            If col.ColumnName.Contains("date_time") Then
                dateColumns.Add(col.ColumnName)
            ElseIf col.ColumnName.Contains("job") Then
                'jobColumns.Add($"CAST(REPLACE(REPLACE({col.ColumnName},'.',','),',','') AS DECIMAL)")
                jobColumns.Add($"CAST(REPLACE({col.ColumnName},',','') AS FLOAT)")
            End If
        Next

        script.AppendLine($"SELECT DISTINCT {String.Join(", ", dateColumns)},")
        script.AppendLine($"CONVERT(NVARCHAR, CONVERT(DATE, date_time, 107), 111) AS date_yyyymmdd,")
        script.AppendLine($"(({String.Join(" + ", jobColumns)}) / 1024) AS total")
        script.AppendLine("FROM informe_dos_data ORDER BY date_yyyymmdd;")

        Return script.ToString()
    End Function

    Protected Function InsertInformeTres() As String
        Dim script As New StringBuilder()
        script.AppendLine("DELETE FROM informe_tres;")
        script.AppendLine(
            "	INSERT INTO informe_tres(client_name, count_job_id, size_gb) " &
            "	SELECT DISTINCT client_name, " &
            "					COUNT(job_id) as count_job_id, " &
            "					SUM(CAST(REPLACE(REPLACE(sizegb,'.',','),',','') AS DECIMAL)) AS total_gb " &
            "			   FROM informe_tres_data " &
            "		   GROUP BY client_name "
        )

        Return script.ToString()
    End Function

End Class