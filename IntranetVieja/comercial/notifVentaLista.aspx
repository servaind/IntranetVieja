<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="notifVentaLista.aspx.cs" Inherits="comercial_notifVentaLista" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">
    <script language="javascript" type="text/javascript">
        var currentPage;

        $(document).ready(function () {
            $('input').change(function () {
                RefreshFilters();
            });
            $('select').change(function () {
                RefreshFilters();
            });

            $('#chkFiltroEstado').change();


            $('#btnExportar').click(function () {
                ExportarExcel();
           });
        });


        function ExportarExcel()
        {
            MostrarLoading();

            var numero = $('#txtFiltroID');
            var iNumero;
            var vendedor = $('#txtFiltroVendedor');
            var cliente = $('#txtFiltroCliente');
            var oc = $('#txtFiltroOC');
            var imputacion = $('#txtFiltroImputacion');
            var estado = $('#cbFiltroEstado');
            var iEstado;

            if (jQuery.trim(numero.val()).length == 0 || isNaN(numero.val())) {
                iNumero = '<%=Constantes.ValorInvalido %>';
                numero.val('');
            } else iNumero = parseInt(numero.val());

            iEstado = $('#chkFiltroEstado').is(':checked') ? estado.val() : '<%=Constantes.ValorInvalido %>';

            $.ajax({
                url: 'notifVentaLista.aspx/ExportarListado',
                data: JSON.stringify({ numero: iNumero, vendedor: vendedor.val(), cliente: cliente.val(), oc: oc.val(), imputacion: imputacion.val(), estado: iEstado }),
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                async: true,
                cache: false,
                success: function (msg) {
                    
                    location.href = msg.d;

                    CerrarVentana();
                },
                error: function (data, ajaxOptions, thrownError) {
                    ErrorMsg(MsgOperationError + '\nDetalle: ' + GetAjaxError(data));
                }
            });
        }

        function RefreshFilters() {
            if ($('#chkFiltroEstado').is(':checked')) {
                $('#cbFiltroEstado').parents('.input_wrapper').removeClass('input_wrapper_selectbox_disabled');
                $('#cbFiltroEstado').removeAttr('disabled');
            }
            else {
                $('#cbFiltroEstado').parents('.input_wrapper').addClass('input_wrapper_selectbox_disabled');
                $('#cbFiltroEstado').attr('disabled', 'disabled');
            }

            GetVentas(1);
        }

        function GetVentas(page) {
            MostrarLoading();

            var numero = $('#txtFiltroID');
            var iNumero;
            var vendedor = $('#txtFiltroVendedor');
            var cliente = $('#txtFiltroCliente');
            var oc = $('#txtFiltroOC');
            var imputacion = $('#txtFiltroImputacion');
            var estado = $('#cbFiltroEstado');
            var iEstado;

            if (jQuery.trim(numero.val()).length == 0 || isNaN(numero.val())) {
                iNumero = '<%=Constantes.ValorInvalido %>';
                numero.val('');
            } else iNumero = parseInt(numero.val());

            iEstado = $('#chkFiltroEstado').is(':checked') ? estado.val() : '<%=Constantes.ValorInvalido %>';

            $.ajax({
                url: 'notifVentaLista.aspx/GetVentas',
                data: JSON.stringify({ pagina: page, numero: iNumero, vendedor: vendedor.val(), cliente: cliente.val(), oc: oc.val(), imputacion: imputacion.val(), estado: iEstado }),
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                async: true,
                cache: false,
                success: function (msg) {
                    currentPage = page;

                    var cant = msg.d.Lista.length;

                    if (cant > 0) {
                        var filas = [];
                        var i = 0;
                        for (i; i < cant; i++) {
                            var item = msg.d.Lista[i];

                            var fila = '<tr class="fila-color" onclick="ViewVenta(\'' + item.ID + '\')">';
                            fila += '<td class="align-center">' + item.Numero + '</td>';
                            fila += '<td class="align-left">' + item.Vendedor + '</td>';
                            fila += '<td class="align-left" title="' + item.Cliente + '">' + ShortenText(item.Cliente, 22) + '</td>';
                            fila += '<td class="align-left">' + item.OC + '</td>';
                            fila += '<td class="align-left" title="' + item.Imputacion + '">' + ShortenText(item.Imputacion, 17) + '</td>';
                            fila += '<td class="align-center">' + item.FechaOC + '</td>';
                            fila += '<td class="align-center">' + GetCellEstado(item.Estado) + '</td>';
                            fila += '</tr>';
                            filas.push(fila);
                        }

                        $('#listado').html(filas.join(''));
                    }
                    else {
                        $('#listado').html('<tr><td colspan="7" class="align-center">No hay datos disponibles.</td></tr>');
                    }

                    DrawPager(msg.d.TotalPaginas);

                    CerrarVentana();
                },
                error: function (data, ajaxOptions, thrownError) {
                    ErrorMsg(MsgOperationError + '\nDetalle: ' + GetAjaxError(data));
                }
            });
        }
        
        function GetCellEstado(estado) {
            var result = '';

            result = '&nbsp;<div class="tag_element ' + (estado == 'abierta' ? 'gray' : 'green') + '"><span>' + estado + '</span></div>';

            return result;
        }

        function DrawPager(totalPaginas) {
            $('#tdPaginas').html();

            var cont = [];

            if (currentPage == 1) {
                cont.push('<span class="disabled">«Anterior</span>');
            }
            else {
                cont.push('<a href="#" onclick="GetVentas(' + (currentPage - 1) + ')">«Anterior</a>');
            }
            cont.push('|');
            cont.push('<a href="#" onclick="GetVentas(1)">Inicio</a>');
            cont.push('|');
            if (totalPaginas == 0 || currentPage == totalPaginas) {
                cont.push('<span class="disabled">Siguiente»</span>');
            }
            else {
                cont.push('<a href="#" onclick="GetVentas(' + (currentPage + 1) + ')">Siguiente»</a>');
            }

            $('#tdPaginas').html(cont.join(' '));
        }
        
        function ViewVenta(id) {
            location.href = 'notifVentaAdmin.aspx?p=' + id;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlacePage" Runat="Server">
    
<div class="page-title">
      <table style="width:100%;">
        <tr>
            <td style="padding-left:20px;"><h1>Listado de ventas</h1></td>
            <td style="text-align:right;padding-right:30px;">
                <i id="btnExportar" class="fas fa-file-excel" data-toggle="tooltip" title="Exportar a Excel"
                    style="font-size:20px; color:forestgreen; cursor:pointer;">

                </i>
            </td>

        </tr>
    </table>
</div>

<div class="full-width">
    <table class="tbl editable listado">
        <thead>
            <tr>
                <td class="border_middle" style="width:70px">Nº</td>
                <td class="border_middle" style="width:170px">Vendedor</td>
                <td class="border_middle" style="width:180px">Cliente</td>
                <td class="border_middle" style="width:170px">OC</td>
                <td class="border_middle">Imputación</td>
                <td class="border_middle" style="width:80px">Fecha</td>
                <td class="border_right" style="width:90px"><input id="chkFiltroEstado" type="checkbox" /> Estado</td>
            </tr>
            <tr class="filter_row">
                <td>
                    <div class="input_wrapper">
                        <div class="cap">
                            <input id="txtFiltroID" type="text"/>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="input_wrapper">
                        <div class="cap">
                            <input id="txtFiltroVendedor" type="text"/>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="input_wrapper">
                        <div class="cap">
                            <input id="txtFiltroCliente" type="text"/>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="input_wrapper">
                        <div class="cap">
                            <input id="txtFiltroOC" type="text"/>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="input_wrapper">
                        <div class="cap">
                            <input id="txtFiltroImputacion" type="text"/>
                        </div>
                    </div>
                </td>
                <td></td>
                <td>
                    <div class="input_wrapper input_wrapper_selectbox">
                        <div class="cap">
                            <span>&nbsp;</span>
                            <select id="cbFiltroEstado">
                                <% List<DataSourceItem> estados = NotifVentas.GetEstados();
                                   estados.ForEach(e =>
                                       {
                                           %>
                                           <option value="<%=e.ValueField %>"><%=e.TextField %></option>
                                           <%
                                       });
                                    %>
                            </select>
                        </div>
                    </div>
                </td>
            </tr>
        </thead>
        <tbody id="listado">
            <tr>
                <td colspan="7" class="align-center">No hay datos disponibles.</td>
            </tr>
        </tbody>
        <tfoot>
            <tr>
                <td colspan="2">
                    <% if (GPermisosPersonal.TieneAcceso(PermisosPersona.SNV_Vendedor)) { %>
                    <a href="notifVentaAdmin.aspx">Alta de venta</a>
                    <% } %>
                </td>
                <td colspan="5" class="align-center" id="tdPaginas">
                &nbsp;
                </td>
            </tr>
        </tfoot>
    </table>
</div>

</asp:Content>

