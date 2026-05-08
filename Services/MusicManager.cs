using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Canciones.Models;

namespace Canciones.Services
{
    public class MusicManager
    {
        private string ruta = Path.Combine("Data", "CancionesAgregadas.json");
        public List<Musica> Canciones { get; private set; } = new List<Musica>();

        public MusicManager()
        {
            CargarDatos();
        }

        private void CargarDatos()
        {
            if (File.Exists(ruta))
            {
                string contenido = File.ReadAllText(ruta);
                if (!string.IsNullOrEmpty(contenido))
                {
                    Canciones = JsonSerializer.Deserialize<List<Musica>>(contenido) ?? new List<Musica>();
                }
            }
        }

        private void GuardarDatos()
        {
            var opciones = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(Canciones, opciones);
            File.WriteAllText(ruta, json);
        }

        public void AgregarMusica(string titulo, string autor)
        {
            Musica nueva = new Musica(titulo, autor);
            Canciones.Add(nueva);
            GuardarDatos();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nCanción agregada y guardada de forma exitosa! ");
            Console.ResetColor();
        }
        public void BuscarMusica()
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("Ingrese el nombre de la Canción: ");
            string buscar = Console.ReadLine()?.ToLower() ?? "";

            var resultado = Canciones.FirstOrDefault(c => c.Cancion.ToLower().Contains(buscar));
            if (resultado != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n[ENCONTRADA] {resultado}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[X] Canción No Encontrada");
            }
            Console.ResetColor();
        }
        public void MostrarLista()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("=== LISTA DE CANCIONES ====");
            Console.ResetColor();

            if (Canciones.Count == 0)
            {
                Console.WriteLine("EPA mi loco esto esta vacio xd");
                return;
            }
            foreach (var item in Canciones)
            {
                Console.WriteLine($"- {item}");
            }
        }
    }
}