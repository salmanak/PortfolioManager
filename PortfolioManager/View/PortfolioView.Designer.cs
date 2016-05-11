namespace PortfolioManager
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
            this.panelGrid = new System.Windows.Forms.Panel();
            this.dataGridViewPortfolio = new System.Windows.Forms.DataGridView();
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
            this.lblMktValue = new System.Windows.Forms.Label();
            this.labelMktValue = new System.Windows.Forms.Label();
            this.labelAvCashValue = new System.Windows.Forms.Label();
            this.labelAvCash = new System.Windows.Forms.Label();
            this.tmrAggregate = new System.Windows.Forms.Timer(this.components);
            this.panelGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPortfolio)).BeginInit();
            this.panelAction.SuspendLayout();
            this.panelSummary.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelGrid
            // 
            this.panelGrid.Controls.Add(this.dataGridViewPortfolio);
            this.panelGrid.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelGrid.Location = new System.Drawing.Point(167, 0);
            this.panelGrid.Name = "panelGrid";
            this.panelGrid.Size = new System.Drawing.Size(804, 271);
            this.panelGrid.TabIndex = 0;
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
            this.dataGridViewPortfolio.Size = new System.Drawing.Size(804, 271);
            this.dataGridViewPortfolio.TabIndex = 0;
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
            this.panelAction.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelAction.Location = new System.Drawing.Point(0, 0);
            this.panelAction.Name = "panelAction";
            this.panelAction.Size = new System.Drawing.Size(167, 140);
            this.panelAction.TabIndex = 1;
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
            // 
            // txtShares
            // 
            this.txtShares.Location = new System.Drawing.Point(53, 40);
            this.txtShares.Name = "txtShares";
            this.txtShares.Size = new System.Drawing.Size(100, 20);
            this.txtShares.TabIndex = 6;
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
            this.panelSummary.Controls.Add(this.lblMktValue);
            this.panelSummary.Controls.Add(this.labelMktValue);
            this.panelSummary.Controls.Add(this.labelAvCashValue);
            this.panelSummary.Controls.Add(this.labelAvCash);
            this.panelSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSummary.Location = new System.Drawing.Point(0, 140);
            this.panelSummary.Name = "panelSummary";
            this.panelSummary.Size = new System.Drawing.Size(167, 131);
            this.panelSummary.TabIndex = 2;
            // 
            // lblUnRlzPLValue
            // 
            this.lblUnRlzPLValue.AutoSize = true;
            this.lblUnRlzPLValue.Location = new System.Drawing.Point(15, 89);
            this.lblUnRlzPLValue.Name = "lblUnRlzPLValue";
            this.lblUnRlzPLValue.Size = new System.Drawing.Size(22, 13);
            this.lblUnRlzPLValue.TabIndex = 5;
            this.lblUnRlzPLValue.Text = "0.0";
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
            // lblMktValue
            // 
            this.lblMktValue.AutoSize = true;
            this.lblMktValue.Location = new System.Drawing.Point(15, 59);
            this.lblMktValue.Name = "lblMktValue";
            this.lblMktValue.Size = new System.Drawing.Size(22, 13);
            this.lblMktValue.TabIndex = 3;
            this.lblMktValue.Text = "0.0";
            // 
            // labelMktValue
            // 
            this.labelMktValue.AutoSize = true;
            this.labelMktValue.Location = new System.Drawing.Point(13, 46);
            this.labelMktValue.Name = "labelMktValue";
            this.labelMktValue.Size = new System.Drawing.Size(85, 13);
            this.labelMktValue.TabIndex = 2;
            this.labelMktValue.Text = "MarketValue ($):";
            // 
            // labelAvCashValue
            // 
            this.labelAvCashValue.AutoSize = true;
            this.labelAvCashValue.Location = new System.Drawing.Point(15, 29);
            this.labelAvCashValue.Name = "labelAvCashValue";
            this.labelAvCashValue.Size = new System.Drawing.Size(22, 13);
            this.labelAvCashValue.TabIndex = 1;
            this.labelAvCashValue.Text = "0.0";
            // 
            // labelAvCash
            // 
            this.labelAvCash.AutoSize = true;
            this.labelAvCash.Location = new System.Drawing.Point(12, 12);
            this.labelAvCash.Name = "labelAvCash";
            this.labelAvCash.Size = new System.Drawing.Size(83, 13);
            this.labelAvCash.TabIndex = 0;
            this.labelAvCash.Text = "Available Cash :";
            // 
            // tmrAggregate
            // 
            this.tmrAggregate.Interval = 700;
            this.tmrAggregate.Tick += new System.EventHandler(this.tmrAggregate_Tick);
            // 
            // PortfolioView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(971, 271);
            this.Controls.Add(this.panelSummary);
            this.Controls.Add(this.panelAction);
            this.Controls.Add(this.panelGrid);
            this.Name = "PortfolioView";
            this.Text = "Portfolio Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PortfolioView_FormClosing);
            this.panelGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPortfolio)).EndInit();
            this.panelAction.ResumeLayout(false);
            this.panelAction.PerformLayout();
            this.panelSummary.ResumeLayout(false);
            this.panelSummary.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelGrid;
        private System.Windows.Forms.DataGridView dataGridViewPortfolio;
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
        private System.Windows.Forms.Label lblMktValue;
        private System.Windows.Forms.Label labelMktValue;
        private System.Windows.Forms.Label labelAvCashValue;
        private System.Windows.Forms.Label labelAvCash;
        private System.Windows.Forms.Timer tmrAggregate;
    }
}

