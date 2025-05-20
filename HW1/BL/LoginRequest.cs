namespace HW1.BL
{
    public class LoginRequest
    {
        private string email;
        private string password;

        
        public LoginRequest(string email,string password)
        {
            Email= email;
            Password= password;
        }

        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
    }
}
