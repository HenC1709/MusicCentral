using System;
using System.IO;
using Canciones.Models;

namespace Canciones.Logic
{
    // =========================================================================================
    // 🐛 BUGS / OBSERVACIONES DE CÓDIGO:
    // =========================================================================================
    // 🐛 DISCREPANCIA EN COMENTARIO vs IMPLEMENTACIÓN:
    //    El comentario original decía: "// nombre unico usando solo fecha y hora", pero
    //    la ruta hardcodeada siempre escribía en "Reporte_General.txt".
    //    Se mantiene el archivo consolidado "Reporte_General.txt" para auditoría y se aclara el comentario.
    //
    // 🗄️ EVOLUCIÓN A SQL:
    //    En un sistema con base de datos, este registro de auditoría se almacena en una tabla
    //    de logs (ej. `AuditLogs` con Id, Evento, Usuario, Fecha, DetalleJson) en lugar de archivos .txt.
    // =========================================================================================
    public class TicketManager
    {
        public void GenerarTicket(Musica cancion, string usuarioNombre)
        {
            try
            {
                // Asegurar que la carpeta exista
                string carpeta = "Tickets";
                if (!Directory.Exists(carpeta))
                {
                    Directory.CreateDirectory(carpeta);
                }

                // Registro consolidado en reporte general
                string rutaArchivo = Path.Combine(carpeta, "Reporte_General.txt");

                string contenido = $@"
==========================================
        NUEVO REGISTRO DETECTADO
==========================================
FECHA: {DateTime.Now:dd/MM/yyyy HH:mm:ss}
USUARIO: {usuarioNombre}
CANCION: {cancion.Cancion}
AUTOR: {cancion.Autor}
-----------------------------------------
";

                File.AppendAllText(rutaArchivo, contenido);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n[SISTEMA] Registro añadido al reporte general.");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ADVERTENCIA] No se pudo generar el ticket: {ex.Message}");
            }
        }
    }
}
