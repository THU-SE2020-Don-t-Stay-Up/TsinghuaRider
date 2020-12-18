using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class WeaponTest
    {
        // 远程武器和近战武器代表
        private WeaponAgent gun = null;
        private MeleeWeaponAgent sword = null;
        private CharacterAgent MahouAgent;


        [SetUp]
        public void SetUp()
        {
            LogAssert.ignoreFailingMessages = true;
            var initialGame = new Initialization();
            initialGame.TestAwake();

            MahouAgent = GameObject.Find("MahouPrefab").GetComponent<CharacterAgent>();
            MahouAgent.TestAwake();

            gun = GameObject.Find("Gun").GetComponent<WeaponAgent>();
            gun.TestAwake(MahouAgent);

            sword = GameObject.Find("Sword").GetComponent<MeleeWeaponAgent>();
            sword.TestAwake(MahouAgent);

            Debug.Log("Set up.");
        }

        [TearDown]
        public void TearDown()
        {
            LogAssert.ignoreFailingMessages = false;

            gun = null;
            sword = null;
            Debug.Log("Tear down.");
        }

        [UnityTest]
        [Order(0)]
        [Description("测试能否正确加载武器及其信息")]
        public IEnumerator WeaponLoadTest()
        {
            LogAssert.ignoreFailingMessages = true;

            Assert.IsNotNull(gun);
            Assert.AreEqual(4, sword.itemIndex);
            Assert.IsNull(sword.bulletPrefab);

            Assert.IsNotNull(sword);
            Assert.AreEqual(5, gun.itemIndex);
            Assert.IsNotNull(gun.bulletPrefab);
            yield return null;

            LogAssert.ignoreFailingMessages = false;
        }

        [UnityTest]
        [Order(1)]
        [Description("测试能否正确发射子弹")]
        public IEnumerator GunShot()
        {
            LogAssert.ignoreFailingMessages = true;
            Assert.IsNull(GameObject.Find("BulletPrefab(Clone)"));

            gun.Attack();
            Assert.IsNotNull(GameObject.Find("BulletPrefab(Clone)"));
            yield return null;

            LogAssert.ignoreFailingMessages = false;
        }

        [UnityTest]
        [Order(2)]
        [Description("测试能否挥刀")]
        public IEnumerator SwordWave()
        {
            LogAssert.ignoreFailingMessages = true;

            bool finishFlag = false;
            sword.TestAttack();
            var frameCount = 0;
            while ((frameCount++) <= 200)
            {
                // 挥刀过程中返回false，完成一次挥刀后sword.Attack()才返回ture
                if (sword.TestAttack() == true)
                {
                    finishFlag = true;
                }
                yield return null;
            }
            Assert.IsTrue(finishFlag);
            yield return null;

            LogAssert.ignoreFailingMessages = false;
        }
    }
}
