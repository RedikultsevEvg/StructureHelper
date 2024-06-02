using StructureHelper.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace StructureHelper.Windows.ViewModels.Help
{
    internal class AboutViewModel : OkCancelViewModelBase
    {
        public string Authors => "Redikultsev Evgeny, Petrov Sergey, Smirnov Nikolay";
        public string Version
        {
            get
            {
                string version;
                version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                return version;
            }
        }
    }
}
