using gestionEmployer.Core.Interfaces;
using gestionEmployer.Core.Models;
using gestionEmployer.Infrastructure.Data.Repositories;
using System.ComponentModel.DataAnnotations;
using gestionEmployer.API.DTOs.EmployeeDTO;
using gestionEmployer.API.Mappers;
using gestionEmployer.Infrastructure.ExternServices.ExternDTOs;


namespace gestionEmployer.Core.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMailService _mailService;
        private readonly IAdminRepository _adminRepository;

        public EmployeeService(IEmployeeRepository employeeRepository, IMailService mailService, IAdminRepository adminRepository)
        {
            _employeeRepository = employeeRepository;
            _mailService = mailService;
            _adminRepository = adminRepository;
        }

        public async Task<Employer> GetEmployeeByIdAsync(int id)
        {
             var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            return employee;
        }


        //Methode de recuperations de tous les employees by IdAdmin

        public async Task<IEnumerable<Employer>> GetEmployesByAdminIdAsync(int AdminId)
        {
            return await _employeeRepository.GetEmployesByAdminIdAsync(AdminId);
        }




        public async Task<ReturnAddedEmployee> AddEmployeeAsync(Employer employee)
        {
            ReturnAddedEmployee _ReturnAddedEmployee = EmployeeMapper.EmployeeToRTE(employee);
            if (_employeeRepository.Exists(e => e.CIN == employee.CIN && e.AdminId == employee.AdminId))
                _ReturnAddedEmployee.errors.Add("Cin deja utilise");

            if (_employeeRepository.Exists(e => e.Email == employee.Email && e.AdminId == employee.AdminId))
                _ReturnAddedEmployee.errors.Add("email deja utilise");

            if (_employeeRepository.Exists(e => e.Username == employee.Username && e.AdminId == employee.AdminId))
                _ReturnAddedEmployee.errors.Add("username deja utilise");

            if (_ReturnAddedEmployee.errors.Count>0)
            {
                return _ReturnAddedEmployee;
            }

            // si toutes les attributs ne se répete pas enregistre l'employé 

            Employer emp = await _employeeRepository.AddEmployeeAsync(employee);
            EmailRequest emailRequest = new EmailRequest(employee.Email, employee.Nom + " " + employee.Prenom, employee.Password);
            var xd = await _mailService.NewEmployeeMail(emailRequest);
            return _ReturnAddedEmployee;

        }

       





        //Dans cette mettre on vas modifier l'employé en verifiant tous les attributs s'ils existent déja
        public async Task<Employer?> UpdateEmployeeAsync( Employer updatedEmploye)
        {
            // Récupérer l'employé existant
            Employer existingEmploye = await _employeeRepository.GetEmployeeByIdAsync(updatedEmploye.Id)
                                    ?? throw new KeyNotFoundException("Employé non trouvé.");
 
            // Liste pour stocker les erreurs trouvées
            var errors = new List<string>();

            // Vérification de l'unicité seulement si l'attribut est modifié
            if (updatedEmploye.CIN != existingEmploye.CIN &&
                _employeeRepository.Exists(e => e.CIN == updatedEmploye.CIN && e.Id != updatedEmploye.Id && e.AdminId == updatedEmploye.AdminId))
            {
                errors.Add("CIN déjà utilisé.");
            }

            if (updatedEmploye.Email != existingEmploye.Email &&
                _employeeRepository.Exists(e => e.Email == updatedEmploye.Email && e.Id != updatedEmploye.Id && e.AdminId == updatedEmploye.AdminId))
            {
                errors.Add("Email déjà utilisé.");
            }

            if (updatedEmploye.Username != existingEmploye.Username &&
                _employeeRepository.Exists(e => e.Username == updatedEmploye.Username && e.Id != updatedEmploye.Id && e.AdminId == updatedEmploye.AdminId))
            {
                errors.Add("Username déjà utilisé.");
            }

            // Si des erreurs sont trouvées, lancer une exception avec toutes les erreurs
            if (errors.Any())
            {
                throw new ValidationException(string.Join(", ", errors));
            }

            // Mise à jour des informations
            existingEmploye.Nom = updatedEmploye.Nom;
            existingEmploye.Prenom = updatedEmploye.Prenom;
            existingEmploye.CIN = updatedEmploye.CIN;
            existingEmploye.PhoneNumber = updatedEmploye.PhoneNumber;
            existingEmploye.Email = updatedEmploye.Email;
            existingEmploye.Username = updatedEmploye.Username;
            existingEmploye.Password = updatedEmploye.Password;
            existingEmploye.Salaire = updatedEmploye.Salaire;

            // Sauvegarder les changements dans la base de données
            return await _employeeRepository.UpdateEmployeeAsync(existingEmploye) ;
        }




        public async Task<Employer> DeleteEmployeeAsync(int id)
        {
            //Vérification si l'employé existe  sinon il retourne un message "Employé non trouvé"
            var existingEmploye = _employeeRepository.GetEmployeeByIdAsync(id)
                                      ?? throw new KeyNotFoundException("Employé non trouvé.");

            //Suppression de l'employé qui a id entré
           return await _employeeRepository.DeleteEmployeeAsync(id);
        }


        public async Task<Employer> ModifyProfileAsync(Employer employer)
        {
            // Récupérer l'employé avec l'ID fourni
            var employee = await _employeeRepository.GetEmployeeByIdAsync(employer.Id);

            // Vérifier si l'employé existe
            if (employee != null)
            {
                // Mettre à jour les informations de l'employé
                employee.Nom = employer.Nom;
                employee.Prenom = employer.Prenom;
                employee.Email = employer.Email;
                employee.PhoneNumber = employer.PhoneNumber;

                // Appeler la méthode pour mettre à jour l'employé dans la base de données
                await _employeeRepository.UpdateEmployeeAsync(employee);

                // Retourner l'employé mis à jour
                return employee;
            }

            // Si l'employé n'est pas trouvé, retourner null ou lever une exception selon ton besoin
            return null;
        }



    }
}
