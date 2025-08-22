using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp.Authenticators;

namespace SAPB1SFIntegration.Controller
{
    public class AuthenticationController
    {        
        //check README file to figure out how to fill this

        string URL_TOKEN = "https://login.salesforce.com/services/oauth2/token";
        string URL_INTROSPECT = "https://companyname.my.salesforce.com/services/oauth2/introspect";

        string GRANT_TYPE = "urn:ietf:params:oauth:grant-type:jwt-bearer";

        string JWT_TOKEN = "jwttokenhere";

        string CLIENT_ID = "clientidhere";
        string CLIENT_SECRET = "clientsecrethere";

        public string BearerToken { get; private set; }

        public AuthenticationController()
        {
            BearerToken = string.Empty;

            bool sandBox = false; //sugestion: save in a user table (e.g @SFINTEGRATION) in SAP B1 a parameter to flag if it is in sandbox mode and retrieve here.

            if (sandBox)
            {
                URL_TOKEN = "https://test.salesforce.com/services/oauth2/token";
                URL_INTROSPECT = "https://companyname--prod1.sandbox.my.salesforce.com/services/oauth2/introspect";
                JWT_TOKEN = "sandboxjwttokenhere";
                CLIENT_ID = "sandboxclientidhere";
                CLIENT_SECRET = "sandboxclientsecrethere";
            }
        }

        public bool Authenticate()
        {
            if (BearerToken == string.Empty)
            {
                string persistedBearerToken = ""; //sugestion: save in a user table (e.g @SFINTEGRATION) in SAP B1 the bearer token and retrieve here.
                try
                {
                    var options = new RestClientOptions(URL_INTROSPECT);
                    options.Authenticator = new HttpBasicAuthenticator(CLIENT_ID, CLIENT_SECRET);

                    var client = new RestClient(options);

                    var request = new RestRequest("", Method.Post);
                    
                    request.AddParameter("token", persistedBearerToken, ParameterType.GetOrPost);
                    request.AddParameter("token_type_hint", "access_token", ParameterType.GetOrPost);
                    var response = client.Execute(request);

                    var introspectEntity = JsonConvert.DeserializeObject<Model.IntrospectEntity>(response.Content);

                    if (introspectEntity.active)
                    {
                        BearerToken = persistedBearerToken;
                    }
                    else
                    {
                        client = new RestClient(URL_TOKEN);
                        request = new RestRequest("", Method.Post);
                        request.AddParameter("grant_type", GRANT_TYPE, ParameterType.GetOrPost);
                        request.AddParameter("assertion", JWT_TOKEN, ParameterType.GetOrPost);
                        response = client.Execute(request);

                        var authEntity = JsonConvert.DeserializeObject<Model.AuthEntity>(response.Content);

                        BearerToken = authEntity.access_token;
                        
                        //sugestion: save in a user table (e.g @SFINTEGRATION) in SAP B1 the bearer token
                        persistedBearerToken = BearerToken;
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    //sugestion: save in a user table (e.g @SFINTEGRATION) in SAP B1 the error to retrieve an alert that the integration is offline and the message

                    return false;
                }
            }
            else return true;
        }
    }
}
