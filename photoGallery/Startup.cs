using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(photoGallery.Startup))]
namespace photoGallery
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
