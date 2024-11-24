<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="herramientasLista.aspx.cs" Inherits="stock_herramientasLista" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">
<!--[if lt IE 9]>     
<script src="/js/IE9.js" type="text/javascript"></script>
<![endif]-->
<script type="text/javascript">
    var current_page;
    var current_page_rh;
    var current_page_ev;
    var current_herramienta;
    var current_evento = null;
    var current_item;

    $(document).ready(function () {
        $('input[type="checkbox"]').click(function () {
            ActualizarCamposFiltros();
        });
        $('#txtFiltroNumeroH').change(function () {
            CambiarPagina(1);
        });
        $('#txtFiltroNumeroI').change(function () {
            CambiarPagina(1);
        });
        $('#txtFiltroTipo').change(function () {
            CambiarPagina(1);
        });
        $('#txtFiltroDescripcion').change(function () {
            CambiarPagina(1);
        });
        $('#contentPlacePage_cbFiltroPersonaCargo').change(function () {
            CambiarPagina(1);
        });
        $('#btnAgregarRegHist').click(function () {
            LimpiarCamposRegHist();

            MostrarVentana('divRegHist');
        });
        $('#btnRegHistCancelar').click(function () {
            MostrarVentana('divRegHistLista');
        });
        $('#btnHerramientaImagenAceptar').click(function () {
            CerrarVentana();
        });
        $('#btnRegHistAceptar').click(function () {
            var idPersonaCargo = $('#contentPlacePage_cbRegHistPersCargo');
            var ubicacion = $('#txtRegHistUbicacion');
            var descripcion = $('#txtRegHistDescripcion');
            var result = true;

            if (isNaN(idPersonaCargo.val()) || idPersonaCargo.val() == '<%= Constantes.IdPersonaInvalido %>') {
                idPersonaCargo.parents('.input_wrapper').addClass('input_wrapper_selectbox_error');
                result = false;
            }
            else {
                idPersonaCargo.parents('.input_wrapper').removeClass('input_wrapper_selectbox_error');
            }

            if (jQuery.trim(ubicacion.val()).length == 0) {
                ubicacion.parents('.input_wrapper').addClass('input_wrapper_error');
                result = false;
            }
            else {
                ubicacion.parents('.input_wrapper').removeClass('input_wrapper_error');
            }

            if (jQuery.trim(descripcion.val()).length == 0) {
                descripcion.parents('.input_wrapper').addClass('input_wrapper_error');
                result = false;
            }
            else {
                descripcion.parents('.input_wrapper').removeClass('input_wrapper_error');
            }

            if (result) {
                MostrarLoading();

                ConsultaAjax.url = 'herramientasLista.aspx/NuevoRegHist';
                ConsultaAjax.data = '{ "idHerramienta": "' + current_herramienta + '", "idPersonaCargo":"' + idPersonaCargo.val() + '", "ubicacion":"' + ubicacion.val() + '", "descripcion":"' + descripcion.val() + '" }';
                ConsultaAjax.AjaxSuccess = function (msg) {
                    LimpiarCamposRegHist();

                    GetRegHistLista(1);
                };
                ConsultaAjax.AjaxError = function (msg) {
                    ult_ventana = 'divRegHist';
                    ErrorMsg(msg);
                };

                ConsultaAjax.Ejecutar();
            }
        });
        $('#btnEliminarAceptar').click(function () {
            var motivo = $('#txtEliminarMotivo');
            var result = true;

            if (jQuery.trim(motivo.val()).length == 0) {
                motivo.parents('.textarea_wrapper').addClass('textarea_wrapper_error');
                result = false;
            }
            else {
                motivo.parents('.textarea_wrapper').removeClass('textarea_wrapper_error');
            }

            if (result) {
                MostrarLoading();

                ConsultaAjax.url = 'herramientasLista.aspx/EliminarHerramienta';
                ConsultaAjax.data = '{ "idHerramienta": "' + current_herramienta + '", "motivo":"' + motivo.val() + '" }';
                ConsultaAjax.AjaxSuccess = function (msg) {
                    $('#txtEliminarMotivo').val('');

                    CambiarPagina(1);
                };
                ConsultaAjax.AjaxError = function (msg) {
                    ult_ventana = 'divRegHist';
                    ErrorMsg(msg);
                };

                ConsultaAjax.Ejecutar();
            }
        });
        $('#btnAgregarEvento').click(function () {
            $('#ev_titulo').text('Agregar evento');
            $('#btnEventoEliminar').hide();

            LimpiarCamposEvento();

            MostrarVentana('divEvento');
        });
        $('#btnEventoCancelar').click(function () {
            LimpiarCamposEvento();

            MostrarVentana('divEventosLista');
        });
        $('#btnEventoAceptar').click(function () {
            var fecha = $('#txtEventoFecha');
            var descripcion = $('#txtEventoDescripcion');
            var result = true;

            if (jQuery.trim(fecha.val()).length == 0) {
                fecha.parents('.input_wrapper').addClass('input_wrapper_error');
                result = false;
            }
            else {
                fecha.parents('.input_wrapper').removeClass('input_wrapper_error');
            }

            if (jQuery.trim(descripcion.val()).length == 0) {
                descripcion.parents('.input_wrapper').addClass('input_wrapper_error');
                result = false;
            }
            else {
                descripcion.parents('.input_wrapper').removeClass('input_wrapper_error');
            }

            if (result) {
                MostrarLoading();

                var url;
                var data;

                if (current_evento == null) {
                    url = 'NuevoEvento';
                    data = '{ "idHerramienta": "' + current_herramienta + '", "fecha": "' + fecha.val() + '", "descripcion": "' + descripcion.val() + '" }'
                }
                else {
                    url = 'ActualizarEvento';
                    data = '{ "idHerramienta": "' + current_herramienta + '", "fecha": "' + fecha.val() + '", "descripcion": "' + descripcion.val() + '", "idEvento": "' + current_evento + '" }'
                }

                ConsultaAjax.url = 'herramientasLista.aspx/' + url;
                ConsultaAjax.data = data;
                ConsultaAjax.AjaxSuccess = function (msg) {
                    LimpiarCamposEvento();

                    GetEventosLista(1);
                };
                ConsultaAjax.AjaxError = function (msg) {
                    ult_ventana = 'divEvento';
                    ErrorMsg(msg);
                };

                ConsultaAjax.Ejecutar();
            }
        });
        
        // Instrumentos --------------------------------------------------------------
        $('#btnCalibracionCancelar').click(function() {
            LimpiarCalibracion();

            CerrarVentana();
        });
        $('#btnCalibracionAceptar').click(function() {
            var frecuencia = $('#contentPlacePage_cbFrecuencia');
            var ultCalib = $('#txtUltCalibracion');
            var proxCalib = $('#txtProxCalibracion');
            var tipoCalib = $('#contentPlacePage_cbTipoCalibracion');
            var observaciones = $('#txtObservaciones');
            var result = true;

            result &= TieneDatos(frecuencia, 'input_wrapper', 'input_wrapper_selectbox_error');
            result &= ContieneFecha(ultCalib, 'input_wrapper', 'input_wrapper_error');
            result &= ContieneFecha(proxCalib, 'input_wrapper', 'input_wrapper_error');
            result &= TieneDatos(tipoCalib, 'input_wrapper', 'input_wrapper_selectbox_error');
            result &= TieneDatos(observaciones, 'textarea_wrapper', 'textarea_wrapper_error');

            if (result) {
                MostrarLoading();

                if (current_item == null) {
                    ConsultaAjax.url = 'herramientasSeguimiento.aspx/NuevaCalibracion';
                    ConsultaAjax.data = JSON.stringify({ idInstrumento: current_item, frecuencia: frecuencia.val(), ultCalibracion: ultCalib.val(), proxCalibracion: proxCalib.val(), tipoCalibracion: tipoCalib.val(), observaciones: observaciones.val() });
                }
                else {
                    ConsultaAjax.url = 'herramientasSeguimiento.aspx/ActualizarCalibracion';
                    ConsultaAjax.data = JSON.stringify({ idInstrumento: current_item, frecuencia: frecuencia.val(), ultCalibracion: ultCalib.val(), proxCalibracion: proxCalib.val(), tipoCalibracion: tipoCalib.val(), observaciones: observaciones.val() });
                }
                ConsultaAjax.AjaxSuccess = function (msg) {
                    CambiarPagina(current_page);
                };
                ConsultaAjax.AjaxError = function (msg) {
                    ult_ventana = 'divCalibracion';
                    ErrorMsg(msg);
                };

                ConsultaAjax.Ejecutar();
            }
        });
        $('#btnEventoEliminar').click(function () {
            Mensaje('El evento se eliminará de forma definitiva, ¿desea continuar?', 'warning', true, true, 'Aceptar', 'Cancelar', 'OnEvEliminar()', 'OnEvCancelEliminar()');
        });

        // Fin instrumentos ------------------------------------------------------------
        $('#contentPlacePage_cbFiltroPersonaCargo').change();
    });

    function GetHerramientas(pagina){
        MostrarLoading();

        var numeroH = $('#txtFiltroNumeroH').val();
        var tipo = $('#txtFiltroTipo').val();
        var descripcion = $('#txtFiltroDescripcion').val();
        var idPersonaCargo = $('#contentPlacePage_cbFiltroPersonaCargo').val();

        if (numeroH.length == 0 || isNaN(numeroH)) {
            numeroH = <%= Constantes.ValorInvalido.ToString() %>;
            $('#txtFiltroNumeroH').val('');
        }
        if (!$('#chkFiltroPersonaCargo').is(':checked')) {
            idPersonaCargo = '<%= Constantes.IdPersonaInvalido %>';
        }

        ConsultaAjax.url = 'herramientasLista.aspx/GetHerramientas';
        ConsultaAjax.data = JSON.stringify({ pagina: pagina, numeroH: numeroH, tipo: tipo, descripcion: descripcion, idPersonaCargo: idPersonaCargo });
        ConsultaAjax.AjaxSuccess = function (msg) {
            current_page = pagina;
            
            $('#listado').html();
            var cant = msg.d.length;

            if (cant > 0) {
                var filas = [];
                var i = 0;
                for (i; i < cant; i++) {
                    var fila = '<tr class="fila-color">';
                    fila += '<td class="align-center">' + msg.d[i][1] + '</td>';
                    fila += '<td class="align-center">' + msg.d[i][2] + '</td>';
                    fila += '<td class="align-center">' + msg.d[i][3] + '</td>';
                    fila += '<td class="align-center">' + msg.d[i][4] + '</td>';
                    fila += '<td class="align-center">';
                    fila += '<a href="javascript:void(0);" onclick="VerHerramienta(\'' + msg.d[i][0] + '\');"><img src="/images/icons/icon_ver.png" alt="Detalles de la herramienta" title="Detalles de la herramienta" /></a> ';
                    fila += '<a href="javascript:void(0);" onclick="VerHistorial(\'' + msg.d[i][0] + '\');"><img src="/images/icons/icon_calendario.png" alt="Ver historial" title="Ver historial" /></a> ';
                    fila += '<a href="javascript:void(0);" onclick="VerEventos(\'' + msg.d[i][0] + '\');"><img src="/images/icons/icon_event.png" alt="Ver eventos" title="Ver eventos" /></a> ';
                    fila += '<a href="javascript:void(0);" onclick="EliminarHerramienta(\'' + msg.d[i][0] + '\');"><img src="/images/icons/icon_delete.png" alt="Eliminar herramienta" title="Eliminar herramienta" /></a>';
                    fila += '</td></tr>';
                    filas.push(fila);
                }

                $('#listado').html(filas.join(''));
            }
            else {
                $('#listado').html('<tr><td colspan="5" class="align-center">No hay herramientas disponibles.</td></tr>');
            }

            DibujarPaginasLista(numeroH, tipo, descripcion, idPersonaCargo);

            CerrarVentana();
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function DibujarPaginasLista(numeroH, tipo, descripcion, idPersonaCargo) {
        ConsultaAjax.url = 'herramientasLista.aspx/GetCantidadPaginas';
        ConsultaAjax.data = JSON.stringify({ numeroH: numeroH, tipo: tipo, descripcion: descripcion, idPersonaCargo: idPersonaCargo });
        ConsultaAjax.AjaxSuccess = function (msg) {
            $('#tdPaginas').html();

            var cont = [];

            if (current_page == 1) {
                cont.push('<span class="disabled">«Anterior</span>');
            }
            else {
                cont.push('<a href="#" onclick="CambiarPagina(' + (current_page - 1) + ')">«Anterior</a>');
            }
            cont.push('|');
            cont.push('<a href="#" onclick="CambiarPagina(1)">Inicio</a>');
            cont.push('|');
            if (msg.d == 0 || current_page == msg.d) {
                cont.push('<span class="disabled">Siguiente»</span>');
            }
            else {
                cont.push('<a href="#" onclick="CambiarPagina(' + (current_page + 1) + ')">Siguiente»</a>');
            }

            $('#tdPaginas').html(cont.join(' '));
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function GetRegHistLista(pagina) {
        MostrarLoading();

        ConsultaAjax.url = 'herramientasLista.aspx/GetHerramientasRegHistLista';
        ConsultaAjax.data = '{ "pagina": "' + pagina + '", "idHerramienta":"' + current_herramienta + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            current_page_rh = pagina;

            $('#regHistLista').html();
            var cant = msg.d.length;

            $('#rhl_Titulo').html('Registros históricos - ' + msg.d[0][0]);

            if (cant > 1) {
                var filas = [];
                var i = 1;
                for (i; i < cant; i++) {
                    var fila = '<tr class="fila-color">';
                    fila += '<td class="align-center">' + msg.d[i][0] + '</td>';
                    fila += '<td class="align-center">' + msg.d[i][1] + '</td>';
                    fila += '<td class="align-center">' + msg.d[i][2] + '</td>';
                    fila += '<td class="align-center">' + msg.d[i][3] + '</td>';
                    fila += '</tr>';
                    filas.push(fila);
                }

                $('#regHistLista').html(filas.join(''));
            }
            else {
                $('#regHistLista').html('<tr><td colspan="4" class="align-center">No hay registros históricos disponibles.</td></tr>');
            }

            DibujarPaginasRegHist(current_herramienta);
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function DibujarPaginasRegHist(idHerramienta) {
        ConsultaAjax.url = 'herramientasLista.aspx/GetCantidadPaginasRegHist';
        ConsultaAjax.data = '{ "idHerramienta":"' + idHerramienta + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            $('#tdPaginasRegHist').html();

            var cont = [];

            if (current_page_rh == 1) {
                cont.push('<span class="disabled">«Anterior</span>');
            }
            else {
                cont.push('<a href="#" onclick="GetRegHistLista(' + (current_page_rh - 1) + ')">«Anterior</a>');
            }
            cont.push('|');
            if (msg.d == 0 || current_page_rh == msg.d) {
                cont.push('<span class="disabled">Siguiente»</span>');
            }
            else {
                cont.push('<a href="#" onclick="GetRegHistLista(' + (current_page_rh + 1) + ')">Siguiente»</a>');
            }

            $('#tdPaginasRegHist').html(cont.join(' '));

            CerrarVentana();

            MostrarVentana('divRegHistLista');
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function GetEventosLista(pagina) {
        MostrarLoading();

        ConsultaAjax.url = 'herramientasLista.aspx/GetHerramientasEventosLista';
        ConsultaAjax.data = '{ "pagina": "' + pagina + '", "idHerramienta":"' + current_herramienta + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            current_page_ev = pagina;

            $('#eventLista').html();
            var cant = msg.d.length;

            $('#evl_titulo').html('Eventos - ' + msg.d[0][0]);

            if (cant > 1) {
                var filas = [];
                var i = 1;
                for (i; i < cant; i++) {
                    var fila = '<tr class="fila-color">';
                    fila += '<td class="align-center">' + msg.d[i][1] + '</td>';
                    fila += '<td class="align-center">' + msg.d[i][2] + '</td>';
                    fila += '<td class="align-center">';
                    fila += '<a href="javascript:void(0);" onclick="EditarEvento(' + msg.d[i][0] + ');"><img src="/images/icons/icon_edit_2.gif" alt="Editar evento" title="Editar evento" /></a> ';
                    fila += '</td></tr>';
                    filas.push(fila);
                }

                $('#eventLista').html(filas.join(''));
            }
            else {
                $('#eventLista').html('<tr><td colspan="3" class="align-center">No hay eventos disponibles.</td></tr>');
            }

            DibujarPaginasEventos(current_herramienta);
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function DibujarPaginasEventos(idHerramienta) {
        ConsultaAjax.url = 'herramientasLista.aspx/GetCantidadPaginasEventos';
        ConsultaAjax.data = '{ "idHerramienta":"' + idHerramienta + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            $('#tdPaginasEventos').html();

            var cont = [];

            if (current_page_ev == 1) {
                cont.push('<span class="disabled">«Anterior</span>');
            }
            else {
                cont.push('<a href="#" onclick="GetEventosLista(' + (current_page_ev - 1) + ')">«Anterior</a>');
            }
            cont.push('|');
            if (msg.d == 0 || current_page_ev == msg.d) {
                cont.push('<span class="disabled">Siguiente»</span>');
            }
            else {
                cont.push('<a href="#" onclick="GetEventosLista(' + (current_page_ev + 1) + ')">Siguiente»</a>');
            }

            $('#tdPaginasEventos').html(cont.join(' '));

            MostrarVentana('divEventosLista');
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function LimpiarCamposRegHist() {
        $('#contentPlacePage_cbRegHistPersCargo').val('<%= Constantes.IdPersonaInvalido %>');
        $('#contentPlacePage_cbRegHistPersCargo').change();
        $('#txtRegHistUbicacion').val('');
        $('#txtRegHistDescripcion').val('');

        $('#contentPlacePage_cbRegHistPersCargo').parents('.input_wrapper').removeClass('input_wrapper_selectbox_error');
        $('#txtRegHistUbicacion').parents('.input_wrapper').removeClass('input_wrapper_error');
        $('#txtRegHistDescripcion').parents('.input_wrapper').removeClass('input_wrapper_error');
    }

    function LimpiarCamposEvento() {
        $('#txtEventoFecha').val('').parents('.input_wrapper').removeClass('input_wrapper_error');
        $('#txtEventoDescripcion').val('').parents('.input_wrapper').removeClass('input_wrapper_error');

        current_evento = null;
    }

    function VerHerramienta(idHerramienta) {
        location.href = 'herramientaAdmin.aspx?p=' + idHerramienta;
    }
    function SeguimientoInstrumento(idHerramienta) {
        location.href = 'herramientasSeguimiento.aspx?p=' + idHerramienta;
    }
    function VerImagen(url) {
        $('#img_titulo').html('Detalle de herramienta / instrumento');

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

    function VerEventos(idHerramienta) {
        current_herramienta = idHerramienta;

        GetEventosLista(1);
    }

    function EliminarHerramienta(idHerramienta) {
        current_herramienta = idHerramienta;

        $('#txtEliminarMotivo').val('');
        $('#txtEliminarMotivo').parents('.textarea_wrapper').removeClass('textarea_wrapper_error');

        MostrarVentana('divEliminarHerramienta');
    }

    function EditarEvento(idEvento) {
        $('#btnEventoEliminar').show();

        current_evento = idEvento;

        ConsultaAjax.url = 'herramientasLista.aspx/GetEvento';
        ConsultaAjax.data = '{ "idEvento":"' + current_evento + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            $('#txtEventoFecha').val(msg.d[0]);
            $('#txtEventoDescripcion').val(msg.d[1]);

            $('#ev_titulo').text('Editar evento');

            MostrarVentana('divEvento');
        };
        ConsultaAjax.AjaxError = function (msg) {
            ult_ventana = 'divEventosLista';
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function OnEvCancelEliminar() {
        MostrarVentana('divEvento');
    }

    function OnEvEliminar() {
        ConsultaAjax.url = 'herramientasLista.aspx/EliminarEvento';
        ConsultaAjax.data = '{ "idEvento": "' + current_evento + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            LimpiarCamposEvento();

            GetEventosLista(1);
        };
        ConsultaAjax.AjaxError = function (msg) {
            ult_ventana = 'divEvento';
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function ActualizarCamposFiltros() {
        if ($('#chkFiltroPersonaCargo').is(':checked')) {
            $('#contentPlacePage_cbFiltroPersonaCargo').parents('.input_wrapper').removeClass('input_wrapper_selectbox_disabled');
            $('#contentPlacePage_cbFiltroPersonaCargo').removeAttr('disabled');
        }
        else {
            $('#contentPlacePage_cbFiltroPersonaCargo').parents('.input_wrapper').addClass('input_wrapper_selectbox_disabled');
            $('#contentPlacePage_cbFiltroPersonaCargo').attr('disabled', 'disabled');
        }

        CambiarPagina(1);
    }

    function CambiarPagina(idx) {
        <% if (this.MostrarHerramientas) { %>
            GetHerramientas(idx);
        <% } else { %>
            GetInstrumentos(idx);
        <% } %>
    }

    // Sección Instrumentos.
    function AgregarCalibracion(idInstrumento) {
        LimpiarCalibracion();

        current_item = idInstrumento;

        MostrarVentana('divCalibracion');
    }
    function VerCalibracion(idInstrumento) {
        MostrarLoading();

        ConsultaAjax.url = 'herramientasLista.aspx/GetCalibracion';
        ConsultaAjax.data = '{ "idInstrumento": "' + idInstrumento + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            if (msg.d.length > 0) {
                $('#contentPlacePage_cbFrecuencia').val(msg.d[1]);
                $('#txtUltCalibracion').val(msg.d[2]);
                $('#txtProxCalibracion').val(msg.d[3]);
                $('#contentPlacePage_cbTipoCalibracion').val(msg.d[4]);
                $('#txtObservaciones').val(msg.d[5]);

                $('#contentPlacePage_cbFrecuencia').change();
                $('#contentPlacePage_cbTipoCalibracion').change();

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
    function DescargarCalibracion() {
        MostrarLoading();

        ConsultaAjax.url = 'herramientasLista.aspx/GetDescargarCalibracion';
        ConsultaAjax.data = '{ "idInstrumento": "' + current_item + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            location.href = msg.d;

            MostrarVentana('divCalibracion');
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }
    function LimpiarCalibracion() {
        LimpiarCampo($('#contentPlacePage_cbFrecuencia'), 'input_wrapper_selectbox');
        LimpiarCampo($('#txtUltCalibracion'), 'input_wrapper');
        LimpiarCampo($('#txtProxCalibracion'), 'input_wrapper');
        LimpiarCampo($('#contentPlacePage_cbTipoCalibracion'), 'input_wrapper_selectbox');
        LimpiarCampo($('#txtObservaciones'), 'textarea_wrapper');
        $('#contentPlacePage_cbFrecuencia').change();
        $('#contentPlacePage_cbTipoCalibracion').change();
        $('#detalles-calib').hide();

        current_item = null;
    }
    function GetInstrumentos(pagina) {
        MostrarLoading();

        var numero = $('#txtFiltroNumeroI').val();
        var tipo = $('#contentPlacePage_cbFiltroTipo').val();
        var descripcion = $('#txtFiltroDescripcion').val();
        var idPersonaCargo = $('#contentPlacePage_cbFiltroPersonaCargo').val();

        if (!$('#chkFiltroTipo').is(':checked')) {
            tipo = <%= Constantes.ValorInvalido.ToString() %>;
        }
        if (!$('#chkFiltroPersonaCargo').is(':checked')) {
            idPersonaCargo = '<%= Constantes.IdPersonaInvalido %>';
        }

        ConsultaAjax.url = 'herramientasLista.aspx/GetInstrumentos';
        ConsultaAjax.data = JSON.stringify({ pagina: pagina, numeroI: numero, tipo: tipo, descripcion: descripcion, idPersonaCargo: idPersonaCargo });
        ConsultaAjax.AjaxSuccess = function (msg) {
            current_page = pagina;

            $('#listado').html();
            var cant = msg.d.length;

            if (cant > 0) {
                var filas = [];
                var i = 0;
                for (i; i < cant; i++) {
                    var fila = '<tr class="fila-color">';
                    fila += '<td class="align-center"><a href="javascript:void(0);" onclick="VerImagen(\'' + msg.d[i][12] + '\');"><img src="/images/icons/icon_camera.png" alt="Ver foto" title="Ver foto" /></a></td>';
                    fila += '<td class="align-center">' + msg.d[i][1] + '</td>';
                    fila += '<td class="align-left">' + msg.d[i][2] + '</td>';
                    fila += '<td class="align-left">' + msg.d[i][3] + '</td>';
                    fila += '<td class="align-center">' + msg.d[i][11] + '</td>';
                    fila += '<td class="align-center">' + msg.d[i][5] + '</td>';
                    fila += '<td class="align-center">' + msg.d[i][6] + '</td>';
                    fila += '<td class="align-center">' + DateTimeToString(msg.d[i][7]) + '</td>';
                    fila += '<td class="align-center">' + DateTimeToString(msg.d[i][8]) + '</td>';
                    fila += '<td class="align-left">' + msg.d[i][9] + '</td>';
                    fila += '<td class="align-center">' + msg.d[i][10] + '</td>';
                    fila += '<td class="align-center">';
                    fila += '<a href="javascript:void(0);" onclick="VerHerramienta(\'' + msg.d[i][0] + '\');"><img src="/images/icons/icon_ver.png" alt="Detalles de la herramienta" title="Detalles de la herramienta" /></a> ';
                    fila += '<a href="javascript:void(0);" onclick="VerHistorial(\'' + msg.d[i][0] + '\');"><img src="/images/icons/icon_calendario.png" alt="Ver historial" title="Ver historial" /></a> ';
                    if(msg.d[i][13].length > 0) {
                        fila += '<a href="javascript:void(0);" onclick="DescargarManual(\'' + msg.d[i][13] + '\');"><img src="/images/icons/icon_pdf.png" alt="Descargar manual del instrumento" title="Descargar manual del instrumento" /></a>';
                    }
                    fila += '<a href="javascript:void(0);" onclick="' + (msg.d[i][7].length == 0 ? 'AgregarCalibracion' : 'VerCalibracion') + '(\'' + msg.d[i][0] + '\');"><img src="/images/icons/icon_event.png" alt="' + (msg.d[i][7].length == 0 ? 'Agregar' : 'Ver') + ' calibración" title="' + (msg.d[i][7].length == 0 ? 'Agregar' : 'Ver') + ' calibración" /></a> ';
                    fila += '<a href="javascript:void(0);" onclick="EliminarHerramienta(\'' + msg.d[i][0] + '\');"><img src="/images/icons/icon_delete.png" alt="Eliminar herramienta" title="Eliminar herramienta" /></a>';
                    fila += '</td></tr>';
                    filas.push(fila);
                }

                $('#listado').html(filas.join(''));
            }
            else {
                $('#listado').html('<tr><td colspan="11" class="align-center">No hay instrumentos disponibles.</td></tr>');
            }

            DibujarPaginasListaInst(numero, tipo, descripcion, idPersonaCargo);

            CerrarVentana();
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function DibujarPaginasListaInst(numero, tipo, descripcion, idPersonaCargo) {
        ConsultaAjax.url = 'herramientasLista.aspx/GetCantidadPaginasInst';
        ConsultaAjax.data = JSON.stringify({ numeroI: numero, tipo: tipo, descripcion: descripcion, idPersonaCargo: idPersonaCargo });
        ConsultaAjax.AjaxSuccess = function (msg) {
            $('#tdPaginas').html();

            var cont = [];

            if (current_page == 1) {
                cont.push('<span class="disabled">«Anterior</span>');
            }
            else {
                cont.push('<a href="#" onclick="CambiarPagina(' + (current_page - 1) + ')">«Anterior</a>');
            }
            cont.push('|');
            cont.push('<a href="#" onclick="CambiarPagina(1)">Inicio</a>');
            cont.push('|');
            if (msg.d == 0 || current_page == msg.d) {
                cont.push('<span class="disabled">Siguiente»</span>');
            }
            else {
                cont.push('<a href="#" onclick="CambiarPagina(' + (current_page + 1) + ')">Siguiente»</a>');
            }

            $('#tdPaginas').html(cont.join(' '));
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
    <h1><%= this.MostrarHerramientas ? "Listado de herramientas" : "Control de equipos de seguimiento y medición"%></h1>
</div>

<div class="full-width scroll-horizontal">
    <table class="tbl listado" <%= this.MostrarHerramientas ? "" : "style=\"width:1720px\"" %> cellspacing="0">
        <thead>
            <tr>
                <% if (this.MostrarHerramientas) { %>
                <td class="border_left" style="width:50px">Nº H</td>
                <% } else { %>
                <td class="border_left" style="width:50px">&nbsp;</td>
                <td class="border_middle" style="width:50px">Nº I</td>
                <% } %>
                <td class="border_middle" style="width:190px">Tipo</td>
                <td class="border_middle" style="width:260px">Descripcion</td>
                <td class="border_middle" style="width:220px"><input id="chkFiltroPersonaCargo" type="checkbox" /> Persona a cargo</td>
                <% if (!this.MostrarHerramientas) { %>
                <td class="border_middle" style="width:150px">Nº serie</td>
                <td class="border_middle" style="width:120px">Frec calibración</td>
                <td class="border_middle" style="width:120px">Últ calibración</td>
                <td class="border_middle" style="width:120px">Prox calibración</td>
                <td class="border_middle" style="width:200px">Ubicación</td>
                <td class="border_middle" style="width:120px">Tipo calibración</td>
                <% } %>
                <td class="border_right" style="width:120px">Acciones</td>
            </tr>
            <tr class="filter_row">                
                <% if (this.MostrarHerramientas) { %>
                <td>
                    <div class="input_wrapper">
                        <div class="cap">
                            <input id="txtFiltroNumeroH" type="text"/>
                        </div>
                    </div>
                </td>
                <% } else { %>
                <td></td>
                <td>
                    <div class="input_wrapper">
                        <div class="cap">
                            <input id="txtFiltroNumeroI" type="text"/>
                        </div>
                    </div>
                </td>
                <% } %>
                <td>
                    <div class="input_wrapper">
                        <div class="cap">
                            <input id="txtFiltroTipo" type="text"/>
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
                            <span id="lblFiltroPersonaCargo" runat="server"></span>
                            <select id="cbFiltroPersonaCargo" runat="server"></select>
                        </div>
                    </div>
                </td>
                <% if (!this.MostrarHerramientas) { %>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <% } %>
                <td></td>
            </tr>
        </thead>
        <tbody id="listado">
            <tr>
                <td colspan="<%= this.MostrarHerramientas ? "5" : "12" %>" class="align-center">No hay <%= this.MostrarHerramientas ? "herramientas" : "instrumentos" %> disponibles.</td>
            </tr>
        </tbody>
        <tfoot>
            <tr>
                <td colspan="<%= this.MostrarHerramientas ? "5" : "12" %>" class="align-center" id="tdPaginas">
                </td>
            </tr>
        </tfoot>
    </table>
</div>

<div class="dialog_wrapper" style="width:650px" id="divRegHistLista">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3 id="rhl_Titulo">Registros históricos</h3></div></div></div>
    </div>

    <div class="dialog_content">
        <table class="tbl" width="97%" cellspacing="0">
            <thead>
                <tr>
                    <td class="border_left" style="width:80px">Fecha</td>
                    <td class="border_middle">Persona a cargo</td>
                    <td class="border_middle" style="width:150px">Ubicación</td>
                    <td class="border_right" style="width:150px">Descripción</td>
                </tr>
            </thead>
            <tbody id="regHistLista">
                <tr>
                    <td colspan="4" class="align-center">No hay registros históricos disponibles.</td>
                </tr>
            </tbody>
            <tfoot>
                <tr>
                    <td class="align-left" style="width:100px">
                        <a id="btnAgregarRegHist">Agregar registro</a>
                    </td>
                    <td colspan="3" class="align-right" id="tdPaginasRegHist">
                    </td>
                </tr>
            </tfoot>
        </table>
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li><div class="btn primary_action_button_small button_100" onclick="CerrarVentana();"><div class="cap"><span>Aceptar</span></div></div></li>
        </ul>
    </div>
</div>

<div class="dialog_wrapper" style="width:500px" id="divRegHist">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3>Agregar registro histórico</h3></div></div></div>
    </div>

    <div class="dialog_content">
        <ul class="middle_form">
            <li class="form_floated_item normal">
                <label class="label" for="txtItemMarca">Persona a cargo</label>
                <div class="input_wrapper input_wrapper_selectbox">
                    <div class="cap">
                        <span id="lblRegHistPersCargo"></span>
                        <select id="cbRegHistPersCargo" runat="server"></select>
                    </div>
                </div>
            </li>
            <li class="form_floated_item normal">
                <label class="label" for="txtRegHistUbicacion">Ubicación</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtRegHistUbicacion" value="" type="text"/>
                    </div>
                </div>
            </li>
            <li class="form_floated_item normal">
                <label class="label" for="txtRegHistDescripcion">Descripcion</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtRegHistDescripcion" value="" type="text"/>
                    </div>
                </div>
            </li>
        </ul>
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li><div class="btn primary_action_button_small button_100" id="btnRegHistAceptar"><div class="cap"><span>Aceptar</span></div></div></li>
            <li><div class="btn secondary_action_button_small button_100" id="btnRegHistCancelar"><div class="cap"><span>Cancelar</span></div></div></li>
        </ul>
    </div>
</div>

<div class="dialog_wrapper" style="width:500px" id="divEliminarHerramienta">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3>Eliminar herramienta</h3></div></div></div>
    </div>

    <div class="dialog_content">
        <p>Debe ingresar un motivo para la eliminación de la herramienta:</p><br />
        <ul class="middle_form">
            <li class="form_floated_item" style="width:460px">
                <div class="textarea_wrapper clear"> 
	                <div class="top">
		                <div class="cap"></div>
	                </div>
	                <div class="inner">
		                <div class="cap">
			                <textarea id="txtEliminarMotivo"></textarea>     
		                </div>
	                </div>
	                <div class="bottom">
		                <div class="cap"></div>
	                </div>
                </div>
            </li>
        </ul>

        <p><span class="important">Importante: </span>la herramienta se eliminará de forma definitiva.</p>
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li><div class="btn primary_action_button_small button_100" onclick="CerrarVentana()"><div class="cap"><span>Cancelar</span></div></div></li>
            <li><div class="btn secondary_action_button_small button_100" id="btnEliminarAceptar"><div class="cap"><span>Eliminar</span></div></div></li>
        </ul>
    </div>
</div>

<div class="dialog_wrapper" style="width:650px" id="divEventosLista">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3 id="evl_titulo">Eventos</h3></div></div></div>
    </div>

    <div class="dialog_content">
        <table class="tbl" width="97%" cellspacing="0">
            <thead>
                <tr>
                    <td class="border_left" style="width:120px">Fecha</td>
                    <td class="border_middle" style="width:380px">Descripción</td>
                    <td class="border_middle">Acciones</td>
                </tr>
            </thead>
            <tbody id="eventLista">
                <tr>
                    <td colspan="3" class="align-center">No hay eventos disponibles.</td>
                </tr>
            </tbody>
            <tfoot>
                <tr>
                    <td class="align-left" style="width:100px">
                        <a id="btnAgregarEvento">Agregar evento</a>
                    </td>
                    <td colspan="2" class="align-right" id="tdPaginasEventos">
                    </td>
                </tr>
            </tfoot>
        </table>
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li><div class="btn primary_action_button_small button_100" onclick="CerrarVentana();"><div class="cap"><span>Aceptar</span></div></div></li>
        </ul>
    </div>
</div>

<div class="dialog_wrapper" style="width:500px" id="divEvento">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3 id="ev_titulo"></h3></div></div></div>
    </div>

    <div class="dialog_content">
        <ul class="middle_form">
            <li class="form_floated_item normal">
                <label class="label" for="txtEventoFecha">Fecha</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtEventoFecha" value="" type="text"/>
                    </div>
                </div>
            </li>
            <li class="form_floated_item normal">
                <label class="label" for="txtEventoDescripcion">Descripcion</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtEventoDescripcion" value="" type="text"/>
                    </div>
                </div>
            </li>
        </ul>
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li><div class="btn primary_action_button_small button_100" id="btnEventoAceptar"><div class="cap"><span>Aceptar</span></div></div></li>
            <li><div class="btn secondary_action_button_small button_100" id="btnEventoCancelar"><div class="cap"><span>Cancelar</span></div></div></li>
            <li><div class="btn secondary_action_button_small button_100" id="btnEventoEliminar"><div class="cap"><span>Eliminar</span></div></div></li>
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

<div class="dialog_wrapper" style="width:500px" id="divCalibracion">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3>Detalle de la calibración</h3></div></div></div>
    </div>

    <div class="dialog_content">
        <ul class="middle_form">
            <li class="form_floated_item form_floated_item_50">
                <label class="label" for="cbFrecuencia">Frecuencia de calibración</label>
                <div class="input_wrapper input_wrapper_selectbox">
                    <div class="cap">
                        <span id="lblFrecuencia"></span>
                        <select id="cbFrecuencia" runat="server"></select>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_50 form_floated_item_right">
                <label class="label" for="txtFechaHasta">Fecha de última calibración</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtUltCalibracion" value="" maxlength="10" type="text" />
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_50">
                <label class="label" for="txtFechaHasta">Fecha de próxima calibración</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtProxCalibracion" value="" maxlength="10" type="text" />
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_50 form_floated_item_right">
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

</asp:Content>

