using System.Collections.Generic;
using Canciones.Models;

namespace Canciones.Data.Repositories
{
    // =========================================================================================
    // 🗄️ CAMINO DE EVOLUCIÓN: JSON -> SQL (Repository Pattern)
    // =========================================================================================
    // Interface que abstrae el almacenamiento y persistencia de playlists.
    // 
    // En SQL:
    //    public class SqlPlaylistRepository : IPlaylistRepository
    //    {
    //        // En BD relacional se maneja mediante JOIN con la tabla intermedia PlaylistCanciones:
    //        // GetByNombre:
    //        //   "SELECT p.Id, p.Nombre, p.UsuarioCreador, p.FechaCreacion, c.Cancion
    //        //    FROM Playlists p
    //        //    LEFT JOIN PlaylistCanciones pc ON p.Id = pc.PlaylistId
    //        //    LEFT JOIN Canciones c ON pc.CancionId = c.Id
    //        //    WHERE p.Nombre = @nombre"
    //    }
    // =========================================================================================
    public interface IPlaylistRepository
    {
        List<Playlist> GetAll();
        Playlist? GetByNombre(string nombre);
        void Add(Playlist playlist);
        void Update(Playlist playlist);
        void SaveChanges();
    }
}

