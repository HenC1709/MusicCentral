using Canciones.Logic;
using Canciones.Models;

namespace Canciones.Services
{
    class Agregar
{
    public void AgregarMusica(Musica nuevaCancion)
    {
        Lista.Canciones.Add(nuevaCancion);

         Musica.GuardarCanciones(nuevaCancion);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Cancion agregada exitosamente");       
    }

}
}
