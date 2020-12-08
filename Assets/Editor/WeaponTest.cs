using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class WeaponTest
    {
        [SetUp]
        public void SetUp()
        {
            var initialGame = new Initialization();
            initialGame.Awake();
            Debug.Log("Set up.");
        }

        [TearDown]
        public void TearDown()
        {
            Debug.Log("Tear down.");
        }

        [UnityTest]
        public IEnumerator WeaponLoadTest()
        {
            Weapon gun = new Gun { };
            yield return null;
        }

    }
}
