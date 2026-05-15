using System;

namespace Canciones.Models
{
    public class Musica
{
    public string Cancion { get; set; } = "";
    public string Autor { get; set; } = "";
    public DateTime FechaDeIngreso { get; set; }

 public Musica()
        {
            FechaDeIngreso = DateTime.Now;
        }
    // Lista donde se guardan canciones
    public Musica(string cancion, string autor)
    {
        Cancion = cancion;
        Autor = autor;
        FechaDeIngreso = DateTime.Now;
    }

    public override string ToString()
    {
        return $"{Cancion} - {Autor} - {FechaDeIngreso.ToLongTimeString()}";
    }
 
}

public class Playlist
    {
        public string Nombre {get; set;} = "";
        public string UsuarioCreador {get; set;} = "";
        public List <string> Canciones { get; set; } = new();

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return $"{Nombre} - {Canciones.Count} canciones - {UsuarioCreador}";
        }
    }
   
}
