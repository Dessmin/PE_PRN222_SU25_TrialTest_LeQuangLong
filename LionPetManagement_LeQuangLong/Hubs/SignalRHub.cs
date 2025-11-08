using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace LionPetManagement_LeQuangLong.Hubs
{
    public class SignalRHub : Hub
    {
        private readonly ILionProfileService _entityService;

        public SignalRHub(ILionProfileService entityService)
        {
            _entityService = entityService;
        }

        public async Task HubDelete(string Id)
        {
            await _entityService.DeleteAsync(int.Parse(Id));
            await Clients.All.SendAsync("ReceiveDelete", Id);
        }
    }
}
