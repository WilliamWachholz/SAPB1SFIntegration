using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace SAPB1SFIntegration.Model
{
    public class PricebookEntryEntity
    {
        public string Pricebook2Id { get; set; }

        public string Product2Id { get; set; }

        public double UnitPrice { get; set; }

        public bool IsActive { get; set; }

        public static IList<JsonProperty> PatchProps(Type type, IList<JsonProperty> allProps)
        {
            if (type == typeof(PricebookEntryEntity))
            {
                List<string> props = new List<string>();
                props.Add("UnitPrice");
                props.Add("IsActive");

                return allProps.Where(r => props.Contains(r.PropertyName)).ToList();
            }
            else
            {
                return allProps;
            }
        }
    }
}
