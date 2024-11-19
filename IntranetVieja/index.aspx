<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">

<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        Animar();

        setInterval("Animar()", 3000);
    });

    function Animar() {

        $('#logoIntranetMedio').fadeTo("slow", 0.3, function () {
            $('#logoIntranetMedio').fadeTo("slow", 1.0);
        });
    }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlacePage" Runat="Server">

    <div id="version">
        <img id="logoIntranetMedio" src="/images/logo_intranet_medio.png" />
        <br /><br />
        Sistemas Internos - versión <%=Configuracion.Version %>.
        <br />© 2019 Servaind S.A.
    </div>

</asp:Content>

