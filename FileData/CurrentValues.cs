using MathAnim.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathAnim.FileData
{
    internal static class CurrentValues
    {
        internal static MathAnimFile? CurrentFile { get; set; }
        internal static GraphicsObjectTree? CurrentTree => CurrentFile?.GraphicsObjectTree;
        internal static IGraphicsObject? CurrentObject
        {
            get => CurrentFile?.SelectedObject;
            set { if (CurrentFile is not null) CurrentFile.SelectedObject = value; }
        }
    }
}
