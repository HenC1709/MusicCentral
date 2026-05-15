using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Canciones.Models;

namespace Canciones.Services
{
   
    public class AuthService
    {
        private string ruta = Path.Combine("Data", "BaseDeUsuarios.json");

        public Usuario? IniciarSesion(string nombre, string id)
        {
            if (!File.Exists(ruta))
                return null;

            try
            {
                string contenido = File.ReadAllText(ruta);

                var listaUsuarios =
                    JsonSerializer.Deserialize<List<Usuario>>(contenido);

                var user = listaUsuarios?.FirstOrDefault(u =>
                    u.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));

                if (user != null && user.ID == id)
                {
                    return user;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return null;
        }
    }
}
