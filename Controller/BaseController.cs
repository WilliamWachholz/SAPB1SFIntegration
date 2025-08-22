using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPB1SFIntegration.Controller
{
    public abstract class BaseController
    {
        protected AuthenticationController authenticationController = new AuthenticationController();

        public BaseController(AuthenticationController authenticationController)
        {
            this.authenticationController = authenticationController;
        }

        public abstract void Execute();

        protected void SaveLog(string msg, string json, bool error)
        {
           //sugestion: save in a user table (e.g @SFINTHISTORY) the results of each execution
        }
    }
}
