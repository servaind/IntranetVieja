<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="cargaAsistenciaEntrada.aspx.cs" Inherits="rrhh_cargaAsistencia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">
    <script language="javascript" type="text/javascript">
        var current_page;
        var current_fecha;
        var personalesID;

        function GetDetalleAsistencia(pagina) {
            MostrarLoading();

            ConsultaAjax.url = 'cargaAsistenciaEntrada.aspx/GetDetalleAsistencia';
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
							fila += '<td><div class="input_wrapper ' + (f.PuedeCargarFecha ? '' : 'input_wrapper_disabled') + '"><div class="hasActions"><div class="cap"><input id="txtHoraEntrada_' + f.PersonalID + '" maxlength="5" type="text" class="align-center" /></div></div><div class="input_actions" style="display:block" id="divInput_' + f.PersonalID + '"><ul><li class="clock" onclick="SetHoraEntrada(' + f.PersonalID + ')"></li></ul></div></div></td>';
                        else
							fila += '<td><div class="input_wrapper input_wrapper_disabled"><div class="hasActions"><div class="cap"><input disabled="disabled" id="txtHoraEntrada_' + f.PersonalID + '" maxlength="5" type="text" class="align-center" value ="'+ f.HoraEntrada +'" /></div></div></div></td>';
						
						if(f.TipoCA == 0)
							fila += '<td><div class="input_wrapper input_wrapper_selectbox ' + (f.PuedeCargarFecha ? '' : 'input_wrapper_selectbox_disabled') + '"><div class="cap"><span id="lblEstado' + f.PersonalID + '"></span><select ' + (f.PuedeCargarFecha ? '' : 'disabled="disabled"') + ' id="cbEstado_' + f.PersonalID + '"><option value="0">Ausente</option><option value="1" ' + ((f.EstadoID == 0 || f.EstadoID == 1) ? "selected=\"selected\"" : "") + '>Presente</option><option value="2" ' + (f.EstadoID == 2 ? "selected=\"selected\"" : "") + '>Licencia</option><option value="3">Ausente ART</option><option value="4">Ausente PMC</option><option value="5">Ausente FALL</option><option value="6">Feriado</option><option value="7">Feriado obras</option></select></div></div></td>';
                        else
							fila += '<td><div class="input_wrapper input_wrapper_selectbox input_wrapper_selectbox_disabled"><div class="cap"><span id="lblEstado' + f.PersonalID + '"></span><select disabled="disabled" id="cbEstado_' + f.PersonalID + '"><option value="0">Ausente</option><option value="1" ' + ((f.EstadoID == 0 || f.EstadoID == 1) ? "selected=\"selected\"" : "") + '>Presente</option><option value="2" ' + (f.EstadoID == 2 ? "selected=\"selected\"" : "") + '>Licencia</option><option value="3">Ausente ART</option><option value="4">Ausente PMC</option><option value="5">Ausente FALL</option><option value="6">Feriado</option><option value="7">Feriado obras</option></select></div></div></td>';
						
						
						if(f.TipoCA == 0)
							fila += '<td><div class="input_wrapper ' + (f.PuedeCargarFecha ? '' : 'input_wrapper_disabled') + '"><div class="cap"><input ' + (f.PuedeCargarFecha ? '' : 'disabled="disabled"') + ' id="txtObservacion_' + f.PersonalID + '" maxlength="300" value="' + f.Observacion + '" type="text" /></div></div></td>';
                        else
							fila += '<td><div class="input_wrapper"><div class="cap"><input id="txtObservacion_' + f.PersonalID + '" maxlength="300" value="' + f.Observacion + '" type="text" /></div></div></td>';
							
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
            $('select').change(function () {
                var pid = $(this).attr('id').replace('cbEstado_', '');

                var txtHoraEntrada = $('#txtHoraEntrada_' + pid);
                var actions = $('#divInput_' + pid);

                if ($(this).is(':enabled')) {
                    if (!($(this).val() == 1 || $(this).val() == 2)) {
                        txtHoraEntrada.parents('.input_wrapper').addClass('input_wrapper_disabled');
                        txtHoraEntrada.attr('disabled', 'disabled');
                        $(actions).hide();
                    } else {
                        txtHoraEntrada.parents('.input_wrapper').removeClass('input_wrapper_disabled');
                        txtHoraEntrada.removeAttr('disabled');
                        $(actions).show();
                    }
                }
            });
        }

        function SetHoraEntrada(pid) {
            var d = new Date();

            $('#txtHoraEntrada_' + pid).val(d.toString('HH:mm'));
        }
		

        function Guardar() {
            CerrarVentana();

            var renglones = [];
            var c = personalesID.length;

            for (var i = 0, j = 0; i < c; i++) {
                
				var pid = personalesID[i];
                var estado = $('#cbEstado_' + pid);
                var observacion = $('#txtObservacion_' + pid);
                var horaEntrada = $('#txtHoraEntrada_' + pid);

                var datoCompleto = true;
                
				if (estado.is(':enabled')) {
                    var estadoID = parseInt(estado.val());
                    switch (estadoID) {
                        case 0:
                            //datoCompleto &= observacion.val().length > 0;
                            break;
                        case 1:
                            datoCompleto &= horaEntrada.val().length > 0;
                            break;
                        case 2:
                            datoCompleto &= horaEntrada.val().length > 0 && observacion.val().length > 0;
                            break;
                    }
                }


                if (datoCompleto) {
                    renglones[j++] = JSON.stringify({
                        PersonalID: pid,
                        EstadoID: estado.val(),
                        Observacion: observacion.val(),
                        HoraEntrada: horaEntrada.val()
                    });
                }
            }

            MostrarLoading();

            ConsultaAjax.url = 'cargaAsistenciaEntrada.aspx/AddAsistencia';
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
    <h1>Asistencia -> Carga de entrada</h1>
</div>

<div class="full_width">
    <div class="suggestion_message">Los horarios de entrada deben tener el siguiente formato: HH:mm (ej. 8:30).</div>

    <table class="tbl listado" cellspacing="0">
        <thead>
            <tr>
                <td class="border_middle" colspan="4" id="lblHoy"></td>
            </tr>
            <tr>
                <td class="border_left" style="width:22%">&nbsp;</td>
                <td class="border_middle" style="width:15%">Hora de entrada</td>
                <td class="border_middle" style="width:20%">Estado</td>
                <td class="border_right">Observación</td>
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

