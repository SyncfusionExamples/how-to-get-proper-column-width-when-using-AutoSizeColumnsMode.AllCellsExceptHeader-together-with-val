#region Copyright Syncfusion Inc. 2001-2018.
// Copyright Syncfusion Inc. 2001-2018. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Styles;
using Syncfusion.WinForms.Input.Enums;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SfDataGridDemo
{
    public partial class MyForm2 : Form
    {
        public MyForm2()
        {
            InitializeComponent();
            SampleCustomization();
            this.Load += MyForm2_Load;
        }

        /// <summary>
        /// Occurs when the form is loading.
        /// </summary>
        /// <param name="sender">The sender of event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> that contains event data.</param>
        private void MyForm2_Load(object sender, EventArgs e)
        {
            // this.sfDataGrid1.ExpandAllDetailsView();
        }
        SfDataGrid firstLevelSourceDataGrid;
        /// <summary>
        /// Sets the sample customization settings.
        /// </summary>
        private void SampleCustomization()
        {
            this.sfDataGrid1.AllowEditing = true;
            this.sfDataGrid1.AllowGrouping = true;
            this.sfDataGrid1.AutoGenerateColumns = false;
            this.sfDataGrid1.DetailsViewPadding = new Padding(27, 0, 0, 0);
            this.sfDataGrid1.NavigationMode = NavigationMode.Row;

            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalDigits = 0;
            nfi.NumberGroupSizes = new int[] { };

            OrderInfoRepository orderInfo = new OrderInfoRepository();
            this.sfDataGrid1.DataSource = orderInfo.GetOrdersDetails(30, false);
            this.sfDataGrid1.Columns.Add(new GridNumericColumn() { MappingName = "OrderID", HeaderText = "Order ID", NumberFormatInfo = nfi });
            this.sfDataGrid1.Columns.Add(new GridTextColumn() { MappingName = "CustomerID", HeaderText = "Customer ID" });            
            this.sfDataGrid1.Columns.Add(new GridTextColumn() { MappingName = "ShipCity", HeaderText = "Ship City" });
            this.sfDataGrid1.Columns.Add(new GridTextColumn() { MappingName = "ShipCountry", HeaderText = "Ship Country" });
            this.sfDataGrid1.Columns.Add(new GridDateTimeColumn() { MappingName = "ShippingDate", HeaderText = "Shipping Date" });
            this.sfDataGrid1.Columns.Add(new GridNumericColumn() { MappingName = "Freight", HeaderText = "Freight", FormatMode = FormatMode.Currency });
            this.sfDataGrid1.Columns.Add(new GridCheckBoxColumn() { MappingName = "IsClosed", HeaderText = "Is Closed", CheckBoxSize = new Size(14, 14) });
            this.sfDataGrid1.DetailsViewExpanding += SfDataGrid1_DetailsViewExpanding;

            #region Relation Creation

            firstLevelSourceDataGrid  = new SfDataGrid();            
            firstLevelSourceDataGrid.AutoGenerateColumns = false;
            //firstLevelSourceDataGrid.AutoSizeColumnsMode = AutoSizeColumnsMode.AllCellsExceptHeader;
            firstLevelSourceDataGrid.RowHeight = 21;
            //firstLevelSourceDataGrid.HeaderRowHeight = 0;
            firstLevelSourceDataGrid.Columns.Add(new GridNumericColumn() { MappingName = "OrderID", HeaderText = "Order ID", NumberFormatInfo = nfi });
            firstLevelSourceDataGrid.Columns.Add(new GridTextColumn() { MappingName = "CustomerID", HeaderText = "Customer ID" });           
            firstLevelSourceDataGrid.Columns.Add(new GridTextColumn() { MappingName = "CustomerCity", HeaderText = "Customer City" });
            //firstLevelSourceDataGrid.Columns.Add(new GridNumericColumn() { MappingName = "ProductID", HeaderText = "Product ID", FormatMode = FormatMode.Numeric, NumberFormatInfo = nfi });
            //firstLevelSourceDataGrid.Columns.Add(new GridHyperlinkColumn() { MappingName = "HyperLink", HeaderText = "HyperLink" });
            //firstLevelSourceDataGrid.Columns.Add(new GridDateTimeColumn() { MappingName = "OrderDate", HeaderText = "Order Date" });
            //firstLevelSourceDataGrid.Columns.Add(new GridNumericColumn() { MappingName = "UnitPrice", HeaderText = "Unit Price", FormatMode = FormatMode.Currency });
            //CellStyleInfo cellStyle = new CellStyleInfo();
            //cellStyle.HorizontalAlignment = HorizontalAlignment.Right;
            //firstLevelSourceDataGrid.Columns.Add(new GridUnboundColumn() { MappingName = "QuantitiesPrice", HeaderText = "Grand Total", Expression = "UnitPrice * Quantity", CellStyle = cellStyle });
            //firstLevelSourceDataGrid.Columns.Add(new GridNumericColumn() { MappingName = "Discount", HeaderText = "Discount", FormatMode = FormatMode.Percent });
            firstLevelSourceDataGrid.Columns.Add(new GridImageColumn() { MappingName = "DummyImageLink", HeaderText = "", ImageLayout = ImageLayout.None });
                     
            GridViewDefinition viewDefinition = new GridViewDefinition();
            viewDefinition.RelationalColumn = "OrderID";
           
            firstLevelSourceDataGrid.QueryImageCellStyle += FirstLevelSourceDataGrid_QueryImageCellStyle;
           
            viewDefinition.DataGrid = firstLevelSourceDataGrid;
            this.sfDataGrid1.DetailsViewDefinitions.Add(viewDefinition);
            #endregion

            this.sfDataGrid1.HideEmptyGridViewDefinition = false;
        }

        private void SfDataGrid1_DetailsViewExpanding(object sender, Syncfusion.WinForms.DataGrid.Events.DetailsViewExpandingEventArgs e)
        {
            OrderInfo orderInfo = e.Record as OrderInfo;            
            e.DetailsViewDataSource.Add("OrderID", GetDetailsViewDataSource(orderInfo));            
        }

        private ObservableCollection<OrderDetails> GetDetailsViewDataSource(OrderInfo orderInfo)
        {
            ObservableCollection<OrderDetails> orderDetails = new ObservableCollection<OrderDetails>();
            orderDetails.Add(new OrderDetails(orderInfo.OrderID, 2, 11, 2, 3, "Alan", DateTime.Today.AddDays(1), "Orlando"));
            orderDetails.Add(new OrderDetails(orderInfo.OrderID, 2, 11, 2, 3, "Michael", DateTime.Today, "Chennai"));
            return orderDetails;
        }

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

        Graphics graphics;

        internal Graphics Graphics
        {
            get
            {
                if (graphics == null)
                    graphics = this.CreateGraphics();
                return graphics;
            }
        }

        internal static StringAlignment ConvertToStringAlignment(VerticalAlignment align)
        {
            switch (align)
            {
                case VerticalAlignment.Bottom: return StringAlignment.Far;
                case VerticalAlignment.Center: return StringAlignment.Center;
                default: return StringAlignment.Near;
            }
        }

        internal static StringAlignment ConvertToStringAlignment(HorizontalAlignment align)
        {
            switch (align)
            {
                case HorizontalAlignment.Right: return StringAlignment.Far;
                case HorizontalAlignment.Center: return StringAlignment.Center;
                default: return StringAlignment.Near;
            }
        }

        internal static RectangleF GetRotatedTextBound(string text, Font font, StringFormat format, float rotation, float dpiY)
        {
            GraphicsPath gp = new GraphicsPath();

            // Default final font size is considered as 72.
            float size = dpiY * font.Size / 72;
            gp.AddString(text, font.FontFamily, (int)font.Style, size, new PointF(0, 0), format);

            Matrix mat = new Matrix();
            mat.Rotate(rotation, MatrixOrder.Append);
            gp.Transform(mat);
            return gp.GetBounds();
        }

        internal static int GetWidthForWeight(GridBorderWeight weight)
        {
            int width = 1;
            switch (weight)
            {
                case GridBorderWeight.ExtraThin:
                    width = 1;
                    break;

                case GridBorderWeight.Thin:
                    width = 1;
                    break;

                case GridBorderWeight.Medium:
                    width = 2;
                    break;

                case GridBorderWeight.Thick:
                    width = 3;
                    break;

                case GridBorderWeight.ExtraThick:
                    width = 4;
                    break;

                case GridBorderWeight.ExtraExtraThick:
                    width = 4;
                    break;
            }

            return width;
        }
        GridBorderWeight leftBorderWeight = new GridBorderWeight();
        GridBorderWeight rightBorderWeight = new GridBorderWeight();
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
    }
}
