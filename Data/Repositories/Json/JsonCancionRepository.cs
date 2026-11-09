using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Canciones.Models;

namespace Canciones.Data.Repositories.Json
{
    // =========================================================================================
    // 📁 IMPLEMENTACIÓN JSON: Persistencia en archivos locales
    // =========================================================================================
    // Esta clase encapsula toda la interacción con 'CancionesAgregadas.json'.
    // Cuando el proyecto evolucione a SQL, se creará una clase hermana 'SqlCancionRepository'
    // que implementará 'ICancionRepository' sin tener que cambiar la lógica de 'MusicManager'.
    // =========================================================================================
    public class JsonCancionRepository : ICancionRepository
    {
        private readonly string _ruta;
        private List<Musica> _canciones = new();

        public JsonCancionRepository(string? rutaPersonalizada = null)
        {
            _ruta = rutaPersonalizada ?? Path.Combine("Data", "CancionesAgregadas.json");
            CargarDatos();
        }

        private void CargarDatos()
        {
            try
            {
                if (File.Exists(_ruta))
                {
                    string contenido = File.ReadAllText(_ruta);
                    // 🐛 BUG CORREGIDO: Validar no solo que el archivo exista, sino que el contenido no esté vacío o sea solo espacios
                    if (!string.IsNullOrWhiteSpace(contenido))
                    {
                        _canciones = JsonSerializer.Deserialize<List<Musica>>(contenido) ?? new List<Musica>();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ADVERTENCIA] Error al leer canciones de JSON: {ex.Message}");
                _canciones = new List<Musica>();
            }
        }

        public void SaveChanges()
        {
            try
            {
                // 🐛 BUG CORREGIDO: Verificar y crear la carpeta si no existe antes de escribir
                // Si la carpeta "Data" no existía, File.WriteAllText lanzaba DirectoryNotFoundException.
                string? directorio = Path.GetDirectoryName(_ruta);
                if (!string.IsNullOrEmpty(directorio) && !Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }

                var opciones = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(_canciones, opciones);
                File.WriteAllText(_ruta, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] No se pudo guardar en el archivo JSON: {ex.Message}");
            }
        }

        public List<Musica> GetAll()
        {
            return new List<Musica>(_canciones);
        }

        public Musica? GetById(int id)
        {
            return _canciones.FirstOrDefault(c => c.Id == id);
        }

        public List<Musica> Search(string query)
        {
            // 🐛 BUG CORREGIDO: En la versión anterior, si el usuario presionaba Enter sin texto,
            // query era "" y "".Contains("") devolvía siempre la primera canción del archivo.
            if (string.IsNullOrWhiteSpace(query))
            {
                return new List<Musica>();
            }

            string normalizado = query.Trim().ToLower();

            // 🐛 MEJORA: Devolver todas las coincidencias y no solo FirstOrDefault()
            return _canciones
                .Where(c => c.Cancion.ToLower().Contains(normalizado) || c.Autor.ToLower().Contains(normalizado))
                .ToList();
        }

        public void Add(Musica cancion)
        {
            // Asignar ID autoincremental si no tiene uno asignado (preparación para SQL)
            if (cancion.Id == 0)
            {
                int maxId = _canciones.Count > 0 ? _canciones.Max(c => c.Id) : 0;
                cancion.Id = maxId + 1;
            }

            _canciones.Add(cancion);
            SaveChanges();
        }
    }
}

