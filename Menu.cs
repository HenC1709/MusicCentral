using System;

namespace MiProyecto;
class Menu
{
    public void Mostrar(Usuario usuario)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine( " ==== BIENVENIDO A LA CUEVA DE LA MUSICA! ==== ");
         Console.WriteLine("2. Buscar Musica");

        if (usuario.Rol == Rol.Admin)
        {
            Console.WriteLine("1. Agregar musica ");
            Console.WriteLine("3. Lista de canciones");
            Console.WriteLine("4. Usuarios");
            Console.WriteLine("0. Salir");

        }
        else
        {
            Console.WriteLine("0. Salir");
        }
       
         Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n Selecciona una opcion we");
    }
}