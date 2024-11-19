<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="informacionObraAdmin.aspx.cs" Inherits="general_informacionObraAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">
    <script language="javascript" type="text/javascript">
        var personas = [];
        var vehiculos = [];
        var persVehicModif = false;

        $(document).ready(function () {
            <% if(IO == null || this.PuedeEditar) { %>
            $('#contentPlacePage_txtFechaEntrega').datepicker({ dayNames: dayNames, dayNamesMin: dayNamesMin, monthNames: monthNames, dateFormat: 'dd/mm/yy' });
            $('#contentPlacePage_txtFechaEstimada').datepicker({ dayNames: dayNames, dayNamesMin: dayNamesMin, monthNames: monthNames, dateFormat: 'dd/mm/yy' <% if(IO == null) { %>,minDate: '<%= this.FechaInicio %>'<% } %> });
            $('#contentPlacePage_txtFechaFinalizacion').datepicker({ dayNames: dayNames, dayNamesMin: dayNamesMin, monthNames: monthNames, dateFormat: 'dd/mm/yy'<% if(IO == null) { %>,minDate: '<%= this.FechaInicio %>'<% } %> });
            <% } %>

            $('input[type="checkbox"]').change(function () {
                SyncGUI();
            });
            $('#btnGuardar').click(function () {
                <% if(this.PuedeCerrar) { %>
                    Mensaje('¿Desea guardar los cambios realizados?', 'warning', true, true, 'Cancelar', 'Guardar', 'custom_dialog.close()', 'GuardarIO()');
                <% } else { %>
                    GuardarIO();
                <% } %>
            });
            <% if(this.PuedeCerrar) { %>
            $('#btnCerrar').click(function () {
                Mensaje('Una vez cerrada, la información no podrá ser editada nuevamente. ¿Desea continuar?', 'warning', true, true, 'Cancelar', 'Cerrar', 'custom_dialog.close()', 'GuardarIO()');
            });
            <% } %>
            $('#btnCancelar').click(function () {
                Mensaje('Todos los datos ingresados serán descartados, ¿desea continuar?', 'warning', true, true, 'Cancelar', 'Aceptar', 'custom_dialog.close()', 'OnCerrar()');
            });
            $('#btnAgregarPersonaMant').click(function () {
                persVehicModif = true;
                AgregarPersonaMant('<%= Constantes.IdPersonaInvalido.ToString() %>');
            });
            $('#btnAgregarPersonaObra').click(function () {
                persVehicModif = true;
                AgregarPersonaObra('<%= Constantes.IdPersonaInvalido.ToString() %>');
            });
            $('#btnAgregarVehiculo').click(function () {
                persVehicModif = true;
                AgregarVehiculo('0');
            });
            <% if(IO != null) { %>
            $('#btnImprimir').click(function () {
                window.open('<%= Encriptacion.GetURLEncriptada("/general/InformacionObraImprimir.aspx", "id=" + IO.IdObra) %>', "Imprimir", "status=0, toolbar=0, resizable=0, "
                          + "menubar=0, directories=0, width='780', height:'500', scrollbars=1");
            });
            <% } %>
            $('#contentPlacePage_txtFechaEstimada').change(function (){
                NormalizarFechas();
            });
            $('#contentPlacePage_txtFechaFinalizacion').change(function (){
                NormalizarFechas();
            });
            $('#btnAutorizAceptar').click(function (){
                var motivo = $('#txtMotivoAutoriz');
                var result = true;

                result &= TieneDatos(motivo, 'textarea_wrapper', 'textarea_wrapper_error');

                if(result) {
                    MostrarLoading();

                    ConsultaAjax.url = 'informacionObraAdmin.aspx/SolicitarAutorizacion';
                    ConsultaAjax.data = JSON.stringify({ motivo: motivo.val() });
                    ConsultaAjax.AjaxSuccess = function (msg) {
                        Mensaje(msg.d, 'success', true, false, 'Aceptar', '', 'OnCerrar()', '');
                    };
                    ConsultaAjax.AjaxError = function (msg) {
                        ult_ventana = 'divAutorizacion';
                        ErrorMsg(msg);
                    };

                    ConsultaAjax.Ejecutar();
                }
            });
            $('#btnAutorizCancelar').click(function (){
                CerrarVentana();
            });

            GetPersonas();
            GetVehiculos();
            GetPersonasAsignadasMant();
            GetPersonasAsignadasObra();
            GetVehiculosAsignados();
            <% if(IO != null) { %>
            GetHistorial();
            <% } %>
            $('select').change();
            SyncGUI();

            <% if(IO != null && IO.Datos.AutorizacionPendiente) { %>
                Mensaje('La Información Interna de Obra posee una autorización pendiente y no podrá ser editada hasta obtener una respuesta.', 'warning', true, false, 'Aceptar', '', 'custom_dialog.close()', '');
            <% } %>
        });

        function OnCerrar() {
            location.href = '/general/InformacionObraLista.aspx';
        }

        <% if(IO == null || this.PuedeEditar) { %>
        function GuardarIO() {
            custom_dialog.close();

            var responsableObra = $('#contentPlacePage_cbResponsableObra');
            var tipoTrabajo = $('#contentPlacePage_cbTipoTrabajo');
            var imputacion = $('#contentPlacePage_txtImputacion');
            var cliente = $('#contentPlacePage_txtCliente');
            var ordenCompra = $('#contentPlacePage_txtOrdenCompra');
            var fechaEntrega = $('#contentPlacePage_txtFechaEntrega');
            var subcontratistas = $('#contentPlacePage_chkSubcontratistas');
            var subcontratEmpresa = $('#contentPlacePage_txtSubcontratEmpresa');
            var predioTerceros = $('#contentPlacePage_chkPredioTerceros');
            var predioTercEmpresa = $('#contentPlacePage_txtPredioTercEmpresa');
            var ubicacion = $('#contentPlacePage_txtUbicacion');
            var provincia = $('#contentPlacePage_txtProvincia');
            var respTecCliente = $('#contentPlacePage_txtRespTecCliente');
            var respTecClienteTel = $('#contentPlacePage_txtRespTecClienteTel');
            var respTecClienteEmail = $('#contentPlacePage_txtRespTecClienteEmail');
            var respSegCliente = $('#contentPlacePage_txtRespSegCliente');
            var respSegClienteTel = $('#contentPlacePage_txtRespSegClienteTel');
            var respSegClienteEmail = $('#contentPlacePage_txtRespSegClienteEmail');
            var contAdminCliente = $('#contentPlacePage_txtContAdminCliente');
            var contAdminClienteTel = $('#contentPlacePage_txtContAdminClienteTel');
            var contAdminClienteEmail = $('#contentPlacePage_txtContAdminClienteEmail');
            var fechaInicio = $('#contentPlacePage_txtFechaEstimada');
            var duracion = $('#contentPlacePage_txtDuracion');
            var descripcionTareas = $('#contentPlacePage_txtDescripcionTareas');
            var fechaFinalizacion = $('#contentPlacePage_txtFechaFinalizacion');
            var gerenteProyecto = $('#contentPlacePage_cbGerenteProyecto');
            var objetivoProyecto = $('#contentPlacePage_txtObjetivoProyecto');
            var pMant = [];
            var pObras = [];
            var pVehic = [];
            var result = true;
            var mostrarError = true;

            result &= TieneDatos(responsableObra, 'input_wrapper', 'input_wrapper_selectbox_error') && ContieneValorDiferente(responsableObra, '<%= Constantes.IdPersonaInvalido.ToString() %>', 'input_wrapper', 'input_wrapper_selectbox_error');
            result &= TieneDatos(tipoTrabajo, 'input_wrapper', 'input_wrapper_selectbox_error');
            result &= TieneDatos(imputacion, 'input_wrapper', 'input_wrapper_error');
            result &= TieneDatos(cliente, 'input_wrapper', 'input_wrapper_error');
            result &= TieneDatos(ordenCompra, 'input_wrapper', 'input_wrapper_error');
            result &= ContieneFecha(fechaEntrega, 'input_wrapper', 'input_wrapper_error');
            if (subcontratistas.is(':checked')) {
                result &= TieneDatos(subcontratEmpresa, 'input_wrapper', 'input_wrapper_error');
            }
            if (predioTerceros.is(':checked')) {
                result &= TieneDatos(predioTercEmpresa, 'input_wrapper', 'input_wrapper_error');
            }
            result &= TieneDatos(contAdminCliente, 'input_wrapper', 'input_wrapper_error');
            result &= TieneDatos(contAdminClienteTel, 'input_wrapper', 'input_wrapper_error');
            result &= TieneDatos(contAdminClienteEmail, 'input_wrapper', 'input_wrapper_error');
            result &= TieneDatos(ubicacion, 'input_wrapper', 'input_wrapper_error');
            result &= TieneDatos(provincia, 'input_wrapper', 'input_wrapper_error');
            result &= TieneDatos(respTecCliente, 'input_wrapper', 'input_wrapper_error');
            result &= TieneDatos(respTecClienteTel, 'input_wrapper', 'input_wrapper_error');
            result &= TieneDatos(respTecClienteEmail, 'input_wrapper', 'input_wrapper_error');
            result &= TieneDatos(respSegCliente, 'input_wrapper', 'input_wrapper_error');
            result &= TieneDatos(respSegClienteTel, 'input_wrapper', 'input_wrapper_error');
            result &= TieneDatos(respSegClienteEmail, 'input_wrapper', 'input_wrapper_error');
            result &= ContieneFecha(fechaInicio, 'input_wrapper', 'input_wrapper_error');
            result &= TieneDatos(duracion, 'input_wrapper', 'input_wrapper_error');
            result &= TieneDatos(descripcionTareas, 'textarea_wrapper', 'textarea_wrapper_error');
            result &= ContieneFecha(fechaFinalizacion, 'input_wrapper', 'input_wrapper_error');
            result &= TieneDatos(objetivoProyecto, 'textarea_wrapper', 'textarea_wrapper_error');
            result &= TieneDatos(gerenteProyecto, 'input_wrapper', 'input_wrapper_selectbox_error');

            var p, i, j;
            var cant = iPersonasMant.length;
            for (i = 0; i < cant; i++) {
                j = iPersonasMant[i];

                p = $('#cbPersonaMant_' + j);

                result &= TieneDatos(p, 'input_wrapper', 'input_wrapper_selectbox_error') && ContieneValorDiferente(p, '<%= Constantes.IdPersonaInvalido.ToString() %>', 'input_wrapper', 'input_wrapper_selectbox_error');

                if (result) {
                    pMant.push(p.val());
                }
            }
            cant = iPersonasObra.length;
            for (i = 0; i < cant; i++) {
                j = iPersonasObra[i];

                p = $('#cbPersonaObra_' + j);

                result &= TieneDatos(p, 'input_wrapper', 'input_wrapper_selectbox_error') && ContieneValorDiferente(p, '<%= Constantes.IdPersonaInvalido.ToString() %>', 'input_wrapper', 'input_wrapper_selectbox_error');

                if (result) {
                    pObras.push(p.val());
                }
            }
            cant = iVehiculos.length;
            for (i = 0; i < cant; i++) {
                j = iVehiculos[i];

                p = $('#cbVehiculo_' + j);

                result &= TieneDatos(p, 'input_wrapper', 'input_wrapper_selectbox_error');

                if (result) {
                    pVehic.push(p.val());
                }
            }

            <% if(!this.TieneAutorizacion) { %>
            var difDiasInicio = DateDiff('<%=DateTime.Now.ToShortDateString() %>', fechaInicio.val());
            if(persVehicModif && difDiasInicio < <%= InformacionObras.MinDiasAnticipo.ToString() %>) {
                Mensaje('Se han realizado modificaciones en Personal / Vehículos y la fecha estimada de inicio de obra en sitio no respeta el anticipo mínimo de <%= InformacionObras.MinDiasAnticipo.ToString() %> días. Para guardar los cambios, modifique la fecha estimada de inicio de obra en sitio o solicite una excepción e intente nuevamente.', 'warning', true, true, 'Cerrar', 'Autorización', 'custom_dialog.close()', 'SolicitarAutorizacion()');
                mostrarError = false;
                result = false;
            }
            else if(difDiasInicio < <%= InformacionObras.MinDiasAnticipo.ToString() %>)
            {
                Mensaje('La fecha estimada de inicio de obra en sitio no respeta el anticipo mínimo de <%= InformacionObras.MinDiasAnticipo.ToString() %> días. Para guardar los cambios, modifique la fecha estimada de inicio de obra en sitio o solicite una excepción e intente nuevamente.', 'warning', true, true, 'Cerrar', 'Autorización', 'custom_dialog.close()', 'SolicitarAutorizacion()');
                mostrarError = false;
                result = false;
            }
            <% } %>

            if (result) {
                MostrarLoading();

                ConsultaAjax.url = 'informacionObraAdmin.aspx/<%= IO != null ? "Actualizar" : "Nueva" %>InformacionObra';
                ConsultaAjax.data = JSON.stringify({ idResponsableObra: responsableObra.val(), tipoTrabajo: tipoTrabajo.val(), imputacion: imputacion.val(), cliente: cliente.val(), ordenCompra: ordenCompra.val(), fechaEntrega: fechaEntrega.val(), subcontratistas: subcontratistas.is(':checked'), subcontratEmpresa: subcontratEmpresa.val(), predioTerceros: predioTerceros.is(':checked'), predioTercEmpresa: predioTercEmpresa.val(), ubicacion: ubicacion.val(), provincia: provincia.val(), respTecCliente: respTecCliente.val(), respTecClienteTel: respTecClienteTel.val(), respTecClienteEmail: respTecClienteEmail.val(), respSegCliente: respSegCliente.val(), respSegClienteTel: respSegClienteTel.val(), respSegClienteEmail: respSegClienteEmail.val(), contAdminCliente: contAdminCliente.val(), contAdminClienteTel: contAdminClienteTel.val(), contAdminClienteEmail: contAdminClienteEmail.val(), fechaInicio: fechaInicio.val(), duracion: duracion.val(), descripcionTareas: descripcionTareas.val(), fechaFinaliz: fechaFinalizacion.val(), pMant: pMant, pObras: pObras, pVehic: pVehic, gerenteProyecto: gerenteProyecto.val(), objetivoProyecto: objetivoProyecto.val() });
                ConsultaAjax.AjaxSuccess = function (msg) {
                    Mensaje(msg.d, 'success', true, false, 'Aceptar', '', 'OnCerrar()', '');
                };
                ConsultaAjax.AjaxError = function (msg) {
                    ErrorMsg(msg);
                };

                ConsultaAjax.Ejecutar();
            }
            else {
                if(mostrarError) {
                    ErrorMsg('La operación no se puede completar debido a que existen errores en la carga de información.');
                }
            }
        }
        <% } %>

        function SyncGUI() {
            if ($('#contentPlacePage_chkSubcontratistas').is(':checked')) {
                $('#contentPlacePage_txtSubcontratEmpresa').parents('.input_wrapper').removeClass('input_wrapper_disabled');
                $('#contentPlacePage_txtSubcontratEmpresa').removeAttr('disabled');
            }
            else {
                $('#contentPlacePage_txtSubcontratEmpresa').parents('.input_wrapper').addClass('input_wrapper_disabled');
                $('#contentPlacePage_txtSubcontratEmpresa').attr('disabled', 'disabled');
                $('#contentPlacePage_txtSubcontratEmpresa').val('');
            }

            if ($('#contentPlacePage_chkPredioTerceros').is(':checked')) {
                $('#contentPlacePage_txtPredioTercEmpresa').parents('.input_wrapper').removeClass('input_wrapper_disabled');
                $('#contentPlacePage_txtPredioTercEmpresa').removeAttr('disabled');
            }
            else {
                $('#contentPlacePage_txtPredioTercEmpresa').parents('.input_wrapper').addClass('input_wrapper_disabled');
                $('#contentPlacePage_txtPredioTercEmpresa').attr('disabled', 'disabled');
                $('#contentPlacePage_txtPredioTercEmpresa').val('');
            }
        }

        function DibujarAccionesPersona() {
            $('.actions_persona').html('<ul><li class="delete"></li></ul>');
        }

        var EnlazarEventos = function () {
            $('.input_actions ul li.delete').unbind('click').click(function () {
                var tipo = $(this).parents('.input_actions').attr('name');
                var value = $(this).parents('.input_actions').attr('value');

                if (tipo == 'mantenimiento') {
                    persVehicModif = true;
                    EliminarPersonaMant(value);
                }
                else if (tipo == 'obra') {
                    persVehicModif = true;
                    EliminarPersonaObra(value);
                }
                else if (tipo == 'vehiculo') {
                    persVehicModif = true;
                    EliminarVehiculo(value);
                }
            });
        }

        function GetPersonas() {
            ConsultaAjax.url = 'informacionObraAdmin.aspx/GetPersonas';
            ConsultaAjax.AjaxSuccess = function (msg) {
                var cant = msg.d.length;

                for (var i = 0; i < cant; i++) {
                    personas.push(msg.d[i]);
                }
            };
            ConsultaAjax.AjaxError = function (msg) {
                ErrorMsg(msg);
            };

            ConsultaAjax.Ejecutar();
        }

        function GetVehiculos() {
            ConsultaAjax.url = 'informacionObraAdmin.aspx/GetVehiculos';
            ConsultaAjax.AjaxSuccess = function (msg) {
                var cant = msg.d.length;

                for (var i = 0; i < cant; i++) {
                    vehiculos.push(msg.d[i]);
                }
            };
            ConsultaAjax.AjaxError = function (msg) {
                ErrorMsg(msg);
            };

            ConsultaAjax.Ejecutar();
        }

        function GetPersonasAsignadasMant() {
            GetPersonasAsignadas(personas_mant);
        }

        function GetPersonasAsignadasObra() {
            GetPersonasAsignadas(personas_obra);
        }

        function GetPersonasAsignadas(tipo) {
            ConsultaAjax.url = 'informacionObraAdmin.aspx/GetPersonas' + (tipo == personas_mant ? 'Mant' : 'Obra');
            ConsultaAjax.AjaxSuccess = function (msg) {
                var cant = msg.d.length;

                for (var i = 0; i < cant; i++) {
                    if(tipo == personas_mant) {
                        AgregarPersonaMant(msg.d[i]);
                    }
                    else {
                        AgregarPersonaObra(msg.d[i]);
                    }
                }
            };
            ConsultaAjax.AjaxError = function (msg) {
                ErrorMsg(msg);
            };

            ConsultaAjax.Ejecutar();
        }

        function GetVehiculosAsignados() {
            ConsultaAjax.url = 'informacionObraAdmin.aspx/GetPatentesVehic';
            ConsultaAjax.AjaxSuccess = function (msg) {
                var cant = msg.d.length;

                for (var i = 0; i < cant; i++) {
                    AgregarVehiculo(msg.d[i]);
                }
            };
            ConsultaAjax.AjaxError = function (msg) {
                ErrorMsg(msg);
            };

            ConsultaAjax.Ejecutar();
        }

        function GetHistorial() {
            ConsultaAjax.url = 'informacionObraAdmin.aspx/GetHistorial';
            ConsultaAjax.AjaxSuccess = function (msg) {
                var cant = msg.d.length;

                if (cant > 0) {
                    var filas = [];
                    var i = 0;
                    for (i; i < cant; i++) {
                        var fila = '<tr class="fila-color" onclick="VerIO(\'' + msg.d[i][2] + '\')">';
                        fila += '<td class="align-center">' + msg.d[i][0] + '</td>';
                        fila += '<td class="align-right">' + msg.d[i][1] + '</td>';
                        fila += '</tr>';
                        filas.push(fila);
                    }

                    $('#historial-items').html(filas.join(''));
                }
                else {
                    $('#historial-items').html('<tr><td colspan="2" class="align-center">No hay revisiones disponibles.</td></tr>');
                }
            };
            ConsultaAjax.AjaxError = function (msg) {
                ErrorMsg(msg);
            };

            ConsultaAjax.Ejecutar();
        }

        function VerIO(idIO) {
            location.href = 'informacionObraAdmin.aspx?p=' + idIO;
        }

        function SolicitarAutorizacion() {
            custom_dialog.close();

            MostrarVentana('divAutorizacion');
        }

        // Personas.
        var ultPersonaMant = 0;
        var ultPersonaObra = 0;
        var iPersonasMant = [];
        var iPersonasObra = [];
        var personas_mant = 0;
        var personas_obra = 1;

        function AgregarPersonaMant(idPersona) {
            AgregarPersona(idPersona, personas_mant);
        }

        function AgregarPersonaObra(idPersona) {
            AgregarPersona(idPersona, personas_obra);
        }

        function AgregarPersona(idPersona, tipo) {
            var ultPersona = (tipo == personas_mant ? ultPersonaMant : ultPersonaObra);
            var prefCombo = (tipo == personas_mant ? 'Mant' : 'Obra');

            var fila = '<tr class="no-editable" id="persona' + prefCombo + '_' + ultPersona + '">';
            fila += '<td class="align-left"><div class="hasActions"><div class="input_wrapper input_wrapper_selectbox"><div class="cap"><span></span><select id="cbPersona' + prefCombo + '_' + ultPersona + '"></select></div></div><div class="input_actions input_actions_selectbox actions_persona" name="' + (tipo == personas_mant ? 'mantenimiento' : 'obra') + '" value="' + ultPersona + '"></div></div></td>';
            fila += '</tr>';

            $('#personas-' + (tipo == personas_mant ? 'mantenimiento' : 'obra') + '').append(fila);

            var cp = personas.length;
            var p = [];
            for (var i = 0; i < cp; i++) {
                p.push('<option value="' + personas[i][0] + '">' + personas[i][1] + '</option>');
            }
            $('#cbPersona' + prefCombo + '_' + ultPersona).html(p.join(''));

            if (tipo == personas_mant) {
                iPersonasMant.push(ultPersona);
            }
            else {
                iPersonasObra.push(ultPersona);
            }

            <% if(IO != null && !this.PuedeEditar) { %>
            $('#cbPersona' + prefCombo + '_' + ultPersona).attr('disabled', 'disabled');
            <% } else { %>
            DibujarAccionesPersona();
            <% } %>

            EnlazarEventosMaster();
            EnlazarEventos();

            $('#cbPersona' + prefCombo + '_' + ultPersona).val(idPersona);
            $('#cbPersona' + prefCombo + '_' + ultPersona).change();

            if (tipo == personas_mant) {
                ultPersonaMant++;
            }
            else {
                ultPersonaObra++;
            }
        }

        function EliminarPersonaMant(idPersona) {
            EliminarPersona(idPersona, personas_mant);
        }

        function EliminarPersonaObra(idPersona) {
            EliminarPersona(idPersona, personas_obra);
        }

        function EliminarPersona(persona, tipo) {
            $('#persona' + (tipo == personas_mant ? 'Mant' : 'Obra') + '_' + persona).remove();

            if (tipo == personas_mant) {
                RemoveArrayElement(iPersonasMant, parseInt(persona));
            }
            else {
                RemoveArrayElement(iPersonasObra, parseInt(persona));
            }
        }

        // ------------------------------------------------------------

        // Vehículos.
        var ultVehiculo = 0;
        var iVehiculos = [];

        function AgregarVehiculo(idVehiculo) {
            var fila = '<tr class="no-editable" id="vehiculo_' + ultVehiculo + '">';
            fila += '<td class="align-left"><div class="hasActions"><div class="input_wrapper input_wrapper_selectbox"><div class="cap"><span></span><select id="cbVehiculo_' + ultVehiculo + '"></select></div></div><div class="input_actions input_actions_selectbox actions_persona" name="vehiculo" value="' + ultVehiculo + '"></div></div></td>';
            fila += '</tr>';

            $('#vehiculos-obra').append(fila);

            var cp = vehiculos.length;
            var p = [];
            for (var i = 0; i < cp; i++) {
                p.push('<option value="' + vehiculos[i][0] + '">' + vehiculos[i][1] + '</option>');
            }
            $('#cbVehiculo_' + ultVehiculo).html(p.join(''));

            iVehiculos.push(ultVehiculo);

            <% if(IO != null && !this.PuedeEditar) { %>
            $('#cbVehiculo_' + ultVehiculo).attr('disabled', 'disabled');
            <% } else { %>
            DibujarAccionesPersona();
            <% } %>

            EnlazarEventosMaster();
            EnlazarEventos();

            $('#cbVehiculo_' + ultVehiculo).val(idVehiculo);
            $('#cbVehiculo_' + ultVehiculo).change();

            ultVehiculo++;
        }

        function EliminarVehiculo(vehiculo) {
            $('#vehiculo_' + vehiculo).remove();

            RemoveArrayElement(iVehiculos, parseInt(vehiculo));
        }
        // ------------------------------------------------------------

        function NormalizarFechas() {
            var fInicio = $('#contentPlacePage_txtFechaEstimada');
            var fFin = $('#contentPlacePage_txtFechaFinalizacion');
            var duracion = $('#contentPlacePage_txtDuracion');

            if(fInicio.val().length == 0 || fFin.val().length == 0) {
                duracion.val('');
            }
            else {
                var auxF1 = fInicio.val().split('/');
                var auxF2 = fFin.val().split('/');

                var fechaInicio = new Date(auxF1[2], auxF1[1] - 1, auxF1[0]);
                var fechaFin = new Date(auxF2[2], auxF2[1] - 1, auxF2[0]);
                var dias;

                if(fechaInicio > fechaFin) {
                    fFin.val(fInicio.val());
                    dias = 0;
                }
                else {
                    var diff = (fechaFin.getTime() - fechaInicio.getTime());
                    dias = (((diff / 1000) / 60) / 60) / 24;
                }

                duracion.val(dias + ' día' + (dias == 1 ? '' : 's') + '.');
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlacePage" Runat="Server">

<div class="page-title">
    <h1>Información interna de obra / proyecto</h1>
</div>

<div class="form-steps" style="width:300px">
    <ul>
        <li>
            <div class="step-number<%= IO == null ? " active" : "" %>">1.</div>
            <div class="step-description<%= IO == null ? " active" : "" %>">Solicitud</div>
        </li>
        <li class="separator"></li>
        <li>
            <div class="step-number<%= IO != null ? " active" : "" %>">2.</div>
            <div class="step-description<%= IO != null ? " active" : "" %>">Generada</div>
        </li>
    </ul>    
</div>

<div class="form_place">
    <h3>Contenido</h3>

    <% if (IO == null) { %>
    <p>Todos los campos deben ser completados.</p>
    <% } else { %>
    <p>Los campos marcados en rojo son aquellos que presentan cambios respecto de la revisión anterior.</p>
    <% } %>
    <br />

    <ul class="middle_form" style="height:1420px">
        <li class="form_floated_item form_floated_item_half">
            <label class="label">Número</label>
            <span id="lblNumero" runat="server">-</span>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label">Fecha</label>
            <span id="lblFecha" runat="server">-</span>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label">Informa</label>
            <span id="lblInforma" runat="server">-</span>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label">Revisión</label>
            <span id="lblRevision" runat="server">-</span>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label" id="lblGerenteProyecto" runat="server">Gerente de proyecto</label>
            <div class="input_wrapper input_wrapper_selectbox">
                <div class="cap">
                    <span id="lblGerenteProyecto"></span>
                    <select id="cbGerenteProyecto" runat="server"></select>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label" id="lblTipoTrabajo" runat="server">Tipo de trabajo</label>
            <div class="input_wrapper input_wrapper_selectbox">
                <div class="cap">
                    <span id="lblTipoTrabajo"></span>
                    <select id="cbTipoTrabajo" runat="server"></select>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label" id="lblResponsableObra" runat="server">Responsable de obra</label>
            <div class="input_wrapper input_wrapper_selectbox">
                <div class="cap">
                    <span id="lblResponsableObra"></span>
                    <select id="cbResponsableObra" runat="server"></select>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_100">
            <label class="label" id="lblImputacion" runat="server">Cotización / Imputación</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtImputacion" value="" maxlength="44" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_100">
            <label class="label" id="lblCliente" runat="server">Cliente</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtCliente" value="" maxlength="44" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label" id="lblOrdenCompra" runat="server">Orden de compra</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtOrdenCompra" value="" maxlength="20" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label" id="lblFechaEntrega" runat="server">Fecha de entrega según OC</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtFechaEntrega" readonly="readonly" value="" maxlength="10" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label" id="lblSubcontratEmpresa" runat="server"><input type="checkbox" id="chkSubcontratistas" runat="server" />Subcontratistas</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtSubcontratEmpresa" value="" maxlength="25" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label" id="lblPredioTercEmpresa" runat="server"><input type="checkbox" id="chkPredioTerceros" runat="server" />Predio de terceros</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtPredioTercEmpresa" value="" maxlength="25" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_100">
            <label class="label" id="lblUbicacion" runat="server">Ubicación / Dirección de obra</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtUbicacion" value="" maxlength="44" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_100">
            <label class="label" id="lblProvincia" runat="server">Provincia</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtProvincia" value="" maxlength="44" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_100">
            <label class="label" id="lblRespTecCliente" runat="server">Responsable Técnico Cliente</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtRespTecCliente" value="" maxlength="44" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label" id="lblRespTecClienteTel" runat="server">[Teléfono]</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtRespTecClienteTel" value="" maxlength="15" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label" id="lblRespTecClienteEmail" runat="server">[E-mail]</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtRespTecClienteEmail" value="" maxlength="50" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_100">
            <label class="label" id="lblRespSegCliente" runat="server">Responsable Seguridad Cliente</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtRespSegCliente" value="" maxlength="44" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label" id="lblRespSegClienteTel" runat="server">[Teléfono]</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtRespSegClienteTel" value="" maxlength="15" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label" id="lblRespSegClienteEmail" runat="server">[E-mail]</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtRespSegClienteEmail" value="" maxlength="50" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_100">
            <label class="label" id="lblContAdminCliente" runat="server">Contacto Administrativo Cliente</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtContAdminCliente" value="" maxlength="44" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label" id="lblContAdminClienteTel" runat="server">[Teléfono]</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtContAdminClienteTel" value="" maxlength="15" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label" id="lblContAdminClienteEmail" runat="server">[E-mail]</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtContAdminClienteEmail" value="" maxlength="50" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label" id="lblFechaEstimada" runat="server">Fecha estimada de inicio de obra en sitio</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtFechaEstimada" readonly="readonly" value="" maxlength="10" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half form_floated_item_right">
            <label class="label" id="lblDuracion" runat="server">Duración estimada de obra en sitio</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtDuracion" readonly="readonly" value="" maxlength="44" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_half">
            <label class="label" id="lblFechaFinalizacion" runat="server">Fecha de finalización de obra</label>
            <div class="input_wrapper">
                <div class="cap">
                    <input id="txtFechaFinalizacion" readonly="readonly" value="" maxlength="10" type="text" runat="server"/>
                </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_100">
            <label class="label" id="lblDescripcionTareas" runat="server" for="txtNovedadesH">Descripción general de las tareas</label>
            <div class="textarea_wrapper"> 
	            <div class="top">
		            <div class="cap"></div>
	            </div>
	            <div class="inner">
		            <div class="cap">
			            <textarea id="txtDescripcionTareas" maxlength="1350" runat="server" onkeyup="return MaxLength(this)"></textarea>     
		            </div>
	            </div>
	            <div class="bottom">
		            <div class="cap"></div>
	            </div>
            </div>
        </li>
        <li class="form_floated_item form_floated_item_100">
            <label class="label" id="lblObjetivoProyecto" runat="server" for="txtNovedadesH">Objetivo del proyecto</label>
            <div class="textarea_wrapper"> 
	            <div class="top">
		            <div class="cap"></div>
	            </div>
	            <div class="inner">
		            <div class="cap">
			            <textarea id="txtObjetivoProyecto" maxlength="1350" runat="server" onkeyup="return MaxLength(this)"></textarea>     
		            </div>
	            </div>
	            <div class="bottom">
		            <div class="cap"></div>
	            </div>
            </div>
        </li>
    </ul>

    <% if(IO == null || this.PuedeEditar) { %>
    <div class="suggestion_message left" style="top:1300px;">La fecha de inicio debe establecerse con un mínimo de <%=InformacionObras.MinDiasAnticipo.ToString() %> días de anticipación.</div>
    <% } %>

    <table class="tbl editable listado" cellspacing="0" style="float:left; width:300px; margin-bottom:20px;">
        <thead>
            <tr>
                <td class="border_left border_right" id="lblPersonasMant" runat="server">Personal de mantenimiento</td>
            </tr>
        </thead>
        <tbody id="personas-mantenimiento">

        </tbody>
        <tfoot>
            <tr>
                <td class="border_left border_right">
                    <% if (IO == null || this.PuedeEditar) { %>
                    <a id="btnAgregarPersonaMant">Agregar persona</a>
                    <% } %>
                </td>
            </tr>
        </tfoot>
    </table>

    <table class="tbl editable listado" cellspacing="0" style="float:right; width:300px; margin-bottom:20px;">
        <thead>
            <tr>
                <td class="border_left border_right" id="lblPersonasObra" runat="server">Personal de obra</td>
            </tr>
        </thead>
        <tbody id="personas-obra">

        </tbody>
        <tfoot>
            <tr>
                <td class="border_left border_right">
                    <% if (IO == null || this.PuedeEditar) { %>
                    <a id="btnAgregarPersonaObra">Agregar persona</a>
                    <% } %>
                </td>
            </tr>
        </tfoot>
    </table>

    <table class="tbl editable listado" cellspacing="0" style="width:100%">
        <thead>
            <tr>
                <td class="border_left border_right" id="lblVehiculos" runat="server">Vehículos afectados</td>
            </tr>
        </thead>
        <tbody id="vehiculos-obra">

        </tbody>
        <tfoot>
            <tr>
                <td class="border_left border_right">
                    <% if (IO == null || this.PuedeEditar) { %>
                    <a id="btnAgregarVehiculo">Agregar vehículo</a>
                    <% } %>
                </td>
            </tr>
        </tfoot>
    </table>
    <br />

    <% if(IO != null) { %>
    <h3>Historial</h3>
    <table class="tbl listado editable" cellspacing="0" style="width:400px">
        <thead>
            <tr>
                <td class="border_left" style="width:200px">Fecha</td>
                <td class="border_right">Revisión Nº</td>
            </tr>
        </thead>
        <tbody id="historial-items">
            <td colspan="2" class="align-center">No hay revisiones disponibles.</td>
        </tbody>
        <tfoot>
            <tr>
                <td class="border_left border_right" colspan="2">
                    
                </td>
            </tr>
        </tfoot>
    </table>
    <% } %>

    <div class="form_buttons_container">
        <ul class="button_list">
            <% if(IO == null || PuedeEditar) { %>
                <li id="btnGuardar"><div class="btn primary_action_button button_100"><div class="cap"><span>Guardar</span></div></div></li>
                <% if (false) { %>
                <li id="btnCerrar"><div class="btn secondary_action_button button_100"><div class="cap"><span>Cerrar</span></div></div></li>
                <% } %>
            <% } %>
            <% if (IO != null) { %>
            <li id="btnImprimir"><div class="btn secondary_action_button button_100"><div class="cap"><span>Imprimir</span></div></div></li>
            <% } %>
            <li id="btnCancelar"><div class="btn secondary_action_button button_100"><div class="cap"><span>Cancelar</span></div></div></li>
        </ul>
    </div>
</div>

<div class="dialog_wrapper" style="width:600px" id="divAutorizacion">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3>Solicitud de autorización</h3></div></div></div>
    </div>

    <div class="dialog_content">
        <ul class="middle_form">
            <li class="form_floated_item form_floated_item_100">
                <label class="label" for="txtMotivoAutoriz">Motivo de la autorización:</label>
                <div class="textarea_wrapper"> 
	                <div class="top">
		                <div class="cap"></div>
	                </div>
	                <div class="inner">
		                <div class="cap">
			                <textarea id="txtMotivoAutoriz" maxlength="176" onkeyup="return MaxLength(this)"></textarea>     
		                </div>
	                </div>
	                <div class="bottom">
		                <div class="cap"></div>
	                </div>
                </div>
            </li>
        </ul>
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li id="btnAutorizAceptar"><div class="btn primary_action_button_small button_100"><div class="cap"><span>Aceptar</span></div></div></li>
            <li id="btnAutorizCancelar"><div class="btn secondary_action_button_small button_100"><div class="cap"><span>Cancelar</span></div></div></li>
        </ul>
    </div>
    
</div>
<div class="form_place">
    <div style="float:left;">FA-054 Rev.01 </div>
    <div style="float:right;">26/09/2012 </div>
</div>
</asp:Content>

