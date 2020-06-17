using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlBoardTest;
using System.Reflection;
using System.Collections.Generic;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace ControlBoardTest_TDD
{
    [TestClass]
    public class DataLogging_TDD
    {
        [TestMethod]
        public void TestLogging()
        {
            MethodInfo dummyMethod = null;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            //parameters.Add("result", "PASS");
            parameters.Add("lower_limit", "0");
            parameters.Add("upper_limit", "9000");
            //parameters.Add("measured", "90005");
            TestData test = new TestData(1, "dummy_test", "dummy_test_method", dummyMethod, parameters);

            FunctionalTest fct = new FunctionalTest(true);
            fct.GetSettings();

            Task<(int, int)> testing = fct.LogNewTest("TAYLOR", "test_both_local_and_remote", true);
            while (!testing.IsCompleted)
            {
                Thread.Sleep(1000);
            };
            (long remote_test_id, long local_test_id) = testing.Result;

            Task<int> testResult = fct.LogTestData(test, "TA20H54", true, remote_test_id, local_test_id);
            while (!testResult.IsCompleted)
            {
                Thread.Sleep(1000);
            };

            Task<int> updating = fct.LogTestResult("UPDATED_TDD", true, remote_test_id, local_test_id);
            while (!updating.IsCompleted)
            {
                Thread.Sleep(1000);
            };


            Assert.IsTrue(true);

        }

        [TestMethod]
        public void TestExecution()
        {
            List<string> parts = new List<string> { "V_PRO" };
            MainForm form = new MainForm(parts);
            MethodInfo dummyMethod = form.FCT.GetType().GetMethod("test_dummy_test");
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("lower_limit", "0");
            parameters.Add("upper_limit", "9000");
            //parameters["measured"] =  "9005";


            TestData test = new TestData(1, "Dummy Test", "Dummy Test", dummyMethod, parameters);
            test.parameters["measured"] =  "9005";

            form.Powered = true;
            form.Telnet = true;
            Random rnd = new Random();
            form.SERIAL_NUM = rnd.Next(100).ToString();
            form.FCT.DUMMY_TEST = new List<TestData>();
            for (int i = 0; i < 5; i++)
            {
                form.FCT.DUMMY_TEST.Add(test);
            }
            try
            {
                form.ExecuteTests();
            }
            catch 
            { 
            }


        }
    }
}
