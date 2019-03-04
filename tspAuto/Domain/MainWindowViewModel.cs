using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tspAuto.Domain
{
    class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            PanelItems = new[]
                {
                    new PanelItem("Ana Sayfa", new Home(), "HomeVariant"),
                    new PanelItem("Taslak", new taslak(), "Worker")
                };
        }

        public PanelItem[] PanelItems { get; }
    }
}
