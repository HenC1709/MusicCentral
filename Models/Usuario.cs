using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Canciones.Models
{
    public enum Rol
    {
        Usuario,
        Admin
    }

    // =========================================================================================
    // 🗄️ EVOLUCIÓN A SQL & 🐛 ANÁLISIS DE BUGS DE RESPONSABILIDAD ÚNICA:
    // =========================================================================================
    // 🐛 BUG DE DISEÑO RESUELTO (Dependencia Circular y Violación de Capas):
    // El modelo de dominio 'Usuario' NO debe depender de 'Canciones.Data' ni de sus repositorios.
    // En Clean Architecture, las dependencias fluyen siempre hacia adentro:
    //      UI -> Services -> Data (Repositories) -> Models
    // 'Models' es la capa base del dominio y no debe importar 'Canciones.Data'.
    //
    // 🗄️ EVOLUCIÓN A SQL:
    // En SQL, esta clase mapea directamente a la tabla 'Usuarios':
    //      CREATE TABLE Usuarios (
    //          Id INT IDENTITY(1,1) PRIMARY KEY,
    //          Nombre NVARCHAR(100) NOT NULL UNIQUE,
    //          ID NVARCHAR(10) NOT NULL,
    //          Rol INT NOT NULL
    //      );
    // =========================================================================================
    public class Usuario
    {
        // 🗄️ SQL: Clave primaria (PK)
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string ID { get; set; } = "";
        public Rol Rol { get; set; } = Rol.Usuario;

        public Usuario() { }

        public Usuario(string nombre, string id, Rol rol = Rol.Usuario)
        {
            Nombre = nombre;
            ID = id;
            Rol = rol;
        }

        // =====================================================================================
        // MÉTODOS DE COMPATIBILIDAD HACIA ATRÁS:
        // Se preservan para compatibilidad sin introducir dependencias circulares con 'Canciones.Data'.
        // =====================================================================================

        // 🐛 BUG ORIGINAL IDENTIFICADO: File.WriteAllText fallaba si la carpeta "Data" no existía.
        public void GuardarUsuario(Usuario nuevoUsuario)
        {
            try
            {
                string ruta = Path.Combine("Data", "BaseDeUsuarios.json");
                string? directorio = Path.GetDirectoryName(ruta);
                if (!string.IsNullOrEmpty(directorio) && !Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }

                List<Usuario> listaActual = new List<Usuario>();
                if (File.Exists(ruta))
                {
                    string contenido = File.ReadAllText(ruta);
                    if (!string.IsNullOrWhiteSpace(contenido))
                    {
                        listaActual = JsonSerializer.Deserialize<List<Usuario>>(contenido) ?? new List<Usuario>();
                    }
                }

                listaActual.Add(nuevoUsuario);

                var opciones = new JsonSerializerOptions { WriteIndented = true };
                string jsonfinal = JsonSerializer.Serialize(listaActual, opciones);
                File.WriteAllText(ruta, jsonfinal);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Usuario guardado en la base de datos (JSON)!");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error al guardar usuario: {ex.Message}");
            }
        }

        // 🐛 BUG ORIGINAL IDENTIFICADO: No validaba deserialización nula (lista! podía causar NullReferenceException).
        public static void VerUsuarios()
        {
            try
            {
                string ruta = Path.Combine("Data", "BaseDeUsuarios.json");
                if (!File.Exists(ruta))
                {
                    Console.WriteLine("No se encontró el archivo de usuarios.");
                    return;
                }

                string contenido = File.ReadAllText(ruta);
                if (string.IsNullOrWhiteSpace(contenido))
                {
                    Console.WriteLine("No hay usuarios registrados actualmente.");
                    return;
                }

                var lista = JsonSerializer.Deserialize<List<Usuario>>(contenido);
                Console.WriteLine("\nLista de usuarios registrados:");
                if (lista == null || lista.Count == 0)
                {
                    Console.WriteLine("No hay usuarios registrados actualmente.");
                    return;
                }

                foreach (var u in lista)
                {
                    Console.WriteLine($"- {u.Nombre} (ID: {u.ID}) - Rol: {u.Rol}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error al leer usuarios: {ex.Message}");
            }
        }

        // 🐛 BUG ORIGINAL IDENTIFICADO: Mezclar lógica de UI y consola dentro de la entidad de dominio.
        // 🐛 BUG DE MENSAJE: Decía "maximo 4" cuando la validación exigía obligatoriamente 4 dígitos exactos.
        public void Registro()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Ingrese nombre para registro: ");
            Nombre = Console.ReadLine()?.Trim() ?? "";

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Ingrese una ID válida (exactamente 4 números): ");
            ID = Console.ReadLine()?.Trim() ?? "";

            // Validar que solo tenga números y sean 4 dígitos
            while (!EsSoloNumeros(ID))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Error: La ID debe contener exactamente 4 números.");
                Console.ResetColor();

                Console.WriteLine("Ingrese una ID válida (4 números): ");
                ID = Console.ReadLine()?.Trim() ?? "";
            }

            // 🐛 BUG / HARDCODING: El rol de Admin estaba atado al nombre fijo "Henry".
            // En SQL esto se reemplaza mediante la columna 'Rol' administrada en la base de datos.
            if (this.Nombre.Equals("Henry", StringComparison.OrdinalIgnoreCase))
            {
                this.Rol = Rol.Admin;
            }
            else
            {
                this.Rol = Rol.Usuario;
            }

            GuardarUsuario(this);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Registro exitoso!");
            Console.ResetColor();
        }

        public static bool EsSoloNumeros(string texto)
        {
            if (string.IsNullOrEmpty(texto) || texto.Length != 4) return false;
            foreach (char c in texto)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        public override string ToString()
        {
            return $"{Nombre} (ID: {ID}) - {Rol}";
        }
    }
}
