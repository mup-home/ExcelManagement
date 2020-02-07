using OfficeOpenXml;
using OMP.BL.ExcelManagement.Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace OMP.BL.ExcelManagement.Helpers
{  
    internal static class DataLoadingHelper
    {
        #region Internal methods
        
        #region Workbook validation methods
        internal static void ValidateExcelBook(string workbookName, Dictionary<string, ExcelWorksheet> fleetWorksheets, ref List<KeyValuePair<string, List<string>>> errors)
        {
            var columnErrors = new List<KeyValuePair<string, List<string>>>();
            //foreach (var sheet in bookType.GetEnumValues())
            //{
            //    var sheetName = ((Enum)sheet).GetName(ResourceType.DataloadingLiteral);
            //    if (!BookHasSheet(fleetWorksheets, sheetName))
            //    {
            //        errors.Add(new KeyValuePair<string, List<string>>("MISSING_SHEET", new List<string> { sheetName }));
            //    }
            //    else
            //    {
            //        ValidateSheetFormat((Enum)sheet, fleetWorksheets[sheetName], ref columnErrors);
            //    }
            //}
            //if (columnErrors.Count > 0)
            //{
            //    errors.Add(new KeyValuePair<string, List<string>>("SHEET_COLUMNS_INFO", new List<string>()));
            //    foreach (var keyPair in columnErrors)
            //    {
            //        errors.Add(new KeyValuePair<string, List<string>>(keyPair.Key, keyPair.Value));
            //    };

            //}
        }

        /// <summary>
        /// Check if exists the worksheet passed as parameter
        /// </summary>
        /// <param name="bookSheets">Excel worksheets</param>
        /// <param name="sheetName">Sheet name</param>
        /// <returns></returns>
        internal static bool BookHasSheet(Dictionary<string, ExcelWorksheet> bookSheets, string sheetName)
        {
            return bookSheets.Any(w => w.Key == sheetName);
        }

        internal static void ValidateSheetFormat(Enum sheet, ExcelWorksheet excelWorksheet, ref List<KeyValuePair<string, List<string>>> errors)
        {
            //var sheetColumns = sheet.GetConfigurationEnum();
            //if (sheetColumns.IsNotNull())
            //{
            //    foreach (var columnsEnum in sheetColumns.GetEnumValues())
            //    {
            //        if (columnsEnum.IsNotNull())
            //        {
            //            var sheetName = sheet.GetName(ResourceType.DataloadingLiteral);
            //            var currentColumn = (Enum)columnsEnum;
            //            var columnTitle = excelWorksheet.Cells[2, currentColumn.GetColumnNumber()].Value?.ToString().Trim() ?? "{empty}";
            //            var columnExpectedTitle = currentColumn.GetExcelColumnName().Trim();
            //            if (columnExpectedTitle.IsNullOrEmpty())
            //            {
            //                throw new DataLoadingColumnException($"Worksheet: {sheetName}, has missing ExcelColumnName attribute for column: {currentColumn.GetName()}.");
            //            }
            //            else if (!columnTitle.Equals(columnExpectedTitle, StringComparison.InvariantCultureIgnoreCase))
            //            {
            //                errors.Add(new KeyValuePair<string, List<string>>("SHEET_INVALID_COLUMN_FORMAT", new List<string> { sheetName, columnTitle, columnExpectedTitle }));
            //            }
            //        }
            //    }
            //}
        }
        #endregion Workbook validation methods

        #region Get data methods
        /// <summary>
        /// Get all the worksheets related to the excel
        /// </summary>
        /// <param name="excelFile">Excel file with info</param>
        /// <returns>Dictionary with info for name and related worksheet</returns>
        internal static Dictionary<string, ExcelWorksheet> GetExcelWorksheets(ExcelPackage excelFile)
        {
            Dictionary<string, ExcelWorksheet> currentWorksheets = new Dictionary<string, ExcelWorksheet>();
            excelFile.Workbook.Worksheets.ToList().ForEach(ws => currentWorksheets.Add(ws.Name, ws));
            foreach (var ws in currentWorksheets)
            {
                if (ws.Value?.Dimension != null)
                {
                    var rowsToDeleteList = new List<int>();
                    for (int i = SheetConstants.DATA_STARTING_ROW; i <= GetMaxRowsUnfilteredWorksheet(ws.Value); i++)
                    {
                        int counterColumns = 0;
                        for (int j = 0; j <= GetSheetMaxColumns(ws.Value); j++)
                        {
                            var cellCheck = ws.Value.GetValue(i, j);
                            if (cellCheck == null)
                            {
                                counterColumns++;
                            }
                        }
                        if (counterColumns >= GetSheetMaxColumns(ws.Value))
                        {
                            rowsToDeleteList.Add(i - rowsToDeleteList.Count);
                        }
                    }
                    foreach (var item in rowsToDeleteList)
                    {
                        ws.Value.DeleteRow(item, 1, true);
                    }
                }
            }

            return currentWorksheets;
        }

        internal static string GetExcelFieldData(ExcelWorksheet sheet, int row, Enum column, ref string errorMessage)
        {
            //var col = column.GetColumnNumber();
            //var data = sheet.Cells[row, col].Value;
            //var defaultValue = column.GetDataFieldDefault();

            //if (data != null)
            //{
            //    if (data.ToString().Trim().IsNullOrEmpty() && !column.GetEmptyMessage().IsNullOrEmpty())
            //    {
            //        errorMessage += $"{column.GetEmptyMessage(ResourceType.DataloadingLiteral)}, ";
            //    }
            //    return data.ToString().Trim();
            //}
            //else if (defaultValue != null)
            //{
            //    return defaultValue;
            //}
            //else if (!column.GetEmptyMessage().IsNullOrEmpty())
            //{
            //    errorMessage += $"{column.GetEmptyMessage(ResourceType.DataloadingLiteral)}, ";
            //}

            return string.Empty;
        }

        internal static string GetExcelFieldData(ExcelWorksheet sheet, int row, Enum column, Enum dependencyField, ref string errorMessage)
        {
            //var col = column.GetColumnNumber();
            //var dependencyFieldColum = dependencyField.GetColumnNumber();
            //var data = sheet.Cells[row, col].Value;
            //var dependencyFieldData = sheet.Cells[row, dependencyFieldColum].Value;

            //if (data != null)
            //{
            //    return data.ToString().Trim();
            //}
            //else if (dependencyFieldData == null && !column.GetEmptyMessage().IsNullOrEmpty())
            //{
            //    errorMessage += $"{column.GetEmptyMessage(ResourceType.DataloadingLiteral)}, ";
            //}

            return string.Empty;
        }

        internal static string GetExcelEntityPrimaryKey(ExcelWorksheet sheet, int row, Type sheetColumns)
        {
            string primaryKey = string.Empty;
            //foreach (Enum column in sheetColumns.GetEnumValues())
            //{
            //    if (column.IsPrimaryKey())
            //    {
            //        var value = GetExcelFieldData(sheet, row, column);
            //        primaryKey += $"{column.GetName()}: {value}, ";
            //    }
            //}

            return primaryKey.Substring(0, primaryKey.Length - 2);
        }


        /// <summary>
        /// Returns the max number of rows
        /// </summary>
        /// <param name="worksheet">Worksheet</param>
        /// <returns>the max number of rows</returns>
        internal static int GetMaxRowsUnfilteredWorksheet(ExcelWorksheet worksheet)
        {
            return worksheet.AutoFilterAddress?.End.Row ?? worksheet.Dimension.End.Row;
        }

        internal static int GetSheetMaxColumns(ExcelWorksheet worksheet)
        {
            return worksheet.AutoFilterAddress?.End.Column ?? worksheet.Dimension.End.Column;
        }

        internal static int GetSheetMaxRows(ExcelWorksheet worksheet)
        {
            if (worksheet.AutoFilterAddress != null)
            {
                int countOfRows = 0;
                for (int i = 1; i <= GetMaxRowsUnfilteredWorksheet(worksheet); i++)
                {

                    for (int j = 0; j <= GetSheetMaxColumns(worksheet); j++)
                    {
                        var cellCheck = worksheet.GetValue(i, j);
                        if (cellCheck != null)
                        {
                            countOfRows++;
                            break;
                        }
                    }
                }
                return countOfRows;
            }
            else
            {
                return worksheet.Dimension.End.Row;
            }
        }

        internal static string GetEntityPrimaryKey(object registry, Type sheetColumns)
        {
            string primaryKey = string.Empty;
            //foreach (Enum column in sheetColumns.GetEnumValues())
            //{
            //    if (column.IsPrimaryKey())
            //    {
            //        // var value = registry.GetType().GetProperty(column.GetName())?.GetValue(registry).ToString();
            //        var value = ObjectPropertyHelper.GetPropertyValue(column.GetName(), registry);
            //        primaryKey += $"{column.GetName()}: {value}, ";
            //    }
            //}

            return primaryKey.Substring(0, primaryKey.Length - 2);
        }
        #endregion Get data methods
        
        #region Conversion methods
        internal static DateTime ConvertDataValueToDate(string maintenanceName, string filledValue, string fieldName, string sheetName, ref List<KeyValuePair<string, List<string>>> errors)
        {
            try
            {
                CultureInfo cultureInfo = new CultureInfo("es-ES");
                cultureInfo.DateTimeFormat.DateSeparator = "/";
                return DateTime.Parse(filledValue, cultureInfo);
            }
            catch (Exception)
            {
                errors.Add(new KeyValuePair<string, List<string>>("INCORRECT_FORMAT_COLUMN", new List<string>
                {
                    sheetName,
                    fieldName,
                    filledValue
                }));
                return DateTime.MinValue;
            }
        }

        internal static int ConvertDataValueToInt(string name, string filledValue, string sheetName, string columnName, ref List<KeyValuePair<string, List<string>>> errors)
        {
            try
            {
                return int.Parse(filledValue);
            }
            catch (Exception)
            {
                if (!string.IsNullOrEmpty(filledValue))
                {
                    errors.Add(new KeyValuePair<string, List<string>>("NON_NUMERIC_VALUE", new List<string> { sheetName, columnName, filledValue }));
                }
                return 0;
            }
        }

        internal static short ConvertDataValueToShort(string name, string filledValue, string sheetName, string columnName, ref List<KeyValuePair<string, List<string>>> errors)
        {
            try
            {
                return short.Parse(filledValue);
            }
            catch (Exception)
            {
                if (!string.IsNullOrEmpty(filledValue))
                {
                    errors.Add(new KeyValuePair<string, List<string>>("NON_NUMERIC_VALUE", new List<string> { sheetName, columnName, filledValue }));
                }
                return 0;
            }
        }

        internal static decimal ConvertDataValueToDecimal(string name, string filledValue, string sheetName, string columnName, ref List<KeyValuePair<string, List<string>>> errors)
        {
            try
            {
                return decimal.Parse(filledValue);
            }
            catch (Exception)
            {
                if (!string.IsNullOrEmpty(filledValue))
                {
                    errors.Add(new KeyValuePair<string, List<string>>("NON_NUMERIC_VALUE", new List<string> { sheetName, columnName, filledValue }));
                }
                return 0;
            }
        }

        internal static decimal ConvertDataValueToDecimal(string columnName, string columnValue, string registryType, string registryKey, string registryPrimaryKey, ref List<KeyValuePair<string, List<string>>> errors)
        {
            try
            {
                return decimal.Parse(columnValue);
            }
            catch (Exception)
            {
                if (!string.IsNullOrEmpty(columnValue))
                {
                    errors.Add(new KeyValuePair<string, List<string>>("INVALID_VALUE_WITH_PRIMARYKEY", new List<string> { registryType, columnName, columnValue, registryKey, registryPrimaryKey }));
                }
                return 0;
            }
        }
        #endregion Conversion methods

        #region Entities validations methods
        internal static void ValidateRegistry(string sheetName, Type sheetColumns, object registry, object fleetData, ref List<KeyValuePair<string, List<string>>> errors)
        {
            string currentColumnName = string.Empty;
            try
            {
                foreach (Enum column in sheetColumns.GetEnumValues())
                {
                    //currentColumnName = column.GetName();
                    //var property = ObjectPropertyHelper.GetProperty(column.GetDataFieldName(), registry);
                    //if (property != null)
                    //{
                    //    var namePropertyValue = ObjectPropertyHelper.GetPropertyValue("Name", registry);
                    //    #region Validations to apply in all columns
                    //    ColumnValidations.ValidateColumnType(sheetName, column, namePropertyValue, column.GetDataFieldName(), registry, ref errors);
                    //    #endregion Validations to apply in all columns

                    //    #region Validations included according attributes definitions
                    //    if (column.GetValidationType() == DataLoadingValidation.ValueExist || column.GetValidationType() == DataLoadingValidation.MIS)
                    //    {
                    //        var dbSearchList = GetColumnValidationList(fleetData, column.GetValidationDbSearchList());
                    //        var searchList = GetColumnValidationList(fleetData, column.GetValidationValue());
                    //        var mfasisValue = ObjectPropertyHelper.GetPropertyValue("Mfasis", registry);
                    //        ColumnValidations.ValidateDataField(namePropertyValue, sheetName, column, registry, searchList, ref errors);
                    //    }
                    //    else if (column.GetValidationType() == DataLoadingValidation.Id)
                    //    {
                    //        var searchList = GetColumnValidationList(fleetData, column.GetValidationValue());
                    //        ColumnValidations.ValidateDataField(namePropertyValue, sheetName, column, registry, searchList, ref errors);
                    //    }
                    //    else if (column.GetValidationType() != DataLoadingValidation.None)
                    //    {
                    //        ColumnValidations.ValidateDataField(ObjectPropertyHelper.GetPropertyValue("Name", registry), sheetName, column, registry, ref errors);
                    //    }
                    //    #endregion Validations included according attributes definitions
                    //}
                    //else
                    //{
                    //    throw new DataLoadingColumnException(sheetName, column.GetDataFieldName(), column.GetName());
                    //}
                }
            }
            catch
            {
                throw;
            }
        }

        internal static void ValidateRegistry(string sheetName, Type sheetColumns, object registry, object fleetData, string mfasisProperty, ref List<KeyValuePair<string, List<string>>> errors)
        {
            string currentColumnName = string.Empty;
            try
            {
                foreach (Enum column in sheetColumns.GetEnumValues())
                {
                    //currentColumnName = column.GetName();
                    //var property = ObjectPropertyHelper.GetProperty(column.GetDataFieldName(), registry);
                    //if (property != null)
                    //{
                    //    var registryName = ObjectPropertyHelper.GetPropertyValue("Name", registry);
                    //    #region Validations to apply in all columns
                    //    ColumnValidations.ValidateColumnType(sheetName, column, registryName, column.GetDataFieldName(), registry, ref errors);
                    //    #endregion Validations to apply in all columns

                    //    #region Validations included according attributes definitions
                    //    if (column.GetValidationType() == DataLoadingValidation.ValueExist || column.GetValidationType() == DataLoadingValidation.Id)
                    //    {
                    //        if (column.GetValidationMultipleListValue() != null)
                    //        {
                    //            ColumnValidations.ValidateDataField(registryName, sheetName, column, registry, fleetData, ref errors);
                    //        }
                    //        else
                    //        {
                    //            var searchList = GetColumnValidationList(fleetData, column.GetValidationValue());
                    //            ColumnValidations.ValidateDataField(registryName, sheetName, column, registry, searchList, ref errors);
                    //        }
                    //    }
                    //    else if (column.GetValidationType() == DataLoadingValidation.MIS)
                    //    {
                    //        var searchList = GetColumnValidationList(fleetData, column.GetValidationValue());
                    //        ColumnValidations.ValidateDataField(registryName, sheetName, column, registry, searchList, mfasisProperty, ref errors);
                    //    }
                    //    else if (column.GetValidationType() != DataLoadingValidation.None)
                    //    {
                    //        ColumnValidations.ValidateDataField(registryName, sheetName, column, registry, ref errors);
                    //    }
                    //    #endregion Validations included according attributes definitions
                    //}
                    //else if (!column.IgnoreOnImport())
                    //{
                    //    throw new DataLoadingColumnException(sheetName, column.GetDataFieldName(), column.GetName());
                    //}
                }
            }
            catch
            {
                throw;
            }
        }

        internal static IList GetColumnValidationList(object fleetData, string searchListName)
        {
            if (string.IsNullOrEmpty(searchListName))
            {
                return null;
            }

            foreach (var property in fleetData.GetType().GetProperties())
            {
                if (property.Name == searchListName)
                {
                    return (IList)property.GetValue(fleetData);
                }
            }
            throw new ArgumentException($"Invalid MIS search propery name: {searchListName}");
        }
        #endregion Entities validations methods

        #region Handle errors methods
        internal static void UpdateSheetLoadErrors(IList sheetData, string errorMessage, string sheetName, bool validateSheetWithoutData, ref List<KeyValuePair<string, List<string>>> errors)
        {
            if (errorMessage != string.Empty)
            {
                errors.Add(new KeyValuePair<string, List<string>>("MANDATORY_LINES_EMPTY", new List<string> { sheetName, errorMessage }));
            }
            else if (validateSheetWithoutData && (sheetData == null || (sheetData?.Count == 0)))
            {
                errors.Add(new KeyValuePair<string, List<string>>("SHEET_WITHOUT_DATA", new List<string> { sheetName }));
            }
        }

        internal static string AddMandatoryRowColumnEmptyError(int row, string emptyMandatoryColumns)
        {
            return string.Format(ErrorConstants.EMPTY_COLUMNS_LIST, row.ToString(), emptyMandatoryColumns.Substring(0, emptyMandatoryColumns.Length - 2));
        }
        #endregion Handle errors methods

        #endregion Internal methods

        #region Private methods
        private static string GetExcelFieldData(ExcelWorksheet sheet, int row, Enum column)
        {
            //var col = column.GetColumnNumber();
            //var data = sheet.Cells[row, col].Value;
            //var defaultValue = column.GetDataFieldDefault();

            //if (data != null)
            //{
            //    return data.ToString().Trim();
            //}
            //else if (defaultValue != null)
            //{
            //    return defaultValue;
            //}

            return string.Empty;
        }

        #endregion Private methods
    }
}