using System;
using System.Data.Common;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Storage;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HETSAPI.Models
{
    /// <summary>
    /// Uility class used to update database column comments or descriptions.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class DbCommentsUpdater<TContext>
        where TContext : DbAppContext
    {
        /// <summary>
        /// Constructor. 
        /// </summary>
        /// <param name="context"></param>
        public DbCommentsUpdater(TContext context)
        {
            _context = context;
        }

        private readonly TContext _context;
        private IDbContextTransaction _transaction;

        /// <summary>
        /// Update the database descriptions
        /// </summary>
        public void UpdateDatabaseDescriptions()
        {
            Type contextType = typeof(TContext);

            var props = contextType.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            DbConnection con = _context.Database.GetDbConnection();

            try
            {
                con.Open();

                _transaction = _context.Database.BeginTransaction();

                foreach (var prop in props)
                {
                    if (prop.PropertyType.InheritsOrImplements((typeof(DbSet<>))))
                    {
                        var tableType = prop.PropertyType.GetGenericArguments()[0];
                        SetTableDescriptions(tableType);
                    }
                }

                _transaction.Commit();
            }
            catch
            {
                _transaction?.Rollback();
                throw;
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        /// <summary>
        /// Set a table comment
        /// </summary>
        /// <param name="tableType"></param>
        private void SetTableDescriptions(Type tableType)
        {
            IEntityType entityType = _context.Model.FindEntityType(tableType);

            string fullTableName = entityType.Relational().TableName;
            Regex regex = new Regex(@"(\[\w+\]\.)?\[(?<table>.*)\]");
            Match match = regex.Match(fullTableName);

            string tableName = match.Success ? match.Groups["table"].Value : fullTableName;

            object[] tableAttrs = tableType.GetTypeInfo().GetCustomAttributes(typeof(TableAttribute), false);

            if (tableAttrs.Length > 0)
            {
                tableName = ((TableAttribute)tableAttrs[0]).Name;
            }

            //  get the table description
            object[] tableExtAttrs = tableType.GetTypeInfo().GetCustomAttributes(typeof(MetaDataAttribute), false);
            if (tableExtAttrs.Length > 0)
            {
                SetTableDescription(tableName, ((MetaDataAttribute)tableExtAttrs[0]).Description);

            }

            foreach (IProperty entityProperty in entityType.GetProperties())
            {
                // Not all properties have MemberInfo, so a null check is required.
                if (entityProperty.PropertyInfo != null)
                {
                    // get the custom attributes for this field.                
                    object[] attrs = entityProperty.PropertyInfo.GetCustomAttributes(typeof(MetaDataAttribute), false);
                    if (attrs.Length > 0)
                    {
                        SetColumnDescription(tableName, entityProperty.Relational().ColumnName, ((MetaDataAttribute)attrs[0]).Description);
                    }
                }
            }
        }

        /// <summary>
        /// Set a column comment
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="description"></param>
        private void SetColumnDescription(string tableName, string columnName, string description)
        {
            // Postgres has the COMMENT command to update a description.
            string query = "COMMENT ON COLUMN \"" + tableName + "\".\"" + columnName + "\" IS '" + description.Replace("'", "\'") + "'";

            _context.Database.ExecuteSqlCommand(query);
        }

        /// <summary>
        /// Set a column comment
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="description"></param>
        private void SetTableDescription(string tableName, string description)
        {
            // Postgres has the COMMENT command to update a description.
            string query = "COMMENT ON TABLE \"" + tableName + "\" IS '" + description.Replace("'", "\'") + "'";
            _context.Database.ExecuteSqlCommand(query);
        }
    }

    /// <summary>
    /// Reflection Utility
    /// </summary>
    public static class ReflectionUtil
    {
        /// <summary>
        /// Check for Inherits or Implements
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static bool InheritsOrImplements(this Type child, Type parent)
        {
            parent = ResolveGenericTypeDefinition(parent);

            var currentChild = child.GetTypeInfo().IsGenericType
                                    ? child.GetGenericTypeDefinition()
                                    : child;

            while (currentChild != typeof(object))
            {
                if (parent == currentChild || HasAnyInterfaces(parent, currentChild))
                    return true;

                currentChild = currentChild.GetTypeInfo().BaseType != null
                                && currentChild.GetTypeInfo().BaseType.GetTypeInfo().IsGenericType
                                    ? currentChild.GetTypeInfo().BaseType.GetGenericTypeDefinition()
                                    : currentChild.GetTypeInfo().BaseType;

                if (currentChild == null)
                    return false;
            }
            return false;
        }

        private static bool HasAnyInterfaces(Type parent, Type child)
        {
            return child.GetInterfaces()
                .Any(childInterface =>
                {
                    var currentInterface = childInterface.GetTypeInfo().IsGenericType
                        ? childInterface.GetGenericTypeDefinition()
                        : childInterface;

                    return currentInterface == parent;
                });
        }

        private static Type ResolveGenericTypeDefinition(Type parent)
        {
            bool shouldUseGenericType = !(parent.GetTypeInfo().IsGenericType && parent.GetGenericTypeDefinition() != parent);

            if (parent.GetTypeInfo().IsGenericType && shouldUseGenericType)
                parent = parent.GetGenericTypeDefinition();

            return parent;
        }
    }
}
