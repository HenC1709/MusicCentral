using System.Text.Json;
using Canciones.Models;

namespace Canciones.Services
{
    public class LoginService
    {
        // La ruta hacia tu carpeta de datos protegida
        private string ruta = Path.Combine("Data", "BaseDeUsuarios.json");

        public Usuario iniciarSesion()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
          Console.WriteLine("Tienes cuenta (S/N)");
          string respuesta = Console.ReadLine()!.ToUpper();
          if (respuesta == "N" )
            {
                Usuario user = new Usuario();
                user.Registro();
                Console.WriteLine("Gracias por registrarse! ");
            }
        
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("======================================");
            Console.WriteLine("     🌸 MUSICA CENTRAL LOGIN 🌸      ");
            Console.WriteLine("======================================");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("\n➤ Nombre de Usuario: ");
            string nombre = Console.ReadLine()?.Trim() ?? "";
            
             Console.ForegroundColor = ConsoleColor.Blue;
             Console.Write("➤ ID de Acceso: ");
            string contra = Console.ReadLine()?.Trim() ?? "";

            if (!File.Exists(ruta))
            {
                Console.WriteLine("\n[!] No hay base de datos de usuarios. Regístrate primero.");
                Thread.Sleep(2000);
                return null!;
            }

            try 
            {
                string contenido = File.ReadAllText(ruta);
                var listaUsuarios = JsonSerializer.Deserialize<List<Usuario>>(contenido);

                if (listaUsuarios == null) return null!;

                // Buscamos al usuario
                var user = listaUsuarios.FirstOrDefault(u => u.Nombre.ToLower() == nombre.ToLower());

                if (user != null)
                {
                    if (user.ID == contra)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n[OK] ¡Acceso concedido! Bienvendo, {user.Nombre}.");
                        Console.ResetColor();
                        Thread.Sleep(1200);
                        return user;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n[X] ID incorrecto.");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n[X] El usuario no existe.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al leer la base de datos: " + ex.Message);
            }

            Console.ResetColor();
            Console.WriteLine("Presiona cualquier tecla para reintentar...");
            Console.ReadKey();
            return null!;
        }
    }

}
