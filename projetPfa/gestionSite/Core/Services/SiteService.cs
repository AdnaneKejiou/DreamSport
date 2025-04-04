using gestionSite.Core.Interfaces.SiteInterfaces;
using gestionSite.Core.Models;
using RabbitMQModel = RabbitMQ.Client.IModel;
using EntityFrameworkModel = Microsoft.EntityFrameworkCore.Metadata.IModel;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using gestionSite.API.DTOs.SiteDtos;
using gestionSite.API.Mappers;


namespace gestionSite.Core.Services
{
    public class SiteService:ISiteService
    {
        private readonly ISiteRepository _siteRepository;

        public SiteService(ISiteRepository siteRepository)
        {
            _siteRepository = siteRepository;
           
        }


       

        public async Task<IEnumerable<Site>> GetSiteByAdminAsync(int adminId)
        {
            
            return await _siteRepository.GetAllComplexInfosAsync(adminId);
        }

        public async Task<Site> GetSiteASync(int adminId)
        {
            return await _siteRepository.getSiteASync(adminId);
        }

        public async Task<Site?> AddSiteAsync(int adminId)
        {
            // Créer un objet Site manuellement avec les données que tu veux
            var site = new Site
            {
                Name = "Mon Site Web",            // Par exemple, le nom du site
                Logo = "https://pub-ae615910610b409dbb3d91c073aa47e6.r2.dev/logo.svg",                // Le logo du site
                Description = GenerateLoremIpsum(200),
                Email = "contact@exemple.com",    // Email de contact
                PhoneNumber = "+212-614329800",       // Numéro de téléphone
                AboutUs = GenerateLoremIpsum(150),
                CouleurPrincipale = "#097E52",    // Couleur principale
                CouleurSecondaire = "#192335",    // Couleur secondaire
                Background = "https://pub-ae615910610b409dbb3d91c073aa47e6.r2.dev/banner.jpg",    // Image de fond
                Addresse = "123 Rue Exemple",     // Adresse
                DomainName = "monsite.com",       // Domaine du site
                IdAdmin = adminId                       // ID de l'administrateur
            };

            // Appeler le repository pour ajouter ce site à la base de données
            return await _siteRepository.AddSiteAsync(site);
        }

        public async Task<ReturnUpdatedSiteDto?> UpdateSiteAsync(Site updatingSite)
        {
            Site existingSite = await _siteRepository.getSiteASync(updatingSite.IdAdmin);
            if(existingSite == null)
            {
                throw new KeyNotFoundException("SIte not found");
            }
            ReturnUpdatedSiteDto dto = SiteMapper.modelToUpdated(existingSite);
            // Vérification de l'unicité seulement si l'attribut est modifié
            if (updatingSite.PhoneNumber != null && !updatingSite.PhoneNumber.Equals(existingSite.PhoneNumber))
            {
                if (await _siteRepository.getByPhoneAsync(updatingSite.PhoneNumber) != null)
                {
                    {
                        dto.Errors.Add("PhoneNumber", "PhoneNumber already exist.");
                    }
                }
            }
            if (updatingSite.Name != null && !updatingSite.Name.Equals(existingSite.Name))
            {
                if (await _siteRepository.getByNameAsync(updatingSite.Name) != null)
                {
                    {
                        dto.Errors.Add("Name", "Name already exist.");
                    }
                }
            }
            if (updatingSite.Email != null && !updatingSite.Email.Equals(existingSite.Email))
            {
                if (await _siteRepository.getByEmailAsync(updatingSite.Email) != null)
                {
                    {
                        dto.Errors.Add("Email", "Email already exist.");
                    }
                }
            }
            if (updatingSite.DomainName != null && !updatingSite.DomainName.Equals(existingSite.DomainName))
            {
                if (await _siteRepository.getByDomaineAsync(updatingSite.DomainName) != null)
                {
                    {
                        dto.Errors.Add("DomainName", "DomainName already exist.");
                    }
                }
            }
            if (dto.Errors.Count == 0)
            {
                
                return SiteMapper.modelToUpdated(await _siteRepository.UpdateSiteAsync(updatingSite));
                
            }
            return dto;

        }

        public string GenerateLoremIpsum(int length)
        {
            var loremIpsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. ";
            var repeatCount = (int)Math.Ceiling((double)length / loremIpsum.Length);
            return string.Concat(Enumerable.Repeat(loremIpsum, repeatCount)).Substring(0, length);
        }


    }
}
