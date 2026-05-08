using Canciones.Models;

namespace Canciones.Logic
{
    class Lista
{
    public static List<Musica> Canciones = new List<Musica>();

    public static void MostrarLista()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("LISTA DE AÑADIDOS! ");
        Console.ResetColor();

        if (Canciones.Count == 0)
        {
         Console.WriteLine("Epa mi loco esto esta vacio xd");
        }
        else
        {
            foreach (var item in Canciones)
            {
                //Propiedades de la clase musica
                Console.WriteLine("-" + item.ToString());
            }
        }
    }
}
}
