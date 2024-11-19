<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="ncAdmin.aspx.cs" Inherits="calidad_ncAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">

<script language="javascript" type="text/javascript">

    $(document).ready(function () {
        <% if (this.NC != null) { %>
        $('#contentPlacePage_cbAreaResponsabilidad').change();
        $('#contentPlacePage_cbCategoria').change();
        $('#contentPlacePage_cbCierre').change();
        <% } %>
        
        <% if (this.NC == null) { %>
        $('#btnEnviar').click(function(){
            var equipo = $('#contentPlacePage_cbEquipo option:selected').text();
            var asunto = $('#contentPlacePage_txtAsunto');
            var hallazgo = $('#contentPlacePage_txtHallazgo');
            var accionInmediata = $('#contentPlacePage_txtAccionInmediata');
            var comentarios = $('#contentPlacePage_txtComentarios');
            var result = true;

            result &= TieneDatos(asunto, 'input_wrapper', 'input_wrapper_error');
            result &= TieneDatos(hallazgo, 'textarea_wrapper', 'textarea_wrapper_error');
            result &= TieneDatos(accionInmediata, 'textarea_wrapper', 'textarea_wrapper_error');

            if (result) {
                MostrarLoading();

                ConsultaAjax.url = 'ncAdmin.aspx/GenerarNoConformidad';
                ConsultaAjax.data = JSON.stringify({ asunto: asunto.val() , equipo: equipo, hallazgo: hallazgo.val() , accionInmediata: accionInmediata.val() , comentarios: comentarios.val() });
                ConsultaAjax.AjaxSuccess = function (msg) {
                    Mensaje(msg.d, 'success', true, false, 'Aceptar', '', 'OnCerrar()', '');                    
                };
                ConsultaAjax.AjaxError = function (msg) {
                    ErrorMsg(msg);
                };

                ConsultaAjax.Ejecutar();
            }
        });
        <% } %>

        <% if (PuedeAdministrador || PuedeImputado) { %>
        $('#btnGuardar').click(function(){
            <% if(PuedeAdministrador) { %>
            GuardarSGC(false);
            <% } %>
            <% else { %>
            GuardarImputado();
            <% } %>
        });
        <% } %>

        <% if (PuedeAdministrador) { %>
        $('#btnCerrar').click(function(){
            Mensaje('Una vez cerrada la solicitud, no podrá ser editada nuevamente. ¿Desea continuar?', 'warning', true, true, 'Cancelar', 'Aceptar', 'custom_dialog.close()', 'OnConfirmarCierreNC()');
        });
        $('#contentPlacePage_chkNormaOHSAS18001').click(function(){
            if($('#contentPlacePage_chkNormaOHSAS18001').is(':checked')) {
                Mensaje('¿Requiere revisión de la Matriz de Riesgos?', 'warning', true, true, 'Si', 'No', 'OnRevMatrizRiesgo(1)', 'OnRevMatrizRiesgo(0)');
            }
            else {
                OnRevMatrizRiesgo(0);
                $('#contentPlacePage_lblRevMatrizRiesgo').hide();
            }
        });
        <% } %>

        <% if (this.NC == null || this.NC.Estado != EstadosNC.Cerrada) { %>
        $('#btnCancelar').click(function () {
            Mensaje('Todos los datos ingresados serán descartados, ¿desea continuar?', 'warning', true, true, 'Cancelar', 'Aceptar', 'custom_dialog.close()', 'OnCancel()');
        });
        <% } %>

        <% if (this.NC != null && (this.NC.Estado == EstadosNC.Cerrada || this.NC.Estado == EstadosNC.NoCorresponde || this.NC.Estado == EstadosNC.ProcesandoSGI)) { %>
        $('#btnImprimir').click(function () {
            window.open('<%= Encriptacion.GetURLEncriptada("calidad/ncImprimir.aspx", "id=" + this.NC.ID) %>', "Imprimir", "status=0, toolbar=0, resizable=0, "
                      + "menubar=0, directories=0, width='680', height:'500', scrollbars=1");
        });
        <% } %>
    });

    <% if (PuedeAdministrador) { %>
    function OnConfirmarCierreNC(){
        custom_dialog.close();
        GuardarSGC(true);
    }
    var revMatrizRiesgo = <%=NC.RevMatrizRiesgo ? "1" : "0" %>;
    function OnRevMatrizRiesgo(v) {
        revMatrizRiesgo = v;

        if (v==1) {
            $('#contentPlacePage_lblRevMatrizRiesgo').text('Requiere revisión de la Matriz de Riesgos');
        }
        else {
            $('#contentPlacePage_lblRevMatrizRiesgo').text('No requiere revisión de la Matriz de Riesgos');
        }

        $('#contentPlacePage_lblRevMatrizRiesgo').show();

        custom_dialog.close()
    }
    <% } %>

    function OnCancel(){
        location.href = '/calidad/ncLista.aspx';
    }

    function OnCerrar(){
        location.href = '/calidad/ncLista.aspx';
    }

    function OnActualizar(){
        location.reload(true);
    }

    <% if(PuedeImputado) { %>
    function GuardarImputado(){
        var accionInmediata = $('#contentPlacePage_txtAccionInmediata');
        var causasRaices = $('#contentPlacePage_txtCausasRaices');
        var accionCorrectiva = $('#contentPlacePage_txtAccionCorrectiva');

        var result = true;

        result &= TieneDatos(accionInmediata, 'textarea_wrapper', 'textarea_wrapper_error');
        result &= TieneDatos(causasRaices, 'textarea_wrapper', 'textarea_wrapper_error');
        result &= TieneDatos(accionCorrectiva, 'textarea_wrapper', 'textarea_wrapper_error');

        if (result) {
            MostrarLoading();

            ConsultaAjax.url = 'ncAdmin.aspx/GuardarNCImputado';
            ConsultaAjax.data = JSON.stringify({ accionInmediata: accionInmediata.val(), causasRaices: causasRaices.val(), accionCorrectiva: accionCorrectiva.val() });
            ConsultaAjax.AjaxSuccess = function (msg) {
                Mensaje(msg.d, 'success', true, false, 'Aceptar', '', 'OnCerrar()', '');                    
            };
            ConsultaAjax.AjaxError = function (msg) {
                ErrorMsg(msg);
            };

            ConsultaAjax.Ejecutar();
        }
    }
    <% } %>

    <% if(PuedeAdministrador) { %>
    function GuardarSGC(cerrar){
        var normaISO9001 = $('#contentPlacePage_chkNormaISO9001');
        var normaISO14001 = $('#contentPlacePage_chkNormaISO14001');
        var normaOHSAS18001 = $('#contentPlacePage_chkNormaOHSAS18001');
        var normaIRAM301 = $('#contentPlacePage_chkNormaIRAM301');
        var area = $('#contentPlacePage_cbAreaResponsabilidad');
        var apartado = $('#contentPlacePage_txtApartado');
        var categoria = $('#contentPlacePage_cbCategoria');
        var equipo = $('#contentPlacePage_cbEquipo option:selected').text();
        var asunto = $('#contentPlacePage_txtAsunto');
        var hallazgo = $('#contentPlacePage_txtHallazgo');
        var accionInmediata = $('#contentPlacePage_txtAccionInmediata');
        var causasRaices = $('#contentPlacePage_txtCausasRaices');
        var accionCorrectiva = $('#contentPlacePage_txtAccionCorrectiva');
        var cierre = $('#contentPlacePage_cbCierre');
        var comentarios = $('#contentPlacePage_txtComentarios');

        var result = true;

        result &= TieneDatos(area, 'input_wrapper', 'input_wrapper_selectbox_error');
        result &= TieneDatos(apartado, 'input_wrapper', 'input_wrapper_error');
        result &= TieneDatos(categoria, 'input_wrapper', 'input_wrapper_selectbox_error');
        //result &= TieneDatos(equipo, 'input_wrapper', 'input_wrapper_error');
        result &= TieneDatos(asunto, 'input_wrapper', 'input_wrapper_error');
        result &= TieneDatos(accionInmediata, 'textarea_wrapper', 'textarea_wrapper_error');
        result &= TieneDatos(cierre, 'input_wrapper', 'input_wrapper_selectbox_error');

        if (result) {
            MostrarLoading();

            ConsultaAjax.url = 'ncAdmin.aspx/' + (cerrar ? 'CerrarNC' : 'GuardarNCSGC');
            ConsultaAjax.data = JSON.stringify({ normaISO9001: normaISO9001.is(':checked') ? 1 : 0, normaISO14001: normaISO14001.is(':checked') ? 1 : 0, normaOHSAS18001: normaOHSAS18001.is(':checked') ? 1 : 0, normaIRAM301: normaIRAM301.is(':checked') ? 1 : 0, revisionMatrizRiesgo: revMatrizRiesgo, idArea: area.val(), apartado: apartado.val(), categoria: categoria.val(), equipo: equipo, asunto: asunto.val(), hallazgo: hallazgo.val(), accionInmediata: accionInmediata.val(), causasRaices: causasRaices.val(), accionCorrectiva: accionCorrectiva.val(), conclusion: cierre.val(), comentarios: comentarios.val() });
            ConsultaAjax.AjaxSuccess = function (msg) {
                Mensaje(msg.d, 'success', true, false, 'Aceptar', '', cerrar ? 'OnActualizar()' : 'OnCerrar()', '');                    
            };
            ConsultaAjax.AjaxError = function (msg) {
                ErrorMsg(msg);
            };

            ConsultaAjax.Ejecutar();
        }
    }
    <% } %>
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlacePage" Runat="Server">


