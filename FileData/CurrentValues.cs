using System.Windows.Shapes;
using Animperium.Graphics;

namespace Animperium.FileData;

internal static class CurrentValues
{
    internal static AppFile? CurrentFile { get; set; }
    internal static GraphicsObjectTree? CurrentTree => CurrentFile?.GraphicsObjectTree;
    internal static Shape? CurrentItem
    {
        get => CurrentFile?.SelectedItem;
        set { if (CurrentFile is not null) CurrentFile.SelectedItem = value; }
    }
}