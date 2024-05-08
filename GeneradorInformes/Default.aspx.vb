Imports System.IO
Imports Microsoft.VisualBasic.FileIO
Imports OfficeOpenXml

Public Class _Default
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        cargar_informes(Me, New EventArgs)
    End Sub

    Protected Sub cargar_informes(ByVal sender As Object, ByVal e As EventArgs)
        Dim MyConnection As SqlClient.SqlConnection
        MyConnection = New SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conexion").ToString)
        MyConnection.Open()

        Dim myTrans As SqlClient.SqlTransaction
        Dim MyCommand As SqlClient.SqlCommand
        myTrans = MyConnection.BeginTransaction()

        Dim InsertCmd As String = ""
        Dim myDataSet As New Data.DataSet()

        Try
            InsertCmd = "	select SUM(informes.total) as totalreg " &
                        "	  from ( " &
                        "		select count(informe_uno.cuantos_job_type) as total " &
                        "		  from informe_uno " &
                        "		 union " &
                        "		select count(informe_dos.date_time) as total " &
                        "		  from informe_dos " &
                        "		 union " &
                        "		select count(informe_tres.client_name) as total " &
                        "		  from informe_tres " &
                        "	  ) as informes "

            MyCommand = New SqlClient.SqlCommand(InsertCmd, MyConnection)

            MyCommand.Transaction = myTrans
            MyCommand.CommandTimeout = 5000
            MyCommand.ExecuteScalar()

            Dim myDataAdapter As New System.Data.SqlClient.SqlDataAdapter(MyCommand)
            myDataAdapter.Fill(myDataSet, "informes")

            If myDataSet.Tables("informes").Rows.Count > 0 Then
                If Not IsDBNull(myDataSet.Tables("informes").Rows(0)("totalreg")) Then
                    Dim totalreg As Integer = Convert.ToInt32(myDataSet.Tables("informes").Rows(0)("totalreg"))

                    If totalreg > 0 Then
                        pnlinforme1.Visible = False
                        pnlinforme2.Visible = False
                        pnlinforme3.Visible = False
                        pnlGridInformes.Visible = True

                        Dim dt As New DataTable("informes")
                        Dim informe As New DataColumn("informe")
                        Dim numinforme As New DataColumn("numinforme")
                        dt.Columns.Add(informe)
                        dt.Columns.Add(numinforme)

                        Dim informeUno As DataRow = dt.NewRow
                        informeUno("informe") = "Detailed Job Status"
                        informeUno("numinforme") = "1"
                        dt.Rows.Add(informeUno)

                        Dim informeDos As DataRow = dt.NewRow
                        informeDos("informe") = "Data respaldada por cliente"
                        informeDos("numinforme") = "2"
                        dt.Rows.Add(informeDos)

                        Dim informeTres As DataRow = dt.NewRow
                        informeTres("informe") = "Restauraciones"
                        informeTres("numinforme") = "3"
                        dt.Rows.Add(informeTres)

                        gridinformes.DataSource = dt.DefaultView
                        gridinformes.DataBind()
                    Else
                        pnlinforme1.Visible = False
                        pnlinforme2.Visible = False
                        pnlinforme3.Visible = False
                        pnlGridInformes.Visible = False
                    End If
                End If
            End If
        Catch ex As Exception
        End Try

        MyConnection.Close()
    End Sub

    Protected Sub accion_grid(ByVal sender As Object, e As CommandEventArgs)
        Select Case e.CommandArgument
            Case 1
                cargar_informeUno(Me, New EventArgs)
            Case 2
                cargar_informeDos(Me, New EventArgs)
            Case 3
                cargar_informeTres(Me, New EventArgs)
        End Select
    End Sub

    Protected Sub cargar_informeUno(ByVal sender As Object, ByVal e As EventArgs)
        pnlinforme1.Visible = True
        pnlinforme2.Visible = False
        pnlinforme3.Visible = False
        pnlGridInformes.Visible = False

        Dim MyConnection As SqlClient.SqlConnection
        MyConnection = New SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conexion").ToString)
        MyConnection.Open()

        Dim myTrans As SqlClient.SqlTransaction
        Dim MyCommand As SqlClient.SqlCommand
        myTrans = MyConnection.BeginTransaction()

        Dim InsertCmd As String = ""
        Dim myDataSet As New Data.DataSet()

        Try
            InsertCmd = "	select distinct informe_uno.job_type, " &
                        "					informe_uno.status_code, " &
                        "					informe_uno.cuantos_job_type " &
                        "			   from informe_uno "

            MyCommand = New SqlClient.SqlCommand(InsertCmd, MyConnection)

            MyCommand.Transaction = myTrans
            MyCommand.CommandTimeout = 5000
            MyCommand.ExecuteScalar()

            Dim myDataAdapter As New System.Data.SqlClient.SqlDataAdapter(MyCommand)
            myDataAdapter.Fill(myDataSet, "informe_uno")

            'If myDataSet.Tables("informe_uno").Rows.Count > 0 Then
            '    If Not IsDBNull(myDataSet.Tables("informe_uno").Rows(0)("job_type")) Then
            '    End If
            'End If
        Catch ex As Exception
        End Try

        MyConnection.Close()

        gridInformeUno.DataSource = myDataSet.Tables("informe_uno").DefaultView
        gridInformeUno.DataBind()
    End Sub

    Protected Sub gridInformeUno_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gridInformeUno.PageIndexChanging
        ' Establecer el nuevo índice de página
        gridInformeUno.PageIndex = e.NewPageIndex

        ' Volver a enlazar los datos al GridView
        cargar_informeUno(sender, e)
    End Sub

    Protected Sub ExportarCSVInformeUno(ByVal sender As Object, ByVal e As EventArgs)
        Dim MyConnection As SqlClient.SqlConnection
        MyConnection = New SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conexion").ToString)
        MyConnection.Open()

        Dim myTrans As SqlClient.SqlTransaction
        Dim MyCommand As SqlClient.SqlCommand
        myTrans = MyConnection.BeginTransaction()

        Dim InsertCmd As String = ""
        Dim myDataSet As New Data.DataSet()

        Try
            InsertCmd = "	select distinct informe_uno.job_type, " &
                        "					informe_uno.status_code, " &
                        "					informe_uno.cuantos_job_type " &
                        "			   from informe_uno "

            MyCommand = New SqlClient.SqlCommand(InsertCmd, MyConnection)

            MyCommand.Transaction = myTrans
            MyCommand.CommandTimeout = 5000
            MyCommand.ExecuteScalar()

            Dim myDataAdapter As New System.Data.SqlClient.SqlDataAdapter(MyCommand)
            myDataAdapter.Fill(myDataSet, "informe_uno")

            If myDataSet.Tables("informe_uno").Rows.Count > 0 Then
                If Not IsDBNull(myDataSet.Tables("informe_uno").Rows(0)("job_type")) Then
                    Using package As New ExcelPackage()
                        ' Agregar una nueva hoja de trabajo al archivo
                        Dim worksheet = package.Workbook.Worksheets.Add("Informe Uno")

                        ' Obtener los datos del DataSet
                        Dim informeUnoTable As DataTable = myDataSet.Tables("informe_uno")

                        ' Verificar si la tabla contiene datos
                        If informeUnoTable IsNot Nothing AndAlso informeUnoTable.Rows.Count > 0 Then
                            ' Definir el encabezado de la hoja de trabajo
                            For i As Integer = 0 To informeUnoTable.Columns.Count - 1
                                worksheet.Cells(1, i + 1).Value = informeUnoTable.Columns(i).ColumnName
                            Next

                            ' Llenar los datos en la hoja de trabajo
                            For i As Integer = 0 To informeUnoTable.Rows.Count - 1
                                For j As Integer = 0 To informeUnoTable.Columns.Count - 1
                                    worksheet.Cells(i + 2, j + 1).Value = informeUnoTable.Rows(i)(j)
                                Next
                            Next

                            ' Autoajustar el ancho de las columnas
                            worksheet.Cells.AutoFitColumns()

                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                            Response.ContentEncoding = System.Text.Encoding.UTF8
                            Response.AppendHeader("content-disposition", "attachment; filename=detailed_job_status_" & DateTime.Now.ToString("ddMMyyyyHHmmss") & ".xlsx")
                            Response.BinaryWrite(package.GetAsByteArray())
                            Response.Flush()
                            Response.End()
                        Else
                            ' No hay datos en la tabla
                            ' Aquí puedes manejar el caso de que la tabla esté vacía
                        End If
                    End Using
                End If
            End If
        Catch ex As Exception
        End Try

        MyConnection.Close()

    End Sub

    Protected Sub cargar_informeDos(ByVal sender As Object, ByVal e As EventArgs)
        pnlinforme1.Visible = False
        pnlinforme2.Visible = True
        pnlinforme3.Visible = False
        pnlGridInformes.Visible = False

        Dim MyConnection As SqlClient.SqlConnection
        MyConnection = New SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conexion").ToString)
        MyConnection.Open()

        Dim myTrans As SqlClient.SqlTransaction
        Dim MyCommand As SqlClient.SqlCommand
        myTrans = MyConnection.BeginTransaction()

        Dim InsertCmd As String = ""
        Dim myDataSet As New Data.DataSet()

        Try
            InsertCmd = "		select informe_dos.date_time_text, " &
                        "			   replace(cast(informe_dos.total_tb as decimal(18,2)), '.', ',') as total_tb " &
                        "		  from informe_dos " &
                        "	  order by informe_dos.date_time asc "

            MyCommand = New SqlClient.SqlCommand(InsertCmd, MyConnection)

            MyCommand.Transaction = myTrans
            MyCommand.CommandTimeout = 5000
            MyCommand.ExecuteScalar()

            Dim myDataAdapter As New System.Data.SqlClient.SqlDataAdapter(MyCommand)
            myDataAdapter.Fill(myDataSet, "informe_dos")
        Catch ex As Exception

        End Try

        MyConnection.Close()

        gridInformeDos.DataSource = myDataSet.Tables("informe_dos").DefaultView
        gridInformeDos.DataBind()
    End Sub

    Protected Sub gridInformeDos_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gridInformeUno.PageIndexChanging
        ' Establecer el nuevo índice de página
        gridInformeDos.PageIndex = e.NewPageIndex

        ' Volver a enlazar los datos al GridView
        cargar_informeDos(sender, e)
    End Sub

    Protected Sub ExportarCSVInformeDos(ByVal sender As Object, ByVal e As EventArgs)
        Dim MyConnection As SqlClient.SqlConnection
        MyConnection = New SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conexion").ToString)
        MyConnection.Open()

        Dim myTrans As SqlClient.SqlTransaction
        Dim MyCommand As SqlClient.SqlCommand
        myTrans = MyConnection.BeginTransaction()

        Dim InsertCmd As String = ""
        Dim myDataSet As New Data.DataSet()

        Try
            InsertCmd = "		select informe_dos.date_time_text as 'Date Time', " &
                        "			   cast(informe_dos.total_tb as float) as 'Total TB' " &
                        "		  from informe_dos " &
                        "	  order by informe_dos.date_time asc "

            MyCommand = New SqlClient.SqlCommand(InsertCmd, MyConnection)

            MyCommand.Transaction = myTrans
            MyCommand.CommandTimeout = 5000
            MyCommand.ExecuteScalar()

            Dim myDataAdapter As New System.Data.SqlClient.SqlDataAdapter(MyCommand)
            myDataAdapter.Fill(myDataSet, "informe_dos")

            If myDataSet.Tables("informe_dos").Rows.Count > 0 Then
                If Not IsDBNull(myDataSet.Tables("informe_dos").Rows(0)("Date Time")) Then
                    Using package As New ExcelPackage()
                        ' Agregar una nueva hoja de trabajo al archivo
                        Dim worksheet = package.Workbook.Worksheets.Add("Informe Dos")

                        ' Obtener los datos del DataSet
                        Dim informeUnoTable As DataTable = myDataSet.Tables("informe_dos")

                        ' Verificar si la tabla contiene datos
                        If informeUnoTable IsNot Nothing AndAlso informeUnoTable.Rows.Count > 0 Then
                            ' Definir el encabezado de la hoja de trabajo
                            For i As Integer = 0 To informeUnoTable.Columns.Count - 1
                                worksheet.Cells(1, i + 1).Value = informeUnoTable.Columns(i).ColumnName
                            Next

                            Dim ultima_fila As Integer = 0

                            For i As Integer = 0 To informeUnoTable.Rows.Count - 1
                                For j As Integer = 0 To informeUnoTable.Columns.Count - 1
                                    ultima_fila = i + 2
                                    worksheet.Cells(i + 2, j + 1).Value = informeUnoTable.Rows(i)(j)

                                    If IsNumeric(informeUnoTable.Rows(i)(j)) Then
                                        worksheet.Cells(i + 2, j + 1).Style.Numberformat.Format = "0.00"
                                    End If
                                Next
                            Next

                            Try
                                InsertCmd = "		select SUM(cast(informe_dos.total_tb as float)) as 'Suma', " &
                                            "			   AVG(cast(informe_dos.total_tb as float)) as 'Prom.' " &
                                            "		  from informe_dos "

                                MyCommand = New SqlClient.SqlCommand(InsertCmd, MyConnection)

                                MyCommand.Transaction = myTrans
                                MyCommand.CommandTimeout = 5000
                                MyCommand.ExecuteScalar()

                                Dim myDataSet2 As New Data.DataSet()
                                Dim myDataAdapter2 As New System.Data.SqlClient.SqlDataAdapter(MyCommand)
                                myDataAdapter2.Fill(myDataSet2, "informe_dos")

                                If myDataSet2.Tables("informe_dos").Rows.Count > 0 Then
                                    If Not IsDBNull(myDataSet2.Tables("informe_dos").Rows(0)("Suma")) Then
                                        worksheet.Cells(ultima_fila + 3, 1).Value = "Suma"
                                        worksheet.Cells(ultima_fila + 3, 2).Value = myDataSet2.Tables("informe_dos").Rows(0)("Suma")
                                        worksheet.Cells(ultima_fila + 3, 2).Style.Numberformat.Format = "0.00"

                                        worksheet.Cells(ultima_fila + 4, 1).Value = "Prom."
                                        worksheet.Cells(ultima_fila + 4, 2).Value = myDataSet2.Tables("informe_dos").Rows(0)("Prom.")
                                        worksheet.Cells(ultima_fila + 4, 2).Style.Numberformat.Format = "0.00"
                                    End If
                                End If
                            Catch ex As Exception

                            End Try

                            ' Autoajustar el ancho de las columnas
                            worksheet.Cells.AutoFitColumns()

                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                            Response.ContentEncoding = System.Text.Encoding.UTF8
                            Response.AppendHeader("content-disposition", "attachment; filename=data_respaldada_por_cliente_" & DateTime.Now.ToString("ddMMyyyyHHmmss") & ".xlsx")
                            Response.BinaryWrite(package.GetAsByteArray())
                            Response.Flush()
                            Response.End()
                        Else
                            ' No hay datos en la tabla
                            ' Aquí puedes manejar el caso de que la tabla esté vacía
                        End If
                    End Using
                End If
            End If
        Catch ex As Exception

        End Try

        MyConnection.Close()
    End Sub

    Protected Sub cargar_informeTres(ByVal sender As Object, ByVal e As EventArgs)
        pnlinforme1.Visible = False
        pnlinforme2.Visible = False
        pnlinforme3.Visible = True
        pnlGridInformes.Visible = False

        Dim MyConnection As SqlClient.SqlConnection
        MyConnection = New SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conexion").ToString)
        MyConnection.Open()

        Dim myTrans As SqlClient.SqlTransaction
        Dim MyCommand As SqlClient.SqlCommand
        myTrans = MyConnection.BeginTransaction()

        Dim InsertCmd As String = ""
        Dim myDataSet As New Data.DataSet()

        Try
            InsertCmd = "		select informe_tres.client_name, " &
                        "			   informe_tres.count_job_id, " &
                        "			   informe_tres.size_gb " &
                        "		  from informe_tres " &
                        "	  order by informe_tres.client_name asc "

            MyCommand = New SqlClient.SqlCommand(InsertCmd, MyConnection)

            MyCommand.Transaction = myTrans
            MyCommand.CommandTimeout = 5000
            MyCommand.ExecuteScalar()

            Dim myDataAdapter As New System.Data.SqlClient.SqlDataAdapter(MyCommand)
            myDataAdapter.Fill(myDataSet, "informe_tres")
        Catch ex As Exception

        End Try

        MyConnection.Close()

        gridInformeTres.DataSource = myDataSet.Tables("informe_tres").DefaultView
        gridInformeTres.DataBind()
    End Sub

    Protected Sub gridInformeTres_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gridInformeUno.PageIndexChanging
        ' Establecer el nuevo índice de página
        gridInformeTres.PageIndex = e.NewPageIndex

        ' Volver a enlazar los datos al GridView
        cargar_informeTres(sender, e)
    End Sub

    Protected Sub ExportarCSVInformeTres(ByVal sender As Object, ByVal e As EventArgs)
        Dim MyConnection As SqlClient.SqlConnection
        MyConnection = New SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conexion").ToString)
        MyConnection.Open()

        Dim myTrans As SqlClient.SqlTransaction
        Dim MyCommand As SqlClient.SqlCommand
        myTrans = MyConnection.BeginTransaction()

        Dim InsertCmd As String = ""
        Dim myDataSet As New Data.DataSet()

        Try
            InsertCmd = "		select informe_tres.client_name as 'Client Name', " &
                        "			   informe_tres.count_job_id as 'Count Job ID', " &
                        "			   informe_tres.size_gb as 'Size GB' " &
                        "		  from informe_tres " &
                        "	  order by informe_tres.client_name asc "

            MyCommand = New SqlClient.SqlCommand(InsertCmd, MyConnection)

            MyCommand.Transaction = myTrans
            MyCommand.CommandTimeout = 5000
            MyCommand.ExecuteScalar()

            Dim myDataAdapter As New System.Data.SqlClient.SqlDataAdapter(MyCommand)
            myDataAdapter.Fill(myDataSet, "informe_tres")

            If myDataSet.Tables("informe_tres").Rows.Count > 0 Then
                If Not IsDBNull(myDataSet.Tables("informe_tres").Rows(0)("Client Name")) Then
                    Using package As New ExcelPackage()
                        ' Agregar una nueva hoja de trabajo al archivo
                        Dim worksheet = package.Workbook.Worksheets.Add("Informe Tres")

                        ' Obtener los datos del DataSet
                        Dim informeUnoTable As DataTable = myDataSet.Tables("informe_tres")

                        ' Verificar si la tabla contiene datos
                        If informeUnoTable IsNot Nothing AndAlso informeUnoTable.Rows.Count > 0 Then
                            ' Definir el encabezado de la hoja de trabajo
                            For i As Integer = 0 To informeUnoTable.Columns.Count - 1
                                worksheet.Cells(1, i + 1).Value = informeUnoTable.Columns(i).ColumnName
                            Next

                            ' Llenar los datos en la hoja de trabajo
                            For i As Integer = 0 To informeUnoTable.Rows.Count - 1
                                For j As Integer = 0 To informeUnoTable.Columns.Count - 1
                                    worksheet.Cells(i + 2, j + 1).Value = informeUnoTable.Rows(i)(j)
                                Next
                            Next

                            ' Autoajustar el ancho de las columnas
                            worksheet.Cells.AutoFitColumns()

                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                            Response.ContentEncoding = System.Text.Encoding.UTF8
                            Response.AppendHeader("content-disposition", "attachment; filename=restauraciones_" & DateTime.Now.ToString("ddMMyyyyHHmmss") & ".xlsx")
                            Response.BinaryWrite(package.GetAsByteArray())
                            Response.Flush()
                            Response.End()
                        Else
                            ' No hay datos en la tabla
                            ' Aquí puedes manejar el caso de que la tabla esté vacía
                        End If
                    End Using
                End If
            End If
        Catch ex As Exception
        End Try

        MyConnection.Close()

    End Sub
End Class

