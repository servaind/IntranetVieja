<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="altaInstrumento.aspx.cs" Inherits="stock_altaInstrumento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $('#contentPlacePage_txtUltCalibracion').datepicker({ dayNames: dayNames, dayNamesMin: dayNamesMin, monthNames: monthNames, dateFormat: 'dd/mm/yy', maxDate: '<%= DateTime.Now.ToShortDateString() %>' });

            $('#btnGuardar').click(function () {
                OcultarError();

                var result = true;
                var numero = $('#contentPlacePage_txtNumero');
                var tipo = $('#contentPlacePage_cbTipo');
                var descripcion = $('#contentPlacePage_txtDescripcion');
                var grupo = $('#contentPlacePage_cbGrupo');
                var ubicacion = $('#contentPlacePage_txtUbicacion');
                var marca = $('#contentPlacePage_cbMarca');
                var modelo = $('#contentPlacePage_txtModelo');
                var numSerie = $('#contentPlacePage_txtNumSerie');
                var rango = $('#contentPlacePage_txtRango');
                var resolucion = $('#contentPlacePage_txtResolucion');
                var clase = $('#contentPlacePage_txtClase');
                var incertidumbre = $('#contentPlacePage_txtincertidumbre');
                var responsable = $('#contentPlacePage_cbResponsable');
                var frecCalib = $('#contentPlacePage_cbFrecuencia');
                var ultCalib = $('#contentPlacePage_txtUltCalibracion');


                result &= TieneDatos(numero, 'input_wrapper', 'input_wrapper_error') && ContieneNumeros(numero, 'input_wrapper', 'input_wrapper_error');
                result &= TieneDatos(tipo, 'input_wrapper_selectbox', 'input_wrapper_selectbox_error');
                result &= TieneDatos(descripcion, 'textarea_wrapper', 'textarea_wrapper_error');
                result &= TieneDatos(grupo, 'input_wrapper_selectbox', 'input_wrapper_selectbox_error');
                result &= TieneDatos(ubicacion, 'input_wrapper', 'input_wrapper_error');
                result &= TieneDatos(marca, 'input_wrapper_selectbox', 'input_wrapper_selectbox_error');
                result &= TieneDatos(modelo, 'input_wrapper', 'input_wrapper_error');
                result &= TieneDatos(numSerie, 'input_wrapper', 'input_wrapper_error');
                result &= TieneDatos(rango, 'input_wrapper', 'input_wrapper_error');
                result &= TieneDatos(resolucion, 'input_wrapper', 'input_wrapper_error');
                result &= TieneDatos(clase, 'input_wrapper', 'input_wrapper_error');
                result &= TieneDatos(numSerie, 'input_wrapper', 'input_wrapper_error');
                result &= TieneDatos(incertidumbre, 'input_wrapper_selectbox', 'input_wrapper_selectbox_error');
                result &= TieneDatos(frecCalib, 'input_wrapper_selectbox', 'input_wrapper_selectbox_error');
                result &= ContieneFecha(ultCalib, 'input_wrapper', 'input_wrapper_error');

                if (result) {
                    MostrarLoading();

                    $.ajax({
                        url: 'altaInstrumento.aspx/AddInstrumento',
                        data: JSON.stringify({ numero: numero.val(), idTipo: tipo.val(), descripcion: descripcion.val(),
                            idGrupo: grupo.val(), ubicacion: ubicacion.val(), idMarca: marca.val(), modelo: modelo.val(),
                            numSerie: numSerie.val(), rango: rango.val(), resolucion: resolucion.val(), clase: clase.val(), incertidumbre: incertidumbre.val(),
                            idResponsable: responsable.val(), idFrecCalibracion: frecCalib.val(),
                            ultCalib: ultCalib.val()
							}),
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        async: true,
                        cache: false,
                        success: function (msg) {
                            if (msg.d.Success) Mensaje('El instrumento fue agregado al sistema.', 'success', true, false, 'Aceptar', '', 'OnCerrar()', '');
                            else MostrarError(msg.d.Message);
                        },
                        error: function (data, ajaxOptions, thrownError) {
                            MostrarError(GetAjaxError(data));
                        }
                    });
                }
                else {
                    MostrarError('Algunos campos no han sido completados o contienen datos inválidos.');
                }
            });
            $('#btnCancelar').click(function () {
                Mensaje('Todos los datos ingresados serán descartados, ¿desea continuar?', 'warning', true, true, 'Cancelar', 'Aceptar', 'custom_dialog.close()', 'OnCerrar()');
            });
        });

        function OnCerrar() {
            location.href = '/stock/instrumentosLista.aspx';
        }

        function OcultarError() {
            $('#error_msg').hide();
        }

        function MostrarError(msg) {
            CerrarVentana();
            $('#error_msg_text').text(msg);
            $('#error_msg').show();
            $('html, body').animate({ scrollTop: $("#error_msg").offset().top - 50 }, 200);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlacePage" Runat="Server">

    <div class="page-title">
        <h1>Alta de instrumento</h1>
    </div>

    <div class="form_place">
        <ul class="middle_form">
            <li class="form_floated_item form_floated_item_100" style="display:none" id="error_msg">
                <div class="suggestion_message error"><span id="error_msg_text"></span></div>
            </li>
            <li class="form_floated_item form_floated_item_half">
                <label class="label" for="txtMarca">Número</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtNumero" value="" maxlength="5" type="text" runat="server"/>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_half form_floated_item_right">
                <label class="label" for="cbTipoHerramienta">Tipo</label>
                <div class="input_wrapper input_wrapper_selectbox">
                    <div class="cap">
                        <span id="lblTipo" runat="server"></span>
                        <select id="cbTipo" runat="server"></select>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_full">
                <label class="label" for="txtDescripcion">Descripción</label>
                <div class="textarea_wrapper clear"> 
	                <div class="top">
		                <div class="cap"></div>
	                </div>
	                <div class="inner">
		                <div class="cap">
			                <textarea id="txtDescripcion" runat="server"></textarea>     
		                </div>
	                </div>
	                <div class="bottom">
		                <div class="cap"></div>
	                </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_half">
                <label class="label" for="cbTipoHerramienta">Grupo</label>
                <div class="input_wrapper input_wrapper_selectbox">
                    <div class="cap">
                        <span id="lblGrupo" runat="server"></span>
                        <select id="cbGrupo" runat="server"></select>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_half form_floated_item_right">
                <label class="label" for="cbTipoHerramienta">Marca</label>
                <div class="input_wrapper input_wrapper_selectbox">
                    <div class="cap">
                        <span id="lblMarca" runat="server"></span>
                        <select id="cbMarca" runat="server"></select>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_half">
                <label class="label" for="txtMarca">Modelo</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtModelo" value="" maxlength="20" type="text" runat="server"/>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_half form_floated_item_right">
                <label class="label" for="txtMarca">Nº de serie</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtNumSerie" value="" maxlength="20" type="text" runat="server"/>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_half">
                <label class="label" for="txtRango">Rango</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtRango" value="" maxlength="20" type="text" runat="server"/>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_half form_floated_item_right">
                <label class="label" for="txtResolucion">Resolución</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtResolucion" value="" maxlength="20" type="text" runat="server"/>
                    </div>
                </div>
            </li>
               <li class="form_floated_item form_floated_item_half">
                <label class="label" for="txtClase">Clase</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtClase" value="" maxlength="20" type="text" runat="server"/>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_half form_floated_item_right">
                <label class="label" for="txtIncertidumbre">Incertidumbre</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtIncertidumbre" value="" maxlength="20" type="text" runat="server"/>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_half">
                <label class="label" for="txtMarca">Ubicación</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtUbicacion" value="" maxlength="5" type="text" runat="server"/>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_half form_floated_item_right">
                <label class="label" for="cbTipoHerramienta">Responsable</label>
                <div class="input_wrapper input_wrapper_selectbox">
                    <div class="cap">
                        <span id="lblResponsable" runat="server"></span>
                        <select id="cbResponsable" runat="server"></select>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_half">
                <label class="label" for="txtMarca">Última calibración</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtUltCalibracion" value="" readonly="readonly" type="text" runat="server"/>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_half form_floated_item_right">
                <label class="label" for="cbTipoHerramienta">Frecuencia de calibración</label>
                <div class="input_wrapper input_wrapper_selectbox">
                    <div class="cap">
                        <span id="lblFrecuencia" runat="server"></span>
                        <select id="cbFrecuencia" runat="server"></select>
                    </div>
                </div>
            </li>
        </ul>
        <div class="clear"></div>

        <div class="form_buttons_container">
            <ul class="button_list">
                <li id="btnGuardar"><div class="btn primary_action_button button_100"><div class="cap"><span>Guardar</span></div></div></li>
                <li id="btnCancelar"><div class="btn secondary_action_button button_100"><div class="cap"><span>Cancelar</span></div></div></li>
            </ul>
        </div>
    </div>

</asp:Content>

