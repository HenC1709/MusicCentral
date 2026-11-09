using System.Collections.Generic;
using Canciones.Models;

namespace Canciones.Data.Repositories
{
    // =========================================================================================
    // 🗄️ CAMINO DE EVOLUCIÓN: JSON -> SQL (Repository Pattern)
    // =========================================================================================
    // Interface que abstrae el almacenamiento de usuarios.
    // 
    // En SQL:
    //    public class SqlUsuarioRepository : IUsuarioRepository
    //    {
    //        // Implementación con consultas SQL:
    //        // GetByNombre("Henry") -> "SELECT * FROM Usuarios WHERE Nombre = @nombre"
    //        // Add(usuario)         -> "INSERT INTO Usuarios (Nombre, ID, Rol) VALUES (@Nombre, @ID, @Rol)"
    //        // GetAll()             -> "SELECT * FROM Usuarios ORDER BY Nombre"
    //    }
    // =========================================================================================
    public interface IUsuarioRepository
    {
        List<Usuario> GetAll();
        Usuario? GetByNombre(string nombre);
        Usuario? GetById(string id);
        Usuario? ValidateCredentials(string nombre, string id);
        void Add(Usuario usuario);
        bool Exists(string nombre, string id);
    }
}

