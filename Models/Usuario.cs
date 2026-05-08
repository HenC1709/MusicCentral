using System.Text.Json;
using System.IO;
using System.Dynamic;
using System.Runtime.InteropServices.Marshalling;
namespace Canciones.Models
{
    public enum Rol
{
    Usuario,

    Admin
}
public class Usuario
{
    public string Nombre {get; set;} ="";
    public string ID {get; set;} ="";
   public Rol Rol { get; set; } = Rol.Usuario;
    // como usar json para listas
public void GuardarUsuario(Usuario NuevoUsuario)
    {
        string ruta = Path.Combine("Data", "BaseDeUsuarios.json");
        List<Usuario> ListaActual = new List<Usuario>();
      //1.  si el archivo existe se lee de una vez 
        if (File.Exists(ruta))
        { // pasamos el resto a una lista
        string contenido = File.ReadAllText(ruta);
        if (!string.IsNullOrEmpty(contenido))
            {
                ListaActual = JsonSerializer.Deserialize<List<Usuario>>(contenido)!;
            }            
        }
     //2. agregamos nuevos usuairos xd
      ListaActual.Add(NuevoUsuario);
      //3. convertimos la nueva listo a texto JSON
      var opciones = new JsonSerializerOptions {WriteIndented = true };
      string jsonfinal = JsonSerializer.Serialize(ListaActual, opciones);
      // 4. ahora guardamos todo
      File.WriteAllText(ruta, jsonfinal);
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine("Usuario guardado en la base de datos.json! ");
      Console.ResetColor();
    }
// nueva referencia para ver usuarios en JSON
    public static void VerUsuarios()
    {
        string ruta = Path.Combine("Data", "BaseDeUsuarios.json");
        if (File.Exists(ruta))
        {
            string contenido = File.ReadAllText(ruta); // este solo lee, no modifica
            var lista = JsonSerializer.Deserialize<List<Usuario>>(contenido);
            Console.WriteLine("Lista de usuarios registrados");
            foreach (var u in lista!)
            {
                Console.WriteLine($"- {u.Nombre} (ID: {u.ID})");
            }
        }
    }
    public void Registro()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Ingrese nombre para registro: ");
        Nombre = Console.ReadLine()!;

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Ingrese una ID válida (solo números): ");
        ID = Console.ReadLine()!;

        // Validar que solo tenga números
        while (!EsSoloNumeros(ID))
        { 
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Error: La ID solo debe contener números. (maximo 4)");
            Console.ResetColor();

            Console.WriteLine("Ingrese una ID válida: ");
            ID = Console.ReadLine()!;
        }
         
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Registro exitoso!");
        GuardarUsuario(this);

        if (this.Nombre.ToLower() == "Henry")
        {
            this.Rol = Rol.Admin;
        }

        else
        {
            this.Rol = Rol.Usuario;
        }
    }

    
    private bool EsSoloNumeros(string texto)
    {
        if (texto.Length != 4) return false;
        foreach (char c in texto)
        {
            if (!char.IsDigit(c))
            {
                return false;
            }
        }

        return true;
    }
}
}

