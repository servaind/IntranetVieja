<%@ Page Language="C#" AutoEventWireup="true" CodeFile="viajeImprimir.aspx.cs" Inherits="general_viajeImprimir" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Servaind ::: Intranet</title>
    <link href="/css/impresion.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="bordeCompleto" style="width:650px; height:850px">
            <div class="logo">
                <img src="../images/logo_servaind_sv.jpg" alt="Servaind" />
            </div>
            <div class="titulo_container">
                <div class="titulo">
                    Solicitud de viaje
                </div>
            </div>
            <div class="celda_100">
                <span class="item item_100">Vehículo:</span>
                <ul class="item chkList">
                    <li id="chkMoto" runat="server">Moto</li>
                    <li id="chkTaxi" runat="server">Taxi</li>
                    <li id="chkFlete" runat="server">Flete</li>
                    <li id="chkAuto" runat="server">Auto</li>
                </ul>
            </div>
            <div class="celda_100">
                <span class="item item_100">Importancia:</span>
                <span class="item">
                    <ul class="chkList">
                        <li id="chkBaja" runat="server">Baja</li>
                        <li id="chkNormal" runat="server">Normal</li>
                        <li id="chkAlta" runat="server">Alta</li>
                    </ul>
                </span>
            </div>
            <div class="titulo_container">
                <div class="titulo">
                    Origen
                </div>
            </div>
            <div class="celda_100">
                <span class="item item_150">Fecha de solicitud:</span>
                <span class="item" id="lblFechaSolicitud" runat="server">01/01/2000</span>
            </div>
            <div class="celda_100">
                <span class="item item_150">Motivo del viaje:</span>
                <span class="item" id="lblMotivo" runat="server">01/01/2000</span>
            </div>
            <div class="celda_100">
                <span class="item">Descripción del material / documentación:</span>
                <span class="item" id="lblDescripcion" runat="server">01/01/2000</span>
            </div>
            <div class="celda_100">
                <span class="item item_150">Origen:</span>
                <span class="item" id="lblOrigen" runat="server">01/01/2000</span>
            </div>
            <div class="celda_100">
                <span class="item item_150">Ruta:</span>
                <span class="item" id="lblRuta" runat="server">01/01/2000</span>
            </div>
            <div class="celda_100">
                <span class="item item_150">Fin de recorrido:</span>
                <span class="item" id="lblFinRecorrido" runat="server">01/01/2000</span>
            </div>
            <div class="celda_100">
                <div class="celda_50 celda_float_left">
                    <span class="item item_150">Fecha de cumplimiento:</span>
                    <span class="item" id="lblFechaCumplimiento" runat="server">01/01/2000</span>
                </div>
                <div class="celda_50 celda_float_left">
                    <span class="item item_200">Hora de cumplimiento:</span>
                    <span class="item" id="lblHoraCumplimiento" runat="server">01/01/2000</span>
                </div>
            </div>
            <div class="celda_100">
                <div class="celda_50 celda_float_left">
                    <span class="item item_150">Fecha límite:</span>
                    <span class="item" id="lblFechaLimite" runat="server">01/01/2000</span>
                </div>
                <div class="celda_50 celda_float_left">
                    <span class="item item_200">Hora límite de la prestación:</span>
                    <span class="item" id="lblHoraLimite" runat="server">01/01/2000</span>
                </div>
            </div>
            <div class="titulo_container">
                <div class="titulo">
                    Destino
                </div>
            </div>
            <div class="celda_100">
                <span class="item item_150">Destinatario:</span>
                <span class="item" id="lblDestinatario" runat="server">01/01/2000</span>
            </div>
            <div class="celda_100">
                <div class="celda_65 celda_float_left">
                    <span class="item item_150">Dirección:</span>
                    <span class="item" id="lblDireccion" runat="server">Alicia M. de Justo 740 Piso 3 Of 306 </span>
                </div>
                <div class="celda_35 celda_float_left">
                    <span class="item item_100">Localidad:</span>
                    <span class="item" id="lblLocalidad" runat="server">01/01/2000</span>
                </div>
            </div>
            <div class="celda_100">
                <div class="celda_65 celda_float_left">
                    <span class="item item_150">Contacto:</span>
                    <span class="item" id="lblContacto" runat="server">Alicia M. de Justo 740 Piso 3 Of 306 </span>
                </div>
                <div class="celda_35 celda_float_left">
                    <span class="item item_100">Teléfono:</span>
                    <span class="item" id="lblTelefono" runat="server">01/01/2000</span>
                </div>
            </div>
            <div class="celda_100">
                <span class="item item_150">Horario de atención:</span>
                <span class="item" id="lblHorarioAtencion" runat="server">01/01/2000</span>
            </div>
            <div class="celda_100">
                <span class="item">Documento de referencia:</span>
                <span class="item" id="lblDocumentoRef" runat="server">01/01/2000</span>
            </div>
            <div class="celda_100">
                <span class="item item_150">Debe retornar con:</span>
                <ul class="item chkList">
                    <li id="chkRetFac" runat="server">Factura</li>
                    <li id="chkRetRem" runat="server">Remito</li>
                    <li id="chkRetRec" runat="server">Recibo</li>
                </ul>
            </div>
            <div class="celda_100">
                <span class="item item_150"></span>
                <span class="item" id="lblRetOtro" runat="server">Otro: </span>
            </div>
            <div class="titulo_container">
                <div class="titulo">
                    Condiciones comerciales (si corresponde)
                </div>
            </div>
            <div class="celda_100">
                <div class="celda_65 celda_float_left">
                    <span class="item item_150">Condición comercial:</span>
                    <span class="item" id="lblCondicionComercial" runat="server">[]</span>
                </div>
                <div class="celda_35 celda_float_left">
                    <span class="item item_100">Imputación:</span>
                    <span class="item" id="lblImputación" runat="server">01/01/2000</span>
                </div>
            </div>
            <div class="celda_100">
                <div class="celda_65 celda_float_left">
                    <span class="item item_150"></span>
                    <span class="item"></span>
                </div>
                <div class="celda_35 celda_float_left">
                    <span class="item item_100">Importe ($):</span>
                    <span class="item" id="lblImporte" runat="server">01/01/2000</span>
                </div>
            </div>
            <div class="celda_100">
                <ul class="item chkList">
                    <li id="chkEfectivo" runat="server">Efectivo</li>
                    <li id="chkCheque" runat="server">Cheque</li>
                </ul>
                <span class="item" id="lblAlaOrden" runat="server"></span>
            </div>
            <div class="celda_100">
                <span class="item item_150">Observaciones:</span>
                <span class="item" id="lblObervaciones" runat="server">[]</span>
            </div>
            <div class="celda_100">
                <div class="celda_50 celda_float_left">
                    <span class="item item_150"></span>
                    <span class="item"></span>
                </div>
                <div class="celda_50 celda_float_left">
                    <span class="item item_100">Firma:</span>
                    <span class="item" id="lblSolicito" runat="server">01/01/2000</span>
                </div>
            </div>
        </div>
    </form>

    <script language="javascript" type="text/javascript">
        window.print();
        window.close();
    </script>
</body>
</html>
