<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="ncLista.aspx.cs" Inherits="calidad_ncLista" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">

<!--[if lt IE 9]>     
<script src="/js/IE9.js" type="text/javascript"></script>
<![endif]-->

<script language="javascript" type="text/javascript">
    var current_page;

    $(document).ready(function () {
        $('#contentPlacePage_cbFiltroCategoria').change();
        $('#contentPlacePage_cbFiltroArea').change();
        $('#contentPlacePage_cbFiltroEmitidaPor').change();
        $('#contentPlacePage_cbFiltroEstado').change();

        $('input[type="checkbox"]').click(function () {
            ActualizarCamposFiltros();
        });
        $('#txtFiltroNumero').change(function () {
            ActualizarCamposFiltros();
        });
        $('#txtFiltroAsunto').change(function () {
            ActualizarCamposFiltros();
        });
        $('#contentPlacePage_cbFiltroCategoria').change(function () {
            ActualizarCamposFiltros();
        });
        $('#contentPlacePage_cbFiltroArea').change(function () {
            ActualizarCamposFiltros();
        });
        $('#contentPlacePage_cbFiltroEmitidaPor').change(function () {
            ActualizarCamposFiltros();
        });
        $('#contentPlacePage_cbFiltroEstado').change(function () {
            ActualizarCamposFiltros();
        });

        ActualizarCamposFiltros();
    });

    function ActualizarCamposFiltros() {
        if ($('#chkFiltroCategoria').is(':checked')) {
            $('#contentPlacePage_cbFiltroCategoria').parents('.input_wrapper').removeClass('input_wrapper_selectbox_disabled');
            $('#contentPlacePage_cbFiltroCategoria').removeAttr('disabled');
        }
        else {
            $('#contentPlacePage_cbFiltroCategoria').parents('.input_wrapper').addClass('input_wrapper_selectbox_disabled');
            $('#contentPlacePage_cbFiltroCategoria').attr('disabled', 'disabled');
        }

        if ($('#chkFiltroAsunto').is(':checked')) {
            $('#txtFiltroAsunto').parents('.input_wrapper').removeClass('input_wrapper_disabled');
            $('#txtFiltroAsunto').removeAttr('disabled');
        }
        else {
            $('#txtFiltroAsunto').parents('.input_wrapper').addClass('input_wrapper_disabled');
            $('#txtFiltroAsunto').attr('disabled', 'disabled');
        }

        if ($('#chkFiltroArea').is(':checked')) {
            $('#contentPlacePage_cbFiltroArea').parents('.input_wrapper').removeClass('input_wrapper_selectbox_disabled');
            $('#contentPlacePage_cbFiltroArea').removeAttr('disabled');
        }
        else {
            $('#contentPlacePage_cbFiltroArea').parents('.input_wrapper').addClass('input_wrapper_selectbox_disabled');
            $('#contentPlacePage_cbFiltroArea').attr('disabled', 'disabled');
        }

        if ($('#chkFiltroEmitidaPor').is(':checked')) {
            $('#contentPlacePage_cbFiltroEmitidaPor').parents('.input_wrapper').removeClass('input_wrapper_selectbox_disabled');
            $('#contentPlacePage_cbFiltroEmitidaPor').removeAttr('disabled');
        }
        else {
            $('#contentPlacePage_cbFiltroEmitidaPor').parents('.input_wrapper').addClass('input_wrapper_selectbox_disabled');
            $('#contentPlacePage_cbFiltroEmitidaPor').attr('disabled', 'disabled');
        }

        if ($('#chkFiltroEstado').is(':checked')) {
            $('#contentPlacePage_cbFiltroEstado').parents('.input_wrapper').removeClass('input_wrapper_selectbox_disabled');
            $('#contentPlacePage_cbFiltroEstado').removeAttr('disabled');
        }
        else {
            $('#contentPlacePage_cbFiltroEstado').parents('.input_wrapper').addClass('input_wrapper_selectbox_disabled');
            $('#contentPlacePage_cbFiltroEstado').attr('disabled', 'disabled');
        }

        GetNCs(1);
    }

    function GetNCs(pagina) {
        MostrarLoading();

        var categoria = $('#contentPlacePage_cbFiltroCategoria').val();
        var asunto = $('#txtFiltroAsunto').val();
        var idArea = $('#contentPlacePage_cbFiltroArea').val();
        var idEmitidaPor = $('#contentPlacePage_cbFiltroEmitidaPor').val();
        var estado = $('#contentPlacePage_cbFiltroEstado').val();
        var numero = $('#txtFiltroNumero').val();

        if (!$('#chkFiltroCategoria').is(':checked')) {
            categoria = '-1';
        }
        if (!$('#chkFiltroAsunto').is(':checked')) {
            asunto = '';
        }
        if (!$('#chkFiltroArea').is(':checked')) {
            idArea = '-1';
        }
        if (!$('#chkFiltroEmitidaPor').is(':checked')) {
            idEmitidaPor = '<%= Constantes.IdPersonaInvalido %>';
        }
        if (!$('#chkFiltroEstado').is(':checked')) {
            estado = '-1';
        }
        if (jQuery.trim(numero).length == 0 || isNaN(numero)) {
            numero = '<%= Constantes.ValorInvalido %>';
            $('#txtFiltroNumero').val('');
        }

        ConsultaAjax.url = 'ncLista.aspx/GetNoConformidades';
        ConsultaAjax.data = '{ "pagina":"' + pagina + '", "categoria":"' + categoria + '", "asunto":"' + asunto + '", "idArea":"' + idArea + '", "idEmitidaPor":"' + idEmitidaPor + '", "estado":"' + estado + '", "numero":"' + numero + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            current_page = pagina;

            $('#listado').html();
            var cant = msg.d.length;

            if (cant > 0) {
                var filas = [];
                var i = 0;
                for (i; i < cant; i++) {
                    var fila = '<tr class="fila-color" onclick="VerNoConformidad(\'' + msg.d[i][0] + '\')">';
                    fila += '<td class="align-center">' + msg.d[i][1] + '</td>';
                    fila += GetCeldaCategoria(msg.d[i][2]);
                    fila += '<td class="align-center">' + msg.d[i][3] + '</td>';
                    fila += '<td class="align-center">' + msg.d[i][4] + '</td>';
                    fila += '<td class="align-center">' + msg.d[i][5] + '</td>';
                    fila += '<td class="align-center">' + msg.d[i][6] + '</td>';
                    fila += '<td class="align-center">' + msg.d[i][7] + '</td>';
                    fila += '<td class="align-center">' + msg.d[i][8] + '</td>';
                    fila += '<td class="align-center">' + msg.d[i][9] + '</td>';
                    fila += '</tr>';
                    filas.push(fila);
                }

                $('#listado').html(filas.join(''));
            }
            else {
                $('#listado').html('<tr><td colspan="10" class="align-center">No hay solicitudes disponibles.</td></tr>');
            }

            DibujarPaginasLista(categoria, asunto, idArea, idEmitidaPor, estado, numero);

            CerrarVentana();
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function DibujarPaginasLista(categoria, asunto, idArea, idEmitidaPor, estado, numero) {
        ConsultaAjax.url = 'ncLista.aspx/GetCantidadPaginas';
        ConsultaAjax.data = '{ "categoria":"' + categoria + '", "asunto":"' + asunto + '", "idArea":"' + idArea + '", "idEmitidaPor":"' + idEmitidaPor + '", "estado":"' + estado + '", "numero":"' + numero + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            $('#tdPaginas').html();

            var cont = [];

            if (current_page == 1) {
                cont.push('<span class="disabled">«Anterior</span>');
            }
            else {
                cont.push('<a href="#" onclick="GetNCs(' + (current_page - 1) + ')">«Anterior</a>');
            }
            cont.push('|');
            cont.push('<a href="#" onclick="GetNCs(1)">Inicio</a>');
            cont.push('|');
            if (msg.d == 0 || current_page == msg.d) {
                cont.push('<span class="disabled">Siguiente»</span>');
            }
            else {
                cont.push('<a href="#" onclick="GetNCs(' + (current_page + 1) + ')">Siguiente»</a>');
            }

            $('#tdPaginas').html(cont.join(' '));
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

    function VerNoConformidad(idNC) {
        location.href = 'ncAdmin.aspx?p=' + idNC;
    }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlacePage" Runat="Server">

<div class="page-title">
    <h1>Listado de NCs / Observaciones / Oportunidades de mejora</h1>
</div>

<div class="full-width scroll-horizontal">
    <table class="tbl editable listado" style="width:1600px" cellspacing="0">
        <thead>
            <tr>
                <td class="border_left" style="width:120px">Nº</td>
                <td class="border_middle" style="width:120px"><input id="chkFiltroCategoria" type="checkbox" /> Categoría</td>
                <td class="border_middle" style="width:250px"><input id="chkFiltroAsunto" type="checkbox" /> Asunto</td>
                <td class="border_middle" style="width:200px"><input id="chkFiltroArea" type="checkbox" /> Área de responsabilidad</td>
                <td class="border_middle" style="width:250px"><input id="chkFiltroEmitidaPor" type="checkbox" /> Emitida por</td>
                <td class="border_middle" style="width:120px">Generada</td>
                <td class="border_middle" style="width:120px">Cierre</td>
                <td class="border_middle"><input id="chkFiltroEstado" type="checkbox" /> Estado</td>
                <td class="border_middle" style="width:200px">Responsable</td>
            </tr>
            <tr class="filter_row">
                <td>
                    <div class="input_wrapper">
                        <div class="cap">
                            <input class="align-right" id="txtFiltroNumero" maxlength="6" type="text"/>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="input_wrapper input_wrapper_selectbox">
                        <div class="cap">
                            <span id="lblFiltroCategoria" runat="server"></span>
                            <select id="cbFiltroCategoria" runat="server"></select>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="input_wrapper">
                        <div class="cap">
                            <input id="txtFiltroAsunto" maxlength="90" type="text"/>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="input_wrapper input_wrapper_selectbox">
                        <div class="cap">
                            <span id="lblFiltroArea" runat="server"></span>
                            <select id="cbFiltroArea" runat="server"></select>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="input_wrapper input_wrapper_selectbox">
                        <div class="cap">
                            <span id="lblFiltroEmitidaPor" runat="server"></span>
                            <select id="cbFiltroEmitidaPor" runat="server"></select>
                        </div>
                    </div>
                </td>
                <td></td>
                <td></td>
                <td>
                    <div class="input_wrapper input_wrapper_selectbox">
                        <div class="cap">
                            <span id="lblFiltroEstado" runat="server"></span>
                            <select id="cbFiltroEstado" runat="server"></select>
                        </div>
                    </div>
                </td>
                <td></td>
                <td></td>
            </tr>
        </thead>
        <tbody id="listado">
            <tr>
                <td colspan="10" class="align-center">No hay solicitudes disponibles.</td>
            </tr>
        </tbody>
        <tfoot>
            <tr>
                <td colspan="10" class="align-center" id="tdPaginas">
                </td>
            </tr>
        </tfoot>
    </table>
</div>
<div id="form-version">
    <div class="left">FORM FG-006 REV 02</div>
    <div class="right">26/09/2012</div>
</div>

</asp:Content>

