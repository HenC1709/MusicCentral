using System;

namespace MiProyecto;
class Program
{
    static void Main()
    {

        LoginService auth = new LoginService();
        Usuario? usuarioActual = null;
        while (usuarioActual == null)
        {
            usuarioActual = auth.iniciarSesion();
        }
        Musica.CargarDatos();
        Menu miMenu = new Menu();
        Agregar miDepo = new Agregar();
        Busqueda buscador = new Busqueda(); 
        // bucle para impedir perdida de terminal

        bool ejecutando = true;
        while (ejecutando)
        {
            miMenu.Mostrar(usuarioActual);
            string opc = Console.ReadLine()!;

            switch (opc)
            {
                
                 case "2":
                buscador.buscar(miDepo);
                break;
                
                
                case "1":
                if (usuarioActual.Rol == Rol.Admin)
                    {
                 Console.Write("Cancion: "); string t = Console.ReadLine()!;
                Console.WriteLine("Autor: "); string a = Console.ReadLine()!;
                miDepo.AgregarMusica(new Musica(t, a));
                    }
                    else
                    {
                        Console.WriteLine("No tienes permiso xd");
                    }
                    break;
               
               

                case "3":
                if (usuarioActual.Rol == Rol.Admin)
                    {
                 Lista.MostrarLista(); 
                    }
                else
                    {
                 Console.WriteLine("No tienes permiso xd");
                    } 
                break;

                case "4":
                if (usuarioActual.Rol == Rol.Admin)
                    {
                         Usuario.VerUsuarios();
                    }
                    else
                    {
                        Console.WriteLine("No tienes permiso xd");
                    }
                break;

                case "0":
                ejecutando = false;
                break;
            }

            Console.ForegroundColor = ConsoleColor.DarkBlue;
Console.WriteLine("\n presione cualquier tecla para continuar ");
Console.ReadKey();
System.Threading.Thread.Sleep(100);
        }

    }
}