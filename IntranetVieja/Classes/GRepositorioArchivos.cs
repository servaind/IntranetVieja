/*
 * Historial:
 * ===================================================================================
 * [XX/10/2011]
 * - En vez de utilizar la base de datos, se basa en un archivo XML en cada carpeta.
 * [09/06/2011]
 * - Versión estable.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Web;
using System.Xml;


public class RepositorioArchivos
{
    // Variables.
    private string nombre;
    private CarpetaRepositorio root;
    private CarpetaRepositorio carpetaActual;

    // Propiedades.
    public string Nombre
    {
        get { return this.nombre; }
    }
    public CarpetaRepositorio Root
    {
        get { return this.root; }
    }
    public CarpetaRepositorio CarpetaActual
    {
        get 
        {
            if (this.carpetaActual == null)
            {
                this.carpetaActual = this.root;
            }

            return this.carpetaActual;
        }
    }
    public bool EstaEnRoot
    {
        get { return this.CarpetaActual.Path.Equals(this.Root.Path); }
    }


    internal RepositorioArchivos(string nombre, CarpetaRepositorio root)
    {
        this.nombre = nombre;
        this.root = root;
    }
    /// <summary>
    /// Abre la carpeta.
    /// </summary>
    public void AbrirCarpeta(string nombre)
    {
        CarpetaRepositorio carpeta = this.CarpetaActual.Subcarpetas.Find(c => c.Nombre.Equals(nombre));
        if (this.CarpetaActual.CarpetaPadre != null && this.CarpetaActual.CarpetaPadre.Nombre.Equals(nombre))
        {
            // Subo un nivel.
            carpeta = this.CarpetaActual.CarpetaPadre;
        }

        if (carpeta != null)
        {
            this.carpetaActual = carpeta;
        }
    }
    /// <summary>
    /// Se mueve un nivel arriba.
    /// </summary>
    public void SubirNivel()
    {
        if (!this.CarpetaActual.Path.Equals(this.Root.Path))
        {
            this.carpetaActual = this.CarpetaActual.CarpetaPadre;
        }
    }
    /// <summary>
    /// Se mueve al inicio.
    /// </summary>
    public void IrARoot()
    {
        this.carpetaActual = this.root;
    }
}
public class CarpetaRepositorio
{
    // Variables.
    private Dictionary<int, PermisosRDA> propiedades;
    private CarpetaRepositorio carpetaPadre;
    private List<CarpetaRepositorio> subcarpetas;
    private string path;
    
    // Propiedades.
    public Dictionary<int, PermisosRDA> Propiedades
    {
        get 
        {
            return this.propiedades; 
        }
    }
    public CarpetaRepositorio CarpetaPadre
    {
        get 
        {
            if (this.carpetaPadre == null)
            {
                DirectoryInfo padre = new DirectoryInfo(this.path);

                try
                {
                    this.carpetaPadre = GRepositorioArchivos.GetCarpetaRepositorio(padre.Parent.FullName);
                }
                catch
                {
                    this.carpetaPadre = null;
                }
            }

            return this.carpetaPadre;
        }
    }
    public List<CarpetaRepositorio> Subcarpetas
    {
        get 
        {
            if (this.subcarpetas == null)
            {
                this.subcarpetas = GRepositorioArchivos.GetSubcarpetas(this.Path);
            }

            return this.subcarpetas; 
        }
    }
    public string Path
    {
        get
        {
            return this.path;
        }
    }
    public string Nombre
    {
        get
        {
            string[] result = this.path.TrimEnd('\\').Split('\\');

            return result[result.Length - 1];
        }
    }


    /// <summary>
    /// Almacena la configuración para una carpeta del repositorio de archivos.
    /// </summary>
    internal CarpetaRepositorio(Dictionary<int, PermisosRDA> propiedades, string path)
    {
        this.propiedades = propiedades;
        this.subcarpetas = null;
        this.path = path.EndsWith("\\") ? path : path + "\\";
    }
    /// <summary>
    /// Obtiene los archivos de la carpeta.
    /// </summary>
    public FileInfo[] GetArchivos()
    {
        FileInfo[] result = new FileInfo[0];
        string fullPath = this.Path;
        string[] extensiones = Constantes.TiposArchivosRepositorio.Split(' ');

        try
        {
            DirectoryInfo dir = new DirectoryInfo(fullPath);
            var busqueda = dir.GetFiles("*.*").Where(s => extensiones.Contains(s.Extension.ToLower()));

            List<FileInfo> archivos = new List<FileInfo>();
            foreach (FileInfo a in busqueda)
            {
                archivos.Add(a);
            }

            result = archivos.ToArray();
        }
        catch
        {
            
        }

        return result;
    }
    /// <summary>
    /// Obtiene si el usuario actual tiene permisos sobre la carpeta.
    /// </summary>
    public bool TienePermiso(PermisosRDA permiso)
    {
        bool result = false;

        if (this.Propiedades.ContainsKey(Constantes.Usuario.ID))
        {
            PermisosRDA permisoUsuario = this.Propiedades[Constantes.Usuario.ID];
            switch (permiso)
            {
                case PermisosRDA.Lectura:
                    result = true;
                    break;
                case PermisosRDA.LecturaEscritura:
                    result = permisoUsuario == permiso;
                    break;
            }
        }
        else
        {
            result = GPermisosPersonal.TieneAcceso(PermisosPersona.Administrador);
        }

        return result;
    }
    /// <summary>
    /// Cambiar el path de la carpeta.
    /// </summary>
    internal void SetPath(string path)
    {
        this.path = path;
    }
    /// <summary>
    /// Establece los permisos de la carpeta.
    /// </summary>
    internal void SetPropiedades(Dictionary<int, PermisosRDA> propiedades)
    {
        this.propiedades = propiedades;
    }
    /// <summary>
    /// Agrega una subcarpeta.
    /// </summary>
    internal void AddSubcarpeta(CarpetaRepositorio carpeta)
    {
        this.subcarpetas.Add(carpeta);
    }
    /// <summary>
    /// Obtiene si el usuario actual puede editar la carpeta.
    /// </summary>
    public bool PuedeEditar()
    {
        return TienePermiso(PermisosRDA.LecturaEscritura);
    }
}
/// <summary>
/// Summary description for RepositorioArchivos
/// </summary>
public class GRepositorioArchivos
{
    /// <summary>
    /// Obtiene una carpeta del repositorio de archivos.
    /// </summary>
    internal static CarpetaRepositorio GetCarpetaRepositorio(string path)
    {
        CarpetaRepositorio result;

        try
        {
            Dictionary<int, PermisosRDA> propiedades = new Dictionary<int, PermisosRDA>();
            DirectorySecurity dirSecurity = Directory.GetAccessControl(path);
            AuthorizationRuleCollection reglas = dirSecurity.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount));
            foreach (FileSystemAccessRule regla in reglas)
            {
                string identityReference = regla.IdentityReference.Value.ToLower();
                if (identityReference.Equals(Constantes.Usuario.NombreDominio)
                    || identityReference.Equals("todos"))
                {
                    if ((regla.FileSystemRights & FileSystemRights.Read) == FileSystemRights.Read)
                    {
                        if (regla.AccessControlType == AccessControlType.Allow)
                        {
                            if ((regla.FileSystemRights & FileSystemRights.Write) == FileSystemRights.Write)
                            {
                                if (regla.AccessControlType == AccessControlType.Allow)
                                {
                                    propiedades.Add(Constantes.Usuario.ID, PermisosRDA.LecturaEscritura);
                                }
                                else
                                {
                                    propiedades.Add(Constantes.Usuario.ID, PermisosRDA.Lectura);
                                }
                            }
                            else
                            {
                                propiedades.Add(Constantes.Usuario.ID, PermisosRDA.Lectura);
                            }
                        }

                        break;
                    }
                }
            }

            result = new CarpetaRepositorio(propiedades, path);
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene el repositorio de archivos.
    /// </summary>
    public static RepositorioArchivos GetRepositorioArchivos(RepositoriosArchivos repositorio)
    {
        RepositorioArchivos result = null;
        CarpetaRepositorio root = null;

        switch (repositorio)
        {
            case RepositoriosArchivos.Comun:
                root = GetCarpetaRepositorio(@"\\10.0.0.15\Repositorio");
                break;
            case RepositoriosArchivos.SGI_Multisitio:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\a.SGI");
                break;
            case RepositoriosArchivos.SGI_Gas:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\GAS");
                break;
            case RepositoriosArchivos.SGI_Liquidos:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\LIQUIDOS");
                break;
            case RepositoriosArchivos.SGI_Valvulas:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\VÁLVULAS");
                break;
            case RepositoriosArchivos.SGI_Bolivia:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\BOLIVIA");
                break;
            case RepositoriosArchivos.Petrobras:
                root = GetCarpetaRepositorio(@"\\10.0.0.15\Repositorio\Publico\petrobras");
                break;
            case RepositoriosArchivos.Manual_SGI:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\Manual de SGI");
                break;
            case RepositoriosArchivos.Politica_SGI:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\Política SGI");
                break;
            case RepositoriosArchivos.Politica_Alcohol_Drogas:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\Política Alcohol y drogas");
                break;
            case RepositoriosArchivos.Certificaciones:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\Certificaciones");
                break;
            case RepositoriosArchivos.SGI_BsAs_AdminFinanz:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\f. DOCUMENTACION AREAS\j.ADM. Y FINANZAS");
                break;
            case RepositoriosArchivos.SGI_BsAs_Compras:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\f. DOCUMENTACION AREAS\f.Compras");
                break;
            case RepositoriosArchivos.SGI_BsAs_Desarrollo:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\f. DOCUMENTACION AREAS\e.Desarrollo");
                break;
            case RepositoriosArchivos.SGI_BsAs_Deposito:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\f. DOCUMENTACION AREAS\K.DEPÓSITO");
                break;
            case RepositoriosArchivos.SGI_BsAs_Informatica:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\f. DOCUMENTACION AREAS\i.Informatica");
                break;
            case RepositoriosArchivos.SGI_BsAs_Ingenieria:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\f. DOCUMENTACION AREAS\c.Ingeniería");
                break;
            case RepositoriosArchivos.SGI_BsAs_Mantenimiento:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\f. DOCUMENTACION AREAS\d.GAS Y LÍQUIDOS");
                break;
            case RepositoriosArchivos.SGI_BsAs_Obras:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\f. DOCUMENTACION AREAS\h.Obras");
                break;
            case RepositoriosArchivos.SGI_BsAs_Metrologia:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\f. DOCUMENTACION AREAS\l.LABORATORIO");
                break;
            case RepositoriosArchivos.SGI_BsAs_Organigramas:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\f. DOCUMENTACION AREAS\Organigrama");
                break;
            case RepositoriosArchivos.SGI_BsAs_Proyectos:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\i. Proyectos");
                break;
            case RepositoriosArchivos.SGI_BsAs_RRHH:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\f. DOCUMENTACION AREAS\a.RRHH");
                break;
            case RepositoriosArchivos.SGI_BsAs_Seguridad_Higiene:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\f. DOCUMENTACION AREAS\g.SEGURIDAD E HIGIENE");
                break;
            case RepositoriosArchivos.SGI_BsAs_Ventas:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\f. DOCUMENTACION AREAS\b.COMERCIAL");
                break;
            case RepositoriosArchivos.SGI_Politica_SGI:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\a.SGI\a. Políticas\POLÍTICA SGI");
                break;
            case RepositoriosArchivos.SGI_Manual_SGI:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\a.SGI\b. Manual de SGI");
                break;
            case RepositoriosArchivos.SGI_Certificaciones:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\a.SGI\d. Certificaciones");
                break;
            case RepositoriosArchivos.SGI_Normas:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\a.SGI\e. Normas");
                break;
            case RepositoriosArchivos.SGI_Procedimientos_SGI:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\a.SGI\f. Procedimientos SGI");
                break;
            case RepositoriosArchivos.SGI_Politica_Alcohol_Drogas:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\a.SGI\a. Políticas\POLÍTICA ALCOHOL Y DROGAS");
                break;
            case RepositoriosArchivos.MA_ControlResiduos:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\b.Medio Ambiente\MANEJO DE RESIDUOS");
                break;
            case RepositoriosArchivos.MA_Emergencias_Ambientales:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\b.Medio Ambiente\EMERGENCIAS AMBIENTALES");
                break;
            case RepositoriosArchivos.MA_Actuacion_Derrames:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\b.Medio Ambiente\ACTUACIÓN ANTE DERRAMES");
                break;
            case RepositoriosArchivos.SEG_Matriz:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\c.Seguridad\MATRIZ DE RIESGOS");
                break;
            case RepositoriosArchivos.SEG_Investigacion_Incidentes:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\c.Seguridad\INVESTIGACIÓN DE INCIDENTES");
                break;
            case RepositoriosArchivos.SEG_EPP:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\c.Seguridad\EPP");
                break;
            case RepositoriosArchivos.SEG_Plan_Emergencias:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\c.Seguridad\PLAN DE EMERGENCIAS");
                break;
            case RepositoriosArchivos.SEG_Seguridad_Salud_Operaciones:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\c.Seguridad\SEGURIDAD Y SALUD EN LAS OPERACIONES");
                break;
            case RepositoriosArchivos.RRHH_Manual_Empleado:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\d.RRHH\Manual del Empleado");
                break;
            case RepositoriosArchivos.RRHH_Organigrama:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\d.RRHH\Organigrama");
                break;
            case RepositoriosArchivos.RRHH_Registro_Capacitacion:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\d.RRHH\Capacitaciones");
                break;
            case RepositoriosArchivos.DE_Lista_Doc_Ext:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\e.Doc Externos\Lista\");
                break;
            case RepositoriosArchivos.DE_Doc_Ext:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\e.Doc Externos\Documentos");
                break;
            case RepositoriosArchivos.ITR:
                root = GetCarpetaRepositorio(@"\\10.0.0.15\Repositorio\ITR");
                break;
            case RepositoriosArchivos.SGI:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\a.SGI");
                break;
            case RepositoriosArchivos.DOC_SGI:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\f. DOCUMENTACION AREAS\m.SGI");
                break;
            case RepositoriosArchivos.DOC_MedioAmbiente:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\f. DOCUMENTACION AREAS\o.MEDIO AMBIENTE");
                break;
            case RepositoriosArchivos.DOC_RRHH:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\f. DOCUMENTACION AREAS\a.RRHH");
                break;
            case RepositoriosArchivos.MedioAmbiente:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\b.Medio ambiente");
                break;
            case RepositoriosArchivos.Seguridad:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\c.Seguridad");
                break;
            case RepositoriosArchivos.RRHH:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\d.RRHH");
                break;
            case RepositoriosArchivos.Mat_Leg_Int:
                root = GetCarpetaRepositorio(@"\\10.0.0.4\Calidad\h. MATRIZ LEGAL INTERNA");
                break;    

            default:
                root = null;
                break;
        }
        if (root != null)
        {
            result = new RepositorioArchivos(GetRepositorios()[(int)repositorio], root);
        }

        return result;
    }
    /// <summary>
    /// Obtiene las subcarpetas de la carpeta.
    /// </summary>
    internal static List<CarpetaRepositorio> GetSubcarpetas(string path)
    {
        List<CarpetaRepositorio> result = new List<CarpetaRepositorio>();

        try
        {
            DirectoryInfo carpeta = new DirectoryInfo(path);
            foreach (DirectoryInfo subcarpeta in carpeta.GetDirectories())
            {
                CarpetaRepositorio c = GetCarpetaRepositorio(subcarpeta.FullName);
                if (c.TienePermiso(PermisosRDA.Lectura))
                {
                    result.Add(c);
                }
            }
        }
        catch
        {

        }

        return result;
    }
    /// <summary>
    /// Actualiza las propiedades de una carpeta.
    /// </summary>
    public static void ActualizarCarpeta(CarpetaRepositorio carpeta, string nombre)
    {
        if (!carpeta.TienePermiso(PermisosRDA.LecturaEscritura))
        {
            throw new ErrorOperacionException();
        }

        string pathOriginal = carpeta.Path.TrimEnd('\\');
        string pathNuevo = "";
        string[] aux = pathOriginal.Split('\\');
        int c = aux.Length;
        for (int i = 0; i < (c - 1); i++)
        {
            pathNuevo += aux[i] + "\\";
        }
        pathNuevo += nombre;

        try
        {
            // Renombro la carpeta.
            if (!pathOriginal.Equals(pathNuevo))
            {
                Directory.Move(pathOriginal, pathNuevo);
            }
        }
        catch
        {
            throw new ErrorOperacionException();
        }
    }
    /// <summary>
    /// Obtiene la descripción para un tipo de archivo.
    /// </summary>
    public static string GetDescripcionTipoArchivo(string extension)
    {
        extension = extension.ToLower();

        if (extension.Equals("txt")) return "Archivo de texto";
        else if (extension.Equals("jpg")) return "Archivo de imagen";
        else if (extension.Equals("png")) return "Archivo de imagen";
        else if (extension.Equals("gif")) return "Archivo de imagen";
        else if (extension.Equals("zip")) return "Archivo comprimido";
        else if (extension.Equals("rar")) return "Archivo comprimido";
        else if (extension.Equals("iso")) return "Imagen de CD / DVD";
        else if (extension.Equals("xls")) return "Documento de Microsoft Excel";
        else if (extension.Equals("xlsx")) return "Documento de Microsoft Excel";
        else if (extension.Equals("doc")) return "Documento de Microsoft Word";
        else if (extension.Equals("docx")) return "Documento de Microsoft Word";
        else if (extension.Equals("pdf")) return "Documento PDF";
        else if (extension.Equals("exe")) return "Aplicación";
        else return "Desconocido";
    }
    /// <summary>
    /// Crea una carpeta.
    /// </summary>
    public static void CrearCarpeta(CarpetaRepositorio carpetaPadre, string nombre)
    {
        if (carpetaPadre == null || nombre.Length == 0)
        {
            throw new DatosInvalidosException();
        }

        if (!carpetaPadre.TienePermiso(PermisosRDA.LecturaEscritura))
        {
            throw new ErrorOperacionException();
        }

        try
        {
            // Creo físicamente la carpeta.
            string fullPath = carpetaPadre.Path + nombre;
            Directory.CreateDirectory(fullPath);

            // Agrego la carpeta al árbol virtual.
            CarpetaRepositorio carpeta = GetCarpetaRepositorio(fullPath);
            carpetaPadre.AddSubcarpeta(carpeta);
        }
        catch
        {
            throw new ErrorOperacionException();
        }
    }
    /// <summary>
    /// Elimina un archivo.
    /// </summary>
    public static void EliminarArchivo(CarpetaRepositorio carpeta, string nombreArchivo)
    {
        if (carpeta == null || String.IsNullOrEmpty(nombreArchivo))
        {
            throw new DatosInvalidosException();
        }

        if (!carpeta.TienePermiso(PermisosRDA.LecturaEscritura))
        {
            throw new ErrorOperacionException();
        }

        try
        {
            string fullPath = carpeta.Path + nombreArchivo;
            File.Delete(fullPath);
        }
        catch
        {
            throw new ErrorOperacionException();
        }
    }
    /// <summary>
    /// Obtiene los tipos acceso de las personas para las carpetas.
    /// </summary>
    public static Dictionary<int, string> GetPermisosPersona()
    {
        Dictionary<int, string> result = new Dictionary<int, string>();

        result.Add((int)PermisosRDA.Lectura, "Lectura");
        result.Add((int)PermisosRDA.LecturaEscritura, "Lectura / Escritura");

        return result;
    }
    /// <summary>
    /// Obtiene los repositorios de archivos disponibles.
    /// </summary>
    public static Dictionary<int, string> GetRepositorios()
    {
        Dictionary<int, string> result = new Dictionary<int, string>();

        if (!Constantes.EsUsuarioPublico)
        {
            result.Add((int)RepositoriosArchivos.Comun, "Repositorio común");
            result.Add((int)RepositoriosArchivos.SGI_Multisitio, "Repositorio SGI: Multisitio");
            result.Add((int)RepositoriosArchivos.SGI_Gas, "Repositorio SGI: Gas");
            result.Add((int)RepositoriosArchivos.SGI_Liquidos, "Repositorio SGI: Líquidos");
            result.Add((int)RepositoriosArchivos.SGI_Valvulas, "Repositorio SGI: Válvulas");
            result.Add((int)RepositoriosArchivos.SGI_Bolivia, "Repositorio SGI: Bolivia");
            result.Add((int)RepositoriosArchivos.Manual_SGI, "Repositorio SGI: Manual de SGI");
            result.Add((int)RepositoriosArchivos.Politica_SGI, "Repositorio SGI: Política de SGI");
            result.Add((int)RepositoriosArchivos.Politica_Alcohol_Drogas, "Repositorio SGI: Política, alcohol y drogas");
            result.Add((int)RepositoriosArchivos.Certificaciones, "Repositorio SGI: Certificaciones");
            result.Add((int)RepositoriosArchivos.SGI_BsAs_AdminFinanz, "Repositorio SGI: BsAs - Administración y finanzas");
            result.Add((int)RepositoriosArchivos.SGI_BsAs_Compras, "Repositorio SGI: BsAs - Compras");
            result.Add((int)RepositoriosArchivos.SGI_BsAs_Deposito, "Repositorio SGI: BsAs - Depósito");
            result.Add((int)RepositoriosArchivos.SGI_BsAs_Desarrollo, "Repositorio SGI: BsAs - Desarrollo");
            result.Add((int)RepositoriosArchivos.SGI_BsAs_Informatica, "Repositorio SGI: BsAs - Informática");
            result.Add((int)RepositoriosArchivos.SGI_BsAs_Ingenieria, "Repositorio SGI: BsAs - Ingeniería");
            result.Add((int)RepositoriosArchivos.SGI_BsAs_Mantenimiento, "Repositorio SGI: BsAs - Gas y Líquidos");
            result.Add((int)RepositoriosArchivos.SGI_BsAs_Obras, "Repositorio SGI: BsAs - Obras");
            result.Add((int)RepositoriosArchivos.SGI_BsAs_RRHH, "Repositorio SGI: BsAs - Recursos Humanos");
            result.Add((int)RepositoriosArchivos.SGI_BsAs_Seguridad_Higiene, "Repositorio SGI: BsAs - Seguridad e Higiene");
            result.Add((int)RepositoriosArchivos.SGI_BsAs_Ventas, "Repositorio SGI: BsAs - Ventas");
            result.Add((int)RepositoriosArchivos.SGI_BsAs_Metrologia, "Repositorio SGI: BsAs - Laboratorio");
            result.Add((int)RepositoriosArchivos.SGI_BsAs_Organigramas, "Repositorio SGI: BsAs - Organigramas");
            result.Add((int)RepositoriosArchivos.SGI_BsAs_Proyectos, "Repositorio SGI: BsAs - Proyectos");
            result.Add((int)RepositoriosArchivos.SGI_Politica_SGI, "SGI: Política SGI");
            result.Add((int)RepositoriosArchivos.DE_Lista_Doc_Ext, "Documentos externos: Lista");
            result.Add((int)RepositoriosArchivos.DE_Doc_Ext, "Documentos externos: Documentos");
            result.Add((int)RepositoriosArchivos.ITR, "Repositorio de ITR");
            result.Add((int)RepositoriosArchivos.SGI, "Repositorio de SGI");
            result.Add((int)RepositoriosArchivos.MedioAmbiente, "Repositorio de Medio Ambiente");
            result.Add((int)RepositoriosArchivos.Seguridad, "Repositorio de Seguridad e Higiene");
            result.Add((int)RepositoriosArchivos.RRHH, "Repositorio de Recursos Humanos");
            result.Add((int)RepositoriosArchivos.Mat_Leg_Int, "Matriz Legal Interna");
			result.Add((int)RepositoriosArchivos.DOC_SGI, "Documentación SGI");
			result.Add((int)RepositoriosArchivos.DOC_MedioAmbiente, "Documentación Medio Ambiente");
			result.Add((int)RepositoriosArchivos.DOC_RRHH, "Documentación Recursos Humanos");
        }
        else
        {
            if (Constantes.RepositoriosPublicos.ContainsKey(Constantes.Usuario.ID))
            {
                RepositoriosArchivos[] repositorios = Constantes.RepositoriosPublicos[Constantes.Usuario.ID];
                foreach (RepositoriosArchivos r in repositorios)
                {
                    string nombre = "";
                    switch (r)
                    {
                        case RepositoriosArchivos.Petrobras:
                            nombre = "Repositorio Petrobras";
                            break;
                    }

                    result.Add((int)r, nombre);
                }
                
            }
        }

        return result;
    }
}