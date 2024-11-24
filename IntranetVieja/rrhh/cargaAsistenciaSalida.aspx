<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="cargaAsistenciaSalida.aspx.cs" Inherits="rrhh_cargaAsistenciaSalida" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">
    <script language="javascript" type="text/javascript">
        var current_page;
        var current_fecha;
        var personalesID;

        function GetDetalleAsistencia(pagina) {
            MostrarLoading();

            ConsultaAjax.url = 'cargaAsistenciaSalida.aspx/GetDetalleAsistencia';
            ConsultaAjax.data = '{ "pagina":"' + pagina + '" }';
            ConsultaAjax.AjaxSuccess = function (msg) {
                current_page = pagina;
                personalesID = [];

                $('#lblHoy').text(msg.d.Fecha);
                current_fecha = msg.d.Fecha;

                $('#listado').html();
                var cantFilas = msg.d.Filas.length;

                if (cantFilas > 0) {
                    var filas = [];
                    for (var i = 0; i < cantFilas; i++) {
                        var f = msg.d.Filas[i];

                        personalesID.push(f.PersonalID);

                        var fila = '<tr class="fila-color">';
                        fila += '<td class="align-left">' + f.Personal + '</td>';
						
						if(f.TipoCA == 0)
							fila += '<td><div class="input_wrapper"><div class="hasActions"><div class="cap"><input id="txtHoraSalida_' + f.PersonalID + '" maxlength="5" type="text" class="align-center" /></div></div><div class="input_actions" style="display:block" id="divInput_' + f.PersonalID + '"><ul><li class="clock" onclick="SetHoraSalida(' + f.PersonalID + ')"></li></ul></div></div></td>';
                        else
							fila += '<td><div class="input_wrapper input_wrapper_disabled"><div class="hasActions"><div class="cap"><input id="txtHoraSalida_' + f.PersonalID + '" maxlength="5" type="text" class="align-center" disabled="disabled" value ="'+ f.HoraSalida +'" /></div></div></div></td>';
						
						fila += '</tr>';
                        filas.push(fila);
                    }

                    $('#listado').html(filas.join(''));
                } else {
                    $('#listado').html('<tr><td colspan="8" class="align-center">No hay datos disponibles.</td></tr>');
                }

                EnlazarEventosMaster();
                EnlazarEventos();

                $('select').change();

                CerrarVentana();
            };
            ConsultaAjax.AjaxError = function(msg) {
                ErrorMsg(msg);
            };

            ConsultaAjax.Ejecutar();
        }
        
        function EnlazarEventos() {

        }

        function SetHoraSalida(pid) {
            var d = new Date();

            $('#txtHoraSalida_' + pid).val(d.toString('HH:mm'));
        }

        function Guardar() {
            CerrarVentana();

            var renglones = [];
            var c = personalesID.length;

            for (var i = 0, j = 0; i < c; i++) {
                var pid = personalesID[i];
                var horaSalida = $('#txtHoraSalida_' + pid);

                var datoCompleto = horaSalida.val().length > 0;

                if (datoCompleto) {
                    renglones[j++] = JSON.stringify({
                        PersonalID: pid,
                        HoraSalida: horaSalida.val()
                    });
                }
            }

            MostrarLoading();

            ConsultaAjax.url = 'cargaAsistenciaSalida.aspx/AddAsistencia';
            ConsultaAjax.data = JSON.stringify({ fecha: current_fecha, renglones: renglones });
            ConsultaAjax.AjaxSuccess = function (msg) {
                Mensaje('Los datos fueron guardados!', 'success', true, false, 'Aceptar', '', 'OnCerrar()', '');
            };
            ConsultaAjax.AjaxError = function (msg) {
                ErrorMsg(msg);
            };

            ConsultaAjax.Ejecutar();
        }

        function OnCerrar() {
            CerrarVentana();
            GetDetalleAsistencia(current_page);
        }

        $(document).ready(function () {
            $('#btnGuardar').click(function () {
                Mensaje('¿Desea confirmar la carga de datos?', 'warning', true, true, 'Guardar', 'Cancelar', 'Guardar()', 'custom_dialog.close()');
            });

            GetDetalleAsistencia(0);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlacePage" Runat="Server">
    
<div class="page-title">
    <h1>Asistencia -> Carga de salida</h1>
</div>

<div class="full_width">
    <div class="suggestion_message">Los horarios de salida deben tener el siguiente formato: HH:mm (ej. 17:30).</div>

    <table class="tbl listado" style="width: 500px" cellspacing="0">
        <thead>
            <tr>
                <td class="border_middle" colspan="4" id="lblHoy"></td>
            </tr>
            <tr>
                <td class="border_left">&nbsp;</td>
                <td class="border_middle" style="width:24%">Hora de salida</td>
            </tr>
            <tr class="filter_row">
                <td colspan="8" class="align-center" id="tdPaginas">
                    <a href="#" onclick="GetDetalleAsistencia(current_page - 1)">«Anterior</a>
                    |
                    <a href="#" onclick="GetDetalleAsistencia(0)">Hoy</a>
                    |
                    <a href="#" onclick="GetDetalleAsistencia(current_page + 1)">Siguiente»</a>
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
    
    <div class="form_buttons_container">
        <ul class="button_list">
            <li id="btnGuardar"><div class="btn primary_action_button button_100"><div class="cap"><span>Guardar</span></div></div></li>
            <li id="btnCancelar"><div class="btn secondary_action_button button_100"><div class="cap"><span>Cancelar</span></div></div></li>
        </ul>
    </div>
</div>

</asp:Content>

