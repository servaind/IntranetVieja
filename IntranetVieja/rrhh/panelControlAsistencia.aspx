<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="panelControlAsistencia.aspx.cs" Inherits="rrhh_panelControlAsistencia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">
    <script language="javascript" type="text/javascript">
        var current_page;

        function GetPanelControl(pagina) {
            MostrarLoading();

            ConsultaAjax.url = 'panelControlAsistencia.aspx/GetPanelControl';
            ConsultaAjax.data = '{ "pagina":"' + pagina + '" }';
            ConsultaAjax.AjaxSuccess = function (msg) {
                current_page = pagina;

                $('#listado').html();
                var cantBases = msg.d.length;

                if (cantBases > 0) {
                    var filas = [];
                    for (var i = 0; i < cantBases; i++) {
                        var fila = '<tr class="fila-color">';
                        fila += '<td class="align-center" colspan="8" style="font-weight:bold">' + msg.d[i].Titulo + '</td>';
                        fila += '</tr>';
                        filas.push(fila);

                        var cantPersonas = msg.d[i].Datos.length;
                        for(var j = 0; j < cantPersonas; j++) {
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
            ConsultaAjax.AjaxError = function(msg) {
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
                result += '<img class="hand" src="/images/icons/bullet_' + GetIconAsistencia(datos.Celdas[i].EstadoID, datos.Celdas[i].ModoEntrada, datos.Celdas[i].ModoSalida) + '.png" ';
                result += 'onclick="EditarAsistencia(' + datos.Celdas[i].ID + ', ' + datos.PersonalID + ', \'' + datos.Personal;
                result += '\', \'' + datos.Celdas[i].Fecha + '\')" />';
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

            switch(dato.EstadoID) {
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
            if (dato.HoraSalida != '00:00') result +=  '\nHorario de salida: \t' + dato.HoraSalida;
			
			if (dato.ModoEntrada != '') result += '\nModo de entrada:\t' + dato.ModoEntrada;
			if (dato.ModoSalida != '') result +=  '\nModo de salida: \t' + dato.ModoSalida;

            if (dato.Observacion.length > 0) result += '\nObservación: ' + dato.Observacion;

            return result;
        }
        
        function GetIconAsistencia(estadoID) {
            switch(estadoID) {
                case -1:
                    return 'white';
                case 1:
                    return 'green';
                case 2:
                    return 'yellow';
                default :
                    return 'red';
            }
        }

        function GetIconAsistencia(estadoID, modoEntrada, modoSalida) {
            
			if(modoEntrada == 'Clave Numérica' || modoSalida == 'Clave Numérica' )
			{
				estadoID = 4;
			} 
		
			switch(estadoID) {
                case -1:
                    return 'white';
                case 1:
                    return 'green';
                case 2:
                    return 'yellow';
				case 4:
				    return 'green_clav';
                default :
                    return 'red';
            }
        }

        var currentID;
        var currentPersonalID;
        function EditarAsistencia(id, personalID, personal, fecha) {
            LimpiarAsistencia();
            <% if (GPermisosPersonal.TieneAcceso(PermisosPersona.LicRRHH))
               { %>
            ConsultaAjax.url = 'panelControlAsistencia.aspx/GetDetalleAsistencia';
            ConsultaAjax.data = '{ "id":"' + id + '" }';
            ConsultaAjax.AjaxSuccess = function (msg) {
                currentID = id;
                currentPersonalID = personalID;

                $('#lblPersona').text(personal);
                $('#lblFecha').text(fecha);

                if (msg.d != null) {
                    $('#cbEstado').val(msg.d.EstadoID);
                    $('#txtHoraEntrada').val(msg.d.HoraEntrada);
                    $('#txtHoraSalida').val(msg.d.HoraSalida);
                    $('#txtObservacion').val(msg.d.Observacion);
                }

                $('#cbEstado').change();

                MostrarVentana('dlgEditarAsistencia');
            };
            ConsultaAjax.AjaxError = function (msg) {
                ErrorMsg(msg);
            };

            ConsultaAjax.Ejecutar();
            <% } %>
        }

        function GuardarAsistencia() {
            var result = true;
            var horaEntrada = $('#txtHoraEntrada');
            var horaSalida = $('#txtHoraSalida');
            var observacion = $('#txtObservacion');

            if ($('#cbEstado').val() == 1) {
                result &= TieneDatos(horaEntrada, 'input_wrapper', 'input_wrapper_error');
                result &= TieneDatos(horaSalida, 'input_wrapper', 'input_wrapper_error');
            }
            result &= TieneDatos(observacion, 'textarea_wrapper', 'textarea_wrapper_error');

            if (result) {
                if ($('#cbEstado').val() == 1) {
                    horaEntrada = horaEntrada.val();
                    horaSalida = horaSalida.val();
                } else horaEntrada = horaSalida = '00:00';

                ConsultaAjax.url = 'panelControlAsistencia.aspx/UpdateAsistencia';
                ConsultaAjax.data = JSON.stringify({
                    asistenciaID: currentID,
                    personalID: currentPersonalID,
                    fecha: $('#lblFecha').text(),
                    estadoID: $('#cbEstado').val(),
                    observacion: observacion.val(),
                    horaEntrada: horaEntrada,
                    horaSalida: horaSalida
                }),
                ConsultaAjax.AjaxSuccess = function (msg) {
                    LimpiarAsistencia();
                    CerrarVentana();

                    GetPanelControl(current_page);
                };
                ConsultaAjax.AjaxError = function(msg) {
                    ult_ventana = 'dlgEditarAsistencia';
                    ErrorMsg(msg);
                };

                ConsultaAjax.Ejecutar();
            }
        }

        function LimpiarAsistencia() {
            currentID = null;
            currentPersonalID = null;
            $('#cbEstado').val(0).change();
            $('#txtHoraEntrada').val('');
            $('#txtHoraSalida').val('');
            $('#txtObservacion').val('');
            $('#liHoraEntrada').show();
            $('#liHoraSalida').show();
        }

        $(document).ready(function () {
            $('#btnAsistenciaCancelar').click(function () {
                LimpiarAsistencia();
                CerrarVentana();
            });

            $('#btnAsistenciaGuardar').click(function () {
                GuardarAsistencia();
            });

            $('#cbEstado').change(function () {
                var estadoID = $(this).val();

                if (estadoID != 1) {
                    $('#liHoraEntrada').hide();
                    $('#liHoraSalida').hide();
                } else {
                    $('#liHoraEntrada').show();
                    $('#liHoraSalida').show();
                }
            });

            GetPanelControl(0);
            
            window.setInterval('GetPanelControl(current_page);', 30000);
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

<div class="dialog_wrapper" style="width:500px" id="dlgEditarAsistencia">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3>Editar asistencia</h3></div></div></div>
    </div>

    <div class="dialog_content">
        <ul class="middle_form">
            <li class="form_floated_item form_floated_item_50">
                <label class="label">Persona</label>
                <span id="lblPersona">[...]</span>
            </li>
            <li class="form_floated_item form_floated_item_50 form_floated_item_right">
                <label class="label">Fecha</label>
                <span id="lblFecha">[...]</span>
            </li>
            <li class="form_floated_item form_floated_item_50">
                <label class="label" for="txtDepartamento">Estado</label>
                <div class="input_wrapper input_wrapper_selectbox">
                    <div class="cap">
                        <span id="lblEstado"></span>
                        <select id="cbEstado">
                            <option value="0">Ausente</option>
                            <option value="1">Presente</option>
                            <option value="2">Licencia</option>
                            <option value="3">Ausente ART</option>
                            <option value="4">Ausente PMC</option>
                            <option value="5">Ausente FALL</option>
                            <option value="6">Feriado</option>
                            <option value="7">Feriado obras</option>
                        </select>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_50" id="liHoraEntrada">
                <label class="label" for="txtFechaDesde">Hora de entrada</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtHoraEntrada" value="" maxlength="5" type="text" />
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_50 form_floated_item_right" id="liHoraSalida">
                <label class="label" for="txtFechaDesde">Hora de salida</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtHoraSalida" value="" maxlength="5" type="text" />
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_100">
                <label class="label" for="txtDepartamento">Observación</label>
                <div class="textarea_wrapper"> 
	                <div class="top">
		                <div class="cap"></div>
	                </div>
	                <div class="inner">
		                <div class="cap">
			                <textarea id="txtObservacion" maxlength="300" onkeyup="return MaxLength(this)"></textarea>     
		                </div>
	                </div>
	                <div class="bottom">
		                <div class="cap"></div>
	                </div>
                </div>
            </li>
        </ul>
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li><div class="btn primary_action_button_small button_100" id="btnAsistenciaGuardar"><div class="cap"><span>Guardar</span></div></div></li>
            <li><div class="btn secondary_action_button_small button_100" id="btnAsistenciaCancelar"><div class="cap"><span>Cancelar</span></div></div></li>
        </ul>
    </div>
</div>

</asp:Content>

