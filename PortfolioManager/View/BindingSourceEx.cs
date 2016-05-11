using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace PortfolioManager.View
{
    /// <summary>
    /// Wrapper class over the BindingSource to provide thread safety
    /// </summary>
    class BindingSourceEx:BindingSource
    {
        private Control _ctl;

        public BindingSourceEx(Control ctl):base()
        {
            _ctl = ctl;
        }

        private delegate void OnListChangedDelegate(ListChangedEventArgs e);
        protected override void OnListChanged(ListChangedEventArgs e)
        {
            if (_ctl != null && _ctl.InvokeRequired)
                _ctl.BeginInvoke(new OnListChangedDelegate(OnListChanged), new object[] { e });
            else
                base.OnListChanged(e);
        }

        /*
        private delegate void IndexListChangedDelegate(object sender, ListChangedEventArgs e);

        protected override void IndexListChanged(object sender, ListChangedEventArgs e)
        {
            if (_ctl != null && _ctl.InvokeRequired)
            {
                _ctl.Invoke(new IndexListChangedDelegate(IndexListChanged), new object[] { sender, e });
            }
            else
                base.IndexListChanged(sender, e);
        }
         */ 
    }
}
