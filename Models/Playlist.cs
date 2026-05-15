using System;
using System.Collections.Generic;


namespace Canciones.Models
{
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

