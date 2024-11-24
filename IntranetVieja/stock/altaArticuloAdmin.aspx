<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="altaArticuloAdmin.aspx.cs" Inherits="stock_altaArticuloAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">

<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        $('#contentPlacePage_cbUnidadMedida').change();

        <% if (PuedeSolicitante) { %>
        $('#btnEnviar').click(function () {
            var descripcionArticulo = $('#contentPlacePage_txtDescripcionArticulo');
            var idUnidadMedida = $('#contentPlacePage_cbUnidadMedida');
            var codigoArticulo = $('#contentPlacePage_txtCodigoArticulo');
            var descripcionUso = $('#contentPlacePage_txtDescripcionUso');
            var result = true;

            result &= TieneDatos(descripcionArticulo, 'input_wrapper', 'input_wrapper_error');
            result &= TieneDatos(idUnidadMedida, 'input_wrapper', 'input_wrapper_selectbox_error');
            result &= TieneDatos(codigoArticulo, 'input_wrapper', 'input_wrapper_error');
            result &= TieneDatos(descripcionUso, 'textarea_wrapper', 'textarea_wrapper_error');

            if (result) {
                MostrarLoading();

                <% if (this.CA == null) { %>
                ConsultaAjax.url = 'altaArticuloAdmin.aspx/NuevaAltaArticulo';
                ConsultaAjax.data = JSON.stringify({ descripcionArticulo : descripcionArticulo.val(), idUnidadMedida: idUnidadMedida.val(), codigoArticulo: codigoArticulo.val(), descripcionUso: descripcionUso.val() });
                ConsultaAjax.AjaxSuccess = function (msg) {
                    Mensaje(msg.d, 'success', true, false, 'Aceptar', '', 'OnCerrar()', '');
                };
                ConsultaAjax.AjaxError = function (msg) {
                    ErrorMsg(msg);
                };

                ConsultaAjax.Ejecutar();
                <% }
                else { %>
                GuardarAltaArticulo();
                <% } %>
            }
        });
        <% } %>

        <% if (this.CA != null && (PuedeAdministrador || PuedeSolicitante)) { %>
        $('#btnGuardar').click(function () {
            GuardarAltaArticulo(false);
        });
        <% } %>

        <% if (PuedeAdministrador) { %>
        $('#btnAprobar').click(function () {
            Mensaje('Una vez aprobada, la solicitud no podrá ser modificada nuevamente. ¿Desea continuar?', 'warning', true, true, 'Cancelar', 'Aprobar', 'custom_dialog.close()', 'OnAprobar()');
        });
        <% } %>

        <% if (PuedeAdministrador) { %>
        $('#btnRechazar').click(function () {
            MostrarVentana('divRechazar');
        });
        $('#btnNoCorresponde').click(function () {
            MostrarVentana('divNoCorresponde');
        });
        <% } %>

        <% if (PuedeAdministrador) { %>
        $('#btnRechazarAceptar').click(function () {
            var motivo = $('#txtRechazarMotivo');
            var result = true;

            result &= TieneDatos(motivo, 'textarea_wrapper', 'textarea_wrapper_error');

            if (result) {
                MostrarLoading();

                ConsultaAjax.url = 'altaArticuloAdmin.aspx/RechazarAltaArticulo';
                ConsultaAjax.data = JSON.stringify({ motivo: motivo.val() });
                ConsultaAjax.AjaxSuccess = function (msg) {
                    Mensaje(msg.d, 'success', true, false, 'Aceptar', '', 'OnCerrar()', '');
                };
                ConsultaAjax.AjaxError = function (msg) {
                    ErrorMsg(msg);
                };

                ConsultaAjax.Ejecutar();
            }
        });
        $('#btnNoCorrespondeAceptar').click(function () {
            var motivo = $('#txtRechazarMotivo');
            var result = true;

            result &= TieneDatos(motivo, 'textarea_wrapper', 'textarea_wrapper_error');

            if (result) {
                MostrarLoading();

                ConsultaAjax.url = 'altaArticuloAdmin.aspx/NoCorrespondeAltaArticulo';
                ConsultaAjax.data = '{ "motivo":"' + motivo.val() + '" }';
                ConsultaAjax.AjaxSuccess = function (msg) {
                    Mensaje(msg.d, 'success', true, false, 'Aceptar', '', 'OnCerrar()', '');
                };
                ConsultaAjax.AjaxError = function (msg) {
                    ErrorMsg(msg);
                };

                ConsultaAjax.Ejecutar();
            }
        });
        <% } %>

        $('#btnCancelar').click(function () {
            Mensaje('Todos los datos ingresados serán descartados, ¿desea continuar?', 'warning', true, true, 'Cancelar', 'Aceptar', 'custom_dialog.close()', 'OnCancel()');
        });
    });

    function OnAprobar(){
        custom_dialog.close();
        GuardarAltaArticulo(true);
    }

    function GuardarAltaArticulo(<% if (PuedeAdministrador) { %>aprobar<% } %>) {
        var descripcionArticulo = $('#contentPlacePage_txtDescripcionArticulo');
        var idUnidadMedida = $('#contentPlacePage_cbUnidadMedida');
        var codigoArticulo = $('#contentPlacePage_txtCodigoArticulo');
        var descripcionUso = $('#contentPlacePage_txtDescripcionUso');
        var result = true;

        result &= TieneDatos(descripcionArticulo, 'input_wrapper', 'input_wrapper_error');
        result &= TieneDatos(idUnidadMedida, 'input_wrapper', 'input_wrapper_selectbox_error');
        result &= TieneDatos(codigoArticulo, 'input_wrapper', 'input_wrapper_error');
        result &= TieneDatos(descripcionUso, 'textarea_wrapper', 'textarea_wrapper_error');

        if (result) {
            MostrarLoading();

            ConsultaAjax.url = 'altaArticuloAdmin.aspx/<% if (PuedeAdministrador) { %>' + (aprobar ? "Aprobar" : "Guardar") + '<% } else { %>Guardar<% } %>AltaArticulo';
            ConsultaAjax.data = JSON.stringify({ descripcionArticulo : descripcionArticulo.val(), idUnidadMedida: idUnidadMedida.val(), codigoArticulo: codigoArticulo.val(), descripcionUso: descripcionUso.val() });
            ConsultaAjax.AjaxSuccess = function (msg) {
                Mensaje(msg.d, 'success', true, false, 'Aceptar', '', 'OnCerrar()', '');
            };
            ConsultaAjax.AjaxError = function (msg) {
                ErrorMsg(msg);
            };

            ConsultaAjax.Ejecutar();
        }
    }

    function OnCancel() {
        location.href = '<%= Constantes.UrlIntraDefault %>';
    }

    function OnCerrar() {
        location.href = '<%= Constantes.UrlIntraDefault %>';
    }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlacePage" Runat="Server">

