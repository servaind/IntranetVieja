<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="instrumentosLista.aspx.cs" Inherits="stock_instrumentosLista" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">

<style type="text/css">
    .tbl_width {width:480px;}
    .col_descripcion {width:210px;}
    .col_tipo {width:170px;}
    .col_tamanio {width:100px;}
</style>

<script language="javascript" type="text/javascript">
    var current_instrumento;
    var current_page;
    $(document).ready(function () {
        $('#txtBuscar').change(function () {
            ActualizarResultados(1);
        });
        $('#btnBuscar').click(function () {
            ActualizarResultados(1);
        });
        $('#btnCalibActualizar').click(function () {
            OnActualizarCalibracion();
        });
        $('#btnCalibCancelar').click(function () {
            LimpiarActualizarCalibracion();
            CerrarVentana();
        });
        $('#btnRegistroActualizar').click(function () {
            OnActualizarRegistro();
        });
        $('#btnRegistroCancelar').click(function () {
            LimpiarActualizarRegistro();
            CerrarVentana();
        });
        $('#btnFotosCerrar').click(function () {
            CerrarVentana();
        });
        $('#btnManualesCerrar').click(function () {
            CerrarVentana();
        });

        $('#txtCalibUltCalib').datepicker({ dayNames: dayNames, dayNamesMin: dayNamesMin, monthNames: monthNames, dateFormat: 'dd/mm/yy', maxDate: '<%= DateTime.Now.ToShortDateString() %>' });

        current_page = 1;
        ActualizarResultados(current_page);
    });

    function ActualizarResultados(page) {
        MostrarLoading();

        $.ajax({
            url: 'instrumentosLista.aspx/GetInstrumentos',
            data: JSON.stringify({ pagina: page, query: $('#txtBuscar').val() }),
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: true,
            cache: false,
            success: function (msg) {
                var count = msg.d.Lista.length;
                var results = [];

                current_page = page;

                if (count > 0) {
                    var i = 0;
                    for (i; i < count; i++) {
                        results.push(GetInstrumento(msg.d.Lista[i]));
                    }
                }
                else {
                    results.push('<div class="suggestion_message success">No se encontraron instrumentos que coincidan con la búsqueda.</div>');
                }

                ActualizarTablaPaginas(msg.d.TotalPaginas);
                $('#inst-results').html(results.join(''));
                $('#inst-results').scrollTop(0);

                BindResultEvents();

                CerrarVentana();
            },
            error: function (data, ajaxOptions, thrownError) {
                ErrorMsg(GetAjaxError(data));
            }
        });
    }

    function ActualizarTablaPaginas(totalPaginas) {
        var grid = $('#dgInstrumentos');

        var disabled = totalPaginas == 0;
        var max_page = parseInt(totalPaginas) + (disabled ? 1 : 0);

        var pages_container = '<div class="page_container"><div class="page_buttons">';
        pages_container += '<div id="btnFirst" class="page_button page_first ' + (disabled ? 'disabled' : '') + '" title="Ir al principio"></div>';
        pages_container += '<div id="btnPrev" class="page_button page_prev ' + (disabled ? 'disabled' : '') + '" title="Página anterior"></div>';
        pages_container += '<div class="page_button page_sep"></div>';
        pages_container += '<div class="page_button_input">';
        pages_container += '<span>Página </span>';
        pages_container += '<input id="txtPage" type="text" value="' + current_page + '" max_page="' + max_page + '" maxlength="4" ' + (disabled ? 'readonly="readonly"' : '') + ' />';
        pages_container += '<span>de ' + max_page + '</span>';
        pages_container += '</div>';
        pages_container += '<div class="page_button page_sep"></div>';
        pages_container += '<div id="btnNext" class="page_button page_next ' + (disabled ? 'disabled' : '') + '" title="Página siguiente"></div>';
        pages_container += '<div id="btnLast" class="page_button page_last ' + (disabled ? 'disabled' : '') + '" title="Ir al final"></div>';
        pages_container += '</div></div>';

        $('#inst-pages').html(pages_container);
    }

    function GetInstrumento(r) {
        var result;

        result = '<div class="inst-container" id="' + r.ID + '">';
        result += '<div class="numero">' + r.Numero + '</div>';
        result += '<div class="foto"><img src="/getImage.aspx?p=' + r.PathImagen + '" /><div class="lupa"></div></div>';
        result += '<div class="info-container">';
        result += '<p class="tipo">' + r.Tipo + '</p>';    // [Tipo]
        result += '<p class="marca">' + r.Marca + ' ' + r.Modelo + ' <span class="serie" title="Nº de serie">(' + r.NumeroSerie + ')</span></p>';    // [Marca][Modelo](Número de serie)
        result += '<p class="Grupo">Grupo: ' + r.Grupo + '</p>';
        result += '<p class="ubicacion">Ubicación: ' + r.Ubicacion + '</p>';
        result += '<p class="responsable">Responsable: ' + r.Responsable + '</p>';
        result += '<p class="resolucion">Resolucion: ' + r.Resolucion + '</p>';
        result += '<p class="Clase">Clase: ' + r.Clase + '</p>';
        result += '<p class="Rango">Rango: ' + r.Rango + '</p>';
        result += '<p class="incertidumbre">Incertidumbre: ' + r.Incertidumbre + '</p>';
        
        result += '</div>';
        
        result += '<div class="seccion calib-container ok">';
        result += '<span class="item titulo">Comprobación</span>';
        result += '<span class="item last" title="Última Comprobación">' + r.FechaComprobacion + '</span>';
        result += '<span class="item next" title="Próxima Comprobación">' + r.ProxComprobacion + ' (' + r.ComprobFrec + ')</span>';
        result += '<span class="item refresh hand act-calibracion">actualizar</span>';
        
        result += '</div>';

        result += '<div class="seccion calib-container ok' + GetClaseCalib(r) + '">';
        result += '<span class="item titulo">Mantenimiento</span>';
        result += '<span class="item last" title="Último Mantenimiento">' + r.FechaMantenimiento + '</span>';
        result += '<span class="item next" title="Próximo Mantenimiento">' + r.ProxMantenimiento + ' (' + r.MtoFrec + ')</span>';
        result += '<span class="item refresh hand act-calibracion">actualizar</span>';

        result += '</div>';

        result += '<div class="seccion calib-container ' + GetClaseCalib(r) + '">';
        result += '<span class="item titulo">Calibración</span>';
        result += '<span class="item last" title="Última calibración">' + r.CalibUlt + '</span>';
        result += '<span class="item next" title="Próxima calibración">' + r.CalibProx + ' (' + r.CalibFrec + ')</span>';
        result += '<span class="item refresh hand act-calibracion">actualizar</span>';

        result += '</div>';

        result += '<div class="seccion actions-container">';
        if (r.HasCertif) result += '<span class="item pdf certif-calibracion" url="' + r.PathCertif + '">Cert. de calibración</span>';
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

    function BindResultEvents() {
        $('.inst-container .foto').unbind('click').click(function () {
            var numero = $(this).prev('.numero').text();

            VerFotos(numero);
        });
        $('.inst-container .certif-calibracion').unbind('click').click(function () {
            var url = $(this).attr('url');

            location.href = '/download.aspx?p=' + url;
        });
        $('.inst-container .eac').unbind('click').click(function () {
            var url = $(this).attr('url');

            location.href = '/download.aspx?p=' + url;
        });
        $('.inst-container .manuales').unbind('click').click(function () {
            var numero = $(this).parents('.inst-container').find('.numero').text();

            VerManuales(numero);
        });
        $('.inst-container .act-calibracion').unbind('click').click(function () {
            var id = $(this).parents('.inst-container').attr('id');
            var numero = $(this).parents('.inst-container').find('.numero').text();

            ActualizarCalibracion(id, numero);
        });
        $('.inst-container .act-registro').unbind('click').click(function () {
            var id = $(this).parents('.inst-container').attr('id');
            var numero = $(this).parents('.inst-container').find('.numero').text();

            ActualizarRegistro(id, numero);
        });
        $('#btnFirst').unbind('click').click(function () {
            var current_page = parseInt($('#txtPage').val());
            var max_page = parseInt($('#txtPage').attr('max_page'));

            if (current_page != 1) {
                ActualizarResultados(1);
            }
        });
        $('#btnPrev').unbind('click').click(function () {
            var current_page = parseInt($('#txtPage').val());
            var max_page = parseInt($('#txtPage').attr('max_page'));

            if (current_page > 1) {
                ActualizarResultados(current_page - 1);
            }
        });
        $('#btnNext').unbind('click').click(function () {
            var current_page = parseInt($('#txtPage').val());
            var max_page = parseInt($('#txtPage').attr('max_page'));

            if (current_page < max_page) {
                ActualizarResultados(current_page + 1);
            }
        });
        $('#btnLast').unbind('click').click(function () {
            var current_page = parseInt($('#txtPage').val());
            var max_page = parseInt($('#txtPage').attr('max_page'));

            if (current_page != max_page) {
                ActualizarResultados(max_page);
            }
        });
        $('#txtPage').unbind('change').change(function () {
            if (isNaN($(this).val())) $(this).val(0);

            var current_page = parseInt($(this).val());
            var max_page = parseInt($(this).attr('max_page'));

            if (current_page > max_page) current_page = max_page;
            else if (current_page <= 0) current_page = 1;

            ActualizarResultados(current_page);
        });
    }

    function LimpiarActualizarCalibracion() {
        OcultarErrorCalibracion();
        $('#lblCalibInstrumento').text('-');
        LimpiarCampo($('#txtCalibUltCalib'), 'input_wrapper');

        current_instrumento = null;
    }

    function ActualizarCalibracion(id, instrumento) {
        LimpiarActualizarCalibracion();
        $('#lblCalibInstrumento').text(instrumento);

        current_instrumento = id;

        MostrarVentana('divCalibracion');

        $('#txtCalibUltCalib').focus();
    }

    function OnActualizarCalibracion() {
        OcultarErrorCalibracion();

        var result = true;
        var ultCalib = $('#txtCalibUltCalib');

        result &= ContieneFecha(ultCalib, 'input_wrapper', 'input_wrapper_error');

        if (result) {
            $.ajax({
                url: 'instrumentosLista.aspx/UpdateCalibInstrumento',
                data: JSON.stringify({ idInstrumento: current_instrumento, fecha: ultCalib.val() }),
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                async: true,
                cache: false,
                success: function (msg) {
                    Mensaje('La fecha de calibración fue actualizada.', 'success', true, false, 'Aceptar', '', 'ActualizarResultados(current_page)', '');
                },
                error: function (data, ajaxOptions, thrownError) {
                    MostrarErrorCalibracion(GetAjaxError(data));
                }
            });
        }
        else {
            MostrarErrorCalibracion('Debe ingresar la fecha de la última calibración.');
        }
    }

    function OcultarErrorCalibracion() {
        $('#calib_error').hide();
    }

    function MostrarErrorCalibracion(msg) {
        $('#calib_error_text').text(msg);
        $('#calib_error').show();
    }

    function LimpiarActualizarRegistro() {
        OcultarErrorCalibracion();
        $('#lblUbicacionInstrumento').text('-');
        LimpiarCampo($('#contentPlacePage_txtUbicacion'), 'input_wrapper');

        current_instrumento = null;
    }

    function ActualizarRegistro(id, instrumento) {
        LimpiarActualizarRegistro();
        $('#lblUbicacionInstrumento').text(instrumento);

        current_instrumento = id;

        MostrarVentana('divRegistro');

        $('#contentPlacePage_cbGrupo').focus();
    }

    function OnActualizarRegistro() {
        OcultarErrorRegistro();

        var result = true;
        var grupo = $('#contentPlacePage_cbGrupo');
        var ubicacion = $('#contentPlacePage_txtUbicacion');
        var responsable = $('#contentPlacePage_cbResponsable');

        result &= TieneDatos(grupo, 'input_wrapper_selectbox', 'input_wrapper_selectbox_error');
        result &= TieneDatos(ubicacion, 'input_wrapper', 'input_wrapper_error');
        result &= TieneDatos(responsable, 'input_wrapper_selectbox', 'input_wrapper_selectbox_error');

        if (result) {
            $.ajax({
                url: 'instrumentosLista.aspx/UpdateRegistroInstrumento',
                data: JSON.stringify({ idInstrumento: current_instrumento, idGrupo: grupo.val(), ubicacion: ubicacion.val(), idResponsable: responsable.val() }),
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                async: true,
                cache: false,
                success: function (msg) {
                    if (msg.d.Success) Mensaje('La ubicación fue actualizada.', 'success', true, false, 'Aceptar', '', 'ActualizarResultados(current_page)', '');
                    else MostrarErrorRegistro(msg.d.Message);
                },
                error: function (data, ajaxOptions, thrownError) {
                    MostrarErrorRegistro(GetAjaxError(data));
                }
            });
        }
        else {
            MostrarErrorRegistro('Debe completar todos los campos.');
        }
    }

    function OcultarErrorRegistro() {
        $('#registro_error').hide();
    }

    function MostrarErrorRegistro(msg) {
        $('#registro_error_text').text(msg);
        $('#registro_error').show();
    }

    function VerFotos(numero) {
        MostrarLoading();

        $.ajax({
            url: 'instrumentosLista.aspx/GetInstrumentoImagenes',
            data: JSON.stringify({ numero: numero }),
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: true,
            cache: false,
            success: function (msg) {
                $('#lblFotosTitulo').text('Instrumento Nº' + numero);

                var thumbnails = '';
                var c = msg.d.length;
                for (var i = 0; i < c; i++) {
                    thumbnails += '<div class="thumbnail" i="' + msg.d[i] + '">';
                    thumbnails += '<img src="/getImage.aspx?p=' + msg.d[i] + '" />';
                    thumbnails += '</div>';
                }

                if (c > 0) {
                    $('#fotosThumbnails').html(thumbnails);
                    BindPhotoNavEvents();

                    ChangePreviewImage('/getImage.aspx?p=' + msg.d[0]);

                    MostrarVentana('divFotos');
                }
                else {
                    CerrarVentana();
                }
            },
            error: function (data, ajaxOptions, thrownError) {
                ErrorMsg('No se pueden cargar las fotos para el instrumento seleccionado.');
            }
        });
    }

    function ChangePreviewImage(src) {
        $('#fotosPreview').attr('src', src);
    }

    function BindPhotoNavEvents() {
        $('.photoNav .thumbnail').unbind('click').click(function () {
            ChangePreviewImage($(this).find('img').attr('src'));
        });
    }

    function VerManuales(numero) {
        MostrarLoading();

        $.ajax({
            url: 'instrumentosLista.aspx/GetInstrumentoManuales',
            data: JSON.stringify({ numero: numero }),
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: true,
            cache: false,
            success: function (msg) {
                $('#lblManualesTitulo').text('Instrumento Nº' + numero);

                var rows = [];
                var c = msg.d.length;
                for (var i = 0; i < c; i++) {
                    var row = '<tr class="hand download-manual" url="' + msg.d[i][0] + '">';
                    row += '<td class="text align-left col_descripcion">' + AcortarTexto(msg.d[i][1], 35) + '</td>';
                    row += '<td class="text align-left col_tipo">' + msg.d[i][2] + '</td>';
                    row += '<td class="text align-right col_tamanio">' + msg.d[i][3] + '</td>';

                    rows.push(row);
                }

                if (c > 0) {
                    $('#dgManuales').setRows(rows.join(''));

                    $('.download-manual').unbind('click').click(function () {
                        var url = $(this).attr('url');

                        location.href = '/download.aspx?p=' + url;
                    });

                    MostrarVentana('divManuales');
                }
                else {
                    CerrarVentana();
                }
            },
            error: function (data, ajaxOptions, thrownError) {
                ErrorMsg('No se pueden cargar los manuales para el instrumento seleccionado.');
            }
        });
    }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlacePage" Runat="Server">

<div class="page-title">
    <h1>Seguimiento de instrumentos</h1>
</div>

<div class="full-width">
        <div class="middle_form" style="width: 370px;margin:0 auto; margin-bottom: 10px;">
            <ul class="lista-colores">
                <li class="titulo">Referencias:</li>
               <li class="verde">Calibrado</li> 
               <li class="amarillo">Próximo a calibrar</li> 
               <li class="rojo">No calibrado</li> 
            </ul>
        </div>

        <ul class="middle_form" style="width:400px;margin:0 auto">
            <li class="form_floated_item form_floated_item_100">
                <label>Ingrese algún patrón a buscar</label>
                <div id="search-container" class="input_wrapper default_text">                    
                    <div class="cap">
                        <input id="txtBuscar" maxlength="50" type="text"/>
                    </div>
                    <div id="btnBuscar" title="Buscar" class="input_button search input_button_inner"></div>
                </div>
            </li>
        </ul>

    <div class="inst-results" id="inst-results"></div>
    <div class="inst-pages" id="inst-pages"></div>
</div>

<div class="dialog_wrapper" style="width:400px" id="divCalibracion">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3>Actualizar calibración</h3></div></div></div>
    </div>

    <div class="dialog_content">
        <ul class="middle_form">
            <li class="form_floated_item form_floated_item_100" style="display:none" id="calib_error">
                <div class="suggestion_message error"><span id="calib_error_text"></span></div>
            </li>
            <li class="form_floated_item form_floated_item_50">
                <label class="label">Instrumento</label>
                <span id="lblCalibInstrumento"></span>
            </li>
            <li class="form_floated_item form_floated_item_50 form_floated_item_right">
                <label class="label">Última calibración</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtCalibUltCalib" readonly="readonly" value="" type="text"/>
                    </div>
                </div>
            </li>
        </ul>
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li><div class="btn primary_action_button_small button_100" id="btnCalibActualizar"><div class="cap"><span>Actualizar</span></div></div></li>
            <li><div class="btn secondary_action_button_small button_100" id="btnCalibCancelar"><div class="cap"><span>Cancelar</span></div></div></li>
        </ul>
    </div>
</div>

<div class="dialog_wrapper" style="width:500px" id="divRegistro">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3>Actualizar ubicación</h3></div></div></div>
    </div>

    <div class="dialog_content">
        <ul class="middle_form">
            <li class="form_floated_item form_floated_item_100" style="display:none" id="registro_error">
                <div class="suggestion_message error"><span id="registro_error_text"></span></div>
            </li>
            <li class="form_floated_item form_floated_item_50">
                <label class="label">Instrumento</label>
                <span id="lblUbicacionInstrumento"></span>
            </li>
            <li class="form_floated_item form_floated_item_50 form_floated_item_right">
                <label class="label" for="cbTipoHerramienta">Grupo</label>
                <div class="input_wrapper input_wrapper_selectbox">
                    <div class="cap">
                        <span id="lblGrupo" runat="server"></span>
                        <select id="cbGrupo" runat="server"></select>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_50">
                <label class="label" for="txtMarca">Ubicación</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtUbicacion" value="" maxlength="5" type="text" runat="server"/>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_50 form_floated_item_right">
                <label class="label" for="cbTipoHerramienta">Responsable</label>
                <div class="input_wrapper input_wrapper_selectbox">
                    <div class="cap">
                        <span id="lblResponsable" runat="server"></span>
                        <select id="cbResponsable" runat="server"></select>
                    </div>
                </div>
            </li>
        </ul>
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li><div class="btn primary_action_button_small button_100" id="btnRegistroActualizar"><div class="cap"><span>Actualizar</span></div></div></li>
            <li><div class="btn secondary_action_button_small button_100" id="btnRegistroCancelar"><div class="cap"><span>Cancelar</span></div></div></li>
        </ul>
    </div>
</div>

<div class="dialog_wrapper" style="width:500px" id="divFotos">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3 id="lblFotosTitulo"></h3></div></div></div>
    </div>

    <div class="dialog_content photoNav">
        <div class="preview"><img id="fotosPreview" /></div>
        <div class="thumbnails" id="fotosThumbnails"></div>
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li><div class="btn primary_action_button_small button_100" id="btnFotosCerrar"><div class="cap"><span>Cerrar</span></div></div></li>
        </ul>
    </div>
</div>

<div class="dialog_wrapper" style="width:520px" id="divManuales">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3 id="lblManualesTitulo"></h3></div></div></div>
    </div>

    <div class="dialog_content photoNav">
        <div class="datagrid-container" style="width:500px;">
            <table id="dgManuales" class="datagrid tbl_width" cellspacing="0" cellpadding="0">
                <thead>
                    <tr>
                        <td class="border_left col_descripcion">Descripcion</td>
                        <td class="border_middle col_tipo">Tipo</td>
                        <td class="border_right col_tamanio">Tamaño</td>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td colspan="3">
                            <div class="blockDiv"></div>
                            <div class="scrollable">
                                <table cellpadding="0" cellspacing="0" class="tbl_width">
                                    <tr>
                                        <td class="align-center text tbl_width">No hay archivos disponibles.</td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="3" class="border"></td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li><div class="btn primary_action_button_small button_100" id="btnManualesCerrar"><div class="cap"><span>Cerrar</span></div></div></li>
        </ul>
    </div>
</div>

</asp:Content>

