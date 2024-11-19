<%@ Page Language="C#" AutoEventWireup="true" CodeFile="parteDiarioVer.aspx.cs" Inherits="general_parteDiarioVer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Servaind ::: Intranet</title>
    <link href="/css/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div id="content" style="border:none;">
        <form id="form1" runat="server">
                <div class="form_place">
                    <% if (this.PD != null) { %>
                    <ul class="middle_form" style="height:<%= this.PD.Estado == EstadosParteDiario.Presente ? "70" : "120" %>px">
                        <li class="form_floated_item form_floated_item_half">
                            <label class="label">Persona</label>
                            <span><%= this.PD.Persona.Nombre %></span>
                        </li>
                        <li class="form_floated_item form_floated_item_half form_floated_item_right">
                            <label class="label">Fecha del parte diario</label>
                            <span><%= this.PD.Fecha.ToShortDateString() %></span>
                        </li>
                        <% if (this.PD.Estado == EstadosParteDiario.Licencia) { %>
                        <li class="form_floated_item form_floated_item_half">
                            <label class="label">Observaciones</label>
                            <span><%= GetDescripcionLicencia() %></span>
                        </li>
                        <% } %>
                    </ul>

                    <% if (this.PD.Estado == EstadosParteDiario.Presente) { %>
                    <h3>Imputaciones</h3>

                        <% foreach(ImputacionParteDiario imputacion in this.PD.Imputaciones) { %>
                    <h4><%= imputacion.Imputacion.DescripcionCompleta %> - <%= imputacion.Horas.ToString() %> horas</h4>
                    <ul class="middle_form" style="height:120px">
                        <li class="form_floated_item form_floated_item_full">
                            <label class="label">Tareas realizadas</label>
                            <span><%= imputacion.TareasRealizadas %></span>
                        </li>
                        <li class="form_floated_item form_floated_item_full">
                            <label class="label">Tareas programadas</label>
                            <span><%= imputacion.TareasProgramadas.Length > 0 ? imputacion.TareasProgramadas : "-" %></span>
                        </li>
                        <li class="form_floated_item form_floated_item_full">
                            <label class="label">Novedades del vehículo</label>
                            <span><%= imputacion.NovedadesVehiculo.Length > 0 ? imputacion.NovedadesVehiculo : "-"%></span>
                        </li>
                        <li class="form_floated_item form_floated_item_full">
                            <label class="label">Novedades de la herramienta</label>
                            <span><%= imputacion.NovedadesHerramienta.Length > 0 ? imputacion.NovedadesHerramienta : "-"%></span>
                        </li>
                        <% if (imputacion.HayItr) { %>
                        <li class="form_floated_item form_floated_item_full">
                            <label class="label">Descargar ITR</label>
                            <span>Presione <a href="<%=Encriptacion.GetURLEncriptada("download.aspx", "f=" + ITR.GetPathITR(PD.Fecha, imputacion.Imputacion.Numero, PD.Persona.Usuario) + "&n=" + 
                ITR.GetNombreITR(PD.Fecha, imputacion.Imputacion.Numero, PD.Persona.Usuario)) %>">aquí</a> para descargar el ITR.</span>
                        </li>
                        <% } %>
                    </ul>
                            <% if (imputacion.PersonalIntervinieron.Count > 0 ) { %>
                    <h5>Personas que participaron</h5>
                    <ul class="middle_form">
                        <li class="form_floated_item form_floated_item_half">
                            <label class="label">Persona</label>
                        </li>
                        <li class="form_floated_item form_floated_item_half form_floated_item_right">
                            <label class="label">Horas</label>
                        </li>
                                <% foreach(PersonaInterviene persona in imputacion.PersonalIntervinieron) { %>
                        <li class="form_floated_item form_floated_item_half">
                            <span><%= persona.Persona.Nombre %></span>
                        </li>
                        <li class="form_floated_item form_floated_item_half form_floated_item_right">
                            <span><%= persona.Horas.ToString() %></span>
                        </li>
                                <% } %>
                    </ul>
                            <% } %>
                        <% } %>
                    <% } %>
                    <% } else { %>
                    <h1>No se ha encontrado un parte diario<br /> para la fecha seleccionada.</h1>
                    <% } %>
                </div>
        </form>
    </div>
</body>
</html>
