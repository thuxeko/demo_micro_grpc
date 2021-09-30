using BlazorClientManager.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorClientManager.Pages
{
    public partial class ConfigVip
    {
        private HubConnection _hubConnection;
        public List<VipModel> lstVip = new List<VipModel>();
        public CRUDVipResponse crudRes = new CRUDVipResponse();

        [Inject] private NavigationManager navigationManager { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; }
        private async Task StartHubConnection()
        {
            _hubConnection = new HubConnectionBuilder()
                            .WithUrl("https://localhost:5031/VipHub")
                            .Build();

            await _hubConnection.StartAsync();
            if (_hubConnection.State == HubConnectionState.Connected)
                Console.WriteLine("connection started");
        }

        protected async override Task OnInitializedAsync()
        {
            await StartHubConnection();
            await _hubConnection.SendAsync("GetListVip");

            _hubConnection.On<List<VipModel>>("ReceiveListVip", (data) =>
            {
                lstVip = data;
                StateHasChanged();
            });

            await ReceiveDelete();
        }

        private async Task DeleteVip(int id)
        {
            await _hubConnection.InvokeAsync("DeleteVip", id);
        }

        private async Task ReceiveDelete()
        {
            _hubConnection.On<CRUDVipResponse>("ReceiveDelete", (data) =>
            {
                JSRuntime.InvokeVoidAsync("alert", data.mes);
                StateHasChanged();
            });
        }

        private void AddNew()
        {
            navigationManager.NavigateTo("/config/addnew");
        }

        private void EditVip(int id)
        {
            navigationManager.NavigateTo("/editconfig?Id=" + id);
        }
    }
}
