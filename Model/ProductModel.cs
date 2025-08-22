using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPB1SFIntegration.Model
{
    public class ProdutoModel
    {
        public int Operation { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string IdSalesForce { get; set; }

        public string Active { get; set; }
    }
}
