using System;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;


namespace MiProyecto;
class Musica
{
    public string Cancion { get; set; } = "";
    public string Autor { get; set; } = "";

 public static void GuardarCanciones(Musica NuevaCancion)
    {
        string ruta = "CancionesAgregadas.json";
        List<Musica> lista = new List<Musica>();
        if (File.Exists(ruta))
        {
            string contenido = File.ReadAllText(ruta);
            if (!string.IsNullOrEmpty(contenido))
            {
                lista = JsonSerializer.Deserialize<List<Musica>>(contenido)!;
            }
        }
        lista.Add(NuevaCancion);
        var opciones = new JsonSerializerOptions { WriteIndented = true };
        string jsonfinal = JsonSerializer.Serialize(lista, opciones);
        File.WriteAllText(ruta, jsonfinal);
        Console.WriteLine("cancion guardada en la base de datos.json! ");
        Console.ResetColor();

    }
    public static void CargarDatos()
    {
        string ruta = "CancionesAgregadas.json";
        if (File.Exists(ruta))
        {
            string contenido = File.ReadAllText(ruta);
            Lista.Canciones = JsonSerializer.Deserialize<List<Musica>>(contenido) ?? new List<Musica>();
        }
    }
    // Lista donde se guardan canciones
    public Musica(string cancion, string autor)
    {
        Cancion = cancion;
        Autor = autor;
    }

    public override string ToString()
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        return $"{Cancion} - {Autor}";
    }
 
}
   