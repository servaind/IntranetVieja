<%@ Page Language="C#" AutoEventWireup="true" CodeFile="propCambioUpload.aspx.cs" Inherits="calidad_propCambioUpload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <script language="javascript" src="/js/jquery.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $('#txtArchivo').change(function () {
                parent.FileSelected($(this).val());
            });
        });

        function Send(sectorId, responsableId, cambioPropuesto, urgenciaId) {
            $('#txtSectorId').val(sectorId);
            $('#txtResponsableId').val(responsableId);
            $('#txtCambioPropuesto').val(cambioPropuesto);
            $('#txtUrgenciaId').val(urgenciaId);

            $('#btnSubirArchivo').click();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:TextBox ID="txtSectorId" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtResponsableId" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtCambioPropuesto" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtUrgenciaId" runat="server"></asp:TextBox>
        <asp:FileUpload ID="txtArchivo" runat="server" />
        <asp:Button ID="btnSubirArchivo" runat="server" Text="Submit" OnClick="SubirArchivo" />
    </form>
</body>
</html>