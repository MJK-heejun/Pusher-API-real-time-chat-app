﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using System.Configuration;
//using Microsoft.Practices.Unity;
using System.IO;
using System.ServiceModel.Activation;
//using Unity.Web;

namespace PusherTest2
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteTable.Routes.Add(new ServiceRoute("main", new WebServiceHostFactory(), typeof(MyChat.Main)));
        }

    }




}