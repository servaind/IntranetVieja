/// <summary>
/// Clase para la obtención de datos del Error.
/// </summary>
public class Error
{
    //Variables privadas.
    int errno = 0;
    System.DateTime fecha = System.DateTime.Now;
    string descripcion = null;
    string descripcionAdic = null;
    string stack = null;
    
    //Variables públicas.
    /// <summary>
    /// Obtiene el número de error.
    /// </summary>
    public int ErrNo
    {
        get
        {
            return errno;
        }
    }
    /// <summary>
    /// Obtiene la fecha.
    /// </summary>
    public System.DateTime Fecha
    {
        get
        {
            return fecha;
        }
    }
    /// <summary>
    /// Obtiene una descripción del error.
    /// </summary>
    public string Descripcion
    {
        get
        {
            return descripcion;
        }
    }
    /// <summary>
    /// Obtiene una descripción adicional del error.
    /// </summary>
    public string DescripcionAdicional
    {
        get
        {
            return descripcionAdic;
        }
    }
    /// <summary>
    /// Obtiene el stack.
    /// </summary>
    public string Stack
    {
        get
        {
            return stack;
        }
    }    


    /// <summary>
    /// Crea una estructura de Error.
    /// </summary>
    /// <param name="errno"></param>
    /// <param name="descripcion"></param>
    /// <param name="descripcionAdicional"></param>
    /// <param name="stack"></param>
	public Error(int errno, string descripcion, string descripcionAdicional, string stack)
	{
        this.errno = errno;
        this.descripcion = descripcion.Trim();
        this.descripcionAdic = descripcionAdicional.Trim();
        this.stack = stack.Trim();
	}
}