<div class="page-title">
    <h1>Solicitud de NC / Observación / Oportunidad de mejora</h1>
</div>

<div class="form-steps" style="width:850px">
    <ul>
        <li>
            <div class="step-number<%= this.NC == null ? " active" : "" %>">1.</div>
            <div class="step-description<%= this.NC == null ? " active" : "" %>">Solicitud</div>
        </li>
        <li class="separator"></li>
        <li>
            <div class="step-number<%= this.NC != null && this.NC.Estado == EstadosNC.ProcesandoSGI ? " active" : "" %>">2.</div>
            <div class="step-description<%= this.NC != null && this.NC.Estado == EstadosNC.ProcesandoSGI ? " active" : "" %>">Procesando SGC</div>
        </li>
        <li class="separator"></li>
        <li>
            <div class="step-number<%= this.NC != null && this.NC.Estado == EstadosNC.ProcesandoImputado ? " active" : "" %>">3.</div>
            <div class="step-description<%= this.NC != null && this.NC.Estado == EstadosNC.ProcesandoImputado ? " active" : "" %>">Procesando Responsable</div>
        </li>
        <li class="separator"></li>
        <li>
            <div class="step-number<%= this.NC != null && this.NC.Estado == EstadosNC.EsperandoCierre ? " active" : "" %>">4.</div>
            <div class="step-description<%= this.NC != null && this.NC.Estado == EstadosNC.EsperandoCierre ? " active" : "" %>">Esperando cierre</div>
        </li>
        <li class="separator"></li>
        <li>
            <div class="step-number<%= this.NC != null && (this.NC.Estado == EstadosNC.Cerrada || this.NC.Estado == EstadosNC.NoCorresponde) ? " active" : "" %>">5.</div>
            <div class="step-description<%= this.NC != null && (this.NC.Estado == EstadosNC.Cerrada || this.NC.Estado == EstadosNC.NoCorresponde) ? " active" : "" %>">Cerrada</div>
        </li>
    </ul>    
