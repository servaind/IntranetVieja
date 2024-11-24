<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="basesLista.aspx.cs" Inherits="sistemas_basesLista" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">
    <script language="javascript" type="text/javascript">
        var currentID;

        function GetBases() {
            MostrarLoading();

            ConsultaAjax.url = 'basesLista.aspx/GetBases';
            ConsultaAjax.AjaxSuccess = function (msg) {
                $('#listado').html();
                var cantBases = msg.d.length;

                if (cantBases > 0) {
                    var filas = [];
                    for (var i = 0; i < cantBases; i++) {
                        var fila = '<tr class="fila-color" onclick="GetBase(' + msg.d[i].ID + ');">';
                        fila += '<td class="align-center">' + msg.d[i].ID + '</td>';
                        fila += '<td class="align-left">' + msg.d[i].Nombre + '</td>';
                        fila += '<td class="align-center">' + msg.d[i].Responsable + '</td>';
                        fila += '<td class="align-center">' + msg.d[i].Alternate + '</td>';
                        fila += '</tr>';
                        filas.push(fila);
                    }

                    $('#listado').html(filas.join(''));
                } else {
                    $('#listado').html('<tr><td colspan="8" class="align-center">No hay datos disponibles.</td></tr>');
                }

                CerrarVentana();
            };
            ConsultaAjax.AjaxError = function (msg) {
                ErrorMsg(msg);
            };

            ConsultaAjax.Ejecutar();
        }

        function GetResponsables() {
            ConsultaAjax.url = 'basesLista.aspx/GetResponsables';
            ConsultaAjax.AjaxSuccess = function (msg) {
                var cantPersonal = msg.d.length;

                if (cantPersonal > 0) {
                    var filas = [];
                    for (var i = 0; i < cantPersonal; i++) {
                        var fila = '<option value="' + msg.d[i].ID + '">' + msg.d[i].Nombre + '</option>';
                        filas.push(fila);
                    }

                    $('#cbResponsables').html(filas.join('')).change();
                    $('#cbAlternates').html(filas.join('')).change();
                }
            };
            ConsultaAjax.AjaxError = function (msg) {
                ErrorMsg(msg);
            };

            ConsultaAjax.Ejecutar();
        }

        function GetBase(baseID) {
            ConsultaAjax.url = 'basesLista.aspx/GetBase';
            ConsultaAjax.data = '{ "baseID":"' + baseID + '" }',
            ConsultaAjax.AjaxSuccess = function (msg) {
                if (msg.d != null) {
                    currentID = baseID;
                    $('#txtNombre').val(msg.d.Nombre);
                    $('#cbResponsables').val(msg.d.ResponsableID).change();
                    $('#cbAlternates').val(msg.d.AlternateID).change();
                    $('#cbEstado').val(msg.d.Activa ? 1 : 0).change();

                    $('#btnBaseEliminar').show();
                    $('#dlgBaseTitulo').text('Editar base');
                    $('#liEstado').show();
                    MostrarVentana('dlgBase');
                    $('#txtNombre').focus();
                }
            };
            ConsultaAjax.AjaxError = function (msg) {
                ErrorMsg(msg);
            };

            ConsultaAjax.Ejecutar();
        }

        function LimpiarBase() {
            currentID = null;
            $('#txtNombre').val('');
            $('#btnBaseEliminar').hide();
            $('#liEstado').hide();
        }

        function GuardarBase() {
            var nombre = $('#txtNombre');
            var responsable = $('#cbResponsables');
            var alternate = $('#cbAlternates');
            var result = true;

            result &= TieneDatos(nombre, 'input_wrapper', 'input_wrapper_error');
            result &= TieneDatos(responsable, 'input_wrapper', 'input_wrapper_selectbox_error') && ContieneValorDiferente(responsable, '<%= Constantes.IdPersonaInvalido.ToString() %>', 'input_wrapper', 'input_wrapper_selectbox_error');
            result &= TieneDatos(alternate, 'input_wrapper', 'input_wrapper_selectbox_error') && ContieneValorDiferente(alternate, '<%= Constantes.IdPersonaInvalido.ToString() %>', 'input_wrapper', 'input_wrapper_selectbox_error');

            if (result) {
                ult_ventana = 'dlgBase';

                if (currentID == null) {
                    ConsultaAjax.url = 'basesLista.aspx/AddBase';
                    ConsultaAjax.data = '{ ';
                }
                else {
                    ConsultaAjax.url = 'basesLista.aspx/UpdateBase';
                    ConsultaAjax.data = '{ "baseID": "' + currentID + '", "activa":"' + $('#cbEstado').val() + '", ';
                }
                ConsultaAjax.data += '"nombre": "' + nombre.val() + '", "responsableID": "' + responsable.val() + '", ';
                ConsultaAjax.data += '"alternateID":"' + alternate.val() + '" }';

                ConsultaAjax.AjaxSuccess = function (msg) {
                    CerrarVentana();
                    LimpiarBase();
                    ult_ventana = null;

                    GetBases();
                };
                ConsultaAjax.AjaxError = function (msg) {
                    ErrorMsg(msg);
                };

                ConsultaAjax.Ejecutar();
            }
        }

        function EliminarBase() {
            ConsultaAjax.url = 'basesLista.aspx/DeleteBase';
            ConsultaAjax.data = '{ "baseID":"' + currentID + '" }',
            ConsultaAjax.AjaxSuccess = function (msg) {
                CerrarVentana();
                LimpiarBase();
                ult_ventana = null;

                GetBases();
            };
            ConsultaAjax.AjaxError = function (msg) {
                ErrorMsg(msg);
            };

            ConsultaAjax.Ejecutar();
        }

        function OnCancelarEliminar() {
            CerrarVentana();
            MostrarVentana('dlgBase');
        }

        $(document).ready(function () {
            $('#btnAgregarBase').click(function () {
                LimpiarBase();

                $('#dlgBaseTitulo').text('Agregar base');
                MostrarVentana('dlgBase');
                $('#txtNombre').focus();
            });

            $('#btnBaseGuardar').click(function () {
                GuardarBase();
            });

            $('#btnBaseEliminar').click(function () {
                CerrarVentana();
                Mensaje('¿Desea confirmar la eliminación de la base?', 'warning', true, true, 'Eliminar', 'Cancelar', 'EliminarBase()', 'OnCancelarEliminar()');
            });

            $('#btnBaseCancelar').click(function () {
                LimpiarBase();
                CerrarVentana();
            });

            GetResponsables();
            GetBases();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlacePage" Runat="Server">
    <div class="page-title">
        <h1>Bases -> Listado</h1>
    </div>

    <div class="form_place">
        <table class="tbl editable listado" cellspacing="0">
            <thead>
                <tr>
                    <td class="border_left" style="width:10%">ID</td>
                    <td class="border_middle" style="width:30%">Nombre</td>
                    <td class="border_middle" style="width:30%">Responsable</td>
                    <td class="border_right" style="width:30%">Alternate</td>
                </tr>
            </thead>
            <tbody id="listado">
                <tr>
                    <td colspan="8" class="align-center">No hay datos disponibles.</td>
                </tr>
            </tbody>
            <tfoot>
                <tr>
                    <td colspan="2"><a id="btnAgregarBase">Agregar</a></td>
                    <td colspan="4" class="align-center" id="tdPaginas">
                    &nbsp;
                    </td>
                </tr>
            </tfoot>
        </table>
    </div>

    <div class="dialog_wrapper" style="width:500px" id="dlgBase">
        <div class="dialog_header">
            <div class="cap right"><div class="cap left"><div class="cap inner"><h3 id="dlgBaseTitulo"></h3></div></div></div>
        </div>

        <div class="dialog_content">
            <ul class="middle_form">
                <li class="form_floated_item form_floated_item_50">
                    <label class="label" for="txtFechaDesde">Nombre</label>
                    <div class="input_wrapper">
                        <div class="cap">
                            <input id="txtNombre" value="" maxlength="30" type="text" />
                        </div>
                    </div>
                </li>
                <li class="form_floated_item form_floated_item_50 form_floated_item_right">
                    <label class="label" for="txtDepartamento">Responsable</label>
                    <div class="input_wrapper input_wrapper_selectbox">
                        <div class="cap">
                            <span id="lblResponsables"></span>
                            <select id="cbResponsables">
                            </select>
                        </div>
                    </div>
                </li>
                <li class="form_floated_item form_floated_item_50">
                    <label class="label" for="txtDepartamento">Alternate</label>
                    <div class="input_wrapper input_wrapper_selectbox">
                        <div class="cap">
                            <span id="lblAlternates"></span>
                            <select id="cbAlternates">
                            </select>
                        </div>
                    </div>
                </li>
                <li class="form_floated_item form_floated_item_50 form_floated_item_right" id="liEstado">
                    <label class="label" for="txtDepartamento">Estado</label>
                    <div class="input_wrapper input_wrapper_selectbox">
                        <div class="cap">
                            <span id="lblEstado"></span>
                            <select id="cbEstado">
                                <option value="1">Activa</option>
                                <option value="0">Inactiva</option>
                            </select>
                        </div>
                    </div>
                </li>
            </ul>
        </div>

        <div class="dialog_footer">
            <ul class="button_list">
                <li><div class="btn primary_action_button_small button_100" id="btnBaseGuardar"><div class="cap"><span>Guardar</span></div></div></li>
                <li><div class="btn secondary_action_button_small button_100" id="btnBaseCancelar"><div class="cap"><span>Cancelar</span></div></div></li>
                <li><div class="btn secondary_action_button_small button_100" id="btnBaseEliminar"><div class="cap"><span>Eliminar</span></div></div></li>
            </ul>
        </div>
    </div>
</asp:Content>