using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PortfolioManager.View;
using System.Windows.Forms.DataVisualization.Charting;
using PortfolioManager.Common.Interfaces.Model;
using PortfolioManager.Common;
using PortfolioManager.Common.Interfaces.View;
using PortfolioManager.Common.Interfaces.Presenter;

namespace PortfolioManager.View
{
    public partial class PortfolioView : Form, IPortfolioView, IExceptionHandler
    {
        #region Declarations and Definitions
        /// <summary>
        /// For Logging
        /// </summary>
        private ILogger _logger = new LoggingService(typeof(PortfolioView));
        /// <summary>
        /// Reference to the Presenter
        /// </summary>
        private IPortfolioPresenter _presenter;
        /// <summary>
        /// Binding Source for Portfolios
        /// </summary>
        private BindingSourceEx _bindingSourcePortfolio;
        /// <summary>
        /// Binding Source for Aggregates
        /// </summary>
        private BindingSourceEx _bindingSourcePortfolioAggregate; 
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public PortfolioView()
        {
            InitializeComponent();

            _bindingSourcePortfolio = new BindingSourceEx(this);

            _bindingSourcePortfolioAggregate = new BindingSourceEx(this);

        }

        /// <summary>
        /// constructor to receive presenter reference
        /// </summary>
        /// <param name="presenter"></param>
        public PortfolioView(IPortfolioPresenter presenter)
            : this()
        {
            _presenter = presenter;
        } 

        #endregion

        #region Methods for binding
        /// <summary>
        /// To bind with the portfolio list
        /// </summary>
        /// <param name="PortfolioItemsList">portfolio list to be bound with</param>
        public void ShowPortfolioItems(BindingList<IPortfolioDataEntity> PortfolioItemsList)
        {
            _bindingSourcePortfolio.DataSource = typeof(BindingList<IPortfolioDataEntity>);
            _bindingSourcePortfolio.DataSource = PortfolioItemsList;
            dataGridViewPortfolio.DataSource = _bindingSourcePortfolio;
            Utility.FormatGrid(dataGridViewPortfolio);
        }
        /// <summary>
        /// To bind with the aggregate object
        /// </summary>
        /// <param name="portfolioAggregate">Aggeregate object to be bound with</param>
        public void ShowPortfolioAggregate(IPortfolioAggregate portfolioAggregate)
        {
            _bindingSourcePortfolioAggregate.DataSource = typeof(IPortfolioAggregate);
            _bindingSourcePortfolioAggregate.DataSource = portfolioAggregate;

            //lblPortfolioCost.DataBindings.Add("Text", _bindingSourcePortfolioAggregate, "CostAggregate", false, DataSourceUpdateMode.OnPropertyChanged);
            //lblUnRlzPLValue.DataBindings.Add("Text", _bindingSourcePortfolioAggregate, "UnrealizedGainAggregate", false, DataSourceUpdateMode.OnPropertyChanged);


            lblPortfolioCost.DataBindings.Add("Text", _bindingSourcePortfolioAggregate, "CostAggregate", true, DataSourceUpdateMode.OnPropertyChanged, "", "$#,0.00;$-#,0.00");
            lblUnRlzPLValue.DataBindings.Add("Text", _bindingSourcePortfolioAggregate, "UnrealizedGainAggregate", true, DataSourceUpdateMode.OnPropertyChanged, "", "$#,0.00;$-#,0.00");

        }
        /// <summary>
        /// Reset the binding of grid
        /// </summary>
        public void UpdatePortfolioItems()
        {
            _bindingSourcePortfolio.ResetBindings(false);
        }
        #endregion

        #region Chart related methods
        /// <summary>
        /// Inintialize the chart with appropriate series information 
        /// </summary>
        private void InitChart()
        {
            chartPortfolio.Annotations.Clear();

            chartPortfolio.Series.Clear();

            var seriesPrice = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "ProfitLoss",
                //Color = System.Drawing.Color.Blue,
                IsVisibleInLegend = true,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Line,
                XValueType = ChartValueType.Time
            };

            chartPortfolio.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
            chartPortfolio.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;

            // show an X label every x Minute
            //chartPortfolio.ChartAreas[0].AxisX.Interval = 0.5;
            //chartPortfolio.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Seconds;
            chartPortfolio.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";

