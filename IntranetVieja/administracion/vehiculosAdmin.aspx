﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="vehiculosAdmin.aspx.cs" Inherits="administracion_vehiculosAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">

<!--[if lt IE 9]>     
<script src="/js/IE9.js" type="text/javascript"></script>
<![endif]-->

<script language="javascript" type="text/javascript">
    var items = [];
    var personas = [];
    var tipos = [];

    $(document).ready(function () {
        $('#btnGuardar').click(function () {
            Guardar();
        });
        $('#btnExportar').click(function () {
            MostrarLoading();

            ConsultaAjax.url = 'vehiculosAdmin.aspx/ExportarVehiculos';
            ConsultaAjax.AjaxSuccess = function (msg) {
                location.href = msg.d;

                CerrarVentana();
            };
            ConsultaAjax.AjaxError = function (msg) {
                ErrorMsg(msg);
            };

            ConsultaAjax.Ejecutar();
        });

        GetPersonas();
        GetTipos();
        GetVehiculos();

        EnlazarEventosMaster();

        $('select').change();

        $('.dtPicker').datepicker({ dayNames: dayNames, dayNamesMin: dayNamesMin, monthNames: monthNames, dateFormat: 'dd/mm/yy' });
    });

    function DibujarAccionesFecha() {
        $('.actions_fecha').html('<ul><li class="delete" title="No aplica"></li></ul>');
    }

    var EnlazarEventos = function () {
        $('.actions_fecha ul li.delete').unbind('click').click(function () {
            var tipo = $(this).parents('.input_actions').attr('name');
            var value = $(this).parents('.input_actions').attr('value');

            $('#' + value).val('<%=Vehiculos.NoAplica %>').removeClass('red').removeClass('yellow').removeClass('green');
        });
    }

    function GetVehiculos() {
        MostrarLoading();

        ConsultaAjax.url = 'vehiculosAdmin.aspx/GetVehiculos';
        ConsultaAjax.AjaxSuccess = function (msg) {
            $('#vehiculos').html();
            var cant = msg.d.length;

            if (cant > 0) {
                var filas = [];
                var i = 0;
                for (i; i < cant; i++) {
                    var fila = '<tr class="fila-color">';
                    fila += '<td class="align-center">' + msg.d[i][1] + '</td>';
                    fila += '<td><div class="input_wrapper"><div class="cap"><input title="' + msg.d[i][1] + ' - Modelo" class="align-left" id="txtModelo_' + i + '" value="' + msg.d[i][2] + '" type="text"/></div></div></td>';
                    fila += GetCeldaTipoVehiculo(i, msg.d[i][3]);
                    fila += '<td><div class="input_wrapper"><div class="cap"><input title="' + msg.d[i][1] + ' - Año" class="align-right" id="txtAnio_' + i + '" value="' + msg.d[i][4] + '" type="text"/></div></div></td>';
                    fila += '<td><div class="input_wrapper"><div class="cap"><input title="' + msg.d[i][1] + ' - Ubicación" class="align-left" id="txtUbicacion_' + i + '" value="' + msg.d[i][5] + '" type="text"/></div></div></td>';
                    fila += GetCeldaResponsable(i, msg.d[i][6]);
                    fila += '<td class="align-center"><input title="' + msg.d[i][1] + ' - Afectado a Gasmed" id="chkGasmed_' + i + '" type="checkbox" ' + (msg.d[i][7] == 1 ? 'checked="checked"' : '') + ' /></td>';
                    fila += '<td><div class="hasActions"><div class="input_wrapper"><div class="cap"><input title="' + msg.d[i][1] + ' - Vto Cédula verde" class="align-left dtPicker ' + GetInputColor(msg.d[i][8], <%= Vehiculos.MinDiasCedula.ToString() %>) + '" id="txtVtoCedulaVerde_' + i + '" value="' + msg.d[i][8] + '" readonly="readonly" type="text"/></div></div><div class="input_actions input_actions_textbox actions_fecha" name="vehiculo" value="txtVtoCedulaVerde_' + i + '"></div></div></td>';
                    fila += '<td><div class="input_wrapper"><div class="cap"><input title="' + msg.d[i][1] + ' - Nro RUTA" class="align-left" id="txtNroRUTA_' + i + '" value="' + msg.d[i][9] + '" type="text"/></div></div></td>';
                    fila += '<td><div class="hasActions"><div class="input_wrapper"><div class="cap"><input title="' + msg.d[i][1] + ' - Vto RUTA" class="align-left dtPicker ' + GetInputColor(msg.d[i][10], <%= Vehiculos.MinDiasAviso.ToString() %>) + '" id="txtVtoRUTA_' + i + '" value="' + msg.d[i][10] + '" readonly="readonly" type="text"/></div></div><div class="input_actions input_actions_textbox actions_fecha" name="vehiculo" value="txtVtoRUTA_' + i + '"></div></div></td>';
                    fila += '<td><div class="hasActions"><div class="input_wrapper"><div class="cap"><input title="' + msg.d[i][1] + ' - Vto VTV" class="align-left dtPicker ' + GetInputColor(msg.d[i][11], <%= Vehiculos.MinDiasAviso.ToString() %>) + '" id="txtVtoVTV_' + i + '" value="' + msg.d[i][11] + '" readonly="readonly" type="text"/></div></div><div class="input_actions input_actions_textbox actions_fecha" name="vehiculo" value="txtVtoVTV_' + i + '"></div></div></td>';
                    fila += '<td><div class="hasActions"><div class="input_wrapper"><div class="cap"><input title="' + msg.d[i][1] + ' - Hab. Prov. Santa Cruz" class="align-left dtPicker ' + GetInputColor(msg.d[i][18], <%= Vehiculos.MinDiasAviso.ToString() %>) + '" id="txtVtoStaCruz_' + i + '" value="' + msg.d[i][18] + '" readonly="readonly" type="text"/></div></div><div class="input_actions input_actions_textbox actions_fecha" name="vehiculo" value="txtVtoStaCruz_' + i + '"></div></div></td>';
                    fila += '<td><div class="hasActions"><div class="input_wrapper"><div class="cap"><input title="' + msg.d[i][1] + ' - Vto Patente" class="align-left dtPicker ' + GetInputColor(msg.d[i][12], <%= Vehiculos.MinDiasAviso.ToString() %>) + '" id="txtVtoPatente_' + i + '" value="' + msg.d[i][12] + '" readonly="readonly" type="text"/></div></div><div class="input_actions input_actions_textbox actions_fecha" name="vehiculo" value="txtVtoPatente_' + i + '"></div></div></td>';
                    fila += '<td><div class="input_wrapper"><div class="cap"><input title="' + msg.d[i][1] + ' - Cia Seguro" class="align-left" id="txtCiaSeguro_' + i + '" value="' + msg.d[i][13] + '" type="text"/></div></div></td>';
                    fila += '<td><div class="input_wrapper"><div class="cap"><input title="' + msg.d[i][1] + ' - Póliza Seguro" class="align-left" id="txtCiaSeguroPoliza_' + i + '" value="' + msg.d[i][14] + '" type="text"/></div></div></td>';
                    fila += '<td><div class="hasActions"><div class="input_wrapper"><div class="cap"><input title="' + msg.d[i][1] + ' - Vto Seguro" class="align-left dtPicker ' + GetInputColor(msg.d[i][15], <%= Vehiculos.MinDiasAviso.ToString() %>) + '" id="txtVtoSeguro_' + i + '" value="' + msg.d[i][15] + '" readonly="readonly" type="text"/></div></div><div class="input_actions input_actions_textbox actions_fecha" name="vehiculo" value="txtVtoSeguro_' + i + '"></div></div></td>';
                    fila += '<td><div class="input_wrapper"><div class="cap"><input title="' + msg.d[i][1] + ' - Nro Chasis" class="align-left" id="txtNroChasis_' + i + '" value="' + msg.d[i][16] + '" type="text"/></div></div></td>';
                    fila += '<td><div class="input_wrapper"><div class="cap"><input title="' + msg.d[i][1] + ' - Nro Motor" class="align-left" id="txtNroMotor_' + i + '" value="' + msg.d[i][17] + '" type="text"/></div></div></td>';
                    fila += '<td class="align-center">' + msg.d[i][1] + '</td>';
                    fila += '</tr>';
                    filas.push(fila);
                    items.push('\'' + msg.d[i][0] + '\'');
                }

                $('#vehiculos').html(filas.join(''));
            }
            else {
                $('#vehiculos').html('<tr><td colspan="18" class="align-center">No hay vehículos disponibles.</td></tr>');
            }

            DibujarAccionesFecha();
            EnlazarEventos();

            CerrarVentana();
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function GetInputColor(value, max) {
        if(value == '<%=Vehiculos.NoAplica %>') {
            return 'green';
        }

        var result = '';
        var today = new Date();
        var date = Date.parse(value);

        var ms_day = 1000*60*60*24;              
        var days = (date - today) / ms_day;     

        if(days > max) result = 'green';
        else if(days < max && days > 0) result = 'yellow';
        else result = 'red';

        return result;
    }

    function GetCeldaTipoVehiculo(item, tipo) {
        var fila = '';
        fila += '<td class="align-left"><div class="input_wrapper input_wrapper_selectbox"><div class="cap"><span></span>';
        fila += '<select id="cbTipo_' + item + '">';

        var cp = tipos.length;
        for (var i = 0; i < cp; i++) {
            fila += '<option value="' + tipos[i][0] + '" ' + (tipo == tipos[i][0] ? 'selected="selected"' : '') + '>' + tipos[i][1] + '</option>';
        }
        
        fila += '</select></div></div></td>';

        return fila;
    }

    function GetCeldaResponsable(item, responsable) {
        var fila = '';
        fila += '<td class="align-left"><div class="input_wrapper input_wrapper_selectbox"><div class="cap"><span></span>';
        fila += '<select id="cbResponsable_' + item + '">';

        var cp = personas.length;
        for (var i = 0; i < cp; i++) {
            fila += '<option value="' + personas[i][0] + '" ' + (responsable == personas[i][0] ? 'selected="selected"' : '') + '>' + personas[i][1] + '</option>';
        }

        fila += '</select></div></div></td>';

        return fila;
    }

    function GetPersonas() {
        ConsultaAjax.url = 'vehiculosAdmin.aspx/GetPersonas';
        ConsultaAjax.AjaxSuccess = function (msg) {
            var cant = msg.d.length;

            for (var i = 0; i < cant; i++) {
                personas.push(msg.d[i]);
            }
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function GetTipos() {
        ConsultaAjax.url = 'vehiculosAdmin.aspx/GetTipos';
        ConsultaAjax.AjaxSuccess = function (msg) {
            var cant = msg.d.length;

            for (var i = 0; i < cant; i++) {
                tipos.push(msg.d[i]);
            }
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function GetCeldaCategoria(categoria) {
        var result;

        result = '<td class="align-center" style="color:';
        if (categoria == 'NC') {
            result += '#FF0000';
        }
        else if (categoria == 'OBS') {
            result += '#1f971c';
        }
        else if (categoria == 'STOCK') {
            result += '#0000FF';
        }
        else {
            result += '#404040';
        }
        result += '">' + categoria + '</td>';

        return result;
    }

    function GetValor(item, campo) {
        var el = $('#' + campo + '_' + item);

        if (campo.startsWith('chk')) {
            return el.is(':checked');
        } else {
            return el.val();
        }
    }

    function Guardar() {
        var datos = [];

        var c = items.length;
        for (var i = 0; i < c; i++) {
            var dato = [items[i], GetValor(i, 'txtModelo'), GetValor(i, 'cbTipo'), GetValor(i, 'txtAnio'),
                GetValor(i, 'txtUbicacion'), GetValor(i, 'cbResponsable'), GetValor(i, 'chkGasmed'), GetValor(i, 'txtVtoCedulaVerde'),
                GetValor(i, 'txtNroRUTA'), GetValor(i, 'txtVtoRUTA'), GetValor(i, 'txtVtoVTV'), GetValor(i, 'txtVtoPatente'),
                GetValor(i, 'txtCiaSeguro'), GetValor(i, 'txtCiaSeguroPoliza'), GetValor(i, 'txtVtoSeguro'),
                GetValor(i, 'txtNroChasis'), GetValor(i, 'txtNroMotor'), GetValor(i, 'txtVtoStaCruz')];
            datos.push(dato);
        }

        MostrarLoading();

        ConsultaAjax.url = 'vehiculosAdmin.aspx/ActualizarVehiculos';
        ConsultaAjax.data = JSON.stringify({ datos: datos });
        ConsultaAjax.AjaxSuccess = function (msg) {
            Mensaje(msg.d, 'success', true, false, 'Aceptar', '', 'custom_dialog.close()', '');
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlacePage" Runat="Server">

<div class="page-title">
    <h1>Listado de vehículos</h1>
</div>

<div class="full-width scroll-horizontal" style="height:400px">
    <table class="tbl listado" id="tbl-listado" style="width:2440px" cellspacing="0">
        <thead>
            <tr>
                <td class="border_left" style="width:100px">Dominio</td>
                <td class="border_middle" style="width:250px">Modelo</td>
                <td class="border_middle" style="width:210px">Tipo</td>
                <td class="border_middle" style="width:80px">Año</td>
                <td class="border_middle" style="width:180px">Ubicacion</td>
                <td class="border_middle" style="width:250px">Responsable</td>
                <td class="border_middle doble" style="width:100px">Afectado a Gasmed</td>
                <td class="border_middle doble" style="width:120px">Vto Cedula Verde</td>
                <td class="border_middle doble" style="width:120px">Número R.U.T.A.</td>
                <td class="border_middle" style="width:120px">Vto R.U.T.A.</td>
                <td class="border_middle" style="width:120px">Vto VTV</td>
                <td class="border_middle" style="width:120px">Hab. Prov. Santa Cruz</td>
                <td class="border_middle" style="width:120px">Vto Patente</td>
                <td class="border_middle doble" style="width:120px">Compañía Seguro</td>
                <td class="border_middle doble" style="width:120px">Póliza de Seguro</td>
                <td class="border_middle" style="width:120px">Vto Seguro</td>
                <td class="border_middle doble" style="width:150px">Nº de Chasis</td>
                <td class="border_middle" style="width:150px">Nº de Motor</td>
                <td class="border_right" style="width:100px">Dominio</td>
            </tr>
            <tr class="filter_row">
                <td colspan="19"></td>
            </tr>
        </thead>
        <tbody id="vehiculos">
            <tr>
                <td colspan="19" class="align-center">No hay vehiculos disponibles.</td>
            </tr>
        </tbody>
        <tfoot>
            <tr>
                <td colspan="19" class="align-center" id="tdPaginas">
                </td>
            </tr>
        </tfoot>
    </table>
</div>

<div class="form_buttons_container">
    <ul class="button_list">
        <li id="btnGuardar"><div class="btn primary_action_button button_100"><div class="cap"><span>Guardar</span></div></div></li>
        <li id="btnExportar"><div class="btn secondary_action_button button_150"><div class="cap"><span>Exportar a Excel</span></div></div></li>
    </ul>
</div>

</asp:Content>

