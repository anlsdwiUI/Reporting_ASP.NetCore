using Microsoft.Data.SqlClient;
using Reporting.Models;
using System.Data;

namespace Reporting.Data
{
    public class PersonRepository : IPersonRepository
    {
        private readonly string _connectionString;

        public PersonRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Person> GetAllPersons()
        {
            var result = new List<Person>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("GetAllPersons", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Person
                {
                    Name = reader["Name"].ToString(),
                    Age = Convert.ToInt32(reader["Age"]),
                    BirthDay = Convert.ToDateTime(reader["BirthDay"]),
                    Job = reader["Job"].ToString(),
                    Education = reader["Education"].ToString()
                });
            }

            return result;
        }
    }
}
