<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="ssm.aspx.cs" Inherits="calidad_ssm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">

<!--[if lt IE 9]>     
<script src="/js/IE9.js" type="text/javascript"></script>
<![endif]-->

<script language="javascript" type="text/javascript">
    var current_page;
    var currentSitio;

    $(document).ready(function () {
        <% if(PuedeAdministrador) { %>
        $('#btnActualizarEstadoAceptar').click(function () {
            var estado = $('#contentPlacePage_cbActualizarEstado');
            var result = true;

            result &= TieneDatos(estado, 'input_wrapper', 'input_wrapper_selectbox_error');

            if (result) {
                custom_dialog.close();

                MostrarLoading();

                ConsultaAjax.url = 'ssm.aspx/ActualizarEstadoSitio';
                ConsultaAjax.data = '{ "valor": "' + currentSitio + '", "idEstado":"' + estado.val() + '", "pagina":"' + current_page + '" }';
                ConsultaAjax.AjaxSuccess = function (msg) {
                    GetPanelControl(current_page);
                };
                ConsultaAjax.AjaxError = function (msg) {
                    ErrorMsg(msg);
                };

                ConsultaAjax.Ejecutar();
            }
        });
        $('#btnActualizarEstadoCancelar').click(function () {
            current_sitio = null;

            CerrarVentana();
        });
        <% } %>

        GetPanelControl(0);
    });

    <% if(PuedeAdministrador) { %>
    function DibujarAcciones() {
        $('.input_actions').html('<ul><li class="email" title="Enviar e-mail de recordatorio"></li></ul>');
    }

    var EnlazarEventos = function () {
        $('.input_actions ul li.email').click(function () {
            var idSitio = $(this).parents('.input_actions').attr('name');
            idSitio = idSitio.replace('act_', '');

            Mensaje('¿Desea enviar un e-mail a los responsables para recordarles la tarea?', 'warning', true, true, 'Cancelar', 'Enviar', 'custom_dialog.close()', 'EnviarRecordatorio(\'' + idSitio + '\')');
        });
        $('.icon_ssm').click(function () {
            var idSitio = $(this).attr('name');
            currentSitio = idSitio.replace('icon_', '');

            MostrarVentana('divActualizarEstado');
        });
    }
    <% } %>

    function GetPanelControl(pagina) {
        MostrarLoading();

        ConsultaAjax.url = 'ssm.aspx/GetPanelControl';
        ConsultaAjax.data = '{ "pagina":"' + pagina + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            current_page = pagina;

            $('#listado').html();
            var cant = msg.d.length;

            if (cant > 0) {
                var encabezado = [];
                var cantEnc = msg.d[0].length;
                for (var j = 0; j < cantEnc; j++) {
                    var filaE = '<td class="border_' + (j == 0 ? 'left' : (j == (j - 1) ? 'right' : 'middle')) +
                        '" style="width:' + (j == 0 ? 200 : 110) + 'px">' + msg.d[0][j] + '</td>';
                    encabezado.push(filaE);
                }
                $('#tablaPC').attr('style', 'width=\"' + ((cantEnc - 1) * 110 + 200) + '\"');
                $('#tdPaginas').attr('colspan', cantEnc);
                $('#encabezado').html(encabezado.join(''));

                var filas = [];
                for (var i = 1; i < cant; i++) {
                    var celdas = msg.d[i].length;
                    var fila = '<tr class="fila-color">';
                    fila += '<td class="align-left">' + msg.d[i][0] + '</td>';
                    for (var k = 1; k < celdas; k+=3) {
                        fila += GetCelda(msg.d[i], k);
                    }
                    fila += '</tr>';
                    filas.push(fila);
                }
                $('#listado').html(filas.join(''));
            }
            else {
                $('#listado').html('<tr><td colspan="8" class="align-center">No hay datos disponibles.</td></tr>');
            }

            <% if(PuedeAdministrador) { %>
            DibujarAcciones();
            EnlazarEventos();
            <% } %>
            EnlazarEventosMaster();
            
            CerrarVentana();
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function GetCelda(celda, index) {
        var result;

        if (celda[index + 1] != '<%=((int)EstadosSitio.NoCumplido).ToString() %>') {
            result = '<td title="' + celda[index + 2] + '" class="align-center hand"><img class="icon_ssm" name="icon_' + celda[index] + '" src="/images/icons/icon_' + (celda[index + 1] == '<%=((int)EstadosSitio.Cumplido).ToString() %>' ? 'ok' : 'na') + '.png" /></td>';
        }
        else {
            <% if(PuedeAdministrador) { %>
            result = '<td title="' + celda[index + 2] + '" class="align-center hand"><div class="hasActions"><img class="icon_ssm" name="icon_' + celda[index] + '" src="/images/icons/icon_error_2.png" /><div class="input_actions" name="act_' + celda[index] + '"></div></div></td>';
            <% } else { %>
            result = '<td title="' + celda[index + 2] + '" class="align-center hand"><img class="icon_ssm" name="icon_' + celda[index] + '" src="/images/icons/icon_error_2.png" /></td>';
            <% } %>
        }

        return result;
    }

    <% if(PuedeAdministrador) { %>
    function EnviarRecordatorio(idSitio) {
        custom_dialog.close();

        MostrarLoading();

        ConsultaAjax.url = 'ssm.aspx/EnviarEmailRecordatorio';
        ConsultaAjax.data = '{ "valor": "' + idSitio + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            Mensaje(msg.d, 'success', true, false, 'Aceptar', '', 'custom_dialog.close()', '');
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }
    <% } %>
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlacePage" Runat="Server">

<div class="page-title">
    <h1>SGI -> Sistema de Seguimiento Multisitio</h1>
</div>

<div class="full-width scroll-horizontal">
    <table class="tbl listado" id="tablePC" cellspacing="0">
        <thead>
            <tr id="encabezado"class="lineaSimple">
                
            </tr>
        </thead>
        <tbody id="listado">
            <tr>
                <td></td>
            </tr>
        </tbody>
        <tfoot>
            <tr>
                <td class="align-center" id="tdPaginas">
                    <a href="#" onclick="GetPanelControl(current_page - 1)">«Anterior</a>
                    |
                    <a href="#" onclick="GetPanelControl(0)">Mes actual</a>
                    |
                    <a href="#" onclick="GetPanelControl(current_page + 1)">Siguiente»</a>
                </td>
            </tr>
        </tfoot>
    </table>
</div>

<% if (PuedeAdministrador)
   { %>
<div class="dialog_wrapper" style="width:500px" id="divActualizarEstado">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3>Actualizar estado</h3></div></div></div>
    </div>

    <div class="dialog_content">
        <p>Seleccione el nuevo estado para el ítem seleccionado:</p>
        <br />
        <ul class="middle_form">
            <li class="form_floated_item form_floated_item_100">
                <div class="input_wrapper input_wrapper_selectbox">
                    <div class="cap">
                        <span id="lblActualizarEstado"></span>
                        <select id="cbActualizarEstado" runat="server">
                        </select>
                    </div>
                </div>
            </li>
        </ul>
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li><div class="btn primary_action_button_small button_100" id="btnActualizarEstadoAceptar"><div class="cap"><span>Aceptar</span></div></div></li>
            <li><div class="btn secondary_action_button_small button_100" id="btnActualizarEstadoCancelar"><div class="cap"><span>Cancelar</span></div></div></li>
        </ul>
    </div>
</div>
<% } %>

</asp:Content>

