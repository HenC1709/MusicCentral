using System;
using Canciones.Models;
using Canciones.Services;

namespace Canciones.UI
{
    // =========================================================================================
    // 🎨 CAPA DE PRESENTACIÓN (UI / CONSOLA)
    // =========================================================================================
    // Maneja la interacción por menús, captura de entradas y presentación al usuario.
    // Desacoplada de la persistencia: solo interactúa con los Services (MusicManager, PlaylistManager).
    // =========================================================================================
    public class Menu
    {
        public static void MostrarMenuUsuario(Usuario usuario, MusicManager manager, PlaylistManager playlistManager, AuthService? authService = null)
        {
            string opc;
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("==== BIENVENIDO A LA CUEVA DE LA MÚSICA! ====");
                Console.ResetColor();

                Console.WriteLine($"Usuario activo: {usuario.Nombre} | Rol: {usuario.Rol}");
                Console.WriteLine(new string('-', 45));

                Console.WriteLine("1. Buscar Música");
                Console.WriteLine("2. Lista de canciones");
                Console.WriteLine("3. Playlists");

                if (usuario.Rol == Rol.Admin)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("4. Agregar música (Admin)");
                    Console.WriteLine("5. Ver Usuarios (Admin)");
                    Console.ResetColor();
                }

                Console.WriteLine("0. Salir");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("\nSeleccione una opción: ");
                opc = Console.ReadLine()?.Trim() ?? "";
                Console.ResetColor();
                Console.WriteLine();

                switch (opc)
                {
                    case "1":
                        manager.BuscarMusica();
                        break;

                    case "2":
                        manager.MostrarLista();
                        break;

                    case "3":
                        MenuPlaylists(usuario, playlistManager);
                        break;

                    case "4":
                        if (usuario.Rol == Rol.Admin)
                        {
                            Console.Write("Título de la canción: ");
                            string t = Console.ReadLine()?.Trim() ?? "";

                            Console.Write("Autor / Artista: ");
                            string a = Console.ReadLine()?.Trim() ?? "";

                            manager.AgregarMusica(t, a, usuario.Nombre);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("No tienes permisos de Administrador para realizar esta acción.");
                            Console.ResetColor();
                        }
                        break;

                    case "5":
                        if (usuario.Rol == Rol.Admin)
                        {
                            if (authService != null)
                            {
                                var lista = authService.ObtenerTodosLosUsuarios();
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("=== USUARIOS REGISTRADOS EN EL SISTEMA ===\n");
                                Console.ResetColor();

                                if (lista.Count == 0)
                                {
                                    Console.WriteLine("No hay usuarios registrados actualmente.");
                                }
                                else
                                {
                                    Console.WriteLine(string.Format("{0,-6} | {1,-18} | {2,-10} | {3,-10}", "ID BD", "Nombre", "PIN/ID", "Rol"));
                                    Console.WriteLine(new string('-', 52));
                                    foreach (var u in lista)
                                    {
                                        Console.WriteLine(string.Format("{0,-6} | {1,-18} | {2,-10} | {3,-10}", u.Id, u.Nombre, u.ID, u.Rol));
                                    }
                                }
                            }
                            else
                            {
                                Usuario.VerUsuarios();
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("No tienes permisos de Administrador para realizar esta acción.");
                            Console.ResetColor();
                        }
                        break;

                    case "0":
                        Console.WriteLine("Saliendo del sistema... ¡Hasta luego!");
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Opción no válida. Intente nuevamente.");
                        Console.ResetColor();
                        break;
                }

                if (opc != "0")
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ResetColor();
                    Console.ReadKey();
                }

            } while (opc != "0");
        }

        private static void MenuPlaylists(Usuario usuario, PlaylistManager playlistManager)
        {
            string opc;

            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("==== MENÚ DE PLAYLISTS ====");
                Console.ResetColor();

                Console.WriteLine("1. Crear playlist");
                Console.WriteLine("2. Ver playlists");
                Console.WriteLine("3. Agregar canción a playlist");
                Console.WriteLine("4. Ver contenido de playlist");
                Console.WriteLine("0. Volver");

                Console.Write("\nSeleccione una opción: ");
                opc = Console.ReadLine()?.Trim() ?? "";

                switch (opc)
                {
                    case "1":
                        Console.Write("Nombre de la nueva playlist: ");
                        string nombre = Console.ReadLine()?.Trim() ?? "";
                        playlistManager.CrearPlaylist(nombre, usuario.Nombre);
                        break;

                    case "2":
                        playlistManager.MostrarPlaylists();
                        break;

                    case "3":
                        Console.Write("Nombre de la playlist: ");
                        string playlist = Console.ReadLine()?.Trim() ?? "";

                        Console.Write("Nombre de la canción: ");
                        string cancion = Console.ReadLine()?.Trim() ?? "";

                        playlistManager.AgregarCancionPlaylist(playlist, cancion);
                        break;

                    case "4":
                        Console.Write("Nombre de la playlist a consultar: ");
                        string ver = Console.ReadLine()?.Trim() ?? "";

                        playlistManager.VerContenidoPlaylist(ver);
                        break;

                    case "0":
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Opción inválida.");
                        Console.ResetColor();
                        break;
                }

                if (opc != "0")
                {
                    Console.WriteLine("\nPresione una tecla para continuar...");
                    Console.ReadKey();
                }

            } while (opc != "0");
        }
    }
}
