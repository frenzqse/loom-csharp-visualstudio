/***
 * Licensed to the Austrian Association for Software Tool Integration (AASTI)
 * under one or more contributor license agreements. See the NOTICE file
 * distributed with this work for additional information regarding copyright
 * ownership. The AASTI licenses this file to you under the Apache License,
 * Version 2.0 (the "License"); you may not use this file except in compliance
 * with the License. You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 ***/

//using NetProxy.Mockup;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace NetProxyTests
{
    
    
    /// <summary>
    ///This is a test class for MockupProxyFactoryTest and is intended
    ///to contain all MockupProxyFactoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MockupProxyFactoryTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes

        //You can use the following additional attributes as you write your tests:

        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            //MockupProxyFactory.Initialize(@"..\..\..\..\testDaten\netProxyMockups\MockupConfig.ini");
        }

        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
        }

        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
        }

        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
        }

        #endregion

        #region Tests for common object-methods

        /// <summary>
        ///A test for CreateNewProxy
        ///</summary>
        public T CreateNewProxyTestHelper<T>() where T : class
        {
            //MockupProxyFactory target = MockupProxyFactory.Instance;
            //T actual = target.CreateNewProxy<T>();

            //Assert.IsNotNull(actual);
            //Assert.IsInstanceOfType(actual, typeof(T));
            //return actual;
            return null;
        }

        /// <summary>
        /// Test if a proxy can be created
        /// </summary>
        [TestMethod]
        public void CreateNewProxyTest()
        {
            //CreateNewProxyTestHelper<InterfaceTest1>();
        }

        /// <summary>
        /// Test if GetType returns the correct type
        /// </summary>
        [TestMethod]
        public void GetTypeTest()
        {
            //InterfaceTest1 helper = CreateNewProxyTestHelper<InterfaceTest1>();

            //Assert.AreEqual<Type>(typeof(InterfaceTest1), helper.GetType());
        }

        /// <summary>
        /// Test if ToString returns the default ToString (Type.ToString())
        /// </summary>
        [TestMethod]
        public void ToStringTest()
        {
            //InterfaceTest1 helper = CreateNewProxyTestHelper<InterfaceTest1>();

            //Assert.AreEqual<string>(typeof(InterfaceTest1).ToString(), helper.ToString());
        }

        /// <summary>
        /// Test if a HashCode is calculated, the HashCode should be the same as the Type-HashCode
        /// </summary>
        [TestMethod]
        public void GetHashCodeTest()
        {
            //InterfaceTest1 helper = CreateNewProxyTestHelper<InterfaceTest1>();

            //Assert.AreEqual<int>(typeof(InterfaceTest1).GetHashCode(), helper.GetHashCode());
        }

        /// <summary>
        /// Helper to test all possible equals-constilations
        /// </summary>
        public void EqualsTestHelper(object o1, object o2, bool? expectedValue = null)
        {
            Assert.AreEqual<bool>(o1.Equals(o2), o2.Equals(o1));

            if (expectedValue.HasValue)
            {
                Assert.AreEqual<bool>(o1.Equals(o2), expectedValue.Value);
            }
        }

        /// <summary>
        /// Tests if Equals with two proxies of the same type works
        /// </summary>
        [TestMethod]
        public void EqualsTest()
        {
            //InterfaceTest1 helper = CreateNewProxyTestHelper<InterfaceTest1>();
            //InterfaceTest1 helper2 = CreateNewProxyTestHelper<InterfaceTest1>();
            //EqualsTestHelper(helper, helper2, true);
        }

        /// <summary>
        /// Tests if Equals with null returns false
        /// </summary>
        [TestMethod]
        public void EqualsTestNull()
        {
            //InterfaceTest1 helper = CreateNewProxyTestHelper<InterfaceTest1>();
            //Assert.IsFalse(helper.Equals(null));
        }

        /// <summary>
        /// Tests if Equals with the type of the proxie is allways the same (doesn't matter if true or false)
        /// </summary>
        [TestMethod]
        public void EqualsTestType()
        {
            //InterfaceTest1 helper = CreateNewProxyTestHelper<InterfaceTest1>();
            //EqualsTestHelper(helper, typeof(InterfaceTest1));
        }

        /// <summary>
        /// Tests if Equals with some random string returns false
        /// </summary>
        [TestMethod]
        public void EqualsTestFalse()
        {
            //InterfaceTest1 helper = CreateNewProxyTestHelper<InterfaceTest1>();
            //EqualsTestHelper(helper, "testString", false);
        }

        #endregion

        [TestMethod]
        public void GetVal1Test()
        {
            //InterfaceTest1 helper = CreateNewProxyTestHelper<InterfaceTest1>();

            //Assert.AreEqual<string>("my First testValue", helper.getVal1());
        }

        [TestMethod]
        public void GetVal2Test()
        {
            //InterfaceTest1 helper = CreateNewProxyTestHelper<InterfaceTest1>();

            //Assert.AreEqual<int>(42, helper.getVal2());
        }

        [TestMethod]
        public void GetVal3Test()
        {
            //InterfaceTest1 helper = CreateNewProxyTestHelper<InterfaceTest1>();

            //Assert.AreEqual<int>(71, helper.getVal3());
        }

        [TestMethod]
        public void SetVal1Test()
        {
            //InterfaceTest1 helper = CreateNewProxyTestHelper<InterfaceTest1>();

            //helper.setVal1("string1");
        }

        [TestMethod]
        public void SetVal13Test()
        {
            //InterfaceTest1 helper = CreateNewProxyTestHelper<InterfaceTest1>();

            //helper.setVal13("string2", 42);
        }

        [TestMethod]
        public void SetVal2Test()
        {
            //InterfaceTest1 helper = CreateNewProxyTestHelper<InterfaceTest1>();

            //helper.setVal2(71);
        }
    }
}
