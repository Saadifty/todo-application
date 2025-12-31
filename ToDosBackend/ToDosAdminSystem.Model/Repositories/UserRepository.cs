namespace ToDosAdminSystem.Model.Repositories;

using System;
using System.Collections.Generic;
using ToDosAdminSystem.Model.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;

public class UserRepository : BaseRepository
{
    public UserRepository(IConfiguration configuration) : base(configuration)
    {
    }

    // Get a single user by ID
    public User GetUserById(int id)
    {
        using (var dbConn = new NpgsqlConnection(ConnectionString))
        {
            try
            {
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = "SELECT * FROM users WHERE id = @id";
                cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = id;

                dbConn.Open();
                using (var data = cmd.ExecuteReader())
                {
                    if (data.Read())
                    {
                        return new User
                        {
                            id = (int)data["id"],
                            username = data["username"].ToString(),
                            email = data["email"].ToString(),
                            password_hash = data["password_hash"].ToString()
                        };
                    }
                }
            }
            finally
            {
                dbConn.Close();
            }
        }
        return null;
    }

    // Get all users
    public List<User> GetAllUsers()
    {
        var users = new List<User>();
        using (var dbConn = new NpgsqlConnection(ConnectionString))
        {
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = "SELECT * FROM users";

            dbConn.Open();
            using (var data = cmd.ExecuteReader())
            {
                while (data.Read())
                {
                    var user = new User
                    {
                        id = (int)data["id"],
                        username = data["username"].ToString(),
                        email = data["email"].ToString(),
                        password_hash = data["password_hash"].ToString()
                    };
                    users.Add(user);
                }
            }
        }
        return users;
    }

    // Insert a new user
    public bool InsertUser(User user)
    {
        using (var dbConn = new NpgsqlConnection(ConnectionString))
        {
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
INSERT INTO users (username, email, password_hash) 
VALUES (@username, @email, @password_hash)";

            cmd.Parameters.AddWithValue("@username", NpgsqlDbType.Varchar, user.username);
            cmd.Parameters.AddWithValue("@email", NpgsqlDbType.Varchar, user.email);
            cmd.Parameters.AddWithValue("@password_hash", NpgsqlDbType.Text, user.password_hash);

            return InsertData(dbConn, cmd);
        }
    }

    // Update user details
    public bool UpdateUser(User user)
    {
        using (var dbConn = new NpgsqlConnection(ConnectionString))
        {
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
UPDATE users 
SET username = @username, 
    email = @email, 
    password_hash = @password_hash 
WHERE id = @id";

            cmd.Parameters.AddWithValue("@username", NpgsqlDbType.Varchar, user.username);
            cmd.Parameters.AddWithValue("@email", NpgsqlDbType.Varchar, user.email);
            cmd.Parameters.AddWithValue("@password_hash", NpgsqlDbType.Text, user.password_hash);
            cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, user.id);

            return UpdateData(dbConn, cmd);
        }
    }

    // Delete a user by ID
    public bool DeleteUser(int id)
    {
        using (var dbConn = new NpgsqlConnection(ConnectionString))
        {
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = "DELETE FROM users WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, id);

            return DeleteData(dbConn, cmd);
        }
    }
}
