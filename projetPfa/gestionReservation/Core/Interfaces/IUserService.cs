using gestionReservation.Infrastructure.ExternServices.Extern_DTo;

namespace gestionReservation.Core.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> FetchUserAsync(int idUser);
    }
}
