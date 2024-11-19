<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="partesDiariosPC.aspx.cs" Inherits="general_partesDiariosPC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">

<!--[if lt IE 9]>     
<script src="/js/IE9.js" type="text/javascript"></script>
<![endif]-->

<script language="javascript" type="text/javascript">
    var current_page;
    var estadosPD = [];

    $(document).ready(function () {
        GetEstadosPD();

        GetPanelControl(0);
    });

    function DibujarAcciones() {
        $('.input_actions').html('<ul><li class="email" title="Enviar e-mail de recordatorio"></li></ul>');
    }

    var EnlazarEventos = function () {
        $('.input_actions ul li.email').click(function () {
            var idPersona = $(this).parents('.input_actions').attr('name');
            idPersona = idPersona.replace('act_', '');
            var value = '\'' + $(this).parents('.input_actions').attr('value') + '\'';

            Mensaje('¿Desea enviar un e-mail a la persona para recordarle la carga del parte diario?', 'warning', true, true, 'Cancelar', 'Enviar', 'custom_dialog.close()', 'EnviarRecordatorio(' + idPersona + ', ' + value + ')');
        });
    }

    function GetPanelControl(pagina) {
        MostrarLoading();

        ConsultaAjax.url = 'partesDiariosPC.aspx/GetPanelControl';
        ConsultaAjax.data = '{ "pagina":"' + pagina + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            current_page = pagina;

            $('#listado').html();
            var cant = msg.d.length;

            if (cant > 0) {
                var filas = [];
                for (var i = 0; i < cant; i++) {
                    var celdas = msg.d[i].length;
                    var fila = '<tr class="fila-color">';
                    fila += '<td class="align-left">' + msg.d[i][1] + '</td>';
                    for (var j = 2; j < celdas; j++) {
                        fila += GetCelda(msg.d[i][j]);
                    }
                    fila += '</tr>';
                    filas.push(fila);
                }

                $('#listado').html(filas.join(''));
            }
            else {
                $('#listado').html('<tr><td colspan="8" class="align-center">No hay partes diarios disponibles.</td></tr>');
            }

            DibujarEncabezado(pagina);

            DibujarAcciones();
            EnlazarEventosMaster();
            EnlazarEventos();

            CerrarVentana();
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function GetCelda(celda) {
        var result;

        if (celda[1] == 'Presente') {
            result = '<td title="' + celda[4] + ' - Presente" onclick="VerParteDiario(\'' + celda[5] + '\')" class="align-center hand"><img src="/images/partesDiarios/presente.png" /></td>';
        }
        else if (celda[1] == 'Licencia') {
            var estado = GetEstadoPD(celda[2]);
            result = '<td title="' + celda[4] + ' - ' + estado[1] + (celda[3] == '<%= ((int)EstadosLicencia.Confirmada).ToString() %>' ? ". Confirmada" : ". Esperando autorización") + '" onclick="VerParteDiario(\'' + celda[5] + '\')" class="align-center hand"><img src="' + (celda[3] == '<%= ((int)EstadosLicencia.Confirmada).ToString() %>' ? (estado[2]) : "/images/partesDiarios/esperando_autoriz.gif") + '" /></td>';
        }
        else {
            if (celda[0] == '<%= Constantes.Usuario.ID.ToString() %>') {
                result = '<td title="' + celda[1] + ' - No hay parte diario cargado" class="align-center hand" onclick="CargarParteDiario(\'' + celda[3] + '\')"><img src="/images/partesDiarios/ausente.png" /></div></div></td>';
            }
            else {
                result = '<td title="' + celda[1] + ' - No hay parte diario cargado" class="align-center"><div class="hasActions"><img src="/images/partesDiarios/ausente.png" /><div class="input_actions" value="' + celda[2] + '" name="act_' + celda[0] + '"></div></div></td>';
            }
        }

        return result;
    }

    function DibujarEncabezado(pagina) {
        ConsultaAjax.url = 'partesDiariosPC.aspx/GetEncabezado';
        ConsultaAjax.data = '{ "pagina":"' + pagina + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {

            $('#encabezado').html();
            var cant = msg.d.length;

            var filas = [];
            var fila = '<td class="border_left">&nbsp;</td>';
            for (var i = 0; i < cant; i++) {
                fila += '<td class="border_' + (i == (cant - 1) ? 'right' : 'middle') + '" style="width:90px">' + msg.d[i] + '</td>';
            }
            filas.push(fila);

            $('#encabezado').html(filas.join(''));

            CerrarVentana();
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function GetEstadosPD() {
        ConsultaAjax.url = 'partesDiariosPC.aspx/GetEstadosPD';
        ConsultaAjax.AjaxSuccess = function (msg) {
            var cant = msg.d.length;
            for (var i = 0; i < cant; i++) {
                estadosPD.push(msg.d[i]);
            }
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function GetEstadoPD(idEstado) {
        var cant = estadosPD.length;

        for (var i = 0; i < cant; i++) {
            if (estadosPD[i][0] == idEstado) {
                return estadosPD[i];
            }
        }

        return null;
    }

    function EnviarRecordatorio(idPersona, fecha) {
        custom_dialog.close();

        MostrarLoading();

        ConsultaAjax.url = 'partesDiariosPC.aspx/EnviarEmailRecordatorio';
        ConsultaAjax.data = '{ "idPersona": "' + idPersona + '", "fecha": "' + fecha + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            Mensaje(msg.d, 'success', true, false, 'Aceptar', '', 'custom_dialog.close()', '');
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function CargarParteDiario(url) {
        location.href = url;
    }

    function VerParteDiario(url) {
        window.open(url, '' , 'status=0, toolbar=0, resizable=no, menubar=0, directories=0, width=500, height=500, scrollbars=1');
    }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlacePage" Runat="Server">

<div class="page-title">
    <h1>Partes diarios -> Panel de control</h1>
</div>

<div class="full-width">
    <table class="tbl listado" cellspacing="0">
        <thead>
            <tr id="encabezado"class="lineaSimple">

            </tr>
            <tr class="filter_row">
                <td colspan="8" class="align-center" id="tdPaginas">
                    <a href="#" onclick="GetPanelControl(current_page - 1)">«Anterior</a>
                    |
                    <a href="#" onclick="GetPanelControl(0)">Semana actual</a>
                    |
                    <a href="#" onclick="GetPanelControl(current_page + 1)">Siguiente»</a>
                </td>
            </tr>
        </thead>
        <tbody id="listado">
            <tr>
                <td colspan="8" class="align-center">No hay partes diarios disponibles.</td>
            </tr>
        </tbody>
        <tfoot>
            <tr>
                <td colspan="8" class="align-center">
                    &nbsp;
                </td>
            </tr>
        </tfoot>
    </table>
</div>

</asp:Content>

