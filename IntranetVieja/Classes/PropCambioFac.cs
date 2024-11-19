using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

public static class PropCambioFac
{
    public static void Create(int sectorId, int responsableId, string cambioPropuesto, int urgenciaId, string adjuntoFilename,
        string adjuntoName)
    {
        if(String.IsNullOrWhiteSpace(cambioPropuesto)) throw new ArgumentException("Debe ingresar una descripción para el cambio propuesto.");

        string strPlantilla = Funciones.ObtenerPlantilla(Constantes.PATH_PLANTILLA_PROP_CAMBIO);
        if (strPlantilla == null)
        {
            throw new ArgumentException("Se produjo un error al enviar la propuesta. Código 01.");
        }

        Area area = GAreas.GetArea(sectorId);
        if(area == null) throw new ArgumentException("El Sector seleccionado no es válido.");

        Persona responsable = GPersonal.GetPersona(responsableId);
        if (responsable == null) throw new ArgumentException("El Responsable seleccionado no es válido.");

        if(!Enum.IsDefined(typeof(PropCambioUrgencia), urgenciaId)) throw new ArgumentException("El tipo de urgencia no es válido.");
        PropCambioUrgencia urgencia = (PropCambioUrgencia) urgenciaId;

        // Cargo los emails de SGI.
        List<ResponsableArea> respSGI = GAreas.GetResponsablesArea(9);
        if(respSGI == null || respSGI.Count == 0) throw new ArgumentException("No hay ningún responsable de SGI.");

        string asunto = "Propuesta de cambio";
        string de = Constantes.Usuario.Email;
        string para = Funciones.Concatenate(respSGI.Select(r => r.Responsable.Email).ToList(), ',');
        //string para = "martin.duran@servaind.com";
        string cc = Constantes.Usuario.Email;

        strPlantilla = strPlantilla.Replace("@USUARIO", Constantes.Usuario.Nombre);
        strPlantilla = strPlantilla.Replace("@SECTOR", area.Descripcion);
        strPlantilla = strPlantilla.Replace("@RESPONSABLE", responsable.Nombre);
        strPlantilla = strPlantilla.Replace("@CAMBIO_PROPUESTO", Funciones.ReemplazarEnters(cambioPropuesto));
        strPlantilla = strPlantilla.Replace("@URGENCIA", urgencia.GetDescription());

        List<AdjuntoEmail> adjuntos = new List<AdjuntoEmail>();
        if (!String.IsNullOrWhiteSpace(adjuntoFilename))
        {
            adjuntos.Add(new AdjuntoEmail(adjuntoFilename, adjuntoName));
        }

        Email email = new Email(de, para, cc, asunto, strPlantilla, adjuntos);

        if (!email.Enviar()) throw new EmailException();
    }

    public static string AdjuntoTempFile()
    {
        string result = String.Empty;

        string path = HttpContext.Current.Server.MapPath("/temp") + "\\";
        do
        {
            result = path + Guid.NewGuid();
        } while (File.Exists(path));

        return result;
    }
}