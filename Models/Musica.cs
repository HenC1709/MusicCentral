using System;

namespace Canciones.Models
{
    // =========================================================================================
    // 🗄️ EVOLUCIÓN DE JSON A SQL:
    // En JSON las entidades se guardan como documentos planos sin identificador numérico único.
    // Para migrar a SQL (ej. SQL Server, PostgreSQL, SQLite con Entity Framework Core o Dapper):
    // 1. Toda tabla en base de datos relacional requiere una Llave Primaria (Primary Key / PK).
    //    Se agregó la propiedad 'Id' (int) para preparar el modelo a SQL: [Key] IDENTITY(1,1).
    // 2. La propiedad 'Cancion' representa el título; en SQL se suele mapear como 'Titulo' VARCHAR(200).
    // 3. 'FechaDeIngreso' se mapea como DATETIME2 / TIMESTAMP.
    // =========================================================================================
    public class Musica
    {
        // 🗄️ SQL: Clave primaria (PK). En JSON existente puede ser 0 o nulo sin romper la deserialización.
        public int Id { get; set; }

        public string Cancion { get; set; } = "";
        public string Autor { get; set; } = "";
        public DateTime FechaDeIngreso { get; set; }

        public Musica()
        {
            FechaDeIngreso = DateTime.Now;
        }

        public Musica(string cancion, string autor)
        {
            Cancion = cancion;
            Autor = autor;
            FechaDeIngreso = DateTime.Now;
        }

        public Musica(int id, string cancion, string autor, DateTime fechaDeIngreso)
        {
            Id = id;
            Cancion = cancion;
            Autor = autor;
            FechaDeIngreso = fechaDeIngreso;
        }

        public override string ToString()
        {
            return $"{Cancion} - {Autor} - {FechaDeIngreso.ToLongTimeString()}";
        }
    }
}
