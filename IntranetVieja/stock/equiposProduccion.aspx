<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="equiposProduccion.aspx.cs" Inherits="stock_equiposProduccion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">

<style type="text/css">
    .tbl_width {width:600px;}
    .col_codigo {width:130px;}
    .col_descripcion {width:310px;}
    .col_cantidad {width:80px;}
    .col_acciones {width:80px;}
</style>

<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        $('#btnEgAceptar').click(function () {
            OnIngresoEgreso();
        });
        $('#btnEgCancelar').click(function () {
            LimpiarEgreso();
            CerrarVentana();
        });

        ActualizarTabla();
    });

    function ActualizarTabla() {
        var grid = $('#dgArticulos');

        grid.updatingDatagrid();

        var current_page = grid.GetCurrentPage();

        $.ajax({
            url: 'equiposProduccion.aspx/GetEquiposProduccion',
            type: "POST",
            dataType: "json",
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
                        row += '<td class="text number col_cantidad"><span>' + GetNumber(msg.d[i][3]) + '</span></td>';
                        row += '<td class="actions col_acciones"><div class="bDiv">';
                        row += '<div class="bGroup" onclick="DoEgreso(\'' + msg.d[i][0] + '\');" title="Registrar egreso"><div class="iButton iOk"><span></span></div></div>';
                        row += '</div></td>';
                        row += '</tr>';

                        rows.push(row);
                    }
                }
                else {
                    rows.push('<tr><td colspan="4" class="align-center text tbl_width">No hay equipos disponibles.</td></tr>');
                }

                grid.setRows(rows.join(''));
                grid.endUpdatingDatagrid();
            },
            error: function (data, ajaxOptions, thrownError) {
                ErrorMsg(GetAjaxError(data));

                grid.endUpdatingDatagrid();
            }
        });
    }

    function LimpiarEgreso() {
        LimpiarCampo($('#txtEgCount'), 'input_wrapper');
        LimpiarCampo($('#txtEgDesc'), 'textarea_wrapper');

        $('#txtEgCount').val('0');

        current_id = null;
    }

    var current_id;

    function DoEgreso(id) {
        LimpiarEgreso();

        current_id = id;

        $('#txtEgCount').focus();
        MostrarVentana('divEgreso');
    }

    function OnIngresoEgreso() {
        var result = true;
        var cantidad = $('#txtEgCount');
        var descripcion = $('#txtEgDesc');

        cantidad.val(GetNumber(cantidad.val()));

        result &= ContieneNumeros(cantidad, 'input_wrapper', 'input_wrapper_error') && ContieneValorDiferente(cantidad, '0', 'input_wrapper', 'input_wrapper_error');
        result &= TieneDatos(descripcion, 'textarea_wrapper', 'textarea_wrapper_error');

        if (result) {
            MostrarLoading();

            $.ajax({
                url: 'equiposProduccion.aspx/EquipoTerminado',
                type: "POST",
                data: JSON.stringify({ idEquipo: current_id, cantidad: parseInt(cantidad.val()), descripcion: descripcion.val() }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                async: true,
                cache: false,
                success: function (msg) {
                    LimpiarEgreso();
                    CerrarVentana();
                    ActualizarTabla();
                },
                error: function (data, ajaxOptions, thrownError) {
                    ult_ventana = 'divEgreso';
                    ErrorMsg(GetAjaxError(data));
                }
            });
        }
    }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlacePage" Runat="Server">

<div class="page-title">
    <h1>Listado de equipos en producción</h1>
</div>

<div class="full-width">
    <div class="datagrid-container" style="width:620px;">
        <table id="dgArticulos" class="datagrid tbl_width" cellspacing="0" cellpadding="0">
            <thead>
                <tr>
                    <td class="border_left col_codigo">Código</td>
                    <td class="border_middle col_descripcion">Descripción</td>
                    <td class="border_middle col_cantidad">Cantidad</td>
                    <td class="border_right col_acciones">Acciones</td>
                </tr>
                <tr class="filter_row">
                    <td class="col_codigo"></td>
                    <td class="col_descripcion"></td>
                    <td class="col_cantidad"></td>
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
                                    <input class="dg-page-ctrl" type="text" size="3" value="1" readonly="readonly" />
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

<div class="dialog_wrapper" id="divEgreso">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3>Producción terminada</h3></div></div></div>
    </div>

    <div class="dialog_content">
        <ul class="middle_form">
            <li class="form_floated_item form_floated_item_50">
                <label class="label">Cantidad</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtEgCount" maxlength="6" value="" type="text"/>
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
			                <textarea id="txtEgDesc" maxlength="100" onkeyup="return MaxLength(this)"></textarea>     
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
            <li><div class="btn primary_action_button_small button_100" id="btnEgAceptar"><div class="cap"><span>Aceptar</span></div></div></li>
            <li><div class="btn secondary_action_button_small button_100" id="btnEgCancelar"><div class="cap"><span>Cancelar</span></div></div></li>
        </ul>
    </div>
</div>

</asp:Content>

