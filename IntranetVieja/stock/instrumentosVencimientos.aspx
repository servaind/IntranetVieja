<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="instrumentosVencimientos.aspx.cs" Inherits="stock_instrumentosVencimientos" %>

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

        ConsultaAjax.url = 'instrumentosVencimientos.aspx/GetVencimientos';
        ConsultaAjax.data = '{ "mes":"' + mes + '", "anio":"' + anio + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            var count = msg.d.length;
            var results = [];

            if (count > 0) {
                var i = 0;
                for (i; i < count; i++) {
                    results.push(GetInstrumento(msg.d[i]));
                }
            }
            else {
                results.push('<div class="suggestion_message success">No se encontraron vencimientos para el intervalo especificado.</div>');
            }


            $('#inst-results').html(results.join(''));

            $('#inst-results').scrollTop(0);

            CerrarVentana();
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function GetInstrumento(r) {
        var result;

        result = '<div class="inst-container" id="' + r.ID + '">';
        result += '<div class="numero">' + r.Numero + '</div>';
        result += '<div class="foto"><img src="/getImage.aspx?p=' + r.PathImagen + '" /><div class="lupa"></div></div>';
        result += '<div class="info-container">';
        result += '<p class="tipo">' + r.Tipo + '</p>';    // [Tipo]
        result += '<p class="marca">' + r.Marca + ' ' + r.Modelo + ' <span class="serie" title="Nº de serie">(' + r.NumeroSerie + ')</span></p>';    // [Marca][Modelo](Número de serie)
        result += '<p class="ubicacion">Grupo: ' + r.Grupo + '</p>';
        result += '<p class="ubicacion">Ubicación: ' + r.Ubicacion + '</p>';
        result += '<p class="responsable">Responsable: ' + r.Responsable + '</p>';
        result += '</div>';
        result += '<div class="seccion calib-container ' + GetClaseCalib(r) + '">';
        result += '<span class="item titulo">Calibración</span>';

        result += '<span class="item last" title="Última calibración">' + r.CalibUlt + '</span>';
        result += '<span class="item next" title="Próxima calibración">' + r.CalibProx + ' (' + r.CalibFrec + ')</span>';
        result += '<span class="item refresh hand act-calibracion">actualizar</span>';

        result += '</div>';
        result += '<div class="seccion actions-container">';
        if (r.HasCertif) result += '<span class="item pdf certif-calibracion" url="' + r.PathCertif + '">Certificado de calibración</span>';
        if (r.HasEAC) result += '<span class="item pdf eac" url="' + r.PathEAC + '">EAC</span>';
        if (r.HasManuales) result += '<span class="item pdf manuales">Manuales</span>';
        result += '<span class="item refresh hand act-registro">actualizar ubicación</span>';
        result += '</div>';
        result += '</div>';

        return result;
    }

    function GetClaseCalib(r) {
        var result;

        if (r.CalibVencida) {
            result = 'vencida';
        }
        else {
            if (r.CalibProxAVencer) {
                result = 'alerta';
            }
            else {
                result = 'ok';
            }
        }

        return result;
    }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlacePage" Runat="Server">

<div class="page-title">
    <h1>Instrumentos: listado de vencimientos</h1>
</div>

<div class="full_width" style="width:880px; margin:0 auto;">
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
    <div class="inst-results" id="inst-results"></div>
</div>

</asp:Content>

