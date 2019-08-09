namespace Ockham.Data.Tests.Fixtures
{
    public class CelestialBody { }
    public class Star : CelestialBody { }
    public class NeutronStar : Star
    {
        public override string ToString() => "I'm a neutron star";
    }
}
