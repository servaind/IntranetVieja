
public class AdjuntoEmail
{
    // Variables.
    private string path;
    private string nombre;

    // Propiedades.
    public string Path
    {
        get { return this.path; }
    }
    public string Nombre
    {
        get { return this.nombre; }
    }


    public AdjuntoEmail(string path, string nombre)
    {
        this.path = path;
        this.nombre = nombre;
    }
}

