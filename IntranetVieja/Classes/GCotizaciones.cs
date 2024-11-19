/*
 * Historial:
 * ===================================================================================
 * [26/05/2011]
 * - Versión estable.
 */

using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Office.Interop.Excel;
using ExcelApp = Microsoft.Office.Interop.Excel.Application;
using System.Reflection;
using ClosedXML.Excel;

public class ItemCotizacion : IComparable
{
    // Variables.
    private ArticuloTango item;
    private float cantidad;
    private float precio;
    private float precioDesc;

    // Properties.
    /// <summary>
    /// Obtiene o establece el artículo asociado.
    /// </summary>
    public ArticuloTango Articulo
    {
        get { return this.item; }
        set { this.item = value; }
    }
    /// <summary>
    /// Obtiene o establece la cantidad.
    /// </summary>
    public float Cantidad
    {
        get { return this.cantidad; }
        set { this.cantidad = value; }
    }
    /// <summary>
    /// Obtiene o establece el precio del artículo.
    /// </summary>
    public float Precio
    {
        get { return this.precio; }
        set { this.precio = value; }
    }
    /// <summary>
    /// Obtiene o establece el precio con descuento.
    /// </summary>
    public float PrecioDescuento
    {
        get { return this.precioDesc; }
        set { this.precioDesc = value; }
    }
    

    /// <summary>
    /// Almacena un ítem.
    /// </summary>
    internal ItemCotizacion(ArticuloTango item, float cantidad, float precio, float precioDesc)
    {
        this.item = item;
        this.cantidad = cantidad;
        this.precio = precio;
        this.precioDesc = precioDesc;
    }

    #region Miembros de IComparable

    public int CompareTo(object obj)
    {
        ItemCotizacion it = (ItemCotizacion)obj;
        return this.Articulo.Descripcion.CompareTo(it.Articulo.Descripcion);
    }

    #endregion
}

public class Cotizacion
{
    // Variables.
    private List<ItemCotizacion> lstItems;

    // Propiedades.
    public List<ItemCotizacion> Items
    {
        get
        {
            lstItems.Sort();
            return lstItems;
        }
    }
    /// <summary>
    /// Obtiene el valor total de la cotización.
    /// </summary>
    public float Total
    {
        get
        {
            float total = 0f;
            foreach (ItemCotizacion item in lstItems)
            {
                total += item.Cantidad * item.Precio;
            }

            return total;
        }
    }
    /// <summary>
    /// Obtiene el valor total con descuento de la cotización.
    /// </summary>
    public float TotalConDescuento
    {
        get
        {
            float total = 0f;
            foreach (ItemCotizacion item in lstItems)
            {
                total += item.Cantidad * item.PrecioDescuento;
            }

            return total;
        }
    }
    /// <summary>
    /// Obtiene un ítem de la cotización.
    /// </summary>
    public ItemCotizacion this[string codigo]
    {
        get
        {
            return BuscarItem(codigo);
        }
    }


    /// <summary>
    /// Arma una nueva cotización.
    /// </summary>
    public Cotizacion() : this(new List<ItemCotizacion>())
    {

    }
    /// <summary>
    /// Arma una nueva cotización.
    /// </summary>
    public Cotizacion(List<ItemCotizacion> lstItems)
    {
        this.lstItems = lstItems;
    }
    /// <summary>
    /// Agrega un ítem. Si el ítem existe, le suma la cantidad.
    /// </summary>
    public void AgregarItem(string codigo, float cantidad, float precio, float precioDesc)
    {
        bool existe = false;
        foreach (ItemCotizacion i in this.lstItems)
        {
            if (i.Articulo.Codigo == codigo)
            {
                i.Cantidad += cantidad;
                existe = true;
                break;
            }
        }

        if (!existe)
        {
            ItemCotizacion item = new ItemCotizacion(GArticuloTango.GetArticuloTango(codigo.ToString()), cantidad, precio, 
                precioDesc);
            item.Articulo.CargarDetalleVenta();

            this.lstItems.Add(item);
        }
    }
    /// <summary>
    /// Agrega un ítem a la cotización.
    /// </summary>
    public void AgregarItem(ItemCotizacion item)
    {
        bool existe = false;
        foreach (ItemCotizacion i in this.lstItems)
        {
            if (i.Articulo.Codigo == item.Articulo.Codigo)
            {
                i.Cantidad += item.Cantidad;
                existe = true;
                break;
            }
        }

        if (!existe)
        {
            this.lstItems.Add(item);
        }
    }
    /// <summary>
    /// Actualiza un ítem.
    /// </summary>
    public void ActualizarItem(string codigo, string nuevoCodigo, float cantidad, float precio, float precioDesc)
    {
        foreach (ItemCotizacion i in this.lstItems)
        {
            if (i.Articulo.Codigo == codigo)
            {
                i.Articulo = GArticuloTango.GetArticuloTango(nuevoCodigo);
                i.Articulo.CargarDetalleVenta();
                i.Cantidad = cantidad;
                i.Precio = precio;
                i.PrecioDescuento = precioDesc;
                break;
            }
        }
    }
    /// <summary>
    /// Borra un ítem.
    /// </summary>
    public void BorrarItem(ArticuloTango articulo)
    {
        BorrarItem(articulo.Codigo);
    }
    /// <summary>
    /// Borra un ítem.
    /// </summary>
    public void BorrarItem(string codigo)
    {
        ItemCotizacion item = null;
        foreach (ItemCotizacion i in this.lstItems)
        {
            if (i.Articulo.Codigo == codigo)
            {
                item = i;
                break;
            }
        }

        if (item != null)
        {
            this.lstItems.Remove(item);
        }
    }
    /// <summary>
    /// Borra los ítems de la cotización.
    /// </summary>
    public void BorrarItems()
    {
        this.lstItems.Clear();
    }
    /// <summary>
    /// Busca un ítem en la lista.
    /// </summary>
    private ItemCotizacion BuscarItem(string codigo)
    {
        ItemCotizacion result = null;

        foreach (ItemCotizacion item in this.lstItems)
        {
            if (item.Articulo.Codigo.Equals(codigo))
            {
                result = item;
                break;
            }
        }

        return result;
    }
}

