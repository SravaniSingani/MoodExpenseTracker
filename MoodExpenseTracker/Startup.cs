using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MoodExpenseTracker.Startup))]
namespace MoodExpenseTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
