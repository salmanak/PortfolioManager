using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace PortfolioManager.Common
{
    public class Utility
    {
        public static double GetDouble(string valueString)
        {
            double valueDouble = 0;
            double.TryParse(valueString, out valueDouble);
            return valueDouble;
        }

        public static void FormatGrid(DataGridView grid)
        {
            foreach (DataGridViewColumn col in grid.Columns)
            {
                if (col.ValueType == typeof(Int64))
                {
                    col.DefaultCellStyle.Format = "#,0;-#,0";
                    //col.DefaultCellStyle.Format = "#,0;(#,0)";
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                else if (col.ValueType == typeof(Double))
                {
                    col.DefaultCellStyle.Format = "#,0.00;-#,0.00";
                    //col.DefaultCellStyle.Format = "#,0.00;(#,0.00)";
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

            }
        }
    }
}
