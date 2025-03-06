namespace Auth.Dtos
{
    public class UserLogin
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int AdminId {  get; set; }
    }
}
