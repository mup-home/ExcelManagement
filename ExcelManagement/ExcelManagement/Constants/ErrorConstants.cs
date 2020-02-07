namespace OMP.BL.ExcelManagement.Constants
{
    public static class ErrorConstants
    {
        public const string MISSING_SHEET = "Worksheet: {0}, does not exist in the document.";
        public const string MISSING_COLUMN = "Worksheet: {0}, column {1} does not exist in the document.";
        public const string SHEET_WITHOUT_DATA = "Worksheet: {0}, has no data.";
        public const string MANDATORY_LINES_EMPTY = "Worksheet: {0},  has some line/s with mandatory data empty: \r\n{1}";
        public const string MANDATORY_DATA_EMPTY = "Worksheet: {0}, {1}, has column: {2}, empty.";
        public const string EMPTY_COLUMNS_LIST = "  - Row: {0}, has {1} empty. \n";
        public const string DUPLICATED_DATA = "Worksheet: {0}, the value: {2}, of column: {1}, is duplicated.";
        public const string DUPLICATED_DATA_WITH_KEY = "Worksheet: {0}, the value: {2}, of column: {1}, {3}is duplicated.";
        public const string INVALID_VALUE = "Worksheet: {0}, the value: {2}, of column: {1}, is not valid for the {0} {3}.";
        public const string INVALID_VALUE_WITH_PRIMARYKEY = "Worksheet: {0}, the value: {2}, of column: {1}, is not valid for the {0} {3} with values: {4}.";
        public const string INVALID_VALUE_FULL_INFO_WITH_MSN = "Worksheet: {0}, the {2}: {3} with MSN: {4}, for {5}: {6}, {7}: {8} & {9}: {10}, of the {0}: {1}, is not a valid value.";
        public const string INVALID_VALUE_RELATED = "Worksheet: {0}, the {2}: {3}, of the {0}: {1}, is not a valid value with {4}: {5}.";
        public const string INVALID_VALUE_LENGTH = "Worksheet: {0}, the value: {2}, of column {1} is not valid. The max length is {3}.";
        public const string INVALID_VALUE_LENGTH_DECIMAL = "Worksheet: {0}, the value: {2}, of column {1} is not valid. The max decimal numbers is {3}.";
        public const string INVALID_VALUE_YES_NO = "Worksheet: {0}, the value: {2}, of column {1} is not valid. The valid values are 'YES', 'NO'.";
        public const string NON_NUMERIC_VALUE = "Worksheet: {0}, the value: {2}, of column {1} should have a integer value.";
        public const string NON_NUMERIC_VALUE_GREATER_THAN_ZERO = "Worksheet: {0}, the value {2} of column {1} should have a integer value greater than zero.";
        public const string INCORRECT_FORMAT = "Worksheet: {0}, does not have the expected format, please check the data.";
        public const string INCORRECT_FORMAT_COLUMN = "Worksheet: {0}, does not have the expected format, please check the column {1} with value {2}.";
        public const string DATE_GREATHER_THAN = "Worksheet: {0} , {1}: {2}, the {3} column value must be greater than the {4} column value.";
        public const string DATE_LESS_THAN = "Worksheet: {0} , {1}: {2}, the {3} column value must be less than the {4} column value.";
        public const string SAVE_ERROR = "There was an error when recording the data in the database, please check the file you are trying to process.";
        public const string ERROR_SAVING_ENTITY = "Error saving {0} entity, with Id {1}.";
        public const string OVERLAPPING_FLIGHT = "There are scheduled activites in the same period as the flight {0} for the Aircraft {1}.";
        public const string OVERLAPPING_TASK = "There are scheduled activities in the same period as the task {0} for the Aircraft {1}.";
        public const string OVERLAPPING_SLOT = "There are scheduled maintenance tasks in the slot: {0} - {1}.";
        public const string OVERLAPPING_SLOT_EXCEL = "Worksheet: {0}, the {0}: {1} and the {2}: {3} overlaps in the same period for {4}: {5}.";
        public const string OVERLAPPING_SLOT_AVAILABLE = "There are scheduled maintenance tasks in the slot: {0} - {1}. Following slots are available in same time period: {2}.";
        public const string INVALID_SHEET_FIELD_CONFIGURATION = "Worksheet: {0}, has an invalid configuration for attribute DataField with value: {1}, in column: {2}.";
        public const string COLUMN_VALIDATION_ERROR = "Worksheet: {0}, validation error in column {1}.";
        public const string MANDATORIES_ID_MISFK_EMPTY = "Worksheet: {0}, the {0}, with name {1}, for {2}, has empty Mfasis ID and Mis FK.";
        public const string INVALID_VALUE_MIS_MFASIS_LIFE = "Worksheet: {0}, the value {2} is not valid for the {1} for the {0} {3}.";
    }
}
