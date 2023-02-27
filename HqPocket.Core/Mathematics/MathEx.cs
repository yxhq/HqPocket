
using System;
using System.Collections.Generic;

namespace HqPocket.Mathematics;

public static class MathEx
{
    /// <summary>
    /// 1E-6
    /// </summary>
    public const double ConstExp = 1E-6;

    public const double TwoPi = 2.0 * Math.PI;

    /// <summary>
    /// 以度为单位计算Cos值
    /// </summary>
    /// <param name="degree"></param>
    /// <returns></returns>
    public static double CosDegree(double degree)
    {
        return Math.Cos(Rad(degree));
    }

    /// <summary>
    /// 以度为单位计算Sin值
    /// </summary>
    /// <param name="degree"></param>
    /// <returns></returns>
    public static double SinDegree(double degree)
    {
        return Math.Sin(Rad(degree));
    }

    /// <summary>
    /// 返回 Math.Atan2(y, x) 以度为单位的值
    /// </summary>
    /// <param name="y"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    public static double Atan2Degree(double y, double x)
    {
        return Degree(Math.Atan2(y, x));
    }

    /// <summary>
    /// 度数转弧度
    /// </summary>
    /// <param name="degree">度数</param>
    /// <returns></returns>
    public static double Rad(double degree)
    {
        return degree * Math.PI / 180.0;
    }

    public static double Degree(double rad)
    {
        return rad * 180.0 / Math.PI;
    }

    public static bool IsZero(double d)
    {
        return Math.Abs(d) < ConstExp;
    }

    /// <summary>
    /// 一个辅助函数，用于轨迹计算中简化计算
    /// </summary>
    /// <param name="deltaDev">单位，弧度</param>
    /// <returns></returns>
    public static double F(double deltaDev)
    {
        return 1 - deltaDev * deltaDev / 24.0;
    }

    public static double F(double deltaDev, double deltaAzi)
    {
        return 1 - (deltaDev * deltaDev + deltaAzi * deltaAzi) / 24.0;
    }

    /// <summary>
    /// 平方和
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>x^2 + y^2</returns>
    public static double SquareSum(double x, double y)
    {
        return x * x + y * y;
    }

    /// <summary>
    /// 平方和
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns>x^2 + y^2 + z^2</returns>
    public static double SquareSum(double x, double y, double z)
    {
        return x * x + y * y + z * z;
    }

    /// <summary>
    /// 平方根
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>sqrt(x^2 + y^2)</returns>
    public static double SquareRoot(double x, double y)
    {
        return Math.Sqrt(SquareSum(x, y));
    }

    /// <summary>
    /// 平方根
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns>sqrt(x^2 + y^2 + z^2)</returns>
    public static double SquareRoot(double x, double y, double z)
    {
        return Math.Sqrt(SquareSum(x, y, z));
    }

    /// <summary>
    /// 将 [-2π,4π) 区间的数转为 [0,2π)
    /// </summary>
    /// <param name="d">[-2π,4π)的数</param>
    /// <returns>[0,2π)的数</returns>
    public static double To0And2Pi(double d)
    {
        return d switch
        {
            < 0 => TwoPi + d,
            >= TwoPi => d - TwoPi,
            _ => d
        };
    }

    /// <summary>
    /// 将 [-360,720) 区间的数转为 [0,360)
    /// </summary>
    /// <param name="d">[-360,720)的数</param>
    /// <returns>[0,360)的数</returns>
    public static double To0And360(double d)
    {
        return d switch
        {
            < 0 => 360 + d,
            >= 360 => d - 360,
            _ => d
        };
    }


    /// <summary>
    /// 按 interval 间隔 生成区间 (min, max) 等差数列，不包含 min 和 max
    /// </summary>
    /// <param name="min">区间最小值</param>
    /// <param name="max">区间最大值</param>
    /// <param name="interval">数列间隔</param>
    /// <returns>生成的等差数列</returns>
    public static IEnumerable<double> CreateArithmeticSequence(double min, double max, double interval)
    {
        for (double d = min + interval; d < max; d += interval)
        {
            yield return d;
        }
    }
}
