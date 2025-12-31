namespace ToDosAdminSystem.Model.Entities;

public class User
{
    public int id { get; set; }
    public string username { get; set; }
    public string email { get; set; }
    public string password_hash { get; set; }
}
