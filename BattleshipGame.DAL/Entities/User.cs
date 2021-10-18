namespace BattleshipGame.DAL.Entities
{
    public class User
    {
        public int ID {get; set;}
        public string Login {get; set;}
        public byte[] PasswordHash {get; set;}
        public byte[] PasswordSalt {get; set;}
        public string Email {get; set;}
    }
}