/// <summary>
/// Descripción breve de GCotizaciones
/// </summary>
public class GCotizaciones
{
    public static string ExportarAExcel(Cotizacion cotizacion)
    { 
		string path = Constantes.PATH_TEMP + "Cotización.xlsx";
       
		var workbook = new XLWorkbook();
		var sheet1 = workbook.Worksheets.Add("Cotización");
		
		sheet1.Cell("A1").Value = "COTIZACIÓN";
		sheet1.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
		sheet1.Cell("A1").Style.Font.Bold = true;
		sheet1.Cell("A1").Style.Font.FontColor = XLColor.White;  
		sheet1.Cell("A1").Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
		
		sheet1.Range("A1", "N1").Merge();	
				
		for(int c = 1; c <= 11; c++)
		{
			var cell = sheet1.Cell(3, c);
     		cell.Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);              
		}
							
		sheet1.Cell("A4").Value = "Item";
		sheet1.Cell("A4").Style.Font.Bold = true;
		sheet1.Cell("A4").Style.Font.FontColor = XLColor.White;  
		sheet1.Cell("A4").Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
		
        sheet1.Cell("B4").Value = "Código";
        sheet1.Cell("B4").Style.Font.Bold = true;
		sheet1.Cell("B4").Style.Font.FontColor = XLColor.White;  
		sheet1.Cell("B4").Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
				
		sheet1.Cell("C4").Value = "Descripción";
		sheet1.Cell("C4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        sheet1.Cell("C4").Style.Font.Bold = true;
		sheet1.Cell("C4").Style.Font.FontColor = XLColor.White;  
		sheet1.Cell("C4").Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
		
		sheet1.Range("C4", "F4").Merge();	
		
		sheet1.Cell("G4").Value = "Cantidad";
        sheet1.Cell("H4").Value = "Precio";
        sheet1.Cell("I4").Value = "Precio c/desc";
        sheet1.Cell("J4").Value = "Total";
        sheet1.Cell("K4").Value = "Total c/desc";
    	
	    for(int c = 7; c <= 11; c++)
		{
			var cell = sheet1.Cell(4, c);
			cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
    		cell.Style.Font.Bold = true;
			cell.Style.Font.FontColor = XLColor.White;
     		cell.Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);	              
		} 
	
