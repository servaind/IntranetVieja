<%@ Page Language="C#" AutoEventWireup="true" CodeFile="vdmImprimir.aspx.cs" Inherits="stock_vdmImprimir" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Servaind ::: Intranet</title>
    <link href="/css/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div id="content" style="border:none;">
        <form id="form1" runat="server">
            <div class="form_place" style="width:850px">
                <h3>Vale de materiales <% if (this.VDM != null) { %>Nº <%=this.VDM.GetNumero() %>  <% } %></h3>
                
                <% if (this.VDM != null) { %>
                <ul class="middle_form" style="height:270px">
                    <li class="form_floated_item form_floated_item_half">
                        <label class="label">Fecha de solicitud</label>
                        <span id="lblFechaSolicitud" runat="server">[...]</span>
                    </li>
                    <li class="form_floated_item form_floated_item_half form_floated_item_right">
                        <label class="label">Emitida por</label>
                        <span id="lblEmitidaPor" runat="server">[...]</span>
                    </li>
                    <li class="form_floated_item form_floated_item_half">
                        <label class="label">Recibida por responsable de área</label>
                        <span id="lblRecibidaResponsable" runat="server">[...]</span>
                    </li>
                    <li class="form_floated_item form_floated_item_half form_floated_item_right">
                        <label class="label">Aprobada por responsable de área</label>
                        <span id="lblAprobadaResponsable" runat="server">[...]</span>
                    </li>
                    <li class="form_floated_item form_floated_item_half">
                        <label class="label">Recibida por depósito</label>
                        <span id="lblRecibidaDeposito" runat="server">[...]</span>
                    </li>
                    <li class="form_floated_item form_floated_item_half form_floated_item_right">
                        <label class="label">Entregada por depósito</label>
                        <span id="lblEntregada" runat="server">[...]</span>
                    </li>
                    <li class="form_floated_item form_floated_item_half">
                        <label class="label" for="txtDepartamento">Departamento</label>
                        <span id="lblDepartamento" runat="server">[...]</span>
                    </li>
                    <li class="form_floated_item form_floated_item_half form_floated_item_right">
                        <label class="label" for="txtSMTL">SMTL</label>
                        <span id="lblSMTL" runat="server">[...]</span>
                    </li>
                    <li class="form_floated_item form_floated_item_half">
                        <label class="label" for="txtCargo">Cargo</label>
                        <span id="lblCargo" runat="server">[...]</span>
                    </li>
                    <li class="form_floated_item form_floated_item_half form_floated_item_right">
                        <label class="label" for="txtDestino">Destino</label>
                        <span id="lblDestino" runat="server">[...]</span>
                    </li>
                </ul>

                <table class="tbl-impresion full-width">
                    <thead>
                        <tr>
                            <td style="width:120px">Código</td>
                            <td>Descripción</td>
                            <td style="width:40px">Un</td>
                            <td style="width:60px">Cant.</td>
                            <td style="width:60px">Imput.</td>
                            <td style="width:190px">Obra</td>
                        </tr>
                    </thead>
                    <tbody>
            <% 
                List<Imputacion> imputaciones = GImputaciones.GetImputaciones();
                foreach (ItemVDM item in this.VDM.Items)
                {
                    Imputacion imp = imputaciones.Find(i => i.ID == item.IDImputacion);
            %>
                        <tr>
                            <td class="align-center"><%= item.Articulo.Codigo %></td>
                            <td class="align-left"><%= item.Articulo.Descripcion %></td>
                            <td class="align-center"><%= item.Articulo.Un %></td>
                            <td class="align-right"><%= item.Cantidad.ToString("") %></td>
                            <td class="align-right"><%= imp != null ? imp.Numero.ToString() : "-" %></td>
                            <td class="align-left"><%= item.Obra %></td>
                        </tr>
            <% } %>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td colspan="6">&nbsp;</td>
                        </tr>
                    </tfoot>
                </table>
                <% } else { %>
                <h1>No se ha encontrado el vale de materiales.</h1>
                <% } %>
            </div>
        </form>
    </div>
</body>
<script language="javascript" type="text/javascript">
    window.print();
    window.close();
</script>
</html>
