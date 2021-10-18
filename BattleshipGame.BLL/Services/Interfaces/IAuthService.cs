using System.Threading.Tasks;
using BattleshipGame.BLL.DTOs;

namespace BattleshipGame.BLL.Services.Interfaces
{
    public interface IAuthService
    {
         Task<UserForAuthResponseDTO> Login(UserForLoginDTO user);
         Task<UserForAuthResponseDTO> Register(UserForRegisterDTO user);
    }
}