		sheet1.Cell("L3").Value = "Detalles de la última compra";
		sheet1.Cell("L3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
		sheet1.Cell("L3").Style.Font.Bold = true;
		sheet1.Cell("L3").Style.Font.FontColor = XLColor.White;  
		sheet1.Cell("L3").Style.Fill.BackgroundColor = XLColor.FromArgb(0, 176, 80);
		
		sheet1.Range("L3", "N3").Merge();
			
		sheet1.Cell("L4").Value = "OC";
        sheet1.Cell("M4").Value = "Fecha";
        sheet1.Cell("N4").Value = "Proveedor";			
				
		for(int c = 12; c <= 14; c++)
		{
			var cell = sheet1.Cell(4, c);
			cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
    		cell.Style.Font.Bold = true;
			cell.Style.Font.FontColor = XLColor.White;
     		cell.Style.Fill.BackgroundColor = XLColor.FromArgb(0, 176, 80);	              
		}
			
		const int FilaInicio = 5;
		int fila = FilaInicio;
		bool celda_con_color = false;
		int total = cotizacion.Items.Count;
       
		for (int x = 0; x < total; fila++, x++, celda_con_color = !celda_con_color)
        {
            ItemCotizacion i = cotizacion.Items[x];
            float precio = i.Articulo.DetalleVenta != null ? i.Articulo.DetalleVenta.Precio : 0f;

            // Nº ítem.
            sheet1.Cell(fila, 1).Value = String.Format("{0}", x + 1);
            // Código.
            sheet1.Cell(fila, 2).Value = "'" + i.Articulo.Codigo;
            sheet1.Cell(fila, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            // Descripción.
            sheet1.Cell(fila, 3).Value = i.Articulo.Descripcion;
            // Cantidad.
            sheet1.Cell(fila, 7).Value = i.Cantidad;
            sheet1.Cell(fila, 7).Style.NumberFormat.Format = "0.00";
            // Precio.
            sheet1.Cell(fila, 8).Value = i.Precio;
            sheet1.Cell(fila, 8).Style.NumberFormat.Format = "0.00";
            // Precio con descuento.
            sheet1.Cell(fila, 9).Value = i.PrecioDescuento;
            sheet1.Cell(fila, 9).Style.NumberFormat.Format = "0.00";
            // Total.
            sheet1.Cell(fila, 10).Value = i.Cantidad * i.Precio;
            sheet1.Cell(fila, 10).Style.NumberFormat.Format = "0.00";
            // Total con descuento.
            sheet1.Cell(fila, 11).Value = i.Cantidad * i.PrecioDescuento;
            sheet1.Cell(fila, 11).Style.NumberFormat.Format = "0.00";
            // Detalles de la última compra.
            if (i.Articulo.DetalleVenta != null)
            {
                // OC.
                sheet1.Cell(fila, 12).Value = i.Articulo.DetalleVenta.OC;
                sheet1.Cell(fila, 12).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                // Fecha.
                sheet1.Cell(fila, 13).Value = i.Articulo.DetalleVenta.Fecha.ToShortDateString();
                sheet1.Cell(fila, 13).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                // Proveedor.
                sheet1.Cell(fila, 14).Value = i.Articulo.DetalleVenta.Proveedor;
            }
        }
			
		int ultFilaItems = fila - 1;

        // Barra divisora.
        sheet1.Range(sheet1.Cell(fila, 1), sheet1.Cell(fila, 14)).Merge();
        sheet1.Range(sheet1.Cell(fila, 1), sheet1.Cell(fila, 9)).Style.Fill.BackgroundColor = sheet1.Cell(1, 1).Style.Fill.BackgroundColor;
        sheet1.Range(sheet1.Cell(fila, 1), sheet1.Cell(fila, 9)).Style.Font.FontColor = sheet1.Cell(1, 1).Style.Font.FontColor;

        fila += 2;
		
		// Total cotización.
        sheet1.Cell(fila, 9).Value = "Total U$S sin descuento:";
        sheet1.Cell(fila, 9).Style.Font.Bold = true;
        sheet1.Cell(fila, 11).FormulaA1 = String.Format("=SUMA(J{0}:J{1})", FilaInicio, ultFilaItems);
        sheet1.Cell(fila, 11).Style.NumberFormat.Format = "0.00";
        sheet1.Cell(fila, 11).Style.Font.Bold = true;
        fila++;
        sheet1.Cell(fila, 9).Value = "Total U$S con descuento:";
        sheet1.Cell(fila, 9).Style.Font.Bold = true;
        sheet1.Cell(fila, 11).FormulaA1 = String.Format("=SUMA(K{0}:K{1})", FilaInicio, ultFilaItems);
		sheet1.Cell(fila, 11).Style.NumberFormat.Format = "0.00";
        sheet1.Cell(fila, 11).Style.Font.Bold = true;
			
		// Cambio el formato.
        sheet1.Range(sheet1.Cell(FilaInicio, 1), sheet1.Cell(fila, 13)).Style.Font.FontName = "Arial";
        sheet1.Range(sheet1.Cell(FilaInicio, 1), sheet1.Cell(fila, 13)).Style.Font.FontSize = 9;
					
		sheet1.Column("A").AdjustToContents();	
		sheet1.Column("B").AdjustToContents();
		sheet1.Column("C").AdjustToContents();
		sheet1.Column("D").AdjustToContents();
		sheet1.Column("E").AdjustToContents();
		sheet1.Column("F").AdjustToContents();		
		sheet1.Column("G").AdjustToContents();
		sheet1.Column("H").AdjustToContents();
		sheet1.Column("I").AdjustToContents();
		sheet1.Column("J").AdjustToContents();
		sheet1.Column("K").AdjustToContents();
		sheet1.Column("L").AdjustToContents();
		sheet1.Column("M").AdjustToContents();
		sheet1.Column("N").AdjustToContents();
			
		workbook.SaveAs(path);
		
        return path;
	
    }
}
