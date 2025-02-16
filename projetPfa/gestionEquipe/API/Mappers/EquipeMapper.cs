using gestionEquipe.API.DTOs;
using gestionEquipe.Core.Models;
using System.Reflection.Metadata.Ecma335;

namespace gestionEquipe.API.Mappers
{
    public class EquipeMapper
    {
        public static Equipe AddToModel(AddEquipeDTO dto)
        {
            return new Equipe
            {
                Name = dto.Name,
                AdminId = dto.AdminId,
                Description = dto.Description,
                Avatar = dto.Avatar,
                CaptainId = dto.CaptainId,
                SportId = dto.SportId,

            };
        }

        

        public static AddedEquipeDTO ModelToAdded(Equipe equipe)
        {
            return new AddedEquipeDTO
            {
                CaptainId = equipe.CaptainId,
                Name = equipe.Name,
                Description = equipe.Description,
                Avatar = equipe.Avatar,
                AdminId = equipe.AdminId,
                SportId= equipe.SportId,
            };
        }
    }
}
