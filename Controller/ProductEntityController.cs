using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPB1SFIntegration.Controller
{
    public class ProductEntityController : BaseEntityController<Model.ProductEntity>
    {
        public ProductEntityController(AutenticacaoController autenticacaoControlller) : base(autenticacaoControlller) { }

        protected override string ResourceName { get { return "Product2"; } }
    }
}
