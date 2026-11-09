using System;
using System.Collections.Generic;
using System.Linq;
using Canciones.Data.Repositories;
using Canciones.Data.Repositories.Json;
using Canciones.Logic;
using Canciones.Models;

namespace Canciones.Services
{
    // =========================================================================================
    // 🗄️ EVOLUCIÓN A SQL & 🐛 BUGS RESUELTOS:
    // =========================================================================================
    // 🐛 BUG 1 (Búsqueda vacía):
    //    En la versión anterior:
    //      var resultado = Canciones.FirstOrDefault(c => c.Cancion.ToLower().Contains(buscar));
    //    Si el usuario presionaba Enter sin escribir nada, 'buscar' era "" y "".Contains("") era true
    //    para toda canción, mostrando erróneamente que encontró la primera canción de la lista.
    //
    // 🐛 BUG 2 (Un solo resultado):
    //    Se usaba FirstOrDefault(), mostrando solo 1 canción aunque existieran 20 coincidencias.
    //
    // 🐛 ACOPLAMIENTO DE PERSISTENCIA Y FALTA DE DIRECTORIO:
    //    MusicManager contenía File.WriteAllText sin asegurar que la carpeta "Data" existiera.
    //    Ahora delega toda la persistencia a 'ICancionRepository'.
    //
    // 🗄️ CAMINO A SQL:
    //    Al inyectar 'ICancionRepository', MusicManager no sabe ni le importa si las canciones
    //    vienen de un archivo .json o de una tabla SQL Server / SQLite.
    // =========================================================================================
    public class MusicManager
    {
        private readonly ICancionRepository _cancionRepository;
        private readonly TicketManager _ticket;

        // Constructor con Inyección de Dependencias
        public MusicManager(ICancionRepository cancionRepository, TicketManager? ticketManager = null)
        {
            _cancionRepository = cancionRepository;
            _ticket = ticketManager ?? new TicketManager();
        }

        // Constructor por defecto (usa el repositorio JSON)
        public MusicManager() : this(new JsonCancionRepository())
        {
        }

        public List<Musica> Canciones => _cancionRepository.GetAll();

        public void AgregarMusica(string titulo, string autor, string nombreUsuario)
        {
            if (string.IsNullOrWhiteSpace(titulo) || string.IsNullOrWhiteSpace(autor))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[X] El título y el autor no pueden estar vacíos.");
                Console.ResetColor();
                return;
            }

            Musica nueva = new Musica(titulo.Trim(), autor.Trim());
            _cancionRepository.Add(nueva);

            _ticket.GenerarTicket(nueva, nombreUsuario);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nCanción agregada y ticket creado exitosamente!");
            Console.ResetColor();
        }

        // 🐛 CORREGIDO: Búsqueda con validación de entrada vacía y soporte de múltiples resultados
        public void BuscarMusica()
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("Ingrese el nombre o autor de la Canción: ");
            string buscar = Console.ReadLine()?.Trim() ?? "";

            // 🐛 BUG PREVENIDO: Evitar que texto vacío coincida con todo
            if (string.IsNullOrWhiteSpace(buscar))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n[!] Debe ingresar al menos un término para buscar.");
                Console.ResetColor();
                return;
            }

            var resultados = _cancionRepository.Search(buscar);

            if (resultados.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n[ENCONTRADAS {resultados.Count} COINCIDENCIA(S)]:");
                foreach (var item in resultados)
                {
                    Console.WriteLine($"  ➤ {item}");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[X] Canción No Encontrada.");
            }
            Console.ResetColor();
        }

        public void MostrarLista()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("=== LISTA DE CANCIONES ====\n");
            Console.ResetColor();

            var lista = _cancionRepository.GetAll();

            if (lista.Count == 0)
            {
                Console.WriteLine("La biblioteca de canciones está vacía actualmente.");
                return;
            }

            foreach (var item in lista)
            {
                Console.WriteLine($"- {item}");
            }
        }
    }
}
