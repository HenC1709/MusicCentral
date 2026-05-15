# 🎵 Music Central — V2 Playlist Update

## 🚀 Evolución del Proyecto

El proyecto comenzó como una aplicación de consola en C# enfocada en la gestión básica de canciones y usuarios.

La primera versión (V1) ya contaba con:

- Sistema de Login
- Registro de usuarios
- Roles (`Admin` / `Usuario`)
- Persistencia con JSON
- Gestión de canciones
- Búsqueda de canciones
- Sistema de tickets/logs
- Menú interactivo en consola

Con esta actualización, el proyecto evoluciona hacia una arquitectura más modular y cercana a una aplicación real de gestión musical.

---

# 🧱 Arquitectura Actual

```text
Canciones/
│
├── Data/
│   ├── BaseDeUsuarios.json
│   ├── CancionesAgregadas.json
│   └── Playlists.json
│
├── Logic/
│   └── TicketManager.cs
│
├── Models/
│   ├── Musica.cs
│   ├── Usuario.cs
│   └── Playlist.cs
│
├── Services/
│   ├── LoginService.cs
│   ├── MusicManager.cs
│   └── PlaylistManager.cs
│
├── UI/
│   └── Menu.cs
│
└── Program.cs