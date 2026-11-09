using System;
using Canciones.Data.Repositories;
using Canciones.Data.Repositories.Json;
using Canciones.Models;
using Canciones.Services;
using Canciones.UI;

class Program
{
    static void Main()
    {
        // =====================================================================================
        // 🗄️ COMPOSICIÓN DE DEPENDENCIAS & CAMINO HACIA SQL:
        // =====================================================================================
        // Actualmente utilizamos los repositorios basados en JSON.
        // Cuando decidas evolucionar a SQL (SQL Server, SQLite, PostgreSQL, etc.):
        // ÚNICAMENTE cambias la instanciación de estos repositorios:
        //
        //   string connectionString = "Server=localhost;Database=MusicaCentral;Trusted_Connection=True;";
        //   IUsuarioRepository usuarioRepo   = new SqlUsuarioRepository(connectionString);
        //   ICancionRepository cancionRepo   = new SqlCancionRepository(connectionString);
        //   IPlaylistRepository playlistRepo = new SqlPlaylistRepository(connectionString);
        //
        // Todo el resto de la aplicación (Servicios, UI, Lógica de Negocio) permanecerá intacto.
        // =====================================================================================

        IUsuarioRepository usuarioRepo = new JsonUsuarioRepository();
        ICancionRepository cancionRepo = new JsonCancionRepository();
        IPlaylistRepository playlistRepo = new JsonPlaylistRepository();

        AuthService authService = new AuthService(usuarioRepo);
        LoginService loginService = new LoginService(authService);
        MusicManager musicManager = new MusicManager(cancionRepo);
        PlaylistManager playlistManager = new PlaylistManager(playlistRepo, cancionRepo);

        // 🔐 Flujo de inicio de sesión / Registro
        Usuario? usuarioActual = null;
        while (usuarioActual == null)
        {
            usuarioActual = loginService.IniciarSesion();
        }

        // 🎵 Menú principal del sistema
        Menu.MostrarMenuUsuario(usuarioActual, musicManager, playlistManager, authService);
    }
}
