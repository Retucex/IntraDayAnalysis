namespace IntradayAnalysis.Charts
{
	using System.ComponentModel;
	using System.Runtime.Remoting.Channels;
	using System.Windows.Forms;
	using System.Windows.Forms.DataVisualization.Charting;

	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Series series11 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint3 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, "3,2,2.2,3");
			System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint4 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(1D, "5,2,3,4");
			System.Windows.Forms.DataVisualization.Charting.Series series12 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Series series13 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Series series14 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Series series15 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Series series16 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Series series17 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Series series18 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Series series19 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Series series20 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
			this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.button1 = new System.Windows.Forms.Button();
			this.searchBox = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
			this.SuspendLayout();
			// 
			// chart1
			// 
			chartArea3.AlignmentOrientation = ((System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations)((System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Vertical | System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Horizontal)));
			chartArea3.AxisX.IsLabelAutoFit = false;
			chartArea3.AxisX.LabelStyle.Angle = 90;
			chartArea3.AxisX.LabelStyle.Enabled = false;
			chartArea3.AxisX.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			chartArea3.AxisX.LabelStyle.Format = "2f";
			chartArea3.AxisX.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
			chartArea3.AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
			chartArea3.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.DashDotDot;
			chartArea3.AxisX.MajorTickMark.Enabled = false;
			chartArea3.AxisX.MinorGrid.Enabled = true;
			chartArea3.AxisX.MinorGrid.LineColor = System.Drawing.Color.Gainsboro;
			chartArea3.AxisX.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
			chartArea3.AxisY.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
			chartArea3.AxisY.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			chartArea3.AxisY.LabelStyle.Format = ".00";
			chartArea3.AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
			chartArea3.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
			chartArea3.AxisY.MajorTickMark.Enabled = false;
			chartArea3.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
			chartArea3.AxisY2.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
			chartArea3.AxisY2.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
			chartArea3.AxisY2.LabelStyle.Format = "0.0000";
			chartArea3.AxisY2.LabelStyle.Interval = 0D;
			chartArea3.AxisY2.LabelStyle.IntervalOffset = 0D;
			chartArea3.AxisY2.LabelStyle.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
			chartArea3.AxisY2.LabelStyle.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
			chartArea3.AxisY2.MajorGrid.Enabled = false;
			chartArea3.AxisY2.MajorTickMark.Enabled = false;
			chartArea3.Name = "PriceArea";
			chartArea3.Position.Auto = false;
			chartArea3.Position.Height = 66F;
			chartArea3.Position.Width = 94F;
			chartArea3.Position.X = 3F;
			chartArea3.Position.Y = 5F;
			chartArea4.AlignmentOrientation = System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Horizontal;
			chartArea4.AlignmentStyle = System.Windows.Forms.DataVisualization.Charting.AreaAlignmentStyles.AxesView;
			chartArea4.AlignWithChartArea = "PriceArea";
			chartArea4.AxisX.IsLabelAutoFit = false;
			chartArea4.AxisX.LabelStyle.Angle = 90;
			chartArea4.AxisX.LabelStyle.Interval = 10D;
			chartArea4.AxisX.LabelStyle.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Minutes;
			chartArea4.AxisX.MajorGrid.Interval = 10D;
			chartArea4.AxisX.MajorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Minutes;
			chartArea4.AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
			chartArea4.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.DashDotDot;
			chartArea4.AxisX.MajorTickMark.Interval = 10D;
			chartArea4.AxisX.MajorTickMark.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Minutes;
			chartArea4.AxisX.MinorGrid.Enabled = true;
			chartArea4.AxisX.MinorGrid.LineColor = System.Drawing.Color.Gainsboro;
			chartArea4.AxisX.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
			chartArea4.AxisY.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
			chartArea4.AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
			chartArea4.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
			chartArea4.AxisY.MajorTickMark.Enabled = false;
			chartArea4.InnerPlotPosition.Auto = false;
			chartArea4.InnerPlotPosition.Height = 80.69314F;
			chartArea4.InnerPlotPosition.Width = 96.42035F;
			chartArea4.InnerPlotPosition.X = 3.33476F;
			chartArea4.InnerPlotPosition.Y = 2.42654F;
			chartArea4.Name = "VolumeArea";
			chartArea4.Position.Auto = false;
			chartArea4.Position.Height = 20F;
			chartArea4.Position.Width = 94F;
			chartArea4.Position.X = 3F;
			chartArea4.Position.Y = 72F;
			this.chart1.ChartAreas.Add(chartArea3);
			this.chart1.ChartAreas.Add(chartArea4);
			this.chart1.Location = new System.Drawing.Point(0, 0);
			this.chart1.Margin = new System.Windows.Forms.Padding(0);
			this.chart1.Name = "chart1";
			series11.ChartArea = "PriceArea";
			series11.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Candlestick;
			series11.Color = System.Drawing.Color.Black;
			series11.CustomProperties = "PriceDownColor=Red, PointWidth=0.5, PriceUpColor=Green";
			series11.MarkerBorderColor = System.Drawing.Color.Black;
			series11.MarkerColor = System.Drawing.Color.Black;
			series11.Name = "Hloc5Min";
			series11.Points.Add(dataPoint3);
			series11.Points.Add(dataPoint4);
			series11.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
			series11.YValuesPerPoint = 4;
			series12.ChartArea = "VolumeArea";
			series12.Name = "Volume5Min";
			series12.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
			series13.ChartArea = "PriceArea";
			series13.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Range;
			series13.Color = System.Drawing.Color.Gold;
			series13.Name = "LocalHL";
			series13.YValuesPerPoint = 2;
			series14.ChartArea = "PriceArea";
			series14.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
			series14.Color = System.Drawing.Color.Lime;
			series14.Name = "HighBound";
			series15.ChartArea = "PriceArea";
			series15.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
			series15.Color = System.Drawing.Color.Red;
			series15.Name = "LowBound";
			series16.ChartArea = "PriceArea";
			series16.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
			series16.Color = System.Drawing.Color.DeepSkyBlue;
			series16.Name = "BuyPrice";
			series17.ChartArea = "PriceArea";
			series17.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
			series17.Color = System.Drawing.Color.Red;
			series17.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Triangle;
			series17.Name = "ShortPoints";
			series18.ChartArea = "PriceArea";
			series18.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
			series18.Color = System.Drawing.Color.Lime;
			series18.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Triangle;
			series18.Name = "LongPoints";
			series19.ChartArea = "PriceArea";
			series19.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
			series19.Color = System.Drawing.Color.Black;
			series19.Name = "GapLine";
			series20.ChartArea = "PriceArea";
			series20.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Range;
			series20.Color = System.Drawing.Color.Red;
			series20.Name = "ProfitMargin";
			series20.YValuesPerPoint = 2;
			this.chart1.Series.Add(series11);
			this.chart1.Series.Add(series12);
			this.chart1.Series.Add(series13);
			this.chart1.Series.Add(series14);
			this.chart1.Series.Add(series15);
			this.chart1.Series.Add(series16);
			this.chart1.Series.Add(series17);
			this.chart1.Series.Add(series18);
			this.chart1.Series.Add(series19);
			this.chart1.Series.Add(series20);
			this.chart1.Size = new System.Drawing.Size(1443, 921);
			this.chart1.TabIndex = 0;
			title2.Alignment = System.Drawing.ContentAlignment.TopCenter;
			title2.DockedToChartArea = "PriceArea";
			title2.DockingOffset = 1;
			title2.IsDockedInsideChartArea = false;
			title2.Name = "TitleStockDetails";
			title2.Text = "terwgfdsgdsf";
			this.chart1.Titles.Add(title2);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(12, 12);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(31, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "Go";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// searchBox
			// 
			this.searchBox.Location = new System.Drawing.Point(49, 14);
			this.searchBox.Name = "searchBox";
			this.searchBox.Size = new System.Drawing.Size(88, 20);
			this.searchBox.TabIndex = 2;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1427, 882);
			this.Controls.Add(this.searchBox);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.chart1);
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Chart";
			((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Chart chart1;
		private Button button1;
		private TextBox searchBox;
	}
}

