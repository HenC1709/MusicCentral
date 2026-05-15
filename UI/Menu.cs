using System;
using Canciones.Models;
using Canciones.Services;

namespace Canciones.UI
{
    public class Menu
    {
        public static void MostrarMenuUsuario(Usuario usuario, MusicManager manager, PlaylistManager playlistManager)
        {
            string opc;
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("==== BIENVENIDO A LA CUEVA DE LA MUSICA! ====");
                Console.WriteLine("2. Buscar Musica");
                Console.WriteLine("3. Lista de canciones");
                Console.WriteLine("5. playlists");

                if (usuario.Rol == Rol.Admin)
                {
                  Console.WriteLine("1. Agregar musica");
                  Console.WriteLine("3. Lista de canciones");
                  Console.WriteLine("4. Ver Usuarios");
                  Console.WriteLine("5. playlists");
                }
                Console.WriteLine("0. Salir");
                Console.ResetColor();
                
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("\nSelecciona una opcion we: ");
                opc = Console.ReadLine()!;

                Console.ResetColor();
                Console.WriteLine(); 

            switch (opc)
                {
                    case "1":
                    if (usuario.Rol == Rol.Admin)
                        {
                         Console.Write("Canción: "); string t = Console.ReadLine()!;
                         Console.Write("Autor: "); string a = Console.ReadLine()!;
                         manager.AgregarMusica(t, a, usuario.Nombre);
                        }
                        else {Console.WriteLine("no hay permiso "); }
                        break;

                    case "2":
                    manager.BuscarMusica();
                    break;

                    case "3":
                    if (usuario.Rol == Rol.Admin) { manager.MostrarLista(); }
                    else if (usuario.Rol == Rol.Usuario) {manager.MostrarLista();}
                    else { Console.WriteLine("Si pasaste ambos if, tienes mi respeto xd"); }
                    break;

                    case "4":
                    if (usuario.Rol == Rol.Admin) { Usuario.VerUsuarios(); }
                    else {Console.WriteLine("No tienes permiso we "); }
                    break;
                    
                    case "5":
                    MenuPlaylists(usuario, playlistManager);
                    break;

                    case "0":
                    Console.WriteLine("Saliendo de la cueva... ");
                    break;

                    default:
                    Console.WriteLine("Opcion no valida. ");
                    break;
                }
                if (opc != "0")
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
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
        Console.WriteLine("==== MENU PLAYLISTS ====");
        Console.ResetColor();

        Console.WriteLine("1. Crear playlist");
        Console.WriteLine("2. Ver playlists");
        Console.WriteLine("3. Agregar canción a playlist");
        Console.WriteLine("4. Ver contenido de playlist");
        Console.WriteLine("0. Volver");

        Console.Write("\nSeleccione una opción: ");
        opc = Console.ReadLine()!;

        switch (opc)
        {
            case "1":

                Console.Write("Nombre de la playlist: ");
                string nombre = Console.ReadLine()!;

                playlistManager.CrearPlaylist(nombre, usuario.Nombre);
                break;

            case "2":

                playlistManager.MostrarPlaylists();
                break;

            case "3":

                Console.Write("Nombre playlist: ");
                string playlist = Console.ReadLine()!;

                Console.Write("Nombre canción: ");
                string cancion = Console.ReadLine()!;

                playlistManager.AgregarCancionPlaylist(playlist, cancion);
                break;

            case "4":

                Console.Write("Nombre playlist: ");
                string ver = Console.ReadLine()!;

                playlistManager.VerContenidoPlaylist(ver);
                break;

            case "0":
                break;

            default:

                Console.WriteLine("Opción inválida.");
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