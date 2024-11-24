<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="personalLista.aspx.cs" Inherits="sistemas_personalLista" %>

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
        $('#txtFiltroIdPersona').change(function () {
            ActualizarCamposFiltros();
        });
        $('#txtFiltroNombre').change(function () {
            ActualizarCamposFiltros();
        });
        $('#txtFiltroUsuario').change(function () {
            ActualizarCamposFiltros();
        });
        $('#contentPlacePage_cbFiltroAutoriza').change(function () {
            ActualizarCamposFiltros();
        });
        $('#cbFiltroEnPanelControl').change(function () {
            ActualizarCamposFiltros();
        });
        $('#contentPlacePage_cbFiltroEstado').change(function () {
            ActualizarCamposFiltros();
        });
        $('#btnPersonaAceptar').click(function () {
            var nombre = $('#txtPersonaNombre');
            var email = $('#txtPersonaEmail');
            var usuario = $('#txtPersonaUsuario');
            var idAutoriza = $('#contentPlacePage_cbPersonaAutoriza');
            var enPC = $('#cbPersonaEnPanelControl');
            var estado = $('#contentPlacePage_cbPersonaEstado');
            var cuil = $('#txtPersonaCUIL');
            var legajo = $('#txtPersonaLegajo');
            var horaEntrada = $('#txtPersonaHoraEntrada');
            var horaSalida = $('#txtPersonaHoraSalida');
            var base = $('#cbPersonaBase');
            var result = true;

            result &= TieneDatos(nombre, 'input_wrapper', 'input_wrapper_error');
            result &= TieneDatos(email, 'input_wrapper', 'input_wrapper_error');
            result &= TieneDatos(usuario, 'input_wrapper', 'input_wrapper_error');
            result &= TieneDatos(idAutoriza, 'input_wrapper', 'input_wrapper_selectbox_error') && ContieneValorDiferente(idAutoriza, '<%= Constantes.IdPersonaInvalido.ToString() %>', 'input_wrapper', 'input_wrapper_selectbox_error');
            result &= TieneDatos(enPC, 'input_wrapper', 'input_wrapper_selectbox_error');
            result &= TieneDatos(estado, 'input_wrapper', 'input_wrapper_selectbox_error');
            result &= TieneDatos(cuil, 'input_wrapper', 'input_wrapper_error');
            result &= TieneDatos(legajo, 'input_wrapper', 'input_wrapper_error');
            result &= TieneDatos(horaEntrada, 'input_wrapper', 'input_wrapper_error');
            result &= TieneDatos(horaSalida, 'input_wrapper', 'input_wrapper_error');
            result &= TieneDatos(base, 'input_wrapper', 'input_wrapper_selectbox_error');

            if (isNaN(horaEntrada.val().replace(':', ''))) {
                result &= ContieneNumeros(horaEntrada, 'input_wrapper', 'input_wrapper_error');
            }

            if (isNaN(horaSalida.val().replace(':', ''))) {
                result &= ContieneNumeros(horaSalida, 'input_wrapper', 'input_wrapper_error');
            }

            if (result) {
                ult_ventana = 'divPersona';

                if (current_item == null) {
                    ConsultaAjax.url = 'personalLista.aspx/AltaPersona';
                    ConsultaAjax.data = '{ ';
                }
                else {
                    ConsultaAjax.url = 'personalLista.aspx/EditarPersona';
                    ConsultaAjax.data = '{ "idPersona": "' + current_item + '", ';
                }
                ConsultaAjax.data += '"nombre": "' + nombre.val() + '", "email": "' + email.val() + '", "usuario": "';
                ConsultaAjax.data += usuario.val() + '", "idAutoriza": "' + idAutoriza.val() + '", "enPanelControl": "';
                ConsultaAjax.data += enPC.val() + '", "estado":"' + estado.val() + '", "cuil":"' + cuil.val() + '", ';
                ConsultaAjax.data += '"horaEntrada":"' + horaEntrada.val().replace(':', '') + '", "horaSalida":"';
                ConsultaAjax.data += horaSalida.val().replace(':', '') + '", "baseID":"' + base.val() + '", ';
                ConsultaAjax.data += '"legajo":"' + legajo.val() + '" }';

                ConsultaAjax.AjaxSuccess = function (msg) {
                    CerrarVentana();
                    ReiniciarCamposItem();
                    ult_ventana = null;

                    GetPersonas(1);
                };
                ConsultaAjax.AjaxError = function (msg) {
                    ErrorMsg(msg);
                };

                ConsultaAjax.Ejecutar();
            }
        });
        $('#btnPersonaCancelar').click(function () {
            ReiniciarCamposItem();
            CerrarVentana();
            ult_ventana = null;
        });
        $('#btnAgregarPersona').click(function () {
            $('#titulo_persona').text('Alta de persona');
            $('#txtPersonaEmail').val('@servaind.com');

            MostrarVentana('divPersona');
        });

        ActualizarCamposFiltros();
    });

    function ReiniciarCamposItem() {
        $('#divPersona .input_wrapper').removeClass('input_wrapper_error').removeClass('input_wrapper_selectbox_error');
        $('#divPersona input').val('');
        $('#contentPlacePage_cbPersonaAutoriza').val('<%= Constantes.IdPersonaInvalido.ToString() %>');
        $('#contentPlacePage_cbPersonaAutoriza').change();
        $('#cbPersonaEnPanelControl').val('1');
        $('#cbPersonaEnPanelControl').change();
        $('#contentPlacePage_cbPersonaEstado').val('1');
        $('#contentPlacePage_cbPersonaEstado').change();
        $('#txtPersonaCUIL').removeAttr('readonly').val('');
        $('#txtPersonaHoraEntrada').val('');
        $('#txtPersonaHoraSalida').val('');
        $('#txtPersonaLegajo').val('');

        current_item_pos = null;
        current_item = null;
    }

    function ActualizarCamposFiltros() {
        if ($('#chkFiltroAutoriza').is(':checked')) {
            $('#contentPlacePage_cbFiltroAutoriza').parents('.input_wrapper').removeClass('input_wrapper_selectbox_disabled');
            $('#contentPlacePage_cbFiltroAutoriza').removeAttr('disabled');
        }
        else {
            $('#contentPlacePage_cbFiltroAutoriza').parents('.input_wrapper').addClass('input_wrapper_selectbox_disabled');
            $('#contentPlacePage_cbFiltroAutoriza').attr('disabled', 'disabled');
        }

        if ($('#chkFiltroEnPanelControl').is(':checked')) {
            $('#cbFiltroEnPanelControl').parents('.input_wrapper').removeClass('input_wrapper_selectbox_disabled');
            $('#cbFiltroEnPanelControl').removeAttr('disabled');
        }
        else {
            $('#cbFiltroEnPanelControl').parents('.input_wrapper').addClass('input_wrapper_selectbox_disabled');
            $('#cbFiltroEnPanelControl').attr('disabled', 'disabled');
        }

        if ($('#chkFiltroEstado').is(':checked')) {
            $('#contentPlacePage_cbFiltroEstado').parents('.input_wrapper').removeClass('input_wrapper_selectbox_disabled');
            $('#contentPlacePage_cbFiltroEstado').removeAttr('disabled');
        }
        else {
            $('#contentPlacePage_cbFiltroEstado').parents('.input_wrapper').addClass('input_wrapper_selectbox_disabled');
            $('#contentPlacePage_cbFiltroEstado').attr('disabled', 'disabled');
        }

        GetBases();
        GetPersonas(1);
    }

    function GetPersonas(pagina) {
        MostrarLoading();

        var idPersona = $('#txtFiltroIdPersona').val();
        var nombre = $('#txtFiltroNombre').val();
        var usuario = $('#txtFiltroUsuario').val();
        var idAutoriza = $('#contentPlacePage_cbFiltroAutoriza').val();
        var enPC = $('#cbFiltroEnPanelControl').val();
        var estado = $('#contentPlacePage_cbFiltroEstado').val();

        if (jQuery.trim(idPersona).length == 0 || isNaN(idPersona)) {
            idPersona = '-1';
        }
        if (!$('#chkFiltroAutoriza').is(':checked')) {
            idAutoriza = '-1';
        }
        if (!$('#chkFiltroEnPanelControl').is(':checked')) {
            enPC = '-1';
        }
        if (!$('#chkFiltroEstado').is(':checked')) {
            estado = '-1';
        }

        ConsultaAjax.url = 'personalLista.aspx/GetPersonas';
        ConsultaAjax.data = '{ "pagina":"' + pagina + '", "idPersona":"' + idPersona + '", "nombre": "' + nombre + '", "usuario": "' + usuario + '", "idAutoriza": "' + idAutoriza + '", "enPanelControl": "' + enPC + '", "estado":"' + estado + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            current_page = pagina;

            $('#listado').html();
            var cant = msg.d.length;

            if (cant > 0) {
                var filas = [];
                var i = 0;
                for (i; i < cant; i++) {
                    var fila = '<tr class="fila-color" onclick="EditarPersona(' + msg.d[i][0] + ')">';
                    fila += '<td class="align-center">' + msg.d[i][0] + '</td>';
                    fila += '<td class="align-left">' + msg.d[i][1] + '</td>';
                    fila += '<td class="align-left">' + msg.d[i][2] + '</td>';
                    fila += '<td class="align-left">' + msg.d[i][3] + '</td>';
                    fila += '<td class="align-center"><img src="/images/icons/icon_' + (msg.d[i][4] == 1 ? 'ok.png" title="Si"' : 'delete.png" title="No"') + ' /></td>';
                    fila += '<td class="align-center"><img src="/images/icons/icon_' + (msg.d[i][5] == 1 ? 'ok.png" title="Activa"' : 'delete.png" title="Inactiva"') + ' /></td>';
                    fila += '</tr>';
                    filas.push(fila);
                }

                $('#listado').html(filas.join(''));
            }
            else {
                $('#listado').html('<tr><td colspan="6" class="align-center">No hay personas disponibles.</td></tr>');
            }

            DibujarPaginasLista(idPersona, nombre, usuario, idAutoriza, enPC, estado);

            CerrarVentana();
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function DibujarPaginasLista(idPersona, nombre, usuario, idAutoriza, enPC, estado) {
        ConsultaAjax.url = 'personalLista.aspx/GetCantidadPaginas';
        ConsultaAjax.data = '{ "idPersona":"' + idPersona + '", "nombre": "' + nombre + '", "usuario": "' + usuario + '", "idAutoriza": "' + idAutoriza + '", "enPanelControl": "' + enPC + '", "estado":"' + estado + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            $('#tdPaginas').html();

            var cont = [];

            if (current_page == 1) {
                cont.push('<span class="disabled">«Anterior</span>');
            }
            else {
                cont.push('<a href="#" onclick="GetPersonas(' + (current_page - 1) + ')">«Anterior</a>');
            }
            cont.push('|');
            cont.push('<a href="#" onclick="GetPersonas(1)">Inicio</a>');
            cont.push('|');
            if (msg.d == 0 || current_page == msg.d) {
                cont.push('<span class="disabled">Siguiente»</span>');
            }
            else {
                cont.push('<a href="#" onclick="GetPersonas(' + (current_page + 1) + ')">Siguiente»</a>');
            }

            $('#tdPaginas').html(cont.join(' '));
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function EditarPersona(idPersona) {
        MostrarLoading();

        ConsultaAjax.url = 'personalLista.aspx/GetPersona';
        ConsultaAjax.data = '{ "idPersona": "' + idPersona + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            if (msg.d.length > 0) {
                $('#txtPersonaNombre').val(msg.d[0]);
                $('#txtPersonaEmail').val(msg.d[1]);
                $('#txtPersonaUsuario').val(msg.d[2]);
                $('#contentPlacePage_cbPersonaAutoriza').val(msg.d[3]);
                $('#contentPlacePage_cbPersonaAutoriza').change();
                $('#cbPersonaEnPanelControl').val(msg.d[4]);
                $('#cbPersonaEnPanelControl').change();
                $('#contentPlacePage_cbPersonaEstado').val(msg.d[5]);
                $('#contentPlacePage_cbPersonaEstado').change();
                $('#txtPersonaCUIL').val(msg.d[6]).attr('readonly', 'readonly');
                $('#txtPersonaHoraEntrada').val(msg.d[7]);
                $('#txtPersonaHoraSalida').val(msg.d[8]);
                $('#cbPersonaBase').val(msg.d[9]);
                $('#cbPersonaBase').change();
                $('#txtPersonaLegajo').val(msg.d[10]);

                $('#titulo_persona').text('Editar persona');
                MostrarVentana('divPersona');
                current_item = idPersona;
            } else {
                CerrarVentana();
            }
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function GetBases() {
        ConsultaAjax.url = 'personalLista.aspx/GetBases';
        ConsultaAjax.AjaxSuccess = function (msg) {
            var c = msg.d.length;

            var bases = [];
            for (var i = 0; i < c; i++) {
                bases.push('<option value="' + msg.d[i].BaseID + '">' + msg.d[i].Nombre + '</option>');
            }

            $('#cbPersonaBase').html(bases.join(''));
            $('#cbPersonaBase').change();
        };
        ConsultaAjax.AjaxError = function (msg) {
            
        };

        ConsultaAjax.Ejecutar();
    }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlacePage" Runat="Server">

<div class="page-title">
    <h1>Administración de personal</h1>
</div>

<div class="full-width">
    <table class="tbl editable listado" cellspacing="0">
        <thead>
            <tr>
                <td class="border_middle" style="width:70px">ID</td>
                <td class="border_middle" style="width:200px">Nombre</td>
                <td class="border_middle" style="width:200px">Usuario</td>
                <td class="border_middle" style="width:200px"><input id="chkFiltroAutoriza" type="checkbox" /> Autoriza</td>
                <td class="border_middle" style="width:100px"><input id="chkFiltroEnPanelControl" type="checkbox" /> En PC</td>
                <td class="border_right"><input id="chkFiltroEstado" type="checkbox" /> Estado</td>
            </tr>
            <tr class="filter_row">
                <td>
                    <div class="input_wrapper">
                        <div class="cap">
                            <input id="txtFiltroIdPersona" type="text"/>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="input_wrapper">
                        <div class="cap">
                            <input id="txtFiltroNombre" type="text"/>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="input_wrapper">
                        <div class="cap">
                            <input id="txtFiltroUsuario" type="text"/>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="input_wrapper input_wrapper_selectbox">
                        <div class="cap">
                            <span id="lblFiltroAutoriza" runat="server"></span>
                            <select id="cbFiltroAutoriza" runat="server"></select>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="input_wrapper input_wrapper_selectbox">
                        <div class="cap">
                            <span id="lblFiltroEnPanelControl"></span>
                            <select id="cbFiltroEnPanelControl">
                                <option value="1">Si</option>
                                <option value="0">No</option>
                            </select>
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
                <td colspan="6" class="align-center">No hay personas disponibles.</td>
            </tr>
        </tbody>
        <tfoot>
            <tr>
                <td colspan="2"><a id="btnAgregarPersona">Agregar</a></td>
                <td colspan="4" class="align-center" id="tdPaginas">
                &nbsp;
                </td>
            </tr>
        </tfoot>
    </table>
</div>

<div class="dialog_wrapper" style="width:500px" id="divPersona">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3 id="titulo_persona"></h3></div></div></div>
    </div>

    <div class="dialog_content">
        <ul class="middle_form">
            <li class="form_floated_item form_floated_item_100">
                <label class="label" for="txtPersonaNombre">Nombre</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtPersonaNombre" maxlength="60" value="" type="text"/>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_100">
                <label class="label" for="txtPersonaUsuario">E-mail</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtPersonaEmail" maxlength="60" value="" type="text"/>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_50">
                <label class="label" for="txtPersonaUsuario">Usuario</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtPersonaUsuario" maxlength="60" value="" type="text"/>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_50 form_floated_item_right" id="li1">
                <label class="label" for="txtFechaDesde">CUIL</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtPersonaCUIL" value="" maxlength="11" type="text" />
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_50" id="li2">
                <label class="label" for="txtFechaDesde">Legajo</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtPersonaLegajo" value="" maxlength="5" type="text" />
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_50 form_floated_item_right">
                <label class="label" for="cbPersonaAutoriza">Autoriza</label>
                <div class="input_wrapper input_wrapper_selectbox">
                    <div class="cap">
                        <span id="lblPersonaAutoriza"></span>
                        <select id="cbPersonaAutoriza" runat="server"></select>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_50">
                <label class="label" for="cbPersonaEnPanelControl">Visible en panel de control</label>
                <div class="input_wrapper input_wrapper_selectbox">
                    <div class="cap">
                        <span id="lblPersonaEnPanelControl"></span>
                        <select id="cbPersonaEnPanelControl">
                            <option value="1">Si</option>
                            <option value="0">No</option>
                        </select>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_50 form_floated_item_right">
                <label class="label" for="cbPersonaEstado">Estado</label>
                <div class="input_wrapper input_wrapper_selectbox">
                    <div class="cap">
                        <span id="lblPersonaEstado"></span>
                        <select id="cbPersonaEstado" runat="server"></select>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_50" id="liHoraEntrada">
                <label class="label" for="txtFechaDesde">Hora de entrada</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtPersonaHoraEntrada" value="" maxlength="5" type="text" />
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_50 form_floated_item_right" id="liHoraSalida">
                <label class="label" for="txtFechaDesde">Hora de salida</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtPersonaHoraSalida" value="" maxlength="5" type="text" />
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_50">
                <label class="label" for="txtDepartamento">Base</label>
                <div class="input_wrapper input_wrapper_selectbox">
                    <div class="cap">
                        <span id="lblPersonaBase"></span>
                        <select id="cbPersonaBase">
                        </select>
                    </div>
                </div>
            </li>
        </ul>
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li><div class="btn primary_action_button_small button_100" id="btnPersonaAceptar"><div class="cap"><span>Aceptar</span></div></div></li>
            <li><div class="btn secondary_action_button_small button_100" id="btnPersonaCancelar"><div class="cap"><span>Cancelar</span></div></div></li>
        </ul>
    </div>
</div>

</asp:Content>

