<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ncImprimir.aspx.cs" Inherits="calidad_ncImprimir" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>SERVAIND ::: Sistemas Internos</title>
    <style type="text/css">
        body
        {
	        font-family: Verdana, Geneva, Arial, helvetica, sans-serif;
	        font-size:8pt;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
          <table width="650px" style="border:2px solid black;background-color:#FFFFFF;" cellspacing="0" cellpadding="1px">
             <tr>
                <td rowspan="3" style="border-right:2px solid black;border-bottom:2px solid black;text-align:center" width="50%">
                   <img src="/images/logo_servaind_nc.png" alt="Servaind S.A." />
                </td>
                <td style="padding-left:2px;font-size:8pt;font-weight:bold" width="50%">
                   NC. N&deg: <span style="font-weight:normal"><%= NC.Numero %></span>
                </td>
             </tr>
             <tr>
                <td>&nbsp;</td>
             </tr>
             <tr>
                <td style="border-bottom:2px solid black;padding-bottom:3px;padding-left:2px;font-size:8pt;font-weight:bold">
                   FECHA: <span style="font-weight:normal"><%= NC.FechaEmision.ToShortDateString() %></span>
                </td>
             </tr>
             <tr>
                <td style="border-bottom:2px solid black;padding-left:2px;background-color:#CCCCCC;text-align:center;font-weight:bold;height:25px;font-size:11pt" colspan="2">
                   NOTA DE NO CONFORMIDAD / OBSERVACION / OPORTUNIDAD DE MEJORA
                </td>
             </tr>
             <tr>
                <td style="padding-left:2px;font-size:8pt;font-weight:bold;border-right:2px solid black">
                   NORMA: <span style="font-weight:normal"><%=NC.NormaISO9001 ? "ISO9001 " : "" %><%=NC.NormaISO14001 ? "ISO14001 " : "" %><%=NC.NormaOHSAS18001 ? "OHSAS18001 " : "" %></span>
                </td>
                <td style="padding-left:2px;font-size:8pt;font-weight:bold;">
                   &Aacute;rea de Responsabilidad: 
                   <span style="font-weight:normal"><%= NC.Area.Descripcion %></span>
                </td>
                </tr>
                <tr>
                   <td style="padding-left:2px;font-size:8pt;font-weight:bold;border-right:2px solid black;border-bottom:2px solid black">
                      <%=NC.RevMatrizRiesgo ? "Requiere revisión de la Matriz de Riesgos" : "" %>
                   </td>
                   <td style="padding-left:2px;font-size:8pt;font-weight:bold;border-bottom:2px solid black">
                      &nbsp;
                   </td>
                </tr>
                <tr>
                   <td style="padding-left:2px;font-size:8pt;font-weight:bold;border-right:2px solid black">
                      APARTADO: <span style="font-weight:normal"><%= NC.Apartado %></span>
                   </td>
                   <td colspan="2" style="padding-left:2px;font-size:8pt;font-weight:bold;">ORIGEN: <span style="font-weight:normal">
                      <%= NC.Equipo %></span>
                   </td>
               </tr>
               <tr>
                  <td style="padding-left:2px;font-size:8pt;font-weight:bold;border-right:2px solid black">CATEGOR&Iacute;A: <span style="font-weight:normal">
                     <%= GNoConformidades.GetDescripcionCategoriaL(NC.Categoria) %></span>
                  </td>
                   <td style="padding-left:2px;font-size:8pt;font-weight:bold;">ASUNTO: <span style="font-weight:normal">
                      <%= NC.Asunto %></span>
                   </td>
               </tr>
               <tr>
                  <td style="padding-left:2px;font-size:8pt;font-weight:bold;border-right:2px solid black">
                     EMITIDA POR: <span style="font-weight:normal"><%= NC.EmitidaPor.Nombre %></span>
                  </td>
                  <td style="padding-left:2px;font-size:8pt;font-weight:bold;">
                     
                  </td>
               </tr>
               <tr>
                  <td style="padding-left:2px;padding-bottom:3px;font-size:8pt;font-weight:bold;border-right:2px solid black;border-bottom:2px solid black">
                     FIRMA <span style="font-weight:normal"><%= NC.EmitidaPor.Nombre %></span>
                  </td>
                  <td style="padding-left:2px;padding-bottom:3px;font-size:8pt;font-weight:bold;border-bottom:2px solid black">

                  </td>
               </tr>
               <tr>
                  <td colspan="2" style="padding-left:2px;font-size:8pt;font-weight:bold">
                     HALLAZGO:
                  </td>
               </tr>
               <tr>
                  <td colspan="2" style="padding-left:2px;font-size:8pt;font-weight:bold;vertical-align:text-top">
                     <span style="font-weight:normal;word-wrap:break-word"><%= NC.Hallazgo %></span>
                  </td>
               </tr>
               <tr>
                  <td style="border-top:2px solid black;border-bottom:2px solid black;padding-left:2px;background-color:#CCCCCC;text-align:center;font-weight:bold;height:25px;font-size:11pt" colspan="2">
                     ACCION PROPUESTA INMEDIATA
                  </td>
               </tr>
               <tr>
                  <td colspan="2" style="padding-left:2px;font-size:8pt;font-weight:bold;height:70px;vertical-align:text-top">
                     <span style="font-weight:normal;word-wrap:break-word"><%= NC.AccionInmediata %></span>
                  </td>
               </tr>
               <tr>
                  <td style="border-top:2px solid black;border-bottom:2px solid black;padding-left:2px;background-color:#CCCCCC;text-align:center;font-weight:bold;height:25px;font-size:11pt" colspan="2">
                     DEFINICI&Oacute;N DE LAS CAUSAS RAICES
                  </td>
               </tr>
               <tr>
                  <td colspan="2" style="padding-left:2px;font-size:8pt;font-weight:bold;height:70px;vertical-align:text-top">
                     <span style="font-weight:normal;word-wrap:break-word"><%= NC.CausasRaices %></span>
                  </td>
               </tr>
               <tr>
                  <td style="border-top:2px solid black;border-bottom:2px solid black;padding-left:2px;background-color:#CCCCCC;text-align:center;font-weight:bold;height:25px;font-size:11pt" colspan="2">
                     ACCI&Oacute;N CORRECTIVA Y PREVENTIVA
                  </td>
               </tr>
               <tr>
                  <td colspan="2" style="padding-left:2px;font-size:8pt;font-weight:bold;height:70px;vertical-align:text-top">
                     <span style="font-weight:normal"><%= NC.AccionCorrectiva %></span>
                  </td>
               </tr>
               <tr>
                  <td style="border-top:2px solid black;border-bottom:2px solid black;padding-left:2px;background-color:#CCCCCC;text-align:center;font-weight:bold;height:25px;font-size:11pt" colspan="2">
                     CIERRE
                  </td>
               </tr>
               <tr>
                  <td colspan="2" style="padding-left:2px;font-size:9pt;">
                     La acci&oacute;n correctiva relacionada con la no-conformidad ha sido revisada por Aseguramiento de la Calidad y la conclusi&oacute;n final es:&nbsp;
                  </td>
               </tr>
               <tr>
                  <td colspan="2" style="padding-left:2px;font-size:8pt;font-weight:bold;">
                     <table width="100%" cellspacing="0">
                        <tr>
                           <td>
                              <img alt="Satisfactoria" src="/images/icons/nc_opt_<%= NC.Conclusion == ConclusionesNC.Satisfactoria ? "y" : "n" %>.png" />
                              <span style="padding-top:5px;font-size:9pt;font-weight:bold">SATISFACTORIA</span>
                           </td>
                           <td>
                              <img alt="En proceso" src="/images/icons/nc_opt_<%= NC.Conclusion == ConclusionesNC.EnProceso ? "y" : "n" %>.png" />
                              <span style="padding-top:5px;font-size:9pt;font-weight:bold">EN PROCESO</span>
                           </td>
                           <td>
                              <img alt="No corresponde" src="/images/icons/nc_opt_<%= NC.Conclusion == ConclusionesNC.NoCorresponde ? "y" : "n" %>.png" />
                              <span style="font-size:9pt;font-weight:bold">NO CORRESPONDE</span>
                           </td>
                        </tr>
                     </table>
                  </td>
               </tr>
               <tr>
                  <td colspan="2" style="padding-left:2px;font-size:9pt;font-weight:bold;">&nbsp;
                  </td>
               </tr>
               <tr>
                  <td colspan="2" style="padding-left:2px;font-size:9pt;font-weight:bold;">COMENTARIOS: <span style="font-weight:normal">
                     <%= NC.Comentarios %></span>
                  </td>
               </tr>
               <tr>
                  <td colspan="2" style="padding-left:2px;font-size:9pt;font-weight:bold;">&nbsp;
                  </td>
               </tr>
               <tr>
                  <td colspan="2" style="padding-left:2px;font-size:9pt;font-weight:bold">NOMBRE Y FIRMA: <span style="font-weight:normal">
                     <%= NC.FirmaCierre.Nombre %></span>
                  </td>
               </tr>
               <tr>
                  <td colspan="2" style="padding-left:2px;padding-bottom:3px;font-size:9pt;font-weight:bold;">
                     FECHA: <span style="font-weight:normal"><%= NC.FechaCierre.ToShortDateString() %></span>
                  </td>
               </tr>
          </table>
        <div style="margin-top:10px;font-size:10px;width:650px;">
            <div style="float:left;width:48%;">FORM FG-005 REV 01</div>
            <div style="float:right;width:48%;text-align:right;">26/09/2012</div>
        </div>
        </div>
    </form>

    <script language="javascript" type="text/javascript">
        window.print();
        window.close();
    </script>
</body>
</html>
