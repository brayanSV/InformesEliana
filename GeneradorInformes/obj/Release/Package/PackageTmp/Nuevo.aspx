<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Nuevo.aspx.vb" Inherits="GeneradorInformes.Nuevo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .formulario {
            background-color: white;
            border: 1px solid #ddd;
            border-radius: 10px;
            padding: 20px;
            width: 500px;
            margin: 0 auto;
        }

        .load-file-max-width-none {
            max-width: none;
        }

        .container2-margin-top-15px {
            margin-top: 15px;
        }

        .container2-margin-top-5px {
            margin-top: 5px;
        }

        .center-button {
            text-align: center;
        }

        .file-upload-error {
            border: 1px solid red;
            color: red;
        }

        .text-error {
            color: red;
        }
    </style>

    <script>
        window.onload = function () {
            disableEnableFileUpload1();
            disableEnableFileUpload2();
            disableEnableFileUpload3();
            disableEnabledButton();
        };

        function disableEnableFileUpload1() {
            var checkbox = document.getElementById('<%= chkInforme1.ClientID %>');
                    var fileUpload = document.getElementById('<%= fileInforme1.ClientID %>');

            if (checkbox.checked) {
                fileUpload.disabled = false;
            } else {
                fileUpload.disabled = true;
            }

            disableEnabledButton();
        }

        function disableEnableFileUpload2() {
            var checkbox = document.getElementById('<%= chkInforme2.ClientID %>');
                    var fileUpload = document.getElementById('<%= fileInforme2.ClientID %>');

            if (checkbox.checked) {
                fileUpload.disabled = false;
            } else {
                fileUpload.disabled = true;
            }

            disableEnabledButton();
        }

        function disableEnableFileUpload3() {
            var checkbox = document.getElementById('<%= chkInforme3.ClientID %>');
                    var fileUpload = document.getElementById('<%= fileInforme3.ClientID %>');

            if (checkbox.checked) {
                fileUpload.disabled = false;
            } else {
                fileUpload.disabled = true;
            }

            disableEnabledButton();
        }

        function validateCSV(input, numinf) {
            var file = input.files[0];

            if (file) {
                var reader = new FileReader();
                reader.readAsText(file);
                reader.onload = function (event) {
                    var isValid = false;
                    var csv = event.target.result;
                    var lines = csv.split(/\r\n|\n/);

                    // Define los criterios de validación para cada valor de numinf
                    var criteria = [
                        { numinf: 1, keywords: ["Job ID", "Throughput (KB/sec)"] },
                        { numinf: 2, keywords: ["Date/Time", "Trend"] },
                        { numinf: 3, keywords: ["Job ID", "Count of Files"] }
                    ];

                    // Itera sobre los criterios de validación
                    for (var i = 0; i < criteria.length; i++) {
                        var criterion = criteria[i];
                        // Si numinf coincide con el criterio actual y todas las palabras clave están presentes en alguna línea, establece isValid en true y sale del bucle
                        if (numinf === criterion.numinf && criterion.keywords.every(keyword => lines.some(line => line.includes(keyword)))) {
                            isValid = true;
                            break;
                        }
                    }

                    switch (numinf) {
                        case 1:
                            document.getElementById('<%= lblfile1IsValid.ClientID %>').value = isValid;
                                    break;
                                case 2:
                                    document.getElementById('<%= lblfile2IsValid.ClientID %>').value = isValid;
                                    break;
                                case 3:
                                    document.getElementById('<%= lblfile3IsValid.ClientID %>').value = isValid;
                            break;
                    }

                    if (!isValid) {
                        input.classList.add("file-upload-error");
                        input.blur();

                        switch (numinf) {
                            case 1:
                                showAlert("Documento invalido, la primera columna debe tener el nombre 'Job ID' y la ultima columna debe ser 'Throughput (KB/sec)'");
                                break;
                            case 2:
                                showAlert("Documento invalido, la primera columna debe tener el nombre 'Date/Time' y la ultima columna debe ser 'Trend'");
                                break;
                            case 3:
                                showAlert("Documento invalido, la primera columna debe tener el nombre 'Job ID' y la ultima columna debe ser 'Count of Files'");
                                break;
                        }
                    } else {
                        input.classList.remove("file-upload-error");
                    }

                    disableEnabledButton();
                };
            }
        }

        function disableEnabledButton() {
            var isValid = false
            var checkbox1 = document.getElementById('<%= chkInforme1.ClientID %>');
            var fileUpload1 = document.getElementById('<%= fileInforme1.ClientID %>');
            var fileUpload1IsValid = (document.getElementById('<%= lblfile1IsValid.ClientID %>').value === "true");

            var checkbox2 = document.getElementById('<%= chkInforme2.ClientID %>');
            var fileUpload2 = document.getElementById('<%= fileInforme2.ClientID %>');
            var fileUpload2IsValid = (document.getElementById('<%= lblfile2IsValid.ClientID %>').value === "true");

            var checkbox3 = document.getElementById('<%= chkInforme3.ClientID %>');
            var fileUpload3 = document.getElementById('<%= fileInforme3.ClientID %>');
            var fileUpload3IsValid = (document.getElementById('<%= lblfile3IsValid.ClientID %>').value === "true");

            var boton = document.getElementById('<%= btnEnviar.ClientID %>');

            var isValid = (
                ((checkbox1.checked && fileUpload1.value !== "" && fileUpload1IsValid) && (checkbox2.checked && fileUpload2.value !== "" && fileUpload2IsValid) && (checkbox3.checked && fileUpload3.value !== "" && fileUpload3IsValid)) ||
                ((checkbox1.checked && fileUpload1.value !== "" && fileUpload1IsValid) && (checkbox2.checked && fileUpload2.value !== "" && fileUpload2IsValid) && (!checkbox3.checked)) ||
                ((checkbox1.checked && fileUpload1.value !== "" && fileUpload1IsValid) && (!checkbox2.checked) && (checkbox3.checked && fileUpload3.value !== "" && fileUpload3IsValid)) ||
                ((!checkbox1.checked) && (checkbox2.checked && fileUpload2.value !== "" && fileUpload2IsValid) && (checkbox3.checked && fileUpload3.value !== "" && fileUpload3IsValid)) ||
                ((checkbox1.checked && fileUpload1.value !== "" && fileUpload1IsValid) && (!checkbox2.checked) && (!checkbox3.checked)) ||
                ((!checkbox1.checked) && (checkbox2.checked && fileUpload2.value !== "" && fileUpload2IsValid) && (!checkbox3.checked)) ||
                ((!checkbox1.checked) && (!checkbox2.checked) && (checkbox3.checked && fileUpload3.value !== "" && fileUpload3IsValid))
            );

            boton.disabled = !isValid;
        }

        function showAlert(msg) {
            alert(msg);
        }
    </script>

    <asp:HiddenField runat="server" id="lblfile1IsValid" Value="false"/>
    <asp:HiddenField runat="server" id="lblfile2IsValid" Value="false"/>
    <asp:HiddenField runat="server" id="lblfile3IsValid" Value="false"/>
    <asp:Label runat="server" ID="lblprueba"></asp:Label>

    <div class="formulario">
        <asp:UpdatePanel ID="uPanel1" runat="server">
            <ContentTemplate>
                <h5>Generar Nuevos Informes</h5>
                <div class="form-group">
                    <label for="chkInforme1">
                        <asp:CheckBox ID="chkInforme1" runat="server" onclick="disableEnableFileUpload1();" />
                        Informe - Detailed Job Status
                    </label>
                    <div class="container2-margin-top-5px">
                        <asp:FileUpload ID="fileInforme1" runat="server" CssClass="form-control load-file-max-width-none" Accept=".csv" onchange="validateCSV(this, 1)"/>
                    </div>
                </div>
                <div class="form-group container2-margin-top-15px">
                    <label for="chkInforme2">
                        <asp:CheckBox ID="chkInforme2" runat="server" onclick="disableEnableFileUpload2();"  />
                        Informe - Data respaldada por cliente
                    </label>
                    <div class="container2-margin-top-5px">
                        <asp:FileUpload ID="fileInforme2" runat="server" CssClass="form-control load-file-max-width-none" Accept=".csv" onchange="validateCSV(this, 2)"/>
                    </div>
                </div>
                <div class="form-group container2-margin-top-15px">
                    <label for="chkInforme3">
                        <asp:CheckBox ID="chkInforme3" runat="server" onclick="disableEnableFileUpload3();" />
                        Informe - Restauraciones
                        </label>
                    <div class="container2-margin-top-5px">
                        <asp:FileUpload ID="fileInforme3" runat="server" CssClass="form-control load-file-max-width-none" Accept=".csv" onchange="validateCSV(this, 3)"/>
                    </div>
                </div>

                <div id="contentError" runat="server" class="center-button container2-margin-top-5px">
                    <asp:Label runat="server" ID="lblmensaje" CssClass="text-error" />
                </div>

                <div class="center-button container2-margin-top-5p">
                    <asp:Button Text="Generar Informe" ID="btnEnviar" class="btn btn-primary container2-margin-top-15px" runat="server" OnClick="btnEnviar_Click" />
                </div>
                
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnEnviar" />
            </Triggers>
        </asp:UpdatePanel>            
    </div>
</asp:Content>
