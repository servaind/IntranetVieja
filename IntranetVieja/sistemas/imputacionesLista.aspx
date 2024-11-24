<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="imputacionesLista.aspx.cs" Inherits="sistemas_imputacionesLista" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">

<!--[if lt IE 9]>     
<script src="/js/IE9.js" type="text/javascript"></script>
<![endif]-->

<script language="javascript" type="text/javascript">
    var current_page;
    var current_item;

    $(document).ready(function () {
        $('input[type="checkbox"]').click(function () {
            ActualizarCamposFiltros();
        });
        $('#txtFiltroDescripcion').change(function () {
            ActualizarCamposFiltros();
        });
        $('#txtFiltroNumero').change(function () {
            ActualizarCamposFiltros();
        });
        $('#contentPlacePage_cbFiltroEstado').change(function () {
            ActualizarCamposFiltros();
        });
        $('#btnImputacionAceptar').click(function () {
            var numero = $('#txtImputacionNumero');
            var descripcion = $('#txtImputacionDescripcion');
            var estado = $('#contentPlacePage_cbImputacionEstado');
            var result = true;

            result &= TieneDatos(numero, 'input_wrapper', 'input_wrapper_error') && ContieneNumeros(numero, 'input_wrapper', 'input_wrapper_error');
            result &= TieneDatos(descripcion, 'input_wrapper', 'input_wrapper_error');
            result &= TieneDatos(estado, 'input_wrapper', 'input_wrapper_selectbox_error');

            if (result) {
                MostrarLoading();

                if (current_item == null) {
                    ConsultaAjax.url = 'imputacionesLista.aspx/NuevaImputacion';
                    ConsultaAjax.data = '{ "numero":"' + numero.val() + '", "descripcion": "' + descripcion.val() + '", "estado":"' + estado.val() + '" }';
                }
                else {
                    ConsultaAjax.url = 'imputacionesLista.aspx/EditarImputacion';
                    ConsultaAjax.data = '{ "idImputacion": "' + current_item + '", "numero":"' + numero.val() + '", "descripcion": "' + descripcion.val() + '", "estado":"' + estado.val() + '" }';
                }

                ConsultaAjax.AjaxSuccess = function (msg) {
                    ReiniciarCamposItem();
                    CerrarVentana();

                    GetImputaciones(1);
                };
                ConsultaAjax.AjaxError = function (msg) {
                    ult_ventana = 'divImputacion';
                    ErrorMsg(msg);
                };

                ConsultaAjax.Ejecutar();
            }
        });
        $('#btnImputacionCancelar').click(function () {
            ReiniciarCamposItem();
            CerrarVentana();
            ult_ventana = null;
        });
        $('#btnAgregarImputacion').click(function () {
            $('#titulo_imputacion').text('Alta de imputación');

            ReiniciarCamposItem();

            MostrarVentana('divImputacion');
        });

        ActualizarCamposFiltros();
    });

    function ReiniciarCamposItem() {
        $('#divImputacion .input_wrapper').removeClass('input_wrapper_error').removeClass('input_wrapper_selectbox_error');
        $('#divImputacion input').val('');
        current_item_pos = null;
        current_item = null;
    }

    function ActualizarCamposFiltros() {
        if ($('#chkFiltroEstado').is(':checked')) {
            $('#contentPlacePage_cbFiltroEstado').parents('.input_wrapper').removeClass('input_wrapper_selectbox_disabled');
            $('#contentPlacePage_cbFiltroEstado').removeAttr('disabled');
        }
        else {
            $('#contentPlacePage_cbFiltroEstado').parents('.input_wrapper').addClass('input_wrapper_selectbox_disabled');
            $('#contentPlacePage_cbFiltroEstado').attr('disabled', 'disabled');
        }

        GetImputaciones(1);
    }

    function GetImputaciones(pagina) {
        MostrarLoading();

        var numero = $('#txtFiltroNumero').val();
        var descripcion = $('#txtFiltroDescripcion').val();
        var estado = $('#contentPlacePage_cbFiltroEstado').val();

        if (jQuery.trim(numero).length == 0 || isNaN(numero)) {
            numero = '-1';
        }
        if (!$('#chkFiltroEstado').is(':checked')) {
            estado = '-1';
        }

        ConsultaAjax.url = 'imputacionesLista.aspx/GetImputaciones';
        ConsultaAjax.data = '{ "pagina":"' + pagina + '", "numero":"' + numero + '", "descripcion": "' + descripcion + '", "estado":"' + estado + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            current_page = pagina;

            $('#listado').html();
            var cant = msg.d.length;

            if (cant > 0) {
                var filas = [];
                var i = 0;
                for (i; i < cant; i++) {
                    var fila = '<tr class="fila-color" onclick="EditarImputacion(' + msg.d[i][0] + ')">';
                    fila += '<td class="align-center">' + msg.d[i][1] + '</td>';
                    fila += '<td class="align-center">' + msg.d[i][2] + '</td>';
                    fila += '<td class="align-center"><img src="/images/icons/icon_' + (msg.d[i][3] == 1 ? 'ok.png" title="Activa"' : 'delete.png" title="Inactiva"') + ' /></td>';
                    fila += '</tr>';
                    filas.push(fila);
                }

                $('#listado').html(filas.join(''));
            }
            else {
                $('#listado').html('<tr><td colspan="3" class="align-center">No hay imputaciones disponibles.</td></tr>');
            }

            DibujarPaginasLista(numero, descripcion, estado);

            CerrarVentana();
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function DibujarPaginasLista(numero, descripcion, estado) {
        ConsultaAjax.url = 'imputacionesLista.aspx/GetCantidadPaginas';
        ConsultaAjax.data = '{ "numero":"' + numero + '", "descripcion": "' + descripcion + '", "estado":"' + estado + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            $('#tdPaginas').html();

            var cont = [];

            if (current_page == 1) {
                cont.push('<span class="disabled">«Anterior</span>');
            }
            else {
                cont.push('<a href="#" onclick="GetImputaciones(' + (current_page - 1) + ')">«Anterior</a>');
            }
            cont.push('|');
            cont.push('<a href="#" onclick="GetImputaciones(1)">Inicio</a>');
            cont.push('|');
            if (msg.d == 0 || current_page == msg.d) {
                cont.push('<span class="disabled">Siguiente»</span>');
            }
            else {
                cont.push('<a href="#" onclick="GetImputaciones(' + (current_page + 1) + ')">Siguiente»</a>');
            }

            $('#tdPaginas').html(cont.join(' '));
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function EditarImputacion(idImputacion) {
        MostrarLoading();

        ConsultaAjax.url = 'imputacionesLista.aspx/GetImputacion';
        ConsultaAjax.data = '{ "idImputacion": "' + idImputacion + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            if (msg.d.length > 0) {
                $('#txtImputacionNumero').val(msg.d[0]);
                $('#txtImputacionDescripcion').val(msg.d[1]);
                $('#contentPlacePage_cbImputacionEstado').val(msg.d[2]);

                $('#titulo_imputacion').text('Editar imputación');
                MostrarVentana('divImputacion');
                current_item = idImputacion;
            } else {
                CerrarVentana();
            }
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
    <h1>Administración de imputaciones</h1>
</div>

<div class="form_place">
    <table class="tbl editable listado" cellspacing="0">
        <thead>
         <tr>
                <td colspan="3"><a id="btnAgregarImputacion">Agregar</a></td>
            </tr>

            <tr>
                <td class="border_left" style="width:100px">Nº</td>
                <td class="border_middle" style="width:300px">Descripción</td>
                <td class="border_right"><input id="chkFiltroEstado" type="checkbox" /> Estado</td>
            </tr>
            <tr class="filter_row">
                <td>
                    <div class="input_wrapper">
                        <div class="cap">
                            <input id="txtFiltroNumero" type="text"/>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="input_wrapper">
                        <div class="cap">
                            <input id="txtFiltroDescripcion" type="text"/>
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
                <td colspan="3" class="align-center">No hay imputaciones disponibles.</td>
            </tr>
        </tbody>
        <tfoot>
            <tr>
                <td><a id="btnAgregarImputacion">Agregar</a></td>
                <td colspan="2" class="align-center" id="tdPaginas">
                &nbsp;
                </td>
            </tr>
        </tfoot>
    </table>
</div>

<div class="dialog_wrapper" style="width:500px" id="divImputacion">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3 id="titulo_imputacion"></h3></div></div></div>
    </div>

    <div class="dialog_content">
        <ul class="middle_form">
            <li class="form_floated_item form_floated_item_half clear" style="width:150px">
                <label class="label" for="txtImputacionNumero">Número</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtImputacionNumero" maxlength="6" value="" type="text"/>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_100">
                <label class="label" for="txtImputacionDescripcion">Descripción</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtImputacionDescripcion" maxlength="200" value="" type="text"/>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_half" style="width:150px">
                <label class="label" for="cbImputacionEstado">Estado</label>
                <div class="input_wrapper input_wrapper_selectbox">
                    <div class="cap">
                        <span id="lblImputacionEstado"></span>
                        <select id="cbImputacionEstado" runat="server">
                        </select>
                    </div>
                </div>
            </li>
        </ul>
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li><div class="btn primary_action_button_small button_100" id="btnImputacionAceptar"><div class="cap"><span>Aceptar</span></div></div></li>
            <li><div class="btn secondary_action_button_small button_100" id="btnImputacionCancelar"><div class="cap"><span>Cancelar</span></div></div></li>
        </ul>
    </div>
</div>

</asp:Content>

