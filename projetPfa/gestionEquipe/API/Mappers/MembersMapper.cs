using gestionEquipe.API.DTOs;
using gestionEquipe.Core.Models;

namespace gestionEquipe.API.Mappers
{
    public class MembersMapper
    {
        public static Members DeleteDTOtoModel(DeleteMemberDTO dto)
        {
            return new Members
            {
                UserId = dto.UserId,
                EquipeId = dto.EquipeId,
            };
        }
    }
}
