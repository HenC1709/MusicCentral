using System.Collections.Generic;
using Canciones.Models;

namespace Canciones.Data.Repositories
{
    // =========================================================================================
    // 🗄️ CAMINO DE EVOLUCIÓN: JSON -> SQL (Repository Pattern)
    // =========================================================================================
    // El patrón Repositorio desacopla la lógica de negocio (Services/MusicManager) de la fuente
    // de datos física.
    // 
    // Actualmente: Se implementa mediante 'JsonCancionRepository', que lee y escribe archivos .json.
    // En el futuro (SQL): Se implementa mediante 'SqlCancionRepository' usando EF Core o Dapper:
    //    public class SqlCancionRepository : ICancionRepository
    //    {
    //        // Implementación con consultas SQL:
    //        // GetAll()  -> "SELECT Id, Cancion, Autor, FechaDeIngreso FROM Canciones"
    //        // Add(c)    -> "INSERT INTO Canciones (Cancion, Autor, FechaDeIngreso) VALUES (@Cancion, @Autor, @FechaDeIngreso)"
    //        // Search(q) -> "SELECT * FROM Canciones WHERE LOWER(Cancion) LIKE '%' + @q + '%'"
    //    }
    //
    // 💡 Ventaja: Ningún servicio ni interfaz de usuario se verá afectado al cambiar a SQL.
    // =========================================================================================
    public interface ICancionRepository
    {
        List<Musica> GetAll();
        Musica? GetById(int id);
        List<Musica> Search(string query);
        void Add(Musica cancion);
        void SaveChanges();
    }
}

