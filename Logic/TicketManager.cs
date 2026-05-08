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
            string rutaArchivo = Path.Combine(carpeta, "Reporte_General.txt");

    string contenido = $@"
==========================================
        NUEVO REGISTRO DETECTADO
==========================================
FECHA: {DateTime.Now:dd/MM/yyyy HH:mm:ss}
USUARIO: {usuarioNombre}
CANCION: {cancion.Cancion}
AUTOR: {cancion.Autor}
------------------------------------------
"; // Dejamos un espacio al final para que el siguiente no pegue pegado

    // LA MAGIA: Append en lugar de Write
    File.AppendAllText(rutaArchivo, contenido);
    
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"\n[SISTEMA] Registro añadido al reporte general.");
    Console.ResetColor();
 }  
 } }