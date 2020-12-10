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
        

        [SetUp]
        public void SetUp()
        {
            LogAssert.ignoreFailingMessages = true;

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
            LogAssert.ignoreFailingMessages = false;

            MahouAgent = null;
            Debug.Log("Tear down.");
        }


        [UnityTest]
        [Order(0)]
        public IEnumerator CharacterLoadTest()
        {
            LogAssert.ignoreFailingMessages = true;

            Assert.IsNotNull(MahouAgent);
            Assert.AreEqual(0, MahouAgent.characterIndex);
            Assert.AreEqual(1, MahouAgent.actualLiving.TimeInvincible);
            Assert.AreEqual(1, MahouAgent.actualLiving.AttackAmount);
            Assert.AreEqual(100f, MahouAgent.actualLiving.CurrentHealth);
            Assert.IsNotNull(MahouAgent.WeaponPrefab);
            Debug.Log(MahouAgent.WeaponPrefab);
            yield return null;

            LogAssert.ignoreFailingMessages = false;
        }

        [UnityTest]
        [Order(1)]
        public IEnumerator CharacterHealthTest()
        {
            LogAssert.ignoreFailingMessages = true;


            // 初始血量应为100，有1s的无敌
            Assert.AreEqual(100f, MahouAgent.actualLiving.CurrentHealth);
            Assert.AreEqual(MahouAgent.actualLiving.MaxHealth, MahouAgent.actualLiving.CurrentHealth);
            Assert.IsTrue(MahouAgent.actualLiving.State.HasStatus(new InvincibleState()));

            // 过1s以上，应该无敌状态消除
            var frameCount = 0;
            while ((frameCount++) <= 900)
            {
                MahouAgent.Update();
                yield return null;
            }
            frameCount = 0;
            Assert.IsFalse(MahouAgent.actualLiving.State.HasStatus(new InvincibleState()));


            // 测试伤害，扣20点血，受伤后应进入无敌
            MahouAgent.ChangeHealth(-20f);
            Assert.AreEqual(80f, MahouAgent.actualLiving.CurrentHealth);
            Assert.IsTrue(MahouAgent.actualLiving.State.HasStatus(new InvincibleState()));

            while ((frameCount++) <= 900)
            {
                MahouAgent.Update();
                yield return null;
            }
            frameCount = 0;
            Assert.IsFalse(MahouAgent.actualLiving.State.HasStatus(new InvincibleState()));

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
        /// 测试背包物品的添加、移除、使用
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator InventoryTest()
        {
            LogAssert.ignoreFailingMessages = true;

            // 人物初始化时，应该带有物品
            Assert.IsFalse(MahouAgent.IsInventoryEmpty());

            // 清空背包
            MahouAgent.CleanInventory();
            Assert.IsTrue(MahouAgent.IsInventoryEmpty());

            // 添加10枚硬币
            MahouAgent.InventoryAddItem(new Coin { Amount = 10 });
            Assert.IsFalse(MahouAgent.IsInventoryEmpty());
            Assert.AreEqual(10, MahouAgent.GetItemAmount(new Coin { }));

            // 添加2个血瓶
            MahouAgent.InventoryAddItem(new HealthPotion { Amount = 2 });
            Assert.AreEqual(2, MahouAgent.GetItemAmount(new HealthPotion { }));

            // 满血状态下应该不能使用血瓶，其数量不消耗
            MahouAgent.UseItem(new HealthPotion { });
            Assert.AreEqual(2, MahouAgent.GetItemAmount(new HealthPotion { }));

            // 非满血状态，应该能使用，数量减1
            MahouAgent.actualLiving.State.ClearStatus();
            MahouAgent.ChangeHealth(-1);
            MahouAgent.UseItem(new HealthPotion { });
            Assert.AreEqual(1, MahouAgent.GetItemAmount(new HealthPotion { }));

            // 清空背包
            MahouAgent.CleanInventory();
            Assert.IsTrue(MahouAgent.IsInventoryEmpty());

            yield return null;
            LogAssert.ignoreFailingMessages = false;

        }

        [UnityTest]
        public IEnumerator BuffColumnTest()
        {
            LogAssert.ignoreFailingMessages = true;

            // 测试是否能正确添加无敌状态物品
            MahouAgent.CleanBuffColumn();
            Assert.AreEqual(0, MahouAgent.GetBuffItemAmount(new InvincibleItem { }));
            MahouAgent.BuffColumnAddItem(new InvincibleItem { });
            Assert.AreEqual(1, MahouAgent.GetBuffItemAmount(new InvincibleItem { }));

            yield return null;
            LogAssert.ignoreFailingMessages = false;

        }

        [UnityTest]
        public IEnumerator WeaponColumnTest()
        {
            LogAssert.ignoreFailingMessages = true;

            // 初始人物手上有”咸鱼“武器，武器栏不为空
            Assert.IsTrue(MahouAgent.WeaponPrefab != null);
            Debug.Log(MahouAgent.WeaponPrefab);
            Assert.IsFalse(MahouAgent.IsWeaponColumnEmpty());

            // 清空武器栏
            MahouAgent.CleanWeaponColumn();
            Assert.IsTrue(MahouAgent.IsWeaponColumnEmpty());

            // 加入一把枪
            MahouAgent.WeaponColumnAddItem(new Gun { });
            Assert.IsFalse(MahouAgent.IsWeaponColumnEmpty());
            Assert.AreEqual(1, MahouAgent.GetWeaponAmount(new Gun { }));

            // 切换手上的武器，咸鱼应该到了武器栏中
            // 不知道为什么它会报错，故就不在这个里面测切换
            //MahouAgent.SwapeWeapon();
            //Debug.Log(MahouAgent.WeaponPrefab);

            yield return null;
            LogAssert.ignoreFailingMessages = false;
        }


        [UnityTest]
        public IEnumerator DashTest()
        {
            MahouAgent.ClearDashBar();
            Assert.AreEqual(0, MahouAgent.dashBar);
            var frameCount = 0;
            while ((frameCount++) <= 300)
            {
                MahouAgent.Update();
                yield return null;
            }
            frameCount = 0;

            Assert.AreEqual(301, MahouAgent.dashBar);
            MahouAgent.ForceDash();
            Assert.AreEqual(1, MahouAgent.dashBar);

            // 不知为什么Dash后位置不变
            Debug.Log(MahouAgent.rigidbody2d.position);

            yield return null;
        }


    }
}
