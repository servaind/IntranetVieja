using System;


/// <summary>
/// Descripción breve de Configuracion
/// </summary>
public static class Configuracion
{
    // Bugs corregidos.
    //  - Cuando se busca un artículo para un VDM, se le quitaba los espacios al final y principio luego de recibir el valor
    //    desde la BD, provocando que cuando se busque el artículo de nuevo, no lo encuentre.

    // Versión.
    //  v4:
    //      - Reestructuración de código fuente.
    //      - Rediseño de formularios.
    //      - Corrección de bugs en código.
    //      - Rediseño de plantillas.
    //      - Reestructuración de directorios.
    //      - Aplicación de jQuery + Ajax.
    //      - Reemplazo del menú de v3 por una barra superior.
    //      - Rediseño de hojas de estilo.
    //  v3:
    //      - Versión funcional actual.

    public const string Version = "5";
}