</div>

<div class="form_place">
    <h3>Datos de la solicitud</h3>

    <p>Los campos marcados con <span class="required"></span> son obligatorios.</p>
    <br />

    <ul class="middle_form" style="height:<%= this.NC != null ? "1100" : "400" %>px">
        <% if (this.NC != null) { %>
        <li class="form_floated_item form_floated_item_half">
            <label class="label">Número</label>
            <span id="lblNumero" runat="server"></span>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label">Fecha</label>
            <span id="lblFecha" runat="server"></span>
        </li>
        <li class="form_floated_item form_floated_item_100">
            <label class="label required" for="txtNorma">Norma</label>
            <ul class="chkList" style="width:400px">
                <li style="width:200px"><input id="chkNormaISO9001" type="checkbox" runat="server" />ISO 9001</li>
                <li style="width:200px"><input id="chkNormaISO14001" type="checkbox" runat="server" />ISO 14001</li>
                <li style="width:200px"><input id="chkNormaOHSAS18001" type="checkbox" runat="server" />OHSAS 18001</li>
                <li style="width:200px"><input id="chkNormaIRAM301" type="checkbox" runat="server" />IRAM 301</li>
            </ul>
            <span id="lblRevMatrizRiesgo" runat="server">Requiere revisión de la Matriz de Riesgos</span>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label required" for="cbAreaResponsabilidad">Área de responsabilidad</label>
            <div class="input_wrapper input_wrapper_selectbox">
                <div class="cap">
                    <span id="lblAreaResponsabilidad"></span>
                    <select id="cbAreaResponsabilidad" runat="server"></select>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label required" for="txtApartado">Apartado</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtApartado" value="" maxlength="40" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label required" for="cbCategoria">Categoría</label>
            <div class="input_wrapper input_wrapper_selectbox">
                <div class="cap">
                    <span id="lblCategoria"></span>
                    <select id="cbCategoria" runat="server"></select>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label">Emitida por</label>
            <span id="lblEmitidaPor" runat="server"></span>
        </li>
        <% } %>

        <li class="form_floated_item form_floated_item_100">
            <label class="label required" for="cbEquipo">Origen</label>
                <div class="input_wrapper input_wrapper_selectbox">
                    <div class="cap">
                        <span></span>
                        <select id="cbEquipo" runat="server">
                        </select>
                    </div>
                </div>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_100">
            <label class="label required" for="txtAsunto">Asunto</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtAsunto" value="" maxlength="90" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_full">
            <label class="label required" for="txtHallazgo">Hallazgo</label>
            <div class="textarea_wrapper clear"> 
	            <div class="top">
		            <div class="cap"></div>
	            </div>
	            <div class="inner">
		            <div class="cap">
			            <textarea rows="10" id="txtHallazgo" maxlength="1000" onkeyup="return MaxLength(this)" runat="server"></textarea>     
		            </div>
	            </div>
	            <div class="bottom">
		            <div class="cap"></div>
	            </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_full">
            <label class="label required" for="txtAccionInmediata">Acción inmediata</label>
            <div class="textarea_wrapper clear"> 
	            <div class="top">
		            <div class="cap"></div>
	            </div>
	            <div class="inner">
		            <div class="cap">
			            <textarea id="txtAccionInmediata" maxlength="800" onkeyup="return MaxLength(this)" runat="server"></textarea>     
		            </div>
	            </div>
	            <div class="bottom">
		            <div class="cap"></div>
	            </div>
            </div>
        </li>

        <% if (this.NC != null) { %>
        <li class="form_floated_item form_floated_item_full">
            <label class="label required" for="txtCausasRaices">Definición de las causas raices</label>
            <div class="textarea_wrapper clear"> 
	            <div class="top">
		            <div class="cap"></div>
	            </div>
	            <div class="inner">
		            <div class="cap">
			            <textarea id="txtCausasRaices" maxlength="800" onkeyup="return MaxLength(this)" runat="server"></textarea>     
		            </div>
	            </div>
	            <div class="bottom">
		            <div class="cap"></div>
	            </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_full">
            <label class="label required" for="txtAccionCorrectiva">Acción correctiva y preventiva</label>
            <div class="textarea_wrapper clear"> 
	            <div class="top">
		            <div class="cap"></div>
	            </div>
	            <div class="inner">
		            <div class="cap">
			            <textarea id="txtAccionCorrectiva" maxlength="800" onkeyup="return MaxLength(this)" runat="server"></textarea>     
		            </div>
	            </div>
	            <div class="bottom">
		            <div class="cap"></div>
	            </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label required" for="cbCierre">Cierre</label>
            <div class="input_wrapper input_wrapper_selectbox">
                <div class="cap">
                    <span id="lblCierre"></span>
                    <select id="cbCierre" runat="server"></select>
                </div>
            </div>
        </li>
        <% } %>

        <li class="form_floated_item form_floated_item_full">
            <label class="label" for="txtComentarios">Comentarios</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtComentarios" value="" maxlength="150" type="text" runat="server"/>
                </div>
            </div>
        </li>

        <% if (this.NC != null && this.NC.Estado == EstadosNC.Cerrada) { %>
        <li class="form_floated_item form_floated_item_half">
            <label class="label">Cerrada por</label>
            <span id="lblCerradaPor" runat="server"></span>
        </li>
        <% } %>
    </ul>

    <div class="form_buttons_container">
        <ul class="button_list">
            <% if (this.NC == null) { %>
            <li id="btnEnviar"><div class="btn primary_action_button button_100"><div class="cap"><span>Enviar</span></div></div></li>
            <% } %>
            <% if (PuedeAdministrador || PuedeImputado) { %>
            <li id="btnGuardar"><div class="btn primary_action_button button_100"><div class="cap"><span>Guardar</span></div></div></li>
            <% } %>
            <% if (PuedeAdministrador) { %>
            <li id="btnCerrar"><div class="btn secondary_action_button button_100"><div class="cap"><span>Cerrar</span></div></div></li>
            <% } %>
            <% if (this.NC == null || this.NC.Estado != EstadosNC.Cerrada) { %>
            <li id="btnCancelar"><div class="btn secondary_action_button button_100"><div class="cap"><span>Cancelar</span></div></div></li>
            <% } %>
            <% if (this.NC != null && (this.NC.Estado == EstadosNC.Cerrada || this.NC.Estado == EstadosNC.NoCorresponde)) { %>
            <li id="btnImprimir"><div class="btn primary_action_button button_100"><div class="cap"><span>Imprimir</span></div></div></li>
            <% } %>
        </ul>
    </div>
</div>

</asp:Content>

