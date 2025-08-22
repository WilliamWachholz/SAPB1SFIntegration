using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPB1SFIntegration.Controller
{
    public class PricebookEntityController : BaseEntityController<Model.PricebookEntity>
    {
        public PricebookEntityController(AutenticacaoController autenticacaoControlller) : base(autenticacaoControlller) { }

        protected override string ResourceName { get { return "Pricebook2"; } }
    }
}
