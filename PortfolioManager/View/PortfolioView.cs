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

namespace PortfolioManager
{
    public partial class PortfolioView : Form, IPortfolioView
    {
        private ILogger _logger = new LoggingService(typeof(PortfolioView)); 

        private PortfolioPresenter _presenter;

        private BindingSourceEx _bindingSourcePortfolio;
        private BindingSourceEx _bindingSourcePortfolioAggregate;

        public PortfolioView()
        {
            InitializeComponent();

            _bindingSourcePortfolio = new BindingSourceEx(this);

            _bindingSourcePortfolioAggregate = new BindingSourceEx(this);

        }

        public PortfolioView(PortfolioDao dao)
            : this()
        {
            _presenter = new PortfolioPresenter(this, dao);
        }

        public PortfolioView(IPortfolioDao dao, IMarketData mktDataAdapter)
            :this()
        {
            _presenter = new PortfolioPresenter(this, dao);
            _presenter.MarketDataAdapter = mktDataAdapter;
        }

        public void ShowPortfolioItems(BindingList<PortfolioDataEntity> PortfolioItemsList)
        {
            _bindingSourcePortfolio.DataSource = PortfolioItemsList;
            dataGridViewPortfolio.DataSource = _bindingSourcePortfolio;
        }

        public void ShowPortfolioAggregate(PortfolioAggregate portfolioAggregate)
        {
            _bindingSourcePortfolioAggregate.DataSource = typeof(PortfolioAggregate);
            _bindingSourcePortfolioAggregate.DataSource = portfolioAggregate;

            lblPortfolioCost.DataBindings.Add("Text", _bindingSourcePortfolioAggregate, "CostAggregate", false, DataSourceUpdateMode.OnPropertyChanged);
            lblUnRlzPLValue.DataBindings.Add("Text", _bindingSourcePortfolioAggregate, "UnrealizedGainAggregate", false, DataSourceUpdateMode.OnPropertyChanged);

            tmrAggregate.Enabled = true;
            //bindingSourcePortfolioAggregate.ResetBindings(false); // might not be needed
        }


        public void UpdatePortfolioItems()
        {
            _bindingSourcePortfolio.ResetBindings(false);
        }

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
            tmrAggregate.Enabled = false;
            _presenter.Close();
        }

        private void tmrAggregate_Tick(object sender, EventArgs e)
        {
            // Workaround as the Property Change is not working
            _bindingSourcePortfolioAggregate.ResetBindings(false);
        }


        private void txtSymbol_Validating(object sender, CancelEventArgs e)
        {

            int maxLength = 8;

            if (txtSymbol.Text.Length > maxLength)
            {
                MessageBox.Show("Please enter correct symbol.");
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
                    MessageBox.Show("Please enter correct shares.");
                    txtShares.Text = "0";
                }
            }
            else
            {
                MessageBox.Show("Please enter correct shares");
                txtShares.Text = "0";
            }

        }

        private void txtPrice_Validating(object sender, CancelEventArgs e)
        {

            double numberEntered;

            if (double.TryParse(txtPrice.Text, out numberEntered))
            {
                if (numberEntered < 1 || numberEntered > 10000)
                {
                    MessageBox.Show("Please enter correct price.");
                    txtPrice.Text = "0";
                }
            }
            else
            {
                MessageBox.Show("Please enter correct price");
                txtPrice.Text = "0";
            }

        }


    }
}
