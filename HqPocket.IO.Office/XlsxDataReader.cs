using OfficeOpenXml;

using System;
using System.Collections.Generic;
using System.Linq;

namespace HqPocket.IO.Office;

public class XlsxDataReader<T> : DataReaderWriterBase<T>, IDataReader<T> where T : class, new()
{
    protected string WorkSheetName { get; }
    protected int HeadLineCount { get; }

    public XlsxDataReader(string fileName, string workSheetName, int headLineCount = 1)
        : base(fileName, false)
    {
        WorkSheetName = workSheetName;
        HeadLineCount = headLineCount;
    }

    public virtual IEnumerable<T> ReadAllData()
    {
        using ExcelPackage excelPackage = new(FileName);
        var worksheet = excelPackage.Workbook.Worksheets[WorkSheetName];
        var rowStart = worksheet.Dimension.Start.Row;       //工作区开始行号
        var rowEnd = worksheet.Dimension.End.Row;       //工作区结束行号

        if (HeadLineCount > rowStart)
        {
            rowStart = HeadLineCount;
        }

        for (var row = rowStart + 1; row <= rowEnd; row++)
        {
            T data = new();
            foreach (var rw in ReadWriteItems.Where(item => item.CanRead))
            {
                var cell = worksheet.Cells[row, rw.Column + 1];
                Type propertyType = rw.PropertyInfo.PropertyType;
                var value = Type.GetTypeCode(propertyType) switch
                {
                    TypeCode.Boolean => cell.GetValue<bool>(),
                    TypeCode.Char => cell.GetValue<char>(),
                    TypeCode.SByte => cell.GetValue<sbyte>(),
                    TypeCode.Byte => cell.GetValue<byte>(),
                    TypeCode.Int16 => cell.GetValue<short>(),
                    TypeCode.UInt16 => cell.GetValue<ushort>(),
                    TypeCode.Int32 => cell.GetValue<int>(),
                    TypeCode.UInt32 => cell.GetValue<uint>(),
                    TypeCode.Int64 => cell.GetValue<long>(),
                    TypeCode.UInt64 => cell.GetValue<ulong>(),
                    TypeCode.Single => cell.GetValue<float>(),
                    TypeCode.Double => cell.GetValue<double>(),
                    TypeCode.Decimal => cell.GetValue<decimal>(),
                    TypeCode.DateTime => cell.GetValue<DateTime>(),
                    TypeCode.String => cell.GetValue<string>(),
                    TypeCode.Object => cell.GetValue<object>(),
                    TypeCode.DBNull => cell.GetValue<DBNull>(),
                    TypeCode.Empty or _ => null
                };
                rw.PropertyInfo.SetValue(data, value);
            }
            yield return data;
        }
    }
}
