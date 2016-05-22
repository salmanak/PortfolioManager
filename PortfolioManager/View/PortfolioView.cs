using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PortfolioManager.MarketData;
using PortfolioManager.View;
using PortfolioManager.Common;
using PortfolioManager.Data;

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
        private PortfolioPresenter _presenter;
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
        /// Constructor with Model injection
        /// </summary>
        /// <param name="dao">Model</param>
        public PortfolioView(PortfolioDao dao)
            : this()
        {
            _presenter = new PortfolioPresenter(this, dao);
        }

        /// <summary>
        /// Constructor with Model and Market Data injection
        /// </summary>
        /// <param name="dao">Model</param>
        /// <param name="mktDataAdapter">Market Data</param>
        public PortfolioView(IPortfolioDao dao)//, IMarketDataAdapter<MarketDataEntity> mktDataAdapter)
            : this()
        {
            _presenter = new PortfolioPresenter(this, dao);
            //_presenter.MarketDataAdapter = mktDataAdapter;
        } 
        #endregion

        #region Methods for binding
        /// <summary>
        /// To bind with the portfolio list
        /// </summary>
        /// <param name="PortfolioItemsList">portfolio list to be bound with</param>
        public void ShowPortfolioItems(BindingList<PortfolioDataEntity> PortfolioItemsList)
        {
            _bindingSourcePortfolio.DataSource = PortfolioItemsList;
            dataGridViewPortfolio.DataSource = _bindingSourcePortfolio;
        }

        /// <summary>
        /// To bind with the aggregate object
        /// </summary>
        /// <param name="portfolioAggregate">Aggeregate object to be bound with</param>
        public void ShowPortfolioAggregate(PortfolioAggregate portfolioAggregate)
        {
            _bindingSourcePortfolioAggregate.DataSource = typeof(PortfolioAggregate);
            _bindingSourcePortfolioAggregate.DataSource = portfolioAggregate;

            lblPortfolioCost.DataBindings.Add("Text", _bindingSourcePortfolioAggregate, "CostAggregate", false, DataSourceUpdateMode.OnPropertyChanged);
            lblUnRlzPLValue.DataBindings.Add("Text", _bindingSourcePortfolioAggregate, "UnrealizedGainAggregate", false, DataSourceUpdateMode.OnPropertyChanged);

        }
        /// <summary>
        /// Reset the binding of grid
        /// </summary>
        public void UpdatePortfolioItems()
        {
            _bindingSourcePortfolio.ResetBindings(false);
        }

        #endregion
        
        #region Control and Form Events
        private void btnAddTrade_Click(object sender, EventArgs e)
        {
            try
            {
                _logger.Log("Adding Trade : " + txtSymbol.Text + " : " + txtShares.Text + " : " + " : " + txtPrice.Text);

                int result = _presenter.AddPortfolioClicked(
                                                txtSymbol.Text.ToUpper(),
                                                long.Parse(txtShares.Text),
                                                double.Parse(txtPrice.Text));
                // TODO: generalize return codes
                if (result == -2)
                {
                    _logger.LogError("Unable to add trade in database");
                    MessageBox.Show("Unable to add trade in database");
                }
                else if (result < 0)
                {
                    _logger.LogError("Unable to Add Trade.");
                    MessageBox.Show("Unable to Add Trade.");
                }
                else
                {
                    txtSymbol.Text = "";
                    txtShares.Text = "";
                    txtPrice.Text = "";
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to Add Trade.", ex);
                MessageBox.Show("Unable to Add Trade due to following error" + Environment.NewLine + ex.Message);
            }
        }

        private void PortfolioView_FormClosing(object sender, FormClosingEventArgs e)
        {
            _presenter.Close();
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
        #endregion



        private delegate void OnExceptionHandle(Exception ex);
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
    }
}
