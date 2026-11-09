using System;
using System.Threading;
using Canciones.Models;

namespace Canciones.Services
{
    // =========================================================================================
    // 🐛 BUGS RESUELTOS & NOTAS DE ARQUITECTURA:
    // =========================================================================================
    // 🐛 CONVENCIÓN C#: El método 'iniciarSesion' comenzaba en minúscula (camelCase).
    //    En C# los métodos públicos usan PascalCase: 'IniciarSesion()'.
    //    Se provee 'iniciarSesion()' como alias compatible para no romper llamadas existentes.
    //
    // 🐛 ERRORES TIPOGRÁFICOS CORREGIDOS:
    //    - "1. inciar Sesión" corregido a "1. Iniciar Sesión".
    //    - "Selecione una opción" corregido a "Seleccione una opción".
    //
    // 💡 SEPARACIÓN DE RESPONSABILIDADES:
    //    Esta clase actualmente maneja la presentación de consola (Console.Clear, ReadLine).
    //    En una arquitectura N-Capas más estricta, esto pertenecería a la carpeta UI (ej. 'LoginUI'),
    //    mientras que 'AuthService' maneja la lógica pura de autenticación.
    // =========================================================================================
    public class LoginService
    {
        private readonly AuthService _authService;

        public LoginService(AuthService? authService = null)
        {
            _authService = authService ?? new AuthService();
        }

        // Método con la convención estándar C# (PascalCase)
        public Usuario? IniciarSesion()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("======================================");
            Console.WriteLine("      MUSICA CENTRAL LOGIN       ");
            Console.WriteLine("======================================");
            Console.ResetColor();

            Console.WriteLine("1. Iniciar Sesión");
            Console.WriteLine("2. Registro");
            Console.WriteLine("3. Salir");
            Console.Write("\nSeleccione una opción: ");
            string opcion = Console.ReadLine()?.Trim() ?? "";

            if (opcion == "3" || opcion.Equals("salir", StringComparison.OrdinalIgnoreCase))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nSaliendo de Música Central... ¡Hasta pronto!");
                Console.ResetColor();
                Environment.Exit(0);
                return null;
            }

            if (opcion == "2")
            {
                RegistrarUsuario();
                return null; // Retorna null para volver al menú de autenticación en Program.cs
            }

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("\n➤ Nombre de Usuario: ");
            string nombre = Console.ReadLine()?.Trim() ?? "";

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("➤ ID de Acceso (4 dígitos): ");
            Console.ResetColor();
            string contra = LeerPasswordOculto().Trim();

            Usuario? user = _authService.IniciarSesion(nombre, contra);

            if (user != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n[OK] ¡Acceso concedido! Bienvenido, {user.Nombre}.");
                Console.ResetColor();

                Thread.Sleep(1200);
                return user;
            }

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\n[X] Datos incorrectos o usuario inexistente.");
            Console.ResetColor();

            Thread.Sleep(1500);
            return null;
        }

        // Alias en camelCase para retrocompatibilidad con código existente
        public Usuario? iniciarSesion() => IniciarSesion();

        private void RegistrarUsuario()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Ingrese nombre para registro: ");
            string nombre = Console.ReadLine()?.Trim() ?? "";

            while (string.IsNullOrWhiteSpace(nombre))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("El nombre es obligatorio. Ingrese nombre para registro: ");
                Console.ResetColor();
                nombre = Console.ReadLine()?.Trim() ?? "";
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Ingrese una ID válida (exactamente 4 números): ");
            Console.ResetColor();
            string id = LeerPasswordOculto().Trim();

            while (!Usuario.EsSoloNumeros(id))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("Error: la ID debe contener exactamente 4 números. Intente de nuevo: ");
                Console.ResetColor();
                id = LeerPasswordOculto().Trim();
            }

            Rol rol = nombre.Equals("Henry", StringComparison.OrdinalIgnoreCase) ? Rol.Admin : Rol.Usuario;
            var usuario = new Usuario(nombre, id, rol);

            try
            {
                _authService.RegistrarUsuario(usuario);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n[OK] Registro exitoso. Ya puede iniciar sesión.");
            }
            catch (InvalidOperationException ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"\n[X] No se pudo completar el registro: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"\n[X] No se pudo completar el registro: {ex.Message}");
            }
            finally
            {
                Console.ResetColor();
                Thread.Sleep(1500);
            }
        }


        //  SEGURIDAD / UX: Lectura de PIN enmascarado con asteriscos

        public static string LeerPasswordOculto()
        {
            string pass = "";
            while (true)
            {
                var tecla = Console.ReadKey(intercept: true);
                if (tecla.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }

                if (tecla.Key == ConsoleKey.Backspace)
                {
                    if (pass.Length > 0)
                    {
                        pass = pass[..^1];
                        Console.Write("\b \b");
                    }
                }
                else if (!char.IsControl(tecla.KeyChar))
                {
                    pass += tecla.KeyChar;
                    Console.Write("*");
                }
            }
            return pass;
        }
    }
}
