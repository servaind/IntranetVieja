<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InformacionObraImprimir.aspx.cs" Inherits="general_InformacionObraImprimir" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Servaind ::: Intranet</title>
    <link href="/css/impresion.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="bordeCompleto" style="width:750px; height:890px" id="marcoGeneral" runat="server">
            <div class="logo">
                <img src="../images/logo_servaind_nc.png" alt="Servaind" />
            </div>
            <div class="titulo_container">
                <div class="titulo">
                    Información Interna de Obra
                </div>
            </div>
            <div class="celda_100">
                <span class="item item_250">Fecha:</span>
                <span class="item" id="lblFecha" runat="server">01/01/2000</span>
            </div>
            <div class="celda_100">
                <span class="item item_250">Revisión:</span>
                <span class="item" id="lblRevision" runat="server">1</span>
            </div>
            <div class="celda_100">
                <span class="item item_250">Informa</span>
                <span class="item" id="lblInforma" runat="server">Martín E. Durán</span>
            </div>
            <div class="celda_100">
                <span class="item item_250">Gerente de proyecto:</span>
                <span class="item" id="lblGerenteProyecto" runat="server">Martín E. Durán</span>
            </div>
            <div class="celda_100 bordeIntInf bordeIntSup">
                <span class="item item_250">Tipo de trabajo:</span>
                <ul class="item chkList">
                    <li id="chkTipoObra" runat="server">Obra</li>
                    <li id="chkTipoMantenimiento" style="width:120px" runat="server">Mantenimiento</li>
                </ul>
            </div>
            <div class="celda_100">
                <span class="item item_250">Responsable de obra:</span>
                <span class="item" id="lblResponsableObra" runat="server">Martín E. Durán</span>
            </div>
            <div class="celda_100">
                <span class="item item_250">Cotización / Imputación:</span>
                <span class="item" id="lblImputacion" runat="server">1</span>
            </div>
            <div class="celda_100">
                <span class="item item_250">Cliente</span>
                <span class="item" id="lblCliente" runat="server">01/01/2000</span>
            </div>
            <div class="celda_100">
                <span class="item item_250">Orden de Compra:</span>
                <span class="item" id="lblOrdenCompra" runat="server">01/01/2000</span>
            </div>
            <div class="celda_100">
                <span class="item item_250">Fecha de Entrega según OC:</span>
                <span class="item" id="lblFechaEntrega" runat="server">01/01/2000</span>
            </div>
            <div class="celda_100">
                <span class="item item_250">Subcontratistas:</span>
                <ul class="item chkList">
                    <li id="chkSubcontratSi" runat="server">Si</li>
                    <li id="chkSubcontratNo" runat="server">No</li>
                </ul>
            </div>
            <div class="celda_100 bordeIntInf">
                <span class="item item_250 nivel_1">Empresa:</span>
                <span class="item" id="lblSubcontratEmpresa" runat="server">01/01/2000</span>
            </div>
            <div class="celda_100">
                <span class="item item_250">Predio de terceros:</span>
                <ul class="item chkList">
                    <li id="chkPredioTercSi" runat="server">Si</li>
                    <li id="chkPredioTercNo" runat="server">No</li>
                </ul>
            </div>
            <div class="celda_100">
                <span class="item item_250 nivel_1">Empresa:</span>
                <span class="item" id="lblPredioTercEmpresa" runat="server">01/01/2000</span>
            </div>
            <div class="celda_100">
                <span class="item item_250">Ubicación / Dirección de obra:</span>
                <span class="item" id="lblUbicacion" runat="server">1</span>
            </div>
            <div class="celda_100">
                <span class="item item_250">Provincia:</span>
                <span class="item" id="lblProvincia" runat="server">1</span>
            </div>
            <div class="celda_100">
                <span class="item item_250">Responsable Técnico Cliente:</span>
                <span class="item" id="lblRespTecCli" runat="server">1</span>
            </div>
            <div class="celda_100">
                <span class="item item_250 nivel_1">Datos de contacto:</span>
                <span class="item item_25">Tel:</span>
                <span class="item item_135" id="lblRespTecCliTel" runat="server">5491143632311</span>
                <span class="item item_50">Email:</span>
                <span class="item" id="lblRespTecCliEmail" runat="server">martin.duran@servaind.com</span>
            </div>
            <div class="celda_100">
                <span class="item item_250">Responsable Seguridad Cliente:</span>
                <span class="item" id="lblRespSegCli" runat="server">1</span>
            </div>
            <div class="celda_100">
                <span class="item item_250 nivel_1">Datos de contacto:</span>
                <span class="item item_25">Tel:</span>
                <span class="item item_135" id="lblRespSegCliTel" runat="server">5491143632311</span>
                <span class="item item_50">Email:</span>
                <span class="item" id="lblRespSegCliEmail" runat="server">martin.duran@servaind.com</span>
            </div>
            <div class="celda_100">
                <span class="item item_250">Contacto Administrativo Cliente:</span>
                <span class="item" id="lblContAdminCliente" runat="server">1</span>
            </div>
            <div class="celda_100">
                <span class="item item_250 nivel_1">Datos de contacto:</span>
                <span class="item item_25">Tel:</span>
                <span class="item item_135" id="lblContAdminClienteTel" runat="server">5491143632311</span>
                <span class="item item_50">Email:</span>
                <span class="item" id="lblContAdminClienteEmail" runat="server">martin.duran@servaind.com</span>
            </div>
            <div class="celda_100">
                <span class="item item_250">Fecha Est. Inicio Obra en Sitio:</span>
                <span class="item" id="lblFechaEstimada" runat="server">01/01/2000</span>
            </div>
            <div class="celda_100">
                <span class="item item_250">Duración Est. Obra en Sitio:</span>
                <span class="item item_135" id="lblDuracion" runat="server">1 mes</span>
                <span class="item item_25"></span>
                <span class="item item_150">Fecha de Finalización:</span>
                <span class="item" id="lblFechaFinalizacion" runat="server">01/01/2000</span>
            </div>
            <div class="celda_100">
                <span class="item item_250" style="height:40px">Descripción General de Tareas:</span>
                <span class="item" id="lblDescripcionTareas" style="width:450px" runat="server">das jkdasj dkajskl djaskldj akljdklasj dkasljdkljaskl djkaslj dkasljdklasjdklasjdklasjdkla sjkldjsakldjkl asjdklasjdkl jaskldjaskljdkl sajdklasjdklasjk ldasjlkdjlskal</span>
            </div>
            <div class="celda_100">
                <span class="item item_250" style="height:40px">Objetivo del Proyecto:</span>
                <span class="item" id="lblObjetivoProyecto" style="width:450px" runat="server">das jkdasj dkajskl djaskldj akljdklasj dkasljdkljaskl djkaslj dkasljdklasjdklasjdklasjdkla sjkldjsakldjkl asjdklasjdkl jaskldjaskljdkl sajdklasjdklasjk ldasjlkdjlskal</span>
            </div>
            <div class="celda_100 bordeIntInf bordeIntSup">
                <span class="item item_50p center">Personal de Mantenimiento:</span>
                <span class="item item_50p center">Personal de Obras:</span>
            </div>
            <div class="celda_50">
                <span class="item item_100p" id="lblPersonalMant" runat="server">- Mantenimiento</span>
            </div>
            <div class="celda_50">
                <span class="item item_100p" id="lblPersonalObras" runat="server">- Obras</span>
            </div>
            <div class="celda_100 bordeIntInf bordeIntSup">
                <span class="item item_100p center">Vehículos:</span>
            </div>
            <div class="celda_100">
                <span class="item item_100p" id="lblVehiculos" runat="server">- Vehículos</span>
            <div  class="celda_100">
                <div style="float:left;">FA-054 Rev.01 </div>
                <div style="float:right;">26/09/2012 </div>
            </div>
            </div>
           

        </div>
    </form>

    <script language="javascript" type="text/javascript">
        window.print();
    </script>
</body>
</html>
