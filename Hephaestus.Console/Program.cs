using System.Threading.Tasks;
using Hephaestus.Core.Application;

namespace Hephaestus.Console
{
    class Program
    {

        static async Task Main(string[] args)
        {
            var model = new MainModel(new Application());
            var controller = new MainController(model);
            var view = new MainView(model, controller);
            view.Title();
        }


        //Run("Mercury", "C:\\source\\Mercury");
        //RebuildCache("Mercury", "C:\\source\\Mercury");



    }


}
