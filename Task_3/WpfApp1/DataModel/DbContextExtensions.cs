using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

public static class DbContextExtensions
{
    public static DataTable ToDataTable<T>(this IList<T> data)
    {
        PropertyDescriptorCollection properties =
            TypeDescriptor.GetProperties(typeof(T));
        DataTable table = new DataTable();
        table.TableName = "Sheet";
        foreach (PropertyDescriptor prop in properties)
            table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        foreach (T item in data)
        {
            DataRow row = table.NewRow();
            foreach (PropertyDescriptor prop in properties)
                row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
            table.Rows.Add(row);
        }
        return table;
    }
}