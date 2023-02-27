
using System;
using System.Linq;
using System.Text;

namespace HqPocket.Communications;

public class BitConverterEx
{
    public static ushort ToUInt16(ReadOnlySpan<byte> value, ByteOrder byteOrder)
    {
        return byteOrder switch
        {
            ByteOrder.LittleEndian => BitConverter.ToUInt16(TransToLittleEndian(value, 2)),
            ByteOrder.BigEndian => BitConverter.ToUInt16(TransToBigEndian(value, 2)),
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
    }

    public static short ToInt16(ReadOnlySpan<byte> value, ByteOrder byteOrder)
    {
        return byteOrder switch
        {
            ByteOrder.LittleEndian => BitConverter.ToInt16(TransToLittleEndian(value, 2)),
            ByteOrder.BigEndian => BitConverter.ToInt16(TransToBigEndian(value, 2)),
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
    }

    public static uint ToUInt32(ReadOnlySpan<byte> value, ByteOrder byteOrder)
    {
        return byteOrder switch
        {
            ByteOrder.LittleEndian => BitConverter.ToUInt32(TransToLittleEndian(value, 4)),
            ByteOrder.BigEndian => BitConverter.ToUInt32(TransToBigEndian(value, 4)),
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
    }

    public static int ToInt32(ReadOnlySpan<byte> value, ByteOrder byteOrder)
    {
        return byteOrder switch
        {
            ByteOrder.LittleEndian => BitConverter.ToInt32(TransToLittleEndian(value, 4)),
            ByteOrder.BigEndian => BitConverter.ToInt32(TransToBigEndian(value, 4)),
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
    }


    public static float ToSingle(ReadOnlySpan<byte> value, ByteOrder byteOrder)
    {
        return byteOrder switch
        {
            ByteOrder.LittleEndian => BitConverter.ToSingle(TransToLittleEndian(value, 4)),
            ByteOrder.BigEndian => BitConverter.ToSingle(TransToBigEndian(value, 4)),
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
    }

    public static double ToDouble(ReadOnlySpan<byte> value, ByteOrder byteOrder)
    {
        return byteOrder switch
        {
            ByteOrder.LittleEndian => BitConverter.ToDouble(TransToLittleEndian(value, 8)),
            ByteOrder.BigEndian => BitConverter.ToDouble(TransToBigEndian(value, 8)),
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
    }

    public static string ToASCII(Span<byte> value, ByteOrder byteOrder)
    {
        static ReadOnlySpan<byte> GetReversedValue(Span<byte> v)
        {
            v.Reverse();
            return v;
        }

        return byteOrder switch
        {
            ByteOrder.LittleEndian => Encoding.ASCII.GetString(value),
            ByteOrder.BigEndian => Encoding.ASCII.GetString(GetReversedValue(value)),
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
    }

    public static string ToUnicode(Span<byte> value, ByteOrder byteOrder)
    {
        if (value.Length % 2 != 0) return "Unicode为双字节";

        return byteOrder switch
        {
            ByteOrder.LittleEndian => Encoding.Unicode.GetString(value),
            ByteOrder.BigEndian => Encoding.BigEndianUnicode.GetString(value),
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
    }

    public static ReadOnlySpan<byte> TransToLittleEndian(ReadOnlySpan<byte> value, int resultLength)
    {
        byte[] lbs = new byte[resultLength];
        for (int i = 0; i < value.Length; i++)
        {
            lbs[i] = value[i];
        }
        return lbs.AsSpan();
    }

    public static ReadOnlySpan<byte> TransToBigEndian(ReadOnlySpan<byte> value, int resultLength)
    {
        byte[] bbs = new byte[resultLength];
        for (int i = 0; i < value.Length; i++)
        {
            bbs[i] = value[value.Length - 1 - i];
        }
        return bbs.AsSpan();
    }

    public static string ToHexString(char value)
    {
        return $"{value:X2}";
    }

    public static string ToHexString(int value, ByteOrder byteOrder)
    {
        var bs = BitConverter.GetBytes(value);
        return byteOrder is ByteOrder.LittleEndian ? bs.ToHexString() : bs.Reverse().ToHexString();
    }

    public static string ToHexString(uint value, ByteOrder byteOrder)
    {
        var bs = BitConverter.GetBytes(value);
        return byteOrder is ByteOrder.LittleEndian ? bs.ToHexString() : bs.Reverse().ToHexString();
    }

    public static string ToHexString(short value, ByteOrder byteOrder)
    {
        var bs = BitConverter.GetBytes(value);
        return byteOrder is ByteOrder.LittleEndian ? bs.ToHexString() : bs.Reverse().ToHexString();
    }

    public static string ToHexString(ushort value, ByteOrder byteOrder)
    {
        var bs = BitConverter.GetBytes(value);
        return byteOrder is ByteOrder.LittleEndian ? bs.ToHexString() : bs.Reverse().ToHexString();
    }

    public static string ToHexString(long value, ByteOrder byteOrder)
    {
        var bs = BitConverter.GetBytes(value);
        return byteOrder is ByteOrder.LittleEndian ? bs.ToHexString() : bs.Reverse().ToHexString();
    }

    public static string ToHexString(ulong value, ByteOrder byteOrder)
    {
        var bs = BitConverter.GetBytes(value);
        return byteOrder is ByteOrder.LittleEndian ? bs.ToHexString() : bs.Reverse().ToHexString();
    }

    public static string ToHexString(float value, ByteOrder byteOrder)
    {
        var bs = BitConverter.GetBytes(value);
        return byteOrder is ByteOrder.LittleEndian ? bs.ToHexString() : bs.Reverse().ToHexString();
    }

    public static string ToHexString(double value, ByteOrder byteOrder)
    {
        var bs = BitConverter.GetBytes(value);
        return byteOrder is ByteOrder.LittleEndian ? bs.ToHexString() : bs.Reverse().ToHexString();
    }
}
