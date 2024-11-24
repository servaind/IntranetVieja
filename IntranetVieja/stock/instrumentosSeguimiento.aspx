<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="instrumentosSeguimiento.aspx.cs" Inherits="stock_instrumentosSeguimiento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">

<!--[if lt IE 9]>     
<script src="/js/IE9.js" type="text/javascript"></script>
<![endif]-->

<script language="javascript" type="text/javascript">
    var current_page;
    var current_item;

    $(document).ready(function () {
        $('#contentPlacePage_cbFiltroTipo').change();

        $('#contentPlacePage_cbFiltroTipo').change(function () {
            ActualizarCamposFiltros();
        });
        $('input[type="checkbox"]').click(function () {
            ActualizarCamposFiltros();
        });
        $('#txtFiltroNumero').change(function () {
            ActualizarCamposFiltros();
        });
        $('#txtFiltroDescripcion').change(function () {
            ActualizarCamposFiltros();
        });
        $('#txtFiltroMarca').change(function () {
            ActualizarCamposFiltros();
        });
        $('#btnHerramientaImagenAceptar').click(function () {
            CerrarVentana();
        });
        $('#btnAgregarCalibracion').click(function () {
            LimpiarCalibracion();

            MostrarVentana('divCalibracion');
        });
        $('#btnCalibracionCancelar').click(function() {
            LimpiarCalibracion();

            CerrarVentana();
        });
        $('#btnCalibracionAceptar').click(function() {
            var equipo = $('#contentPlacePage_cbEquipo');
            var frecuencia = $('#contentPlacePage_cbFrecuencia');
            var ultCalib = $('#txtUltCalibracion');
            var proxCalib = $('#txtProxCalibracion');
            var tipoCalib = $('#contentPlacePage_cbTipoCalibracion');
            var observaciones = $('#txtObservaciones');
            var result = true;

            result &= TieneDatos(equipo, 'input_wrapper', 'input_wrapper_selectbox_error');
            result &= TieneDatos(frecuencia, 'input_wrapper', 'input_wrapper_selectbox_error');
            result &= ContieneFecha(ultCalib, 'input_wrapper', 'input_wrapper_error');
            result &= ContieneFecha(proxCalib, 'input_wrapper', 'input_wrapper_error');
            result &= TieneDatos(tipoCalib, 'input_wrapper', 'input_wrapper_selectbox_error');
            result &= TieneDatos(observaciones, 'textarea_wrapper', 'textarea_wrapper_error');

            if (result) {
                MostrarLoading();

                if (current_item == null) {
                    ConsultaAjax.url = 'instrumentosSeguimiento.aspx/NuevaCalibracion';
                    ConsultaAjax.data = JSON.stringify({ equipo: equipo.val(), frecuencia: frecuencia.val(), ultCalibracion: ultCalib.val(), proxCalibracion: proxCalib.val(), tipoCalibracion: tipoCalib.val(), observaciones: observaciones.val() });
                }
                else {
                    ConsultaAjax.url = 'instrumentosSeguimiento.aspx/ActualizarCalibracion';
                    ConsultaAjax.data = JSON.stringify({ equipo: current_item, frecuencia: frecuencia.val(), ultCalibracion: ultCalib.val(), proxCalibracion: proxCalib.val(), tipoCalibracion: tipoCalib.val(), observaciones: observaciones.val() });
                }
                ConsultaAjax.AjaxSuccess = function (msg) {
                    GetListaSeguimiento(1);
                };
                ConsultaAjax.AjaxError = function (msg) {
                    ult_ventana = 'divCalibracion';
                    ErrorMsg(msg);
                };

                ConsultaAjax.Ejecutar();
            }
        });

        ActualizarCamposFiltros();
    });

    function LimpiarCalibracion() {
        LimpiarCampo($('#contentPlacePage_cbEquipo'), 'input_wrapper_selectbox');
        $('#contentPlacePage_cbEquipo').removeAttr('disabled');
        LimpiarCampo($('#contentPlacePage_cbFrecuencia'), 'input_wrapper_selectbox');
        LimpiarCampo($('#txtUltCalibracion'), 'input_wrapper');
        LimpiarCampo($('#txtProxCalibracion'), 'input_wrapper');
        LimpiarCampo($('#contentPlacePage_cbTipoCalibracion'), 'input_wrapper_selectbox');
        LimpiarCampo($('#txtObservaciones'), 'textarea_wrapper');
        $('#detalles-calib').hide();

        current_item = null;
    }

    function ActualizarCamposFiltros() {
        if ($('#chkFiltroTipo').is(':checked')) {
            $('#contentPlacePage_cbFiltroTipo').parents('.input_wrapper').removeClass('input_wrapper_selectbox_disabled');
            $('#contentPlacePage_cbFiltroTipo').removeAttr('disabled');
        }
        else {
            $('#contentPlacePage_cbFiltroTipo').parents('.input_wrapper').addClass('input_wrapper_selectbox_disabled');
            $('#contentPlacePage_cbFiltroTipo').attr('disabled', 'disabled');
        }

        GetListaSeguimiento(1);
    }

    function GetListaSeguimiento(pagina) {
        MostrarLoading();

        var numero = $('#txtFiltroNumero').val();
        var tipo = $('#contentPlacePage_cbFiltroTipo').val();
        var descripcion = $('#txtFiltroDescripcion').val();
        var marca = $('#txtFiltroMarca').val();

        if (numero.length == 0 || isNaN(numero)) {
            numero = <%= Constantes.ValorInvalido.ToString() %>;
        }
        if (!$('#chkFiltroTipo').is(':checked')) {
            tipo = <%= Constantes.ValorInvalido.ToString() %>;
        }

        ConsultaAjax.url = 'instrumentosSeguimiento.aspx/GetListaSeguimiento';
        ConsultaAjax.data = JSON.stringify({ pagina: pagina, numeroI: numero, tipo: tipo, descripcion: descripcion, marca: marca });
        ConsultaAjax.AjaxSuccess = function (msg) {
            current_page = pagina;

            $('#listado').html();
            var cant = msg.d.length;

            if (cant > 0) {
                var filas = [];
                var i = 0;
                for (i; i < cant; i++) {
                    var fila = '<tr class="fila-color">';
                    fila += '<td class="align-center"><a href="javascript:void(0);" onclick="VerImagen(\'' + msg.d[i][11] + '\');"><img src="/images/icons/icon_camera.png" alt="Ver foto" title="Ver foto" /></a></td>';
                    fila += '<td class="align-center">' + msg.d[i][1] + '</td>';
                    fila += '<td class="align-left">' + msg.d[i][2] + '</td>';
                    fila += '<td class="align-left">' + msg.d[i][3] + '</td>';
                    fila += '<td class="align-center">' + msg.d[i][4] + '</td>';
                    fila += '<td class="align-center">' + msg.d[i][5] + '</td>';
                    fila += '<td class="align-center">' + msg.d[i][6] + '</td>';
                    fila += '<td class="align-center">' + DateTimeToString(msg.d[i][7]) + '</td>';
                    fila += '<td class="align-center">' + DateTimeToString(msg.d[i][8]) + '</td>';
                    fila += '<td class="align-left">' + msg.d[i][9] + '</td>';
                    fila += '<td class="align-center">' + msg.d[i][10] + '</td>';

                    fila += '<td class="align-center">';
                    fila += '<a href="javascript:void(0);" onclick="VerHerramienta(\'' + msg.d[i][0] + '\');"><img src="/images/icons/icon_ver.png" alt="Detalles de la herramienta" title="Detalles de la herramienta" /></a> ';
                    fila += '<a href="javascript:void(0);" onclick="VerHistorial(' + msg.d[i][0] + ');"><img src="/images/icons/icon_calendario.png" alt="Ver historial" title="Ver historial" /></a> ';
                    if(msg.d[i][12].length > 0) {
                        fila += '<a href="javascript:void(0);" onclick="DescargarManual(\'' + msg.d[i][12] + '\');"><img src="/images/icons/icon_pdf.png" alt="Descargar manual del instrumento" title="Descargar manual del instrumento" /></a>';
                    }
                    fila += '<a href="javascript:void(0);" onclick="EliminarHerramienta(' + msg.d[i][1] + ');"><img src="/images/icons/icon_delete.png" alt="Eliminar herramienta" title="Eliminar herramienta" /></a>';
                    fila += '</td></tr>';
                    filas.push(fila);
                }

                $('#listado').html(filas.join(''));
            }
            else {
                $('#listado').html('<tr><td colspan="10" class="align-center">No hay equipos disponibles.</td></tr>');
            }

            DibujarPaginasLista(numero, tipo, descripcion, marca);

            CerrarVentana();
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function DibujarPaginasLista(numero, tipo, descripcion, marca) {
        ConsultaAjax.url = 'instrumentosSeguimiento.aspx/GetCantidadPaginas';
        ConsultaAjax.data = JSON.stringify({ numeroI: numero, tipo: tipo, descripcion: descripcion, marca: marca });
        ConsultaAjax.AjaxSuccess = function (msg) {
            $('#tdPaginas').html();

            var cont = [];

            if (current_page == 1) {
                cont.push('<span class="disabled">«Anterior</span>');
            }
            else {
                cont.push('<a href="#" onclick="GetListaSeguimiento(' + (current_page - 1) + ')">«Anterior</a>');
            }
            cont.push('|');
            if (msg.d == 0 || current_page == msg.d) {
                cont.push('<span class="disabled">Siguiente»</span>');
            }
            else {
                cont.push('<a href="#" onclick="GetListaSeguimiento(' + (current_page + 1) + ')">Siguiente»</a>');
            }

            $('#tdPaginas').html(cont.join(' '));
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function VerCalibracion(idH) {
        MostrarLoading();

        ConsultaAjax.url = 'instrumentosSeguimiento.aspx/GetCalibracion';
        ConsultaAjax.data = '{ "id": "' + idH + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            if (msg.d.length > 0) {
                $('#contentPlacePage_cbEquipo').val(msg.d[0]);
                $('#contentPlacePage_cbEquipo').attr('disabled', 'disabled');
                $('#contentPlacePage_cbFrecuencia').val(msg.d[1]);
                $('#txtUltCalibracion').val(msg.d[2]);
                $('#txtProxCalibracion').val(msg.d[3]);
                $('#contentPlacePage_cbTipoCalibracion').val(msg.d[4]);
                $('#txtObservaciones').val(msg.d[5]);

                $('#detalles-calib').show();

                MostrarVentana('divCalibracion');

                current_item = idH;
            }
            else {
                CerrarVentana();
            }
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function VerHerramienta() {
        location.href = 'herramientaAdmin.aspx?p=' + current_item;
    }

    function VerImagen(url) {
        $('#img_titulo').html('Detalle de instrumento');

        $('#imgHerramienta').attr('src', '/getImage.aspx?p=' + url);

        MostrarVentana('divHerramientaImagen');
    }
    function DescargarManual(url) {
        location.href = '/download.aspx?p=' + url;
    }
    function VerHistorial(idHerramienta) {
        current_herramienta = idHerramienta;

        GetRegHistLista(1);
    }

    function DescargarCalibracion() {
        MostrarLoading();

        ConsultaAjax.url = 'instrumentosSeguimiento.aspx/GetDescargarCalibracion';
        ConsultaAjax.data = '{ "equipo": "' + current_item + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            location.href = msg.d;

            MostrarVentana('divCalibracion');
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
    <h1>Control de equipos de seguimiento y medición</h1>
</div>

<div class="full-width scroll-horizontal">
    <table class="tbl editable listado" style="width:2000px" cellspacing="0">
        <thead>
            <tr>
                <td class="border_left" style="width:50px">&nbsp;</td>
                <td class="border_middle" style="width:70px">Nº</td>
                <td class="border_middle" style="width:190px"><input id="chkFiltroTipo" type="checkbox" /> Tipo</td>
                <td class="border_middle" style="width:260px">Descripcion</td>
                <td class="border_middle" style="width:190px">Marca</td>
                <td class="border_middle" style="width:220px"><input id="chkFiltroPersonaCargo" type="checkbox" /> Persona a cargo</td>
                <td class="border_middle" style="width:150px">Nº serie</td>
                <td class="border_middle" style="width:120px">Frec calibración</td>
                <td class="border_middle" style="width:120px">Últ calibración</td>
                <td class="border_middle" style="width:120px">Prox calibración</td>
                <td class="border_middle" style="width:200px">Ubicación</td>
                <td class="border_middle" style="width:120px">Tipo calibración</td>
                <td class="border_right">Acciones</td>
            </tr>
            <tr class="filter_row">
                <td></td>
                <td>
                    <div class="input_wrapper">
                        <div class="cap">
                            <input id="txtFiltroNumero" type="text"/>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="input_wrapper input_wrapper_selectbox">
                        <div class="cap">
                            <span id="lblFiltroTipo" runat="server"></span>
                            <select id="cbFiltroTipo" runat="server"></select>
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
                    <div class="input_wrapper">
                        <div class="cap">
                            <input id="txtFiltroMarca" type="text"/>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="input_wrapper input_wrapper_selectbox">
                        <div class="cap">
                            <span id="lblFiltroPersonaCargo" runat="server"></span>
                            <select id="cbFiltroPersonaCargo" runat="server"></select>
                        </div>
                    </div>
                </td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
        </thead>
        <tbody id="listado">
            <tr>
                <td colspan="13" class="align-center">No hay equipos disponibles.</td>
            </tr>
        </tbody>
        <tfoot>
            <tr>
                <td colspan="3">
                    <a href="#" id="btnAgregarCalibracion">Agregar calibración</a>
                </td>
                <td colspan="10" class="align-center" id="tdPaginas">
                </td>
            </tr>
        </tfoot>
    </table>
</div>

<div class="dialog_wrapper" style="width:500px" id="divCalibracion">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3>Detalle de la calibración</h3></div></div></div>
    </div>

    <div class="dialog_content">
        <ul class="middle_form">
            <li class="form_floated_item form_floated_item_50">
                <label class="label" for="cbFrecuencia">Equipo</label>
                <div class="input_wrapper input_wrapper_selectbox">
                    <div class="cap">
                        <span id="lblEquipo"></span>
                        <select id="cbEquipo" runat="server"></select>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_50 form_floated_item_right">
                <label class="label" for="cbFrecuencia">Frecuencia de calibración</label>
                <div class="input_wrapper input_wrapper_selectbox">
                    <div class="cap">
                        <span id="lblFrecuencia"></span>
                        <select id="cbFrecuencia" runat="server"></select>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_50">
                <label class="label" for="txtFechaHasta">Fecha de última calibración</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtUltCalibracion" value="" maxlength="10" type="text" />
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_50 form_floated_item_right">
                <label class="label" for="txtFechaHasta">Fecha de próxima calibración</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtProxCalibracion" value="" maxlength="10" type="text" />
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_50">
                <label class="label" for="cbFrecuencia">Tipo de calibración</label>
                <div class="input_wrapper input_wrapper_selectbox">
                    <div class="cap">
                        <span id="lblTipoCalibracion"></span>
                        <select id="cbTipoCalibracion" runat="server"></select>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_100">
                <label class="label" for="txtNovedadesH">Observaciones</label>
                <div class="textarea_wrapper"> 
	                <div class="top">
		                <div class="cap"></div>
	                </div>
	                <div class="inner">
		                <div class="cap">
			                <textarea id="txtObservaciones" maxlength="100" onkeyup="return MaxLength(this)"></textarea>     
		                </div>
	                </div>
	                <div class="bottom">
		                <div class="cap"></div>
	                </div>
                </div>
            </li>
        </ul>

        <ul class="icon-list" id="detalles-calib">
            <li class="see" onclick="VerHerramienta()">Ver detalles de la herramienta</li>
            <li class="excel" onclick="">Descargar detalles de la calibración</li>
        </ul>
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li id="btnCalibracionAceptar"><div class="btn primary_action_button_small button_100"><div class="cap"><span>Aceptar</span></div></div></li>
            <li id="btnCalibracionCancelar"><div class="btn secondary_action_button_small button_100"><div class="cap"><span>Cancelar</span></div></div></li>
        </ul>
    </div>
</div>

<div class="dialog_wrapper" style="width:500px" id="divHerramientaImagen">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3 id="img_titulo"></h3></div></div></div>
    </div>

    <div class="dialog_content align-center">
        <img id="imgHerramienta" style="width:400px; height:400px"/>
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li><div class="btn primary_action_button_small button_100" id="btnHerramientaImagenAceptar"><div class="cap"><span>Aceptar</span></div></div></li>
        </ul>
    </div>
</div>

</asp:Content>

