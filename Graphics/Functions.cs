using System.Numerics;

namespace MathAnim.Graphics
{
    internal static class Functions
    {
        private delegate double UnivariateFunction(double x);

        private delegate Vector<double> ParametricFunction(double s);

        private delegate Vector<double> VectorField(Vector<short> v);

        private delegate UnivariateFunction FunctionGraph(short s);
    }
}
