<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="viajeAdmin.aspx.cs" Inherits="general_viajeAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $('#contentPlacePage_chkCheque').click(function () {
                if ($(this).is(':checked')) {
                    $('#contentPlacePage_txtAlaOrden').removeAttr('readonly');
                    $('#contentPlacePage_txtAlaOrden').removeAttr('disabled');
                }
                else {
                    $('#contentPlacePage_txtAlaOrden').val('');
                    $('#contentPlacePage_txtAlaOrden').attr('readonly', 'readonly');
                    $('#contentPlacePage_txtAlaOrden').attr('disabled', 'disabled');
                }
            });
            <% if (this.SV == null) { %>
            $('#contentPlacePage_txtFechaCumplimiento').datepicker({ dayNames: dayNames, dayNamesMin: dayNamesMin, monthNames: monthNames, dateFormat: 'dd/mm/yy', minDate: '<%= this.FechaInicio %>' });
            $('#contentPlacePage_txtFechaLimite').datepicker({ dayNames: dayNames, dayNamesMin: dayNamesMin, monthNames: monthNames, dateFormat: 'dd/mm/yy', minDate: '<%= this.FechaInicio %>' });
            $('#contentPlacePage_txtHoraCumplimiento').timepicker({	timeOnlyTitle: 'Seleccionar hora', timeText: '', hourText: 'Hora',	minuteText: 'Minutos', secondText: 'Segundos', currentText: 'Actual', closeText: 'Listo' });
            $('#contentPlacePage_txtHoraLimite').timepicker({	timeOnlyTitle: 'Seleccionar hora', timeText: '', hourText: 'Hora',	minuteText: 'Minutos', secondText: 'Segundos', currentText: 'Actual', closeText: 'Listo' });

            $('#btnEnviar').click(function () {
                var idVehiculo = $('#contentPlacePage_cbVehiculo');
                var idImportancia = $('#contentPlacePage_cbImportancia');
                var motivo = $('#contentPlacePage_txtMotivo');
                var descripcion = $('#contentPlacePage_txtDescripcion');
                var origen = $('#contentPlacePage_txtOrigen');
                var ruta = $('#contentPlacePage_txtRuta');
                var finRecorrido = $('#contentPlacePage_txtFinRecorrido');
                var fCumplimiento = $('#contentPlacePage_txtFechaCumplimiento');
                var hCumplimiento = $('#contentPlacePage_txtHoraCumplimiento');
                var fLimite = $('#contentPlacePage_txtFechaLimite');
                var hLimite = $('#contentPlacePage_txtHoraLimite');
                var destino = $('#contentPlacePage_txtDestinatario');
                var direccion = $('#contentPlacePage_txtDireccion');
                var localidad = $('#contentPlacePage_txtLocalidad');
                var contacto = $('#contentPlacePage_txtContacto');
                var telefono = $('#contentPlacePage_txtTelefono');
                var hAtencion = $('#contentPlacePage_txtHorarioAtencion');
                var docReferencia = $('#contentPlacePage_txtDocumentoRef');
                var retFac = $('#contentPlacePage_chkRetFac');
                var retRec = $('#contentPlacePage_chkRetRec');
                var retRem = $('#contentPlacePage_chkRetRem');
                var retOtro = $('#contentPlacePage_txtRetOtro');
                var condComer = $('#contentPlacePage_txtCondComer');
                var idImputacion = $('#contentPlacePage_cbImputacion');
                var importe = $('#contentPlacePage_txtImporte');
                var efectivo = $('#contentPlacePage_chkEfectivo');
                var cheque = $('#contentPlacePage_chkCheque');
                var aLaOrden = $('#contentPlacePage_txtAlaOrden');
                var observaciones = $('#contentPlacePage_txtObservaciones');
                var result = true;

                result &= TieneDatos(idVehiculo, 'input_wrapper', 'input_wrapper_selectbox_error');
                result &= TieneDatos(idImportancia, 'input_wrapper', 'input_wrapper_selectbox_error');
                result &= TieneDatos(motivo, 'input_wrapper', 'input_wrapper_error');
                result &= TieneDatos(descripcion, 'input_wrapper', 'input_wrapper_error');
                result &= TieneDatos(origen, 'input_wrapper', 'input_wrapper_error');
                result &= TieneDatos(finRecorrido, 'input_wrapper', 'input_wrapper_error');
                result &= TieneDatos(fCumplimiento, 'input_wrapper', 'input_wrapper_error');
                result &= TieneDatos(fLimite, 'input_wrapper', 'input_wrapper_error');
                result &= TieneDatos(destino, 'input_wrapper', 'input_wrapper_error');
                result &= TieneDatos(direccion, 'input_wrapper', 'input_wrapper_error');
                result &= TieneDatos(localidad, 'input_wrapper', 'input_wrapper_error');
                result &= TieneDatos(contacto, 'input_wrapper', 'input_wrapper_error');
                result &= TieneDatos(telefono, 'input_wrapper', 'input_wrapper_error');
                result &= TieneDatos(hAtencion, 'input_wrapper', 'input_wrapper_error');
                result &= TieneDatos(idImputacion, 'input_wrapper', 'input_wrapper_selectbox_error') && ContieneValorDiferente(idImputacion, '<%= Constantes.IdImputacionInvalida.ToString() %>', 'input_wrapper', 'input_wrapper_selectbox_error');
                result &= ContieneNumeros(importe, 'input_wrapper', 'input_wrapper_error');
                if (idVehiculo.val() == '<%= ((int)VehiculosSolViaje.Flete).ToString() %>') {
                    result &= TieneDatos(observaciones, 'input_wrapper', 'input_wrapper_error');    
                }
                else {
                    QuitarEstilos(observaciones, 'input_wrapper');
                }

                if (result) {
                    MostrarLoading();

                    ConsultaAjax.url = 'viajeAdmin.aspx/NuevaSolicitudViaje';
                    ConsultaAjax.data = JSON.stringify({ idVehiculo: idVehiculo.val(), idImportancia: idImportancia.val(), motivo: motivo.val(), descripcion: descripcion.val(), origen: origen.val(), ruta: ruta.val(), finRecorrido: finRecorrido.val(), fechaCumplimiento: fCumplimiento.val(), horaCumplimiento: hCumplimiento.val(), fechaLimite: fLimite.val(), horaLimite: hLimite.val(), destinatario: destino.val(), direccion: direccion.val(), localidad: localidad.val(), contacto: contacto.val(), telefono: telefono.val(), horarioAtencion: hAtencion.val(), docReferencia: docReferencia.val(), retFactura: (retFac.is(':checked') ? 1 : 0), retRecibo: (retRec.is(':checked') ? 1 : 0), retRemito: (retRem.is(':checked') ? 1 : 0), retOtro: retOtro.val(), condComerciales: condComer.val(), idImputacion: idImputacion.val(), importe: importe.val(), efectivo: (efectivo.is(':checked') ? 1 : 0), cheque: (cheque.is(':checked') ? 1 : 0), aLaOrden: aLaOrden.val(), observaciones: observaciones.val() });
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
            <% if (PuedeAdministrador && this.SV.Estado == EstadosSolViaje.Leida) { %>
            $('#btnAprobar').click(function () {
                MostrarLoading();

                ConsultaAjax.url = 'viajeAdmin.aspx/AprobarSolicitudViaje';
                ConsultaAjax.AjaxSuccess = function (msg) {
                    Mensaje(msg.d, 'success', true, false, 'Aceptar', '', 'OnCerrar()', '');
                };
                ConsultaAjax.AjaxError = function (msg) {
                    ErrorMsg(msg);
                };

                ConsultaAjax.Ejecutar();
            });
            $('#btnRechazar').click(function () {
                Mensaje('¿Desea rechazar la solicitud de viaje?', 'warning', true, true, 'Cancelar', 'Rechazar', 'custom_dialog.close()', 'RechazarSolicitud()');
            });
            <% } %>
            <% if (PuedeAdministrador && this.SV.Estado == EstadosSolViaje.Aprobada) { %>
            $('#btnConfirmar').click(function () {
                MostrarLoading();

                ConsultaAjax.url = 'viajeAdmin.aspx/ConfirmarSolicitudViaje';
                ConsultaAjax.AjaxSuccess = function (msg) {
                    Mensaje(msg.d, 'success', true, false, 'Aceptar', '', 'OnCerrar()', '');
                };
                ConsultaAjax.AjaxError = function (msg) {
                    ErrorMsg(msg);
                };

                ConsultaAjax.Ejecutar();
            });
            <% } %>
            $('#btnCancelar').click(function () {
                Mensaje('Todos los datos ingresados serán descartados, ¿desea continuar?', 'warning', true, true, 'Cancelar', 'Aceptar', 'custom_dialog.close()', 'OnCerrar()');
            });
            <% if (this.SV != null && (this.SV.Estado == EstadosSolViaje.Aprobada || this.SV.Estado == EstadosSolViaje.Confirmada)) { %>
            $('#btnImprimir').click(function () {
                window.open('<%= Encriptacion.GetURLEncriptada("general/viajeImprimir.aspx", "id=" + this.SV.IDViaje) %>', "Imprimir", "status=0, toolbar=0, resizable=0, "
                          + "menubar=0, directories=0, width='680', height:'500', scrollbars=1");
            });
            <% } %>

            $('#contentPlacePage_txtAlaOrden').attr('readonly', 'readonly');
            $('#contentPlacePage_txtAlaOrden').attr('disabled', 'disabled');

            <% if (!this.HorarioPermitido) { %>
            Mensaje('Las solicitudes de viaje pueden ser cargadas hasta las <%=GSolicitudesViaje.MaxHsSolicitud.ToString() %>hs.', 'info', true, false, 'Aceptar', '', 'OnCerrar()', '');
            <% } %>
        });
        
        function OnCerrar() {
            location.href = '<%= Constantes.UrlIntraDefault %>';
        }

        <% if (PuedeAdministrador && this.SV.Estado == EstadosSolViaje.Leida) { %>
        function RechazarSolicitud() {
            MostrarLoading();

            ConsultaAjax.url = 'viajeAdmin.aspx/RechazarSolicitudViaje';
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
    <h1>Solicitud de viaje</h1>
</div>

<div class="form-steps" style="width:600px">
    <ul>
        <li>
            <div class="step-number<%= this.SV == null ? " active" : "" %>">1.</div>
            <div class="step-description<%= this.SV == null ? " active" : "" %>">Solicitud</div>
        </li>
        <li class="separator"></li>
        <li>
            <div class="step-number<%= this.SV != null && this.SV.Estado == EstadosSolViaje.Leida ? " active" : "" %>">2.</div>
            <div class="step-description<%= this.SV != null && this.SV.Estado == EstadosSolViaje.Leida ? " active" : "" %>">Leída</div>
        </li>
        <li class="separator"></li>
        <% if (this.SV != null && this.SV.Estado == EstadosSolViaje.Cancelada) { %>
        <li>
            <div class="step-number<%= this.SV != null && this.SV.Estado == EstadosSolViaje.Cancelada ? " active" : "" %>">3.</div>
            <div class="step-description<%= this.SV != null && this.SV.Estado == EstadosSolViaje.Cancelada ? " active" : "" %>">Cancelada</div>
        </li>
        <% }
           else { %>
        <li>
            <div class="step-number<%= this.SV != null && this.SV.Estado == EstadosSolViaje.Aprobada ? " active" : "" %>">3.</div>
            <div class="step-description<%= this.SV != null && this.SV.Estado == EstadosSolViaje.Aprobada ? " active" : "" %>">Aprobada</div>
        </li>
        <li class="separator"></li>
        <li>
            <div class="step-number<%= this.SV != null && this.SV.Estado == EstadosSolViaje.Confirmada ? " active" : "" %>">4.</div>
            <div class="step-description<%= this.SV != null && this.SV.Estado == EstadosSolViaje.Confirmada ? " active" : "" %>">Confirmada</div>
        </li>
        <% } %>
    </ul>    
</div>

<div class="form_place">
    <% if (this.SV == null) { %>
    <div class="suggestion_message">La solicitud de viaje debe cargarse con un mínimo de <%=GSolicitudesViaje.MinHsAntipo.ToString() %>hs de anticipación.</div>
    <% } %>

    <h3>1. Datos de la solicitud</h3>

    <% if (this.SV == null) { %>
    <p>Los campos marcados con <span class="required"></span> son obligatorios.</p>
    <% } %>
    <br />

    <ul class="middle_form" style="height:<%= this.SV == null ? 110 : 240 %>px">
        <% if (this.SV != null) { %>
        <li class="form_floated_item form_floated_item_half">
            <label class="label">Número</label>
            <span id="lblNumero" runat="server"></span>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label">Fecha de solicitud</label>
            <span id="lblFecha" runat="server">-</span>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label">Fecha de lectura</label>
            <span id="lblFechaLectura" runat="server">-</span>
        </li>
            <% if (SV.Estado != EstadosSolViaje.Cancelada) { %>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label">Fecha de aprobación</label>
            <span id="lblFechaAprobacion" runat="server">-</span>
        </li>
        <li class="form_floated_item form_floated_item_100">
            <label class="label">Fecha de confirmación</label>
            <span id="lblFechaConfirmacion" runat="server">-</span>
        </li>
            <% } 
               else { %>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label">Fecha de cancelación</label>
            <span id="lblFechaCancelacion" runat="server">-</span>
        </li>
            <% } %>
        <% } %>

        <li class="form_floated_item form_floated_item_half">
            <label class="label required" for="txtEquipo">Vehículo</label>
            <div class="input_wrapper input_wrapper_selectbox">
                <div class="cap">
                    <span id="lblVehiculo"></span>
                    <select id="cbVehiculo" runat="server"></select>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label required" for="txtAsunto">Importancia</label>
            <div class="input_wrapper input_wrapper_selectbox">
                <div class="cap">
                    <span id="lblImportancia"></span>
                    <select id="cbImportancia" runat="server"></select>
                </div>
            </div>
        </li>
    </ul>

    <h3>2. Origen</h3>

    <ul class="middle_form" style="height:470px">
        <li class="form_floated_item form_floated_item_full">
            <label class="label required" for="txtMotivo">Motivo del viaje</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtMotivo" value="" maxlength="100" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_full">
            <label class="label required" for="txtDescripcion">Descripción del material / documentación</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtDescripcion" value="" maxlength="100" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_full">
            <label class="label required" for="txtOrigen">Origen</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtOrigen" value="" maxlength="60" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_full">
            <label class="label" for="txtRuta">Ruta</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtRuta" value="" maxlength="35" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_full">
            <label class="label required" for="txtFinRecorrido">Fin del recorrido</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtFinRecorrido" value="" maxlength="65" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label required" for="txtFechaCumplimiento">Fecha de cumplimiento</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtFechaCumplimiento" readonly="readonly" value="" maxlength="10" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label" for="txtHoraCumplimiento">Hora de cumplimiento</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtHoraCumplimiento" value="" maxlength="5" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label required" for="txtFechaCumplimiento">Fecha límite</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtFechaLimite" readonly="readonly" value="" maxlength="10" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label" for="txtHoraCumplimiento">Hora límite de la prestación</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtHoraLimite" value="" maxlength="5" type="text" runat="server"/>
                </div>
            </div>
        </li>
    </ul>

    <h3>3. Destino</h3>

    <ul class="middle_form" style="height:490px">
        <li class="form_floated_item form_floated_item_full">
            <label class="label required" for="txtDestinatario">Destinatario</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtDestinatario" value="" maxlength="100" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_full">
            <label class="label required" for="txtDireccion">Dirección</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtDireccion" value="" maxlength="40" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label required" for="txtLocalidad">Localidad</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtLocalidad" value="" maxlength="30" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label required" for="txtContacto">Contacto</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtContacto" value="" maxlength="40" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label required" for="txtTelefono">Teléfono</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtTelefono" value="" maxlength="20" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label required" for="txtHorarioAtencion">Horario de atención</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtHorarioAtencion" value="" maxlength="40" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label" for="txtDocumentoRef">Documento de referencia</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtDocumentoRef" value="" maxlength="40" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_100">
            <label class="label" for="txtDocumentoRef">Debe retornar con</label>
            <ul class="chkList">
                <li><input id="chkRetFac" type="checkbox" runat="server" />Factura</li>
                <li><input id="chkRetRem" type="checkbox" runat="server" />Remito</li>
                <li><input id="chkRetRec" type="checkbox" runat="server" />Recibo</li>
            </ul>
            Otro
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtRetOtro" value="" maxlength="40" type="text" runat="server"/>
                </div>
            </div>
        </li>
    </ul>

    <h3>4. Condiciones comerciales (si corresponde)</h3>

    <ul class="middle_form" style="height:255px">
        <li class="form_floated_item form_floated_item_full">
            <label class="label" for="txtCondComer">Condición comercial</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtCondComer" value="" maxlength="40" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label required" for="cbImputacion">Imputación</label>
            <div class="input_wrapper input_wrapper_selectbox">
                <div class="cap">
                    <span id="lblImputacion"></span>
                    <select id="cbImputacion" runat="server"></select>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label" for="txtImporte">Importe ($)</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtImporte" value="" maxlength="6" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label" for="txtCondComer"><input type="checkbox" id="chkEfectivo" runat="server" />Efectivo</label>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label" for="txtAlaOrden"><input type="checkbox" id="chkCheque" runat="server" />Cheque</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtAlaOrden" value="" maxlength="28" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_full">
            <label class="label required" for="txtObservaciones">Observaciones</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtObservaciones" value="(Pesado / Liviano) [KG], (Medidas paquete), (Requiere vehiculo: Si / No)" maxlength="100" type="text" runat="server"/>
                </div>
            </div>
        </li>
    </ul>

    <div class="form_buttons_container">
        <ul class="button_list">
            <% if (this.SV == null && this.HorarioPermitido) { %>
            <li id="btnEnviar"><div class="btn primary_action_button button_100"><div class="cap"><span>Enviar</span></div></div></li>
            <% } %>
            <% if (PuedeAdministrador && this.SV.Estado == EstadosSolViaje.Leida) { %>
            <li id="btnAprobar"><div class="btn primary_action_button button_100"><div class="cap"><span>Aprobar</span></div></div></li>
            <li id="btnRechazar"><div class="btn secondary_action_button button_100"><div class="cap"><span>Rechazar</span></div></div></li>
            <% } %>
            <% if (PuedeAdministrador && this.SV.Estado == EstadosSolViaje.Aprobada) { %>
            <li id="btnConfirmar"><div class="btn primary_action_button button_100"><div class="cap"><span>Confirmar</span></div></div></li>
            <% } %>
            <% if (this.SV == null || (PuedeAdministrador && (this.SV.Estado != EstadosSolViaje.Aprobada || this.SV.Estado != EstadosSolViaje.Cancelada))) { %>
            <li id="btnCancelar"><div class="btn secondary_action_button button_100"><div class="cap"><span>Cancelar</span></div></div></li>
            <% } %>
            <% if (this.SV != null && (this.SV.Estado == EstadosSolViaje.Aprobada || this.SV.Estado == EstadosSolViaje.Confirmada)) { %>
            <li id="btnImprimir"><div class="btn primary_action_button button_100"><div class="cap"><span>Imprimir</span></div></div></li>
            <% } %>
        </ul>
    </div>
</div>

</asp:Content>

