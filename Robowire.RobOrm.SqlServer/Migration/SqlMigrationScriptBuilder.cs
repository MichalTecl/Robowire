﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Robowire.RobOrm.Core.Internal;
using Robowire.RobOrm.Core.Migration;

namespace Robowire.RobOrm.SqlServer.Migration
{
    public class SqlMigrationScriptBuilder : IMigrationScriptBuilder
    {
        private const string c_verTableName = "Roborm_Schema_Version";
        private const string c_applyDtColumnName = "ApplyDt";
        private const string c_schemaHash = "SchemaHash";
        private const string c_skipLabel = "rbrm_afterscript";

        private readonly StringBuilder m_tablesBuilder = new StringBuilder();
        private readonly StringBuilder m_columnsBuilder = new StringBuilder();
        private readonly StringBuilder m_keysBuilder = new StringBuilder();
        
        public void CreateTable(string tableName, string pkColumnName, Type pkColumnType, bool pkAutogenerated)
        {
            var identity = string.Empty;
            var pkTypeName = SqlTypeMapper.GetSqlTypeName(pkColumnType, 0);

            if (pkAutogenerated)
            {
                identity = "IDENTITY(1,1) ";
            }

            m_tablesBuilder.AppendLine($"IF NOT EXISTS(SELECT TOP 1 1 FROM sys.tables WHERE name = '{tableName}')");
            m_tablesBuilder.AppendLine("BEGIN");
            m_tablesBuilder.Append("\t");
            m_tablesBuilder.AppendLine($"CREATE TABLE [{tableName}] ([{pkColumnName}] {pkTypeName} {identity}NOT NULL, ");
            m_tablesBuilder.AppendLine(
                $"\tCONSTRAINT [PK_{tableName}] PRIMARY KEY CLUSTERED ([{pkColumnName}] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            m_tablesBuilder.AppendLine("\t);");
            m_tablesBuilder.AppendLine("END");
            m_tablesBuilder.AppendLine();
        }

        public void CreateColumn(string tableName, string columnName, string columnType, bool allowsNulls, bool unique)
        {
            m_columnsBuilder.AppendLine(
                $"IF NOT EXISTS(SELECT TOP 1 1 FROM sys.tables t INNER JOIN sys.columns c ON t.object_id = c.object_id WHERE t.name = '{tableName}' AND c.name = '{columnName}')");
            m_columnsBuilder.AppendLine("BEGIN");

            m_columnsBuilder.AppendLine($"\tALTER TABLE [{tableName}] ADD [{columnName}] {columnType};");
            
            m_columnsBuilder.AppendLine("END");

            m_columnsBuilder.Append(
                $"ALTER TABLE [{tableName}] ALTER COLUMN [{columnName}] {columnType}");

            m_columnsBuilder.Append(allowsNulls ? " NULL" : " NOT NULL").AppendLine(";").AppendLine();

            var uniConstraintName = $"UQ_{tableName}_{columnName}";

            if (!unique)
            {
                m_columnsBuilder.Append($"IF OBJECT_ID('{uniConstraintName}') IS NOT NULL ")
                    .AppendLine($"ALTER TABLE [{tableName}] DROP CONSTRAINT [{uniConstraintName}];");
            }
            else
            {
                m_columnsBuilder.Append($"IF OBJECT_ID('{uniConstraintName}') IS NULL ")
                    .AppendLine($"ALTER TABLE [{tableName}] ADD CONSTRAINT [{uniConstraintName}] UNIQUE NONCLUSTERED [{columnName}];");
            }

        }

        public void CreateForeignKey(
            string referringTableName,
            string referringColumnName,
            string referredTableName,
            string referredColumnName)
        {
            var constraintName = $"FK_{referringTableName}_{referringColumnName}__{referredTableName}_{referredColumnName}";

            m_keysBuilder.AppendLine($"IF OBJECT_ID('{constraintName}') IS NULL ");

            m_keysBuilder.Append(
                    $"\tALTER TABLE [{referringTableName}] ADD CONSTRAINT [{constraintName}] FOREIGN KEY ([{referringColumnName}])")
                .AppendLine($" REFERENCES [{referredTableName}] ([{referredColumnName}]);");

            m_keysBuilder.AppendLine();
        }

        private void RenderHead(StringBuilder sb, string schemeHash)
        {
            sb.AppendLine($"IF NOT EXISTS(SELECT TOP 1 1 FROM sys.tables WHERE name = '{c_verTableName}')");
            sb.AppendLine($"\tCREATE TABLE {c_verTableName} ({c_applyDtColumnName} DATETIME NOT NULL, {c_schemaHash} VARCHAR(256) NOT NULL);");
            sb.AppendLine();

            sb.AppendLine(
                $"IF ((SELECT TOP 1 {c_schemaHash} FROM {c_verTableName} ORDER BY {c_applyDtColumnName} DESC) = '{schemeHash}') \r\n\t GOTO {c_skipLabel};");

            sb.AppendLine("BEGIN TRAN;");
            sb.AppendLine("BEGIN TRY");
            sb.AppendLine();
        }

        private void RenderFoot(StringBuilder sb, string schemeHash)
        {
            sb.AppendLine();

            sb.AppendLine(
                $"INSERT INTO {c_verTableName} ({c_applyDtColumnName}, {c_schemaHash}) VALUES (GETDATE(), '{schemeHash}');");

            sb.AppendLine("\r\n\tCOMMIT;");

            sb.AppendLine("END TRY");
            sb.AppendLine("BEGIN CATCH");
            sb.AppendLine("\tROLLBACK;");
            sb.AppendLine("\tTHROW;");
            sb.AppendLine("END CATCH");

            sb.AppendLine($"{c_skipLabel}:");
        }

        public string ToString(string versionHash)
        {


            var sb = new StringBuilder();

            RenderHead(sb, versionHash);

            sb.Append(m_tablesBuilder);
            sb.Append(m_columnsBuilder);
            sb.Append(m_keysBuilder);

            RenderFoot(sb, versionHash);

            return sb.ToString();
        }
    }
}
