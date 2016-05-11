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

namespace PortfolioManager
{
    public partial class PortfolioView : Form, IPortfolioView
    {
        private PortfolioPresenter _presenter;

        private BindingSourceEx bindingSourcePortfolio;

        private BindingSourceEx bindingSourcePortfolioAggregate;
        
        public PortfolioView()
        {
            InitializeComponent();

            bindingSourcePortfolio = new BindingSourceEx(this);

            bindingSourcePortfolioAggregate = new BindingSourceEx(this);

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
            bindingSourcePortfolio.DataSource = PortfolioItemsList;
            dataGridViewPortfolio.DataSource = bindingSourcePortfolio;
        }

        public void ShowPortfolioAggregate(PortfolioAggregate portfolioAggregate)
        {
            bindingSourcePortfolioAggregate.DataSource = typeof(PortfolioAggregate);
            bindingSourcePortfolioAggregate.DataSource = portfolioAggregate;

            lblMktValue.DataBindings.Add("Text", bindingSourcePortfolioAggregate, "CostAggregate", false, DataSourceUpdateMode.OnPropertyChanged);
            lblUnRlzPLValue.DataBindings.Add("Text", bindingSourcePortfolioAggregate, "UnrealizedGainAggregate", false, DataSourceUpdateMode.OnPropertyChanged);

            tmrAggregate.Enabled = true;
            //bindingSourcePortfolioAggregate.ResetBindings(false); // might not be needed
        }


        public void UpdatePortfolioItems()
        {
            bindingSourcePortfolio.ResetBindings(false);
        }

        private void btnAddTrade_Click(object sender, EventArgs e)
        {
            try
            {
                if (_presenter.AddPortfolioClicked(
                                                txtSymbol.Text,
                                                long.Parse(txtShares.Text),
                                                double.Parse(txtPrice.Text)) < 0)
                {
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
            bindingSourcePortfolioAggregate.ResetBindings(false);
        }
    }
}
