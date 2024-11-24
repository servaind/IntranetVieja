<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="cotizador.aspx.cs" Inherits="stock_cotizador" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">

<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        $('#btnCancelar').click(function () {
            Mensaje('Todos los datos ingresados serán descartados, ¿desea continuar?', 'warning', true, true, 'Cancelar', 'Aceptar', 'custom_dialog.close()', 'OnCancel()');
        });
        $('#btnExportar').click(function(){
            if (CalcularTotales()) {
                MostrarLoading();

                ConsultaAjax.url = 'cotizador.aspx/ExportarCotizacion';
                ConsultaAjax.data = JSON.stringify({ items: GetItemsCotizacion() });
                ConsultaAjax.AjaxSuccess = function (msg) {
                    location.href = msg.d;

                    CerrarVentana();
                };
                ConsultaAjax.AjaxError = function (msg) {
                    ErrorMsg(msg);
                };

                ConsultaAjax.Ejecutar();
            } else {
                ErrorMsg('Algunos de los campos contienen datos inválidos.');
            }
        });
        $('#btnAgregarItem').click(function () {
            BuscarArticulo(-1);
        });
        $('#btnAgregarProducto').click(function () {
            BuscarProducto();
        });
        $('#btnLimpiarLista').click(function () {
            Mensaje('¿Está seguro que desea eliminar todos los elementos de la lista?', 'warning', true, true, 'Cancelar', 'Limpiar', 'custom_dialog.close()', 'BorrarItemsLista()');
        });
        $('#btnBuscarArticuloCancelar').click(function () {
            current_item = null;
            CerrarVentana();
        });
        $('#btnBuscarArticuloAceptar').click(function () {
            var articulo = $('#cbBuscarResultado');
            var result = true;

            result &= TieneDatos(articulo, 'input_wrapper', 'input_wrapper_selectbox_error');

            if (result) {
                MostrarLoading();

                ConsultaAjax.url = 'cotizador.aspx/GetArticulo';
                ConsultaAjax.data = '{ "codigo": "' + articulo.val() + '" }';
                ConsultaAjax.AjaxSuccess = function (msg) {
                    if (msg.d != null) {
                        if (current_item >= 0) {
                            EditarItem(current_item, msg.d[0], msg.d[1], msg.d[2], msg.d[3], msg.d[4], msg.d[5], msg.d[6]);
                        }
                        else {
                            AgregarItem(msg.d[0], msg.d[1], 1, msg.d[2], msg.d[3], msg.d[4], msg.d[5], msg.d[6]);
                        }
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

                    ConsultaAjax.url = 'cotizador.aspx/GetArticulo';
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

                    ConsultaAjax.url = 'cotizador.aspx/GetArticulos';
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
        <% if(PuedeProductos) { %>
        $('#btnBuscarProductoCancelar').click(function () {
            CerrarVentana();
        });
        $('#btnBuscarProductoAceptar').click(function () {
            var producto = $('#contentPlacePage_cbBuscarProducto');
            var result = true;

            result &= TieneDatos(producto, 'input_wrapper', 'input_wrapper_selectbox_error');

            if (result) {
                MostrarLoading();

                ConsultaAjax.url = 'cotizador.aspx/GetArticulosProducto';
                ConsultaAjax.data = '{ "idProducto": "' + producto.val() + '" }';
                ConsultaAjax.AjaxSuccess = function (msg) {
                    if (msg.d != null) {
                        var articulo;
                        for (articulo in msg.d) {
                            try{
                                AgregarItem(msg.d[articulo][0], msg.d[articulo][1], msg.d[articulo][2], msg.d[articulo][3], msg.d[articulo][4], msg.d[articulo][5], msg.d[articulo][6], msg.d[articulo][7]);
                            } catch (err){
                                // En IE arroja un error sobre 'length' no definido 0.0
                            }
                        }
                    }

                    CerrarVentana();
                };
                ConsultaAjax.AjaxError = function (msg) {
                    ErrorMsg(msg);
                };

                ConsultaAjax.Ejecutar();
            }
        });
        <% } %>

        items = [];
        ultItem = 0;
    });

    function OnCancel() {
        location.href = '<%= Constantes.UrlIntraDefault %>';
    }

    function DibujarAcciones() {
        $('.input_actions').html('<ul><li class="search"></li><li class="delete"></li></ul>');
    }

    var EnlazarEventos = function () {
        $('.input_actions ul li.search').unbind('click').click(function () {
            var item = $(this).parents('.input_actions').attr('name');
            item = item.replace('act_', '');

            BuscarArticulo(item);
        });
        $('.input_actions ul li.delete').unbind('click').click(function () {
            var item = $(this).parents('.input_actions').attr('name');
            item = item.replace('act_', '');

            BorrarItem(item);
        });
    }

    var ultItem;
    var items;
    function AgregarItem(codigo, descripcion, cantidad, oc, precio, precioDesc, fecha, proveedor) {
        var j;
        if ((j = GetItemIndex(codigo)) != -1) {
            var cant = $('#txtItemCantidad_' + j);

            cant.val(cant.val().replace(',', '.'));

            cant.val(parseFloat(cant.val()) + cantidad);
        }
        else {
            var fila = '<tr id="fila_' + ultItem + '" class="no-editable">';
            fila += '<td class="align-center" id="lblItemCodigo_' + ultItem + '"><div class="hasActions">' + codigo + '<div class="input_actions" name="act_' + ultItem + '"></div></div></td>';
            fila += '<td class="align-left hasActions" title="' + descripcion + '" id="lblItemDescripcion_' + ultItem + '"><div class="hasActions">' + AcortarTexto(descripcion, 36) + '<div class="input_actions" name="act_' + ultItem + '"></div></div></td>';
            fila += '<td><div class="input_wrapper"><div class="cap"><input class="align-right" onchange="CalcularTotales()" id="txtItemCantidad_' + ultItem + '" value="' + cantidad + '" type="text"/></div></div></td>';
            fila += '<td class="align-center" id="lblItemOC_' + ultItem + '">' + oc + '</td>';
            fila += '<td><div class="input_wrapper"><div class="cap"><input class="align-right" onchange="CalcularTotales()" id="txtItemPrecio_' + ultItem + '" value="' + precio + '" type="text"/></div></div></td>';
            fila += '<td><div class="input_wrapper"><div class="cap"><input class="align-right" onchange="CalcularTotales()" id="txtItemPrecioDesc_' + ultItem + '" value="' + precioDesc + '" type="text"/></div></div></td>';
            fila += '<td class="align-center" id="lblItemFecha_' + ultItem + '">' + fecha + '</td>';
            fila += '<td class="align-left" id="lblItemProveedor_' + ultItem + '">' + proveedor + '</td>';
            fila += '</tr>';

            $('#items-cotizacion').append(fila);

            DibujarAcciones();
            EnlazarEventosMaster();
            EnlazarEventos();

            items.push(ultItem++);
        }

        CalcularTotales();
    }

    function EditarItem(item, codigo, descripcion, oc, precio, precioDesc, fecha, proveedor) {
        if (GetItemIndex(codigo) != -1) {
            ErrorMsg('El artículo seleccionado ya se encuentra en la lista.');
            return;
        }

        $('#lblItemCodigo_' + item).html('<div class="hasActions">' + codigo + '<div class="input_actions" name="act_' + item + '"></div></div>');
        $('#lblItemDescripcion_' + item).html('<div class="hasActions">' + AcortarTexto(descripcion, 36) + '<div class="input_actions" name="act_' + item + '"></div></div>');
        $('#lblItemDescripcion_' + item).attr('title', descripcion);
        $('#lblItemOC_' + item).html(oc);
        $('#txtItemPrecio_' + item).val(precio);
        $('#txtItemPrecioDesc_' + item).val(precioDesc);
        $('#lblItemFecha_' + item).html(fecha);
        $('#lblItemProveedor_' + item).html(proveedor);

        DibujarAcciones();
        EnlazarEventosMaster();
        EnlazarEventos();
        CalcularTotales();
    }

    function GetItemIndex(codigo) {
        var result = -1;
        var i;

        for (i = 0; i < items.length; i++) {
            var j = items[i];

            if ($('#lblItemCodigo_' + j)) {
                if ($('#lblItemCodigo_' + j).text() == codigo) {
                    result = j;
                    break;
                }
            }
        }

        return result;
    }

    function ActualizarGUIBuscarArticulo() {
        if ($('#rbBuscarCodigo').is(':checked')) {
            $('#txtBuscarCodigo').removeAttr('disabled');
            $('#txtBuscarDescripcion').attr('disabled', 'disabled');
            $('#txtBuscarCodigo').focus();
        }
        else {
            $('#txtBuscarCodigo').attr('disabled', 'disabled');
            $('#txtBuscarDescripcion').removeAttr('disabled');
            $('#txtBuscarDescripcion').focus();
        }

        $('#cbBuscarResultado').html('');
        $('#cbBuscarResultado').parents('.input_wrapper').removeClass('input_wrapper_selectbox_error');
        $('#lblBuscarResultado').text('');
        $('#lblBuscarError').hide();
    }

    function BuscarProducto() {
        MostrarVentana('divBuscarProducto');
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

        $('#fila_' + item).remove();

        RemoveArrayElement(items, parseInt(item));

        current_item = null;

        CalcularTotales();
    }

    function BorrarItemsLista() {
        var i;
        var aux = items.slice();
        for (i = 0; i < aux.length; i++) {
            var j = aux[i];

            BorrarItem(j);
        }

        custom_dialog.close();
    }

    function CalcularTotales() {
        var totalSinDesc = 0.0;
        var totalConDesc = 0.0;
        var cant;
        var precio;
        var precioDesc;
        var result = true;

        var i;
        for (i = 0; i < items.length; i++) {
            var j = items[i];
            if ($('#lblItemCodigo_' + j)) {
                var valid = true;

                cant = $('#txtItemCantidad_' + j);
                precio = $('#txtItemPrecio_' + j);
                precioDesc = $('#txtItemPrecioDesc_' + j);

                cant.val(parseFloat(cant.val().replace(',', '.')).toFixed(3));
                precio.val(parseFloat(precio.val().replace(',', '.')).toFixed(3));
                precioDesc.val(parseFloat(precioDesc.val().replace(',', '.')).toFixed(3));

                valid &= TieneDatos(cant, 'input_wrapper', 'input_wrapper_error') && ContieneNumeros(cant, 'input_wrapper', 'input_wrapper_error');
                valid &= TieneDatos(precio, 'input_wrapper', 'input_wrapper_error') && ContieneNumeros(precio, 'input_wrapper', 'input_wrapper_error') && ContieneValorDiferente(precio, 0, 'input_wrapper', 'input_wrapper_error');
                valid &= TieneDatos(precioDesc, 'input_wrapper', 'input_wrapper_error') && ContieneNumeros(precioDesc, 'input_wrapper', 'input_wrapper_error') && ContieneValorDiferente(precioDesc, 0, 'input_wrapper', 'input_wrapper_error');

                if (valid && !isNaN(cant.val()) && !isNaN(precio.val()) && !isNaN(precioDesc.val())) {
                    totalSinDesc += cant.val() * precio.val();
                    totalConDesc += cant.val() * precioDesc.val();
                } else {
                    result = false;
                }
            }
        }

        $('#lblTotalSinDesc').text(totalSinDesc.toFixed(3));
        $('#lblTotalConDesc').text(totalConDesc.toFixed(3));

        return result;
    }

    function GetItemsCotizacion() {
        var result = [];
        var codigo;
        var cant;
        var precio;
        var precioDesc;

        var i;
        for (i = 0; i < items.length; i++) {
            var j = items[i];
            if ($('#lblItemCodigo_' + j)) {
                codigo = $('#lblItemCodigo_' + j).text();
                cant = $('#txtItemCantidad_' + j).val();
                precio = $('#txtItemPrecio_' + j).val();
                precioDesc = $('#txtItemPrecioDesc_' + j).val();

                result.push([codigo, cant, precio, precioDesc]);
            }
        }

        return result;
    }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlacePage" Runat="Server">

<div class="page-title">
    <h1>Cotizador</h1>
</div>

<div class="form_place" style="width:870px">
    <h3>Contenido de la cotización</h3>
</div>

<div class="full-width scroll-horizontal" style="width:870px">
    <table class="tbl editable listado" style="width:1250px;" cellspacing="0">
        <thead>
            <tr>
                <td class="border_left" style="width:150px">Código</td>
                <td class="border_middle" style="width:280px">Descripción</td>
                <td class="border_middle" style="width:90px">Cantidad</td>
                <td class="border_middle" style="width:90px">OC</td>
                <td class="border_middle" style="width:100px">Precio</td>
                <td class="border_middle" style="width:100px">Precio desc.</td>
                <td class="border_middle" style="width:120px">Fecha</td>
                <td class="border_right">Proveedor</td>
            </tr>
        </thead>
        <tbody id="items-cotizacion">

        </tbody>
        <tfoot>
            <tr>
                <td colspan="8" class="border_middle">
                    <a id="btnAgregarItem">Agregar ítem</a>
                    &nbsp;|&nbsp;
                    <% if (PuedeProductos) { %>
                    <a id="btnAgregarProducto">Agregar Producto</a>
                    &nbsp;|&nbsp;
                    <% } %>
                    <a class="secondary" id="btnLimpiarLista">Limpiar lista</a>
                </td>
            </tr>
        </tfoot>
    </table>
</div>

<div class="form_place" style="width:870px">
    <br />
    <ul class="middle_form">
        <li class="form_floated_item form_floated_item_half">
            <label class="label">Importe total sin descuento</label>
            U$S <span id="lblTotalSinDesc">0.00</span>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label">Importe total con descuento</label>
            U$S <span id="lblTotalConDesc">0.00</span>
        </li>
    </ul>

    <div class="clear"></div>
    <br />
    <p><span class="important">Importante: </span>los precios se encuentran expresados en U$S.</p>

    <div class="form_buttons_container">
        <ul class="button_list">
            <li id="btnExportar"><div class="btn primary_action_button button_100"><div class="cap"><span>Exportar</span></div></div></li>
            <li id="btnCancelar"><div class="btn secondary_action_button button_100"><div class="cap"><span>Cancelar</span></div></div></li>
        </ul>
    </div>
</div>

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
                <label class="label" for="cbBuscarResultado">Resultado</label>
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
            <li><div class="btn primary_action_button_small button_100" id="btnBuscarArticuloAceptar"><div class="cap"><span>Aceptar</span></div></div></li>
            <li><div class="btn secondary_action_button_small button_100" id="btnBuscarArticuloCancelar"><div class="cap"><span>Cancelar</span></div></div></li>
        </ul>
    </div>
</div>

<% if (PuedeProductos) { %>
<div class="dialog_wrapper" style="width:500px" id="divBuscarProducto">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3>Buscar Producto</h3></div></div></div>
    </div>

    <div class="dialog_content">
        <p>Seleccione un Producto de la lista</p>
        <br />
        <ul class="middle_form">
            <li class="form_floated_item form_floated_item_100">
                <div class="input_wrapper input_wrapper_selectbox">
                    <div class="cap">
                        <span id="lblBuscarProducto"></span>
                        <select id="cbBuscarProducto" runat="server">
                        </select>
                    </div>
                </div>
            </li>
        </ul>
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li><div class="btn primary_action_button_small button_100" id="btnBuscarProductoAceptar"><div class="cap"><span>Aceptar</span></div></div></li>
            <li><div class="btn secondary_action_button_small button_100" id="btnBuscarProductoCancelar"><div class="cap"><span>Cancelar</span></div></div></li>
        </ul>
    </div>
</div>
<% } %>

</asp:Content>

