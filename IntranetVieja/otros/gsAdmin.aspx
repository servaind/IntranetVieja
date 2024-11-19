<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.master" AutoEventWireup="true" CodeFile="gsAdmin.aspx.cs" Inherits="otros_gsAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHead" Runat="Server">
    <link href="/css/gs.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript" src="../js/highcharts.js"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $('.participante').corner();

            <% if(MostrarResultados) { %>
            MostrarResultados();
            <% } %>
        });

        <% if(PuedeVotar) { %>
        function Votar(idParticipante) {
            MostrarLoading();

            ConsultaAjax.url = 'gsAdmin.aspx/Votar';
            ConsultaAjax.data = '{ "idParticipante": "' + idParticipante + '" }';
            ConsultaAjax.AjaxSuccess = function (msg) {
                Mensaje('El voto ha sido computado!', 'success', true, false, 'Aceptar', '', 'custom_dialog.close()', '');
                
                $('.votar').hide();
            };
            ConsultaAjax.AjaxError = function (msg) {
                ErrorMsg(msg);
            };

            ConsultaAjax.Ejecutar();
        }
        <% } %>

        <% if(MostrarResultados) { %>
        function MostrarResultados() {
            MostrarLoading();

            ConsultaAjax.url = 'gsAdmin.aspx/GetResultados';
            ConsultaAjax.AjaxSuccess = function (msg) {
                var participantes = [];
                var votos = [];

                var cant = msg.d.length;

                if (cant > 0) {
                    var i = 0;
                    for (i; i < cant; i++) {
                        participantes.push(msg.d[i][0]);
                        votos.push(msg.d[i][1]);
                    }
                }

                var chart2 = new Highcharts.Chart({
                    chart: {
                        renderTo: 'graphVotacion',
                        defaultSeriesType: 'column'
                    },
                    title: {
                        text: ''
                    },
                    xAxis: {
                        categories: participantes
                    },
                    yAxis: {
                        title: {
                            text: 'Votos'
                        }
                    },
                    tooltip: {
                        formatter: function () {
                            return '' +
								this.x + ': ' + this.y + ' votos';
                        }
                    },
                    plotOptions: {
                        column: {
                            pointPadding: 0.2,
                            borderWidth: 0
                        }
                    },
                    series: [{
                        name: 'Votos',
                        data: votos
                    }]
                });

                CerrarVentana();
            };
            ConsultaAjax.AjaxError = function (msg) {
                ErrorMsg(msg);
            };

            ConsultaAjax.Ejecutar();
        }
        <% } %>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlacePage" Runat="Server">

<div class="page-title">
    <h1>Gran Servaind</h1>
</div>

<div class="full-width">

<% if(!this.PuedeVotar && !this.MostrarResultados) { %>
    <div class="suggestion_message success">
        Ud ya ha votado. Espere al día viernes para visualizar los resultados.
    </div>
<% } %>
    <div id="participantes-container">
<%
    List<GSParticipante> participantes = GranServaind.GetParticipantes();
    foreach (GSParticipante participante in participantes) { 
%>
        <div class="participante">
            <div class="foto"><img src="/images/gs/<%= participante.Nombre.Replace(" ", "_") %>.png" alt="Test" /></div>
            <div class="nombre"><%= participante.Nombre %></div>
            <% if(this.PuedeVotar) { %>
            <div class="votar" onclick="javascript:Votar('<%=Encriptacion.Encriptar(participante.IdParticipante.ToString()) %>')">VOTAR</div>
            <% } %>
        </div>
<% } %>
    </div>
    <br />
<% if(this.MostrarResultados) { %>
    <h1>Resultados de la votación</h1>
    <div id="graphVotacion" style="width: 90%; height: 400px; margin: 0 auto"></div>
<% } %>
</div>

</asp:Content>

