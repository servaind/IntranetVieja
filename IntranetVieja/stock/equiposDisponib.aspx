<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="equiposDisponib.aspx.cs" Inherits="stock_equiposDisponib" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">

<style type="text/css">
    .tbl_width {width:790px;}
    .col_codigo {width:130px;}
    .col_descripcion {width:400px;}
    .col_cantidad {width:130px;}
    .col_cantidadNec {width:130px;}
</style>

<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        $('#txtEquipoCount').keypress(function (e) {
            if (e.which == 13) {
                ActualizarTabla();
            }
        });
        $('#btnEquipoDisp').click(function () {
            alert('asda');
            ActualizarTabla();
        });

        $('#cbEquipoDesc').val('<%= this.EquipoID %>');
        $('#cbEquipoDesc').change();
        $('#txtEquipoCount').val('<%= this.Cantidad.ToString() %>');

        ActualizarTabla();
    });

    function ActualizarTabla() {
        var equipo = $('#contentPlacePage_cbEquipoDesc');
        var cantidad = $('#txtEquipoCount');
        var result = true;

        cantidad.val(GetNumber(cantidad.val()));
        result &= TieneDatos(equipo, 'input_wrapper_selectbox', 'input_wrapper_selectbox_error');
        result &= ContieneNumeros(cantidad, 'input_wrapper', 'input_wrapper_error') && ContieneValorDiferente(cantidad, '0', 'input_wrapper', 'input_wrapper_error');

        if (result) {
            var grid = $('#dgArticulos');

            grid.updatingDatagrid();

            equipo.attr('disabled', 'disabled');
            cantidad.attr('readonly', 'readonly');

            $.ajax({
                url: 'equiposDisponib.aspx/GetArticulos',
                type: "POST",
                dataType: "json",
                data: '{ "idEquipo": "' + equipo.val() + '", "cantidad": "' + cantidad.val() + '" }',
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
                            row += '<td class="text align-center col_codigo">' + msg.d[i][0] + '</td>';
                            row += '<td class="text align-left col_descripcion"><span>' + msg.d[i][1] + '</span></td>';
                            row += '<td class="text number col_cantidad"><span class="' + (GetNumber(msg.d[i][2]) - GetNumber(msg.d[i][3]) < 0 ? 'important' : '') + '">' + GetNumber(msg.d[i][2]) + '</span></td>';
                            row += '<td class="text number col_cantidadNec last_column"><span>' + GetNumber(msg.d[i][3]) + '</span></td>';
                            row += '</tr>';

                            rows.push(row);
                        }
                    }
                    else {
                        rows.push('<tr><td colspan="4" class="align-center text tbl_width">No hay artículos disponibles.</td></tr>');
                    }

                    grid.setRows(rows.join(''));
                    grid.endUpdatingDatagrid();

                    equipo.removeAttr('disabled');
                    cantidad.removeAttr('readonly');
                },
                error: function (data, ajaxOptions, thrownError) {
                    ErrorMsg(GetAjaxError(data));

                    grid.endUpdatingDatagrid();

                    equipo.removeAttr('disabled');
                    cantidad.removeAttr('readonly');
                }
            });
        }
        else {
            LimpiarBtnDisponibilidad();
            $('#btnEquipoDisponib').addClass('search');
        }
    }

    function LimpiarBtnDisponibilidad() {
        $('#btnEquipoDisponib').removeClass('ok').removeClass('error').removeClass('load').removeClass('warning').removeClass('link').html('').attr('title', 'Comprobar disponibilidad');
    }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlacePage" Runat="Server">

<div class="page-title">
    <h1>Comprobador de disponibilidad para equipos</h1>
</div>

<div class="full-width">
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

    <div class="datagrid-container" style="width:820px;">
        <table id="dgArticulos" class="datagrid tbl_width" cellspacing="0" cellpadding="0">
            <thead>
                <tr>
                    <td class="border_left col_codigo">Código</td>
                    <td class="border_middle col_descripcion">Descripción</td>
                    <td class="border_middle col_cantidad">Cantidad actual</td>
                    <td class="border_right col_cantidadNec">Cantidad necesaria</td>
                </tr>
                <tr class="filter_row">
                    <td class="col_codigo"></td>
                    <td class="col_descripcion"></td>
                    <td class="col_cantidad"></td>
                    <td class="col_cantidadNec"></td>
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

</asp:Content>

