namespace Maui.DataGrid;

using Maui.DataGrid.Extensions;
using Microsoft.Maui.Controls;

internal sealed class DataGridRow : Grid
{
    #region Fields

    private Color? _bgColor;
    private Color? _textColor;
    private bool _hasSelected;

    #endregion Fields

    #region Properties

    public DataGrid DataGrid
    {
        get => (DataGrid)GetValue(DataGridProperty);
        set => SetValue(DataGridProperty, value);
    }

    #endregion Properties

    #region Bindable Properties

    public static readonly BindableProperty DataGridProperty =
        BindablePropertyExtensions.Create<DataGrid>(null, BindingMode.OneTime,
            propertyChanged: (b, o, n) =>
            {
                var self = (DataGridRow)b;

                if (o is DataGrid oldDataGrid)
                {
                    oldDataGrid.ItemSelected -= self.DataGrid_ItemSelected;
                }

                if (n is DataGrid newDataGrid && newDataGrid.SelectionEnabled)
                {
                    newDataGrid.ItemSelected += self.DataGrid_ItemSelected;
                }
            });

    #endregion Bindable Properties

    #region Methods

    private void CreateView()
    {
        ColumnDefinitions.Clear();
        Children.Clear();

        UpdateColors();

        for (var i = 0; i < DataGrid.Columns.Count; i++)
        {
            var col = DataGrid.Columns[i];

            ColumnDefinitions.Add(col.ColumnDefinition);

            if (!col.IsVisible)
            {
                continue;
            }

            var cell = CreateCell(col);

            SetColumn((BindableObject)cell, i);
            Children.Add(cell);
        }
    }

    private Border CreateCell(DataGridColumn col)
    {
        var cellBorder = new Border
        {
            BackgroundColor = _bgColor,
            Stroke = DataGrid.BorderColor,
            StrokeThickness = DataGrid.BorderThickness / 2
        };

        View cell;

        if (col.CellTemplate != null)
        {
            cell = new ContentView
            {
                Content = col.CellTemplate.CreateContent() as View
            };

            if (!string.IsNullOrWhiteSpace(col.PropertyName))
            {
                cell.SetBinding(BindingContextProperty,
                    new Binding(col.PropertyName, source: BindingContext));
            }
        }
        else
        {
            cell = new Label
            {
                TextColor = _textColor,
                BackgroundColor = _bgColor,
                VerticalTextAlignment = col.VerticalTextAlignment,
                HorizontalTextAlignment = col.HorizontalTextAlignment,
                LineBreakMode = col.LineBreakMode,
                FontSize = DataGrid.FontSize,
                FontFamily = DataGrid.FontFamily
            };

            if (!string.IsNullOrWhiteSpace(col.PropertyName))
            {
                cell.SetBinding(Label.TextProperty,
                    new Binding(col.PropertyName, BindingMode.Default, stringFormat: col.StringFormat, source: BindingContext));
            }
        }

        cellBorder.Content = cell;

        return cellBorder;
    }

    private void UpdateColors()
    {
        _hasSelected = DataGrid.SelectedItem == BindingContext;
        var rowIndex = DataGrid.InternalItems?.IndexOf(BindingContext) ?? -1;

        if (rowIndex < 0)
        {
            return;
        }

        _bgColor = DataGrid.SelectionEnabled && _hasSelected
                ? DataGrid.ActiveRowColor
                : DataGrid.RowsBackgroundColorPalette.GetColor(rowIndex, BindingContext);
        _textColor = DataGrid.RowsTextColorPalette.GetColor(rowIndex, BindingContext);

        foreach (var border in Children.OfType<Border>())
        {
            border.BackgroundColor = _bgColor;

            if (border.Content is Label label)
            {
                label.TextColor = _textColor;
            }
        }
    }

    /// <inheritdoc/>
    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (BindingContext != DataGrid.BindingContext)
        {
            CreateView();
        }
    }

    /// <inheritdoc/>
    protected override void OnParentSet()
    {
        base.OnParentSet();

        if (Parent == null)
        {
            DataGrid.ItemSelected -= DataGrid_ItemSelected;
        }
    }

    private void DataGrid_ItemSelected(object? sender, SelectionChangedEventArgs e)
    {
        if (!DataGrid.SelectionEnabled)
        {
            return;
        }

        if (_hasSelected || (e.CurrentSelection.Count > 0 && e.CurrentSelection[^1] == BindingContext))
        {
            UpdateColors();
        }
    }

    #endregion Methods
}
