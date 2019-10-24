using NetTopologySuite.Geometries;

namespace GasWeb.Domain
{
    internal static class SharedMappings
    {
        public static Point ToPoint(this Shared.Location location)
        {
            return new Point(location.Longitude, location.Latitude);
        }
    }
}
