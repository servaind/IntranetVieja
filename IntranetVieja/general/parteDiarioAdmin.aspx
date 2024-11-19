<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="parteDiarioAdmin.aspx.cs" Inherits="general_parteDiarioAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">

<script language="javascript" type="text/javascript">
    var current_item;
    var personas = [];
    var maxHoras = parseInt('<%= GPartesDiarios.MaxHorasParteDiario.ToString() %>');

    $(document).ready(function () {
        $('#txtFechaParte').datepicker({ dayNames: dayNames, dayNamesMin: dayNamesMin, monthNames: monthNames, dateFormat: 'dd/mm/yy' });

        $('#txtFechaParte').keydown(function (e) {
            if (e.keyCode == '13') {
                $('#btnFechaParteAceptar').click();
            }
        });
        $('#btnFechaParteAceptar').click(function () {
            var fecha = $('#txtFechaParte');
            var result = true;

            result &= ContieneFecha(fecha, 'input_wrapper', 'input_wrapper_error');

            if (result) {
                MostrarLoading();

                ConsultaAjax.url = 'parteDiarioAdmin.aspx/CargaParteDiario';
                ConsultaAjax.data = '{ "fecha": "' + fecha.val() + '" }';
                ConsultaAjax.AjaxSuccess = function (msg) {
                    MostrarImputacionesPD();

                    $('#lblFechaParte').text(fecha.val());
                };
                ConsultaAjax.AjaxError = function (msg) {
                    MostrarVentana('divFechaParte');
                    $('#lblFechaError').html(msg);
                    $('#lblFechaError').show();
                };

                ConsultaAjax.Ejecutar();
            }
        });
        $('#btnFechaParteCancelar').click(function () {
            location.href = '<%= Constantes.UrlIntraDefault %>';
        });
        $('#btnAgregarImputacion').click(function () {
            ReiniciarCamposImputacion();

            MostrarVentana('divImputacion');
        });
        $('#btnImputacionAceptar').click(function () {
            var idImputacion = $('#contentPlacePage_cbImputacion');
            var horas = $('#contentPlacePage_cbImputacionHoras');
            var tareasR = $('#txtTareasR');
            var tareasP = $('#txtTareasP');
            var novedadesV = $('#txtNovedadesV');
            var novedadesH = $('#txtNovedadesH');
            var archivo = $('#txtArchivo');
            var intervinieron = [];
            var result = true;

            var hayITR = archivo.val().length > 0;

            result &= TieneDatos(idImputacion, 'input_wrapper', 'input_wrapper_selectbox_error') && ContieneValorDiferente(idImputacion, '<%= Constantes.IdImputacionInvalida.ToString() %>', 'input_wrapper', 'input_wrapper_selectbox_error');
            result &= TieneDatos(horas, 'input_wrapper', 'input_wrapper_selectbox_error');
            result &= TieneDatos(tareasR, 'textarea_wrapper', 'textarea_wrapper_error');

            var p;
            var h;
            var cant = iPersonas.length;
            for (var i = 0; i < cant; i++) {
                var j = iPersonas[i];
                var persona = [];

                p = $('#cbPersona_' + j);
                h = $('#cbHoraPersona_' + j);

                result &= TieneDatos(p, 'input_wrapper', 'input_wrapper_selectbox_error') && ContieneValorDiferente(p, '<%= Constantes.IdPersonaInvalido.ToString() %>', 'input_wrapper', 'input_wrapper_selectbox_error');
                result &= ContieneNumeros(h, 'input_wrapper', 'input_wrapper_error');

                if (result) {
                    persona = [p.val(), h.val()]
                    intervinieron.push(persona);
                }
            }

            if (result) {
                MostrarLoading();

                if (hayITR) {
                    $('#frameAux').contents().find("#txtFecha").val($('#lblFechaParte').text());
                    $('#frameAux').contents().find("#txtImputacion").val($('#contentPlacePage_cbImputacion option:selected').text().split('-')[0]);
                    $('#frameAux').contents().find("#btnSubirArchivo").click();
                }

                if (current_item == null) {
                    ConsultaAjax.url = 'parteDiarioAdmin.aspx/AgregarImputacion';
                    ConsultaAjax.data = JSON.stringify({ idImputacion: idImputacion.val(), horas: horas.val(), tareasR: tareasR.val(), tareasP: tareasP.val(), novedadesV: novedadesV.val(), novedadesH: novedadesH.val(), hayITR : hayITR, personasIntervienen: intervinieron });
                } else {
                    ConsultaAjax.url = 'parteDiarioAdmin.aspx/ActualizarImputacion';
                    ConsultaAjax.data = JSON.stringify({ item: current_item, idImputacion: idImputacion.val(), horas: horas.val(), tareasR: tareasR.val(), tareasP: tareasP.val(), novedadesV: novedadesV.val(), novedadesH: novedadesH.val(), hayITR : hayITR, personasIntervienen: intervinieron });
                }
                ConsultaAjax.AjaxSuccess = function (msg) {
                    ReiniciarCamposImputacion();
                    current_item = null;

                    MostrarImputacionesPD();
                };
                ConsultaAjax.AjaxError = function (msg) {
                    ult_ventana = 'divImputacion';
                    ErrorMsg(msg);
                };

                ConsultaAjax.Ejecutar();
            }
        });
        $('#btnImputacionCancelar').click(function () {
            current_item = null;

            CerrarVentana();
            ReiniciarCamposImputacion();
        });
        $('#btnAgregarPersona').click(function () {
            AgregarPersona('<%= Constantes.IdPersonaInvalido.ToString() %>', 8);
        });
        $('#btnGuardar').click(function () {
            MostrarLoading();

            ConsultaAjax.url = 'parteDiarioAdmin.aspx/GuardarParteDiario';
            ConsultaAjax.AjaxSuccess = function (msg) {
                OnCerrar();
            };
            ConsultaAjax.AjaxError = function (msg) {
                ErrorMsg(msg);
            };

            ConsultaAjax.Ejecutar();
        });
        $('#btnFinalizar').click(function () {
            Mensaje('Una vez finalizado, no podrá editar nuevamente el parte diario. ¿Desea continuar?', 'warning', true, true, 'Cancelar', 'Finalizar', 'custom_dialog.close()', 'OnFinalizarParteDiario()');
        });

        $('#txtArchivo').click(function () {
            $('#frameAux').contents().find("#txtArchivo").click();
        });

        GetPersonas();

        <% if(this.AutoAceptarFecha) { %>
        $('#btnFechaParteAceptar').click();
        <% }
        else { %>
        MostrarSeleccionarFecha();
        <% } %>

        ActualizarUpload();
    });

    function ActualizarUpload() {
        $('#frameAux').attr('src', '/general/itrUpload.aspx');
    }
    function SetFile(file) {
        $('#txtArchivo').val(file);
    }

    function OnCerrar() {
        location.href = '<%= Constantes.UrlIntraDefault %>';
    }

    function OnFinalizarParteDiario() {
        MostrarLoading();

        ConsultaAjax.url = 'parteDiarioAdmin.aspx/FinalizarParteDiario';
        ConsultaAjax.AjaxSuccess = function (msg) {
            OnCerrar();
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function ReiniciarCamposImputacion() {
        $('#divImputacion textarea').val('');
        $('#contentPlacePage_cbImputacion').val('<%= Constantes.IdImputacionInvalida.ToString() %>');
        $('#contentPlacePage_cbImputacion').change();
        $('#txtArchivo').val('');

        $('#personas-intervinieron').html('');
        iPersonas = [];
        ultPersona = 0;
    }

    function MostrarSeleccionarFecha() {
        $('#lblFechaError').hide();

        MostrarVentana('divFechaParte');
    }

    function DibujarAcciones() {
        $('.input_actions').html('<ul><li class="edit"></li><li class="delete"></li></ul>');
    }

    function DibujarAccionesPersona() {
        $('.actions_persona').html('<ul><li class="delete"></li></ul>');
    }

    var EnlazarEventos = function () {
        $('.input_actions ul li.edit').unbind('click').click(function () {
            var tipo = $(this).parents('.input_actions').attr('name');
            var value = $(this).parents('.input_actions').attr('value');

            MostrarImputacion(value);
        });
        $('.input_actions ul li.delete').unbind('click').click(function () {
            var tipo = $(this).parents('.input_actions').attr('name');
            var value = $(this).parents('.input_actions').attr('value');

            if (tipo == 'imputacion') {
                EliminarImputacion(value);
            }
            else if (tipo == 'persona') {
                EliminarPersona(value);
            }
        });
    }

    function GetPersonas() {
        ConsultaAjax.url = 'parteDiarioAdmin.aspx/GetPersonas';
        ConsultaAjax.AjaxSuccess = function (msg) {
            var cant = msg.d.length;

            for (var i = 0; i < cant; i++) {
                personas.push(msg.d[i]);
            }
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function MostrarImputacionesPD() {
        ConsultaAjax.url = 'parteDiarioAdmin.aspx/GetImputaciones';
        ConsultaAjax.AjaxSuccess = function (msg) {
            $('#imputaciones').html('');

            var cant = msg.d.length;
            if (cant > 0) {
                for (var i = 0; i < cant; i++) {
                    var fila = '<tr class="no-editable">';
                    fila += '<td class="align-left"><div class="hasActions">' + msg.d[i][1] + '<div class="input_actions" name="imputacion" value="' + msg.d[i][0] + '"></div></div></td>';
                    fila += '<td class="align-center">' + msg.d[i][2] + '</td>';
                    fila += '</tr>';

                    $('#imputaciones').append(fila);
                }
            }

            DibujarAcciones();
            EnlazarEventosMaster();
            EnlazarEventos();

            CerrarVentana();
        };
        ConsultaAjax.AjaxError = function (msg) {
            MostrarVentana('divFechaParte');
            $('#lblFechaError').html(msg);
            $('#lblFechaError').show();
        };

        ConsultaAjax.Ejecutar();
    }

    function MostrarImputacion(item) {
        MostrarLoading();

        ConsultaAjax.url = 'parteDiarioAdmin.aspx/GetImputacion';
        ConsultaAjax.data = '{ "item": "' + item + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            ReiniciarCamposImputacion();

            $('#contentPlacePage_cbImputacion').val(item);
            $('#contentPlacePage_cbImputacion').change();
            $('#contentPlacePage_cbImputacionHoras').val(msg.d[0]);
            $('#contentPlacePage_cbImputacionHoras').change();
            $('#txtTareasR').val(msg.d[1]);
            $('#txtTareasP').val(msg.d[2]);
            $('#txtNovedadesV').val(msg.d[3]);
            $('#txtNovedadesH').val(msg.d[4]);

            var c = msg.d[5].length;
            for (var i = 0; i < c; i++) {
                AgregarPersona(msg.d[5][i][0], msg.d[5][i][1]);
            }

            current_item = item;

            MostrarVentana('divImputacion');
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    var ultPersona = 0;
    var iPersonas = [];
    function AgregarPersona(idPersona, horas) {
        var fila = '<tr class="no-editable" id="persona_' + ultPersona + '">';
        fila += '<td class="align-left"><div class="hasActions"><div class="input_wrapper input_wrapper_selectbox"><div class="cap"><span></span><select id="cbPersona_' + ultPersona + '"></select></div></div><div class="input_actions input_actions_selectbox actions_persona" name="persona" value="' + ultPersona + '"></div></div></td>';
        fila += '<td><div class="input_wrapper input_wrapper_selectbox"><div class="cap"><span></span><select id="cbHoraPersona_' + ultPersona + '"></select></div></div></td>';
        fila += '</tr>';

        $('#personas-intervinieron').append(fila);

        var cp = personas.length;
        var p = [];
        for (var i = 0; i < cp; i++) {
            p.push('<option value="' + personas[i][0] + '">' + personas[i][1] + '</option>');
        }
        $('#cbPersona_' + ultPersona).html(p.join(''));

        iPersonas.push(ultPersona);

        var h = [];
        for (var i = 1; i <= maxHoras; i++) {
            h.push('<option value="' + i + '">' + i + '</option>');
        }
        $('#cbHoraPersona_' + ultPersona).html(h.join(''));

        DibujarAccionesPersona();
        EnlazarEventosMaster();
        EnlazarEventos();

        $('#cbPersona_' + ultPersona).val(idPersona);
        $('#cbPersona_' + ultPersona).change();

        $('#cbHoraPersona_' + ultPersona).val(horas);
        $('#cbHoraPersona_' + ultPersona).change();

        ultPersona++;
    }

    function EliminarPersona(persona) {
        $('#persona_' + persona).remove();

        RemoveArrayElement(iPersonas, parseInt(persona));
    }

    function EliminarImputacion(idImputacion) {
        MostrarLoading();

        ConsultaAjax.url = 'parteDiarioAdmin.aspx/EliminarImputacion';
        ConsultaAjax.data = '{ "idImputacion": "' + idImputacion + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            MostrarImputacionesPD();
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlacePage" Runat="Server">

<div class="page-title">
    <h1>Carga de parte diario</h1>
</div>

<div class="form_place">

    <ul class="middle_form" style="height:70px">
        <li class="form_floated_item form_floated_item_half">
            <label class="label">Fecha del parte diario</label>
            <span id="lblFechaParte"></span>
        </li>
    </ul>

    <h3>Imputaciones</h3>

    <table class="tbl editable listado" cellspacing="0">
        <thead>
            <tr>
                <td class="border_left">Imputación</td>
                <td class="border_middle" style="width:90px">Horas</td>
            </tr>
        </thead>
        <tbody id="imputaciones">

        </tbody>
        <tfoot>
            <tr>
                <td colspan="2" class="border_middle">
                    <a id="btnAgregarImputacion">Agregar imputación</a>
                </td>
            </tr>
        </tfoot>
    </table>

    <div class="form_buttons_container">
        <ul class="button_list">
            <li id="btnGuardar"><div class="btn primary_action_button button_100"><div class="cap"><span>Guardar</span></div></div></li>
            <li id="btnFinalizar"><div class="btn secondary_action_button button_100"><div class="cap"><span>Finalizar</span></div></div></li>
        </ul>
    </div>
</div>

<div class="dialog_wrapper" style="width:400px" id="divFechaParte">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3>Seleccionar fecha</h3></div></div></div>
    </div>

    <div class="dialog_content">
        <p>Ingrese la fecha para el parte diario que desea cargar</p>
        <br />
        <ul class="middle_form">
            <li class="form_floated_item" style="width:150px">
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtFechaParte" maxlength="10" value="<%=FechaParte.ToShortDateString() %>" type="text"/>
                    </div>
                </div>
            </li>
        </ul>
        <br />
        <p class="clear" id="lblFechaError">ERROR</p>
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li id="btnFechaParteAceptar"><div class="btn primary_action_button_small button_100"><div class="cap"><span>Aceptar</span></div></div></li>
            <li id="btnFechaParteCancelar"><div class="btn secondary_action_button_small button_100"><div class="cap"><span>Cancelar</span></div></div></li>
        </ul>
    </div>
</div>

<div class="dialog_wrapper" style="width:600px" id="divImputacion">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3>Datos de la imputación</h3></div></div></div>
    </div>

    <div class="dialog_content" style="height:400px">
        <p>Los campos marcados con <span class="required"></span> son obligatorios.</p>
        <br />
        <ul class="middle_form">
            <li class="form_floated_item form_floated_item_100">
                <label class="label required" for="cbBuscarResultado">Imputación</label>
                <div class="input_wrapper input_wrapper_selectbox">
                    <div class="cap">
                        <span id="lblImputacion"></span>
                        <select id="cbImputacion" runat="server">
                        </select>
                    </div>
                </div>
            </li>
            <li class="form_floated_item" style="width:130px">
                <label class="label required" for="cbImputacionHoras">Horas</label>
                <div class="input_wrapper input_wrapper_selectbox">
                    <div class="cap">
                        <span id="lblImputacionHoras"></span>
                        <select id="cbImputacionHoras" runat="server">
                        </select>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_100">
                <label class="label required" for="txtTareasR">Tareas realizadas (máx 3000 caracteres)</label>
                <div class="textarea_wrapper"> 
	                <div class="top">
		                <div class="cap"></div>
	                </div>
	                <div class="inner">
		                <div class="cap">
			                <textarea id="txtTareasR" maxlength="3000" onkeyup="return MaxLength(this)"></textarea>     
		                </div>
	                </div>
	                <div class="bottom">
		                <div class="cap"></div>
	                </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_100">
                <label class="label" for="txtTareasP">Tareas programadas (máx 3000 caracteres)</label>
                <div class="textarea_wrapper"> 
	                <div class="top">
		                <div class="cap"></div>
	                </div>
	                <div class="inner">
		                <div class="cap">
			                <textarea id="txtTareasP" maxlength="3000" onkeyup="return MaxLength(this)"></textarea>     
		                </div>
	                </div>
	                <div class="bottom">
		                <div class="cap"></div>
	                </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_100">
                <label class="label" for="txtNovedadesV">Novedades del vehículo (máx 1000 caracteres)</label>
                <div class="textarea_wrapper"> 
	                <div class="top">
		                <div class="cap"></div>
	                </div>
	                <div class="inner">
		                <div class="cap">
			                <textarea id="txtNovedadesV" maxlength="1000" onkeyup="return MaxLength(this)"></textarea>     
		                </div>
	                </div>
	                <div class="bottom">
		                <div class="cap"></div>
	                </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_100">
                <label class="label" for="txtNovedadesH">Novedades de la herramienta (máx 1000 caracteres)</label>
                <div class="textarea_wrapper"> 
	                <div class="top">
		                <div class="cap"></div>
	                </div>
	                <div class="inner">
		                <div class="cap">
			                <textarea id="txtNovedadesH" maxlength="1000" onkeyup="return MaxLength(this)"></textarea>     
		                </div>
	                </div>
	                <div class="bottom">
		                <div class="cap"></div>
	                </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_100">
                <label class="label" for="txtArchivo">Carga de ITR (sólo archivos PDF)</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtArchivo" readonly="readonly" maxlength="255" type="text" />
                    </div>
                </div>
            </li>
        </ul>

        <label class="label clear">Personas que intervinieron</label>
        <table class="tbl editable listado" cellspacing="0">
            <thead>
                <tr>
                    <td class="border_left">Persona</td>
                    <td class="border_middle" style="width:90px">Horas</td>
                </tr>
            </thead>
            <tbody id="personas-intervinieron">

            </tbody>
            <tfoot>
                <tr>
                    <td colspan="2" class="border_middle">
                        <a id="btnAgregarPersona">Agregar persona</a>
                    </td>
                </tr>
            </tfoot>
        </table>

        <!--[if lt IE 9]>     
        <br>
        <![endif]-->
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li id="btnImputacionAceptar"><div class="btn primary_action_button_small button_100"><div class="cap"><span>Aceptar</span></div></div></li>
            <li id="btnImputacionCancelar"><div class="btn secondary_action_button_small button_100"><div class="cap"><span>Cancelar</span></div></div></li>
        </ul>
    </div>
</div>

</asp:Content>

