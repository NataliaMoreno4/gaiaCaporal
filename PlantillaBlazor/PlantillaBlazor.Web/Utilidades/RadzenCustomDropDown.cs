using Radzen.Blazor;
using Radzen;

namespace PlantillaBlazor.Web.Utilidades
{
    public class RadzenCustomDropDown<TValue> : RadzenDropDown<TValue>
    {
        public RadzenCustomDropDown() : base()
        {
            AllowFiltering = true;
            FilterCaseSensitivity = FilterCaseSensitivity.CaseInsensitive;
            SelectedItemsText = "item(s) selecionado(s)";
            SelectAllText = "Seleccionar todos";
            Style = "width:100%;";
        }
    }
}
