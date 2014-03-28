using Microsoft.AspNet.SignalR;
using Microsoft.WindowsAzure;
using Owin;
namespace TresEnRaya
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalHost.DependencyResolver.UseRedis("127.0.0.1", 6379, "password1", "demo");

            app.MapSignalR();
        }
    }
} 
