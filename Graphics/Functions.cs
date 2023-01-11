using System.Numerics;

namespace MathAnim.Graphics
{
    static class Functions
    {
        delegate double UnivariateFunction(double x);
        delegate Vector<double> ParametricFunction(double s);
        delegate Vector<double> VectorField(Vector<short> v);
        delegate UnivariateFunction FunctionGraph(short s);
    }
}