            this.chartPortfolio.Series.Add(seriesPrice);

            this.chartPortfolio.Legends[0].Docking = Docking.Bottom;
            
            chartPortfolio.Invalidate();

        }

        /// <summary>
        /// Force update the chart
        /// </summary>
        private void UpdateChart()
        {
            double dMin = 0;
            double dMax = 0;

            float fDiff = (float)(dMax) - (float)dMin;
            chartPortfolio.ChartAreas[0].AxisY.Minimum = Convert.ToDouble((float)dMin - ((5f / 100f) * (fDiff)));
            chartPortfolio.ChartAreas[0].AxisY.Maximum = Convert.ToDouble((float)dMax + ((5f / 100f) * (fDiff)));

            chartPortfolio.Invalidate();
        }
        #endregion


        #region Control and Form Events
        /// <summary>
        /// Update Presenter with the View once the form has been loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PortfolioView_Load(object sender, EventArgs e)
        {
            InitChart();
            _presenter.SetView(this);
        }

        /// <summary>
        /// Close the open components
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PortfolioView_FormClosing(object sender, FormClosingEventArgs e)
        {
            timerChart.Enabled = false;
            _presenter.Close();
        }

        private void btnAddTrade_Click(object sender, EventArgs e)
        {
            try
            {
                _logger.Log("Adding Trade : " + txtSymbol.Text + " : " + txtShares.Text + " : " + " : " + txtPrice.Text);

                _presenter.AddPortfolioClicked(
                            txtSymbol.Text.ToUpper(),
                            long.Parse(txtShares.Text),
                            double.Parse(txtPrice.Text));

                txtSymbol.Focus();
                txtSymbol.Text = "";
                txtShares.Text = "";
                txtPrice.Text = "";

            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to Add Trade.", ex);
                MessageBox.Show("Unable to Add Trade due to following error" + Environment.NewLine + ex.Message);
            }
        }

        private void txtSymbol_Validating(object sender, CancelEventArgs e)
        {

            int maxLength = 8;

            if (txtSymbol.Text.Length > maxLength)
            {
                MessageBox.Show("Please enter correct _symbol.");
                txtSymbol.Text = txtSymbol.Text.Substring(0, maxLength);
            }
        }

        private void txtShares_Validating(object sender, CancelEventArgs e)
        {
            long numberEntered;

            if (long.TryParse(txtShares.Text, out numberEntered))
            {
                if (numberEntered < 1 || numberEntered > 100000000)
                {
                    MessageBox.Show("Please enter correct _shares.");
                    txtShares.Text = "0";
                }
            }
            else
            {
                MessageBox.Show("Please enter correct _shares");
                txtShares.Text = "0";
            }

        }

        private void txtPrice_Validating(object sender, CancelEventArgs e)
        {

            double numberEntered = Utility.GetDouble(txtPrice.Text);

            if (numberEntered < 1 || numberEntered > 10000)
            {
                MessageBox.Show("Please enter correct _price.");
                txtPrice.Text = "0";
            }
            
        }

        private void lblUnRlzPLValue_TextChanged(object sender, EventArgs e)
        {
            this.lblUnRlzPLValue.TextChanged -= new System.EventHandler(this.lblUnRlzPLValue_TextChanged);

            timerChart.Enabled = true;
        }

        /// <summary>
        /// Update the chart using the value on the Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerChart_Tick(object sender, EventArgs e)
        {
            string value = lblUnRlzPLValue.Text;

            value = value.Replace("$", String.Empty).Replace(",",String.Empty);

            //TODO: See how we can use BindingSource to get conflated data
            chartPortfolio.Series[0].Points.AddXY(DateTime.Now, Utility.GetDouble(value));
        }
        #endregion


        #region Exception Handling
        private delegate void OnExceptionHandle(Exception ex);
        /// <summary>
        /// exception catcher which to be called by other components 
        /// when there is a non-GUI thread exception
        /// </summary>
        /// <param name="ex"></param>
        public void HandleException(Exception ex)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new OnExceptionHandle(HandleException), new object[] { ex });
            }
            else
            {
                MessageBox.Show(this, ex.Message, "ERROR");
            }
        }
        #endregion


    }
}
