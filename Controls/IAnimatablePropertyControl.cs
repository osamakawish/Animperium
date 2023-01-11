namespace MathAnim.Controls
{
    interface IAnimatablePropertyControl 
    {
        bool IsAnimatable { get; }
    }

    interface IAnimatablePropertyControl<T>
    {
        T Value { get; set; }
    }
}
