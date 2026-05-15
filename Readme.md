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
```

---

# 🔧 V2 Refactor Update

Durante el desarrollo de la V2, el proyecto comenzó a crecer en complejidad debido a:

- múltiples sistemas
- persistencia JSON
- roles
- playlists
- autenticación
- lógica separada

Esto llevó a iniciar un proceso gradual de refactorización y separación de responsabilidades.

---

## ✅ Nuevos Cambios Arquitectónicos

### 🔐 AuthService
Se creó un nuevo servicio dedicado exclusivamente a la autenticación de usuarios.

Responsabilidades:
- validar credenciales
- iniciar sesión
- lectura de usuarios para autenticación

Esto permitió desacoplar la lógica de login del sistema UI.

---

## 📂 Playlist System Improvements

Se integró:
- `Playlist.cs`
- `PlaylistManager.cs`
- `Playlists.json`

Con soporte para:
- crear playlists
- agregar canciones
- visualizar playlists
- persistencia automática

---

## 🧠 Mejora del Menú Principal

El menú fue reorganizado para:
- evitar opciones duplicadas
- mejorar separación por roles
- mejorar navegación

Usuarios normales y administradores ahora visualizan diferentes opciones de forma más limpia.

***

## ⚠️ Arquitectura Actual

El proyecto actualmente se encuentra en transición hacia una estructura más modular.

Se identificó que algunas clases poseen demasiadas responsabilidades, especialmente:

- `Usuario`

Esto será mejorado progresivamente en futuras versiones mediante:
- `UsuarioService`
- separación UI/lógica
- validaciones dedicadas

---

# 🛣️ Próximos Objetivos

## 🔥 V2.5
- conectar playlists con canciones reales
- validaciones avanzadas
- evitar canciones inexistentes

---

## 🔥 V3
- favoritos
- shuffle
- filtros
- paginación
- estadísticas

---

## 🔥 V3.5
- refactor completo de arquitectura
- separación de persistencia
- interfaces
- diseño más escalable