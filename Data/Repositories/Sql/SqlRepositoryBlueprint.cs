using System;
using System.Collections.Generic;
using Canciones.Models;

namespace Canciones.Data.Repositories.Sql
{
    // =========================================================================================
    // 🗄️ BLUEPRINT / PLANTILLA: IMPLEMENTACIÓN FUTURA EN SQL
    // =========================================================================================
    // NO está activa actualmente, pero deja el camino 100% trazado para cuando decidas
    // cambiar de JSON a SQL (usando Entity Framework Core, Dapper o Microsoft.Data.SqlClient).
    //
    // PASOS PARA ACTIVAR SQL:
    // 1. Instalar paquete NuGet:
    //      dotnet add package Microsoft.Data.SqlClient
    //      (o dotnet add package Microsoft.EntityFrameworkCore.SqlServer)
    // 2. Ejecutar el script SQL de creación de tablas (ver README_SQL.md en esta misma carpeta).
    // 3. En Program.cs, simplemente reemplazar:
    //      ICancionRepository cancionRepo = new JsonCancionRepository();
    //    Por:
    //      ICancionRepository cancionRepo = new SqlCancionRepository(cadenaConexion);
    //
    // ¡Listo! Ningún controlador, menú o servicio requerirá cambios porque todos dependen
    // de la interfaz ICancionRepository.
    // =========================================================================================
    public class SqlCancionRepositoryBlueprint : ICancionRepository
    {
        private readonly string _connectionString;

        public SqlCancionRepositoryBlueprint(string connectionString = "Server=localhost;Database=MusicaCentral;Trusted_Connection=True;")
        {
            _connectionString = connectionString;
        }

        public List<Musica> GetAll()
        {
            // EJEMPLO CON DAPPER / SQL:
            // using var conn = new SqlConnection(_connectionString);
            // return conn.Query<Musica>("SELECT Id, Cancion, Autor, FechaDeIngreso FROM Canciones").AsList();
            throw new NotImplementedException("Plantilla demostrativa: Implementar conexión SQL cuando se configure el servidor.");
        }

        public Musica? GetById(int id)
        {
            // using var conn = new SqlConnection(_connectionString);
            // return conn.QueryFirstOrDefault<Musica>("SELECT * FROM Canciones WHERE Id = @Id", new { Id = id });
            throw new NotImplementedException("Plantilla demostrativa.");
        }

        public List<Musica> Search(string query)
        {
            // using var conn = new SqlConnection(_connectionString);
            // string sql = "SELECT * FROM Canciones WHERE Cancion LIKE '%' + @Q + '%' OR Autor LIKE '%' + @Q + '%'";
            // return conn.Query<Musica>(sql, new { Q = query }).AsList();
            throw new NotImplementedException("Plantilla demostrativa.");
        }

        public void Add(Musica cancion)
        {
            // using var conn = new SqlConnection(_connectionString);
            // string sql = @"INSERT INTO Canciones (Cancion, Autor, FechaDeIngreso) 
            //                VALUES (@Cancion, @Autor, @FechaDeIngreso);
            //                SELECT CAST(SCOPE_IDENTITY() as int);";
            // cancion.Id = conn.QuerySingle<int>(sql, cancion);
            throw new NotImplementedException("Plantilla demostrativa.");
        }

        public void SaveChanges()
        {
            // En SQL, las transacciones o el DbContext.SaveChanges() manejan esto automáticamente.
        }
    }
}

