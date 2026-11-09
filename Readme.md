# 🎵 Music Central — Arquitectura & Camino a SQL

## 🚀 Evolución del Proyecto

El proyecto comenzó como una aplicación de consola en C# enfocada en la gestión básica de canciones y usuarios con persistencia en archivos JSON.

---

# 🧱 Arquitectura Actual (Preparada para SQL)

```text
Canciones/
│
├── Data/
│   ├── BaseDeUsuarios.json
│   ├── CancionesAgregadas.json
│   ├── Playlist.json
│   └── Repositories/
│       ├── ICancionRepository.cs
│       ├── IUsuarioRepository.cs
│       ├── IPlaylistRepository.cs
│       ├── Json/
│       │   ├── JsonCancionRepository.cs
│       │   ├── JsonUsuarioRepository.cs
│       │   └── JsonPlaylistRepository.cs
│       └── Sql/
│           ├── README_SQL.md            <-- DDL y guía de migración
│           └── SqlRepositoryBlueprint.cs <-- Plantilla de implementación SQL
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
│   ├── AuthService.cs
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

## 🐛 Bugs Detectados y Corregidos en Código (con comentarios explicativos)

1. **Bug Crítico en `PlaylistManager.cs` (Doble negación `!!`)**:
   - `if (!!string.IsNullOrEmpty(contenido))` provocaba que cuando `Playlist.json` tenía canciones, la condición evaluara a `false`, por lo que **nunca se cargaban las playlists existentes**. Y si estaba vacío, intentaba deserializar texto vacío arrojando excepción.
   - *Solución:* Se corrigió a `if (!string.IsNullOrWhiteSpace(contenido))`.
2. **Bug de Búsqueda Vacía en `MusicManager.cs`**:
   - Al presionar Enter en blanco, `"".Contains("")` siempre devolvía la primera canción de la lista como encontrada.
   - *Solución:* Validación de cadenas vacías/espacios y retorno de lista completa de coincidencias.
3. **Falta de Validación de Directorio `Data/`**:
   - `File.WriteAllText` lanzaba `DirectoryNotFoundException` si el directorio `Data` no existía previamente.
   - *Solución:* Verificación y creación automática (`Directory.CreateDirectory`).
4. **Violación SRP en `Usuario.cs`**:
   - El modelo de datos leía/escribía archivos JSON y capturaba entrada de consola (`Console.ReadLine`).
   - *Solución:* Se separó la persistencia en `IUsuarioRepository` y se limpió el modelo POCO.
5. **Inconsistencia de Validación en ID**:
   - El mensaje indicaba "(máximo 4)", pero el código requería exactamente 4 números. Se sincronizó el mensaje y la validación.
6. **Convenciones de Nombres C# y Archivos**:
   - `Models/Musicas.cs` renombrado a `Models/Musica.cs`.
   - `Services/AuthServices.cs` renombrado a `Services/AuthService.cs`.
   - Métodos y propiedades en minúscula (`iniciarSesion`, `playlists`) ajustados a PascalCase (`IniciarSesion`, `Playlists`).

---

# 🗄️ Camino Trazado: De JSON a SQL

Para evitar sobreingeniería pero dejar el camino totalmente listo:

1. **Patrón Repositorio (`IRepository`)**:
   - Los servicios (`MusicManager`, `PlaylistManager`, `AuthService`) ya no interactúan con archivos JSON directos, sino con interfaces (`ICancionRepository`, `IUsuarioRepository`, `IPlaylistRepository`).
2. **Entidades con Llaves Primarias**:
   - Modelos (`Musica`, `Playlist`, `Usuario`) incorporan la propiedad `Id` (PK) sin romper la deserialización de los JSON actuales.
3. **Migración a SQL sin tocar la lógica de negocio**:
   - Para cambiar a SQL Server, SQLite o PostgreSQL, solo se crea la clase `SqlCancionRepository : ICancionRepository` y en `Program.cs` se cambia una línea:
   ```csharp
   // Antes (JSON):
   ICancionRepository cancionRepo = new JsonCancionRepository();

   // Después (SQL):
   ICancionRepository cancionRepo = new SqlCancionRepository(cadenaConexion);
   ```
4. Consulta `Data/Repositories/Sql/README_SQL.md` para el script DDL con las tablas normalizadas (`Usuarios`, `Canciones`, `Playlists`, `PlaylistCanciones`).
# 📘 Arquitectura del Proyecto & Guía de Migración

## 🎯 Resumen de la Refactorización

Este proyecto ha sido refactorizado para separar responsabilidades mediante el **Patrón Repositorio**, corrigiendo bugs críticos de persistencia y dejando el camino preparado para evolucionar de almacenamiento **JSON** a una base de datos relacional **SQL** sin sobreingeniería.

---

## 🏗️ Diagrama de Arquitectura

```
[UI: Menu / LoginService]
        │
        ▼
