using HW1.DAL;

namespace HW1.BL
{
    public class Users
    {
        private int id;
        private string name;
        private string email;
        private string password;
        private bool active = true;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public bool Active { get => active; set => active = value; }

        public Users(int id,string name, string email,string password)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
        }
        public Users() { }

    
        public bool Register()
        {
            DBservices dbs = new DBservices();
            return dbs.AddNewUser(this);
        }

        public Users Login(string email, string hashedPassword)
        {
            DBservices dbs = new DBservices();
            return dbs.Login(email, hashedPassword);
        }
        public bool DeleteUser(int id)
        {
            DBservices dbs = new DBservices();
            return dbs.DeleteUser(id);
        }
        public Users UpdateUser(int id)
        {
            DBservices dbs = new DBservices();
            bool success = dbs.UpdateUser(id, this);
            if (success)
                return this; 
            else
                return null;
        }

        public List<Users> GetAllActiveUsers(int currentUserId)
        {
            DBservices dbs = new DBservices();
            return dbs.GetAllActiveUsers(currentUserId);
        }

        public List<Users> GetAllUsers()
        {
            DBservices dbs = new DBservices();
            return dbs.GetAllUsers();
        }
        public bool UpdateActiveStatus(int id,bool active)
        {

            DBservices dbs = new DBservices();
            return dbs.UpdateActiveStatus(id,active);
        }

    }
}
