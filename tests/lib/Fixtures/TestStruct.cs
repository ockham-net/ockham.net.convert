using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;



namespace Ockham.Data.Tests.Fixtures
{
    [ExcludeFromCodeCoverage]
    public struct TestStruct
    {
        public string StringField;
        public int IntProp { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public struct Point3D
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        public Point3D(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }
}
