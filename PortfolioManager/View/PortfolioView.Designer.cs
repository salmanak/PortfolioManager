namespace PortfolioManager.View
{
    partial class PortfolioView
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.splitContainerMaster = new System.Windows.Forms.SplitContainer();
            this.splitContainerLeft = new System.Windows.Forms.SplitContainer();
            this.panelAction = new System.Windows.Forms.Panel();
            this.btnAddTrade = new System.Windows.Forms.Button();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.txtShares = new System.Windows.Forms.TextBox();
            this.labelPrice = new System.Windows.Forms.Label();
            this.labelShares = new System.Windows.Forms.Label();
            this.txtSymbol = new System.Windows.Forms.TextBox();
            this.labelSymbol = new System.Windows.Forms.Label();
            this.panelSummary = new System.Windows.Forms.Panel();
            this.lblUnRlzPLValue = new System.Windows.Forms.Label();
            this.labelUnRlzPL = new System.Windows.Forms.Label();
            this.lblPortfolioCost = new System.Windows.Forms.Label();
            this.labelPortfolioCost = new System.Windows.Forms.Label();
            this.splitContainerRight = new System.Windows.Forms.SplitContainer();
            this.panelGrid = new System.Windows.Forms.Panel();
            this.dataGridViewPortfolio = new System.Windows.Forms.DataGridView();
            this.panelChart = new System.Windows.Forms.Panel();
            this.chartPortfolio = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.timerChart = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMaster)).BeginInit();
            this.splitContainerMaster.Panel1.SuspendLayout();
            this.splitContainerMaster.Panel2.SuspendLayout();
            this.splitContainerMaster.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLeft)).BeginInit();
            this.splitContainerLeft.Panel1.SuspendLayout();
            this.splitContainerLeft.Panel2.SuspendLayout();
            this.splitContainerLeft.SuspendLayout();
            this.panelAction.SuspendLayout();
            this.panelSummary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRight)).BeginInit();
            this.splitContainerRight.Panel1.SuspendLayout();
            this.splitContainerRight.Panel2.SuspendLayout();
            this.splitContainerRight.SuspendLayout();
            this.panelGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPortfolio)).BeginInit();
            this.panelChart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartPortfolio)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerMaster
            // 
            this.splitContainerMaster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMaster.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMaster.Name = "splitContainerMaster";
            // 
            // splitContainerMaster.Panel1
            // 
            this.splitContainerMaster.Panel1.Controls.Add(this.splitContainerLeft);
            // 
            // splitContainerMaster.Panel2
            // 
            this.splitContainerMaster.Panel2.Controls.Add(this.splitContainerRight);
            this.splitContainerMaster.Size = new System.Drawing.Size(952, 404);
            this.splitContainerMaster.SplitterDistance = 170;
            this.splitContainerMaster.TabIndex = 4;
            // 
            // splitContainerLeft
            // 
            this.splitContainerLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerLeft.Location = new System.Drawing.Point(0, 0);
            this.splitContainerLeft.Name = "splitContainerLeft";
            this.splitContainerLeft.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerLeft.Panel1
            // 
            this.splitContainerLeft.Panel1.Controls.Add(this.panelAction);
            // 
            // splitContainerLeft.Panel2
            // 
            this.splitContainerLeft.Panel2.Controls.Add(this.panelSummary);
            this.splitContainerLeft.Size = new System.Drawing.Size(170, 404);
            this.splitContainerLeft.SplitterDistance = 190;
            this.splitContainerLeft.TabIndex = 0;
            // 
            // panelAction
            // 
            this.panelAction.Controls.Add(this.btnAddTrade);
            this.panelAction.Controls.Add(this.txtPrice);
            this.panelAction.Controls.Add(this.txtShares);
            this.panelAction.Controls.Add(this.labelPrice);
            this.panelAction.Controls.Add(this.labelShares);
            this.panelAction.Controls.Add(this.txtSymbol);
            this.panelAction.Controls.Add(this.labelSymbol);
            this.panelAction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAction.Location = new System.Drawing.Point(0, 0);
            this.panelAction.Name = "panelAction";
            this.panelAction.Size = new System.Drawing.Size(170, 190);
            this.panelAction.TabIndex = 2;
            // 
            // btnAddTrade
            // 
            this.btnAddTrade.Location = new System.Drawing.Point(53, 98);
            this.btnAddTrade.Name = "btnAddTrade";
            this.btnAddTrade.Size = new System.Drawing.Size(100, 23);
            this.btnAddTrade.TabIndex = 8;
            this.btnAddTrade.Text = "Add Trade";
            this.btnAddTrade.UseVisualStyleBackColor = true;
            this.btnAddTrade.Click += new System.EventHandler(this.btnAddTrade_Click);
            // 
            // txtPrice
            // 
            this.txtPrice.Location = new System.Drawing.Point(53, 68);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(100, 20);
            this.txtPrice.TabIndex = 7;
            this.txtPrice.Validating += new System.ComponentModel.CancelEventHandler(this.txtPrice_Validating);
            // 
            // txtShares
            // 
            this.txtShares.Location = new System.Drawing.Point(53, 40);
            this.txtShares.Name = "txtShares";
            this.txtShares.Size = new System.Drawing.Size(100, 20);
            this.txtShares.TabIndex = 6;
            this.txtShares.Validating += new System.ComponentModel.CancelEventHandler(this.txtShares_Validating);
            // 
            // labelPrice
            // 
            this.labelPrice.AutoSize = true;
            this.labelPrice.Location = new System.Drawing.Point(9, 76);
            this.labelPrice.Name = "labelPrice";
            this.labelPrice.Size = new System.Drawing.Size(31, 13);
            this.labelPrice.TabIndex = 5;
            this.labelPrice.Text = "Price";
            // 
            // labelShares
            // 
            this.labelShares.AutoSize = true;
            this.labelShares.Location = new System.Drawing.Point(9, 48);
            this.labelShares.Name = "labelShares";
            this.labelShares.Size = new System.Drawing.Size(40, 13);
            this.labelShares.TabIndex = 3;
            this.labelShares.Text = "Shares";
            // 
            // txtSymbol
            // 
            this.txtSymbol.Location = new System.Drawing.Point(53, 12);
            this.txtSymbol.Name = "txtSymbol";
            this.txtSymbol.Size = new System.Drawing.Size(100, 20);
            this.txtSymbol.TabIndex = 2;
            this.txtSymbol.Validating += new System.ComponentModel.CancelEventHandler(this.txtSymbol_Validating);
            // 
            // labelSymbol
            // 
            this.labelSymbol.AutoSize = true;
            this.labelSymbol.Location = new System.Drawing.Point(9, 16);
            this.labelSymbol.Name = "labelSymbol";
            this.labelSymbol.Size = new System.Drawing.Size(41, 13);
            this.labelSymbol.TabIndex = 1;
            this.labelSymbol.Text = "Symbol";
            // 
            // panelSummary
            // 
            this.panelSummary.Controls.Add(this.lblUnRlzPLValue);
            this.panelSummary.Controls.Add(this.labelUnRlzPL);
            this.panelSummary.Controls.Add(this.lblPortfolioCost);
            this.panelSummary.Controls.Add(this.labelPortfolioCost);
            this.panelSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSummary.Location = new System.Drawing.Point(0, 0);
            this.panelSummary.Name = "panelSummary";
            this.panelSummary.Size = new System.Drawing.Size(170, 210);
            this.panelSummary.TabIndex = 3;
            // 
            // lblUnRlzPLValue
            // 
            this.lblUnRlzPLValue.AutoSize = true;
            this.lblUnRlzPLValue.Location = new System.Drawing.Point(15, 89);
            this.lblUnRlzPLValue.Name = "lblUnRlzPLValue";
            this.lblUnRlzPLValue.Size = new System.Drawing.Size(22, 13);
            this.lblUnRlzPLValue.TabIndex = 5;
            this.lblUnRlzPLValue.Text = "0.0";
            this.lblUnRlzPLValue.TextChanged += new System.EventHandler(this.lblUnRlzPLValue_TextChanged);
            // 
            // labelUnRlzPL
            // 
            this.labelUnRlzPL.AutoSize = true;
            this.labelUnRlzPL.Location = new System.Drawing.Point(15, 76);
            this.labelUnRlzPL.Name = "labelUnRlzPL";
            this.labelUnRlzPL.Size = new System.Drawing.Size(137, 13);
            this.labelUnRlzPL.TabIndex = 4;
            this.labelUnRlzPL.Text = "Un-Realized Profit/Loss ($):";
            // 
            // lblPortfolioCost
            // 
            this.lblPortfolioCost.AutoSize = true;
            this.lblPortfolioCost.Location = new System.Drawing.Point(15, 59);
            this.lblPortfolioCost.Name = "lblPortfolioCost";
            this.lblPortfolioCost.Size = new System.Drawing.Size(22, 13);
            this.lblPortfolioCost.TabIndex = 3;
            this.lblPortfolioCost.Text = "0.0";
            // 
            // labelPortfolioCost
            // 
            this.labelPortfolioCost.AutoSize = true;
            this.labelPortfolioCost.Location = new System.Drawing.Point(13, 46);
            this.labelPortfolioCost.Name = "labelPortfolioCost";
            this.labelPortfolioCost.Size = new System.Drawing.Size(87, 13);
            this.labelPortfolioCost.TabIndex = 2;
            this.labelPortfolioCost.Text = "Portfolio Cost ($):";
            // 
            // splitContainerRight
            // 
            this.splitContainerRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerRight.Location = new System.Drawing.Point(0, 0);
            this.splitContainerRight.Name = "splitContainerRight";
            this.splitContainerRight.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerRight.Panel1
            // 
            this.splitContainerRight.Panel1.Controls.Add(this.panelGrid);
            // 
            // splitContainerRight.Panel2
            // 
            this.splitContainerRight.Panel2.Controls.Add(this.panelChart);
            this.splitContainerRight.Size = new System.Drawing.Size(778, 404);
            this.splitContainerRight.SplitterDistance = 220;
            this.splitContainerRight.TabIndex = 0;
            // 
            // panelGrid
            // 
            this.panelGrid.Controls.Add(this.dataGridViewPortfolio);
            this.panelGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGrid.Location = new System.Drawing.Point(0, 0);
            this.panelGrid.Name = "panelGrid";
            this.panelGrid.Size = new System.Drawing.Size(778, 220);
            this.panelGrid.TabIndex = 1;
            // 
            // dataGridViewPortfolio
            // 
            this.dataGridViewPortfolio.AllowUserToAddRows = false;
            this.dataGridViewPortfolio.AllowUserToDeleteRows = false;
            this.dataGridViewPortfolio.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPortfolio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewPortfolio.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewPortfolio.Name = "dataGridViewPortfolio";
            this.dataGridViewPortfolio.ReadOnly = true;
            this.dataGridViewPortfolio.Size = new System.Drawing.Size(778, 220);
            this.dataGridViewPortfolio.TabIndex = 0;
            // 
            // panelChart
            // 
            this.panelChart.Controls.Add(this.chartPortfolio);
            this.panelChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelChart.Location = new System.Drawing.Point(0, 0);
            this.panelChart.Name = "panelChart";
            this.panelChart.Size = new System.Drawing.Size(778, 180);
            this.panelChart.TabIndex = 4;
            // 
            // chartPortfolio
            // 
            this.chartPortfolio.BackColor = System.Drawing.Color.Silver;
            this.chartPortfolio.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
            this.chartPortfolio.BackSecondaryColor = System.Drawing.Color.White;
            chartArea1.BackColor = System.Drawing.Color.DarkGray;
            chartArea1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
            chartArea1.Name = "ChartArea1";
            this.chartPortfolio.ChartAreas.Add(chartArea1);
            this.chartPortfolio.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chartPortfolio.Legends.Add(legend1);
            this.chartPortfolio.Location = new System.Drawing.Point(0, 0);
            this.chartPortfolio.Name = "chartPortfolio";
            this.chartPortfolio.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartPortfolio.Series.Add(series1);
            this.chartPortfolio.Size = new System.Drawing.Size(778, 180);
            this.chartPortfolio.TabIndex = 0;
            this.chartPortfolio.Text = "Portfolio Chart";
            // 
            // timerChart
            // 
            this.timerChart.Interval = 1000;
            this.timerChart.Tick += new System.EventHandler(this.timerChart_Tick);
            // 
            // PortfolioView
            // 
            this.AcceptButton = this.btnAddTrade;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(952, 404);
            this.Controls.Add(this.splitContainerMaster);
            this.Name = "PortfolioView";
            this.Text = "Portfolio Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PortfolioView_FormClosing);
            this.splitContainerMaster.Panel1.ResumeLayout(false);
            this.splitContainerMaster.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMaster)).EndInit();
            this.splitContainerMaster.ResumeLayout(false);
            this.splitContainerLeft.Panel1.ResumeLayout(false);
            this.splitContainerLeft.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLeft)).EndInit();
            this.splitContainerLeft.ResumeLayout(false);
            this.panelAction.ResumeLayout(false);
            this.panelAction.PerformLayout();
            this.panelSummary.ResumeLayout(false);
            this.panelSummary.PerformLayout();
            this.splitContainerRight.Panel1.ResumeLayout(false);
            this.splitContainerRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRight)).EndInit();
            this.splitContainerRight.ResumeLayout(false);
            this.panelGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPortfolio)).EndInit();
            this.panelChart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartPortfolio)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMaster;
        private System.Windows.Forms.SplitContainer splitContainerLeft;
        private System.Windows.Forms.Panel panelAction;
        private System.Windows.Forms.Button btnAddTrade;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.TextBox txtShares;
        private System.Windows.Forms.Label labelPrice;
        private System.Windows.Forms.Label labelShares;
        private System.Windows.Forms.TextBox txtSymbol;
        private System.Windows.Forms.Label labelSymbol;
        private System.Windows.Forms.Panel panelSummary;
        private System.Windows.Forms.Label lblUnRlzPLValue;
        private System.Windows.Forms.Label labelUnRlzPL;
        private System.Windows.Forms.Label lblPortfolioCost;
        private System.Windows.Forms.Label labelPortfolioCost;
        private System.Windows.Forms.SplitContainer splitContainerRight;
        private System.Windows.Forms.Panel panelGrid;
        private System.Windows.Forms.DataGridView dataGridViewPortfolio;
        private System.Windows.Forms.Panel panelChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartPortfolio;
        private System.Windows.Forms.Timer timerChart;

    }
}

