using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Serialization;

namespace SAPB1SFIntegration.Model
{
    public class PricebookEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public static IList<JsonProperty> PatchProps(Type type, IList<JsonProperty> allProps)
        {
            if (type == typeof(PricebookEntity))
            {
                List<string> props = new List<string>();
                props.Add("Name");

                return allProps.Where(r => props.Contains(r.PropertyName)).ToList();
            }
            else
            {
                return allProps;
            }
        }
    }
}
