﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="sgim.aspx.cs" Inherits="calidad_sgim" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">

<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        $('#matriz-legal').click(function () {
            
        });
        $('#proyectos').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.SGI_BsAs_Proyectos) %>';
        });
        $('#propuestas-cambio').click(function () {
            location.href = 'propCambio.aspx';
        });
         $('#NNC').click(function () {
            location.href = 'ncAdmin.aspx';
        });
        $('#sgi').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.SGI) %>';
        });
        $('#medio-ambiente').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.MedioAmbiente) %>';
        });
        $('#seguridad-higiene').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.Seguridad) %>';
        });
        
        $('#da-seguridad-higiene').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.Seguridad) %>';
        });
        $('#recursos-humanos').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.RRHH) %>';
        });
        $('#documentos-externos').click(function () {
            MostrarVentana('documentos-externos-container');
        });
        
        $('#matriz-legal').click(function () {
            //MostrarVentana('matriz-legal-container');
			location.href = 'https://www.estrucplan.com.ar/legis/Index.asp';
        });
        $('.botonDialogoCerrar').click(function () {
            CerrarVentana();
        });

        $('#da-admin').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.SGI_BsAs_AdminFinanz) %>';
        });
        $('#da-deposito').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.SGI_BsAs_Deposito) %>';
        });
        $('#da-compras').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.SGI_BsAs_Compras) %>';
        });
        $('#da-desarrollo').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.SGI_BsAs_Desarrollo) %>';
        });
        $('#da-informatica').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.SGI_BsAs_Informatica) %>';
        });
        $('#da-ingenieria').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.SGI_BsAs_Ingenieria) %>';
        });
        $('#da-mantenimiento').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.SGI_BsAs_Mantenimiento) %>';
        });
        $('#da-obras').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.SGI_BsAs_Obras) %>';
        });
        $('#da-rrhh').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.DOC_RRHH) %>';
        });
        $('#rrhh').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.SGI_BsAs_RRHH) %>';
        });
         $('#da-medio-ambiente').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.DOC_MedioAmbiente) %>';
        });
        $('#da-seguridad-higiene').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.SGI_BsAs_Seguridad_Higiene) %>';
        });
        $('#da-ventas').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.SGI_BsAs_Ventas) %>';
        });
        $('#da-metrologia').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.SGI_BsAs_Metrologia) %>';
        });
         $('#da-sgi').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.DOC_SGI) %>';
        });
        $('#politica-sgi').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.SGI_Politica_SGI) %>';
        });
        $('#manual-sgi').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.SGI_Manual_SGI) %>';
        });
        $('#politica-alcohol-drogas').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.SGI_Politica_Alcohol_Drogas) %>';
        });
        $('#certificaciones').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.SGI_Certificaciones) %>';
        });
        $('#normas').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.SGI_Normas) %>';
        });
        $('#procedimientos-sgi').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.SGI_Procedimientos_SGI) %>';
        });
        $('#control-residuos').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.MA_ControlResiduos) %>';
        });
        $('#emergencias-ambientales').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.MA_Emergencias_Ambientales) %>';
        });
        $('#actuacion-derrames').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.MA_Actuacion_Derrames) %>';
        });
        $('#matriz').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.SEG_Matriz) %>';
        });
        $('#investigacion-incidentes').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.SEG_Investigacion_Incidentes) %>';
        });
        $('#epp').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.SEG_EPP) %>';
        });
        $('#seguridad-salud-operaciones').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.SEG_Seguridad_Salud_Operaciones) %>';
        });
        $('#plan-emergencias').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.SEG_Plan_Emergencias) %>';
        });
        $('#manual-empleado').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.RRHH_Manual_Empleado) %>';
        });
        $('#organigrama').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.RRHH_Organigrama) %>';
        });
        $('#registro-capacitacion').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.RRHH_Registro_Capacitacion) %>';
        });
        $('#lista-documentos-externos').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.DE_Lista_Doc_Ext) %>';
        });
        $('#documentos-ext').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.DE_Doc_Ext) %>';
        });
        
        $('#Matriz-Legal-Externo').click(function () {
            location.href = 'http://www.legislacion-ambiental.com/intranet/admin.php';
        });
        $('#Matriz-Legal-Interno').click(function () {
            location.href = '../general/repositorioArchivos.aspx?p=<%= Encriptacion.GetParametroEncriptado("idRepositorio=" + 
                    (int)RepositoriosArchivos.Mat_Leg_Int) %>';
        });

        $('#sgi-multisitio').click(function () {
            MostrarVentana('da-container');
        });
    });
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlacePage" Runat="Server">

