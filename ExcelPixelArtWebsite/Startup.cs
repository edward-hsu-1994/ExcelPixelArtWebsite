using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ExcelPixelArtWebsite.Startup))]
namespace ExcelPixelArtWebsite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app){
        }
    }
}
