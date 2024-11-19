<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="notifVentaAdmin.aspx.cs" Inherits="comercial_notifVentaAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">
    <link href="/css/autocomplete.css" rel="stylesheet" type="text/css" />
    <script language="javascript" src="/js/jquery.autocomplete.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        var idVendedor;
        var e = <%=NotifVenta != null ? 1 : 0 %>;
        var busy = false;
		
        $(document).ready(function() {
		    
		    $('#contentPlacePage_txtFechaEntrega').datepicker({ dayNames: dayNames, dayNamesMin: dayNamesMin, monthNames: monthNames, dateFormat: 'dd/mm/yy'});
     
		
            $('#btnCancelar').click(function() {
                Mensaje('Se descartarán los cambios realizados, ¿desea continuar?', 'warning', true, true, 'Descartar', 'Cancelar', 'GoToListado()', 'custom_dialog.close()');
            });

            $('#btnGuardar').click(function() {
                <%=NotifVenta == null || NotifVenta.AbleToVendedor() ? "GuardarVendedor()" : (NotifVenta.AbleToCargaRemito() ? "GuardarRemitoTransporte()" : (NotifVenta.AbleToRem() ? "GuardarRem()" : (NotifVenta.AbleToFac() ? "GuardarFac()" : ""))) %>;
            });

            $('#btnVolver').click(function() {
                GoToListado();
            });

            <% if(NotifVenta != null && NotifVenta.AbleToAprobar()) { %>
            $('#btnAprobar').click(function() {
                AprobarRechazar(true);
            });
            
            $('#btnRechazar').click(function() {
                ShowRechazarWindow();
            });

            $('#btnRechazarAceptar').click(function() {
                AprobarRechazar(false);
            });

            $('#btnRechazarCancelar').click(function() {
                CerrarVentana();
            });
            <% } %>

            <% if(NotifVenta != null) { %>
            $('#lnkDownloadITR').click(function() {
                location.href = '<%=Encriptacion.GetURLEncriptada("download.aspx", "f=" + ITR.GetPathITR(NotifVenta.FileITR) + "&n=" + NotifVenta.FileITR + "&idPath=" + (int)PathDescargas.ITR) %>';
            });
            <% } %>
            
            <% if(NotifVenta != null) { %>
            $('#lnkDownloadOC').click(function() {
                location.href = '<%=Encriptacion.GetURLEncriptada("download.aspx", "f=" + NotifVenta.Imputacion + "&n=" + NotifVenta.Imputacion + "&idPath=" + (int)PathDescargas.OC) %>';
				
			});
            <% } %>

            <% if(NotifVenta == null) { %>
            var options = { serviceUrl: 'notifVentaAdmin.aspx/GetClientes', minChars: 3 };
            $('#txtCliente').autocomplete(options);
            
            options = { serviceUrl: 'notifVentaAdmin.aspx/GetOCs', minChars: 3, onSelect: function(value, data) {
                LoadTemplate();
            } };
            $('#txtOC').autocomplete(options);

            $('#txtImputacionNum').change(function() {
                GetDescripcionImputacion();
            });
            <% } %>

            <% if(NotifVenta == null) { %>
            $('#cbVendedor').change(function() {
                idVendedor = $(this).val();
            });
            
            $('#cbTipoVenta').change(function() {
                if ($(this).val() == '<%=(int)TipoNotifVenta.Producto %>' || $(this).val() == '<%=(int)TipoNotifVenta.ProductoProser %>' || $(this).val() == '<%=(int)TipoNotifVenta.Servicio %>' || $(this).val() == '<%=(int)TipoNotifVenta.ServicioCA %>' || $(this).val() == '<%=(int)TipoNotifVenta.ServicioWD %>' || $(this).val() == '<%=(int)TipoNotifVenta.Obra %>' || $(this).val() == '<%=(int)TipoNotifVenta.RMA %>') {
                    $('#lblVendedor').hide();
                    $('#liVendedor').show();
                    $('#cbVendedor').change();
                } else {
                    idVendedor = '<%=Constantes.Usuario.ID %>';
                    $('#liVendedor').hide();
                    $('#lblVendedor').show();
                }
				
				if($(this).val() == '<%=(int)TipoNotifVenta.ServicioCA %>'){
					$("#li-laboratorio").css("display", "list-item");		
				}else{
					document.getElementById("chkLabExterno").checked = false;			
					$('#chkLabExterno').change();				
					$("#li-laboratorio").css("display", "none");
				}
					
            }).change();
			
			$('#chkLabExterno').change(function(){
			
			    var ischecked = $(this).is(":checked");
				
				if(ischecked)
				{
					document.getElementById('txt-lab-externo').disabled = false;
				}
				else
				{
					document.getElementById('txt-lab-externo').value = '';
					document.getElementById('txt-lab-externo').disabled = true;
				}
			}).change();
            
            $('#cbMoneda').change(function() {
                $('#lblMontoRemitoMoneda').text($(this).find('option:selected').text());
            });
            $('#cbMoneda').change();
            <% } %>
			
			<% if(NotifVenta != null && NotifVenta.CalibracionExterna ) { %>
				$("#li-laboratorio").css("display", "list-item");
			<%}%>
			
            <% if(NotifVenta != null && NotifVenta.AbleToFac()) { %>
            $('#chkFactura').change(function() {
                if ($(this).is(':checked')) {
                    $('#txtFactura').removeAttr('disabled').parents('.input_wrapper').removeClass('input_wrapper_disabled');
                    QuitarEstilos($('#txtFactura'), 'input_wrapper');
                } else {
                    $('#txtFactura').attr('disabled', 'disabled').val('').parents('.input_wrapper').addClass('input_wrapper_disabled');
                }
            });
            <% } %>

		    $('#txtDatosEnvio').change();
            $('#txtObservaciones').change();
            $('#txtRemitoDesc').change();
        });
        
        <% if (NotifVenta == null) { %>
        function LoadTemplate() {
            var oc = $('#txtOC');

            if (oc.val().length == 0) return;

            $.ajax({
                url: 'notifVentaAdmin.aspx/GetTemplate',
                data: JSON.stringify({ oc: oc.val() }),
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                async: true,
                cache: false,
                success: function(msg) {
                    if (msg.d.Success) {
                        $('#cbTipoVenta').val(msg.d.TipoVentaID).change();
                        $('#txtCliente').val(msg.d.Cliente);
                        $('#txtImputacion').val(msg.d.Imputacion);
                        $('#cbMoneda').val(msg.d.TipoMonedaID).change();
                        $('#txtMontoOC').val(msg.d.MontoOC);
                        $('#txtObservaciones').val(msg.d.Observaciones).change();
                        $('#txtRemitoDesc').val(msg.d.RemitoDesc).change();
                    }
                },
                error: function(data, ajaxOptions, thrownError) {

                }
            });
        }
        
        function GetDescripcionImputacion() {
            var numero = $('#txtImputacionNum')

            if (numero.val().length == 0) return;

            $.ajax({
                url: 'notifVentaAdmin.aspx/GetDescripcionImputacion',
                data: JSON.stringify({ numero: numero.val() }),
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                async: true,
                cache: false,
                success: function(msg) {
                    if (msg.d.Success) {
                        $('#txtImputacionDesc').val(msg.d.Descripcion);
                        $('#cbMoneda').focus();
                    } else {
                        ErrorMsg('No se pudo encontrar la descripción para el número de imputación ingresado.');
                        $('#txtImputacionDesc').val('');
                    }
                },
                error: function(data, ajaxOptions, thrownError) {

                }
            });
        }
        <% } %>

        function GoToListado() {
            location.href = 'notifVentaLista.aspx';
        }

        <% if (NotifVenta == null || NotifVenta.AbleToVendedor()) {%>
        function GuardarVendedor() {
		  
			var act = e == 1;
            var tipoVenta = $('#cbTipoVenta');
            var cliente = $('#txtCliente');
            var oc = $('#txtOC');
            var imputacionNum = $('#txtImputacionNum');
            var imputacionDesc = $('#txtImputacionDesc');
            var moneda = $('#cbMoneda');
            var montoOC = $('#txtMontoOC');
			var fechaEntrega = $('#contentPlacePage_txtFechaEntrega');
			var datosEnvio = $('#txtDatosEnvio');
            var observaciones = $('#txtObservaciones');
            var remitoMonto = $('#txtMontoOC');
            var remitoDestino = $('#txtRemitoDestino');
            var remitoContacto = $('#txtRemitoContacto');
            var remitoEntrega = $('#txtRemitoEntrega');
            var remitoDesc = $('#txtObservaciones');
            var confirmar = $('#chkConfirmar').length && $('#chkConfirmar').is(':checked');
			var calibExterna = $('#chkLabExterno').length && $('#chkLabExterno').is(':checked');
			var laboratorio = $('#txt-lab-externo');
            var result = true;
						
            if (!act) {
                result &= TieneDatos(tipoVenta, 'input_wrapper', 'input_wrapper_selectbox_error');
                result &= TieneDatos(cliente, 'input_wrapper', 'input_wrapper_error');
                result &= TieneDatos(oc, 'input_wrapper', 'input_wrapper_error');
				result &= TieneDatos(fechaEntrega, 'input_wrapper', 'input_wrapper_error');
                result &= TieneDatos(imputacionNum, 'input_wrapper', 'input_wrapper_error');
                result &= TieneDatos(imputacionDesc, 'input_wrapper', 'input_wrapper_error');
                result &= TieneDatos(moneda, 'input_wrapper', 'input_wrapper_selectbox_error');

                QuitarEstilos(montoOC, 'input_wrapper');
                if (montoOC.val().length != 0 || (montoOC.val().length == 0 && tipoVenta.val() != <%=(int)TipoNotifVenta.RemitoOficial %> && tipoVenta.val() != <%=(int)TipoNotifVenta.RemitoInterno %>)) {
                    result &= ContieneNumeros(montoOC, 'input_wrapper', 'input_wrapper_error') && ContieneValorDiferente(montoOC, '0', 'input_wrapper', 'input_wrapper_error');
                    if (result) montoOC = montoOC.val();
                } else {
                    montoOC = <%=Constantes.ValorInvalido %>;
                }
                
                QuitarEstilos(remitoMonto, 'input_wrapper');
                if (remitoMonto.val().length != 0 || (remitoMonto.val().length == 0 && tipoVenta.val() != <%=(int)TipoNotifVenta.RemitoOficial %> && tipoVenta.val() != <%=(int)TipoNotifVenta.RemitoInterno %>)) {
                    result &= ContieneNumeros(remitoMonto, 'input_wrapper', 'input_wrapper_error') && ContieneValorDiferente(remitoMonto, '0', 'input_wrapper', 'input_wrapper_error');
                    if (result) remitoMonto = remitoMonto.val();
                } else {
                    remitoMonto = <%=Constantes.ValorInvalido %>;
                }
            } else {
                montoOC = <%=Constantes.ValorInvalido %>;
                remitoMonto = <%=Constantes.ValorInvalido %>;

                if (confirmar) {
                    if (remitoDestino.length) result &= TieneDatos(remitoDestino, 'input_wrapper', 'input_wrapper_error');
                    if (remitoContacto.length) result &= TieneDatos(remitoContacto, 'input_wrapper', 'input_wrapper_error');
                    if (remitoEntrega.length) result &= TieneDatos(remitoEntrega, 'input_wrapper', 'input_wrapper_error');
                }

                result &= TieneDatos(remitoDesc, 'textarea_wrapper', 'textarea_wrapper_error');
            }

            if (result && !busy) {
                busy = true;

                var imputacion = imputacionNum.val() + '-' + imputacionDesc.val();
				
				var dataStr = '';
				
				if(calibExterna)
				{
					dataStr = JSON.stringify({ vendedor: idVendedor, tipoVenta: tipoVenta.val(), cliente: cliente.val(), oc: oc.val(), imputacion: imputacion, moneda: moneda.val(), montoOC: montoOC, fechaEntrega: fechaEntrega.val(), datosEnvio: datosEnvio.val(), observaciones: observaciones.val(), remitoMonto: remitoMonto, remitoDestino: remitoDestino.val(), remitoContacto: remitoContacto.val(), remitoEntrega: remitoEntrega.val(), remitoDesc: remitoDesc.val(), confirmar: confirmar, calibExterna: calibExterna, laboratorio: laboratorio.val()});
				}
				else
				{
					dataStr = JSON.stringify({ vendedor: idVendedor, tipoVenta: tipoVenta.val(), cliente: cliente.val(), oc: oc.val(), imputacion: imputacion, moneda: moneda.val(), montoOC: montoOC, fechaEntrega: fechaEntrega.val(), datosEnvio: datosEnvio.val(), observaciones: observaciones.val(), remitoMonto: remitoMonto, remitoDestino: remitoDestino.val(), remitoContacto: remitoContacto.val(), remitoEntrega: remitoEntrega.val(), remitoDesc: remitoDesc.val(), confirmar: confirmar, calibExterna: calibExterna, laboratorio: laboratorio.val()});
				}
				
                $.ajax({
                    url: 'notifVentaAdmin.aspx/' + (act ? (remitoDestino.length ? 'GuardarRemitoVendedor' : 'GuardarVendedor') : 'Nueva'),
                    data: dataStr,
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    async: true,
                    cache: false,
                    success: function(msg) {
                        if (msg.d.Success) Mensaje(act ? 'Los datos fueron actualizados.' : ('Se ha generado una nueva notificación de venta con el Nº ' + msg.d.Message), 'success', true, false, 'Aceptar', '', 'GoToListado()', '');
                        else ErrorMsg(MsgOperationError + '\nDetalle: ' + msg.d.Message);

                        busy = false;
                    },
                    error: function(data, ajaxOptions, thrownError) {
                        ErrorMsg(MsgOperationError + '\nDetalle: ' + GetAjaxError(data));
                        busy = false;
                    }
                });
            }
        }
        <% } %>
               
        <% if (NotifVenta != null && NotifVenta.AbleToRem()) {%>
        function GuardarRem() {
		  
            var remito = $('#txtRemito');
            var result = true;

            result &= ContieneNumeros(remito, 'input_wrapper', 'input_wrapper_error') && ContieneValorDiferente(remito, '0', 'input_wrapper', 'input_wrapper_error');
            
            if (result && !busy) {
                busy = true;

                $.ajax({
                    url: 'notifVentaAdmin.aspx/GuardarRem',
                    data: JSON.stringify({ remito: remito.val() }),
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    async: true,
                    cache: false,
                    success: function(msg) {
                        if (msg.d.Success) Mensaje('Los datos fueron actualizados.', 'success', true, false, 'Aceptar', '', 'GoToListado()', '');
                        else ErrorMsg(MsgOperationError + '\nDetalle: ' + msg.d.Message);
                        
                        busy = false;
                    },
                    error: function(data, ajaxOptions, thrownError) {
                        ErrorMsg(MsgOperationError + '\nDetalle: ' + GetAjaxError(data));
                        busy = false;
                    }
                });
            }
        }
        <% } %>
        
        <% if (NotifVenta != null && NotifVenta.AbleToFac()) {%>
        function GuardarFac() {
		  
    	  var factura = $('#txtFactura');
          var result = true;
            
            if ($('#chkFactura').is(':checked')) {
                result &= ContieneNumeros(factura, 'input_wrapper', 'input_wrapper_error') && ContieneValorDiferente(factura, '0', 'input_wrapper', 'input_wrapper_error');
                if (result) factura = factura.val();
            } else {
                factura = <%=Constantes.ValorInvalido %>;
            }
            
            if (result && !busy) {
                busy = true;

                $.ajax({
                    url: 'notifVentaAdmin.aspx/GuardarFac',
                    data: JSON.stringify({ factura: factura }),
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    async: true,
                    cache: false,
                    success: function(msg) {
                        if (msg.d.Success) Mensaje('Los datos fueron actualizados.', 'success', true, false, 'Aceptar', '', 'GoToListado()', '');
                        else ErrorMsg(MsgOperationError + '\nDetalle: ' + msg.d.Message);
                        
                        busy = false;
                    },
                    error: function(data, ajaxOptions, thrownError) {
                        ErrorMsg(MsgOperationError + '\nDetalle: ' + GetAjaxError(data));
                        busy = false;
                    }
                });
            }
        }
        <% } %>

        <% if (NotifVenta != null && NotifVenta.AbleToCargaRemito()) {%>
        function GuardarRemitoTransporte() {
		 
            var remitoTransporte = $('#txtRemitoTransporte');
            var result = true;

            result &= TieneDatos(remitoTransporte, 'input_wrapper', 'input_wrapper_error');
            
            if (result && !busy) {
                busy = true;
                
                $.ajax({
                    url: 'notifVentaAdmin.aspx/GuardarRemitoTransporte',
                    data: JSON.stringify({ remitoTransporte: remitoTransporte.val() }),
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    async: true,
                    cache: false,
                    success: function(msg) {
                        if (msg.d.Success) Mensaje('Los datos fueron actualizados.', 'success', true, false, 'Aceptar', '', 'GoToListado()', '');
                        else ErrorMsg(MsgOperationError + '\nDetalle: ' + msg.d.Message);
                        
                        busy = false;
                    },
                    error: function(data, ajaxOptions, thrownError) {
                        ErrorMsg(MsgOperationError + '\nDetalle: ' + GetAjaxError(data));
                        busy = false;
                    }
                });
            }
        }
        <% } %>
        
        <% if (NotifVenta != null && NotifVenta.AbleToAprobar()) {%>
        function ClearRechazarWindow() {
            $('#report-error').hide();
            LimpiarCampo($('#txtRechazoMotivo'), 'textarea_wrapper');
        }

        function ShowRechazarWindow() {
            ClearRechazarWindow();

            MostrarVentana('divRechazar');
            $('#txtRechazoMotivo').focus();
        }

        function AprobarRechazar(aprobar) {           
            var result = true;
            var motivo = $('#txtRechazoMotivo');

            if (!aprobar) {
                ult_ventana = 'divRechazar';
                result &= TieneDatos(motivo, 'textarea_wrapper', 'textarea_wrapper_error');
            }

            if (result && !busy) {
                busy = true;

                $.ajax({
                    url: 'notifVentaAdmin.aspx/AprobarRechazar',
                    data: JSON.stringify({ aprobar: aprobar, motivo: motivo.val() }),
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    async: true,
                    cache: false,
                    success: function(msg) {
                        if (msg.d.Success) Mensaje(aprobar ? 'La venta fue aprobada.' : 'El remito fue rechazado.', 'success', true, false, 'Aceptar', '', 'GoToListado()', '');
                        else ErrorMsg(MsgOperationError + '\nDetalle: ' + msg.d.Message);
                        
                        busy = false;
                    },
                    error: function(data, ajaxOptions, thrownError) {
                        ErrorMsg(MsgOperationError + '\nDetalle: ' + GetAjaxError(data));
                        busy = false;
                    }
                });
            }
        }
        <% } %>
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlacePage" Runat="Server">
    
