using Canciones.UI;
using Canciones.Models;
using Canciones.Services;
using System;
class Program
{
    static void Main()
    {
          
        LoginService auth = new LoginService();
        Usuario? usuarioActual = null;
        while (usuarioActual == null)
        {
            usuarioActual = auth.iniciarSesion();
        }
        MusicManager manager = new MusicManager();
       PlaylistManager playlistManager = new PlaylistManager();

        Menu.MostrarMenuUsuario(usuarioActual, manager, playlistManager);
    }
}