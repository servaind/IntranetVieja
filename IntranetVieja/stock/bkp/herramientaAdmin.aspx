<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="herramientaAdmin.aspx.cs" Inherits="stock_herramientaAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">
<!--[if lt IE 9]>     
<script src="/js/IE9.js" type="text/javascript"></script>
<![endif]--> 
<script language="javascript" type="text/javascript">
    var current_item_pos = null;
    $(document).ready(function () {
        $('#btnAgregarItem').click(function () {
            ReiniciarCamposItem();
            AgregarItem();
        });

        $('#btnItemAceptar').click(function () {
            var marc = $('#txtItemMarca');
            var desc = $('#txtItemDescripcion');
            var cant = $('#txtItemCantidad');
            var result = true;

            if (jQuery.trim(desc.val()).length == 0) {
                desc.parents('.input_wrapper').addClass('input_wrapper_error');
                result = false;
            }
            else {
                desc.parents('.input_wrapper').removeClass('input_wrapper_error');
            }

            if (jQuery.trim(cant.val()).length == 0 || cant.val() <= 0) {
                cant.parents('.input_wrapper').addClass('input_wrapper_error');
                result = false;
            }
            else {
                cant.parents('.input_wrapper').removeClass('input_wrapper_error');
            }

            if (result) {
                if (current_item_pos == null) {
                    result = OnAgregarItem(marc.val(), desc.val(), cant.val());
                }
                else {
                    result = OnEditarItem(marc.val(), desc.val(), cant.val());
                }

                if (result) {
                    CerrarVentana();
                    ReiniciarCamposItem();
                    ult_ventana = null;
                }
            }
        });

        $('#btnItemCancelar').click(function () {
            ReiniciarCamposItem();
            CerrarVentana();
            ult_ventana = null;
        });

        $('#btnItemEliminar').click(function () {
            EliminarItem();
            CerrarVentana();
            ReiniciarCamposItem();
            ult_ventana = null;
        });

        $('#btnGuardar').click(function () {
            var numero = $('#contentPlacePage_txtNumero');
            var clasificacion = $('#contentPlacePage_cbClasifHerramienta');
            var tipoHerramienta = $('#contentPlacePage_cbTipoHerramienta');
            var marca = $('#contentPlacePage_txtMarca');
            var descripcion = $('#contentPlacePage_txtDescripcion');
            var numSerie = $('#contentPlacePage_txtNumSerie');
            var personaCargo = $('#contentPlacePage_cbPersonaCargo');
            var ubicacion = $('#contentPlacePage_txtUbicacion');
            var numEAC = $('#contentPlacePage_txtNumEAC');
            var result = true;

            ult_ventana = null;

            if(clasificacion.val() == 1) {
                result &= TieneDatos(numero, 'input_wrapper', 'input_wrapper_error');
            }
            else {
                numero.val('0');
            }

            if (isNaN(tipoHerramienta.val())) {
                tipoHerramienta.parents('.input_wrapper').addClass('input_wrapper_selectbox_error');
                result = false;
            }
            else {
                tipoHerramienta.parents('.input_wrapper').removeClass('input_wrapper_selectbox_error');
            }
            
            if (jQuery.trim(descripcion.val()).length == 0) {
                descripcion.parents('.input_wrapper').addClass('input_wrapper_error');
                result = false;
            }
            else {
                descripcion.parents('.input_wrapper').removeClass('input_wrapper_error');
            }

            if (isNaN(personaCargo.val()) || personaCargo.val() == '<%= Constantes.IdPersonaInvalido %>') {
                personaCargo.parents('.input_wrapper').addClass('input_wrapper_selectbox_error');
                result = false;
            }
            else {
                personaCargo.parents('.input_wrapper').removeClass('input_wrapper_selectbox_error');
            }

            result &= TieneDatos(numEAC, 'input_wrapper', 'input_wrapper_error') && ContieneNumeros(numEAC, 'input_wrapper', 'input_wrapper_error');

            <% if (IDHerramienta != Constantes.ValorInvalido){ %>
                var motivo = $('#contentPlacePage_txtMotivo');
                if (jQuery.trim(motivo.val()).length == 0) {
                    motivo.parents('.textarea_wrapper').addClass('textarea_wrapper_error');
                    result = false;
                }
                else {
                    motivo.parents('.textarea_wrapper').removeClass('textarea_wrapper_error');
                }
            <% } %>

            if(result){
                MostrarLoading();

                <% if (IDHerramienta == Constantes.ValorInvalido){ %>
                ConsultaAjax.url = 'herramientaAdmin.aspx/GuardarHerramienta';
                ConsultaAjax.data = JSON.stringify({ idTipo: tipoHerramienta.val(), marca: marca.val() , descripcion: descripcion.val(), 
                                    numSerie: numSerie.val() , idPersonaCargo: personaCargo.val(), ubicacion: ubicacion.val(), 
                                    numEAC: numEAC.val(), clasificacion: clasificacion.val(), numeroInst: numero.val() });
                <% }
                   else { %>
                ConsultaAjax.url = 'herramientaAdmin.aspx/ActualizarHerramienta';
                ConsultaAjax.data = JSON.stringify({ idTipo: tipoHerramienta.val(), marca: marca.val() , descripcion: descripcion.val(), 
                                    numSerie: numSerie.val(), numEAC: numEAC.val(), clasificacion: clasificacion.val(), numeroInst: numero.val(), motivo: motivo.val() });
                <% } %>

                ConsultaAjax.AjaxSuccess = function (msg) {
                    Mensaje(msg.d, 'success', true, false, 'Aceptar', '', 'OnCancel()', '');
                };
                ConsultaAjax.AjaxError = function (msg) {
                    ErrorMsg(msg);
                };

                ConsultaAjax.Ejecutar();
            }
        });

        $('#btnCancelar').click(function () {
            Mensaje('Todos los datos ingresados serán descartados, ¿desea continuar?', 'warning', true, true, 'Aceptar', 'Cancelar', 'OnCancel()', 'custom_dialog.close()');
        });

        ActualizarListaItems();

        <% if (IDHerramienta == Constantes.ValorInvalido){ %>
            $('#contentPlacePage_cbTipoHerramienta').change();
            $('#contentPlacePage_cbPersonaCargo').change();
        <% } %>
    });

    function OnCancel() {
        this.location.href = 'herramientaAdmin.aspx';
    }

    var bind_events = function () {
        $('.item-herramienta').unbind('click').click(function () {
            var index = $(this).attr('idx');

            EditarItem(index);
        });
    }

    function ReiniciarCamposItem() {
        $('#divItem .input_wrapper').removeClass('input_wrapper_error');
        $('#divItem input').val('');
        current_item_pos = null;
    }

    function AgregarItem() {
        $('#item_titulo').text('Agregar ítem');
        $('#btnItemEliminar').hide();
        MostrarVentana('divItem');

        ult_ventana = 'divItem';
    }

    function OnAgregarItem(marca, descripcion, cantidad) {
        MostrarLoading();

        ConsultaAjax.url = 'herramientaAdmin.aspx/AgregarItem';
        ConsultaAjax.data = JSON.stringify({ marca: marca, descripcion: descripcion, cantidad: cantidad });
        ConsultaAjax.AjaxSuccess = function (msg) {
            ActualizarListaItems();
            return true;
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
            return false;
        };

        ConsultaAjax.Ejecutar();
    }

    function OnEditarItem(marca, descripcion, cantidad) {
        MostrarLoading();

        ConsultaAjax.url = 'herramientaAdmin.aspx/ActualizarItem';
        ConsultaAjax.data = JSON.stringify({ posicion: current_item_pos, marca: marca, descripcion: descripcion, cantidad: cantidad });
        ConsultaAjax.AjaxSuccess = function (msg) {
            ActualizarListaItems();
            return true;
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
            return false;
        };

        ConsultaAjax.Ejecutar();
    }

    function EditarItem(index) {
        MostrarLoading();

        ConsultaAjax.url = 'herramientaAdmin.aspx/GetItem';
        ConsultaAjax.data = '{ "posicion":"' + index + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            if (msg.d.length >= 3) {
                $('#txtItemMarca').val(msg.d[0]);
                $('#txtItemDescripcion').val(msg.d[1]);
                $('#txtItemCantidad').val(msg.d[2]);

                $('#item_titulo').text('Editar ítem');
                $('#btnItemEliminar').show();
                MostrarVentana('divItem');

                current_item_pos = index;
                ult_ventana = 'divItem';
            } else {
                CerrarVentana();
            }
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function EliminarItem() {
        MostrarLoading();

        ConsultaAjax.url = 'herramientaAdmin.aspx/EliminarItem';
        ConsultaAjax.data = '{ "posicion":"' + current_item_pos + '" }';
        ConsultaAjax.AjaxSuccess = function (msg) {
            ActualizarListaItems();
        };
        ConsultaAjax.AjaxError = function (msg) {
            ErrorMsg(msg);
        };

        ConsultaAjax.Ejecutar();
    }

    function ActualizarListaItems() {
        MostrarLoading();

        ConsultaAjax.url = 'herramientaAdmin.aspx/GetItems';
        ConsultaAjax.data = null;
        ConsultaAjax.AjaxSuccess = function (msg) {
            $('#itemsHerramienta').html('');

            if (msg.d.length > 0) {
                var listItems = [];
                for (i = 0; i < msg.d.length; i++) {
                    listItems.push('<tr class="item-herramienta" idx="' + i + '"><td>' + msg.d[i][0] + '</td><td>'
                                   + msg.d[i][1] + '</td><td class="align-center">' + msg.d[i][2] + '</td></tr>');
                }

                $('#itemsHerramienta').append(listItems.join(''));
            }
            else{
                $('#itemsHerramienta').html('<tr><td colspan="3" class="align-center">No hay ítems disponibles.</td></tr>');
            }

            bind_events();

            CerrarVentana();
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
    <h1><span id="lblTitulo" runat="server">Alta de</span> herramienta</h1>
</div>

<div class="form_place">
    <h3>1. Datos de la herramienta</h3>

    <p>Los campos marcados con <span class="required"></span> son obligatorios.</p>
    <br />

    <ul class="middle_form" style="height:<%= IDHerramienta == Constantes.ValorInvalido ? 330 : 450 %>px">
        <li class="form_floated_item form_floated_item_half">
            <label class="label required" for="txtMarca">Número (sólo para Instrumentos)</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtNumero" value="" maxlength="5" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label required" for="cbClasifHerramienta">Clasificación</label>
            <div class="input_wrapper input_wrapper_selectbox">
                <div class="cap">
                    <span id="lblClasifHerramienta" runat="server"></span>
                    <select id="cbClasifHerramienta" runat="server"></select>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label required" for="cbTipoHerramienta">Tipo de herramienta</label>
            <div class="input_wrapper input_wrapper_selectbox">
                <div class="cap">
                    <span id="lblTipoHerramienta" runat="server"></span>
                    <select id="cbTipoHerramienta" runat="server"></select>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label" for="txtMarca">Marca</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtMarca" value="" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label required" for="txtDescripcion">Descripción</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtDescripcion" value="" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label" for="txtNumSerie">Nº de serie</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtNumSerie" value="" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label required" for="cbPersonaCargo">Persona a cargo</label>
            <div class="input_wrapper input_wrapper_selectbox">
                <div class="cap">
                    <span id="lblPersonaCargo" runat="server"></span>
                    <select id="cbPersonaCargo" runat="server">
                    </select>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label" for="txtUbicacion">Ubicación</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtUbicacion" value="" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label required" for="txtNumEAC">Número de EAC</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtNumEAC" value="" type="text" runat="server"/>
                </div>
            </div>
        </li>

        <% if (IDHerramienta != Constantes.ValorInvalido) { %>
        <li class="form_floated_item form_floated_item_full">
            <label class="label required" for="txtMotivo">Motivo de la modificación</label>
            <div class="textarea_wrapper clear"> 
	            <div class="top">
		            <div class="cap"></div>
	            </div>
	            <div class="inner">
		            <div class="cap">
			            <textarea id="txtMotivo" runat="server"></textarea>     
		            </div>
	            </div>
	            <div class="bottom">
		            <div class="cap"></div>
	            </div>
            </div>
        </li>
        <% } %>
    </ul>
    
    <h3 class="middle_h3">2. Ítems que contiene (si aplica)</h3>

    <div class="suggestion_message right" style="top:<%= (IDHerramienta == Constantes.ValorInvalido) ? "495" : "610" %>px;">Puede editar o eliminar los ítems haciendo click sobre la fila.</div>

    <table class="tbl editable" cellspacing="0" cellpadding="0" style="width:620px">
        <thead>
            <tr>
                <td class="border_left align-center" style="width:150px">Marca</td>
                <td class="border_middle align-center" style="width:250px">Descripción</td>
                <td class="border_right align-center" style="width:100px">Cantidad</td>
            </tr>
        </thead>
        <tbody id="itemsHerramienta">

        </tbody>
        <tfoot>
            <tr>
                <td colspan="3" class="border_middle">
                    <a id="btnAgregarItem">Agregar ítem</a>
                </td>
            </tr>
        </tfoot>
    </table>

    <div class="form_buttons_container">
        <ul class="button_list">
            <li id="btnGuardar"><div class="btn primary_action_button button_100"><div class="cap"><span>Guardar</span></div></div></li>
            <li id="btnCancelar"><div class="btn secondary_action_button button_100"><div class="cap"><span>Cancelar</span></div></div></li>
        </ul>
    </div>
</div>

<div class="dialog_wrapper" style="width:500px" id="divItem">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3 id="item_titulo"></h3></div></div></div>
    </div>

    <div class="dialog_content">
        <ul class="middle_form">
            <li class="form_floated_item normal">
                <label class="label" for="txtItemMarca">Marca</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtItemMarca" value="" type="text"/>
                    </div>
                </div>
            </li>
            <li class="form_floated_item normal">
                <label class="label required" for="txtItemDescripcion">Descripción</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtItemDescripcion" value="" type="text"/>
                    </div>
                </div>
            </li>
            <li class="form_floated_item normal" style="width:120px">
                <label class="label required" for="txtItemCantidad">Cantidad</label>
                <div class="input_wrapper">
                    <div class="cap">
                        <input id="txtItemCantidad" value="" type="text"/>
                    </div>
                </div>
            </li>
        </ul>
        <p>Los campos marcados con <span class="required"></span> son obligatorios.</p>
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li><div class="btn primary_action_button_small button_100" id="btnItemAceptar"><div class="cap"><span>Aceptar</span></div></div></li>
            <li><div class="btn secondary_action_button_small button_100" id="btnItemCancelar"><div class="cap"><span>Cancelar</span></div></div></li>
            <li><div class="btn secondary_action_button_small button_100" id="btnItemEliminar"><div class="cap"><span>Eliminar</span></div></div></li>
        </ul>
    </div>
</div>

</asp:Content>

