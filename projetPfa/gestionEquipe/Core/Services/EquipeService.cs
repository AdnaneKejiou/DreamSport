using gestionEquipe.API.DTOs;
using gestionEquipe.API.Mappers;
using gestionEquipe.Core.Interfaces;
using gestionEquipe.Core.Models;
using gestionEquipe.Infrastructure.Data.Repositories;
using gestionEquipe.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace gestionEquipe.Core.Services
{
    public class EquipeService : IEquipeService

    {
        private readonly IEquipeRepository _equipeRepository;
        private readonly ISiteService _siteService;
        private readonly IMembersRepository _membersRepository;
        private readonly IUnitOfWork _unitOfWork;


        public EquipeService(IEquipeRepository equipeRepository,ISiteService siteService,IMembersRepository membersRepository,IUnitOfWork unitOfWork) {
            _equipeRepository = equipeRepository;
            _siteService = siteService;
            _membersRepository = membersRepository;
            _unitOfWork = unitOfWork;
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
            
            
             await AddEquipeWithMemberAsync(_equipe);
            return ReturningEquipe;

        }

        private async Task<Equipe> AddEquipeWithMemberAsync(Equipe equipe)
        {
            // Start the transaction if necessary
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Add both the Equipe and the Member
                equipe = await _equipeRepository.AddEquipeAsync(equipe);
                Members member = new Members
                {
                    UserId = equipe.CaptainId,
                    EquipeId = equipe.Id,
                };
                await _membersRepository.AddMemberAsync(member);

                await _unitOfWork.SaveChangesAsync();  // Save changes

                await _unitOfWork.CommitAsync();  // Commit transaction
                return equipe;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();  // Rollback if any errors occur
                throw;
            }
        }

        // Méthode pour supprimer l'équipe avec ses membres
        public async Task SupprimerEquipeAvecMembresAsync(int equipeId)
        {
            var equipe = await _equipeRepository.GetEquipeById(equipeId);
            if (equipe == null) throw new KeyNotFoundException("Team not found");
            await _equipeRepository.SupprimerEquipeAvecMembresAsync(equipeId);
        }

        

        public async Task<UpdatedEquipeDTO> UpdateEquipeAsync(Equipe _equipe) {

            var ReturningEquipe = EquipeMapper.EquipetoUpdatedEquipeDTO(_equipe);

             Equipe existingEquipe = await _equipeRepository.GetEquipeById(_equipe.Id);

            if (existingEquipe == null)
            {
                ReturningEquipe.Errors.Add("Count", "Team not exist");
            }

            // Update the properties of the existing equipe

            if (_equipe.Name != null)
            {
                existingEquipe.Name = _equipe.Name;
            }

            if (_equipe.Description != null)
            {
                existingEquipe.Description = _equipe.Description;
            }

            if (_equipe.Avatar != null)
            {
                existingEquipe.Avatar = _equipe.Avatar;
            }

            if (_equipe.CaptainId > 0)
            {
                existingEquipe.CaptainId = _equipe.CaptainId;
            }

            if (ReturningEquipe.Errors.Count > 0)
            {
                return ReturningEquipe;
            }

            
             var UpdatedEquipe = await _equipeRepository.UpdateEquipeAsync(existingEquipe);

            return EquipeMapper.EquipetoUpdatedEquipeDTO(UpdatedEquipe);



        }
        public async Task<UpdatedEquipeDTO> TransferCaptaincyAsync(Equipe _equipe)
        {

            var ReturningEquipe = EquipeMapper.EquipeChangedCapitain(_equipe);

            if (!await _membersRepository.ExistInTeamWithIdAsync(_equipe.CaptainId,_equipe.Id))
            {
                ReturningEquipe.Errors.Add("Count", "This user not exist in this team");
            }
            var equipe = await _equipeRepository.GetEquipeById(_equipe.Id);

            if (equipe == null)
            {
                ReturningEquipe.Errors.Add("Count", "This team not exist");
            }

            if (ReturningEquipe.Errors.Count > 0)
            {
                return ReturningEquipe;
            }

            equipe.CaptainId = _equipe.CaptainId;

            var UpdatedEquipe = await UpdateEquipeAsync(equipe);

            

            return UpdatedEquipe;



        }

    }
}
