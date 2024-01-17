namespace Maui.DataGrid.Extensions;

internal static class ViewExtensions
{
    internal static Grid WrapCellWithBorder(this View cellContent, DataGrid dataGrid, Color? bgColor)
    {
        // Define rows and columns for the border lines
        var cellGrid = new Grid
        {
            BackgroundColor = bgColor,
            RowDefinitions = { new RowDefinition { Height = new GridLength(dataGrid.BorderThickness.Top) }, new RowDefinition { Height = GridLength.Star }, new RowDefinition { Height = new GridLength(dataGrid.BorderThickness.Bottom) } },
            ColumnDefinitions = { new ColumnDefinition { Width = new GridLength(dataGrid.BorderThickness.Left) }, new ColumnDefinition { Width = GridLength.Star }, new ColumnDefinition { Width = new GridLength(dataGrid.BorderThickness.Right) } }
        };

        // Add border lines
        var leftBorder = new BoxView { Color = dataGrid.BorderColor };
        Grid.SetColumn(leftBorder, 0);
        Grid.SetRowSpan(leftBorder, 3);
        cellGrid.Children.Add(leftBorder);

        var rightBorder = new BoxView { Color = dataGrid.BorderColor };
        Grid.SetColumn(rightBorder, 2);
        Grid.SetRowSpan(rightBorder, 3);
        cellGrid.Children.Add(rightBorder);

        var topBorder = new BoxView { Color = dataGrid.BorderColor };
        Grid.SetRow(topBorder, 0);
        Grid.SetColumnSpan(topBorder, 3);
        cellGrid.Children.Add(topBorder);

        var bottomBorder = new BoxView { Color = dataGrid.BorderColor };
        Grid.SetRow(bottomBorder, 2);
        Grid.SetColumnSpan(bottomBorder, 3);
        cellGrid.Children.Add(bottomBorder);

        // Add content inside the borders
        Grid.SetColumn(cellContent, 1);
        Grid.SetRow(cellContent, 1);
        cellGrid.Children.Add(cellContent);

        return cellGrid;
    }
}
