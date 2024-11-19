<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="propCambio.aspx.cs" Inherits="calidad_propCambio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">
    <script type="text/javascript">
        var fileSelected;

        $(document).ready(function() {
            $('#btnCancelar').click(DiscardAlert);
            $('#btnEnviar').click(SendAlert);

            $('#frameAux').attr('src', '/calidad/propCambioUpload.aspx');
            FileSelected(null);
        });

        function SelectFile() {
            $('#frameAux').contents().find("#txtArchivo").click();
        }

        function FileSelected(f) {
            if (fileSelected = (jQuery.trim(f).length > 0)) $('#file-container').html('<div class="tag_element green hand" onclick="SelectFile()"><span>Archivo seleccionado</span></div>');
            else $('#file-container').html('<ul class="rda_actions"><li class="add" id="btnSubirArchivo" onclick="SelectFile()">Click aquí para subir un archivo</li></ul>');
        }

        function SendSuccess() {
            custom_dialog.close();
            Mensaje('La propuesta de cambio fue enviada.', 'success', true, false, 'Aceptar', '', 'Discard()', '');
        }

        function Discard() {
            location.href = '/calidad/sgim.aspx';
        }

        function SendAlert() {
            Mensaje('¿Desea confirmar el envío de la propuesta?', 'warning', true, true, 'Enviar', 'Cancelar', 'Send()', 'custom_dialog.close()');
        }

        function DiscardAlert() {
            Mensaje('Se descartarán los datos ingresados. ¿Desea continuar?', 'warning', true, true, 'Cancelar', 'Continuar', 'custom_dialog.close()', 'Discard()');
        }

        function Send() {
            document.getElementById('frameAux').contentWindow.Send(
                $('#cbSector').val(),
                $('#cbResponsable').val(),
                $('#txtCambio').val(),
                $('#cbUrgencia').val()
            );

            MostrarLoading();
        }

        function SendError(msg) {
            ErrorMsg(msg + (fileSelected ? '<br/>Por favor, vuelva a cargar el archivo adjunto.' : ''));
            FileSelected(null);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlacePage" Runat="Server">
    <div class="page-title">
        <h1>Propuesta de cambio</h1>
    </div>

    <div class="form_place">
        <ul class="middle_form">
            <li class="form_floated_item form_floated_item_half">
                <label class="label" for="cbSector">Sector</label>
                <div class="input_wrapper input_wrapper_selectbox">
                    <div class="cap">
                        <span id="lblSector"></span>
                        <select id="cbSector">
                            <% List<Area> areas = GAreas.GetAreas();
                               areas.ForEach(v =>
                                    {
                                        %>
                                        <option value="<%=v.ID %>"><%=v.Descripcion %></option>
                                        <%
                                    });
                                %>
                        </select>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_half form_floated_item_right">
                <label class="label" for="cbResponsable">Responsable</label>
                <div class="input_wrapper input_wrapper_selectbox">
                    <div class="cap">
                        <span id="lblResponsable"></span>
                        <select id="cbResponsable">
                            <% List<Persona> responsables = GPersonal.GetPersonas(true);
                               responsables.ForEach(v =>
                                    {
                                        %>
                                        <option <%=v.ID == Constantes.Usuario.ID ? "selected=\"selected\"" : "" %> value="<%=v.ID %>"><%=v.Nombre %></option>
                                        <%
                                    });
                                %>
                        </select>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_full">
                <label class="label" for="txtCambio">Cambio propuesto (máx. 3000 caracteres)</label>
                <div class="textarea_wrapper clear"> 
	                <div class="top">
		                <div class="cap"></div>
	                </div>
	                <div class="inner">
		                <div class="cap">
			                <textarea id="txtCambio" maxlength="3000" onkeyup="return MaxLength(this)" rows="5" cols="20"></textarea>
		                </div>
	                </div>
	                <div class="bottom">
		                <div class="cap"></div>
	                </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_half">
                <label class="label" for="cbUrgencia">Urgencia</label>
                <div class="input_wrapper input_wrapper_selectbox">
                    <div class="cap">
                        <span id="lblUrgencia"></span>
                        <select id="cbUrgencia">
                            <% List<DataSourceItem> urgencias = EnumExtensions.EnumToList<PropCambioUrgencia>();
                               urgencias.ForEach(v =>
                                    {
                                        %>
                                        <option value="<%=v.ValueField %>"><%=v.TextField %></option>
                                        <%
                                    });
                                %>
                        </select>
                    </div>
                </div>
            </li>
            <li class="form_floated_item form_floated_item_half form_floated_item_right">
                <label class="label" for="txtFile">Adjuntar archivo</label>
                <div id="file-container">

                </div>
            </li>
        </ul>
        <div class="clear"></div>

        <div class="form_buttons_container">
            <ul class="button_list">
                <li id="btnCancelar"><div class="btn secondary_action_button button_100"><div class="cap"><span>Volver</span></div></div></li>
                <li id="btnEnviar"><div class="btn primary_action_button button_100"><div class="cap"><span>Enviar</span></div></div></li>
            </ul>
        </div>
    </div>
</asp:Content>

