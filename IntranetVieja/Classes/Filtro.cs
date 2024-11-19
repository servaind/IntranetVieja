/*
 * Historial:
 * ===================================================================================
 * [27/05/2011]
 * - Versión estable.
 */

using System;

/// <summary>
/// Summary description for Filtro
/// </summary>
public class Filtro
{
    // Variables.
    private int tipo;
    private object valor;

    // Propiedades.
    public int Tipo
    {
        get { return this.tipo; }
    }
    public object Valor
    {
        get { return this.valor; }
    }


    /// <summary>
    /// Almacena un filtro.
    /// </summary>
	public Filtro(int tipo, object valor)
	{
        this.tipo = tipo;
        this.valor = valor;
	}
}