<div class="page-title">
    <h1>Solicitud de alta de artículo</h1>
</div>

<div class="form-steps" style="width:<%= (this.CA == null || this.CA.Estado != EstadosCodArt.Rechazado) ? 400 : 550 %>px">
    <ul>
        <li>
            <div class="step-number<%= this.CA == null ? " active" : "" %>">1.</div>
            <div class="step-description<%= this.CA == null ? " active" : "" %>">Solicitud</div>
        </li>
        <li class="separator"></li>
        <% if (this.CA != null && this.CA.Estado == EstadosCodArt.Rechazado) { %>
        <li>
            <div class="step-number<%= this.CA != null && this.CA.Estado == EstadosCodArt.Rechazado ? " active" : "" %>">R</div>
            <div class="step-description<%= this.CA != null && this.CA.Estado == EstadosCodArt.Rechazado ? " active" : "" %>">Rechazada</div>
        </li>
        <li class="separator"></li>
        <% } %>
        <li>
            <div class="step-number<%= this.CA != null && this.CA.Estado == EstadosCodArt.Revision ? " active" : "" %>">2.</div>
            <div class="step-description<%= this.CA != null && this.CA.Estado == EstadosCodArt.Revision ? " active" : "" %>">Revisión</div>
        </li>
        <li class="separator"></li>
        <li>
            <div class="step-number<%= this.CA != null && this.CA.Estado == EstadosCodArt.Aprobado ? " active" : "" %>">3.</div>
            <div class="step-description<%= this.CA != null && this.CA.Estado == EstadosCodArt.Aprobado ? " active" : "" %>">Aprobado</div>
        </li>
    </ul>    
</div>

