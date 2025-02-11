using gestionEquipe.API.DTOs;
using gestionEquipe.API.Mappers;
using gestionEquipe.Core.Interfaces;
using gestionEquipe.Core.Models;

namespace gestionEquipe.Core.Services
{
    public class EquipeService : IEquipeService

    {
        private readonly IEquipeRepository _equipeRepository;
        private readonly ISiteService _siteService;

        public EquipeService(IEquipeRepository equipeRepository,ISiteService siteService) {
            _equipeRepository = equipeRepository;
            _siteService = siteService;
        }

        public async Task<AddedEquipeDTO> AddEquipeAsync(Equipe _equipe)
        {
            int c = await _equipeRepository.CountEquipesBySportAndUser(_equipe.CaptainId, _equipe.SportId);
            var ReturningEquipe = EquipeMapper.ModelToAdded(_equipe);
            if(c > 1)
            {
                ReturningEquipe.Errors.Add("Count", "This user had more than one team in that sport");
            }
            if(await _equipeRepository.ExistWithName(_equipe.Name, _equipe.AdminId))
            {
                ReturningEquipe.Errors.Add("Name", "A team with this name already exist");
            }
            var sports = await _siteService.GetSportsAsync();
            if (!sports.Select(s => s.Id).Contains(_equipe.SportId)) // Extract IDs and check
            {
                ReturningEquipe.Errors.Add("Sport", "Sport with this id dont exist");
            }
            if(ReturningEquipe.Errors.Count > 0)
            {
                return ReturningEquipe;
            }
            var AddeddEquipe = await _equipeRepository.AddEquipeAsync(_equipe);
            return EquipeMapper.ModelToAdded(AddeddEquipe);

        }
    }
}
