# 🛣️ Guía de Migración: De JSON a SQL

Esta carpeta contiene el diseño de arquitectura preparado para transformar la persistencia basada en archivos JSON a un motor de base de datos relacional SQL (SQL Server, SQLite, PostgreSQL o MySQL).

---

## 📐 1. Script de Base de Datos (DDL)

Cuando decidas crear la base de datos SQL, puedes ejecutar este script:

```sql
-- 1. Tabla de Usuarios
CREATE TABLE Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL UNIQUE,
    ID NVARCHAR(10) NOT NULL, -- PIN o clave de acceso (en producción se debe hashear)
    Rol INT NOT NULL DEFAULT 0 -- 0: Usuario, 1: Admin
);

-- 2. Tabla de Canciones
CREATE TABLE Canciones (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Cancion NVARCHAR(200) NOT NULL,
    Autor NVARCHAR(200) NOT NULL,
    FechaDeIngreso DATETIME2 NOT NULL DEFAULT GETDATE()
);

-- 3. Tabla de Playlists
CREATE TABLE Playlists (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(150) NOT NULL UNIQUE,
    UsuarioCreador NVARCHAR(100) NOT NULL,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE()
);

-- 4. Tabla Intermedia Muchos-a-Muchos: PlaylistCanciones
-- (En JSON se guardaban como List<string>, en SQL se normaliza en 3FN)
CREATE TABLE PlaylistCanciones (
    PlaylistId INT NOT NULL FOREIGN KEY REFERENCES Playlists(Id) ON DELETE CASCADE,
    CancionId INT NOT NULL FOREIGN KEY REFERENCES Canciones(Id) ON DELETE CASCADE,
    FechaAgregada DATETIME2 NOT NULL DEFAULT GETDATE(),
    PRIMARY KEY (PlaylistId, CancionId)
);
```

---

## 🔄 2. Cómo realizar la transición en el código

Gracias al **Patrón Repositorio** que dejamos estructurado, el cambio a SQL requiere **0 modificaciones** en:
- `MusicManager.cs`
- `PlaylistManager.cs`
- `AuthService.cs`
- `Menu.cs`

Solo necesitas:
1. Crear tu repositorio SQL implementando las interfaces existentes (`ICancionRepository`, `IUsuarioRepository`, `IPlaylistRepository`).
2. En `Program.cs`, cambiar la inyección:

```csharp
// ANTES (JSON):
ICancionRepository cancionRepo = new JsonCancionRepository();
IUsuarioRepository usuarioRepo = new JsonUsuarioRepository();
IPlaylistRepository playlistRepo = new JsonPlaylistRepository();

// DESPUÉS (SQL):
ICancionRepository cancionRepo = new SqlCancionRepository(connectionString);
IUsuarioRepository usuarioRepo = new SqlUsuarioRepository(connectionString);
IPlaylistRepository playlistRepo = new SqlPlaylistRepository(connectionString);
```

