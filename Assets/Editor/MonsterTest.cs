using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [Order(0)]
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
        [Order(1)]
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
        [Order(2)]
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

        [UnityTest]
        [Order(3)]
        [Description("测试怪物状态机能否变为”追逐“")]
        public IEnumerator ChasingTest()
        {
            var frameCount = 0;
            while ((frameCount++) <= 9)
            {
                DuoYuanJiFenAgent.Update();
                LagrangeAgent.Update();
                yield return null;
            }
            Assert.IsTrue(DuoYuanJiFenAgent.IsChasing());
            Assert.IsTrue(LagrangeAgent.IsChasing());

            yield return null;
        }

        [UnityTest]
        [Order(4)]
        [Description("测试怪物有没有指定的skill")]
        public IEnumerator SkillTest()
        {
            // 多元鸡分有远程攻击技能、分裂技能
            var duoSkills = DuoYuanJiFenAgent.GetCurrentSkills();
            Skill splitSkill = duoSkills.FirstOrDefault(e => e is SplitSkill);
            Assert.IsNotNull(splitSkill);
            Skill duoMissleSkill = duoSkills.FirstOrDefault(e => e is MissleAttackSkill);
            Assert.IsNotNull(duoMissleSkill);

            // 拉格朗日有远程攻击、强化攻击、火球攻击、激光攻击技能
            var lagSkills = LagrangeAgent.GetCurrentSkills();
            Skill laserSkill = lagSkills.FirstOrDefault(e => e is LaserSkill);
            Assert.IsNotNull(laserSkill);
            Skill lagMissleSkill = lagSkills.FirstOrDefault(e => e is MissleAttackSkill);
            Assert.IsNotNull(lagMissleSkill);
            Skill barrageSkill = lagSkills.FirstOrDefault(e => e is BarrageSkill);
            Assert.IsNotNull(barrageSkill);
            Skill fierceSkill = lagSkills.FirstOrDefault(e => e is FierceSkill);
            Assert.IsNotNull(fierceSkill);

            yield return null;

        }

        [UnityTest]
        [Order(5)]
        [Description("测试Boss血量空后会不会被清除，由于Edit Mode下无法调用Destroy，故检测它是否掉落金币")]
        public IEnumerator LagrangeDeathTest()
        {
            LogAssert.ignoreFailingMessages = true;

            // 初始地图中无金币
            Assert.IsFalse(GameObject.Find("Coin(Clone)"));

            // Boss死亡后掉落金币
            LagrangeAgent.ChangeHealth(-800f);
            Assert.AreEqual(0f, LagrangeAgent.actualLiving.CurrentHealth);
            Assert.IsTrue(LagrangeAgent.IsDead());
            Assert.IsTrue(GameObject.Find("Coin(Clone)"));

            // 恢复血量，为了通过后续测试
            LagrangeAgent.ChangeHealth(800f);

            yield return null;
            LogAssert.ignoreFailingMessages = false;
        }

        [UnityTest]
        [Order(6)]
        [Description("测试多元鸡分死亡后能否产生3个微多元鸡分")]
        public IEnumerator DuoDeathTest()
        {
            LogAssert.ignoreFailingMessages = true;

            Assert.IsFalse(GameObject.Find("微多元鸡分(Clone)"));

            DuoYuanJiFenAgent.ChangeHealth(-40f);
            Assert.AreEqual(0f, DuoYuanJiFenAgent.actualLiving.CurrentHealth);
            Assert.IsTrue(DuoYuanJiFenAgent.IsDead());

            // 找到所有的微多元鸡分，应该有3个
            var objs = new List<GameObject>();
            foreach ( GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)))
            {
                if (go.name == "微多元鸡分(Clone)")
                {
                    objs.Add(go);
                }
            }
            Assert.AreEqual(3, objs.Count());

            // 恢复血量，为了通过后续测试
            DuoYuanJiFenAgent.ChangeHealth(40f);

            yield return null;
            LogAssert.ignoreFailingMessages = false;
        }


        [UnityTest]
        [Order(7)]
        [Description("测试小怪血量改变函数否正确")]
        public IEnumerator DuoYuanJiFenHealthTest()
        {
            LogAssert.ignoreFailingMessages = true;

            // 多元鸡分的血量为40，初始没有无敌状态
            Assert.AreEqual(40f, DuoYuanJiFenAgent.actualLiving.MaxHealth);
            Assert.AreEqual(DuoYuanJiFenAgent.actualLiving.MaxHealth, DuoYuanJiFenAgent.actualLiving.CurrentHealth);
            Assert.IsFalse(DuoYuanJiFenAgent.actualLiving.State.HasStatus(new InvincibleState()));

            // 扣血后怪物不会进入无敌
            DuoYuanJiFenAgent.ChangeHealth(-20f);
            Assert.AreEqual(20f, DuoYuanJiFenAgent.actualLiving.CurrentHealth);
            Assert.IsFalse(DuoYuanJiFenAgent.actualLiving.State.HasStatus(new InvincibleState()));

            // 加血后血量不超过上限
            DuoYuanJiFenAgent.ChangeHealth(200f);
            Assert.AreEqual(DuoYuanJiFenAgent.actualLiving.MaxHealth, DuoYuanJiFenAgent.actualLiving.CurrentHealth);

            // 扣血后血量不会为负数
            DuoYuanJiFenAgent.ChangeHealth(-2000f);
            Assert.AreEqual(0f, DuoYuanJiFenAgent.actualLiving.CurrentHealth);

            LogAssert.ignoreFailingMessages = false;
            yield return null;
        }

        [UnityTest]
        [Order(8)]
        [Description("测试Boss血量改变函数否正确")]
        public IEnumerator LagrangeHealthTest()
        {
            LogAssert.ignoreFailingMessages = true;

            // 拉格朗日的血量为800，初始没有无敌状态
            Assert.AreEqual(800f, LagrangeAgent.actualLiving.MaxHealth);
            Assert.AreEqual(LagrangeAgent.actualLiving.MaxHealth, LagrangeAgent.actualLiving.CurrentHealth);
            Assert.IsFalse(LagrangeAgent.actualLiving.State.HasStatus(new InvincibleState()));

            // 扣血后怪物不会进入无敌
            LagrangeAgent.ChangeHealth(-20f);
            Assert.AreEqual(780f, LagrangeAgent.actualLiving.CurrentHealth);
            Assert.IsFalse(LagrangeAgent.actualLiving.State.HasStatus(new InvincibleState()));

            // 加血后，血量不超过上限
            LagrangeAgent.ChangeHealth(2000f);
            Assert.AreEqual(LagrangeAgent.actualLiving.MaxHealth, LagrangeAgent.actualLiving.CurrentHealth);


            // 扣血后血量不会为负数
            LagrangeAgent.ChangeHealth(-8000f);
            Assert.AreEqual(0f, LagrangeAgent.actualLiving.CurrentHealth);

            LogAssert.ignoreFailingMessages = false;
            yield return null;
        }
        
    }
}
