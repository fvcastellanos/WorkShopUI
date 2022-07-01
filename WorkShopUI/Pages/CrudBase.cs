
using Blazorise;
using Microsoft.AspNetCore.Components.Forms;

namespace WorkShopUI.Pages
{
    public abstract class CrudBase: PageBase
    {
        protected EditContext EditContext;
        protected bool DisplayModal;
        protected bool DisplayDeleteModal;
        protected bool DisplayViewModal;
        protected bool ModifyModal;
        protected bool HasModalError;
        protected string ModalErrorMessage;

        protected Validations Validations;

        protected void DisplayModalError(string error)
        {
            HasModalError = true;
            ModalErrorMessage = error;
        }

        protected void HideModalError()
        {
            HasModalError = false;
            ModalErrorMessage = "";
        }

        protected void ShowModal()
        {
            DisplayModal = true;
        }

        protected void HideModal()
        {
            DisplayModal = false;
        }

        protected void HideAddModal()
        {
            HideModalError();
            HideModal();
        }

        protected void ShowEditModal()
        {
            HideModalError();
            ShowModal();
            ModifyModal = true;
        }

        protected abstract void Update();
        protected abstract void Add();

        protected abstract void Search();

        protected abstract void DisplayPage(int pageNumber);

        protected void SaveChanges()
        {
            if (!EditContext.Validate())
            {
                return;
            }

            if (ModifyModal)
            {
                Update();
                
                return;
            }

            Add();
        }        
    }
}