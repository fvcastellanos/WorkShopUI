namespace WorkShopUI.Pages
{
    public class UpdateSearchIndexBase : PageBase
    {
        protected bool DisplayModal;

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

        protected void UpdateSearchIndexes()
        {
            
        }
    }
}