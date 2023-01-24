﻿using System.Windows.Media;

namespace MathAnim.ColorThemes;

public record ArgbColor(byte R, byte G, byte B, byte A = 255)
{
    public Color Color => Color.FromArgb(A, R, G, B);

    public static implicit operator Brush(ArgbColor color) => new SolidColorBrush(color.Color);
    public static implicit operator ArgbColor(Color color) => new(color.A, color.R, color.G, color.B);
}

public record PenStrokeData(ArgbColor Color, double Thickness);