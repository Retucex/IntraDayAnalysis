namespace IntradayAnalysis.Charts
{
	partial class VolumeShortLong
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

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
			System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
			this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
			((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
			this.SuspendLayout();
			// 
			// chart1
			// 
			chartArea1.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
			chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.DarkGray;
			chartArea1.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.DashDotDot;
			chartArea1.AxisX.MinorGrid.Enabled = true;
			chartArea1.AxisX.MinorGrid.LineColor = System.Drawing.Color.Gainsboro;
			chartArea1.AxisX.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
			chartArea1.AxisY.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
			chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.DarkGray;
			chartArea1.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.DashDotDot;
			chartArea1.AxisY.MinorGrid.Enabled = true;
			chartArea1.AxisY.MinorGrid.LineColor = System.Drawing.Color.Gainsboro;
			chartArea1.AxisY.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
			chartArea1.Name = "VolumeArea";
			this.chart1.ChartAreas.Add(chartArea1);
			this.chart1.Location = new System.Drawing.Point(0, 0);
			this.chart1.Margin = new System.Windows.Forms.Padding(0);
			this.chart1.Name = "chart1";
			series1.ChartArea = "VolumeArea";
			series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.RangeColumn;
			series1.MarkerColor = System.Drawing.Color.Red;
			series1.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Square;
			series1.Name = "VolumeSerie";
			series1.YValuesPerPoint = 3;
			this.chart1.Series.Add(series1);
			this.chart1.Size = new System.Drawing.Size(1490, 895);
			this.chart1.TabIndex = 0;
			this.chart1.Text = "chart1";
			// 
			// VolumeShortLong
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1492, 903);
			this.Controls.Add(this.chart1);
			this.Name = "VolumeShortLong";
			this.Text = "VolumeShortLong";
			((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
	}
}