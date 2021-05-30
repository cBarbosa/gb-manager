namespace gb_manager.Domain.Shared.Command
{
    public class AuthenticationCommand
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}