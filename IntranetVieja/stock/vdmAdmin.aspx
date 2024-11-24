<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="vdmAdmin.aspx.cs" Inherits="stock_vdmAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">
<!--[if lt IE 9]>     
<script src="/js/IE9.js" type="text/javascript"></script>
<![endif]-->

<script language="javascript" src="/js/json2.js" type="text/javascript"></script>

<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        <% if (PuedeSolicitante) { %>
        DibujarAcciones();
        <% } %>

        <% if (PuedeSolicitante) { %>
        $('#btnEnviar').click(function () {
            var departamento = $('#contentPlacePage_txtDepartamento');
            var smtl = $('#contentPlacePage_txtSMTL');
            var cargo = $('#contentPlacePage_txtCargo');
            var destino = $('#contentPlacePage_txtDestino');
            var result = true;

            result &= TieneDatos(departamento, 'input_wrapper', 'input_wrapper_error');
            result &= ContieneNumeros(smtl, 'input_wrapper', 'input_wrapper_error');
            result &= TieneDatos(cargo, 'input_wrapper', 'input_wrapper_error');
            result &= TieneDatos(destino, 'input_wrapper', 'input_wrapper_error');
            result &= ComprobarItems();

            if (result) {
                Mensaje('¿Esta seguro que desea generar la solicitud de vale de materiales?', 'warning', true, true, 'Cancelar', 'Aceptar', 'custom_dialog.close()', 'NuevoValeDeMateriales()');
            }
        });
        <% } %>
        <% if (PuedeResponsable) { %>
        $('#btnAprobar').click(function () {
            Mensaje('Una vez aprobado, el vale de materiales no podrá ser editado nuevamente. ¿Desea continuar?', 'warning', true, true, 'Cancelar', 'Aprobar', 'custom_dialog.close()', 'AprobarValeDeMateriales()');
        });
        $('#btnRechazar').click(function () {
            MostrarVentana('divRechazar');
        });
        $('#btnRechazarAceptar').click(function () {
            var motivo = $('#txtRechazarMotivo');
            var result = true;

            result &= TieneDatos(motivo, 'textarea_wrapper', 'textarea_wrapper_error');

            if (result) {
                RechazarValeDeMateriales();
            }
        });
        <% } %>
        <% if (PuedeDeposito) { %>
        $('#btnEntregado').click(function () {
            Mensaje('¿Desea confirmar la entrega del vale de materiales?', 'warning', true, true, 'Cancelar', 'Entregado', 'custom_dialog.close()', 'EntregarValeDeMateriales()');
        });
        <% } %>
        $('#btnCancelar').click(function () {
            Mensaje('Todos los datos ingresados serán descartados, ¿desea continuar?', 'warning', true, true, 'Cancelar', 'Aceptar', 'custom_dialog.close()', 'OnCancel()');
        });
        $('.input_actions ul li.search').click(function () {
            var item = $(this).parents('.input_actions').attr('name');
            item = item.replace('act_', '');

            BuscarArticulo(item);
        });
        $('.input_actions ul li.delete').click(function () {
            var item = $(this).parents('.input_actions').attr('name');
            item = item.replace('act_', '');

            BorrarItem(item);
        });
        $('#btnBuscarCancelar').click(function () {
            current_item = null;
            CerrarVentana();
        });
        $('#btnBuscarAceptar').click(function () {
            var articulo = $('#cbBuscarResultado');
            var result = true;

            result &= TieneDatos(articulo, 'input_wrapper', 'input_wrapper_selectbox_error');

            if (result) {
                MostrarLoading();

                ConsultaAjax.url = 'vdmAdmin.aspx/GetArticulo';
                ConsultaAjax.data = '{ "codigo": "' + articulo.val() + '" }';
                ConsultaAjax.AjaxSuccess = function (msg) {
                    if (msg.d != null) {
                        CompletarItem(msg.d[0], msg.d[1], msg.d[2]);
                    }

                    current_item = null;
                    CerrarVentana();
                };
                ConsultaAjax.AjaxError = function (msg) {
                    ErrorMsg(msg);
                };

                ConsultaAjax.Ejecutar();
            }
        });
        $('#rbBuscarCodigo').click(function () {
            ActualizarGUIBuscarArticulo();
        });
        $('#rbBuscarDescripcion').click(function () {
            ActualizarGUIBuscarArticulo();
        });
        $('#txtBuscarCodigo').change(function () {
            if ($(this).val().length > 0) {
                var codigo = $('#txtBuscarCodigo');
                var result = true;

                result &= TieneDatos(codigo, 'input_wrapper', 'input_wrapper_error');

                if (result) {
                    MostrarLoading();

                    ConsultaAjax.url = 'vdmAdmin.aspx/GetArticulo';
                    ConsultaAjax.data = '{ "codigo": "' + codigo.val() + '" }';
                    ConsultaAjax.AjaxSuccess = function (msg) {
                        $('#cbBuscarResultado').html('');

                        if (msg.d != null) {
                            $('#cbBuscarResultado').html('<option value="' + msg.d[0] + '">' + msg.d[1] + '</option>');
                            $('#cbBuscarResultado').change();

                            $('#lblBuscarError').hide();
                        }
                        else {
                            $('#lblBuscarResultado').text('');
                            $('#lblBuscarError').show();
                        }

                        $('#cbBuscarResultado').parents('.input_wrapper').removeClass('input_wrapper_selectbox_error');

                        MostrarVentana('divBuscarArticulo');
                    };
                    ConsultaAjax.AjaxError = function (msg) {
                        ErrorMsg(msg);
                    };

                    ConsultaAjax.Ejecutar();
                }
            }
        });
        $('#txtBuscarDescripcion').change(function () {
            if ($(this).val().length > 0) {
                var descripcion = $('#txtBuscarDescripcion');
                var result = true;

                result &= TieneDatos(descripcion, 'input_wrapper', 'input_wrapper_error');

                if (result) {
                    MostrarLoading();

                    ConsultaAjax.url = 'vdmAdmin.aspx/GetArticulos';
                    ConsultaAjax.data = '{ "descripcion": "' + descripcion.val() + '" }';
                    ConsultaAjax.AjaxSuccess = function (msg) {
                        $('#cbBuscarResultado').html('');

                        if (msg.d.length > 0) {
                            var articulos = [];
                            for (i = 0; i < msg.d.length; i++) {
                                articulos.push('<option value="' + msg.d[i][0] + '">' + msg.d[i][1] + '</option>');
                            }

                            $('#cbBuscarResultado').html(articulos.join(''));
                            $('#cbBuscarResultado').change();

                            $('#lblBuscarError').hide();
                        }
                        else {
                            $('#lblBuscarResultado').text('');
                            $('#lblBuscarError').show();
                        }

                        $('#cbBuscarResultado').parents('.input_wrapper').removeClass('input_wrapper_selectbox_error');

                        MostrarVentana('divBuscarArticulo');
                    };
                    ConsultaAjax.AjaxError = function (msg) {
                        ErrorMsg(msg);
                    };

                    ConsultaAjax.Ejecutar();
                }
            }
        });

        <% if(this.VDM != null) { %>
            $('#btnImprimir').click(function() {
                var url = 'vdmImprimir.aspx?p=<%=Encriptacion.GetParametroEncriptado("id=" + (this.VDM != null ? this.VDM.ID : Constantes.ValorInvalido )) %>';
                window.open(url, '' , 'status=0, toolbar=0, resizable=no, menubar=0, directories=0, width=820, scrollbars=1');
            });
        <% } %>
    });

    <% if (PuedeSolicitante) { %>
    function DibujarAcciones() {
        $('.input_actions').html('<ul><li class="search"></li><li class="delete"></li></ul>');
    }
    <% } %>

    function CompletarItem(codigo, descripcion, un) {
        $('#txtItemCodigo_' + current_item).val(codigo);
        $('#txtItemDescripcion_' + current_item).val(descripcion);
        $('#txtItemUn_' + current_item).val(un);
    }

    function ActualizarGUIBuscarArticulo() {
        if ($('#rbBuscarCodigo').is(':checked')) {
            $('#txtBuscarCodigo').removeAttr('disabled');
            $('#txtBuscarDescripcion').attr('disabled', 'disabled');
        }
        else {
            $('#txtBuscarCodigo').attr('disabled', 'disabled');
            $('#txtBuscarDescripcion').removeAttr('disabled');
        }

        $('#cbBuscarResultado').html('');
        $('#cbBuscarResultado').parents('.input_wrapper').removeClass('input_wrapper_selectbox_error');
        $('#lblBuscarResultado').text('');
        $('#lblBuscarError').hide();
    }

    var current_item;
    function BuscarArticulo(item) {
        current_item = item;

        ActualizarGUIBuscarArticulo();

        $('#txtBuscarCodigo').val('');
        $('#txtBuscarDescripcion').val('');
        $('#lblBuscarError').hide();

        MostrarVentana('divBuscarArticulo');
    }

    function BorrarItem(item) {
        current_item = item;

        $('#txtItemCodigo_' + current_item).val('');
        $('#txtItemDescripcion_' + current_item).val('');
        $('#txtItemUn_' + current_item).val('');
        $('#txtItemCantidad_' + current_item).val('');
        $('#cbItemImputacion_' + current_item).val('<%= Constantes.IdImputacionInvalida.ToString() %>');
        $('#cbItemImputacion_' + current_item).change();
        $('#txtItemObra_' + current_item).val('');

        QuitarEstiloItem(current_item);

        current_item = null;
    }

    function ComprobarItems() {
        var result = true;
        var vacios = true;
        var i;
        
        var maxItems = <%= this.VDM == null || PuedeSolicitante ? MaxItems.ToString() : this.VDM.Items.Count.ToString() %>;
        for (i = 0; i < maxItems; i++) {
            QuitarEstiloItem(i);
            if(!EsItemVacio(i)){
                vacios = false;

                if(!EsItemValido(i)) {
                    result = false;
                }
            }
        }

        result &= !vacios;

        return result;
    }

    function EsItemValido(item) {
        var codigo = $('#txtItemCodigo_' + item);
        var descripcion = $('#txtItemDescripcion_' + item);
        var un = $('#txtItemUn_' + item);
        var cantidad = $('#txtItemCantidad_' + item);
        var imputacion = $('#cbItemImputacion_' + item);
        var obra = $('#txtItemObra_' + item);
        var result = true;

        result &= TieneDatos(codigo, 'input_wrapper', 'input_wrapper_error');
        result &= TieneDatos(descripcion, 'input_wrapper', 'input_wrapper_error');
        //result &= TieneDatos(un, 'input_wrapper', 'input_wrapper_error');
        result &= TieneDatos(cantidad, 'input_wrapper', 'input_wrapper_error') && ContieneNumeros(cantidad, 'input_wrapper', 'input_wrapper_error');
        result &= ContieneValorDiferente(imputacion, '<%= Constantes.IdImputacionInvalida.ToString() %>', 'input_wrapper', 'input_wrapper_selectbox_error');
        result &= TieneDatos(obra, 'input_wrapper', 'input_wrapper_error');

        return result;
    }

    function EsItemVacio(item) {
        var result;

        result = $('#txtItemCodigo_' + item).val().length == 0 &&
                 $('#txtItemDescripcion_' + item).val().length == 0 &&
                 //$('#txtItemUn_' + item).val().length == 0 &&
                 $('#txtItemCantidad_' + item).val().length == 0 &&
                 $('#cbItemImputacion_' + item).val() == '<%= Constantes.IdImputacionInvalida.ToString() %>';
                 $('#txtItemObra_' + item).val().length == 0;

        return result;
    }

    function QuitarEstiloItem(item){
        $('#txtItemCodigo_' + item).parents('.input_wrapper').removeClass('input_wrapper_error');
        $('#txtItemDescripcion_' + item).parents('.input_wrapper').removeClass('input_wrapper_error');
        $('#txtItemUn_' + item).parents('.input_wrapper').removeClass('input_wrapper_error');
        $('#txtItemCantidad_' + item).parents('.input_wrapper').removeClass('input_wrapper_error');
        $('#cbItemImputacion_' + item).parents('.input_wrapper').removeClass('input_wrapper_selectbox_error');
        $('#txtItemObra_' + item).parents('.input_wrapper').removeClass('input_wrapper_error');
    }

    function GetItemVDM(item) {
        var result;
        var codigo = $('#txtItemCodigo_' + item);
        var descripcion = $('#txtItemDescripcion_' + item);
        var un = $('#txtItemUn_' + item);
        var cantidad = $('#txtItemCantidad_' + item);
        var imputacion = $('#cbItemImputacion_' + item);
        var obra = $('#txtItemObra_' + item);

        result = [codigo.val(), cantidad.val(), imputacion.val(), obra.val()];

        return result;
    }

    function GetItemsVDM() {
        var result = [];
        
        var maxItems = <%= this.VDM == null || PuedeSolicitante ? MaxItems.ToString() : this.VDM.Items.Count.ToString() %>;
        for (i = 0; i < maxItems; i++) {
            if (!EsItemVacio(i)) {
                result.push(GetItemVDM(i));
            }
        }

        return result;
    }

    <% if (PuedeSolicitante) { %>
    function NuevoValeDeMateriales() {
        custom_dialog.close();

        var departamento = $('#contentPlacePage_txtDepartamento').val();
        var smtl = $('#contentPlacePage_txtSMTL').val();
        var cargo = $('#contentPlacePage_txtCargo').val();
        var destino = $('#contentPlacePage_txtDestino').val();
        var items = GetItemsVDM();

        if(smtl.length == 0){
            smtl = 0;
        }
        
        MostrarLoading();

        <% if (this.VDM == null) { %>
        ConsultaAjax.url = 'vdmAdmin.aspx/NuevoVDM';
        ConsultaAjax.data = JSON.stringify({ departamento: departamento, smtl: smtl, cargo: cargo, destino: destino, items: items });
        <% } 
           else if (this.VDM.Estado == EstadosVDM.RechazadaResponsable) { %>
        ConsultaAjax.url = 'vdmAdmin.aspx/ActualizarVDM';
        ConsultaAjax.data = JSON.stringify({ items: items });
        <% } %>
        ConsultaAjax.AjaxSuccess = function (msg) {
            Mensaje(msg.d, 'success', true, false, 'Aceptar', '', 'OnCerrar()', '');
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }
    <% } %>
    <% if (PuedeResponsable) { %>
    function AprobarValeDeMateriales() {
        custom_dialog.close();
        
        MostrarLoading();

        ConsultaAjax.url = 'vdmAdmin.aspx/AprobarVDM';
        ConsultaAjax.AjaxSuccess = function (msg) {
            Mensaje(msg.d, 'success', true, false, 'Aceptar', '', 'OnCerrar()', '');
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }
    function RechazarValeDeMateriales() {
        custom_dialog.close();
        
        var motivo = $('#txtRechazarMotivo').val();

        MostrarLoading();

        ConsultaAjax.url = 'vdmAdmin.aspx/RechazarVDM';
        ConsultaAjax.data = '{ "motivo": "' + motivo + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            Mensaje(msg.d, 'success', true, false, 'Aceptar', '', 'OnCerrar()', '');
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }
    <% } %>
    <% if (PuedeDeposito) { %>
    function EntregarValeDeMateriales() {
        custom_dialog.close();
        
        MostrarLoading();

        ConsultaAjax.url = 'vdmAdmin.aspx/EntregarVDM';
        ConsultaAjax.AjaxSuccess = function (msg) {
            Mensaje(msg.d, 'success', true, false, 'Aceptar', '', 'OnCerrar()', '');
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }
    <% } %>

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
    <h1>Solicitud de vale de materiales</h1>
</div>

<div class="form-steps" style="width:800px; height:100px">
    <ul>
        <li>
            <div class="step-number<%= (this.VDM == null || this.VDM.Estado == EstadosVDM.Enviada) ? " active" : "" %>">1.</div>
            <div class="step-description<%= (this.VDM == null || this.VDM.Estado == EstadosVDM.Enviada) ? " active" : "" %>"><%= this.VDM != null ? "Enviada" : "Solicitud" %></div>
        </li>
        <li class="separator"></li>
        <li>
            <div class="step-number<%= this.VDM != null && this.VDM.Estado == EstadosVDM.RecibidaResponsable ? " active" : "" %>">2.</div>
            <div class="step-description<%= this.VDM != null && this.VDM.Estado == EstadosVDM.RecibidaResponsable ? " active" : "" %>">Recibida<br /> por responsable</div>
        </li>
        <li class="separator"></li>
        <% if (this.VDM != null && this.VDM.Estado == EstadosVDM.RechazadaResponsable) { %>
        <li>
            <div class="step-number<%= this.VDM != null && this.VDM.Estado == EstadosVDM.RechazadaResponsable ? " active" : "" %>">3</div>
            <div class="step-description<%= this.VDM != null && this.VDM.Estado == EstadosVDM.RechazadaResponsable ? " active" : "" %>">Rechazada<br /> por responsable</div>
        </li>
        <% }
           else { %>
        <li>
            <div class="step-number<%= this.VDM != null && this.VDM.Estado == EstadosVDM.AprobadaResponsable ? " active" : "" %>">3.</div>
            <div class="step-description<%= this.VDM != null && this.VDM.Estado == EstadosVDM.AprobadaResponsable ? " active" : "" %>">Aprobada<br /> por responsable</div>
        </li>
        <% } %>
        <li class="separator"></li>
        <li>
            <div class="step-number<%= this.VDM != null && this.VDM.Estado == EstadosVDM.RecibidaDeposito ? " active" : "" %>">4.</div>
            <div class="step-description<%= this.VDM != null && this.VDM.Estado == EstadosVDM.RecibidaDeposito ? " active" : "" %>">Recibida<br /> por depósito</div>
        </li>
        <li class="separator"></li>
        <li>
            <div class="step-number<%= this.VDM != null && this.VDM.Estado == EstadosVDM.EntregadaDeposito ? " active" : "" %>">5.</div>
            <div class="step-description<%= this.VDM != null && this.VDM.Estado == EstadosVDM.EntregadaDeposito ? " active" : "" %>">Entregada</div>
        </li>
    </ul>    
</div>

<div class="form_place" style="width:870px">
    <h3>1. Datos del vale de materiales</h3>

    <% if (this.VDM == null) { %>
    <p>Los campos marcados con <span class="required"></span> son obligatorios.</p>
    <br />
    <% } %>

    <ul class="middle_form">
        <% if (this.VDM != null) { %>
        <li class="form_floated_item form_floated_item_half">
            <label class="label">Fecha de solicitud</label>
            <span id="lblFechaSolicitud" runat="server">[...]</span>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label">Emitida por</label>
            <span id="lblEmitidaPor" runat="server">[...]</span>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label">Recibida por responsable de área</label>
            <span id="lblRecibidaResponsable" runat="server">[...]</span>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label">Aprobada por responsable de área</label>
            <span id="lblAprobadaResponsable" runat="server">[...]</span>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label">Recibida por depósito</label>
            <span id="lblRecibidaDeposito" runat="server">[...]</span>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label">Entregada por depósito</label>
            <span id="lblEntregada" runat="server">[...]</span>
        </li>
        <% } %>
        <li class="form_floated_item form_floated_item_half">
            <label class="label required" for="txtDepartamento">Departamento</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtDepartamento" maxlength="25" value="" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label" for="txtSMTL">SMTL</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtSMTL" value="0" maxlength="5" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label required" for="txtCargo">Cargo</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtCargo" value="" maxlength="20" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label required" for="txtDestino">Destino</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtDestino" value="" maxlength="25" type="text" runat="server"/>
                </div>
            </div>
        </li>
    </ul>
    
    <div class="clear"></div>

    <h3 class="middle_h3">2. Ítems de la solicitud</h3>

    <table class="tbl" cellspacing="0" cellpadding="0">
        <thead>
            <tr>
                <td class="border_left align-center" style="width:150px">Código</td>
                <td class="border_middle align-center" style="width:280px">Descripción</td>
                <td class="border_middle align-center" style="width:50px">Un</td>
                <td class="border_middle align-center" style="width:90px">Cantidad</td>
                <td class="border_middle align-center" style="width:100px">Imputación</td>
                <td class="border_right align-center" style="width:170px">Obra</td>
            </tr>
        </thead>
        <tbody id="itemsVDM">
            <% 
                List<Imputacion> imputaciones = GImputaciones.GetImputaciones();
                int maxItems = (this.VDM == null || PuedeSolicitante) ? MaxItems : this.VDM.Items.Count;
                for (int i = 0; i < maxItems; i++) {
                    ItemVDM item = this.VDM != null ? this.VDM[i] : null;
            %>
            <tr class="no-editable">
                <td>
                    <div class="input_wrapper hasActions">
                        <div class="input_actions" name="act_<%= i.ToString() %>"></div>
                        <div class="cap">
                            <input id="txtItemCodigo_<%= i.ToString() %>" readonly="readonly" maxlength="15" value="<%= item != null ? item.Articulo.Codigo : "" %>" type="text"/>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="input_wrapper hasActions">
                        <div class="input_actions" name="act_<%= i.ToString() %>"></div>
                        <div class="cap">
                            <input id="txtItemDescripcion_<%= i.ToString() %>" readonly="readonly" value="<%= item != null ? item.Articulo.Descripcion.Replace("\"", "''") : "" %>" type="text"/>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="input_wrapper">
                        <div class="cap">
                            <input id="txtItemUn_<%= i.ToString() %>" readonly="readonly" value="<%= item != null ? item.Articulo.Un : "" %>" type="text"/>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="input_wrapper">
                        <div class="cap">
                            <input id="txtItemCantidad_<%= i.ToString() %>" <%= PuedeSolicitante ? "" : "readonly=\"readonly\"" %> value="<%= item != null ? item.Cantidad.ToString() : "" %>" type="text"/>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="input_wrapper input_wrapper_selectbox">
                        <div class="cap">
                            <span></span>
                            <select id="cbItemImputacion_<%= i.ToString() %>" <%= PuedeSolicitante ? "" : "disabled=\"disabled\"" %>>
                                <%
                                    foreach(Imputacion imp in imputaciones)
                                    {
                                        %>
                                            <option value="<%= imp.ID.ToString() %>" <%= item != null && item.IDImputacion == imp.ID ? "selected=\"selected\"" : "" %>><%= imp.Numero.ToString() %></option>
                                        <%
                                    }
                                 %>
                            </select>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="input_wrapper">
                        <div class="cap">
                            <input id="txtItemObra_<%= i.ToString() %>" maxlength="20" <%= PuedeSolicitante ? "" : "readonly=\"readonly\"" %> value="<%= item != null ? item.Obra : "" %>" type="text"/>
                        </div>
                    </div>
                </td>
            </tr>
            <% } %>
        </tbody>
        <tfoot>
            <tr>
                <td colspan="6" class="border_middle"></td>
            </tr>
        </tfoot>
    </table>

    <div class="form_buttons_container">
        <ul class="button_list">
            <% if(PuedeSolicitante) { %>
            <li id="btnEnviar"><div class="btn primary_action_button button_100"><div class="cap"><span>Enviar</span></div></div></li>
            <% } %>
            <% if(PuedeResponsable) { %>
            <li id="btnAprobar"><div class="btn primary_action_button button_100"><div class="cap"><span>Aprobar</span></div></div></li>
            <li id="btnRechazar"><div class="btn secondary_action_button button_100"><div class="cap"><span>Rechazar</span></div></div></li>
            <% } %>
            <% if(PuedeDeposito) { %>
            <li id="btnEntregado"><div class="btn primary_action_button button_100"><div class="cap"><span>Entregado</span></div></div></li>
            <% } %>
            <% if (this.VDM == null || this.VDM.Estado != EstadosVDM.EntregadaDeposito) { %>
            <li id="btnCancelar"><div class="btn secondary_action_button button_100"><div class="cap"><span>Cancelar</span></div></div></li>
            <% } %>
            <% if (this.VDM != null && this.VDM.Estado == EstadosVDM.EntregadaDeposito) { %>
            <li onclick="javascript:location.href='/stock/vdmLista.aspx'"><div class="btn primary_action_button button_100"><div class="cap"><span>Volver</span></div></div></li>
            <% } %>
            <% if (this.VDM != null) { %>
            <li id="btnImprimir"><div class="btn secondary_action_button button_100"><div class="cap"><span>Imprimir</span></div></div></li>
            <% } %>
        </ul>
    </div>
</div>

<% if (PuedeSolicitante) { %>
<div class="dialog_wrapper" style="width:500px" id="divBuscarArticulo">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3>Buscar artículo</h3></div></div></div>
    </div>

    <div class="dialog_content">
        <ul class="middle_form">
            <li class="form_floated_item form_floated_item_100">
                <label class="label" for="txtBuscarCodigo"><input type="radio" name="rbBuscarFiltro" checked="checked" id="rbBuscarCodigo" /> Código</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtBuscarCodigo" maxlength="15" value="" type="text"/>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_100">
                <label class="label" for="txtBuscarDescripcion"><input type="radio" name="rbBuscarFiltro" id="rbBuscarDescripcion" /> Descripción</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtBuscarDescripcion" maxlength="100" value="" type="text"/>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_100">
                <label class="label" for="cbPersonaCargo">Resultado</label>
                <div class="input_wrapper input_wrapper_selectbox">
                    <div class="cap">
                        <span id="lblBuscarResultado"></span>
                        <select id="cbBuscarResultado">
                        </select>
                    </div>
                </div>
            </li>
        </ul>
        <p id="lblBuscarError">No se encontraron resultados para la búsqueda.</p>
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li><div class="btn primary_action_button_small button_100" id="btnBuscarAceptar"><div class="cap"><span>Aceptar</span></div></div></li>
            <li><div class="btn secondary_action_button_small button_100" id="btnBuscarCancelar"><div class="cap"><span>Cancelar</span></div></div></li>
        </ul>
    </div>
</div>
<% } %>

<% if (PuedeResponsable) { %>
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
			                <textarea id="txtRechazarMotivo"></textarea>     
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
<% } %>

</asp:Content>

