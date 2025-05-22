using Microsoft.AspNetCore.Components;
using Radzen;

namespace PlantillaBlazor.Web.Utilidades
{
    public class TooltipHelper(TooltipService _tooltipService)
    {

        public void ShowHelpInfo(string contenido, ElementReference elementReference)
        {
            ShowTooltip(elementReference, contenido, new TooltipOptions() { Position = TooltipPosition.Top, Style = "background-color: var(--rz-secondary); color: var(--rz-text-contrast-color)", Duration = null });
        }

        private void ShowTooltip(ElementReference elementReference, string contenido, TooltipOptions options = null) => _tooltipService.Open(elementReference, contenido, options);
    }
}