[Services: MusicManager / PlaylistManager / AuthService]
        │
        ▼
[Data Abstractions: ICancionRepository / IUsuarioRepository / IPlaylistRepository]
       /                                                        \
      ▼                                                          ▼
[Json Repositories (ACTIVO)]                               [Sql Repositories (FUTURO)]
  - JsonCancionRepository                                    - SqlCancionRepository
  - JsonUsuarioRepository                                    - SqlUsuarioRepository
  - JsonPlaylistRepository                                   - SqlPlaylistRepository
```

---

## 🐛 Bugs Resueltos y Señalizados en Código

1. **`PlaylistManager.cs` - Doble Negación (`!!`)**:
   - `if (!!string.IsNullOrEmpty(contenido))` causaba que las playlists existentes nunca se cargaran desde `Playlist.json`.
   - *Solución:* Corregido a `if (!string.IsNullOrWhiteSpace(contenido))`.
2. **`MusicManager.cs` - Búsqueda en Blanco**:
   - `"".Contains("")` coincidía con todas las canciones, devolviendo falsos positivos al presionar Enter sin texto.
   - *Solución:* Validación de cadenas vacías y retorno de todas las coincidencias.
3. **Persistencia - Falta de Directorio `Data/`**:
   - `File.WriteAllText` arrojaba excepción si la carpeta `Data` no existía en el entorno.
   - *Solución:* Creación automática preventiva del directorio antes de escribir.
4. **`Usuario.cs` - Violación SRP**:
   - El modelo mezclaba captura de consola, deserialización y guardado en archivo.
   - *Solución:* Modelo convertido en POCO; persistencia delegada a `IUsuarioRepository`.
5. **`PlaylistManager.cs` - Integridad Referencial**:
   - Se podían agregar canciones ficticias o inexistentes a las playlists.
   - *Solución:* Conexión con `ICancionRepository` para validar que la canción exista en el catálogo (preparación para Foreign Keys en SQL).

---

## 🗄️ Transición a SQL en 2 Pasos

1. **Crear las tablas en SQL**:
   Ejecutar el script DDL ubicado en `Data/Repositories/Sql/README_SQL.md` (`Usuarios`, `Canciones`, `Playlists`, `PlaylistCanciones`).

2. **Reemplazar la inyección en `Program.cs`**:
   ```csharp
   // Reemplazar:
   ICancionRepository cancionRepo   = new JsonCancionRepository();
   IUsuarioRepository usuarioRepo   = new JsonUsuarioRepository();
   IPlaylistRepository playlistRepo = new JsonPlaylistRepository();

   // Por:
   ICancionRepository cancionRepo   = new SqlCancionRepository(cadenaConexion);
   IUsuarioRepository usuarioRepo   = new SqlUsuarioRepository(cadenaConexion);
   IPlaylistRepository playlistRepo = new SqlPlaylistRepository(cadenaConexion);
   ```
   **Ningún servicio, menú ni lógica de la aplicación requerirá cambios.**