<div class="page-title">
    <h1><%=NotifVenta == null ? "Alta de" : "Detalle de la" %> venta</h1>
</div>

<% if(NotifVenta != null) { %>
<div class="form-steps" style="width:600px">
    <ul>
        <li>
            <div class="step-number <%=NotifVenta.Estado == EstadoNotifVenta.CargandoDatos ? "active" : "" %>">1.</div>
            <div class="step-description <%=NotifVenta.Estado == EstadoNotifVenta.CargandoDatos ? "active" : "" %>">Cargando datos</div>
        </li>
        <li class="separator"></li>
        <li>
            <div class="step-number <%=NotifVenta.Estado == EstadoNotifVenta.CargandoRemito ? "active" : "" %>">2.</div>
            <div class="step-description <%=NotifVenta.Estado == EstadoNotifVenta.CargandoRemito ? "active" : "" %>">Cargando remito</div>
        </li>
        <li class="separator"></li>
        <li>
            <div class="step-number <%=NotifVenta.Estado == EstadoNotifVenta.EsperandoAprobacion ? "active" : "" %>">3.</div>
            <div class="step-description <%=NotifVenta.Estado == EstadoNotifVenta.EsperandoAprobacion ? "active" : "" %>">Esperando aprobación</div>
        </li>
        <li class="separator"></li>
    </ul>
</div>
<div class="form-steps" style="width:600px">
    <ul>
        <li>
            <div class="step-number <%=NotifVenta.Estado == EstadoNotifVenta.ConfeccionRem ? "active" : "" %>">4.</div>
            <div class="step-description <%=NotifVenta.Estado == EstadoNotifVenta.ConfeccionRem ? "active" : "" %>">Confección de remito</div>
        </li>
        <li class="separator"></li>
        <li>
            <div class="step-number <%=NotifVenta.Estado == EstadoNotifVenta.ConfeccionFac ? "active" : "" %>">5.</div>
            <div class="step-description <%=NotifVenta.Estado == EstadoNotifVenta.ConfeccionFac ? "active" : "" %>">Confección de factura</div>
        </li>
        <li class="separator"></li>
        <% if(NotifVenta.LlevaITR) { %>
        <li>
            <div class="step-number <%=NotifVenta.Estado == EstadoNotifVenta.EsperandoITR ? "active" : "" %>">6.</div>
            <div class="step-description <%=NotifVenta.Estado == EstadoNotifVenta.EsperandoITR ? "active" : "" %>">Esperando carga de ITR</div>
        </li>
        <li class="separator"></li>
    </ul>
</div>
<div class="form-steps" style="width:600px">
    <ul>
        <% } %>
        <li>
            <div class="step-number <%=NotifVenta.Estado == EstadoNotifVenta.Cerrada ? "active" : "" %>"><%=NotifVenta.LlevaITR ? "7" : "6" %>.</div>
            <div class="step-description <%=NotifVenta.Estado == EstadoNotifVenta.Cerrada ? "active" : "" %>">Cerrada</div>
        </li>
    </ul>
</div>
<% } %>