<div id="map-sgim">
    <div class="link" id="sgi-multisitio"></div>
    <div class="link" id="matriz-legal"></div>
    <div class="link" id="proyectos"></div>
    <div class="link" id="propuestas-cambio"></div>
    <div class="link" id="NNC"></div>

    <div class="link rect" id="sgi"></div>
    <div class="link rect" id="medio-ambiente"></div>
    <div class="link rect" id="seguridad-higiene"></div>
    <div class="link rect" id="recursos-humanos"></div>
    <div class="link rect" id="documentos-externos"></div>
</div>

<div class="dialog_wrapper" style="width:600px" id="da-container">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3>Buenos Aires</h3></div></div></div>
    </div>

    <div class="dialog_content">
        <ul class="middle_form">
            <li class="form_floated_item form_floated_item_50 hand" id="da-admin">
                <img src="/images/sgim/administracion_finanzas.png" alt="Administración y finanzas" title="Administración y finanzas" />
            </li>
            <li class="form_floated_item form_floated_item_50 form_floated_item_right hand" id="da-ventas">
                <img src="/images/sgim/ventas.png" alt="Ventas" title="Ventas" />
            </li>
            <li class="form_floated_item form_floated_item_50 hand" id="da-compras">
                <img src="/images/sgim/compras.png" alt="Compras" title="Compras" />
            </li>
            <li class="form_floated_item form_floated_item_50 form_floated_item_right hand" id="da-deposito">
                <img src="/images/sgim/deposito.png" alt="Depósito" title="Depósito" />
            </li>
            <li class="form_floated_item form_floated_item_50 hand" id="da-desarrollo">
                <img src="/images/sgim/desarrollo.png" alt="Desarrollo" title="Desarrollo" />
            </li>
            <li class="form_floated_item form_floated_item_50 form_floated_item_right hand" id="da-informatica">
                <img src="/images/sgim/informatica.png" alt="Informática" title="Informática" />
            </li>
            <li class="form_floated_item form_floated_item_50 hand" id="da-ingenieria">
                <img src="/images/sgim/ingenieria.png" alt="Ingeniería" title="Ingeniería" />
            </li>
            <li class="form_floated_item form_floated_item_50 form_floated_item_right hand" id="da-mantenimiento">
                <img src="/images/sgim/mantenimiento.png" alt="Gas y Líquidos" title="Gas y Líquidos" />
            </li>
            <li class="form_floated_item form_floated_item_50 hand" id="da-medio-ambiente">
                <img src="/images/sgim/medioambiente.png" alt="Medio Ambiente" title="Medio Ambiente" />
            </li>
            <li class="form_floated_item form_floated_item_50 form_floated_item_right hand" id="da-metrologia">
                <img src="/images/sgim/metrologia.png" alt="Laboratorio" title="Laboratorio" />
            </li>
            <li class="form_floated_item form_floated_item_50 hand" id="da-obras">
                <img src="/images/sgim/obras.png" alt="Obras" title="Obras" />
            </li>     
            <li class="form_floated_item form_floated_item_50 form_floated_item_right hand" id="da-rrhh">
                <img src="/images/sgim/rrhh.png" alt="Recursos Humanos" title="Recursos Humanos" />
            </li>
            <li class="form_floated_item form_floated_item_50 hand" id="da-seguridad-higiene">
                <img src="/images/sgim/seh.png" alt="Seguridad e Higiene" title="Seguridad e Higiene" />
            </li>
            <li class="form_floated_item form_floated_item_50 form_floated_item_right hand" id="da-sgi">
                <img src="/images/sgim/sgi.png" alt="SGI" title="SGI" />
            </li>           
          </ul>

        <br />
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li class="botonDialogoCerrar"><div class="btn primary_action_button_small button_100"><div class="cap"><span>Aceptar</span></div></div></li>
        </ul>
    </div>
