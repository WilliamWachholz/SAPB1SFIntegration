using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Authenticators;
using RestSharp.Authenticators.OAuth2;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SAPB1SFIntegration.Controller
{
    public abstract class BaseEntityController<T>
    {
        string URL_BASE = "https://companyname.my.salesforce.com/services/data/v57.0/sobjects";

        string URL_QUERY = "https://companyname.my.salesforce.com/services/data/v57.0/query/?q=";

        protected AuthenticationController authenticationController = new AuthenticationController();

        public BaseEntityController(AuthenticationController authenticationController)
        {
            this.authenticationController = authenticationController;

            bool sandBox = false; //sugestion: save in a user table (e.g @SFINTEGRATION) in SAP B1 a parameter to flag if it is in sandbox mode and retrieve here.

            if (sandBox)
            {
                URL_BASE = "https://companyname--prod1.sandbox.my.salesforce.com/services/data/v57.0/sobjects";
                URL_QUERY = "https://companyname--prod1.sandbox.my.salesforce.com/services/data/v57.0/query/?q=";
            }
        }

        protected abstract string ResourceName { get; }

        public void Patch(T entity, string id, IContractResolver contractResolver = null)
        {
            string json = string.Empty;

            Patch(entity, id, ref json, contractResolver);
        }

        public void Patch(T entity, string id, ref string json, IContractResolver contractResolver = null)
        {
            var options = new RestClientOptions(string.Format("{0}/{1}/{2}", URL_BASE, ResourceName, id));
            options.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(autenticacaoController.BearerToken, "Bearer");

            var client = new RestClient(options);

            var request = new RestRequest("", Method.Patch);

            json = ConvertToJsonString(entity, contractResolver);

            request.AddParameter("application/json", json, ParameterType.RequestBody);

            var response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                List<Model.ErrorMessageEntity> errorMessageEntity = JsonConvert.DeserializeObject<List<Model.ErrorMessageEntity>>(response.Content);

                throw new Exception(string.Join(", ", errorMessageEntity.Select(r => r.message).ToArray()));
            }
        }

        public void Delete(T entity, string id)
        {
            var options = new RestClientOptions(string.Format("{0}/{1}/{2}", URL_BASE, ResourceName, id));
            options.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(autenticacaoController.BearerToken, "Bearer");

            var client = new RestClient(options);

            var request = new RestRequest("", Method.Delete);

            var response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                List<Model.ErrorMessageEntity> errorMessageEntity = JsonConvert.DeserializeObject<List<Model.ErrorMessageEntity>>(response.Content);

                throw new Exception(string.Join(", ", errorMessageEntity.Select(r => r.message).ToArray()));
            }
        }

        public void Post(T entity, IContractResolver contractResolver = null)
        {
            string json = string.Empty;

            Post(entity, ref json, contractResolver);
        }

        public string Post(T entity, ref string json, IContractResolver contractResolver = null)
        {
            var options = new RestClientOptions(string.Format("{0}/{1}", URL_BASE, ResourceName));
            options.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(autenticacaoController.BearerToken, "Bearer");

            var client = new RestClient(options);

            var request = new RestRequest("", Method.Post);

            json = ConvertToJsonString(entity, contractResolver);

            request.AddParameter("application/json", json, ParameterType.RequestBody);

            var response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                List<Model.ErrorMessageEntity> errorMessageEntity = JsonConvert.DeserializeObject<List<Model.ErrorMessageEntity>>(response.Content);

                throw new Exception(string.Join(", ", errorMessageEntity.Select(r => r.message).ToArray()));
            }
            else
            {
                Model.SuccessEntity successEntity = JsonConvert.DeserializeObject<Model.SuccessEntity>(response.Content);

                return successEntity.id;
            }
        }

        public List<T> GetQueryRecords(string query)
        {
            var options = new RestClientOptions(string.Format("{0}{1}", URL_QUERY, query));
            options.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(autenticacaoController.BearerToken, "Bearer");

            var client = new RestClient(options);

            var request = new RestRequest("", Method.Get);

            var response = client.Execute(request);

            if (!response.IsSuccessful)
                throw new Exception("Invalid query: " + query);

            var queryRecords = JsonConvert.DeserializeObject<Model.QueryRecordsEntity<T>>(response.Content);

            return queryRecords.records;
        }

        private string ConvertToJsonString(object obj, IContractResolver contractResolver = null)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = contractResolver == null ? new DefaultContractResolver() : contractResolver,
                DateFormatString = "yyyy-MM-dd"
            });
        }
    }
}
