using System.Windows.Media.Media3D;

namespace HqPocket.Wpf.Common;

public class Point3DEx
{
    public static Point3D Center(Point3D p1, Point3D p2)
    {
        return new Point3D((p1.X + p2.X) / 2.0, (p1.Y + p2.Y) / 2.0, (p1.Z + p2.Z) / 2.0);
    }
}