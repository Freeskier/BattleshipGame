namespace BattleshipGame.BLL.Authentication
{
    public interface IJWTAuthenticationManager
    {
        string Authenticate(string name, int ID);
    }
} 