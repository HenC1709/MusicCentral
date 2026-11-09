using System;
using System.Collections.Generic;

namespace Canciones.Models
{
    // =========================================================================================
    // 🗄️ EVOLUCIÓN DE JSON A SQL:
    // En JSON: Una playlist almacena directamente una lista de nombres de canciones (strings).
    // En SQL Relacional (Modelo Normalizado):
    // 1. No se almacenan listas dentro de una celda. Se utiliza una relación Muchos a Muchos:
    //    - Tabla `Playlists` (Id [PK], Nombre VARCHAR(100), UsuarioId [FK], FechaCreacion DATETIME)
    //    - Tabla `Canciones` (Id [PK], Titulo VARCHAR(100), Autor VARCHAR(100), FechaDeIngreso DATETIME)
    //    - Tabla intermedia `PlaylistCanciones` (PlaylistId [FK], CancionId [FK])
    // 2. 'UsuarioCreador' pasa de ser un string plano a un Foreign Key (`UsuarioId` o navigation property).
    // =========================================================================================
    public class Playlist
    {
        // 🗄️ SQL: Clave primaria (PK) para tabla Playlists
        public int Id { get; set; }

        public string Nombre { get; set; } = "";

        // 🗄️ SQL: En BD relacional se reemplaza por 'int UsuarioId' o referencia a Usuario
        public string UsuarioCreador { get; set; } = "";

        // 🗄️ SQL: En EF Core / SQL se mapea como:
        // public virtual ICollection<Musica> Canciones { get; set; } = new HashSet<Musica>();
        // Para compatibilidad con Playlist.json actual, se mantiene List<string>.
        public List<string> Canciones { get; set; } = new();

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return $"{Nombre} - {Canciones.Count} canciones - Creador: {UsuarioCreador}";
        }
    }
}
