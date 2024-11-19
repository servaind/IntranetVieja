<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="informacionObraLista.aspx.cs" Inherits="general_informacionObraLista" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">

<!--[if lt IE 9]>     
<script src="/js/IE9.js" type="text/javascript"></script>
<![endif]-->

<script language="javascript" type="text/javascript">
    var current_page;

    $(document).ready(function () {
        $('#contentPlacePage_cbFiltroResponsableObra').change();
        $('#contentPlacePage_cbFiltroInformante').change();

        $('input[type="checkbox"]').click(function () {
            ActualizarCamposFiltros();
        });
        $('#txtFiltroNumero').change(function () {
            ActualizarCamposFiltros();
        });
        $('#txtFiltroCliente').change(function () {
            ActualizarCamposFiltros();
        });
        $('#txtFiltroImputacion').change(function () {
            ActualizarCamposFiltros();
        });
        $('#txtFiltroOC').change(function () {
            ActualizarCamposFiltros();
        });
        $('#contentPlacePage_cbFiltroResponsableObra').change(function () {
            ActualizarCamposFiltros();
        });
        $('#contentPlacePage_cbFiltroInformante').change(function () {
            ActualizarCamposFiltros();
        });

        ActualizarCamposFiltros();
    });

    function ActualizarCamposFiltros() {
        if ($('#chkFiltroResponsableObra').is(':checked')) {
            $('#contentPlacePage_cbFiltroResponsableObra').parents('.input_wrapper').removeClass('input_wrapper_selectbox_disabled');
            $('#contentPlacePage_cbFiltroResponsableObra').removeAttr('disabled');
        }
        else {
            $('#contentPlacePage_cbFiltroResponsableObra').parents('.input_wrapper').addClass('input_wrapper_selectbox_disabled');
            $('#contentPlacePage_cbFiltroResponsableObra').attr('disabled', 'disabled');
        }

        if ($('#chkFiltroInformante').is(':checked')) {
            $('#contentPlacePage_cbFiltroInformante').parents('.input_wrapper').removeClass('input_wrapper_selectbox_disabled');
            $('#contentPlacePage_cbFiltroInformante').removeAttr('disabled');
        }
        else {
            $('#contentPlacePage_cbFiltroInformante').parents('.input_wrapper').addClass('input_wrapper_selectbox_disabled');
            $('#contentPlacePage_cbFiltroInformante').attr('disabled', 'disabled');
        }

        GetIOs(1);
    }

    function GetIOs(pagina) {
        MostrarLoading();

        var numero = $('#txtFiltroNumero').val();
        var cliente = $('#txtFiltroCliente').val();
        var imputacion = $('#txtFiltroImputacion').val();
        var responsableObra = $('#contentPlacePage_cbFiltroResponsableObra').val();
        var informante = $('#contentPlacePage_cbFiltroInformante').val();
        var ordenCompra = $('#txtFiltroOC').val();

        if (numero.length == 0 || isNaN(numero)) {
            numero = '<%= Constantes.ValorInvalido %>';
        }
        if (!$('#chkFiltroResponsableObra').is(':checked')) {
            responsableObra = '<%= Constantes.IdPersonaInvalido %>';
        }
        if (!$('#chkFiltroInformante').is(':checked')) {
            informante = '<%= Constantes.IdPersonaInvalido %>';
        }

        ConsultaAjax.url = 'informacionObraLista.aspx/GetIOs';
        ConsultaAjax.data = '{ "pagina":"' + pagina + '", "numero":"' + numero + '", "cliente":"' + cliente + '", "idResponsableObra": "' + responsableObra + '", "idInformante": "' + informante + '", "ordenCompra":"' + ordenCompra + '", "imputacion":"' + imputacion + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            current_page = pagina;

            $('#listado').html();
            var cant = msg.d.length;

            if (cant > 0) {
                var filas = [];
                var i = 0;
                for (i; i < cant; i++) {
                    var fila = '<tr class="fila-color" onclick="VerIO(\'' + msg.d[i][0] + '\')">';
                    fila += '<td class="align-center">' + msg.d[i][1] + '</td>';
                    fila += '<td class="align-center">' + msg.d[i][2] + '</td>';
                    fila += '<td class="align-center">' + msg.d[i][6] + '</td>';
                    fila += '<td class="align-right">' + msg.d[i][5] + '</td>';
                    fila += '<td class="align-center">' + msg.d[i][3] + '</td>';
                    fila += '<td class="align-center">' + msg.d[i][4] + '</td>';
                    fila += '</tr>';
                    filas.push(fila);
                }

                $('#listado').html(filas.join(''));
            }
            else {
                $('#listado').html('<tr><td colspan="6" class="align-center">No hay Información Interna de Obra disponibles.</td></tr>');
            }

            DibujarPaginasLista(numero, cliente, responsableObra, informante, ordenCompra, imputacion);

            CerrarVentana();
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function DibujarPaginasLista(numero, cliente, responsableObra, informante, ordenCompra, imputacion) {
        ConsultaAjax.url = 'informacionObraLista.aspx/GetCantidadPaginas';
        ConsultaAjax.data = '{ "numero":"' + numero + '", "cliente":"' + cliente + '", "idResponsableObra": "' + responsableObra + '", "idInformante": "' + informante + '", "ordenCompra":"' + ordenCompra + '", "imputacion":"' + imputacion + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            $('#tdPaginas').html();

            var cont = [];

            if (current_page == 1) {
                cont.push('<span class="disabled">«Anterior</span>');
            }
            else {
                cont.push('<a href="#" onclick="GetIOs(' + (current_page - 1) + ')">«Anterior</a>');
            }
            cont.push('|');
            cont.push('<a href="#" onclick="GetIOs(1)">Inicio</a>');
            cont.push('|');
            if (msg.d == 0 || current_page == msg.d) {
                cont.push('<span class="disabled">Siguiente»</span>');
            }
            else {
                cont.push('<a href="#" onclick="GetIOs(' + (current_page + 1) + ')">Siguiente»</a>');
            }

            $('#tdPaginas').html(cont.join(' '));
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function VerIO(idIO) {
        location.href = 'informacionObraAdmin.aspx?p=' + idIO;
    }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlacePage" Runat="Server">

<div class="page-title">
    <h1>Listado de Informacion Interna de Obras / Proyectos</h1>
</div>

<div class="full-width">
    <table class="tbl editable listado" cellspacing="0">
        <thead>
            <tr>
                <td class="border_left" style="width:80px">Nº</td>
                <td class="border_middle">Cliente</td>
                <td class="border_middle" style="width:100px">Imputacion</td>
                <td class="border_middle" style="width:80px">OC</td>
                <td class="border_middle" style="width:200px"><input id="chkFiltroResponsableObra" type="checkbox" /> Responsable de obra</td>
                <td class="border_right" style="width:200px"><input id="chkFiltroInformante" type="checkbox" /> Informante</td>
            </tr>
            <tr class="filter_row">
                <td>
                    <div class="input_wrapper">
                        <div class="cap">
                            <input id="txtFiltroNumero" maxlength="4" type="text"/>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="input_wrapper">
                        <div class="cap">
                            <input id="txtFiltroCliente" maxlength="44" type="text"/>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="input_wrapper">
                        <div class="cap">
                            <input id="txtFiltroImputacion" maxlength="10" type="text"/>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="input_wrapper">
                        <div class="cap">
                            <input id="txtFiltroOC" maxlength="8" type="text"/>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="input_wrapper input_wrapper_selectbox">
                        <div class="cap">
                            <span id="lblFiltroResponsableObra" runat="server"></span>
                            <select id="cbFiltroResponsableObra" runat="server"></select>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="input_wrapper input_wrapper_selectbox">
                        <div class="cap">
                            <span id="lblFiltroInformante" runat="server"></span>
                            <select id="cbFiltroInformante" runat="server"></select>
                        </div>
                    </div>
                </td>
            </tr>
        </thead>
        <tbody id="listado">
            <tr>
                <td colspan="6" class="align-center">No hay Información Interna de Obra disponibles.</td>
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