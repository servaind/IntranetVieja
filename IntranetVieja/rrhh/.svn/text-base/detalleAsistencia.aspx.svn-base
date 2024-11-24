<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="detalleAsistencia.aspx.cs" Inherits="rrhh_detalleAsistencia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $('#txtFechaDesde').datepicker({ dayNames: dayNames, dayNamesMin: dayNamesMin, monthNames: monthNames, dateFormat: 'dd/mm/yy' });
            $('#txtFechaHasta').datepicker({ dayNames: dayNames, dayNamesMin: dayNamesMin, monthNames: monthNames, dateFormat: 'dd/mm/yy' });

            $('#btnProcesar').click(function () {
                var persona = $('#contentPlacePage_cbPersona');
                var filtro = $('#contentPlacePage_cbFiltro');
                var desde = $('#txtFechaDesde');
                var hasta = $('#txtFechaHasta');
                var result = true;

                result &= TieneDatos(persona, 'input_wrapper', 'input_wrapper_selectbox_error');
                result &= TieneDatos(filtro, 'input_wrapper', 'input_wrapper_selectbox_error');
                result &= ContieneFecha(desde, 'input_wrapper', 'input_wrapper_error');
                result &= ContieneFecha(hasta, 'input_wrapper', 'input_wrapper_error');

                if (result) {
                    MostrarLoading();

                    ConsultaAjax.url = 'detalleAsistencia.aspx/GetPartesDiarios';
                    ConsultaAjax.data = '{ "idPersona": "' + persona.val() + '", "desde": "' + desde.val() + '", "hasta": "' + hasta.val() + '", "filtro": "' + filtro.val() + '" }';
                    ConsultaAjax.AjaxSuccess = function (msg) {
                        var cant = msg.d.length;

                        $('#listado').html('');
                        var filas = [];
                        var fila;
                        if (cant > 0) {
                            for (var i = 0; i < cant; i++) {
                                fila = '<tr><td class="align-left">' + DateTimeToString(msg.d[i][1]) + '</td></tr>';
                                filas.push(fila);
                            }
                        }
                        else {
                            fila = '<tr><td class="align-center">No se encontraron coincidencias.</td></tr>';
                            filas.push(fila);
                        }
                        $('#listado').html(filas.join(''));

                        $('#lblCantDias').text(cant);

                        CerrarVentana();

                        $('#resultados').show();
                    };
                    ConsultaAjax.AjaxError = function (msg) {
                        $('#resultados').hide();

                        ErrorMsg(msg);
                    };

                    ConsultaAjax.Ejecutar();
                }
            });
            $('#btnCancelar').click(function () {
                OnCerrar();
            });

            $('#resultados').hide();
        });

        function OnCerrar() {
            location.href = '<%= Constantes.UrlIntraDefault %>';
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlacePage" Runat="Server">

<div class="page-title">
    <h1>Detalle de asistencia</h1>
</div>

<div class="form_place">
    <h3>Datos de la consulta</h3>
    <ul class="middle_form" style="height:140px">
        <li class="form_floated_item form_floated_item_half">
            <label class="label" for="cbPersona">Persona</label>
            <div class="input_wrapper input_wrapper_selectbox">
                <div class="cap">
                    <span id="lblPersona"></span>
                    <select id="cbPersona" runat="server"></select>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label" for="cbFiltro">Filtro</label>
            <div class="input_wrapper input_wrapper_selectbox">
                <div class="cap">
                    <span id="lblFiltro"></span>
                    <select id="cbFiltro" runat="server"></select>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label" for="txtFechaDesde">Fecha de inicio</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtFechaDesde" value="" maxlength="10" type="text" />
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label" for="txtFechaHasta">Fecha de finalización</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtFechaHasta" value="" maxlength="10" type="text" />
                </div>
            </div>
        </li>
    </ul>
    
    <div id="resultados">
        <h3>Resultados</h3>

        <ul class="middle_form" style="height:50px">
            <li class="form_floated_item form_floated_item_half">
                <label class="label" for="cbPersona">Cantidad de días</label>
                <span id="lblCantDias">0</span>
            </li>
        </ul>

        <table class="tbl editable listado" cellspacing="0">
            <thead>
                <tr>
                    <td class="border_left border_right">Detalle</td>
                </tr>
            </thead>
            <tbody id="listado">
                <tr>
                    <td class="align-center">No se encontraron coincidencias.</td>
                </tr>
            </tbody>
            <tfoot>
                <tr>
                    <td class="align-center" id="tdPaginas">
                    </td>
                </tr>
            </tfoot>
        </table>
    </div>

    <div class="form_buttons_container">
        <ul class="button_list">
            <li id="btnProcesar"><div class="btn primary_action_button button_100"><div class="cap"><span>Procesar</span></div></div></li>
            <li id="btnCancelar"><div class="btn secondary_action_button button_100"><div class="cap"><span>Cancelar</span></div></div></li>
        </ul>
    </div>
</div>

</asp:Content>

