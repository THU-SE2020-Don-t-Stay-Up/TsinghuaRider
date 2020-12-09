using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class MonsterTest
    {
        private MonsterAgent DuoYuanJiFenAgent;
        private BossAgent LagrangeAgent;

        [SetUp]
        public void SetUp()
        {
            LogAssert.ignoreFailingMessages = true;
            DuoYuanJiFenAgent = GameObject.Find("多元鸡分").GetComponent<MonsterAgent>();
            LagrangeAgent = GameObject.Find("拉格朗日").GetComponent<BossAgent>();

            var initialGame = new Initialization();
            initialGame.Awake();

            DuoYuanJiFenAgent.Start();
            LagrangeAgent.Start();
            Debug.Log("Set up.");
        }

        [TearDown]
        public void TearDown()
        {
            LogAssert.ignoreFailingMessages = false;

            DuoYuanJiFenAgent = null;
            LagrangeAgent = null;
            Debug.Log("Tear down.");
        }

        [UnityTest]
        [Description("测试怪物能否正确加载")]
        public IEnumerator MonsterLoadTest()
        {
            Assert.IsNotNull(DuoYuanJiFenAgent);
            Assert.AreEqual(2, DuoYuanJiFenAgent.monsterIndex);
            Assert.AreEqual(new Vector3(0, 2, 0), DuoYuanJiFenAgent.transform.position);
            Assert.IsNotNull(DuoYuanJiFenAgent.bulletPrefab);

            Assert.IsNotNull(LagrangeAgent);
            Assert.AreEqual(5, LagrangeAgent.monsterIndex);
            Assert.AreEqual(new Vector3(3, 4, 0), LagrangeAgent.transform.position);
            Assert.IsNotNull(LagrangeAgent.bulletPrefab);

            Debug.Log("怪物成功加载");
            yield return null;
        }

        [UnityTest]
        [Order(0)]
        [Description("测试怪物能否正确找到目标")]
        public IEnumerator FindCharacterTest()
        {
            // 怪物一开始没有找玩家
            Assert.IsFalse(DuoYuanJiFenAgent.target != null);
            Assert.IsFalse(LagrangeAgent.target != null);

            // 经过1帧后，找到玩家
            var frameCount = 0;
            while ((frameCount++) <= 0)
            {
                DuoYuanJiFenAgent.Update();
                LagrangeAgent.Update();
                yield return null;
            }
            Assert.IsTrue(DuoYuanJiFenAgent.target != null);
            Assert.IsTrue(LagrangeAgent.target != null);
            Debug.Log("成功找到目标");

            yield return null;
        }

        [UnityTest]
        [Description("测试小怪血量改变函数否正确")]
        public IEnumerator DuoYuanJiFenHealthTest()
        {
            LogAssert.ignoreFailingMessages = true;

            // 多元鸡分的血量为40，初始没有无敌状态
            Assert.AreEqual(40f, DuoYuanJiFenAgent.actualLiving.MaxHealth);
            Assert.AreEqual(DuoYuanJiFenAgent.actualLiving.MaxHealth, DuoYuanJiFenAgent.actualLiving.CurrentHealth);
            Assert.IsFalse(DuoYuanJiFenAgent.actualLiving.State.HasStatus(new InvincibleState()));

            // 扣血后怪物也会进入无敌
            DuoYuanJiFenAgent.ChangeHealth(-20f);
            Assert.AreEqual(20f, DuoYuanJiFenAgent.actualLiving.CurrentHealth);
            Assert.IsTrue(DuoYuanJiFenAgent.actualLiving.State.HasStatus(new InvincibleState()));

            // 无敌状态不会扣血
            DuoYuanJiFenAgent.ChangeHealth(-20f);
            Assert.AreEqual(20f, DuoYuanJiFenAgent.actualLiving.CurrentHealth);

            // 消除无敌状态，进行扣血操作
            DuoYuanJiFenAgent.actualLiving.State.ClearStatus();

            // 扣血后血量不会为负数
            DuoYuanJiFenAgent.ChangeHealth(-2000f);
            Assert.AreEqual(0f, DuoYuanJiFenAgent.actualLiving.CurrentHealth);

            // 无敌状态下可以加血，血量不超过上限
            Assert.IsTrue(DuoYuanJiFenAgent.actualLiving.State.HasStatus(new InvincibleState()));
            DuoYuanJiFenAgent.ChangeHealth(200f);
            Assert.AreEqual(DuoYuanJiFenAgent.actualLiving.MaxHealth, DuoYuanJiFenAgent.actualLiving.CurrentHealth);

            LogAssert.ignoreFailingMessages = false;
            yield return null;
        }

        [UnityTest]
        [Description("测试Boss血量改变函数否正确")]
        public IEnumerator LagrangeHealthTest()
        {
            LogAssert.ignoreFailingMessages = true;

            // 拉格朗日的血量为800，初始没有无敌状态
            Assert.AreEqual(800f, LagrangeAgent.actualLiving.MaxHealth);
            Assert.AreEqual(LagrangeAgent.actualLiving.MaxHealth, LagrangeAgent.actualLiving.CurrentHealth);
            Assert.IsFalse(LagrangeAgent.actualLiving.State.HasStatus(new InvincibleState()));

            // 扣血后怪物也会进入无敌
            LagrangeAgent.ChangeHealth(-20f);
            Assert.AreEqual(780f, LagrangeAgent.actualLiving.CurrentHealth);
            Assert.IsTrue(LagrangeAgent.actualLiving.State.HasStatus(new InvincibleState()));

            // 无敌状态不会扣血
            LagrangeAgent.ChangeHealth(-20f);
            Assert.AreEqual(780f, LagrangeAgent.actualLiving.CurrentHealth);

            // 消除无敌状态，进行扣血操作
            LagrangeAgent.actualLiving.State.ClearStatus();

            // 扣血后血量不会为负数
            LagrangeAgent.ChangeHealth(-800f);
            Assert.AreEqual(0f, LagrangeAgent.actualLiving.CurrentHealth);

            // 无敌状态下可以加血，血量不超过上限
            Assert.IsTrue(LagrangeAgent.actualLiving.State.HasStatus(new InvincibleState()));
            LagrangeAgent.ChangeHealth(2000f);
            Assert.AreEqual(LagrangeAgent.actualLiving.MaxHealth, LagrangeAgent.actualLiving.CurrentHealth);

            LogAssert.ignoreFailingMessages = false;
            yield return null;
        }

        [UnityTest]
        [Description("测试能否获得正确的攻击方向")]
        public IEnumerator AttackDirTest()
        {
            var frameCount = 0;
            while ((frameCount++) <= 0)
            {
                DuoYuanJiFenAgent.Update();
                LagrangeAgent.Update();
                yield return null;
            }
            var targetPosition = DuoYuanJiFenAgent.target.transform.position;
            var monsterPosition = DuoYuanJiFenAgent.transform.position;
            var lagrangePosition = LagrangeAgent.transform.position;
            Assert.AreEqual((targetPosition - monsterPosition).normalized, DuoYuanJiFenAgent.GetAttackDirection());
            Assert.AreEqual((targetPosition - lagrangePosition).normalized, LagrangeAgent.GetAttackDirection());
        }

    }
}
