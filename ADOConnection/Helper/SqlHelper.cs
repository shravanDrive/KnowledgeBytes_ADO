using ADOConnection.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ADOConnection.Helper
{
    public static class SqlHelper
    {
        public static DataTable BuildGuidListTable(IList<Guid> itemIds)
        {
            var guidListTable = new DataTable("type.GuidListTable");
            guidListTable.Columns.Add("ItemId", typeof(Guid));

            if(itemIds == null || itemIds.Count < 1)
                return guidListTable;

            foreach(var item in itemIds)
            {
                var dataRow = guidListTable.NewRow();
                dataRow["ItemId"] = item;
                guidListTable.Rows.Add(dataRow);
            }
            return guidListTable;
        }

        public static DataTable BuildVarcharListTable(IList<string> itemIds)
        {
            var varcharListTable = new DataTable("type.VarcharListTable");
            varcharListTable.Columns.Add("ItemId", typeof(string)).MaxLength = 100;

            if(itemIds == null || itemIds.Count < 1)
                return varcharListTable;

            foreach (var itemId in itemIds)
            {
                var dataRow = varcharListTable.NewRow();
                dataRow["ItemId"] = itemId;
                varcharListTable.Rows.Add(dataRow);
            }
            return varcharListTable;
        }

        public static DataTable BuildIntegerListTable(IList<int> itemIds)
        {
            var intListTable = new DataTable("type.IntegerListTable");
            intListTable.Columns.Add("ItemId", typeof(int));

            if(itemIds == null || itemIds.Count < 1)
                return intListTable;

            foreach (var itemid in itemIds)
            {
                var dataRow = intListTable.NewRow();
                dataRow["ItemId"] = itemid;
                intListTable.Rows.Add( dataRow);
            }
            return intListTable;
        }

        public static DataTable BuildDevelopmentCourseListTable(IList<CourseDTO> items)
        {
            var courseListTable = new DataTable("type.DevCourseListTable");
            courseListTable.Columns.Add("CourseId", typeof(Guid));
            courseListTable.Columns.Add("CourseVersion", typeof(Guid));
            courseListTable.Columns.Add("EstimatedCompletionTimeinSeconds", typeof(Guid));

            if(items == null || items.Count < 1)
                return courseListTable;

            foreach (var item in items)
            {
                var dataRow = courseListTable.NewRow();
                dataRow["CourseId"] = item.CourseId;
                dataRow["CourseVersion"] = item.CourseId;
                dataRow["EstimatedCompletionTimeinSeconds"] = item.CourseId;
                courseListTable.Rows.Add(dataRow);
            }

            return courseListTable;
        }

        public static T GetValue<T>(this IDataReader reader, int fieldOrdinal)
        {
            var type = typeof(T);
            var destinationType = Nullable.GetUnderlyingType(type) ?? type;
            return (T)Convert.ChangeType(reader.GetValue(fieldOrdinal), destinationType);
        }

        public static T GetValue<T>(this IDataReader reader, int fieldOrdinal, T defaultOutput)
        {
            T result;
            TryGetValue(reader, fieldOrdinal, defaultOutput, out result);
            return result;
        }

        public static bool TryGetValue<T>( this IDataReader reader, int fieldOrdinal, out T output)
        {
            return TryGetValue(reader, fieldOrdinal, default(T), out output);
        }

        public static bool TryGetValue<T>(this IDataReader reader, int fieldOrdinal, T dDbNullReturn, out T output)
        {
            try
            {
                output = (DBNull.Value == reader.GetValue(fieldOrdinal)) ? dDbNullReturn : reader.GetValue<T>(fieldOrdinal);
                return true;
            }
            catch (Exception ex)
            {
                if(ex is NullReferenceException || ex is InvalidCastException || ex is FormatException || ex is OverflowException)
                {
                    output = dDbNullReturn;
                    return false;
                }
                throw;
            }
        }

        private static void AttachParameters(
            SqlCommand sqlCommand,
            List<SqlParameter> sqlParameters)
        {
            if (sqlCommand == null || sqlParameters == null || sqlParameters.Any())
                return;

            foreach (var sqlParameter in sqlParameters)
            {
                if (sqlParameter == null)
                    continue;

                if ((sqlParameter.Direction == ParameterDirection.InputOutput
                    || sqlParameter.Direction == ParameterDirection.Input)
                    && sqlParameter.Value ==  null)
                {
                    sqlParameter.Value = DBNull.Value;
                }

                sqlCommand.Parameters.Add(sqlParameter);
            }
        }
    }
}
