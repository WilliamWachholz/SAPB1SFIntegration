using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPB1SFIntegration.Controller
{
    public class PricebookEntryEntityController : BaseEntityController<Model.PricebookEntryEntity>
    {
        public PricebookEntryEntityController(AutenticacaoController autenticacaoControlller) : base(autenticacaoControlller) { }

        protected override string ResourceName { get { return "PricebookEntry"; } }
    }
}
