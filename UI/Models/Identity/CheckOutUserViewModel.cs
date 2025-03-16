using Microsoft.AspNetCore.Mvc.Rendering;

namespace UI.Models.Identity
{
    public class CheckOutUserViewModel
    {
        public List<SelectListItem> AddressList { get; set; } = new List<SelectListItem>();
        public string SelectedAddress { get; set; }  
    }

}
