namespace ToDosAdminSystem.Model.Entities;

public class Todo
{
    public Todo (int Id){id = Id;}
    public int id { get; set; }
    public int user_id { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public string priority { get; set; }
    public bool is_completed { get; set; }
    public DateTime created_at { get; set; }
    public DateTime completed_at { get; set; }

    
}
