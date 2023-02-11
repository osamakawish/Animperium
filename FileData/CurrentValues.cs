using Animperium.Graphics;

namespace Animperium.FileData;

internal static class CurrentValues
{
    internal static AppFile? CurrentFile { get; set; }
    internal static GraphicsObjectTree? CurrentTree => CurrentFile?.GraphicsObjectTree;
    internal static IGraphicsObject? CurrentObject
    {
        get => CurrentFile?.SelectedObject;
        set { if (CurrentFile is not null) CurrentFile.SelectedObject = value; }
    }
}