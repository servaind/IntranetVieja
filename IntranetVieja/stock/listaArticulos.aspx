<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="listaArticulos.aspx.cs" Inherits="stock_listaArticulos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">

<style type="text/css">
    .tbl_width {width:850px;}
    .col_codigo {width:130px;}
    .col_descripcion {width:400px;}
    .col_cantidad {width:75px;}
    .col_ptopedido {width:80px;}
    .col_sc {width:75px;}
    .col_acciones {width:90px;}
    
    .tbl_product_width {width:475px;}
</style>

<script language="javascript" type="text/javascript">
    var current_page;
    var current_product;
    var current_product_count;

    $(document).ready(function () {
        $('#dgArticulos').bindDatagridEvents(ActualizarTabla);

        $('#btnAgregarArticulo').click(function () {
            LimpiarAgregarArticulo();
            MostrarVentana('divAgregarArticulo');
            $('#txtAddCodigo').select();
        });
        $('#btnAddAceptar').click(function () {
            OnAgregarArticulo();
        });
        $('#btnAddCancelar').click(function () {
            LimpiarAgregarArticulo();
            CerrarVentana();
        });
        $('#btnIngEgAceptar').click(function () {
            OnIngresoEgreso();
        });
        $('#btnIngEgCancelar').click(function () {
            LimpiarIngresoEgreso();
            CerrarVentana();
        });
        $('#btnEquipoProduccion').click(function () {
            LimpiarEquipo();

            MostrarVentana('divEquipoProduccion');
        });
        $('#btnEquipoAceptar').click(function () {
            OnShowProductPreview();
        });
        $('#btnEquipoCancelar').click(function () {
            LimpiarEquipo();
            CerrarVentana();
        });
        $('#btnProductPreviewAccept').click(function () {
            OnEquipoProduccion();
        });
        $('#btnProductPreviewCancel').click(function () {
            LimpiarEquipo();
            CerrarVentana();
        });
        $('#txtEquipoCount').keypress(function (e) {
            if (e.which == 13) {
                OnEquipoComprobarDisponibilidad();
            }
        });
        $('#btnEquipoDisp').click(function (e) {
            OnEquipoComprobarDisponibilidad();
        });
        $('#btnPtoPedidoActualizar').click(function () {
            OnActualizarPuntoPedido();
        });
        $('#btnPtoPedidoCancelar').click(function () {
            LimpiarPuntoPedido();
            CerrarVentana();
        });
        $('#txtPtoPedidoNuevo').keypress(function (e) {
            if (e.which == 13) {
                OnActualizarPuntoPedido();
            }
        });
        $('#cbEquipoDesc').change(function () {
            LimpiarBtnDisponibilidad();

            $('#txtEquipoCount').val('0');
            $('#txtEquipoCount').removeAttr('readonly');

            $('#btnEquipoAceptar').hide();
        });

        current_product = current_product_count = null;

        ActualizarTabla();
    });

    function ActualizarTabla() {
        var grid = $('#dgArticulos');

        grid.updatingDatagrid();

        var current_page = grid.GetCurrentPage();

        $.ajax({
            url: 'listaArticulos.aspx/GetArticulos',
            type: "POST",
            dataType: "json",
            data: '{ "pagina": "' + current_page + '" }',
            contentType: "application/json; charset=utf-8",
            async: true,
            cache: false,
            success: function (msg) {
                grid.setRows('');
                var count = msg.d.length;
                var rows = [];

                if (count > 0) {
                    var i = 0;
                    for (i; i < count; i++) {
                        var row = '<tr>';
                        row += '<td class="text align-center col_codigo">' + msg.d[i][1] + '</td>';
                        row += '<td class="text align-left col_descripcion"><span>' + msg.d[i][2] + '</span></td>';
                        row += '<td class="text number col_cantidad"><span class="' + (GetNumber(msg.d[i][3]) < GetNumber(msg.d[i][4]) ? 'important' : '') + '">' + GetNumber(msg.d[i][3]) + '</span></td>';
                        row += '<td class="text number col_ptopedido"><div class="hasActions"><span>' + msg.d[i][4] + '</span><div class="input_actions input_actions_left"><ul><li class="pencil" title="Actualizar punto de pedido" alt="Actualizar punto de pedido" onclick="DoActualizarPuntoPedido(\'' + msg.d[i][0] + '\', ' + msg.d[i][4] + ');"></li></ul></div></div></td>';
                        row += '<td class="text align-center col_sc"><span>' + GetNumber(msg.d[i][5]) + '</span></td>';
                        row += '<td class="actions col_acciones"><div class="bDiv">';
                        if (msg.d[i][6] != 1) {
                            row += '<div class="bGroup" onclick="DoIngreso(\'' + msg.d[i][0] + '\');" title="Registrar ingreso"><div class="iButton iAdd"><span></span></div></div>';
                        }
                        row += '<div class="bGroup" onclick="DoEgreso(\'' + msg.d[i][0] + '\');" title="Registrar egreso"><div class="iButton iDelete"><span></span></div></div>';
                        row += '</div></td>';
                        row += '</tr>';

                        rows.push(row);
                    }
                }
                else {
                    rows.push('<tr><td colspan="6" class="align-center text tbl_width">No hay artículos disponibles.</td></tr>');
                }

                grid.setRows(rows.join(''));

                EnlazarEventosMaster();

                ActualizarTablaPaginas();
            },
            error: function (data, ajaxOptions, thrownError) {
                ErrorMsg(GetAjaxError(data));

                grid.endUpdatingDatagrid();
            }
        });
    }

    function ActualizarTablaPaginas() {
        var grid = $('#dgArticulos');

        $.ajax({
            url: 'listaArticulos.aspx/GetArticulosPaginas',
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: true,
            cache: false,
            success: function (msg) {
                grid.SetMaxPages(msg.d);
                grid.endUpdatingDatagrid();
            },
            error: function (data, ajaxOptions, thrownError) {
                ErrorMsg(GetAjaxError(data));

                grid.endUpdatingDatagrid();
            }
        });
    }

    function LimpiarIngresoEgreso() {
        LimpiarCampo($('#txtIngEgCount'), 'input_wrapper');
        LimpiarCampo($('#txtIngEgDesc'), 'textarea_wrapper');

        $('#txtIngEgCount').val('0');

        current_id = null;
        current_op = null;
    }

    var current_id;
    var current_op;
    function DoIngreso(id) {
        LimpiarIngresoEgreso();

        current_id = id;
        current_op = 'ingreso';

        $('#lblIngEgTitle').text('Ingreso de stock');

        MostrarVentana('divIngresoEgreso');
        $('#txtIngEgCount').focus().select();
    }

    function DoEgreso(id) {
        LimpiarIngresoEgreso();

        current_id = id;
        current_op = 'egreso';

        $('#lblIngEgTitle').text('Egreso de stock');

        $('#txtIngEgCount').focus();
        MostrarVentana('divIngresoEgreso');
    }

    function OnIngresoEgreso() {
        var result = true;
        var cantidad = $('#txtIngEgCount');
        var descripcion = $('#txtIngEgDesc');

        cantidad.val(GetNumber(cantidad.val()));

        result &= ContieneNumeros(cantidad, 'input_wrapper', 'input_wrapper_error') && ContieneValorDiferente(cantidad, '0', 'input_wrapper', 'input_wrapper_error');
        result &= TieneDatos(descripcion, 'textarea_wrapper', 'textarea_wrapper_error');

        if (result) {
            MostrarLoading();

            $.ajax({
                url: 'listaArticulos.aspx/' + (current_op == 'ingreso' ? 'IngresoStock' : 'EgresoStock'),
                type: "POST",
                data: JSON.stringify({ idArticulo: current_id, cantidad: cantidad.val(), descripcion: descripcion.val() }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                async: true,
                cache: false,
                success: function (msg) {
                    LimpiarIngresoEgreso();
                    CerrarVentana();
                    ActualizarTabla();
                },
                error: function (data, ajaxOptions, thrownError) {
                    ult_ventana = 'divIngresoEgreso';
                    ErrorMsg(GetAjaxError(data));
                }
            });
        }
    }

    function LimpiarAgregarArticulo() {
        LimpiarCampo($('#txtAddCodigo'), 'input_wrapper');
        LimpiarCampo($('#txtAddPuntoPedido'), 'input_wrapper');

        $('#txtAddPuntoPedido').val('0');
    }

    function OnAgregarArticulo() {
        var result = true;
        var codigo = $('#txtAddCodigo');
        var puntoPedido = $('#txtAddPuntoPedido');

        puntoPedido.val(GetNumber(puntoPedido.val()));

        result &= TieneDatos(codigo, 'input_wrapper', 'input_wrapper_error');
        result &= ContieneNumeros(puntoPedido, 'input_wrapper', 'input_wrapper_error') && ContieneValorDiferente(puntoPedido, '0', 'input_wrapper', 'input_wrapper_error');
        

        if (result) {
            MostrarLoading();

            $.ajax({
                url: 'listaArticulos.aspx/AgregarArticulo',
                type: "POST",
                data: JSON.stringify({ codigo: codigo.val(), puntoPedido: parseInt(puntoPedido.val()) }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                async: true,
                cache: false,
                success: function (msg) {
                    LimpiarAgregarArticulo();
                    CerrarVentana();

                    ActualizarTabla();
                },
                error: function (data, ajaxOptions, thrownError) {
                    ult_ventana = 'divAgregarArticulo';
                    ErrorMsg(GetAjaxError(data));
                }
            });
        }
    }

    function LimpiarEquipo() {
        LimpiarCampo($('#contentPlacePage_cbEquipoDesc'), 'input_wrapper_selectbox');
        LimpiarCampo($('#txtEquipoCount'), 'input_wrapper');

        $('#contentPlacePage_cbEquipoDesc option:first-child').attr('selected', 'selected');
        $('#contentPlacePage_cbEquipoDesc').change();

        LimpiarBtnDisponibilidad();

        $('#txtEquipoCount').val('0');
        $('#txtEquipoCount').removeAttr('readonly');

        $('#btnEquipoAceptar').hide();
    }

    function OnShowProductPreview() {
        var equipo = $('#contentPlacePage_cbEquipoDesc');
        var cantidad = $('#txtEquipoCount');
        var result = true;

        cantidad.val(GetNumber(cantidad.val()));

        result &= TieneDatos(equipo, 'input_wrapper_selectbox', 'input_wrapper_selectbox_error');
        result &= ContieneNumeros(cantidad, 'input_wrapper', 'input_wrapper_error') && ContieneValorDiferente(cantidad, '0', 'input_wrapper', 'input_wrapper_error');

        if (result) {
            MostrarLoading();

            var grid = $('#dgProduct');

            $.ajax({
                url: 'equiposDisponib.aspx/GetArticulos',
                type: "POST",
                dataType: "json",
                data: '{ "idEquipo": "' + equipo.val() + '", "cantidad": "' + cantidad.val() + '" }',
                contentType: "application/json; charset=utf-8",
                async: true,
                cache: false,
                success: function (msg) {
                    $('#lblProduct').text($('#contentPlacePage_cbEquipoDesc option:selected').text());
                    $('#lblProductCount').text(cantidad.val());

                    grid.setRows('');
                    var count = msg.d.length;
                    var rows = [];

                    if (count > 0) {
                        var i = 0;
                        for (i; i < count; i++) {
                            var row = '<tr>';
                            row += '<td class="text align-left col_descripcion"><span>' + msg.d[i][1] + '</span></td>';
                            row += '<td class="text number col_cantidad"><span>' + GetNumber(msg.d[i][3]) + '</span></td>';
                            row += '</tr>';

                            rows.push(row);
                        }
                    }
                    else {
                        rows.push('<tr><td colspan="2" class="align-center text tbl_product_width">No hay artículos disponibles.</td></tr>');
                    }

                    grid.setRows(rows.join(''));
                    grid.endUpdatingDatagrid();

                    current_product = equipo.val();
                    current_product_count = cantidad.val();

                    MostrarVentana('divProductPreview');
                },
                error: function (data, ajaxOptions, thrownError) {
                    ErrorMsg(GetAjaxError(data));

                    current_product = current_product_count = null;
                }
            });
        }
    }

    function OnEquipoProduccion() {
        var result = true;
        var equipo = $('#contentPlacePage_cbEquipoDesc');
        var cantidad = $('#txtEquipoCount');

        cantidad.val(GetNumber(cantidad.val()));

        result &= TieneDatos(equipo, 'input_wrapper_selectbox', 'input_wrapper_selectbox_error');
        result &= ContieneNumeros(cantidad, 'input_wrapper', 'input_wrapper_error') && ContieneValorDiferente(cantidad, '0', 'input_wrapper', 'input_wrapper_error');
        
        if (result) {
            MostrarLoading();

            $.ajax({
                url: 'listaArticulos.aspx/ProduccionEquipo',
                type: "POST",
                data: JSON.stringify({ idEquipo: equipo.val(), cantidad: parseInt(cantidad.val()) }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                async: true,
                cache: false,
                success: function (msg) {
                    Mensaje('El equipo se ha agregado a producción.', 'success', true, false, 'Aceptar', '', 'CerrarVentana()', '');

                    LimpiarEquipo();
                    ActualizarTabla();
                },
                error: function (data, ajaxOptions, thrownError) {
                    ult_ventana = 'divEquipoProduccion';
                    ErrorMsg(GetAjaxError(data));
                }
            });
        }
    }

    var on_comprobando = false;
    function OnEquipoComprobarDisponibilidad() {
        if (!on_comprobando) {            
            $('#btnEquipoAceptar').hide();

            var equipo = $('#contentPlacePage_cbEquipoDesc');
            var cantidad = $('#txtEquipoCount');
            var result = true;

            cantidad.val(GetNumber(cantidad.val()));
            result &= TieneDatos(equipo, 'input_wrapper_selectbox', 'input_wrapper_selectbox_error');
            result &= ContieneNumeros(cantidad, 'input_wrapper', 'input_wrapper_error') && ContieneValorDiferente(cantidad, '0', 'input_wrapper', 'input_wrapper_error');

            if (result) {
                on_comprobando = true;

                $('#txtEquipoCount').attr('readonly', 'readonly');
                LimpiarBtnDisponibilidad();
                $('#btnEquipoDisponib').addClass('load').attr('title', 'Comprobando disponibilidad...');

                $.ajax({
                    url: 'listaArticulos.aspx/EquipoDisponibilidad',
                    type: "POST",
                    data: JSON.stringify({ idEquipo: equipo.val(), cantidad: parseInt(cantidad.val()) }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    async: true,
                    cache: false,
                    success: function (msg) {
                        $('#btnEquipoAceptar').show();
                        $('#txtEquipoCount').removeAttr('readonly');
                        LimpiarBtnDisponibilidad();
                        $('#btnEquipoDisponib').addClass('ok').attr('title', 'Artículos disponibles');
                        on_comprobando = false;
                    },
                    error: function (data, ajaxOptions, thrownError) {
                        var c = GetAjaxError(data);

                        $('#btnEquipoAceptar').hide();
                        $('#txtEquipoCount').removeAttr('readonly');
                        LimpiarBtnDisponibilidad();
                        if (c == '0') {
                            $('#btnEquipoDisponib').addClass('warning').addClass('link').attr('title', 'Artículos faltantes').html('<span onclick="VerFaltantesEquipo(\'' + equipo.val() + '\');">ver detalle</span>');
                        } else {
                            $('#btnEquipoDisponib').addClass('error').attr('title', 'Error');
                        }
                        on_comprobando = false;
                    }
                });
            }
            else {
                LimpiarBtnDisponibilidad();
                $('#btnEquipoDisponib').addClass('search');                
            }
        }
    }

    function LimpiarBtnDisponibilidad() {
        $('#btnEquipoDisponib').removeClass('ok').removeClass('error').removeClass('load').removeClass('warning').removeClass('link').html('').attr('title', 'Comprobar disponibilidad');
    }

    function VerFaltantesEquipo(id) {
        location.href = 'equiposDisponib.aspx?p=' + id + '&c=' + $('#txtEquipoCount').val();
    }

    function DoActualizarPuntoPedido(id, ptoPedido) {
        LimpiarPuntoPedido();

        current_id = id;

        $('#lblPtoPedidoActual').text(ptoPedido);

        MostrarVentana('divActualizarPtoPedido');
        $('#txtPtoPedidoNuevo').focus().select();
    }

    function LimpiarPuntoPedido() {
        LimpiarCampo($('#txtPtoPedidoNuevo'), 'input_wrapper');

        $('#txtPtoPedidoNuevo').val('0');

        current_id = null;
    }

    function OnActualizarPuntoPedido() {
        var result = true;
        var puntoPedido = $('#txtPtoPedidoNuevo');

        puntoPedido.val(GetNumber(puntoPedido.val()));

        result &= ContieneNumeros(puntoPedido, 'input_wrapper', 'input_wrapper_error') && ContieneValorDiferente(puntoPedido, '0', 'input_wrapper', 'input_wrapper_error');


        if (result) {
            MostrarLoading();

            $.ajax({
                url: 'listaArticulos.aspx/ActualizarPuntoPedido',
                type: "POST",
                data: JSON.stringify({ idArticulo: current_id, puntoPedido: puntoPedido.val() }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                async: true,
                cache: false,
                success: function (msg) {
                    LimpiarPuntoPedido();
                    CerrarVentana();
                    ActualizarTabla();
                },
                error: function (data, ajaxOptions, thrownError) {
                    ult_ventana = 'divActualizarPtoPedido';
                    ErrorMsg(GetAjaxError(data));
                }
            });
        }
    }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlacePage" Runat="Server">

<div class="page-title">
    <h1>Listado de artículos en stock</h1>
</div>

<div class="full-width">
    <div class="datagrid-container" style="width:860px;">
        <table id="dgArticulos" class="datagrid tbl_width" cellspacing="0" cellpadding="0">
            <thead>
                <tr>
                    <td class="border_left col_codigo">Código</td>
                    <td class="border_middle col_descripcion">Descripción</td>
                    <td class="border_middle col_cantidad">Cantidad</td>
                    <td class="border_middle col_ptopedido">Pto pedido</td>
                    <td class="border_middle col_sc">Nº SC</td>
                    <td class="border_right col_acciones">Acciones</td>
                </tr>
                <tr class="filter_row">
                    <td class="col_codigo"></td>
                    <td class="col_descripcion"></td>
                    <td class="col_cantidad"></td>
                    <td class="col_ptopedido"></td>
                    <td class="col_sc"></td>
                    <td class="col_acciones"></td>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td colspan="3">
                        <div class="blockDiv"></div>
                        <div class="scrollable">
                            <table cellpadding="0" cellspacing="0" class="tbl_width">
                                <tr>
                                    <td class="align-center text tbl_width">No hay información disponible.</td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </tbody>
            <tfoot>
                <tr>
                    <td colspan="3" class="border">
                        <div class="bDiv">
                            <div class="bGroup">
                                <div class="tButton" id="btnAgregarArticulo">
                                    <div>
                                        <span class="add" style="padding-left:20px">Agregar artículo</span>
                                    </div>
                                </div>
                            </div>
                            <div class="bGroup">
                                <div class="tButton" id="btnEquipoProduccion">
                                    <div>
                                        <span class="hammer" style="padding-left:20px">Producción de equipo</span>
                                    </div>
                                </div>
                            </div>
                            <div class="bGroup">
                                <div class="iSeparator">                        
                                </div>
                            </div>
                            <div class="bGroup">
                                <div class="iButton iFirst">
                                    <span></span>
                                </div>
                            </div>
                            <div class="bGroup">
                                <div class="iButton iPrev">
                                    <span></span>
                                </div>
                            </div>
                            <div class="bGroup">
                                <div class="iSeparator">                        
                                </div>
                            </div>
                            <div class="bGroup">
                                <span class="bControl">
                                    Página
                                    <input class="dg-page-ctrl" type="text" size="3" value="1" />
                                    de
                                    <span class="dg-page-count">1</span>
                                </span>
                            </div>
                            <div class="bGroup">
                                <div class="iSeparator">                        
                                </div>
                            </div>
                            <div class="bGroup">
                                <div class="iButton iNext">
                                    <span></span>
                                </div>
                            </div>
                            <div class="bGroup">
                                <div class="iButton iLast">
                                    <span></span>
                                </div>
                            </div>
                            <div class="bLoading"></div>
                        </div>
                    </td>
                </tr>
            </tfoot>
        </table>
    </div>
</div>

<div class="dialog_wrapper" id="divIngresoEgreso">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3 id="lblIngEgTitle"></h3></div></div></div>
    </div>

    <div class="dialog_content">
        <ul class="middle_form">
            <li class="form_floated_item form_floated_item_50">
                <label class="label">Cantidad</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtIngEgCount" maxlength="6" value="" type="text"/>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_100">
                <label class="label">Ingrese una descripción de la operación</label>
                <div class="textarea_wrapper"> 
	                <div class="top">
		                <div class="cap"></div>
	                </div>
	                <div class="inner">
		                <div class="cap">
			                <textarea id="txtIngEgDesc" maxlength="100" onkeyup="return MaxLength(this)"></textarea>     
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
            <li><div class="btn primary_action_button_small button_100" id="btnIngEgAceptar"><div class="cap"><span>Aceptar</span></div></div></li>
            <li><div class="btn secondary_action_button_small button_100" id="btnIngEgCancelar"><div class="cap"><span>Cancelar</span></div></div></li>
        </ul>
    </div>
</div>

<div class="dialog_wrapper" style="width:400px" id="divAgregarArticulo">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3>Agregar artículo</h3></div></div></div>
    </div>

    <div class="dialog_content">
        <ul class="middle_form">
            <li class="form_floated_item form_floated_item_50">
                <label class="label">Código</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtAddCodigo" maxlength="15" value="" type="text"/>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_50 form_floated_item_right">
                <label class="label">Punto de pedido</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtAddPuntoPedido" maxlength="6" value="" type="text"/>
                    </div>
                </div>
            </li>
        </ul>
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li><div class="btn primary_action_button_small button_100" id="btnAddAceptar"><div class="cap"><span>Agregar</span></div></div></li>
            <li><div class="btn secondary_action_button_small button_100" id="btnAddCancelar"><div class="cap"><span>Cancelar</span></div></div></li>
        </ul>
    </div>
</div>

<div class="dialog_wrapper" style="width:400px" id="divActualizarPtoPedido">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3>Actualizar punto de pedido</h3></div></div></div>
    </div>

    <div class="dialog_content">
        <ul class="middle_form">
            <li class="form_floated_item form_floated_item_50">
                <label class="label">Punto de pedido actual</label>
                <span id="lblPtoPedidoActual"></span>
            </li>
            <li class="form_floated_item form_floated_item_50 form_floated_item_right">
                <label class="label">Nuevo punto de pedido</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtPtoPedidoNuevo" maxlength="6" value="" type="text"/>
                    </div>
                </div>
            </li>
        </ul>
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li><div class="btn primary_action_button_small button_100" id="btnPtoPedidoActualizar"><div class="cap"><span>Actualizar</span></div></div></li>
            <li><div class="btn secondary_action_button_small button_100" id="btnPtoPedidoCancelar"><div class="cap"><span>Cancelar</span></div></div></li>
        </ul>
    </div>
</div>

<div class="dialog_wrapper" id="divEquipoProduccion">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3>Producción de equipo</h3></div></div></div>
    </div>

    <div class="dialog_content">
        <ul class="middle_form" style="width:400px; margin:0 auto;">
            <li class="form_floated_item form_floated_item_100">
                <label class="label">Equipo</label>
                <div class="input_wrapper input_wrapper_selectbox">
                    <div class="cap">
                        <span id="lblEquipoDesc" runat="server"></span>
                        <select id="cbEquipoDesc" runat="server"></select>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_50">
                <label class="label">Cantidad</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtEquipoCount" maxlength="6" value="" type="text"/>
                    </div>
                    <div id="btnEquipoDisponib" title="Comprobar disponibilidad" class="input_button input_button_inner input_button_text search"></div>
                </div>
            </li>
        </ul>
        <div class="clear"></div>
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li><div class="btn primary_action_button_small button_100" id="btnEquipoAceptar"><div class="cap"><span>Aceptar</span></div></div></li>
            <li><div class="btn secondary_action_button_small button_100" id="btnEquipoCancelar"><div class="cap"><span>Cancelar</span></div></div></li>
        </ul>
    </div>
</div>

<div class="dialog_wrapper" id="divProductPreview">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3>Producción de equipo - Vista previa</h3></div></div></div>
    </div>

    <div class="dialog_content">
        <ul class="middle_form" style="width:400px; margin:0 auto;">
            <li class="form_floated_item form_floated_item_100">
                <label class="label">Equipo</label>
                <span id="lblProduct"></span>
            </li>
            <li class="form_floated_item form_floated_item_50">
                <label class="label">Cantidad</label>
                <span id="lblProductCount"></span>
            </li>
        </ul>
        <div class="clear"></div>
        <table id="dgProduct" class="datagrid tbl_product_width" cellspacing="0" cellpadding="0">
            <thead>
                <tr>
                    <td class="border_left col_descripcion">Código</td>
                    <td class="border_right col_cantidad">Cantidad</td>
                </tr>
                <tr class="filter_row">
                    <td class="col_descripcion"></td>
                    <td class="col_cantidad"></td>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td colspan="3">
                        <div class="blockDiv"></div>
                        <div class="scrollable">
                            <table cellpadding="0" cellspacing="0" class="tbl_product_width">
                                <tr>
                                    <td class="align-center text tbl_product_width">No hay información disponible.</td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </tbody>
            <tfoot>
                <tr>
                    <td colspan="3" class="border">
                    </td>
                </tr>
            </tfoot>
        </table>
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li><div class="btn primary_action_button_small button_100" id="btnProductPreviewAccept"><div class="cap"><span>Aceptar</span></div></div></li>
            <li><div class="btn secondary_action_button_small button_100" id="btnProductPreviewCancel"><div class="cap"><span>Cancelar</span></div></div></li>
        </ul>
    </div>
</div>

</asp:Content>

