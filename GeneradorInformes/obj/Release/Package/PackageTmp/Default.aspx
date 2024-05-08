<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="GeneradorInformes._Default" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>   
        /* Estilo para el encabezado de la tabla */
        .gridInformes th {
            background-color: #007bff; /* Azul */
            color: white; /* Texto blanco */
            font-weight: bold; /* Negrita */
        }

        /* Estilo para las filas pares de la tabla */
        .gridInformes tr:nth-child(even) {
            background-color: #f2f2f2; /* Gris claro */
        }

        /* Estilo para las filas impares de la tabla */
        .gridInformes tr:nth-child(odd) {
            background-color: #ffffff; /* Blanco */
        }

        /* Estilo para el borde de la tabla */
        .gridInformes {
            border-collapse: collapse;
            border: 1px solid #dddddd; /* Borde gris */
            margin: auto;
        }

        /* Estilo para las celdas de la tabla */
        .gridInformes td, .gridInformes th {
            border: 1px solid #dddddd; /* Borde gris */
            padding: 8px; /* Espaciado interno */
        }

        .green-button {
            background-color: #4CAF50;
            border: none;
            color: white;
            padding: 15px 32px;
            text-align: center;
            text-decoration: none;
            display: inline-block;
            font-size: 16px;
            cursor: pointer;
            width: 100%;
            transition: background-color 0.3s;
            max-width: none;
        }

        .green-button:hover {
            background-color: #45a049;
        }
    </style>      
        
    <asp:Panel runat="server" ID="pnlGridInformes">
        <asp:Table runat="server" CellPadding="0" CellSpacing="0" Width="100%">
            <asp:TableRow runat="server" VerticalAlign="Middle">
                <asp:TableCell runat="server" HorizontalAlign="Center">
                    <asp:Table runat="server" CellPadding="0" CellSpacing="0">
                        <asp:TableRow runat="server" VerticalAlign="Middle">
                            <asp:TableCell runat="server">
                                
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server" Height="5px">
                            <asp:TableCell runat="server"></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server">
                            <asp:TableCell runat="server">
                                <asp:Table runat="server" CellPadding="0" CellSpacing="0">
                                    <asp:TableRow runat="server">
                                        <asp:TableCell runat="server">                                            
                                            <asp:gridview id="gridinformes" 
                                                autogeneratecolumns="False"
                                                emptydatatext="No data available." 
                                                allowpaging="False" 
                                                CssClass="gridInformes"
                                                runat="server"
                                                style="width:320px;">
                                                <Columns>
                                                    <asp:BoundField DataField="informe" HeaderText="Informes" />
                                                    <asp:TemplateField HeaderText="Acción">
                                                        <ItemTemplate>
                                                            <asp:LinkButton Text="seleccionar" runat="server" CommandArgument='<%# Eval("numinforme") %>' OnCommand="accion_grid" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>                
                                                </Columns>
                                            </asp:gridview>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>                
            </asp:TableRow>            
        </asp:Table>             
    </asp:Panel>

    <asp:Panel runat="server" ID="pnlinforme1">
        <asp:Table runat="server" CellPadding="0" CellSpacing="0" Width="100%">
            <asp:TableRow runat="server" VerticalAlign="Middle">
                <asp:TableCell runat="server" HorizontalAlign="Center">
                    <asp:Table runat="server" CellPadding="0" CellSpacing="0">                        
                        <asp:TableRow runat="server">
                            <asp:TableCell runat="server">
                                <asp:Table runat="server" CellPadding="0" CellSpacing="0">
                                    <asp:TableRow runat="server">
                                        <asp:TableCell runat="server">                                            
                                            <asp:gridview id="gridInformeUno" 
                                              autogeneratecolumns="False"
                                              emptydatatext="No data available." 
                                              allowpaging="True" 
                                              CssClass="gridInformes"
                                              OnPageIndexChanging="gridInformeUno_PageIndexChanging"
                                              runat="server"
                                              PageSize="8"
                                              style="width:320px;">
                                                <Columns>
                                                    <asp:BoundField DataField="job_type" HeaderText="Job Type" />
                                                    <asp:BoundField DataField="status_code" HeaderText="Status Code" />
                                                    <asp:BoundField DataField="cuantos_job_type" HeaderText="Total" />                
                                                </Columns>
                                            </asp:gridview>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server" Height="10px">
                            <asp:TableCell runat="server"></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server" VerticalAlign="Middle">
                            <asp:TableCell runat="server" HorizontalAlign="Center">
                                <asp:Button runat="server" Text="Descargar" class="green-button" OnClick="ExportarCSVInformeUno"  />                                
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>                
            </asp:TableRow>            
        </asp:Table>             
    </asp:Panel>

    <asp:Panel runat="server" ID="pnlinforme2">
        <asp:Table runat="server" CellPadding="0" CellSpacing="0" Width="100%">
            <asp:TableRow runat="server" VerticalAlign="Middle">
                <asp:TableCell runat="server" HorizontalAlign="Center">
                    <asp:Table runat="server" CellPadding="0" CellSpacing="0">
                        <asp:TableRow runat="server">
                            <asp:TableCell runat="server">
                                <asp:Table runat="server" CellPadding="0" CellSpacing="0">
                                    <asp:TableRow runat="server">
                                        <asp:TableCell runat="server">                                            
                                            <asp:gridview id="gridInformeDos" 
                                                autogeneratecolumns="False"
                                                emptydatatext="No data available." 
                                                allowpaging="True" 
                                                CssClass="gridInformes"
                                                OnPageIndexChanging="gridInformeDos_PageIndexChanging"
                                                runat="server"
                                                PageSize="8"
                                                style="width:320px;">
                                                <Columns>
                                                    <asp:BoundField DataField="date_time_text" HeaderText="Date Time" />
                                                    <asp:BoundField DataField="total_tb" HeaderText="Total TB" ItemStyle-HorizontalAlign="Right" />            
                                                </Columns>
                                            </asp:gridview>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server" Height="10px">
                            <asp:TableCell runat="server"></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server" VerticalAlign="Middle">
                            <asp:TableCell runat="server" HorizontalAlign="Center">
                                <asp:Button runat="server" Text="Descargar" class="green-button" OnClick="ExportarCSVInformeDos"  />                                
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>                
            </asp:TableRow>            
        </asp:Table>
    </asp:Panel>
    
    <asp:Panel runat="server" ID="pnlinforme3">
        <asp:Table runat="server" CellPadding="0" CellSpacing="0" Width="100%">
            <asp:TableRow runat="server" VerticalAlign="Middle">
                <asp:TableCell runat="server" HorizontalAlign="Center">
                    <asp:Table runat="server" CellPadding="0" CellSpacing="0">                                                
                        <asp:TableRow runat="server">
                            <asp:TableCell runat="server">
                                <asp:Table runat="server" CellPadding="0" CellSpacing="0">
                                    <asp:TableRow runat="server">
                                        <asp:TableCell runat="server">                                            
                                            <asp:gridview id="gridInformeTres" 
                                              autogeneratecolumns="False"
                                              emptydatatext="No data available." 
                                              allowpaging="True" 
                                              CssClass="gridInformes"
                                              OnPageIndexChanging="gridInformeTres_PageIndexChanging"
                                              runat="server"
                                              PageSize="8"
                                              style="width:320px;">
                                                <Columns>
                                                    <asp:BoundField DataField="client_name" HeaderText="Client Name" />
                                                    <asp:BoundField DataField="count_job_id" HeaderText="Count Job ID" />            
                                                    <asp:BoundField DataField="size_gb" HeaderText="Size GB" />            
                                                </Columns>
                                            </asp:gridview>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server" Height="10px">
                            <asp:TableCell runat="server"></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server" VerticalAlign="Middle">
                            <asp:TableCell runat="server" HorizontalAlign="Center">
                                <asp:Button runat="server" Text="Descargar" class="green-button" OnClick="ExportarCSVInformeTres"  />                                
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>                
            </asp:TableRow>            
        </asp:Table>
    </asp:Panel>
</asp:Content>
