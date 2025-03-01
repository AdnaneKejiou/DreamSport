using chatEtInvitation.Core.Interfaces.IRepositories;
using chatEtInvitation.Core.Models;
using gestionEmployer.Infrastructure.Data;
using System;

namespace chatEtInvitation.Infrastructure.Data.Repositories
{
    public class ChatAmisMessageRepository : IChatAmisMessageRepository
    {
        private readonly AppDbContext _context;

        public ChatAmisMessageRepository(AppDbContext context)
        {
            _context = context;
        }


       
    }
}
