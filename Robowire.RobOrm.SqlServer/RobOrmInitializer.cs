using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Robowire.RobOrm.Core;
using Robowire.RobOrm.Core.DefaultRules;
using Robowire.RobOrm.Core.Migration;
using Robowire.RobOrm.Core.Migration.Internal;
using Robowire.RobOrm.Core.Query.Model;
using Robowire.RobOrm.SqlServer.Migration;

namespace Robowire.RobOrm.SqlServer
{
    public static class RobOrmInitializer
    {
        public static void Initialize(
            IContainer container,
            Func<ISqlConnectionStringProvider> connectionStringProviderFactory,
            bool applySchemaMigration,
            params Assembly[] entitiesOrigin)
        {
            container.Setup(s => s.For<ISqlConnectionStringProvider>().Import.FromFactory(locator => connectionStringProviderFactory()));

            container.Setup(c => c.For<IDataModelHelper>().Use<CachedDataModelHelper>());
            container.Setup(c => c.For<ITransactionManager<SqlConnection>>().Use<ConnectionCreator>());
            container.Setup(c => c.For<IDatabase>().Use<Database>());
            container.Setup(c => c.For<ISchemaMigrator>().Use<SchemaMigrator>());
            container.Setup(c => c.For<IEntityNamingConvention>().Use<DefaultEntityNamingConvention>());

            foreach (var assembly in entitiesOrigin)
            {
                container.Setup(s => s.ScanAssembly(assembly));
            }

            if (!applySchemaMigration)
            {
                return;
            }

            var sqlScriptGenerator = new SqlMigrationScriptBuilder();
            var hashBuilder = new MigrationHashBuilder();
            var proxy = new ScriptBuilderProxy(hashBuilder, sqlScriptGenerator);

            using (var locator = container.GetLocator())
            {
                var migrator = locator.Get<ISchemaMigrator>();
                migrator.MigrateStructure(locator, proxy);

                var hash = hashBuilder.GetHash();
                var script = sqlScriptGenerator.ToString(hash);

                var connectionBuilder = locator.Get<ITransactionManager<SqlConnection>>();

                using (var connection = connectionBuilder.Open())
                {
                    using (var command = new SqlCommand(script, connection.GetConnection()))
                    {
                        command.ExecuteNonQuery();
                    }

                    connection.Commit();
                }

            }
        }
    }
}
