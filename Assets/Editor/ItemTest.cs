using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ItemTest
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
        public IEnumerator HealthItemTest()
        {
            LogAssert.ignoreFailingMessages = true;

            // 准备测试，先把血量扣到80
            MahouAgent.actualLiving.State.ClearStatus();
            Assert.IsFalse(MahouAgent.actualLiving.State.HasStatus(new InvincibleState()));
            MahouAgent.ChangeHealth(-20f);
            Assert.AreEqual(80f, MahouAgent.actualLiving.CurrentHealth);
            MahouAgent.actualLiving.State.ClearStatus();
            Assert.IsFalse(MahouAgent.actualLiving.State.HasStatus(new InvincibleState()));

            MahouAgent.CleanInventory();

            // 测试使用血瓶，血瓶每次回复10%最大生命值，但使用完之后没有无敌
            MahouAgent.InventoryAddItem(new HealthPotion { });
            MahouAgent.UseItem(new HealthPotion { });
            Assert.AreEqual(90f, MahouAgent.actualLiving.CurrentHealth);
            Assert.IsFalse(MahouAgent.actualLiving.State.HasStatus(new InvincibleState()));

            // 血扣到20
            MahouAgent.ChangeHealth(-70f);
            MahouAgent.actualLiving.State.ClearStatus();

            // 测试使用血包，血瓶每次回复70%最大生命值，但使用完之后没有无敌
            MahouAgent.InventoryAddItem(new Medkit { });
            MahouAgent.UseItem(new Medkit { });
            Assert.AreEqual(90f, MahouAgent.actualLiving.CurrentHealth);
            Assert.IsFalse(MahouAgent.actualLiving.State.HasStatus(new InvincibleState()));

            // 90点血再使用血包，血量也不会超过100
            MahouAgent.InventoryAddItem(new Medkit { });
            MahouAgent.UseItem(new Medkit { });
            Assert.AreEqual(100f, MahouAgent.actualLiving.CurrentHealth);

            yield return null;
            LogAssert.ignoreFailingMessages = false;

        }

        [UnityTest]
        public IEnumerator BuffItemTest()
        {
            LogAssert.ignoreFailingMessages = true;

            // 先去除人物初始化时的无敌状态
            MahouAgent.BuffColumnAddItem(new InvincibleItem { });
            MahouAgent.actualLiving.State.ClearStatus();
            Assert.IsFalse(MahouAgent.actualLiving.State.HasStatus(new InvincibleState()));

            // 测试无敌物品是否能给人物无敌，且无敌时间是无穷
            MahouAgent.UseBuffItem(new InvincibleItem { });
            Assert.IsTrue(MahouAgent.actualLiving.State.HasStatus(new InvincibleState()));
            Assert.IsTrue(float.IsNaN(MahouAgent.actualLiving.State.StateDuration[new InvincibleState()]));

            MahouAgent.actualLiving.State.ClearStatus();

            yield return null;
            LogAssert.ignoreFailingMessages = false;
        }

        [UnityTest]
        public IEnumerator StrengthItemTest()
        {
            LogAssert.ignoreFailingMessages = true;

            // 初始人物攻击力应该为1
            Assert.AreEqual(1, MahouAgent.actualLiving.AttackAmount);

            MahouAgent.CleanInventory();
            MahouAgent.InventoryAddItem(new StrengthPotion { });

            // 使用力量药后，攻击力应该为1.2
            MahouAgent.UseItem(new StrengthPotion { });
            Assert.IsTrue(Mathf.Approximately(1.2f, MahouAgent.actualLiving.AttackAmount));

            yield return null;
            LogAssert.ignoreFailingMessages = false;
        }
        

        [UnityTest]
        [Description("测试物品生成函数生成的物品的位置和数量是否正确")]
        public IEnumerator ItemGenerateTest()
        {
            ItemAgent.GenerateItem(new Vector3(5, 5), new HealthPotion { Amount = 10 });

            var item = GameObject.Find("HealthPotion(Clone)");
            Assert.AreEqual(new Vector3(5, 5, 0), item.transform.position);
            Assert.AreEqual(10, item.GetComponent<ItemAgent>().Item.Amount);
            yield return null;
        }

    }
}