</div>

<div class="dialog_wrapper" style="width:600px" id="sgi-container">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3>SGI</h3></div></div></div>
    </div>

    <div class="dialog_content">
        <ul class="middle_form">
            <li class="form_floated_item form_floated_item_50 hand" id="politica-sgi">
                <img src="/images/sgim/politica_sgi.png" alt="Política SGI" title="Política SGI" />
            </li>
            <li class="form_floated_item form_floated_item_50 form_floated_item_right hand" id="manual-sgi">
                <img src="/images/sgim/manual_sgi.png" alt="Manual SGI" title="Manual SGI" />
            </li>
            <li class="form_floated_item form_floated_item_50 hand" id="politica-alcohol-drogas">
                <img src="/images/sgim/politica_alcohol_drogas.png" alt="Política Alcohol y Drogas" title="Política Alcohol y Drogas" />
            </li>
            <li class="form_floated_item form_floated_item_50 form_floated_item_right hand" id="certificaciones">
                <img src="/images/sgim/certificaciones.png" alt="Certificaciones" title="Certificaciones" />
            </li>
            <li class="form_floated_item form_floated_item_50 hand" id="normas">
                <img src="/images/sgim/normas.png" alt="Normas" title="Normas" />
            </li>
            <li class="form_floated_item form_floated_item_50 form_floated_item_right hand" id="procedimientos-sgi">
                <img src="/images/sgim/procedimientos_sgi.png" alt="Procedimientos SGI" title="Procedimientos SGI" />
            </li>
        </ul>

        <br />
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li class="botonDialogoCerrar"><div class="btn primary_action_button_small button_100"><div class="cap"><span>Aceptar</span></div></div></li>
        </ul>
    </div>
</div>

<div class="dialog_wrapper" style="width:600px" id="medio-ambiente-container">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3>Medio Ambiente</h3></div></div></div>
    </div>

    <div class="dialog_content">
        <ul class="middle_form">
            <li class="form_floated_item form_floated_item_50 hand" id="control-residuos">
                <img src="/images/sgim/control_residuos.png" alt="Control de residuos" title="Control de residuos" />
            </li>
            <li class="form_floated_item form_floated_item_50 form_floated_item_right hand" id="emergencias-ambientales">
                <img src="/images/sgim/emergencias_ambientales.png" alt="Emergencias Ambientales" title="Emergencias Ambientales" />
            </li>
            <li class="form_floated_item form_floated_item_100 hand" id="actuacion-derrames">
                <img src="/images/sgim/actuacion_derrames.png" alt="Actuación ante Derrames" title="Actuación ante Derrames" />
            </li>
        </ul>

        <br />
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li class="botonDialogoCerrar"><div class="btn primary_action_button_small button_100"><div class="cap"><span>Aceptar</span></div></div></li>
        </ul>
    </div>
</div>

