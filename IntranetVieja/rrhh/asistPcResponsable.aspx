<%@ Page Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="asistPcResponsable.aspx.cs" Inherits="rrhh_asistPcResponsable" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">
    <script language="javascript" type="text/javascript">
        var current_page;

        function GetPanelControl(pagina) {
            MostrarLoading();

            ConsultaAjax.url = 'asistPcResponsable.aspx/GetPanelControl';
            ConsultaAjax.data = '{ "pagina":"' + pagina + '" }';
            ConsultaAjax.AjaxSuccess = function (msg) {
                current_page = pagina;

                $('#listado').html();
                var cantBases = msg.d.length;

                if (cantBases > 0) {
                    var filas = [];
                    for (var i = 0; i < cantBases; i++) {
                        var cantPersonas = msg.d[i].Datos.length;
                        for (var j = 0; j < cantPersonas; j++) {
                            filas.push(GetFilaBase(msg.d[i].Datos[j]));
                        }
                    }

                    $('#listado').html(filas.join(''));
                } else {
                    $('#listado').html('<tr><td colspan="8" class="align-center">No hay datos disponibles.</td></tr>');
                }

                DibujarEncabezado(pagina);

                CerrarVentana();
            };
            ConsultaAjax.AjaxError = function (msg) {
                ErrorMsg(msg);
            };

            ConsultaAjax.Ejecutar();
        }

        function DibujarEncabezado(pagina) {
            ConsultaAjax.url = 'panelControlAsistencia.aspx/GetEncabezado';
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

        function GetFilaBase(datos) {
            var result = '<tr>';

            var noHabil = 'style="background-color:#f8dede;"';

            result += '<td pid="' + datos.PersonalID + '">';
            result += datos.Personal;
            result += '</td>';

            var cantCeldas = datos.Celdas.length;
            for (var i = 0; i < cantCeldas; i++) {
                result += '<td class="align-center relative" ' + (datos.Celdas[i].DiaNoHabil ? noHabil : '') + ' title="' + GetEstadoAsistencia(datos.Celdas[i]) + '">';
                result += '<img src="/images/icons/bullet_' + GetIconAsistencia(datos.Celdas[i].EstadoID) + '.png" />';
                if (datos.Celdas[i].LlegoTarde) {
                    result += '<div class="notif star"></div>';
                }
                result += '</td>';
            }

            result += '</tr>';

            return result;
        }

        function GetEstadoAsistencia(dato) {
            var result;

            switch (dato.EstadoID) {
                case -1:
                    result = 'No disponible';
                    break;
                case 0:
                    result = 'Ausente';
                    break;
                case 1:
                    result = 'Presente ' + (dato.LlegoTarde ? '(llegada tarde)' : '');
                    break;
                case 2:
                    result = 'Licencia';
                    break;
                case 3:
                    result = 'Licencia por accidente ART';
                    break;
                case 4:
                    result = 'Licencia por enfermedad';
                    break;
                case 5:
                    result = 'Licencia por fallecimiento de familiar';
                    break;
                case 6:
                    result = 'Feriado Nacional / Provincial / Sindical';
                    break;
                case 7:
                    result = 'Feriado personal de obras';
                    break;
            }

            if (dato.HoraEntrada != '00:00') result += '\nHorario de entrada:\t' + dato.HoraEntrada;
            if (dato.HoraSalida != '00:00') result += '\nHorario de salida:\t\t' + dato.HoraSalida;

            if (dato.Observacion.length > 0) result += '\nObservación: ' + dato.Observacion;

            return result;
        }

        function GetIconAsistencia(estadoID) {
            switch (estadoID) {
                case -1:
                    return 'white';
                case 1:
                    return 'green';
                case 2:
                    return 'yellow';
                default:
                    return 'red';
            }
        }

        $(document).ready(function () {
            GetPanelControl(0);

            window.setInterval('GetPanelControl(current_page);', 60000);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlacePage" Runat="Server">
    
<div class="page-title">
    <h1>Asistencia -> Panel de control</h1>
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
                <td colspan="8" class="align-center">No hay datos disponibles.</td>
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