<div class="form_place">
    <h3>Datos del artículo</h3>

    <ul class="middle_form" style="height:<%= (this.CA != null) ? 300 : 250 %>px">
        <li class="form_floated_item form_floated_item_full">
            <label class="label" for="txtDescripcionArticulo">Descripción del artículo (máx 50 caracteres)</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtDescripcionArticulo" value="" type="text" maxlength="50" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label" for="cbUnidadMedida">Unidad de medida</label>
            <div class="input_wrapper input_wrapper_selectbox">
                <div class="cap">
                    <span id="lblUnidadMedida" runat="server"></span>
                    <select id="cbUnidadMedida" runat="server">
                    </select>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label" for="txtCodigoArticulo">Código del artículo (15 caracteres)</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtCodigoArticulo" value="" type="text" maxlength="15" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_full">
            <label class="label" for="txtDescripcionUso">Descripción del uso (máx 300 caracteres)</label>
            <div class="textarea_wrapper clear"> 
	            <div class="top">
		            <div class="cap"></div>
	            </div>
	            <div class="inner">
		            <div class="cap">
			            <textarea id="txtDescripcionUso" maxlength="300" onkeyup="return MaxLength(this)" runat="server"></textarea>     
		            </div>
	            </div>
	            <div class="bottom">
		            <div class="cap"></div>
	            </div>
            </div>
        </li>
        <% if (this.CA != null) { %>
        <li class="form_floated_item form_floated_item_half">
            <label class="label">Solicitó</label>
            <span id="lblSolicito" runat="server"></span>
        </li>
        <% } %>
        <% if (this.CA != null && this.CA.Estado == EstadosCodArt.Aprobado) { %>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label">Fecha de aprobación</label>
            <span id="lblFechaAprobado" runat="server"></span>
        </li>
        <% } %>
    </ul>

    <% if (PuedeAdministrador || PuedeSolicitante) { %>
    <div class="form_buttons_container">
        <ul class="button_list">
            <% if (PuedeSolicitante){ %>
            <li id="btnEnviar"><div class="btn primary_action_button button_100"><div class="cap"><span>Enviar</span></div></div></li>
            <% } %>
            <% if (PuedeAdministrador) { %>
            <li id="btnGuardar"><div class="btn primary_action_button button_100"><div class="cap"><span>Guardar</span></div></div></li>
            <% } %>
            <% if (PuedeAdministrador) { %>
            <li id="btnAprobar"><div class="btn secondary_action_button button_100"><div class="cap"><span>Aprobar</span></div></div></li>
            <% } %>
            <% if (PuedeAdministrador) { %>
            <li id="btnRechazar"><div class="btn secondary_action_button button_100"><div class="cap"><span>Rechazar</span></div></div></li>
            <li id="btnNoCorresponde"><div class="btn secondary_action_button button_150"><div class="cap"><span>No corresponde</span></div></div></li>
            <% } %>
            <% if (PuedeAdministrador || PuedeSolicitante) { %>
            <li id="btnCancelar"><div class="btn secondary_action_button button_100"><div class="cap"><span>Cancelar</span></div></div></li>
            <% } %>
        </ul>
    </div>
    <% } %>
</div>

<div class="dialog_wrapper" style="width:500px" id="divRechazar">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3>Rechazar solicitud</h3></div></div></div>
    </div>

    <div class="dialog_content">
        <p>Debe ingresar un motivo para el rechazo de la solicitud:</p><br />
        <ul class="middle_form">
            <li class="form_floated_item" style="width:460px">
                <div class="textarea_wrapper clear"> 
	                <div class="top">
		                <div class="cap"></div>
	                </div>
	                <div class="inner">
		                <div class="cap">
			                <textarea id="txtRechazarMotivo" maxlength="300" onkeyup="return MaxLength(this)"></textarea>     
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
            <li><div class="btn primary_action_button_small button_100" onclick="CerrarVentana()"><div class="cap"><span>Cancelar</span></div></div></li>
            <li><div class="btn secondary_action_button_small button_100" id="btnRechazarAceptar"><div class="cap"><span>Rechazar</span></div></div></li>
        </ul>
    </div>
</div>

<div class="dialog_wrapper" style="width:500px" id="divNoCorresponde">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3>Rechazar solicitud</h3></div></div></div>
    </div>

    <div class="dialog_content">
        <p>Debe ingresar un motivo para el rechazo de la solicitud:</p><br />
        <ul class="middle_form">
            <li class="form_floated_item" style="width:460px">
                <div class="textarea_wrapper clear"> 
	                <div class="top">
		                <div class="cap"></div>
	                </div>
	                <div class="inner">
		                <div class="cap">
			                <textarea id="txtNoCorrespondeMotivo" maxlength="300" onkeyup="return MaxLength(this)"></textarea>     
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
            <li><div class="btn primary_action_button_small button_100" onclick="CerrarVentana()"><div class="cap"><span>Cancelar</span></div></div></li>
            <li><div class="btn secondary_action_button_small button_100" id="btnNoCorrespondeAceptar"><div class="cap"><span>Aceptar</span></div></div></li>
        </ul>
    </div>
</div>

</asp:Content>

