using StructureHelper.Infrastructure;
using StructureHelperCommon.Infrastructures.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace StructureHelper.Windows.ViewModels.Help
{
    internal class AboutViewModel : OkCancelViewModelBase
    {
        public string Authors => "Redikultsev Evgeny, Redikultseva Svetlana";
        public string Assembly
        {
            get
            {
                string version;
                version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                return version;
            }
        }

        private string version1;

        public string Version
        {
            get
            {
                var fileVersion = ProgramSetting.GetCurrentFileVersion();
                return $"{fileVersion.VersionNumber}.{fileVersion.SubVersionNumber}";
            }
        }
    }
}
