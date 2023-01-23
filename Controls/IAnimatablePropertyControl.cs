namespace MathAnim.Controls;

internal interface IAnimatablePropertyControl 
{
    bool IsAnimatable { get; }
}

internal interface IAnimatablePropertyControl<T>
{
    T Value { get; set; }
}