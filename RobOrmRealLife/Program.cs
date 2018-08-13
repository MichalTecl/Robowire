using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Robowire;
using Robowire.RobOrm.Core;
using Robowire.RobOrm.Core.Query.Model;
using Robowire.RobOrm.SqlServer;

namespace RobOrmRealLife
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var locator = GetLocator())
            {
                var db = locator.Get<IDatabase>();


                var auto = db.SelectFrom<IManufacturer>().Join(c => c.Models).Execute().ToList();       

                
                
                
                Console.ReadLine();

            }
            
        }


        private static IServiceLocator GetLocator()
        {
            var container = new Container();

            Action<IContainer> migrator = null;

            container.Setup(c =>
                {
                    migrator = RobOrmInitializer.InitializeAndGetMigrator(
                        c,
                        () => new ConnectionStringProvider(),
                        typeof(ICar).Assembly);
                });

            migrator(container);

            return container.GetLocator();
        }
        

    }
}