<div class="dialog_wrapper" style="width:600px" id="seguridad-container">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3>Seguridad</h3></div></div></div>
    </div>

    <div class="dialog_content">
        <ul class="middle_form">
            <li class="form_floated_item form_floated_item_50 hand" id="matriz">
                <img src="/images/sgim/matriz.png" alt="Matriz de riesgos" title="Matriz de riesgos" />
            </li>
            <li class="form_floated_item form_floated_item_50 form_floated_item_right hand" id="investigacion-incidentes">
                <img src="/images/sgim/investigacion_incidentes.png" alt="Investigación de incidentes" title="Investigación de incidentes" />
            </li>
            <li class="form_floated_item form_floated_item_50 hand" id="epp">
                <img src="/images/sgim/equipo_proteccion_personal.png" alt="Equipo de Protección Personal" title="Equipo de Protección Personal" />
            </li>
            <li class="form_floated_item form_floated_item_50 form_floated_item_right hand" id="seguridad-salud-operaciones">
                <img src="/images/sgim/seguridad_salud_operaciones.png" alt="Seguridad y Salud en las Operaciones" title="Seguridad y Salud en las Operaciones" />
            </li>
            <li class="form_floated_item form_floated_item_100 hand" id="plan-emergencias">
                <img src="/images/sgim/plan_emergencias.png" alt="Plan de Emergencias" title="Plan de Emergencias" />
            </li>
        </ul>

        <br />
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li class="botonDialogoCerrar"><div class="btn primary_action_button_small button_100"><div class="cap"><span>Aceptar</span></div></div></li>
        </ul>
    </div>
</div>

<div class="dialog_wrapper" style="width:600px" id="rrhh-container">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3>Recursos Humanos</h3></div></div></div>
    </div>

    <div class="dialog_content">
        <ul class="middle_form">
            <li class="form_floated_item form_floated_item_50 hand" id="manual-empleado">
                <img src="/images/sgim/manual_empleado.png" alt="Manual del empleado" title="Manual del empleado" />
            </li>
            <li class="form_floated_item form_floated_item_50 form_floated_item_right hand" id="organigrama">
                <img src="/images/sgim/organigrama.png" alt="Organigrama" title="Organigrama" />
            </li>
            <li class="form_floated_item form_floated_item_50 hand" id="registro-capacitacion">
                <img src="/images/sgim/registro_capacitacion.png" alt="Registro de capacitación" title="Registro de capacitación" />
            </li>
        </ul>

        <br />
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li class="botonDialogoCerrar"><div class="btn primary_action_button_small button_100"><div class="cap"><span>Aceptar</span></div></div></li>
        </ul>
    </div>
</div>

<div class="dialog_wrapper" style="width:600px" id="documentos-externos-container">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3>Documentos externos</h3></div></div></div>
    </div>

    <div class="dialog_content">
        <ul class="middle_form">
            <li class="form_floated_item form_floated_item_50 hand" id="lista-documentos-externos">
                <img src="/images/sgim/lista_documentos_externos.png" alt="Lista de documentos externos" title="Lista de documentos externos" />
            </li>
            <li class="form_floated_item form_floated_item_50 form_floated_item_right hand" id="documentos-ext">
                <img src="/images/sgim/documentos_externos.png" alt="Documentos externos" title="Documentos externos" />
            </li>
        </ul>

        <br />
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li class="botonDialogoCerrar"><div class="btn primary_action_button_small button_100"><div class="cap"><span>Aceptar</span></div></div></li>
        </ul>
    </div>
</div>

<!-- test german --> 
<div class="dialog_wrapper" style="width:600px" id="matriz-legal-container">
    <div class="dialog_header">
        <div class="cap right"><div class="cap left"><div class="cap inner"><h3>Matriz Legal</h3></div></div></div>
    </div>

    <div class="dialog_content">
        <ul class="middle_form">
            <li class="form_floated_item form_floated_item_50 hand" id="Matriz-Legal-Externo">
                <img src="/images/sgim/matriz_externo.png" alt="Matriz de Requisito Legal Externo" title="Matriz de Requisito Legal Externo" />
            </li>
            <li class="form_floated_item form_floated_item_50 form_floated_item_right hand" id="Matriz-Legal-Interno">
                <img src="/images/sgim/matriz_interno.png" alt="Matriz de Requisito Legal Interno" title="Matriz de Requisito Legal Interno" />
            </li>
        </ul>

        <br />
    </div>

    <div class="dialog_footer">
        <ul class="button_list">
            <li class="botonDialogoCerrar"><div class="btn primary_action_button_small button_100"><div class="cap"><span>Aceptar</span></div></div></li>
        </ul>
    </div>
</div>

</asp:Content>