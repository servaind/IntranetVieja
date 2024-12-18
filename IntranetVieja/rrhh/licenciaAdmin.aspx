﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="licenciaAdmin.aspx.cs" Inherits="rrhh_licenciaAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">

<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        <% if (this.Licencia == null) { %>
        $('#contentPlacePage_txtFechaDesde').datepicker({ dayNames: dayNames, dayNamesMin: dayNamesMin, monthNames: monthNames, dateFormat: 'dd/mm/yy'/*, minDate: '<%= this.FechaInicio %>'*/ });
        $('#contentPlacePage_txtFechaHasta').datepicker({ dayNames: dayNames, dayNamesMin: dayNamesMin, monthNames: monthNames, dateFormat: 'dd/mm/yy'/*, minDate: '<%= this.FechaInicio %>'*/ });

        $('#btnEnviar').click(function () {
            var motivo = $('#contentPlacePage_cbMotivo');
            var desde = $('#contentPlacePage_txtFechaDesde');
            var hasta = $('#contentPlacePage_txtFechaHasta');
            var observaciones = $('#contentPlacePage_txtObservaciones');
            var result = true;

            result &= TieneDatos(motivo, 'input_wrapper', 'input_wrapper_selectbox_error');
            result &= ContieneFecha(desde, 'input_wrapper', 'input_wrapper_error');
            result &= ContieneFecha(hasta, 'input_wrapper', 'input_wrapper_error');
            result &= TieneDatos(observaciones, 'textarea_wrapper', 'textarea_wrapper_error');

            if (result) {
                MostrarLoading();

                ConsultaAjax.url = 'licenciaAdmin.aspx/NuevaLicencia';
                ConsultaAjax.data = JSON.stringify({ motivo: motivo.val(), desde: desde.val(), hasta: hasta.val(), observaciones: observaciones.val() });
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
        <% if (PuedeResponsable || PuedeRRHH) { %>
        $('#btnAprobar').click(function () {
            MostrarLoading();

            ConsultaAjax.url = 'licenciaAdmin.aspx/Aprobar<%= PuedeResponsable ? "Responsable" : "RRHH" %>';
            ConsultaAjax.AjaxSuccess = function (msg) {
                Mensaje(msg.d, 'success', true, false, 'Aceptar', '', 'OnCerrar()', '');
            };
            ConsultaAjax.AjaxError = function (msg) {
                ErrorMsg(msg);
            };

            ConsultaAjax.Ejecutar();
        });
        $('#btnRechazar').click(function () {
            Mensaje('¿Desea rechazar la solicitud de licencia?', 'warning', true, true, 'Cancelar', 'Rechazar', 'custom_dialog.close()', 'OnRechazarSolicitud()');
        });
        <% } %>
        $('#btnCancelar').click(function () {
            OnCerrar();
        });
    });

    function OnCerrar() {
        location.href = '<%= Constantes.UrlIntraDefault %>';
    }

    <% if (PuedeResponsable || PuedeRRHH) { %>
    function OnRechazarSolicitud() {
        custom_dialog.close();
        
        MostrarLoading();

        ConsultaAjax.url = 'licenciaAdmin.aspx/Rechazar<%= PuedeResponsable ? "Responsable" : "RRHH" %>';
        ConsultaAjax.AjaxSuccess = function (msg) {
            Mensaje(msg.d, 'success', true, false, 'Aceptar', '', 'OnCerrar()', '');
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
    <h1>Solicitud de licencia</h1>
</div>

<div class="form-steps" style="min-width: 450px; margin-left:200px;">
    <ul>
        <li>
            <div class="step-number<%= this.Licencia == null || this.Licencia.EstadoAutorizacion == EstadosLicencia.NoRecibida ? " active" : "" %>">1.</div>
            <div class="step-description<%= this.Licencia == null || this.Licencia.EstadoAutorizacion == EstadosLicencia.NoRecibida ? " active" : "" %>">Solicitud</div>
        </li>
        <li class="separator"></li>
        <% if (this.Licencia == null || (this.Licencia != null && this.Licencia.EstadoAutorizacion != EstadosLicencia.RechazadaResponsable)) { %>
        <li>
            <div class="step-number<%= (this.Licencia != null && this.Licencia.EstadoAutorizacion == EstadosLicencia.AprobadaResponsable) ? " active" : "" %>">2.</div>
            <div class="step-description<%= (this.Licencia != null && this.Licencia.EstadoAutorizacion == EstadosLicencia.AprobadaResponsable) ? " active" : "" %>">Aprobada<br /> por responsable</div>
        </li>
        <li class="separator"></li>
            <% if (this.Licencia == null || (this.Licencia != null && this.Licencia.EstadoAutorizacion != EstadosLicencia.RechazadaRRHH)) { %>
        <li>
            <div class="step-number<%= (this.Licencia != null && this.Licencia.EstadoAutorizacion == EstadosLicencia.Confirmada) ? " active" : "" %>">3.</div>
            <div class="step-description<%= (this.Licencia != null && this.Licencia.EstadoAutorizacion == EstadosLicencia.Confirmada) ? " active" : "" %>">Confirmada<br /> por rrhh</div>
        </li>
            <% }
               else { %>
        <li>
            <div class="step-number active">3.</div>
            <div class="step-description active">Rechazada<br /> por rrhh</div>
        </li>
            <% } %>
        <% }
           else { %>
        <li>
            <div class="step-number active">2.</div>
            <div class="step-description active">Rechazada<br /> por responsable</div>
        </li>
        <% } %>
    </ul>    
</div>

<div class="form_place" style="min-height: 500px;">
    <h3>Datos de la solicitud</h3>
    <% if (this.Licencia == null) { %>
    <p>Recuerde completar el campo de observaciones con los detalles de la solicitud.</p>
    <br />
    <% } %>
    <ul class="middle_form" style="height:<%= this.Licencia == null ? "250" : "" %>px">
        <% if(this.Licencia != null) { %>
        <li class="form_floated_item form_floated_item_half">
            <label class="label">Solicitante</label>
            <span id="lblSolicito" runat="server"></span>
        </li>
        <% } %>
        <li class="form_floated_item form_floated_item_full">
            <label class="label" for="cbMotivo">Motivo</label>
            <div class="input_wrapper input_wrapper_selectbox">
                <div class="cap">
                    <span id="lblMotivo"></span>
                    <select id="cbMotivo" runat="server"></select>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label" for="txtFechaDesde">Fecha de inicio</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtFechaDesde" value="" maxlength="10" type="text" runat="server" />
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label" for="txtFechaHasta">Fecha de finalización</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtFechaHasta" value="" maxlength="10" type="text" runat="server" />
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_100">
            <label class="label" for="txtNovedadesH">Observaciones</label>
            <div class="textarea_wrapper"> 
	            <div class="top">
		            <div class="cap"></div>
	            </div>
	            <div class="inner">
		            <div class="cap">
			            <textarea id="txtObservaciones" maxlength="300" onkeyup="return MaxLength(this)" runat="server"></textarea>     
		            </div>
	            </div>
	            <div class="bottom">
		            <div class="cap"></div>
	            </div>
            </div>
        </li>
    </ul>
    <% if (this.Licencia == null || PuedeResponsable || PuedeRRHH) { %>
    <div class="form_buttons_container">
        <ul class="button_list">
            <% if (this.Licencia == null) { %>
            <li id="btnEnviar"><div class="btn primary_action_button button_100"><div class="cap"><span>Enviar</span></div></div></li>
            <% } %>
            <% if (PuedeResponsable || PuedeRRHH) { %>
            <li id="btnAprobar"><div class="btn primary_action_button button_100"><div class="cap"><span>Aprobar</span></div></div></li>
            <li id="btnRechazar"><div class="btn secondary_action_button button_100"><div class="cap"><span>Rechazar</span></div></div></li>
            <% } %>
            <li id="btnCancelar"><div class="btn secondary_action_button button_100"><div class="cap"><span>Cancelar</span></div></div></li>
        </ul>
    </div>
    <% } %>
</div>
</asp:Content>

