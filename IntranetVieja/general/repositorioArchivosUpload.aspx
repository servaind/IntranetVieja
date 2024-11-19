<%@ Page Language="C#" AutoEventWireup="true" CodeFile="repositorioArchivosUpload.aspx.cs" Inherits="general_repositorioArchivosUpload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <script language="javascript" src="/js/jquery.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function(){
            $('#txtArchivo').change(function(){
                parent.SetFile($(this).val());
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:FileUpload ID="txtArchivo" runat="server" ClientIDMode="Static" />
        <asp:Button ID="btnSubirArchivo" ClientIDMode="Static" runat="server" Text="Submit" OnClick="SubirArchivo" />
    </form>
</body>
</html>
