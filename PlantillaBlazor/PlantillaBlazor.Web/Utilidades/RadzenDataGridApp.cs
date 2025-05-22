using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace PlantillaBlazor.Web.Utilidades
{
    public class RadzenDataGridApp<TItem> : RadzenDataGrid<TItem>
    {
        public RadzenDataGridApp() : base()
        {
            FilterCaseSensitivity = Radzen.FilterCaseSensitivity.CaseInsensitive;
            FilterPopupRenderMode = PopupRenderMode.OnDemand;
            FilterMode = Radzen.FilterMode.Advanced;
            ColumnWidth = "200px";
            AllowAlternatingRows = true;
            AllowFiltering = true;
            AllowPaging = true;
            PagerHorizontalAlign = HorizontalAlign.Justify;
            AllowColumnResize = true;
            AllowVirtualization = true;
            AllowFiltering = true;
            AllowPaging = true;
            AllowSorting = true;
            PageSize = 10;
            Density = Density.Default;
            GridLines = DataGridGridLines.Horizontal;


            //Traducción a español
            AndOperatorText = "Y";
            EqualsText = "Igual a";
            NotEqualsText = "No es igual a";
            LessThanText = "Menor qué";
            LessThanOrEqualsText = "Menor que o igual";
            GreaterThanText = "Mayor qué";
            GreaterThanOrEqualsText = "Mayor que o Igual";
            IsNullText = "Es nulo";
            IsNotNullText = "No es nulo";
            AndOperatorText = "Y";
            OrOperatorText = "O";
            ContainsText = "Contiene";
            DoesNotContainText = "No contiene";
            StartsWithText = "Inicia con";
            EndsWithText = "Termina con";
            ClearFilterText = "Limpiar";
            ApplyFilterText = "Aplicar";
            FilterText = "Filtrar";
            IsEmptyText = "Está vacío";
            IsNotEmptyText = "No está vacío";
            PageSizeText = "Página";
            EmptyText = "No hay registros para mostrar";
            PageSizeText = "Registros por página";
            LogicalFilterOperator = LogicalFilterOperator.Or;


            //Resumen de tabla
            string pagingSummaryFormat = "Página {0} de {1} ({2} registro(s) en total)";
            PagingSummaryFormat = pagingSummaryFormat;

            //Pagesize options
            IEnumerable<int> pageSizeOptions = new int[] { 10, 20, 30 };
            PageSizeOptions = pageSizeOptions;

            ShowPagingSummary = true;
            Responsive = true;

            this.Sort = EventCallback.Factory.Create<DataGridColumnSortEventArgs<TItem>>(this, ShowLoading);
            this.Page = EventCallback.Factory.Create<PagerEventArgs>(this, ShowLoading);
            this.Group = EventCallback.Factory.Create<DataGridColumnGroupEventArgs<TItem>>(this, ShowLoading);
            this.Filter = EventCallback.Factory.Create<DataGridColumnFilterEventArgs<TItem>>(this, ShowLoading);
        }

        async Task ShowLoading()
        {
            this.IsLoading = true;
            await Task.Yield();
            this.IsLoading = false;
        }
    }
}
