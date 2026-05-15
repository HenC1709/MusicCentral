using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Canciones.Models;

namespace Canciones.Services
{
    public class PlaylistManager
    {
        private string ruta = Path.Combine("Data", "Playlist.json");

        public List<Playlist> playlists { get; private set; } = new();

        public PlaylistManager()
        {
            CargarDatos();
        }
       private void CargarDatos()
        {
           if (File.Exists(ruta))
            {
                string contenido = File.ReadAllText(ruta);

                if(!!string.IsNullOrEmpty(contenido))
                {
                    playlists = JsonSerializer.Deserialize<List<Playlist>>(contenido) ?? new List<Playlist>();
                }
            } 
        }
    private void GuardarDatos()
        {
            var opciones = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(playlists, opciones);
            File.WriteAllText(ruta, json);
        }

        public void CrearPlaylist(string nombrePlaylist, string usuario)
        {
            // validar que no existe otra playlist con el mismo nombre
            bool existe = playlists.Any(p => p.Nombre.Equals(nombrePlaylist, StringComparison.OrdinalIgnoreCase));

            if (existe)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\n[X] Ya existe una playlist con este nombre. ");
                Console.ResetColor();
                return;
            }

            Playlist nueva = new Playlist
            {
                Nombre = nombrePlaylist,
                UsuarioCreador = usuario
            };

            playlists.Add(nueva);
            GuardarDatos();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n[OK] Playlist creada correctamente.");
            Console.ResetColor();
        }

        public void MostrarPlaylists()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("=== PLAYLISTS DISPONIBLES ===\n");
            Console.ResetColor();

            if (playlists.Count == 0)
            {
             Console.ForegroundColor = ConsoleColor.Red;
             Console.WriteLine("No hay playlists creadas.");
             Console.ResetColor();
             return;
            }
            foreach (var playlist in playlists)
            {
                Console.WriteLine(playlist);
            }
        }
        public void AgregarCancionPlaylist(string nombrePlaylist, string nombreCancion)
        {
            // buscamos la playlist
            var playlist = playlists.FirstOrDefault(p => p.Nombre.Equals(nombrePlaylist, StringComparison.OrdinalIgnoreCase));

            if (playlist == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[X] Playlist no encontrada.");
                Console.ResetColor();
                return;
            }
            // duplicados
            bool yaExiste = playlist.Canciones.Any(c => c.Equals(nombreCancion, StringComparison.OrdinalIgnoreCase));
            if (yaExiste)
            {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n[!] Esa canción ya está en la playlist.");
            Console.ResetColor();
            return;
            }

            // por fin agregamos las canciones xd
            playlist.Canciones.Add(nombreCancion);
            GuardarDatos();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n[OK] Cancion agregada con exito a la playlist. ");
            Console.ResetColor();
        }
        public void VerContenidoPlaylist(string nombrePlaylist)
        {
            var playlist = playlists.FirstOrDefault(p => p.Nombre.Equals(nombrePlaylist, StringComparison.OrdinalIgnoreCase));

            if (playlist == null)
            {
             Console.ForegroundColor = ConsoleColor.Red;
             Console.WriteLine("\n[X] Playlist no encontrada.");  
             Console.ResetColor();
             return;
            }
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"==== PLAYLIST: {playlist.Nombre} ====");
            Console.ResetColor();

            Console.WriteLine($"Creador: {playlist.UsuarioCreador}");
            Console.WriteLine($"Fecha: {playlist.FechaCreacion}");
            Console.WriteLine();

            if (playlist.Canciones.Count == 0)
            {
              Console.ForegroundColor = ConsoleColor.Yellow;
              Console.WriteLine("Esta playlist está vacía.");
              Console.ResetColor();
              return;
            }
            foreach (var cancion in playlist.Canciones)
            {
                Console.WriteLine($"- {cancion}");
            }

        }

    }
}