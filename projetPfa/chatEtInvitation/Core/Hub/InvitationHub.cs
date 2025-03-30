using chatEtInvitation.API.DTOs;
using chatEtInvitation.Core.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class InvitationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        // Récupération directe de l'userId depuis les query parameters
        var userId = Context.GetHttpContext()?.Request.Query["userId"];

        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            Console.WriteLine($"User {userId} added to group");
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.GetHttpContext()?.Request.Query["userId"];
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
            Console.WriteLine($"User {userId} removed from group");
        }
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendInvitationToUser(string userId, MemberInvitationDTO invitation)
    {
        // Assurez-vous d'envoyer l'objet complet
        await Clients.Group(userId).SendAsync("ReceiveInvitation", new
        {
            Emetteur = invitation.Emetteur,
            Recepteur = invitation.Recepteur,
            // Ajoutez toutes les propriétés nécessaires
        });
    }
}