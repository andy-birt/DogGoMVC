﻿using DogGoMVC.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DogGoMVC.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly IConfiguration _config;

        public OwnerRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Owner> GetAllOwners()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, Email, [Name], [Address], NeighborhoodId, Phone
                        FROM [Owner]
                    ";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Owner> owners = new List<Owner>();
                    while (reader.Read())
                    {
                        Owner owner = new Owner
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Address = reader.GetString(reader.GetOrdinal("Address")),
                            Phone = reader.GetString(reader.GetOrdinal("Phone")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                        };

                        owners.Add(owner);
                    }

                    reader.Close();

                    return owners;
                }
            }
        }

        public Owner GetOwnerById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT o.Id As OwnerId, o.[Name] AS OwnerName, o.Email, o.[Address], o.NeighborhoodId, o.Phone,
                               d.Id AS DogId, d.[Name], d.Breed, d.OwnerId AS DogsOwnerId
                        FROM Owner o
                            LEFT JOIN Dog d ON d.OwnerId = o.Id
                        Where o.Id = @id
                    ";
                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    
                    Owner owner = null;
                    while (reader.Read())
                    {
                        if (owner == null)
                        {
                            owner = new Owner
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                Name = reader.GetString(reader.GetOrdinal("OwnerName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                Phone = reader.GetString(reader.GetOrdinal("Phone")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                            };
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("DogId")))
                        {
                            owner.Dogs.Add(new Dog()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("DogId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Breed = reader.GetString(reader.GetOrdinal("Breed")),
                                OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId"))
                            });
                        }

                    }
                    reader.Close();
                    return owner;
                }
            }
        }

        public Owner GetOwnerByEmail(string email)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT o.Id As OwnerId, o.[Name] AS OwnerName, o.Email, o.[Address], o.NeighborhoodId, o.Phone,
                               d.Id AS DogId, d.[Name], d.Breed, d.OwnerId AS DogsOwnerId
                        FROM Owner o
                            LEFT JOIN Dog d ON d.OwnerId = o.Id
                        Where o.Email = @email
                    ";
                    cmd.Parameters.AddWithValue("@email", email);

                    SqlDataReader reader = cmd.ExecuteReader();


                    Owner owner = null;
                    while (reader.Read())
                    {
                        if (owner == null)
                        {
                            owner = new Owner
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                Name = reader.GetString(reader.GetOrdinal("OwnerName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                Phone = reader.GetString(reader.GetOrdinal("Phone")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                            };
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("DogId")))
                        {
                            owner.Dogs.Add(new Dog()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("DogId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Breed = reader.GetString(reader.GetOrdinal("Breed")),
                                OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId"))
                            });
                        }

                    }
                    reader.Close();
                    return owner;
                }
            }
        }

        public void AddOwner(Owner owner)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO Owner ([Name], Email, Phone, Address, NeighborhoodId)
                    OUTPUT INSERTED.ID
                    VALUES (@name, @email, @phoneNumber, @address, @neighborhoodId);
                ";

                    cmd.Parameters.AddWithValue("@name", owner.Name);
                    cmd.Parameters.AddWithValue("@email", owner.Email);
                    cmd.Parameters.AddWithValue("@phoneNumber", owner.Phone);
                    cmd.Parameters.AddWithValue("@address", owner.Address);
                    cmd.Parameters.AddWithValue("@neighborhoodId", owner.NeighborhoodId);

                    int id = (int)cmd.ExecuteScalar();

                    owner.Id = id;
                }
            }
        }

        public void UpdateOwner(Owner owner)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Owner
                            SET 
                                [Name] = @name, 
                                Email = @email, 
                                Address = @address, 
                                Phone = @phone, 
                                NeighborhoodId = @neighborhoodId
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@name", owner.Name);
                    cmd.Parameters.AddWithValue("@email", owner.Email);
                    cmd.Parameters.AddWithValue("@address", owner.Address);
                    cmd.Parameters.AddWithValue("@phone", owner.Phone);
                    cmd.Parameters.AddWithValue("@neighborhoodId", owner.NeighborhoodId);
                    cmd.Parameters.AddWithValue("@id", owner.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteOwner(int ownerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Owner
                            WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", ownerId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
