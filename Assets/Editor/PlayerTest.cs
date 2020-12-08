using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class PlayerTest
    {
        private CharacterAgent MahouAgent;
        
        //[SetUp]
        //public void Setup()
        //{
        //    MahouAgent = GameObject.Find("MahouPrefabs").GetComponent<CharacterAgent>();

        //    Debug.Log("Set up.");
        //}

        //[TearDown]
        //public void Teardown()
        //{
        //    MahouAgent = null;
        //    Debug.Log("Tear down");
        //}

        [SetUp]
        public void SetUp()
        {
            MahouAgent = GameObject.Find("MahouPrefabs").GetComponent<CharacterAgent>();
            var initialGame = new Initialization();
            initialGame.Awake();
            MahouAgent.Awake();

            var uiHealthBar = new UIHealthBar();
            uiHealthBar.Awake();

            var uiDashBar = new UIDashBar();
            uiDashBar.Awake();
            Debug.Log("Set up.");
        }

        [TearDown]
        public void TearDown()
        {
            MahouAgent = null;
            Debug.Log("Tear down.");
        }



        [Test]
        public void LogError()
        {
            //LogAssert.Expect(LogType.Error, "Failed.");
            LogAssert.ignoreFailingMessages = true;
            Debug.LogError("Wooops!.");
            LogAssert.ignoreFailingMessages = false;

        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        [Order(1)]
        public IEnumerator CharacterLoadedSuccessfully()
        {
            LogAssert.ignoreFailingMessages = true;

            Assert.IsTrue(MahouAgent != null);
            Debug.Log(MahouAgent.characterIndex);
            Debug.Log(MahouAgent.actualLiving.CurrentHealth);
            Debug.Log(MahouAgent.WeaponPrefab);
            Debug.Log(MahouAgent.actualLiving.TimeInvincible);
            yield return null;

            LogAssert.ignoreFailingMessages = false;
        }

        [UnityTest]
        [Order(2)]
        public IEnumerator CharacterHealthTest()
        {
            LogAssert.ignoreFailingMessages = true;

            // 初始血量应为100，有1s的无敌
            Assert.AreEqual(100f, MahouAgent.actualLiving.CurrentHealth);
            Assert.AreEqual(MahouAgent.actualLiving.MaxHealth, MahouAgent.actualLiving.CurrentHealth);
            Assert.IsTrue(MahouAgent.actualLiving.State.HasStatus(new InvincibleState()));

            //System.Threading.Thread.Sleep(1000);
            var frameCount = 0;
            while ((frameCount++) <= 65)
            {
                MahouAgent.Update();
                yield return null;
            }
            frameCount = 0;
            Assert.IsFalse(MahouAgent.actualLiving.State.HasStatus(new InvincibleState()));


            // 测试伤害，受伤后应进入无敌

            Assert.IsFalse(MahouAgent.actualLiving.State.HasStatus(new InvincibleState()));

            MahouAgent.ChangeHealth(-20f);
            Assert.IsTrue(MahouAgent.actualLiving.State.HasStatus(new InvincibleState()));

            while ((frameCount++) <= 60)
            {
                MahouAgent.Update();
                yield return null;
            }
            frameCount = 0;
            Assert.IsFalse(MahouAgent.actualLiving.State.HasStatus(new InvincibleState()));

            Assert.AreEqual(80f, MahouAgent.actualLiving.CurrentHealth);

            // 测试使用血瓶，血瓶每次回复10%最大生命值
            MahouAgent.UseItem(new HealthPotion { });
            Assert.AreEqual(90f, MahouAgent.actualLiving.CurrentHealth);

            // 测试使用血包，血包每次回复70%最大生命值
            MahouAgent.actualLiving.State.ClearStatus();

            MahouAgent.ChangeHealth(-70f);
            Assert.IsTrue(MahouAgent.actualLiving.State.HasStatus(new InvincibleState()));
            while ((frameCount++) <= 60)
            {
                MahouAgent.Update();
                yield return null;
            }
            frameCount = 0;
            Assert.IsFalse(MahouAgent.actualLiving.State.HasStatus(new InvincibleState()));

            MahouAgent.UseItem(new Medkit { });
            Assert.AreEqual(90f, MahouAgent.actualLiving.CurrentHealth);

            // 就算伤害值超过当前血量，血量最低也是0
            MahouAgent.ChangeHealth(-700f);
            Assert.AreEqual(0f, MahouAgent.actualLiving.CurrentHealth);

            // 血量上限不超过最大血量
            MahouAgent.ChangeHealth(700f);
            Assert.AreEqual(100f, MahouAgent.actualLiving.CurrentHealth);
            Assert.AreEqual(MahouAgent.actualLiving.MaxHealth, MahouAgent.actualLiving.CurrentHealth);

            LogAssert.ignoreFailingMessages = false;

        }

        /// <summary>
        /// 想测移动、速度的，但是似乎对rigidbody2d的操作不起作用；直接操作transform.position可行，但是没意义
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        [Order(3)]
        public IEnumerator MoveTest()
        {
            Assert.AreEqual(new Vector3(0, 0, 0), MahouAgent.GetPosition());

            var frameCount = 0;
            while ((frameCount++) <= 600)
            {
                MahouAgent.Update();
                MahouAgent.rigidbody2d.velocity = new Vector2(1, 0) * MahouAgent.ActualCharacter.MoveSpeed;
                //MahouAgent.transform.position = new Vector3(1, 2, 3);
            }
            Debug.Log(MahouAgent.GetPosition());
            yield return null;
        }

        /// <summary>
        /// 测试背包物品的添加、移除、使用
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        [Order(4)]
        public IEnumerator InventoryTest()
        {
            
            yield return null;
        }

    }
}
