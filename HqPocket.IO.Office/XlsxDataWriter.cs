using HqPocket.Helpers;

using OfficeOpenXml;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace HqPocket.IO.Office;

public class XlsxDataWriter<T> : DataReaderWriterBase<T>, IDataWriter<T>, IDisposable where T : class
{
    protected string WorkSheetName { get; }
    protected ExcelPackage ExcelPackage { get; }
    protected int RowNumber { get; set; }

    public XlsxDataWriter(string fileName, string workSheetName)
        : base(fileName, false)
    {
        ExcelPackage = new ExcelPackage(new FileInfo(fileName));
        WorkSheetName = workSheetName;
    }

    public virtual void WriteHeadLine(string? head = null)
    {
        var worksheets = ExcelPackage.Workbook.Worksheets;
        if (worksheets[WorkSheetName] is not null)
        {
            worksheets.Delete(WorkSheetName);
        }
        var worksheet = ExcelPackage.Workbook.Worksheets.Add(WorkSheetName);
        RowNumber = 1;
        foreach (var rw in ReadWriteItems)
        {
            worksheet.Cells[RowNumber, rw.Column + 1].Value = rw.DisplayName;
        }
    }

    public virtual void AppendWrite(T data)
    {
        var worksheet = ExcelPackage.Workbook.Worksheets[WorkSheetName];
        RowNumber++;
        foreach (var rw in ReadWriteItems)
        {
            var cell = worksheet.Cells[RowNumber, rw.Column + 1];
            cell.Value = Type.GetTypeCode(rw.PropertyInfo.PropertyType) switch
            {
                TypeCode.DateTime => string.Format(rw.Format, rw.PropertyInfo.GetValue(data)),
                TypeCode.Double => Math.Round((double)rw.PropertyInfo.GetValue(data)!, FormatHelper.GetDigits(rw.Format)),
                _ => rw.PropertyInfo.GetValue(data)
            };
        }
    }

    public virtual void AppendWrite(IEnumerable<T> datas)
    {
        foreach (var data in datas)
        {
            AppendWrite(data);
        }
    }

    public virtual void SaveAndClose()
    {
        ExcelPackage.Save();
    }

    public async Task SaveAndCloseAsync(CancellationToken cancellationToken = default)
    {
        await ExcelPackage.SaveAsync(cancellationToken);
    }

    public void Dispose()
    {
        ExcelPackage.Dispose();
        GC.SuppressFinalize(this);
    }
}
