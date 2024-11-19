<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="vehiculosVencimientos.aspx.cs" Inherits="administracion_vehiculosVencimientos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">

<!--[if lt IE 9]>     
<script src="/js/IE9.js" type="text/javascript"></script>
<![endif]-->

<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        $('#txtFecha').datepicker({ dayNames: dayNames, dayNamesMin: dayNamesMin, monthNames: monthNames, dateFormat: 'mm/yy', selectOtherMonths: true });

        $('#txtFecha').change(function () {
            var f = $(this).val().split('/');

            GetVencimientos(f[0], f[1]);
        });

        $('#btnExportar').click(function () {
            MostrarLoading();

            var f = $('#txtFecha').val().split('/');

            ConsultaAjax.url = 'vehiculosVencimientos.aspx/ExportarVencimientos';
            ConsultaAjax.data = '{ "mes":"' + f[0] + '", "anio":"' + f[1] + '" }';
            ConsultaAjax.AjaxSuccess = function (msg) {
                location.href = msg.d;

                CerrarVentana();
            };
            ConsultaAjax.AjaxError = function (msg) {
                ErrorMsg(msg);
            };

            ConsultaAjax.Ejecutar();
        });

        GetVencimientosActual();
    });

    function GetVencimientosActual() {
        var currDate = new Date();
        var currDay = currDate.getDate();
        var currMonth = currDate.getMonth();
        var currYear = currDate.getFullYear();

        if (currMonth > 12) {
            currMonth = 1;
            currYear = currYear + 1;
        }

        fecha = new Date(currYear, currMonth, 1, 1, 1, 1, 1);

        $('#txtFecha').val(fecha.toString('MM/yyyy'));

        $('#txtFecha').change();
    }

    function GetVencimientos(mes, anio) {
        MostrarLoading();

        ConsultaAjax.url = 'vehiculosVencimientos.aspx/GetVencimientos';
        ConsultaAjax.data = '{ "mes":"' + mes + '", "anio":"' + anio + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            $('#vencimientos').html();
            var cant = msg.d.length;

            if (cant > 0) {
                var filas = [];
                var i = 0;
                for (i; i < cant; i++) {
                    filas.push(GetTabla(msg.d[i]));
                }

                $('#vencimientos').html(filas.join(''));
                $('#botones').show();
            }
            else {
                $('#botones').hide();
                $('#vencimientos').html('No hay vencimientos para el mes seleccionado.');
            }

            CerrarVentana();
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function GetTabla(item) {
        var tabla = '';

        tabla += '<table class="tbl listado" style="width:350px" cellspacing="0">';
        tabla += '<thead>';
        tabla += '<tr>';
        tabla += '<td class="border_left border_right align-center" colspan="2">' + item[0] + '</td>';
        tabla += '</tr>';
        tabla += '<tr>';
        tabla += '<td class="border_left" style="width:150px">Fecha de vencimiento</td>';
        tabla += '<td class="border_right">Dominio</td>';
        tabla += '</tr>';
        tabla += '</thead>';
        tabla += '<tbody>';

        var cant = item.length;
        for (var i = 1; i < cant; i++) {
            tabla += '<tr>';
            tabla += '<td class="align-center">' + item[i][0] + '</td>';
            tabla += '<td class="align-center">' + item[i][1] + '</td>';
            tabla += '</tr>';
        }

        tabla += '</tbody>';
        tabla += '<tfoot>';
        tabla += '<tr>';
        tabla += '<td colspan="2" class="align-center">';
        tabla += '</td>';
        tabla += '</tr>';
        tabla += '</tfoot>';
        tabla += '</table>';
        tabla += '</br>';

        return tabla;
    }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlacePage" Runat="Server">

<div class="page-title">
    <h1>Vehículos: listado de vencimientos</h1>
</div>

<div class="form_place">
    <h3>Período</h3>
    <ul class="middle_form" style="height:60px">
        <li class="form_floated_item form_floated_item_half">
            <label class="label">Seleccione el mes</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtFecha" readonly="readonly" value="" maxlength="10" type="text"/>
                </div>
            </div>
        </li>
    </ul>
    <br />

    <h3>Vencimientos</h3>
    <br />
    <div id="vencimientos">
    </div>
    
    <div class="form_buttons_container" id="botones" style="display:none">
        <ul class="button_list">
            <li id="btnExportar"><div class="btn primary_action_button button_150"><div class="cap"><span>Exportar a Excel</span></div></div></li>
        </ul>
    </div>
</div>

</asp:Content>

