using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Canciones.Models;

namespace Canciones.Services
{
    public class LoginService
    {
        // La ruta hacia tu carpeta de datos protegida
        private string ruta = Path.Combine("Data", "BaseDeUsuarios.json");

        public Usuario? iniciarSesion()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("======================================");
            Console.WriteLine("     🌸 MUSICA CENTRAL LOGIN 🌸      ");
            Console.WriteLine("======================================");
            Console.ResetColor();

            Console.WriteLine("1. inciar Sesión");
            Console.WriteLine("2. Registro");
            Console.Write("\nSelecione una opción: ");
            string opcion = Console.ReadLine()!;
            if ( opcion == "2")
            {
                Usuario nuevo = new Usuario();
                nuevo.Registro();
                return null; // retornamos un null para entrar al main:)
            }

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("\n➤ Nombre de Usuario: ");
            string nombre = Console.ReadLine()?.Trim() ?? "";
            
             Console.ForegroundColor = ConsoleColor.Blue;
             Console.Write("➤ ID de Acceso: ");
            string contra = Console.ReadLine()?.Trim() ?? "";

            if (!File.Exists(ruta)) return null;

              try 
            {
                string contenido = File.ReadAllText(ruta);
                var listaUsuarios = JsonSerializer.Deserialize<List<Usuario>>(contenido);
                var user = listaUsuarios?.FirstOrDefault(u => u.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));

                if (user != null && user.ID == contra)
                {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n[OK] ¡Acceso concedido! Bienvendo, {user.Nombre}.");
                        Thread.Sleep(1200);
                        return user;
                }
                 Console.ForegroundColor = ConsoleColor.DarkRed;
              Console.WriteLine("\n[X] Datos incorrectos o usuario inexistente.");
           }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            Thread.Sleep(1500);
            return null!;
        }
    }
 }
