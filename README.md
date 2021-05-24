# How to set content based column width GridImageColumn in Winforms DetailsViewDataGrid (SfDataGrid)?

## About the sample
This example illustrates how to set content based column width GridImageColumn in [Winforms DetailsViewDataGrid](https://www.syncfusion.com/winforms-ui-controls/datagrid) (SfDataGrid)? 

[WinForms DataGrid](https://www.syncfusion.com/winforms-ui-controls/datagrid) (SfDataGrid) does not provide the direct support to set content based column width [GridImageColumn](https://help.syncfusion.com/cr/windowsforms/Syncfusion.WinForms.DataGrid.GridImageColumn.html). You can set content based column width [GridImageColumn](https://help.syncfusion.com/cr/windowsforms/Syncfusion.WinForms.DataGrid.GridImageColumn.html) by customization the [QueryImageCellStyle](https://help.syncfusion.com/cr/windowsforms/Syncfusion.WinForms.DataGrid.SfDataGrid.html#Syncfusion_WinForms_DataGrid_SfDataGrid_QueryImageCellStyle) event in [WinForms DataGrid](https://www.syncfusion.com/winforms-ui-controls/datagrid) (SfDataGrid).

```C#

private void FirstLevelSourceDataGrid_QueryImageCellStyle(object sender, Syncfusion.WinForms.DataGrid.Events.QueryImageCellStyleEventArgs e)
{
            var orderDetails = (OrderDetails)e.Record;

            if (e.Column.MappingName == "DummyImageLink")
            {
                if (orderDetails.ImageLink.ToString() == "Sufficient")
                {
                    e.Image = Image.FromFile(@"..\..\Images\Sufficient.png");
                    e.DisplayText = string.Format("Product: {0} Quantity: {1} Discount: {2}", orderDetails.ProductID, orderDetails.Quantity, orderDetails.Discount);
                    e.TextImageRelation = TextImageRelation.ImageBeforeText;
                }

                else if (orderDetails.ImageLink.ToString() == "Perfect")
                {
                    e.Image = Image.FromFile(@"..\..\Images\Perfect.png");
                    e.DisplayText = string.Format("Product: {0} Quantity: {1} Discount: {2}", orderDetails.ProductID, orderDetails.Quantity, orderDetails.Discount);
                    e.TextImageRelation = TextImageRelation.ImageBeforeText;
                }

                else if (orderDetails.ImageLink.ToString() == "Insufficient")
                {
                    e.Image = Image.FromFile(@"..\..\Images\Insufficient.png");
                    e.DisplayText = string.Format("Product: {0} Quantity: {1} Discount: {2}", orderDetails.ProductID, orderDetails.Quantity, orderDetails.Discount);
                    e.TextImageRelation = TextImageRelation.ImageBeforeText;
                }
                
                //Get the DetailsViewDataGrid
                var detailsViewDataGrid = (e.OriginalSender as DetailsViewDataGrid);

                //Calculate the width DisplayText
                var textLength = columnwidth(e.DisplayText, e.Column.CellStyle, detailsViewDataGrid.Style.CellStyle);
                // add the image width and DisplayText width
                var width = e.Image.Width + textLength.Width;

                //set the width to ImageColumn
                e.Column.Width = e.Column.Width < width || double.IsNaN(e.Column.Width) ? width : e.Column.Width;
                //set the width to AutoSizeController
                detailsViewDataGrid.AutoSizeController.SetColumnWidth(e.Column, e.Column.Width);              
            }
}

private Size columnwidth(string displayText, CellStyleInfo columnStyle, CellStyleInfo gridStyle, int columnWidth = -1)
{       
            Size size = Size.Empty;
            int orientation = 0;
            
            if (columnWidth != -1)
            {
                if (columnStyle.HasValue(CellStyleInfoStore.TextMarginsProperty) || !gridStyle.HasValue(CellStyleInfoStore.TextMarginsProperty))
                    columnWidth -= columnStyle.TextMargins.Left + columnStyle.TextMargins.Right;
                else
                    columnWidth -= gridStyle.TextMargins.Left + gridStyle.TextMargins.Right;
                if (columnWidth <= 0)
                    columnWidth = 1;
            }
            //// WF-38338 - Column sizing didn't consider Grid wise styles.
            if (columnStyle.HasFont || !gridStyle.HasFont)
            {
                Font = columnStyle.GetFont();
                orientation = columnStyle.Font.Orientation;
            }
            else
            {
                Font = gridStyle.GetFont();
                orientation = gridStyle.Font.Orientation;
            }

            StringFormat StringFormat = new StringFormat();
            StringFormat.LineAlignment = ConvertToStringAlignment(columnStyle.HasVerticalAlignment ? columnStyle.VerticalAlignment : gridStyle.HasVerticalAlignment ? gridStyle.VerticalAlignment : columnStyle.VerticalAlignment);
            StringFormat.Alignment = ConvertToStringAlignment(columnStyle.HasHorizontalAlignment ? columnStyle.HorizontalAlignment : gridStyle.HasHorizontalAlignment ? gridStyle.HorizontalAlignment : columnStyle.HorizontalAlignment);
            leftBorderWeight = columnStyle.HasBorders ? columnStyle.Borders.Left.Weight :
                (gridStyle.HasBorders ? gridStyle.Borders.Left.Weight : columnStyle.Borders.Left.Weight);
            rightBorderWeight = columnStyle.HasBorders ? columnStyle.Borders.Left.Weight :
                (gridStyle.HasBorders ? gridStyle.Borders.Left.Weight : columnStyle.Borders.Left.Weight);

            if (orientation != 0)
            {
                var rect = GetRotatedTextBound(displayText, Font, StringFormat, orientation, Graphics.DpiY);
                size = Rectangle.Ceiling(rect).Size;
                size.Height += gridStyle.Borders.Top.Width + gridStyle.Borders.Bottom.Width;
            }
            else
            {

                if (columnWidth == -1)
                    size = Size.Ceiling(Graphics.MeasureString(displayText, Font, Point.Empty, StringFormat));
                else
                    size = Size.Ceiling(Graphics.MeasureString(displayText, Font, columnWidth, StringFormat));
            }

            //Adjusts width based on border weight.
            if (leftBorderWeight != GridBorderWeight.Thin)
                size.Width += GetWidthForWeight(leftBorderWeight);
            if (rightBorderWeight != GridBorderWeight.Thin)
                size.Width += GetWidthForWeight(rightBorderWeight);

            return size;
}  

```

![Shows the content based column width in DetailsViewDataGrid](ContentBasedColumnWidthGridImageColumn.gif)

Take a moment to peruse the [WinForms DataGrid – Column Sizing](https://help.syncfusion.com/windowsforms/datagrid/columns#column-sizing) documentation, where you can find about column sizing with code examples.

## Requirements to run the demo
Visual Studio 2015 and above versions
