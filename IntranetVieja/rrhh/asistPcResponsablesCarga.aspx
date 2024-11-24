<%@ Page Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="asistPcResponsablesCarga.aspx.cs" Inherits="rrhh_asistPcResponsablesCarga" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $('#contentPlacePage_cbPersona').change(function () {
                var responsableId = $(this).val();

                if (responsableId == '<%= Constantes.IdPersonaInvalido.ToString() %>') {
                    ActualizarCheckboxes(true);
                    return;
                }

                MostrarLoading();

                ConsultaAjax.url = 'asistPcResponsablesCarga.aspx/GetPersonasResponsable';
                ConsultaAjax.data = '{ "responsableId":"' + responsableId + '" }';
                ConsultaAjax.AjaxSuccess = function (msg) {
                    var cant = msg.d.length;

                    LimpiarCheckboxes();

                    if (cant > 0) {
                        for (var i = 0; i < cant; i++) {
                            $('#chkPersona_' + msg.d[i]).attr('checked', 'checked');
                        }
                    }

                    ActualizarCheckboxes(false);

                    CerrarVentana();
                };
                ConsultaAjax.AjaxError = function (msg) {
                    ErrorMsg(msg);
                };
                ConsultaAjax.Ejecutar();
            });

            $('#btnGuardar').click(function () {
                var responsableId = $('#contentPlacePage_cbPersona').val();
                var personas = [];

                if (responsableId == null || responsableId == '<%= Constantes.IdPersonaInvalido.ToString() %>') {
                    ActualizarCheckboxes(true);
                    return;
                }

                var seleccionados = $('input[type="checkbox"]:checked');
                var cant = seleccionados.length;
                for (var i = 0; i < cant; i++) {
                    var personaId = $(seleccionados[i]).attr('id').replace('chkPersona_', '');

                    personas.push(parseInt(personaId));
                }

                MostrarLoading();

                ConsultaAjax.url = 'asistPcResponsablesCarga.aspx/UpdatePcResponsable';
                ConsultaAjax.data = JSON.stringify({ responsableId: parseInt(responsableId), personas: personas });
                ConsultaAjax.AjaxSuccess = function (msg) {
                    Mensaje('Los cambios se aplicaron correctamente.', 'success', true, false, 'Aceptar', '', 'custom_dialog.close()', '');
                };
                ConsultaAjax.AjaxError = function (msg) {
                    ErrorMsg(msg);
                };
                ConsultaAjax.Ejecutar();
            });

            $('#contentPlacePage_cbPersona').change();
        });

        function ActualizarCheckboxes(bloquear) {
            if (bloquear) {
                $('input[type="checkbox"]').attr('disabled', 'disabled');
                LimpiarCheckboxes();
                $('#action-buttons').hide();
            }
            else {
                $('input[type="checkbox"]').removeAttr('disabled');
                $('#action-buttons').show();
            }
        }

        function LimpiarCheckboxes() {
            $('input[type="checkbox"]').removeAttr('checked');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlacePage" Runat="Server">

<div class="page-title">
    <h1>Asistencia -> Administración de responsables</h1>
</div>

<div class="form_place">
    <ul class="middle_form" style="height:60px">
        <li class="form_floated_item form_floated_item_half">
            <label class="label" for="cbPersona">Persona</label>
            <div class="input_wrapper input_wrapper_selectbox">
                <div class="cap">
                    <span id="lblPersona"></span>
                    <select id="cbPersona" runat="server"></select>
                </div>
            </div>
        </li>
    </ul>

    <table class="tbl listado full_width" cellspacing="0">
        <thead>
            <tr>
                <td class="border_left" style="width:100px"></td>
                <td class="border_right">Personal</td>
            </tr>
        </thead>
        <tbody id="listado-personas">
            <% List<Persona> personas = GPersonal.GetPersonasActivas();
               foreach (Persona p in personas) { %>
                <tr>
                    <td class="align-center"><input type="checkbox" id="chkPersona_<%=p.ID %>" /></td>
                    <td class="align-left"><%=p.Nombre %></td>
                </tr>
            <% } %>
        </tbody>
        <tfoot>
            <tr>
                <td class="align-center" colspan="3" id="tdNavegacion">
                    
                </td>
            </tr>
        </tfoot>
    </table>

    <div id="action-buttons" class="form_buttons_container">
        <ul class="button_list">
            <li id="btnGuardar"><div class="btn primary_action_button button_100"><div class="cap"><span>Guardar</span></div></div></li>
        </ul>
    </div>
</div>

</asp:Content>
