using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de ITR
/// </summary>
public static class ITR
{
    public static string GetNombreITR(DateTime fecha, int imputacion, string usuario)
    {
        string result;

        result = String.Format("{0}.{1}.{2}.{3}", fecha.ToString("yyyy-MM-dd"), imputacion, usuario, "pdf");

        return result;
    }

    public static void EliminarITRTemp(DateTime fecha, int imputacion, string usuario)
    {
        try
        {
            File.Delete(Constantes.PATH_TEMP + GetNombreITR(fecha, imputacion, usuario));
        }
        catch
        {

        }
    }

    public static void EliminarITR(DateTime fecha, int imputacion, string usuario)
    {
        try
        {
            RepositorioArchivos rep = GRepositorioArchivos.GetRepositorioArchivos(RepositoriosArchivos.ITR);

            File.Delete(rep.CarpetaActual.Path + GetNombreITR(fecha, imputacion, usuario));
        }
        catch
        {

        }
    }

    public static string GetPathITR(DateTime fecha, int imputacion, string usuario)
    {
        RepositorioArchivos rep = GRepositorioArchivos.GetRepositorioArchivos(RepositoriosArchivos.ITR);

        return rep.CarpetaActual.Path + GetNombreITR(fecha, imputacion, usuario);
    }

    public static string GetPathITR(string fileITR)
    {
        RepositorioArchivos rep = GRepositorioArchivos.GetRepositorioArchivos(RepositoriosArchivos.ITR);

        return rep.CarpetaActual.Path + fileITR;
    }
}