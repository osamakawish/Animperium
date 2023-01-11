using MathAnim.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathAnim.FileData
{
    static class CurrentValues
    {
        static MathAnimFile? CurrentFile { get; set; }
        static IGraphicsObject? CurrentObject { get; set; }
    }
}