<div class="clear"></div>

<div class="form_place">
    <h3>Datos de la venta</h3>

    <ul class="middle_form">
        <% if (NotifVenta != null) { %>
        <li class="form_floated_item form_floated_item_half">
            <label class="label">Nº de seguimiento</label>
            <span id="lblNotifVentaID"><%=NotifVenta.ID %></span>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label">Fecha de alta OC</label>
            <span id="lblFecha"><%=NotifVenta.FechaOC.ToShortDateString() %></span>
        </li>
        <% } %>

        <li class="form_floated_item form_floated_item_half">
            <label class="label" for="cbVendedor">Vendedor</label>
            <% if (NotifVenta == null) { %>
                <span id="lblVendedor" style="display:none;"><%=Constantes.Usuario.Nombre %></span>
                <div class="input_wrapper input_wrapper_selectbox" id="liVendedor" style="display:none;">
                    <div class="cap">
                        <span></span>
                        <select id="cbVendedor">
                            <% List<DataSourceItem> vendedores = GPersonal.GetPersonas(PermisosPersona.SNV_Vendedor);
                               vendedores.ForEach(v =>
                                    {
                                        %>
                                        <option value="<%=v.ValueField %>"><%=v.TextField %></option>
                                        <%
                                    });
                                %>
                        </select>
                    </div>
                </div>
            <% } else { %>
                <span><%=NotifVenta.Vendedor %></span>
            <% } %>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label" for="cbVendedor">Tipo de venta</label>
            <% if (NotifVenta == null) { %>
                <div class="input_wrapper input_wrapper_selectbox">
                    <div class="cap">
                        <span></span>
                        <select id="cbTipoVenta">
                            <% List<DataSourceItem> tiposVenta = NotifVentas.GetTiposVenta();
                               tiposVenta.ForEach(t =>
                                    {
                                        %>
                                        <option value="<%=t.ValueField %>"><%=t.TextField %></option>
                                        <%
                                    });
                                %>
                        </select>
                    </div>
                </div>
            <% } else { %>
                <span><%=NotifVenta.TipoVentaToString() %></span>
            <% } %>
        </li>
		
		
		<div id="li-laboratorio" style="display:none;">
			<li class="form_floated_item form_floated_item_half">
				<% if (NotifVenta == null) { %>
				<label class="label">
					<input type="checkbox" id="chkLabExterno"> 
					Calibración externa
				</label>
				
				<% } else { %>
					<label class="label" for="txt-lab-externo">Calibración externa</label>
					<% if (NotifVenta.CalibracionExterna) { %>				
						<span>Sí</span>
				    <% } %>
				<% } %>
			</li>
			<li class="form_floated_item form_floated_item_half form_floated_item_right">
				<% if (NotifVenta == null) { %>
				<label class="label" for="txt-lab-externo">Laboratorio externo</label>
				<div class="input_wrapper">
					<div class="cap">
						<input id="txt-lab-externo" value="" maxlength="200" type="text"/>
					</div>
				</div>
				<% } else { %>
					<% if (NotifVenta.CalibracionExterna) { %>
						<label class="label" for="txt-lab-externo">Laboratorio externo</label>
						<span><%=NotifVenta.LaboratorioExterno %></span>
				    <% } %>
					
				<% } %>
			</li>
		</div>
		
		
        <li class="form_floated_item form_floated_item_full">
            <label class="label" for="txtCliente">Cliente</label>
            <% if (NotifVenta == null) { %>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtCliente" value="" type="text" maxlength="60"/>
                    </div>
                </div>
            <% } else { %>
                <span><%=NotifVenta.Cliente %></span>
            <% } %>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label" for="txtCliente">OC</label>
            <% if (NotifVenta == null) { %>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtOC" value="" type="text" maxlength="20"/>
                    </div>
                </div>
            <% } else { %>
                <span><%=NotifVenta.OC %></span>
            <% } %>
        </li>
        <% if (NotifVenta != null && NotifVenta.LlevaFileOC) { %>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label" for="txtCliente">Descarga de OC</label>
            <% if(NotifVenta.TieneFileOC) { %>
            <div class="tag_element green hand" id="lnkDownloadOC"><span>OC cargada: presione aquí para descargar</span></div>
            <% } else { %>
            <div class="tag_element yellow"><span>Esperando carga de OC...</span></div>
            <% } %>
        </li>
        <% } %>
        <li class="form_floated_item form_floated_item_full">
            <label class="label" for="txtCliente">Imputación <%=NotifVenta == null ? "(ingrese sólo el número)" : "" %></label>
            <% if (NotifVenta == null) { %>
                <div class="input_wrapper" style="width:10%; display:block;float:left">
                    <div class="cap">
                        <input id="txtImputacionNum" value="" type="text" maxlength="5"/>
                    </div>
                </div>
                <div class="input_wrapper" style="margin-left:5px;display:block;float:left;width:89%">
                    <div class="cap">
                        <input id="txtImputacionDesc" readonly="readonly" value="" type="text" maxlength="93"/>
                    </div>
                </div>
            <% } else { %>
                <span><%=NotifVenta.Imputacion %></span>
            <% } %>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label" for="cbVendedor">Monto OC</label>
            <% if (NotifVenta == null) { %>
                <div class="input_wrapper input_wrapper_selectbox" style="width:20%; display:block;float:left">
                    <div class="cap">
                        <span></span>
                        <select id="cbMoneda">
                            <% List<DataSourceItem> monedas = NotifVentas.GetMonedas();
                               monedas.ForEach(m =>
                                    {
                                        %>
                                        <option value="<%=m.ValueField %>"><%=m.TextField %></option>
                                        <%
                                    });
                                %>
                        </select>
                    </div>
                </div>
                <div class="input_wrapper" style="width:50%;margin-left:5px; display:block;float:left">
                    <div class="cap">
                        <input id="txtMontoOC" class="number" value="" type="text" maxlength="12"/>
                    </div>
                </div>
            <% } else { %>
                <span><%=NotifVenta.MontoOCToString() %></span>
            <% } %>
        </li>
        <% if (NotifVenta != null && NotifVenta.LlevaITR) { %>
        <li class="form_floated_item form_floated_item_half">
            <label class="label" for="txtCliente">ITR (Informe de Trabajo Realizado)</label>
            <% if(NotifVenta.TieneItr) { %>
            <div class="tag_element green hand" id="lnkDownloadITR"><span>ITR cargado: presione aquí para descargar</span></div>
            <% } else { %>
            <div class="tag_element yellow"><span>Esperando carga de ITR...</span></div>
            <% } %>
        </li>
        <% } %>
		

		<li class="form_floated_item form_floated_item_half">
            <label class="label" for="txtFechaEntrega">Fecha de entrega</label>
			<% if (NotifVenta == null) { %>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtFechaEntrega" value="" maxlength="10" type="text"  runat="server" />
                </div>
            </div>
			
			<% } else { %>
                <span><%=NotifVenta.FechaEntrega %></span>
            <% } %>		
        </li>
		
		 <li class="form_floated_item form_floated_item_full">
            <label class="label" for="txtObservaciones">Detalle de la venta <%=(NotifVenta == null || (NotifVenta.AbleToVendedor() && GPermisosPersonal.TieneAcceso(PermisosPersona.SNV_Vendedor))) ? "(máx 6000 caracteres)" : "" %></label>
            <% if (NotifVenta == null || NotifVenta.AbleToVendedor()) { %>
                <div class="textarea_wrapper clear"> 
	                <div class="top">
		                <div class="cap"></div>
	                </div>
	                <div class="inner">
		                <div class="cap">
		                    <span class="max-length-status">0/0</span>
			                <textarea id="txtObservaciones" maxlength="6000" onkeyup="return MaxLength(this)"><%=NotifVenta != null ? NotifVenta.Observaciones.Replace("<br>", "\n") : ""%></textarea>
		                </div>
	                </div>
	                <div class="bottom">
		                <div class="cap"></div>
	                </div>
                </div>
            <% } else { %>
                <span><%=NotifVenta.Observaciones %></span>
            <% } %>
        </li>
		
		  <li class="form_floated_item form_floated_item_full">
            <label class="label" for="txtDatosEnvio">Datos del envío <%=(NotifVenta == null || (NotifVenta.AbleToVendedor() && GPermisosPersonal.TieneAcceso(PermisosPersona.SNV_Vendedor))) ? "(máx 6000 caracteres)" : "" %></label>
            <% if (NotifVenta == null || NotifVenta.AbleToVendedor()) { %>
			<div class="textarea_wrapper clear"> 
	            <div class="top">
		            <div class="cap"></div>
	            </div>
	            <div class="inner">
		            <div class="cap">
					    <span class="max-length-status">0/0</span>
			            <textarea id="txtDatosEnvio" maxlength="6000" onkeyup="return MaxLength(this)"><%=NotifVenta != null ? NotifVenta.DatosEnvio.Replace("<br>", "\n") : ""%></textarea>     
		            </div>
	            </div>
	            <div class="bottom">
		            <div class="cap"></div>
	            </div>
            </div>
			<% } else { %>
                <span><%=NotifVenta.DatosEnvio%></span>
            <% } %>
        </li>
        
		
    </ul>
        
    <div class="clear"></div>

	<ul class="middle_form">
		
        <% if (NotifVenta != null && NotifVenta.AbleToVendedor()) { %>
        <li class="form_floated_item form_floated_item_100">
            <label class="label"><input type="checkbox" id="chkConfirmar" />Confirmar datos (no podrán ser editados nuevamente)</label>
        </li>
        <% } %>
    </ul>
    
    <div class="clear"></div>

    <div class="form_buttons_container">
        <ul class="button_list">
            <% if(NotifVenta == null || NotifVenta.AbleToVendedor() || NotifVenta.AbleToCargaRemito() || NotifVenta.AbleToRem() || NotifVenta.AbleToFac()) { %>
            <li id="btnGuardar"><div class="btn primary_action_button button_100"><div class="cap"><span>Guardar</span></div></div></li>
            <% } %>
            <% if(NotifVenta != null && NotifVenta.AbleToAprobar()) { %>
            <li id="btnAprobar"><div class="btn primary_action_button button_100"><div class="cap"><span>Aprobar</span></div></div></li>
            <li id="btnRechazar"><div class="btn secondary_action_button button_100"><div class="cap"><span>Rechazar</span></div></div></li>
            <% } %>
            <li id="btnVolver"><div class="btn secondary_action_button button_100"><div class="cap"><span>Volver</span></div></div></li>
        </ul>
    </div>
    
    <div class="clear"></div>
</div>

<div class="dialog_wrapper" id="divRechazar" style="width:400px">
	<div class="dialog_header">
		<div class="cap right"><div class="cap left"><div class="cap inner"><h3>Rechazar remito</h3></div></div></div>
	</div>

	<div class="dialog_content">
		<div class="suggestion_message error" id="report-error">Este es un mensaje de error!.</div>
				
		<p><strong>¿Por qué motivo desea rechazar el remito?</strong></p>
		<br />
		<ul class="middle_form">
			<li class="form_floated_item normal">
				<div class="textarea_wrapper default_text"> 
					<div class="top">
						<div class="cap"></div>
					</div>
					<div class="inner">
						<div class="cap">
							<textarea id="txtRechazoMotivo" maxlength="500" onkeyup="return MaxLength(this)"></textarea>     
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
            <li><div class="btn primary_action_button_small button_100" id="btnRechazarAceptar"><div class="cap"><span>Rechazar</span></div></div></li>
            <li><div class="btn secondary_action_button_small button_100" id="btnRechazarCancelar"><div class="cap"><span>Cancelar</span></div></div></li>
		</ul>
	</div>
</div>

</asp:Content>