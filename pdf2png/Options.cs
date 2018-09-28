using Ghostscript.NET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdf2png
{
    public class Options
    {
        public string Input { get; set; }

        public string OutputPath => Path.GetDirectoryName(Input); //TODO

        public string DestFileNameWithoutExtension => Path.GetFileNameWithoutExtension(Input); //TODO

        public int Desired_Y_dpi { get; set; } = 96;

        public int Desired_X_dpi { get; set; } = 96;

        public GhostscriptVersionInfo Gvi { get; } = new GhostscriptVersionInfo(Path.GetFullPath(@"..\..\..\..\3rdParty\gsdll32.dll"));
    }
}
