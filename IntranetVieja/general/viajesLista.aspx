<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="viajesLista.aspx.cs" Inherits="general_viajesLista" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">

<!--[if lt IE 9]>     
<script src="/js/IE9.js" type="text/javascript"></script>
<![endif]-->

<script language="javascript" type="text/javascript">
    var current_page;

    $(document).ready(function () {
        ActualizarCamposFiltros();
    });

    function ActualizarCamposFiltros() {
        if ($('#chkFiltroImputacion').is(':checked')) {
            $('#contentPlacePage_cbFiltroImputacion').parents('.input_wrapper').removeClass('input_wrapper_selectbox_disabled');
            $('#contentPlacePage_cbFiltroImputacion').removeAttr('disabled');
        }
        else {
            $('#contentPlacePage_cbFiltroImputacion').parents('.input_wrapper').addClass('input_wrapper_selectbox_disabled');
            $('#contentPlacePage_cbFiltroImputacion').attr('disabled', 'disabled');
        }

        if ($('#chkFiltroVehiculo').is(':checked')) {
            $('#contentPlacePage_cbFiltroVehiculo').parents('.input_wrapper').removeClass('input_wrapper_selectbox_disabled');
            $('#contentPlacePage_cbFiltroVehiculo').removeAttr('disabled');
        }
        else {
            $('#contentPlacePage_cbFiltroVehiculo').parents('.input_wrapper').addClass('input_wrapper_selectbox_disabled');
            $('#contentPlacePage_cbFiltroVehiculo').attr('disabled', 'disabled');
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

    function DibujarAcciones() {
        $('.input_actions').html('<ul><li class="confirm" title="Confirmar solicitud"></li></ul>');
    }

    var EnlazarEventos = function () {
        $('input[type="checkbox"]').click(function () {
            ActualizarCamposFiltros();
        });
        $('#contentPlacePage_cbFiltroImputacion').change(function () {
            ActualizarCamposFiltros();
        });
        $('#contentPlacePage_cbFiltroVehiculo').change(function () {
            ActualizarCamposFiltros();
        });
        $('#contentPlacePage_cbFiltroEstado').change(function () {
            ActualizarCamposFiltros();
        });
        $('#txtFiltroDestinatario').change(function () {
            ActualizarCamposFiltros();
        });

        <% if (PuedeAdministrador) { %>
        $('.input_actions ul li.confirm').click(function () {
            var idSV = $(this).parents('.input_actions').attr('name');
            var s = $(this).parents('.input_actions').attr('value');

            idSV = idSV.replace('act_', '');

            MostrarLoading();

            ConsultaAjax.url = 'viajesLista.aspx/ConfirmarSolicitud';
            ConsultaAjax.data = '{ "s": "' + s + '" }';
            ConsultaAjax.AjaxSuccess = function (msg) {
                $('#lblEstado_' + idSV).text(msg.d[i][0]);
                $('#lblEstado_' + idSV).parent().attr('class', 'tag ' + msg.d[i][1]);

                CerrarVentana();
            };
            ConsultaAjax.AjaxError = function (msg) {
                ErrorMsg(msg);
            };

            ConsultaAjax.Ejecutar();
        });
        <% } %>
    }

    function GetSolicitudes(pagina) {
        MostrarLoading();

        var destinatario = $('#txtFiltroDestinatario').val();
        var idVehiculo = $('#contentPlacePage_cbFiltroVehiculo').val();
        var idImputacion = $('#contentPlacePage_cbFiltroImputacion').val();
        var estado = $('#contentPlacePage_cbFiltroEstado').val();

        if (!$('#chkFiltroImputacion').is(':checked')) {
            idImputacion = '<%= Constantes.IdImputacionInvalida %>';
        }
        if (!$('#chkFiltroVehiculo').is(':checked')) {
            idVehiculo = '-1';
        }
        if (!$('#chkFiltroEstado').is(':checked')) {
            estado = '-1';
        }

        ConsultaAjax.url = 'viajesLista.aspx/GetSolicitudes';
        ConsultaAjax.data = '{ "pagina":"' + pagina + '", "destinatario":"' + destinatario + '", "idImputacion":"' + idImputacion + '", "idVehiculo":"' + idVehiculo + '", "estado":"' + estado + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            current_page = pagina;

            $('#listado').html();
            var cant = msg.d.length;

            if (cant > 0) {
                var filas = [];
                var i = 0;
                for (i; i < cant; i++) {
                    var fila = '<tr class="fila-color">';
                    fila += '<td onclick="VerSolicitud(\'' + msg.d[i][6] + '\')" class="align-center">' + msg.d[i][0] + '</td>';
                    fila += '<td onclick="VerSolicitud(\'' + msg.d[i][6] + '\')" class="align-center">' + msg.d[i][1] + '</td>';
                    fila += '<td onclick="VerSolicitud(\'' + msg.d[i][6] + '\')" class="align-left">' + msg.d[i][2] + '</td>';
                    fila += '<td onclick="VerSolicitud(\'' + msg.d[i][6] + '\')" class="align-center">' + msg.d[i][3] + '</td>';
                    fila += '<td onclick="VerSolicitud(\'' + msg.d[i][6] + '\')" class="align-center">' + msg.d[i][4] + '</td>';
                    fila += '<td class="align-center tag-container">';
                    <% if (PuedeAdministrador) { %>
                    if (msg.d[i][5] == 'Aprobada') {
                        fila += '<div class="hasActions"><div class="tag ' + msg.d[i][8] + '" style="width:90px"><span id="lblEstado_' + msg.d[i][0] + '">' + msg.d[i][5] + '</span></div><div class="input_actions" value="' + msg.d[i][7] + '" name="act_' + msg.d[i][0] + '"></div></div>';
                    }
                    else {
                        fila += '<div class="tag ' + msg.d[i][8] + '" style="width:90px">' + msg.d[i][5] + '</div>';
                    }
                    <% }
                       else { %>
                    fila += '<div class="tag ' + msg.d[i][8] + '" style="width:90px">' + msg.d[i][5] + '</div>';
                    <% } %>
                    fila += '</td></tr>';
                    filas.push(fila);
                }

                $('#listado').html(filas.join(''));
            }
            else {
                $('#listado').html('<tr><td colspan="6" class="align-center">No hay solicitudes disponibles.</td></tr>');
            }

            DibujarAcciones();
            EnlazarEventosMaster();
            EnlazarEventos();

            DibujarPaginasLista(destinatario, idImputacion, idVehiculo, estado);

            CerrarVentana();
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function DibujarPaginasLista(destinatario, idImputacion, idVehiculo, estado) {
        ConsultaAjax.url = 'viajesLista.aspx/GetCantidadPaginas';
        ConsultaAjax.data = '{ "destinatario":"' + destinatario + '", "idImputacion":"' + idImputacion + '", "idVehiculo":"' + idVehiculo + '", "estado":"' + estado + '" }';
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

    function VerSolicitud(url) {
        location.href = url;
    }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlacePage" Runat="Server">

<div class="page-title">
    <h1>Listado de solicitudes de viaje</h1>
</div>

<div class="full-width">
    <table class="tbl editable listado" cellspacing="0">
        <thead>
            <tr>
                <td class="border_left" style="width:90px">Número</td>
                <td class="border_middle" style="width:120px">Fecha</td>
                <td class="border_middle" style="width:300px">Destinatario</td>
                <td class="border_middle" style="width:100px"><input id="chkFiltroImputacion" type="checkbox" /> Imputación</td>
                <td class="border_middle" style="width:120px"><input id="chkFiltroVehiculo" type="checkbox" /> Vehículo</td>
                <td class="border_right"><input id="chkFiltroEstado" type="checkbox" /> Estado</td>
            </tr>
            <tr class="filter_row">
                <td></td>
                <td></td>
                <td>
                    <div class="input_wrapper">
                        <div class="cap">
                            <input id="txtFiltroDestinatario" maxlength="100" type="text"/>
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
                            <span id="lblFiltroVehiculo" runat="server"></span>
                            <select id="cbFiltroVehiculo" runat="server"></select>
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

