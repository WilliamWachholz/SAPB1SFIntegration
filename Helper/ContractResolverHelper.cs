using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SAPB1SFIntegration.Helper
{
   
    public class PatchListPrice : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var allProps = base.CreateProperties(type, memberSerialization);

            allProps = Model.PricebookEntity.PatchProps(type, allProps);

            return allProps;
        }
    }

    public class PatchListPriceItem : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var allProps = base.CreateProperties(type, memberSerialization);

            allProps = Model.PricebookEntryEntity.PatchProps(type, allProps);

            return allProps;
        }
    }
}
