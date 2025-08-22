using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPB1SFIntegration.Model
{
    public class ProductEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ProductCode { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
