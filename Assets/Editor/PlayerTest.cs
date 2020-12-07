using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class PlayerTest
    {
        private static CharacterAgent MahouAgent;
        
        [SetUp]
        public void Setup()
        {
            MahouAgent = GameObject.Find("MahouPrefabs").GetComponent<CharacterAgent>();
            Debug.Log("Set up.");
        }

        [TearDown]
        public void Teardown()
        {
            MahouAgent = null;
            Debug.Log("Tear down");
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Debug.Log("One set up.");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Debug.Log("One tear down.");
        }

        [Test]
        [Order(2)]
        public void LogError()
        {
            //LogAssert.Expect(LogType.Error, "Failed.");
            LogAssert.ignoreFailingMessages = true;
            Debug.LogError("Wooops!.");
            LogAssert.ignoreFailingMessages = false;

        }


        // A Test behaves as an ordinary method
        [Test]
        [Order(1)]
        public void PlayerTestSimplePasses()
        {
            Assert.IsTrue(MahouAgent != null);
           // Assert.AreEqual(MahouAgent.actualLiving.CurrentHealth, 100f);
           // MahouAgent.ChangeHealth(10);
           // Assert.AreEqual(MahouAgent.actualLiving.CurrentHealth, 90f);


            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator PlayerTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
