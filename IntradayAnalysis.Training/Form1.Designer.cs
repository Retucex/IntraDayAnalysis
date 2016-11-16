namespace IntradayAnalysis.Training
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
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint1 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, "3,2,2.2,3");
			System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint2 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(1D, "5,2,3,4");
			System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Series series9 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Series series10 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
			this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.goLongButton = new System.Windows.Forms.Button();
			this.shortButton = new System.Windows.Forms.Button();
			this.nextDataButton = new System.Windows.Forms.Button();
			this.rakeInButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
			this.SuspendLayout();
			// 
			// chart1
			// 
			chartArea1.AlignmentOrientation = ((System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations)((System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Vertical | System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Horizontal)));
			chartArea1.AxisX.IsLabelAutoFit = false;
			chartArea1.AxisX.LabelStyle.Angle = 90;
			chartArea1.AxisX.LabelStyle.Enabled = false;
			chartArea1.AxisX.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			chartArea1.AxisX.LabelStyle.Format = "2f";
			chartArea1.AxisX.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
			chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
			chartArea1.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.DashDotDot;
			chartArea1.AxisX.MajorTickMark.Enabled = false;
			chartArea1.AxisX.MinorGrid.Enabled = true;
			chartArea1.AxisX.MinorGrid.LineColor = System.Drawing.Color.Gainsboro;
			chartArea1.AxisX.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
			chartArea1.AxisY.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
			chartArea1.AxisY.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			chartArea1.AxisY.LabelStyle.Format = ".00";
			chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
			chartArea1.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
			chartArea1.AxisY.MajorTickMark.Enabled = false;
			chartArea1.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
			chartArea1.AxisY2.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
			chartArea1.AxisY2.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
			chartArea1.AxisY2.LabelStyle.Format = "0.0000";
			chartArea1.AxisY2.LabelStyle.Interval = 0D;
			chartArea1.AxisY2.LabelStyle.IntervalOffset = 0D;
			chartArea1.AxisY2.LabelStyle.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
			chartArea1.AxisY2.LabelStyle.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
			chartArea1.AxisY2.MajorGrid.Enabled = false;
			chartArea1.AxisY2.MajorTickMark.Enabled = false;
			chartArea1.Name = "PriceArea";
			chartArea1.Position.Auto = false;
			chartArea1.Position.Height = 66F;
			chartArea1.Position.Width = 94F;
			chartArea1.Position.X = 3F;
			chartArea1.Position.Y = 5F;
			chartArea2.AlignmentOrientation = System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Horizontal;
			chartArea2.AlignmentStyle = System.Windows.Forms.DataVisualization.Charting.AreaAlignmentStyles.AxesView;
			chartArea2.AlignWithChartArea = "PriceArea";
			chartArea2.AxisX.IsLabelAutoFit = false;
			chartArea2.AxisX.LabelStyle.Angle = 90;
			chartArea2.AxisX.LabelStyle.Interval = 10D;
			chartArea2.AxisX.LabelStyle.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Minutes;
			chartArea2.AxisX.MajorGrid.Interval = 10D;
			chartArea2.AxisX.MajorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Minutes;
			chartArea2.AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
			chartArea2.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.DashDotDot;
			chartArea2.AxisX.MajorTickMark.Interval = 10D;
			chartArea2.AxisX.MajorTickMark.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Minutes;
			chartArea2.AxisX.MinorGrid.Enabled = true;
			chartArea2.AxisX.MinorGrid.LineColor = System.Drawing.Color.Gainsboro;
			chartArea2.AxisX.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
			chartArea2.AxisY.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
			chartArea2.AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
			chartArea2.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
			chartArea2.AxisY.MajorTickMark.Enabled = false;
			chartArea2.InnerPlotPosition.Auto = false;
			chartArea2.InnerPlotPosition.Height = 80.69314F;
			chartArea2.InnerPlotPosition.Width = 96.42035F;
			chartArea2.InnerPlotPosition.X = 3.33476F;
			chartArea2.InnerPlotPosition.Y = 2.42654F;
			chartArea2.Name = "VolumeArea";
			chartArea2.Position.Auto = false;
			chartArea2.Position.Height = 20F;
			chartArea2.Position.Width = 94F;
			chartArea2.Position.X = 3F;
			chartArea2.Position.Y = 72F;
			this.chart1.ChartAreas.Add(chartArea1);
			this.chart1.ChartAreas.Add(chartArea2);
			this.chart1.Location = new System.Drawing.Point(0, 0);
			this.chart1.Margin = new System.Windows.Forms.Padding(0);
			this.chart1.Name = "chart1";
			series1.ChartArea = "PriceArea";
			series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Candlestick;
			series1.Color = System.Drawing.Color.Black;
			series1.CustomProperties = "PriceDownColor=Red, PointWidth=0.5, PriceUpColor=Green";
			series1.MarkerBorderColor = System.Drawing.Color.Black;
			series1.MarkerColor = System.Drawing.Color.Black;
			series1.Name = "Hloc5Min";
			series1.Points.Add(dataPoint1);
			series1.Points.Add(dataPoint2);
			series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
			series1.YValuesPerPoint = 4;
			series2.ChartArea = "VolumeArea";
			series2.Name = "Volume5Min";
			series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
			series3.ChartArea = "PriceArea";
			series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Range;
			series3.Color = System.Drawing.Color.Gold;
			series3.Name = "LocalHL";
			series3.YValuesPerPoint = 2;
			series4.ChartArea = "PriceArea";
			series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
			series4.Color = System.Drawing.Color.Lime;
			series4.Name = "HighBound";
			series5.ChartArea = "PriceArea";
			series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
			series5.Color = System.Drawing.Color.Red;
			series5.Name = "LowBound";
			series6.ChartArea = "PriceArea";
			series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
			series6.Color = System.Drawing.Color.DeepSkyBlue;
			series6.Name = "BuyPrice";
			series7.ChartArea = "PriceArea";
			series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
			series7.Color = System.Drawing.Color.Red;
			series7.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Triangle;
			series7.Name = "ShortPoints";
			series8.ChartArea = "PriceArea";
			series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
			series8.Color = System.Drawing.Color.Lime;
			series8.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Triangle;
			series8.Name = "LongPoints";
			series9.ChartArea = "PriceArea";
			series9.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
			series9.Color = System.Drawing.Color.Black;
			series9.Name = "GapLine";
			series10.ChartArea = "PriceArea";
			series10.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Range;
			series10.Color = System.Drawing.Color.Red;
			series10.Name = "ProfitMargin";
			series10.YValuesPerPoint = 2;
			this.chart1.Series.Add(series1);
			this.chart1.Series.Add(series2);
			this.chart1.Series.Add(series3);
			this.chart1.Series.Add(series4);
			this.chart1.Series.Add(series5);
			this.chart1.Series.Add(series6);
			this.chart1.Series.Add(series7);
			this.chart1.Series.Add(series8);
			this.chart1.Series.Add(series9);
			this.chart1.Series.Add(series10);
			this.chart1.Size = new System.Drawing.Size(1443, 921);
			this.chart1.TabIndex = 0;
			title1.Alignment = System.Drawing.ContentAlignment.TopCenter;
			title1.DockedToChartArea = "PriceArea";
			title1.DockingOffset = 1;
			title1.IsDockedInsideChartArea = false;
			title1.Name = "TitleStockDetails";
			title1.Text = "terwgfdsgdsf";
			this.chart1.Titles.Add(title1);
			// 
			// goLongButton
			// 
			this.goLongButton.Location = new System.Drawing.Point(12, 12);
			this.goLongButton.Name = "goLongButton";
			this.goLongButton.Size = new System.Drawing.Size(75, 23);
			this.goLongButton.TabIndex = 1;
			this.goLongButton.Text = "Go Long";
			this.goLongButton.UseVisualStyleBackColor = true;
			// 
			// shortButton
			// 
			this.shortButton.Location = new System.Drawing.Point(93, 12);
			this.shortButton.Name = "shortButton";
			this.shortButton.Size = new System.Drawing.Size(75, 23);
			this.shortButton.TabIndex = 2;
			this.shortButton.Text = "Short";
			this.shortButton.UseVisualStyleBackColor = true;
			// 
			// nextDataButton
			// 
			this.nextDataButton.Location = new System.Drawing.Point(203, 12);
			this.nextDataButton.Name = "nextDataButton";
			this.nextDataButton.Size = new System.Drawing.Size(75, 23);
			this.nextDataButton.TabIndex = 3;
			this.nextDataButton.Text = "+5 Min";
			this.nextDataButton.UseVisualStyleBackColor = true;
			// 
			// rakeInButton
			// 
			this.rakeInButton.Location = new System.Drawing.Point(284, 12);
			this.rakeInButton.Name = "rakeInButton";
			this.rakeInButton.Size = new System.Drawing.Size(75, 23);
			this.rakeInButton.TabIndex = 4;
			this.rakeInButton.Text = "Rake In";
			this.rakeInButton.UseVisualStyleBackColor = true;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1427, 882);
			this.Controls.Add(this.rakeInButton);
			this.Controls.Add(this.nextDataButton);
			this.Controls.Add(this.shortButton);
			this.Controls.Add(this.goLongButton);
			this.Controls.Add(this.chart1);
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Chart";
			((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private Chart chart1;
		private Button goLongButton;
		private Button shortButton;
		private Button nextDataButton;
		private Button rakeInButton;
	}
}

