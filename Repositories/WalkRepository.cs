using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using DogGoMVC.Models;

namespace DogGoMVC.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly IConfiguration _config;

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public WalkRepository(IConfiguration config)
        {
            _config = config;
        }

        public List<Walk> GetAllWalks()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT w.Id AS 'Walk Id', w.Date, w.Duration, w.WalkerId, w.DogId,
                               wr.Id AS 'Walker Id', wr.[Name] AS 'Walker Name', wr.NeighborhoodId AS 'Walkers NeighborhoodId', wr.ImageUrl AS 'Walker Img',
                               n.Id AS 'Hood Id', n.[Name] AS 'Hood Name',
                               d.Id AS 'Dog Id', d.[Name] AS 'Dog Name', d.Breed, d.ImageUrl AS 'Dog Img',
                               o.Id AS 'Owner Id', o.[Name] AS 'Owner Name', o.Email, o.[Address], o.Phone, o.NeighborhoodId AS 'Owners NeighborhoodId'
                        FROM Walks w
                            LEFT JOIN Walker wr ON w.WalkerId = wr.Id
                            LEFT JOIN Neighborhood n ON wr.NeighborhoodId = n.Id
                            LEFT JOIN Dog d ON w.DogId = d.Id
                            LEFT JOIN Owner o ON d.OwnerId = o.Id
                    ";

                    

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Walk> walks = new List<Walk>();

                    while (reader.Read())
                    {
                        Walk walk = new Walk()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Walk Id")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                            WalkerId = reader.GetInt32(reader.GetOrdinal("Walker Id")),
                            DogId = reader.GetInt32(reader.GetOrdinal("Dog Id")),
                            Walker = new Walker()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Walker Id")),
                                Name = reader.GetString(reader.GetOrdinal("Walker Name")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("Walkers NeighborhoodId")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("Walker Img")),
                                Neighborhood = new Neighborhood()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Hood Id")),
                                    Name = reader.GetString(reader.GetOrdinal("Hood Name"))
                                }
                            },
                            Dog = new Dog()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Dog Id")),
                                Name = reader.GetString(reader.GetOrdinal("Dog Name")),
                                Breed = reader.GetString(reader.GetOrdinal("Breed")),
                                Owner = new Owner()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Owner Id")),
                                    Name = reader.GetString(reader.GetOrdinal("Owner Name")),
                                    Address = reader.GetString(reader.GetOrdinal("Address")),
                                    Phone = reader.GetString(reader.GetOrdinal("Phone")),
                                    NeighborhoodId = reader.GetInt32(reader.GetOrdinal("Owners NeighborhoodId"))
                                }
                            }
                        };

                        walks.Add(walk);
                    }

                    reader.Close();

                    return walks;
                }
            }
        }

        public List<Walk> GetWalksByWalkerId(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT w.Id AS 'Walk Id', w.Date, w.Duration, w.WalkerId, w.DogId,
                               wr.Id AS 'Walker Id', wr.[Name] AS 'Walker Name', wr.NeighborhoodId AS 'Walkers NeighborhoodId', wr.ImageUrl AS 'Walker Img',
                               n.Id AS 'Hood Id', n.[Name] AS 'Hood Name',
                               d.Id AS 'Dog Id', d.[Name] AS 'Dog Name', d.Breed, d.ImageUrl AS 'Dog Img',
                               o.Id AS 'Owner Id', o.[Name] AS 'Owner Name', o.Email, o.[Address], o.Phone, o.NeighborhoodId AS 'Owners NeighborhoodId'
                        FROM Walks w
                            LEFT JOIN Walker wr ON w.WalkerId = wr.Id
                            LEFT JOIN Neighborhood n ON wr.NeighborhoodId = n.Id
                            LEFT JOIN Dog d ON w.DogId = d.Id
                            LEFT JOIN Owner o ON d.OwnerId = o.Id
                        WHERE w.WalkerId = @id
                    ";

                    cmd.Parameters.AddWithValue("@id", id);
                    
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Walk> walks = new List<Walk>();

                    while(reader.Read())
                    {
                        Walk walk = new Walk()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Walk Id")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                            WalkerId = reader.GetInt32(reader.GetOrdinal("Walker Id")),
                            DogId = reader.GetInt32(reader.GetOrdinal("Dog Id")),
                            Walker = new Walker()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Walker Id")),
                                Name = reader.GetString(reader.GetOrdinal("Walker Name")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("Walkers NeighborhoodId")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("Walker Img")),
                                Neighborhood = new Neighborhood()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Hood Id")),
                                    Name = reader.GetString(reader.GetOrdinal("Hood Name"))
                                }
                            },
                            Dog = new Dog()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Dog Id")),
                                Name = reader.GetString(reader.GetOrdinal("Dog Name")),
                                Breed = reader.GetString(reader.GetOrdinal("Breed")),
                                Owner = new Owner()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Owner Id")),
                                    Name = reader.GetString(reader.GetOrdinal("Owner Name")),
                                    Address = reader.GetString(reader.GetOrdinal("Address")),
                                    Phone = reader.GetString(reader.GetOrdinal("Phone")),
                                    NeighborhoodId = reader.GetInt32(reader.GetOrdinal("Owners NeighborhoodId"))
                                }
                            }
                        };

                        walks.Add(walk);
                    }

                    reader.Close();

                    return walks;
                }
            }
        }

        public void AddWalk(Walk walk)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Walks (Date, Duration, WalkerId, DogId)
                        OUTPUT INSERTED.ID
                        VALUES (@date, @duration, @walkerId, @dogId)
                    ";

                    cmd.Parameters.AddWithValue("@date", walk.Date);
                    cmd.Parameters.AddWithValue("@duration", walk.Duration);
                    cmd.Parameters.AddWithValue("@walkerId", walk.WalkerId);
                    cmd.Parameters.AddWithValue("@dogId", walk.DogId);

                    int id = (int)cmd.ExecuteScalar();

                    walk.Id = id;
                }
            }
        }

        public void DeleteWalks(List<int> ids)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    // Nice Sql command that lets you take a list of ints and place into statement
                    // The parameter is fine itself but instead of doing just:
                    //      cmd.Parameters.AddWithValue("@ids", ids);
                    
                    // We join the values together in a string like:
                    //      string.Join(", ", ids);
                    // And that's the variable for our parameter to add value

                    // Likewise, with the statement itself in the WHERE clause we can't just say in this case:
                    //      WHERE Id IN (@ids)

                    // Because it is still a string in Sql and it will not execute the command
                    // But it will if you just want the values and select them from splitting the string

                    cmd.CommandText = @$"
                        DELETE FROM Walks
                        WHERE Id IN (SELECT * FROM string_split(@ids, ','))
                    ";
                    cmd.Parameters.AddWithValue("@ids", string.Join(", ", ids));
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
