# SAPB1SFIntegration

This project cover an example of how integrate SAP B1 with salesforce.  

Below is the step-by-step on how to generate the token to use in the authentication process.  

## SSL Certificate    
How to generate SSL certificate to use in salesforce  

○ openssl genrsa -des3 -passout pass:yourpassword -out server.pass.key 2048  
○ openssl rsa -passin pass:yourpassword -in server.pass.key -out server.key  
○ openssl req -new -key server.key -out server.csr  
○ Country Name (2 letter code) [AU]:BR  
○ State or Province Name (full name) [Some-State]:SC  
○ Locality Name (eg, city) []:Blumenau  
○ Organization Name (eg, company) [Internet Widgits Pty Ltd]:CompanyName  
○ Organizational Unit Name (eg, section) []:Company  
○ Common Name (e.g. server FQDN or YOUR name) []:John  
○ Email Address []:your@email.com  
○ openssl x509 -req -sha256 -days 1095 -in server.csr -signkey server.key -out server.crt  


## SalesForce Token  
How to generate the token in SalesForce  

○ generate the SSL certificate (described above)  
○ generate jwt token  
○ acess https://jwt.io/  
○ parameters:  
    ○ algoritmo: RS256  
    ○ header: { "alg": "RS256" }  
    ○ payload:   
        {  
          "iss": [client key in salesforce],  
          "sub": [user name in salesforce],  
          "aud": "https://login.salesforce.com" ou "https://test.salesforce.com",  
          "exp": [get timestamp in https://www.unixtimestamp.com/. It must be the same expiration date of the certificate used]  
        }  
        ○ signature: text in server.crt + text in server.key  
        ○ before first use, the cliente id must be authenticated, to do this paste the following URL in browser (change client id): https://test.salesforce.com/services/oauth2/authorize?client_id=YOURCLIENTEID&redirect_uri=https://login.salesforce/com/oauth2/callback&response_type=cod  
