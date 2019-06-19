using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace RD.CodeGenerator.Model
{
    public class TileItemModel
    {
        public string Name { get; set; }

        public ImageSource Icon { get; set; }

        public string Description { get; set; }

        public string Color { get; set; }

        public string Header { get; set; }

        public ImageSource SlideImage { get; set; }

        public bool CanSlide { get; set; }
    }
}
