using System;
using Canciones.Data.Repositories;
using Canciones.Data.Repositories.Json;
using Canciones.Models;

namespace Canciones.Services
{
    // =========================================================================================
    // 🗄️ EVOLUCIÓN A SQL & 🐛 BUGS RESUELTOS:
    // =========================================================================================
    // 🐛 BUG / CONVENCIÓN: El archivo se llamaba 'AuthServices.cs' mientras que la clase era 'AuthService'.
    //    Se renombró el archivo a 'AuthService.cs' para cumplir la convención estándar de C#.
    //
    // 🐛 ACOPLAMIENTO DE PERSISTENCIA:
    //    AuthService leía directamente 'BaseDeUsuarios.json' con File.ReadAllText y JsonSerializer.
    //    Esto impedía usar SQL u otro motor sin reescribir este servicio.
    //
    // 💡 SOLUCIÓN (Inyección de Repositorio):
    //    AuthService ahora depende de 'IUsuarioRepository'.
    //    - Para JSON: Recibe 'JsonUsuarioRepository'.
    //    - Para SQL:  Recibirá 'SqlUsuarioRepository' sin modificar una sola línea de lógica de autenticación.
    // =========================================================================================
    public class AuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        // Constructor con Inyección de Dependencias
        public AuthService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        // Constructor por defecto (para retrocompatibilidad, usa JSON)
        public AuthService() : this(new JsonUsuarioRepository())
        {
        }

        public Usuario? IniciarSesion(string nombre, string id)
        {
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            try
            {
                // 🗄️ EVOLUCIÓN A SQL:
                // El repositorio se encarga de ejecutar:
                // SELECT * FROM Usuarios WHERE Nombre = @nombre AND ID = @id
                return _usuarioRepository.ValidateCredentials(nombre.Trim(), id.Trim());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al validar credenciales: " + ex.Message);
                return null;
            }
        }

        public (bool Exito, string Mensaje, Usuario? Usuario) Registrar(string nombre, string id)
        {
            string nombreLimpio = nombre?.Trim() ?? "";
            string idLimpia = id?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(nombreLimpio))
            {
                return (false, "El nombre de usuario no puede estar vacío.", null);
            }

            if (!Usuario.EsSoloNumeros(idLimpia))
            {
                return (false, "La ID/PIN debe contener exactamente 4 números.", null);
            }

            if (_usuarioRepository.GetByNombre(nombreLimpio) != null)
            {
                return (false, $"Ya existe un usuario registrado con el nombre '{nombreLimpio}'.", null);
            }

            // Asignación de rol: Henry es Admin por defecto, o si es el primer usuario del sistema
            Rol rolAsignado = Rol.Usuario;
            if (nombreLimpio.Equals("Henry", StringComparison.OrdinalIgnoreCase) || _usuarioRepository.GetAll().Count == 0)
            {
                rolAsignado = Rol.Admin;
            }

            var nuevoUsuario = new Usuario(nombreLimpio, idLimpia, rolAsignado);
            _usuarioRepository.Add(nuevoUsuario);

            return (true, $"¡Usuario '{nombreLimpio}' registrado exitosamente con rol {rolAsignado}!", nuevoUsuario);
        }

        public void RegistrarUsuario(Usuario usuario)
        {
            if (usuario == null)
            {
                throw new ArgumentNullException(nameof(usuario));
            }

            usuario.Nombre = usuario.Nombre?.Trim() ?? "";
            usuario.ID = usuario.ID?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(usuario.Nombre))
            {
                throw new ArgumentException("El nombre de usuario es obligatorio.", nameof(usuario));
            }

            if (!Usuario.EsSoloNumeros(usuario.ID))
            {
                throw new ArgumentException("La ID debe contener exactamente 4 números.", nameof(usuario));
            }

            _usuarioRepository.Add(usuario);
        }

        public List<Usuario> ObtenerTodosLosUsuarios()
        {
            return _usuarioRepository.GetAll();
        }
    }
}
