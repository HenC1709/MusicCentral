using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Canciones.Models;

namespace Canciones.Data.Repositories.Json
{
    // =========================================================================================
    // 📁 IMPLEMENTACIÓN JSON: Persistencia de Usuarios
    // =========================================================================================
    // Encapsula la lectura y escritura en 'BaseDeUsuarios.json'.
    // Resuelve las violaciones SRP donde el modelo 'Usuario' se guardaba y leía a sí mismo.
    // =========================================================================================
    public class JsonUsuarioRepository : IUsuarioRepository
    {
        private readonly string _ruta;
        private List<Usuario> _usuarios = new();

        public JsonUsuarioRepository(string? rutaPersonalizada = null)
        {
            _ruta = rutaPersonalizada ?? Path.Combine("Data", "BaseDeUsuarios.json");
            CargarDatos();
        }

        private void CargarDatos()
        {
            try
            {
                if (File.Exists(_ruta))
                {
                    string contenido = File.ReadAllText(_ruta);
                    if (!string.IsNullOrWhiteSpace(contenido))
                    {
                        _usuarios = JsonSerializer.Deserialize<List<Usuario>>(contenido) ?? new List<Usuario>();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ADVERTENCIA] Error al leer usuarios de JSON: {ex.Message}");
                _usuarios = new List<Usuario>();
            }
        }

        private void GuardarDatos()
        {
            try
            {
                // 🐛 BUG CORREGIDO: Asegurar que la carpeta exista antes de escribir el archivo
                string? directorio = Path.GetDirectoryName(_ruta);
                if (!string.IsNullOrEmpty(directorio) && !Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }

                var opciones = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(_usuarios, opciones);
                File.WriteAllText(_ruta, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error al guardar usuarios en JSON: {ex.Message}");
            }
        }

        public List<Usuario> GetAll()
        {
            return new List<Usuario>(_usuarios);
        }

        public Usuario? GetByNombre(string nombre)
        {
            return _usuarios.FirstOrDefault(u => u.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
        }

        public Usuario? GetById(string id)
        {
            return _usuarios.FirstOrDefault(u => u.ID == id);
        }

        public Usuario? ValidateCredentials(string nombre, string id)
        {
            // 🗄️ EVOLUCIÓN A SQL:
            // En SQL esto sería:
            // "SELECT * FROM Usuarios WHERE LOWER(Nombre) = LOWER(@nombre) AND ID = @id"
            return _usuarios.FirstOrDefault(u =>
                u.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase) && u.ID == id);
        }

        public void Add(Usuario usuario)
        {
            // 🐛 BUG CORREGIDO: Validar duplicados antes de insertar
            var existente = GetByNombre(usuario.Nombre);
            if (existente != null)
            {
                throw new InvalidOperationException($"Ya existe un usuario con el nombre '{usuario.Nombre}'.");
            }

            // 🗄️ SQL: Asignar ID autoincremental si no tiene (preparación para SQL)
            if (usuario.Id == 0)
            {
                int maxId = _usuarios.Count > 0 ? _usuarios.Max(u => u.Id) : 0;
                usuario.Id = maxId + 1;
            }

            _usuarios.Add(usuario);
            GuardarDatos();
        }

        public bool Exists(string nombre, string id)
        {
            return _usuarios.Any(u =>
                u.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase) || u.ID == id);
        }
    }
}

