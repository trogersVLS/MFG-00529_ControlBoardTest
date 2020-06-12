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

            Task<int> testing = fct.LogNewTest("test_user", "TA20H054");
            while (!testing.IsCompleted)
            {
                Thread.Sleep(1000);
            };
            long test_id = testing.Result;

            Task<int> testResult = fct.LogTestData(test, "TA20H54", test_id);
            while (!testResult.IsCompleted)
            {
                Thread.Sleep(1000);
            };

            Task<int> updating = fct.LogTestResult("UPDATE", test_id);
            while (!updating.IsCompleted)
            {
                Thread.Sleep(1000);
            };


            Assert.IsTrue(true);

        }
    }
}
