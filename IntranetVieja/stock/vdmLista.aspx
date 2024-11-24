<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="vdmLista.aspx.cs" Inherits="stock_vdmLista" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">

<!--[if lt IE 9]>     
<script src="/js/IE9.js" type="text/javascript"></script>
<![endif]-->

<script language="javascript" type="text/javascript">
    var current_page;

    $(document).ready(function () {
        $('#contentPlacePage_cbFiltroSolicito').change();
        $('#contentPlacePage_cbFiltroEstado').change();

        $('input[type="checkbox"]').click(function () {
            ActualizarCamposFiltros();
        });
        $('#contentPlacePage_cbFiltroSolicito').change(function () {
            ActualizarCamposFiltros();
        });
        $('#contentPlacePage_cbFiltroImputacion').change(function () {
            ActualizarCamposFiltros();
        });
        $('#contentPlacePage_cbFiltroEstado').change(function () {
            ActualizarCamposFiltros();
        });
        $('#txtFiltroCodigo').change(function () {
            ActualizarCamposFiltros();
        });

        ActualizarCamposFiltros();
    });

    function ActualizarCamposFiltros() {
        if ($('#chkFiltroSolicito').is(':checked')) {
            $('#contentPlacePage_cbFiltroSolicito').parents('.input_wrapper').removeClass('input_wrapper_selectbox_disabled');
            $('#contentPlacePage_cbFiltroSolicito').removeAttr('disabled');
        }
        else {
            $('#contentPlacePage_cbFiltroSolicito').parents('.input_wrapper').addClass('input_wrapper_selectbox_disabled');
            $('#contentPlacePage_cbFiltroSolicito').attr('disabled', 'disabled');
        }

        if ($('#chkFiltroImputacion').is(':checked')) {
            $('#contentPlacePage_cbFiltroImputacion').parents('.input_wrapper').removeClass('input_wrapper_selectbox_disabled');
            $('#contentPlacePage_cbFiltroImputacion').removeAttr('disabled');
        }
        else {
            $('#contentPlacePage_cbFiltroImputacion').parents('.input_wrapper').addClass('input_wrapper_selectbox_disabled');
            $('#contentPlacePage_cbFiltroImputacion').attr('disabled', 'disabled');
        }

        if ($('#chkFiltroEstado').is(':checked')) {
            $('#contentPlacePage_cbFiltroEstado').parents('.input_wrapper').removeClass('input_wrapper_selectbox_disabled');
            $('#contentPlacePage_cbFiltroEstado').removeAttr('disabled');
        }
        else {
            $('#contentPlacePage_cbFiltroEstado').parents('.input_wrapper').addClass('input_wrapper_selectbox_disabled');
            $('#contentPlacePage_cbFiltroEstado').attr('disabled', 'disabled');
        }

        GetSolicitudes(1);
    }

    function GetSolicitudes(pagina) {
        MostrarLoading();

        var idSolicito = $('#contentPlacePage_cbFiltroSolicito').val();
        var codigo = $('#txtFiltroCodigo').val();
        var idImputacion = $('#contentPlacePage_cbFiltroImputacion').val();
        var estado = $('#contentPlacePage_cbFiltroEstado').val();

        if (!$('#chkFiltroSolicito').is(':checked')) {
            idSolicito = '<%= Constantes.IdPersonaInvalido %>';
        }
        if (!$('#chkFiltroImputacion').is(':checked')) {
            idImputacion = '<%= Constantes.IdImputacionInvalida %>';
        }
        if (!$('#chkFiltroEstado').is(':checked')) {
            estado = '-1';
        }

        ConsultaAjax.url = 'vdmLista.aspx/GetSolicitudes';
        ConsultaAjax.data = '{ "pagina":"' + pagina + '", "idSolicito":"' + idSolicito + '", "codigo":"' + codigo + '", "idImputacion":"' + idImputacion + '", "estado":"' + estado + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            current_page = pagina;

            $('#listado').html();
            var cant = msg.d.length;

            if (cant > 0) {
                var filas = [];
                var i = 0;
                for (i; i < cant; i++) {
                    var fila = '<tr class="fila-color" onclick="VerSolicitud(\'' + msg.d[i][0] + '\')">';
                    fila += '<td class="align-center">' + msg.d[i][1] + '</td>';
                    fila += '<td class="align-center">' + msg.d[i][2] + '</td>';
                    fila += '<td class="align-center">' + msg.d[i][3] + '</td>';
                    fila += '<td class="align-center"></td>';
                    fila += '<td class="align-center"></td>';
                    fila += '<td class="align-center">' + msg.d[i][4] + '</td>';
                    fila += '</tr>';
                    filas.push(fila);
                }

                $('#listado').html(filas.join(''));
            }
            else {
                $('#listado').html('<tr><td colspan="6" class="align-center">No hay solicitudes disponibles.</td></tr>');
            }

            DibujarPaginasLista(idSolicito, codigo, idImputacion, estado);

            CerrarVentana();
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function DibujarPaginasLista(idSolicito, codigo, idImputacion, estado) {
        ConsultaAjax.url = 'vdmLista.aspx/GetCantidadPaginas';
        ConsultaAjax.data = '{ "idSolicito":"' + idSolicito + '", "codigo":"' + codigo + '", "idImputacion":"' + idImputacion + '", "estado":"' + estado + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            $('#tdPaginas').html();

            var cont = [];

            if (current_page == 1) {
                cont.push('<span class="disabled">«Anterior</span>');
            }
            else {
                cont.push('<a href="#" onclick="GetSolicitudes(' + (current_page - 1) + ')">«Anterior</a>');
            }
            cont.push('|');
            cont.push('<a href="#" onclick="GetSolicitudes(1)">Inicio</a>');
            cont.push('|');
            if (msg.d == 0 || current_page == msg.d) {
                cont.push('<span class="disabled">Siguiente»</span>');
            }
            else {
                cont.push('<a href="#" onclick="GetSolicitudes(' + (current_page + 1) + ')">Siguiente»</a>');
            }

            $('#tdPaginas').html(cont.join(' '));
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function VerSolicitud(idVDM) {
        location.href = 'vdmAdmin.aspx?p=' + idVDM;
    }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlacePage" Runat="Server">

<div class="page-title">
    <h1>Listado de vales de materiales</h1>
</div>

<div class="full-width">
    <table class="tbl editable listado" cellspacing="0">
        <thead>
            <tr>
                <td class="border_left" style="width:120px">Número</td>
                <td class="border_middle" style="width:100px">Fecha</td>
                <td class="border_middle" style="width:210px"><input id="chkFiltroSolicito" type="checkbox" /> Solicitó</td>
                <td class="border_middle" style="width:150px">Código</td>
                <td class="border_middle" style="width:100px"><input id="chkFiltroImputacion" type="checkbox" /> Imputación</td>
                <td class="border_right"><input id="chkFiltroEstado" type="checkbox" /> Estado</td>
            </tr>
            <tr class="filter_row">
                <td></td>
                <td></td>
                <td>
                    <div class="input_wrapper input_wrapper_selectbox">
                        <div class="cap">
                            <span id="lblFiltroSolicito" runat="server"></span>
                            <select id="cbFiltroSolicito" runat="server"></select>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="input_wrapper">
                        <div class="cap">
                            <input id="txtFiltroCodigo" maxlength="15" type="text"/>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="input_wrapper input_wrapper_selectbox">
                        <div class="cap">
                            <span id="lblFiltroImputacion" runat="server"></span>
                            <select id="cbFiltroImputacion" runat="server"></select>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="input_wrapper input_wrapper_selectbox">
                        <div class="cap">
                            <span id="lblFiltroEstado" runat="server"></span>
                            <select id="cbFiltroEstado" runat="server"></select>
                        </div>
                    </div>
                </td>
            </tr>
        </thead>
        <tbody id="listado">
            <tr>
                <td colspan="6" class="align-center">No hay solicitudes disponibles.</td>
            </tr>
        </tbody>
        <tfoot>
            <tr>
                <td colspan="6" class="align-center" id="tdPaginas">
                </td>
            </tr>
        </tfoot>
    </table>
</div>

</asp:Content>

