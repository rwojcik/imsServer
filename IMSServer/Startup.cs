﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(IMSServer.Startup))]

namespace IMSServer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
