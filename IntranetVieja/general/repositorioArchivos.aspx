<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="repositorioArchivos.aspx.cs" Inherits="general_repositorioArchivos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">

<script src="/js/pdfobject.js" type="text/javascript" language="javascript"></script>

<script language="javascript" type="text/javascript">
    var current_item;
    var current_file;

    $(document).ready(function () {
        $('#btnCrearCarpeta').click(function () {
            ReiniciarCamposCarpeta();
            current_item = null;
            $('#lblTituloCarpeta').text('Crear carpeta');
            MostrarVentana('divCarpeta');
        });
        $('#btnCarpetaAceptar').click(function () {
            ActualizarCarpeta();
        });
        $('#btnCarpetaCancelar').click(function () {
            current_item = null;

            CerrarVentana();
        });
        $('#btnVerDocumentoAceptar').click(function () {
            CerrarVentana();
        });
        $('#btnAgregarPersona').click(function () {
            AgregarPersona('<%= Constantes.IdPersonaInvalido.ToString() %>', 8);
        });
        $('#btnEditarPropiedades').click(function () {
            MostrarLoading();

            ConsultaAjax.url = 'repositorioArchivos.aspx/GetPropiedades';
            ConsultaAjax.AjaxSuccess = function (msg) {
                ReiniciarCamposCarpeta();

                $('#lblTituloCarpeta').text('Editar carpeta');
                $('#txtNombreCarpeta').val(msg.d[0][0]);
                current_item = msg.d[0][0];
                MostrarVentana('divCarpeta');
            };
            ConsultaAjax.AjaxError = function (msg) {
                ErrorMsg(msg);
            };

            ConsultaAjax.Ejecutar();
        });
        $('#btnSubirArchivo').click(function () {
            LimpiarCampo($('#txtArchivo'), 'input_wrapper');
            $('#lblSubirArchivoError').hide();
            MostrarVentana('divSubirArchivo');
        });
        $('#btnSubirArchivoAceptar').click(function () {
            var archivo = $('#txtArchivo');
            var result = true;

            result &= TieneDatos(archivo, 'input_wrapper', 'input_wrapper_error');

            if (result) {
                MostrarLoading();
                $('#frameAux').contents().find("#btnSubirArchivo").click();
            }
        });
        $('#btnSubirArchivoCancelar').click(function () {
            CerrarVentana();
        });
        $('#txtArchivo').click(function () {
            $('#frameAux').contents().find("#txtArchivo").click();
        });
        $('#contentPlacePage_cbRepositorio').change(function () {
            console.log('change');
            location.href = 'repositorioArchivos.aspx?p=' + $(this).val();
        });
		
        GetDirectorio('');
        ActualizarUpload();
    });

    function ActualizarUpload() {
        $('#frameAux').attr('src', '/general/repositorioArchivosUpload.aspx');
    }

    function SetFile(file) {
        $('#txtArchivo').val(file);
    }

    function GetDirectorio(nombre) {
        MostrarLoading();

        ConsultaAjax.url = 'repositorioArchivos.aspx/GetDirectorio';
        ConsultaAjax.data = '{ "nombre": "' + nombre + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            $('#listado').html('');

            var cant = msg.d.length;
            if (cant > 0) {
                var filas = [];
                for (var i = 0; i < cant; i++) {
                    var fila = '<tr>';
                    if (msg.d[i][0] == 0) {
                        fila += '<td class="align-left rda-celda descripcion folder" onclick="GetDirectorio(\'' + msg.d[i][1] + '\')">' + msg.d[i][2] + '</td>';
                    } else {
                        fila += '<td class="align-left rda-celda descripcion ' + msg.d[i][5] + '"><div class="hasActions"><span onclick="' + (msg.d[i][5] != 'pdf' ? 'Descargar' : 'VerDocumento') + '(\'' + (msg.d[i][5] != 'pdf' ? msg.d[i][1] : msg.d[i][2]) + '\')">' + msg.d[i][2] + '</span><div class="input_actions actions_archivo" value="' + msg.d[i][6] + '"></div></div></td>';
                    }
                    fila += '<td class="align-left rda-celda">' + msg.d[i][3] + '</td>';
                    fila += '<td class="align-right rda-celda">' + msg.d[i][4] + '</td>';
                    fila += '</tr>';
                    filas.push(fila);
                }

                $('#listado').html(filas.join(''));

                MostrarAcciones();
            }
            else {
                $('#listado').html('<tr><td colspan="3" class="align-center">No hay archivos disponibles.</td></tr>');

                MostrarAcciones();
            }

            GetInfo();
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function EliminarArchivo() {
        custom_dialog.close();
        MostrarLoading();

        ConsultaAjax.url = 'repositorioArchivos.aspx/EliminarArchivo';
        ConsultaAjax.data = JSON.stringify({ archivo: current_file });
        ConsultaAjax.AjaxSuccess = function (msg) {
            current_file = null;
            GetDirectorio(msg.d);
        };
        ConsultaAjax.AjaxError = function (msg) {
            current_file = null;
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function MostrarAcciones() {
        ConsultaAjax.url = 'repositorioArchivos.aspx/TienePermisos';
		ConsultaAjax.data = null;
        ConsultaAjax.AjaxSuccess = function (msg) {
            DibujarAcciones();
            EnlazarEventosMaster();
            EnlazarEventos();

            $('#accionesCarpeta').show();
        };
        ConsultaAjax.AjaxError = function (msg) {
            $('#accionesCarpeta').hide();
        };

        ConsultaAjax.Ejecutar();   
    }

    function ReiniciarCamposCarpeta() {
        LimpiarCampo($('#txtNombreCarpeta'), 'input_wrapper');
        $('#txtNombreCarpeta').removeAttr('readonly');
        $('#lblCarpetaError').hide();
    }

    function ActualizarCarpeta() {
        var nombre = $('#txtNombreCarpeta');
        var result = true;
        
        result &= TieneDatos(nombre, 'input_wrapper', 'input_wrapper_error');

        if (result) {
            ConsultaAjax.url = 'repositorioArchivos.aspx/' + (current_item == null ? 'CrearCarpeta' : 'ActualizarCarpeta');
            ConsultaAjax.data = JSON.stringify({ nombre: nombre.val() });
            ConsultaAjax.AjaxSuccess = function (msg) {
                GetDirectorio(msg.d);
            };
            ConsultaAjax.AjaxError = function (msg) {
                $('#lblCarpetaError').html(msg);
                $('#lblCarpetaError').show();
                MostrarVentana('divCarpeta');
            };

            ConsultaAjax.Ejecutar();
        }
    }

    function GetInfo() {
        ConsultaAjax.url = 'repositorioArchivos.aspx/GetInfo';
        ConsultaAjax.AjaxSuccess = function (msg) {
            var cant = msg.d.length;

            if (cant > 0) {
                var cont = []

                cont.push('<a href="#" onclick="GetDirectorio(\'\')">Ir al inicio</a>');
                cont.push(' | ');
                if (cant == 1) {
                    cont.push('<a href="#" onclick="GetDirectorio(\'' + msg.d[0] + '\')">Subir un nivel</a>');
                }
                else {
                    cont.push('<span class="disabled">Subir un nivel</span>');
                }

                $('#tdNavegacion').html(cont.join(''));
            }

            CerrarVentana();
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function Descargar(url) {
        location.href = url;
    }

    function VerDocumento(documento) {
        ConsultaAjax.url = 'repositorioArchivos.aspx/GetDocumento';
        ConsultaAjax.data = '{ "documento": "' + documento + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            var cant = msg.d.length;

            if (cant > 0) {
                try {
                    var success = new PDFObject({ url: msg.d }).embed("pdf");
                }
                catch (err) {
                    
                }
                CerrarVentana();
                MostrarVentana('docViewer-container');
            }
        };
        ConsultaAjax.AjaxError = function (msg) {
            CerrarVentana();
        };

        ConsultaAjax.Ejecutar();
    }

    function DibujarAcciones() {
        $('.actions_archivo').html('<ul><li class="delete" alt="Eliminar" title="Eliminar"></li></ul>');
    }

    var EnlazarEventos = function () {
        $('.actions_archivo ul li.delete').unbind('click').click(function () {
            var value = $(this).parents('.input_actions').attr('value');
            current_file = value;

            Mensaje('El archivo se eliminará de forma definitiva. ¿Desea continuar?', 'warning', true, true, 'Cancelar', 'Eliminar', 'custom_dialog.close()', 'EliminarArchivo()');
        });
    }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlacePage" Runat="Server">

<div class="page-title">
    <h1>Repositorio de archivos</h1>
</div>

<span id="url"></span>

<div class="form_place">
    <ul class="middle_form" style="height:70px">
        <li class="form_floated_item form_floated_item_half" style="width:400px">
            <label class="label" for="txtAsunto">Repositorio actual</label>
            <div class="input_wrapper input_wrapper_selectbox">
                <div class="cap">
                    <span id="lblRepositorio"><%= Repositorio.Nombre %></span>
                    <select id="cbRepositorio" runat="server"></select>
                </div>
            </div>
        </li>
    </ul>
</div>

<div class="full-width">
    <table class="tbl editable listado" cellspacing="0">
        <thead>
            <tr>
                <td class="border_left">Nombre</td>
                <td class="border_middle" style="width:250px">Tipo</td>
                <td class="border_right" style="width:120px">Tamaño</td>
            </tr>
            <tr id="accionesCarpeta" class="filter_row">
                <td colspan="3">
                    <ul class="rda_actions">
                        <li class="folder" id="btnCrearCarpeta">Crear carpeta</li>
                        <li class="folder" id="btnEditarPropiedades">Editar propiedades</li>
                        <li class="add" id="btnSubirArchivo">Subir archivo</li>
                    </ul>
                </td>
            </tr>
        </thead>
        <tbody id="listado">

        </tbody>
        <tfoot>
            <tr>
                <td class="align-center" colspan="3" id="tdNavegacion">
                    
                </td>
            </tr>
        </tfoot>
    </table>
</div>

<div class="dialog_wrapper" style="width:500px" id="divCarpeta">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3 id="lblTituloCarpeta"></h3></div></div></div>
    </div>

    <div class="dialog_content">
        <ul class="middle_form">
            <li class="form_floated_item form_floated_item_100">
                <label class="label" for="txtNombreCarpeta">Nombre de la carpeta</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtNombreCarpeta" maxlength="50" type="text"/>
                    </div>
                </div>
            </li>
        </ul>

        <br />
        <p class="clear" id="lblCarpetaError">ERROR</p>
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li id="btnCarpetaAceptar"><div class="btn primary_action_button_small button_100"><div class="cap"><span>Aceptar</span></div></div></li>
            <li id="btnCarpetaCancelar"><div class="btn secondary_action_button_small button_100"><div class="cap"><span>Cancelar</span></div></div></li>
        </ul>
    </div>
</div>

<div class="dialog_wrapper" style="width:400px" id="divSubirArchivo">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3>Subir archivo</h3></div></div></div>
    </div>

    <div class="dialog_content">
        <ul class="middle_form">
            <li class="form_floated_item form_floated_item_100">
                <label class="label" for="txtArchivo">Seleccione un archivo</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtArchivo" readonly="readonly" maxlength="255" type="text" />
                    </div>
                </div>
            </li>
        </ul>

        <br />
        <p class="clear" id="lblSubirArchivoError">ERROR</p>
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li id="btnSubirArchivoAceptar"><div class="btn primary_action_button_small button_100"><div class="cap"><span>Aceptar</span></div></div></li>
            <li id="btnSubirArchivoCancelar"><div class="btn secondary_action_button_small button_100"><div class="cap"><span>Cancelar</span></div></div></li>
        </ul>
    </div>
</div>

<div class="dialog_wrapper" id="docViewer-container" style="width:950px">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3>Ver documento</h3></div></div></div>
    </div>

    <div class="dialog_content">
        <div id="pdf" style="height:500px">Al parecer, no posee un lector de archivos pdf o no es compatible con el navegador. <a class="link" target="_blank" href="http://get.adobe.com/es/reader/">Presione aquí para descargar uno</a>.</div>
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li id="btnVerDocumentoAceptar"><div class="btn primary_action_button_small button_100"><div class="cap"><span>Aceptar</span></div></div></li>
        </ul>
    </div>
</div>
</asp:Content>

