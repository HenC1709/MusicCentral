using System;
using System.Collections.Generic;
using System.Linq;
using Canciones.Data.Repositories;
using Canciones.Data.Repositories.Json;
using Canciones.Models;

namespace Canciones.Services
{
    // =========================================================================================
    // 🗄️ EVOLUCIÓN A SQL & 🐛 BUGS RESUELTOS:
    // =========================================================================================
    // 🐛 BUG CRÍTICO IDENTIFICADO (Doble Negación en Carga de Datos):
    //    En el código original:
    //      if(!!string.IsNullOrEmpty(contenido))
    //    El operador '!!' provocaba que:
    //    - Si el archivo contenía playlists, string.IsNullOrEmpty era falso -> !!falso = falso.
    //      Por tanto, ¡NUNCA se cargaban las playlists desde el archivo!
    //    - Si el archivo estaba vacío, entraba a deserializar un texto en blanco, causando excepción.
    //
    // 🐛 CONVENCIÓN DE NOMBRES:
    //    La propiedad pública 'playlists' estaba en minúscula, violando PascalCase en C#.
    //    Se corrigió a 'Playlists' manteniendo alias para retrocompatibilidad.
    //
    // 🗄️ CAMINO A SQL (INTEGRIDAD REFERENCIAL / FOREIGN KEYS):
    //    En SQL, la relación entre Playlists y Canciones se asegura mediante llaves foráneas (FK):
    //      FOREIGN KEY (CancionId) REFERENCES Canciones(Id)
    //    Esto impide que se agreguen canciones inexistentes a una playlist.
    //    Ahora PlaylistManager recibe opcionalmente 'ICancionRepository' para emular esta
    //    integridad referencial antes de que exista la base de datos SQL.
    // =========================================================================================
    public class PlaylistManager
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly ICancionRepository? _cancionRepository;

        // Constructor con Inyección de Dependencias
        public PlaylistManager(IPlaylistRepository playlistRepository, ICancionRepository? cancionRepository = null)
        {
            _playlistRepository = playlistRepository;
            _cancionRepository = cancionRepository;
        }

        // Constructor por defecto (usa JsonPlaylistRepository)
        public PlaylistManager() : this(new JsonPlaylistRepository())
        {
        }

        // Propiedad con convención C# PascalCase
        public List<Playlist> Playlists => _playlistRepository.GetAll();

        // Alias para compatibilidad con código anterior
        public List<Playlist> playlists => Playlists;

        public void CrearPlaylist(string nombrePlaylist, string usuario)
        {
            if (string.IsNullOrWhiteSpace(nombrePlaylist))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\n[X] El nombre de la playlist no puede estar vacío.");
                Console.ResetColor();
                return;
            }

            // Validar que no exista otra playlist con el mismo nombre
            var existente = _playlistRepository.GetByNombre(nombrePlaylist.Trim());

            if (existente != null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\n[X] Ya existe una playlist con este nombre.");
                Console.ResetColor();
                return;
            }

            Playlist nueva = new Playlist
            {
                Nombre = nombrePlaylist.Trim(),
                UsuarioCreador = usuario
            };

            _playlistRepository.Add(nueva);

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

            var lista = _playlistRepository.GetAll();

            if (lista.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No hay playlists creadas.");
                Console.ResetColor();
                return;
            }

            foreach (var playlist in lista)
            {
                Console.WriteLine($"- {playlist}");
            }
        }

        public void AgregarCancionPlaylist(string nombrePlaylist, string nombreCancion)
        {
            if (string.IsNullOrWhiteSpace(nombrePlaylist) || string.IsNullOrWhiteSpace(nombreCancion))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[X] Debes ingresar el nombre de la playlist y de la canción.");
                Console.ResetColor();
                return;
            }

            var playlist = _playlistRepository.GetByNombre(nombrePlaylist.Trim());

            if (playlist == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[X] Playlist no encontrada.");
                Console.ResetColor();
                return;
            }

            // =========================================================================
            // 🗄️ VALIDACIÓN DE INTEGRIDAD REFERENCIAL (Preparación para SQL Foreign Key):
            // En SQL, una Llave Foránea rechazaría insertar una canción que no existe en
            // la tabla Canciones. Aquí emulamos esa validación si se proveyó ICancionRepository.
            // =========================================================================
            if (_cancionRepository != null)
            {
                var catalogo = _cancionRepository.GetAll();
                var cancionReal = catalogo.FirstOrDefault(c => c.Cancion.Equals(nombreCancion.Trim(), StringComparison.OrdinalIgnoreCase));
                if (cancionReal == null)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\n[AVISO] '{nombreCancion}' no existe en el catálogo de canciones.");
                    Console.Write("¿Desea agregarla de todas formas a la playlist? (s/n): ");
                    string confirmar = Console.ReadLine()?.Trim().ToLower() ?? "";
                    if (confirmar != "s")
                    {
                        Console.WriteLine("Operación cancelada.");
                        Console.ResetColor();
                        return;
                    }
                }
            }

            // Validar duplicados dentro de la playlist
            bool yaExiste = playlist.Canciones.Any(c => c.Equals(nombreCancion.Trim(), StringComparison.OrdinalIgnoreCase));
            if (yaExiste)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n[!] Esa canción ya está en la playlist.");
                Console.ResetColor();
                return;
            }

            // 🗄️ EVOLUCIÓN A SQL:
            // En SQL, esto se traduce a:
            // INSERT INTO PlaylistCanciones (PlaylistId, CancionId) VALUES (@PlaylistId, @CancionId)
            playlist.Canciones.Add(nombreCancion.Trim());
            _playlistRepository.Update(playlist);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n[OK] Canción agregada con éxito a la playlist.");
            Console.ResetColor();
        }

        public void VerContenidoPlaylist(string nombrePlaylist)
        {
            if (string.IsNullOrWhiteSpace(nombrePlaylist))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[X] Ingrese un nombre de playlist válido.");
                Console.ResetColor();
                return;
            }

            var playlist = _playlistRepository.GetByNombre(nombrePlaylist.Trim());

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
            Console.WriteLine($"Fecha:   {playlist.FechaCreacion:dd/MM/yyyy HH:mm}");
            Console.WriteLine(new string('-', 35));

            if (playlist.Canciones.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Esta playlist está vacía.");
                Console.ResetColor();
                return;
            }

            Console.WriteLine("Canciones en la lista:");
            for (int i = 0; i < playlist.Canciones.Count; i++)
            {
                Console.WriteLine($" {i + 1}. {playlist.Canciones[i]}");
            }
        }
    }
}
