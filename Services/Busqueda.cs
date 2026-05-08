using Canciones.Logic;

namespace Canciones.Services
{
    class Busqueda
{

    public void buscar(Agregar miLista)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Ingrese el nombre de la Canción: ");

        string buscar = Console.ReadLine()!;

        var resultado = Lista.Canciones.Find(l => l.Cancion.Contains(buscar));

         
        if (resultado != null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Encontrada {resultado.Cancion} de {resultado.Autor}");
            Console.Clear();
    
        }
        
        // ahora mandamos la cancion a la lista 
      
      else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Canción No Encontrada");
        }
            
    }

}
}
