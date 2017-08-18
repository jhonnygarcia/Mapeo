using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestorMapeos.Models.Model.Erp20;
using GestorMapeos.Models.Model.Gestor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace GestorMapeos.Test
{
    [TestClass]
    public class GestorMapeosServicesTest
    {
        [AssemblyInitialize]
        public static void TestAssemblyInitialize(TestContext context)
        {
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;

            if (Database.Exists("erp"))
                Database.Delete("erp");

            if (Database.Exists("gestor"))
                Database.Delete("gestor");

            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<GestorContext>());
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ErpContext>());
        }
    }
}
