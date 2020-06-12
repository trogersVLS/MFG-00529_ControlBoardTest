using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ControlBoardTest;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;


namespace ControlBoardTest_TDD
{
    [TestClass]
    public class DatabaseManager_TDD
    {
        [TestMethod]
        public void InsertTestInstance_TDD()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("eqid", "equipment-id");
            data.Add("user-id", "sw_svc");
            data.Add("location", "location");
            data.Add("timestamp", DateTime.UtcNow.ToString());
            data.Add("serial", "VA20H045");
            data.Add("result", "TEST");

            string connStr = "Provider=MSOLEDBSQL;Server=vls-test-database.database.windows.net;Initial Catalog=vls-production-test;Authentication=SqlPassword;User ID=sw_svc;Password=Ventec123;Use Encryption for Data=true;";

            Task<int> task =  SQLServer.InsertOneRow(connStr,
                                                     "[dbo].[test-instance]",
                                                     data);
            
            while (!task.IsCompleted) 
            {
                Thread.Sleep(1000);
            };
            Console.WriteLine(task.Result);
            Assert.IsTrue(task.IsCompleted);
        }

        [TestMethod]
        public void UpdateTestInstace_TDD()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("result", "FAIL-TEST");
            string connStr = "Provider=MSOLEDBSQL;Server=vls-test-database.database.windows.net;Initial Catalog=vls-production-test;Authentication=SqlPassword;User ID=sw_svc;Password=Ventec123;Use Encryption for Data=true;";

            Task<int> task = SQLServer.UpdateResult(connStr,
                                                    "[dbo].[test-instance]",
                                                    "8",
                                                    data);

            while (!task.IsCompleted)
            {
                Thread.Sleep(1000);
            };
            Console.WriteLine(task.Result);
            Assert.IsTrue(task.IsCompleted);

        }
        [TestMethod]
        public void InsertTestResult_TDD()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("test-id", "612");
            data.Add("serial", "serial");
            data.Add("test-name", "test-name");
            data.Add("upper-bound", "upper-bound");
            data.Add("lower-bound", "lower-bound");
            data.Add("measured", "measured");
            data.Add("result", "result");

            string connStr = "Provider=MSOLEDBSQL;Server=vls-test-database.database.windows.net;Initial Catalog=vls-production-test;Authentication=SqlPassword;User ID=sw_svc;Password=Ventec123;Use Encryption for Data=true;";
            try
            {
                Task<int> task = SQLServer.InsertOneRow(connStr,
                                                         "[dbo].[tests]",
                                                         data);
                while (!task.IsCompleted)
                {
                    Thread.Sleep(1000);
                };

                Console.WriteLine(task.Result);
                Assert.IsTrue(task.IsCompleted);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                if (e.InnerException != null)
                {
                    Console.WriteLine(e.InnerException.Message);
                }
            }
            
        }
    }
}
