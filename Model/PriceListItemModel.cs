using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPB1SFIntegration.Model
{
    public class PriceListItemModel
    {
        public string PriceListName { get; set; }

        public int PriceListCode { get; set; }

        public string ItemCode { get; set; }

        public string IdSalesForce { get; set; }

        public string IdSalesForceList { get; set; }

        public string IdSalesForceProduct { get; set; }        

        public double Price { get; set; }
    }
}
