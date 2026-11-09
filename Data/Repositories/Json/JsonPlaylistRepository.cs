using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Canciones.Models;

namespace Canciones.Data.Repositories.Json
{
    // =========================================================================================
    // 📁 IMPLEMENTACIÓN JSON: Persistencia de Playlists
    // =========================================================================================
    // Maneja la serialización y deserialización de listas de reproducción en 'Playlist.json'.
    // Corrige el bug crítico de doble negación '!!' y provee una base sólida para migrar a SQL.
    // =========================================================================================
    public class JsonPlaylistRepository : IPlaylistRepository
    {
        private readonly string _ruta;
        private List<Playlist> _playlists = new();

        public JsonPlaylistRepository(string? rutaPersonalizada = null)
        {
            // Nota: En el disco el archivo se llama "Playlist.json"
            _ruta = rutaPersonalizada ?? Path.Combine("Data", "Playlist.json");
            CargarDatos();
        }

        private void CargarDatos()
        {
            try
            {
                if (File.Exists(_ruta))
                {
                    string contenido = File.ReadAllText(_ruta);

                    // =========================================================================
                    // 🐛 BUG CRÍTICO IDENTIFICADO Y CORREGIDO:
                    // En el código original decía:
                    //   if(!!string.IsNullOrEmpty(contenido)) { ... }
                    // La doble negación (!!) hace que:
                    //   - Si el archivo TIENE texto: string.IsNullOrEmpty es false -> !false = true -> !true = false.
                    //     ¡NUNCA cargaba las playlists existentes!
                    //   - Si el archivo estaba VACÍO: string.IsNullOrEmpty es true -> !true = false -> !false = true.
                    //     ¡Intentaba deserializar un string vacío y lanzaba excepción!
                    // Solución correcta:
                    // =========================================================================
                    if (!string.IsNullOrWhiteSpace(contenido))
                    {
                        _playlists = JsonSerializer.Deserialize<List<Playlist>>(contenido) ?? new List<Playlist>();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ADVERTENCIA] Error al leer playlists de JSON: {ex.Message}");
                _playlists = new List<Playlist>();
            }
        }

        public void SaveChanges()
        {
            try
            {
                // 🐛 BUG CORREGIDO: Creación automática de carpeta contenedora
                string? directorio = Path.GetDirectoryName(_ruta);
                if (!string.IsNullOrEmpty(directorio) && !Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }

                var opciones = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(_playlists, opciones);
                File.WriteAllText(_ruta, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error al guardar playlists en JSON: {ex.Message}");
            }
        }

        public List<Playlist> GetAll()
        {
            return new List<Playlist>(_playlists);
        }

        public Playlist? GetByNombre(string nombre)
        {
            return _playlists.FirstOrDefault(p => p.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
        }

        public void Add(Playlist playlist)
        {
            // Asignar ID autoincremental si no tiene (preparación SQL)
            if (playlist.Id == 0)
            {
                int maxId = _playlists.Count > 0 ? _playlists.Max(p => p.Id) : 0;
                playlist.Id = maxId + 1;
            }

            _playlists.Add(playlist);
            SaveChanges();
        }

        public void Update(Playlist playlist)
        {
            var index = _playlists.FindIndex(p => p.Nombre.Equals(playlist.Nombre, StringComparison.OrdinalIgnoreCase));
            if (index != -1)
            {
                _playlists[index] = playlist;
            }
            SaveChanges();
        }
    }
}

