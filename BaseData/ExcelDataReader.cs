using BaseData.Entities;
using OfficeOpenXml;

namespace BaseData
{
    public class ExcelDataReader
    {
        public static List<Pollution> ReadData(string filePath, string sheetName)
        {
            var entities = new List<Pollution>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[sheetName] ?? throw new Exception($"Лист '{sheetName}' не найден.");
                int lastRow = 0, lastCol = 0;
                for (int row = 1; row <= worksheet.Dimension.Rows; row++)
                {
                    if (!string.IsNullOrWhiteSpace(worksheet.Cells[row, 1].Text)) lastRow = row;
                }

                for (int col = 1; col <= worksheet.Dimension.Columns; col++)
                {
                    if (!string.IsNullOrWhiteSpace(worksheet.Cells[1, col].Text)) lastCol = col;
                }

                var columnData = new List<List<string>>();

                for (int col = 1; col <= lastCol; col++)
                {
                    var values = new List<string>();
                    for (int row = 2; row <= lastRow; row++)
                    {
                        values.Add(worksheet.Cells[row, col].Text);
                    }
                    columnData.Add(values);
                }

                for (int i = 0; i < columnData[0].Count; i++)
                {
                    var entity = new Pollution
                    {
                        Date = DateTime.Parse(columnData[0][i]),
                        PointID = int.Parse(columnData[1][i]),
                        Concentration = decimal.Parse(columnData[2][i])
                    };
                    entities.Add(entity);
                }
            }
            return entities;
        }
    }
}
