using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortfolioManager.Common
{
    public interface IExceptionHandler
    {
        void HandleException(Exception ex);
    }
}
