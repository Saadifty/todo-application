namespace ToDosAdminSystem.Model.Repositories;

using System;
using ToDosAdminSystem.Model.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
public class TodoRepository : BaseRepository
{
    public TodoRepository(IConfiguration configuration) : base(configuration)
    {
    }
    public virtual Todo GetTodoById(int id)
    {
        NpgsqlConnection dbConn = null;
        try
        {
            //create a new connection for database
            dbConn = new NpgsqlConnection(ConnectionString);
            //creating an SQL command
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = "select * from todos where id = @id";
            cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = id;
            //call the base method to get data
            var data = GetData(dbConn, cmd);
            if (data != null)
            {
                if (data.Read()) //every time loop runs it reads next like from fetched rows
                {
                    return new Todo(Convert.ToInt32(data["id"]))
                    {
                        user_id = (int)data["user_id"],
                        title = data["title"].ToString(),
                        description = data["description"].ToString(),
                        priority = data["priority"].ToString(),
                        is_completed = (bool)data["is_completed"],
                        created_at = data["created_at"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(data["created_at"]),
                        completed_at = data["completed_at"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(data["completed_at"])

                    };
                }
            }
            return null;
        }
        finally
        {
            dbConn?.Close();
        }
    }
    public List<Todo> GetTodos()
    {
        NpgsqlConnection dbConn = null;
        var todos = new List<Todo>();
        try
        {
            //create a new connection for database
            dbConn = new NpgsqlConnection(ConnectionString);
            //creating an SQL command
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = "select * from todos";
            //call the base method to get data
            var data = GetData(dbConn, cmd);
            if (data != null)
            {
                while (data.Read()) //every time loop runs it reads next like from fetched rows
                {
                    Todo t = new Todo(Convert.ToInt32(data["id"]))
                    {
                        user_id = (int)data["user_id"],
                        title = data["title"].ToString(),
                        description = data["description"].ToString(),
                        priority = data["priority"].ToString(),
                        is_completed = (bool)data["is_completed"],
                        created_at = data["created_at"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(data["created_at"]),
                        completed_at = data["completed_at"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(data["completed_at"])
                    };
                    todos.Add(t);
                }
            }
            return todos;
        }
        finally
        {
            dbConn?.Close();
        }
    }
    //add a new todo
    public bool InsertTodo(Todo t)
    {
        NpgsqlConnection dbConn = null;
        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
insert into todos
(user_id, title, description, priority, is_completed, created_at, completed_at)
values
(@user_id, @title, @description, @priority, @is_completed, @created_at, @completed_at)
";
            //adding parameters in a better way
            cmd.Parameters.AddWithValue("@user_id", NpgsqlDbType.Integer, t.user_id);
            cmd.Parameters.AddWithValue("@title", NpgsqlDbType.Text, t.title);
            cmd.Parameters.AddWithValue("@description", NpgsqlDbType.Text, t.description);
            cmd.Parameters.AddWithValue("@priority", NpgsqlDbType.Text, t.priority);
            cmd.Parameters.AddWithValue("@is_completed", NpgsqlDbType.Boolean, t.is_completed);
            cmd.Parameters.AddWithValue("@created_at", NpgsqlDbType.Timestamp, t.created_at.ToLocalTime());
            cmd.Parameters.AddWithValue("@completed_at", NpgsqlDbType.Timestamp, t.completed_at.ToLocalTime());

            //will return true if all goes well
            bool result = InsertData(dbConn, cmd);
            return result;
        }
        finally
        {
            dbConn?.Close();
        }
    }
    public bool UpdateTodo(Todo t)
    {
        var dbConn = new NpgsqlConnection(ConnectionString);
        var cmd = dbConn.CreateCommand();
        cmd.CommandText = @"
update todos set
user_id=@user_id,
title=@title,
description=@description,
priority=@priority,
is_completed=@is_completed,
created_at=@created_at,
completed_at=@completed_at
where
id = @id";
        cmd.Parameters.AddWithValue("@user_id", NpgsqlDbType.Integer, t.user_id);
        cmd.Parameters.AddWithValue("@title", NpgsqlDbType.Text, t.title);
        cmd.Parameters.AddWithValue("@description", NpgsqlDbType.Text, t.description);
        cmd.Parameters.AddWithValue("@priority", NpgsqlDbType.Text, t.priority);
        cmd.Parameters.AddWithValue("@is_completed", NpgsqlDbType.Boolean, t.is_completed);
        cmd.Parameters.AddWithValue("@created_at", NpgsqlDbType.Timestamp, t.created_at.ToLocalTime());
        cmd.Parameters.AddWithValue("@completed_at", NpgsqlDbType.Timestamp, t.completed_at.ToLocalTime());
        cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, t.id);

        bool result = UpdateData(dbConn, cmd);
        return result;
    }
    public virtual bool DeleteToDo(int id)
    {
        var dbConn = new NpgsqlConnection(ConnectionString);
        var cmd = dbConn.CreateCommand();
        cmd.CommandText = @"
delete from todos
where id = @id
";
        //adding parameters in a better way
        cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, id);
        //will return true if all goes well
        bool result = DeleteData(dbConn, cmd);
        return result;
    }
    


    public List<Todo> GetTodosByUserId(int userId)
{
    var todos = new List<Todo>();
    using (var dbConn = new NpgsqlConnection(ConnectionString))
    {
        dbConn.Open();

        // Query to fetch todos by user_id and sort by priority
        var cmd = dbConn.CreateCommand();
        cmd.CommandText = @"
            SELECT * 
            FROM todos 
            WHERE user_id = @userId
            ORDER BY 
                CASE 
                    WHEN priority = 'high' THEN 1
                    WHEN priority = 'medium' THEN 2
                    WHEN priority = 'low' THEN 3
                    ELSE 4
                END;
        ";
        cmd.Parameters.AddWithValue("@userId", NpgsqlDbType.Integer, userId);

        using (var data = cmd.ExecuteReader())
        {
            while (data.Read())
            {
                todos.Add(new Todo(Convert.ToInt32(data["id"]))
                {
                    user_id = (int)data["user_id"],
                    title = data["title"].ToString(),
                    description = data["description"].ToString(),
                    priority = data["priority"].ToString(),
                    is_completed = (bool)data["is_completed"],
                    created_at = data["created_at"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(data["created_at"]),
                    completed_at = data["completed_at"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(data["completed_at"])
                });
            }
        }
    }
    return todos;
}


    
}
