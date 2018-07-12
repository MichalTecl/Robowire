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
            var ownerName = "Meriva";

            using (var locator = GetLocator())
            {
                var db = locator.Get<IDatabase>();

                var startDt = DateTime.Now;

                using (var transaction = db.OpenTransaction())
                {
                    //var merivyModely = db.SelectFrom<ICarModel>().Where(c => c.Name.Like("%"+ ownerName + "%")).Transform(m => m.Id);
                    //var merivy = db.SelectFrom<ICar>().Where(c => c.ModelId.InSubquery(merivyModely)).Execute();
                    
                    //db.DeleteAll(merivy);

                 
                    
                        
                    transaction.Commit();
                }


                
                Console.WriteLine((DateTime.Now - startDt).TotalMilliseconds);
                Console.ReadLine();

            }
            
        }


        private static IServiceLocator GetLocator()
        {
            var container = new Container();

            RobOrmInitializer.Initialize(container, () => new ConnectionStringProvider(), true, typeof(ICar).Assembly);

            return container.GetLocator();
        }
        

    }
}
