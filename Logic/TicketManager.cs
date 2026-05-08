using System;
using System.IO;
using Canciones.Models;

namespace Canciones.Logic
{
    public class TicketManager
    {
        public void GenerarTicket(Musica cancion, string usuarioNombre)
        {
            // Creamos una carpeta para los tickets si no existe
            string carpeta = "Tickets";
            if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);

            // nombre unico usando solo fecha y hora
            string nombreArchivo = Path.Combine(carpeta, $"Ticket_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
            string contenido = $@"
==========================================
    REPORTE DE NUEVA CANCIÓN
==========================================
FECHA: {DateTime.Now:dd/MM/yyyy HH:mm:ss}
REGISTRADO POR: {usuarioNombre}
------------------------------------------
DETALLES:
CANCIÓN: {cancion.Cancion}
AUTOR: {cancion.Autor}
------------------------------------------
¡Cueva de la Música - V2! 
==========================================";




File.AppendAllText(nombreArchivo, contenido);
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine($"\n[TICKET] Generado en: {nombreArchivo}");
Console.ResetColor();
        }
    }
}