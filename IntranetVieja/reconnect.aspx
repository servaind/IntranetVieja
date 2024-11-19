<%@ Page Language="C#" AutoEventWireup="true" CodeFile="reconnect.aspx.cs" Inherits="reconnect" %>
<%@ OutputCache Location="None" VaryByParam="none" %>

<html>
<head>
    <script language="javascript" type="text/javascript">
        setInterval('Actualizar()', 60000);

        function Actualizar() {
            window.location.reload();
        }
    </script>
</head>
</html>
