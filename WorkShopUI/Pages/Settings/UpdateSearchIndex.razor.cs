using Microsoft.AspNetCore.Components;
using WorkShopUI.Services;

namespace WorkShopUI.Pages
{
    public class UpdateSearchIndexBase : PageBase
    {
        protected bool DisplayModal;

        [Inject]
        protected SearchIndexService SearchIndexService { get; set; }

        protected override void OnInitialized()
        {
            DisplayModal = false;
        }

        protected void HideModal()
        {
            DisplayModal = false;
        }

        protected void ShowModal()
        {
            DisplayModal = true;
        }

        protected async void UpdateSearchIndexes()
        {
            HideModal();
            await SearchIndexService.UpdateSearchIndexes();
        }
    }
}