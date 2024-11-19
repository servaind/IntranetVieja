<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="autorizAdmin.aspx.cs" Inherits="general_autorizAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">

<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        $('#btnAprobar').click(function () {
            Actualizar(true);
        });
        $('#btnRechazar').click(function () {
            MostrarVentana('divMotivo');
        });
        $('#btnAutorizAceptar').click(function () {
            Actualizar(false);
        });
        $('#btnAutorizCancelar').click(function () {
            CerrarVentana();
        });
        $('#btnSalir').click(function () {
            OnCerrar();
        });
    });

    function Actualizar(aprobar) {
        var motivo = $('#txtMotivoRechazo');
        var result = true;

        if (!aprobar) {
            result &= TieneDatos(motivo, 'textarea_wrapper', 'textarea_wrapper_error');
        }

        if (result) {
            MostrarLoading();

            ConsultaAjax.url = 'autorizAdmin.aspx/' + (aprobar ? 'Aprobar' : 'Rechazar') + 'Solicitud';
            if (!aprobar) {
                ConsultaAjax.data = JSON.stringify({ motivo: motivo.val() });
            }
            ConsultaAjax.AjaxSuccess = function (msg) {
                Mensaje(msg.d, 'success', true, false, 'Aceptar', '', 'OnCerrar()', '');
            };
            ConsultaAjax.AjaxError = function (msg) {
                if (!aprobar) {
                    ult_ventana = 'divMotivo';
                }
                ErrorMsg(msg);
            };

            ConsultaAjax.Ejecutar();
        }
    }

    function OnCerrar() {
        location.href = '<%= Constantes.UrlIntraDefault %>';
    }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlacePage" Runat="Server">

<div class="page-title">
    <h1>Solicitud de autorización</h1>
</div>

<div class="form-steps" style="width:340px">
    <ul>
        <li>
            <div class="step-number<%= this.AU.Estado == EstadoAutorizacion.Pendiente ? " active" : "" %>">1.</div>
            <div class="step-description<%= this.AU.Estado == EstadoAutorizacion.Pendiente ? " active" : "" %>">Pendiente</div>
        </li>
        <li class="separator"></li>
        <% if (this.AU.Estado == EstadoAutorizacion.Aprobada) { %>
        <li>
            <div class="step-number active">2.</div>
            <div class="step-description active">Aprobada</div>
        </li>
        <% } %>
        <% else if (this.AU.Estado == EstadoAutorizacion.Rechazada) { %>
        <li>
            <div class="step-number active">2.</div>
            <div class="step-description active">Rechazada</div>
        </li>
        <% } %>
        <% else { %>
        <li>
            <div class="step-number">2.</div>
            <div class="step-description">Decisión</div>
        </li>
        <% } %>
    </ul>    
</div>

<div class="form_place">
    <h3>Datos de la solicitud</h3>

    <ul class="middle_form" style="height:<%=this.AU.Estado == EstadoAutorizacion.Pendiente ? "250" : "300" %>px">
        <li class="form_floated_item form_floated_item_half">
            <label class="label">Número</label>
            <span id="lblNumero" runat="server"></span>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label">Emitida por</label>
            <span id="lblEmitidaPor" runat="server"></span>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label">Fecha de solicitud</label>
            <span id="lblFechaSol" runat="server"></span>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label">Responsable</label>
            <span id="lblResponsable" runat="server"></span>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label">Referencia</label>
            <span id="lblReferencia" runat="server"></span>
        </li>
        <li class="form_floated_item form_floated_item_full">
            <label class="label">Motivo de la autorización</label>
            <span id="lblMotivoAutoriz" runat="server"></span>
        </li>

        <% if (this.AU.Estado != EstadoAutorizacion.Pendiente) { %>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label">Fecha de autorización (<%=this.AU.Estado == EstadoAutorizacion.Aprobada ? "aprobada" : "rechazada"%>)</label>
            <span id="lblFechaAutoriz" runat="server"></span>
        </li>
        <% if (this.AU.Estado == EstadoAutorizacion.Rechazada) { %>
        <li class="form_floated_item form_floated_item_full">
            <label class="label">Motivo del rechazo</label>
            <span id="lblMotivoRechazo" runat="server"></span>
        </li>
        <% } %>
        <% } %>
    </ul>
        
    <div class="form_buttons_container">
        <ul class="button_list">
            <% if(this.PuedeAutorizar) { %>
            <li id="btnAprobar"><div class="btn primary_action_button button_100"><div class="cap"><span>Aprobar</span></div></div></li>
            <li id="btnRechazar"><div class="btn secondary_action_button button_100"><div class="cap"><span>Rechazar</span></div></div></li>
            <% } else { %>
            <li id="btnSalir"><div class="btn secondary_action_button button_100"><div class="cap"><span>Salir</span></div></div></li>
            <% } %>
        </ul>
    </div>
</div>

<div class="dialog_wrapper" style="width:600px" id="divMotivo">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3>Rechazo de solicitud</h3></div></div></div>
    </div>

    <div class="dialog_content">
        <ul class="middle_form">
            <li class="form_floated_item form_floated_item_100">
                <label class="label" for="txtMotivoAutoriz">Motivo del rechazo:</label>
                <div class="textarea_wrapper"> 
	                <div class="top">
		                <div class="cap"></div>
	                </div>
	                <div class="inner">
		                <div class="cap">
			                <textarea id="txtMotivoRechazo" maxlength="200" onkeyup="return MaxLength(this)"></textarea>     
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
            <li id="btnAutorizAceptar"><div class="btn primary_action_button_small button_100"><div class="cap"><span>Aceptar</span></div></div></li>
            <li id="btnAutorizCancelar"><div class="btn secondary_action_button_small button_100"><div class="cap"><span>Cancelar</span></div></div></li>
        </ul>
    </div>
</div>

</asp:Content>

