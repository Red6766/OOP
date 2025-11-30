using System;
using Xunit;
using InventorySystem.Core.Enums;
using InventorySystem.Items;

namespace InventorySystem
{
    public class Tests
    {
        private readonly Inventory _inventory;
        private readonly Builder _builder;

        public Tests()
        {
            _inventory = new Inventory(5);
            _builder = new Builder();
        }

        [Fact]
        public void AddItem_ValidItem_ShouldAddSuccessfully()
        {
            var sword = _builder.WithName("Test Sword")
                              .WithType(ItemType.Weapon)
                              .WithDamage(10)
                              .Build();

            var result = _inventory.AddItem(sword);

            Assert.True(result);
            Assert.Single(_inventory.Items);
            Assert.Equal(sword.Id, _inventory.Items[0].Id);
        }

        [Fact]
        public void AddItem_DuplicateId_ShouldReturnFalse()
        {
            var sword = _builder.WithId("same-id")
                              .WithName("Sword")
                              .WithType(ItemType.Weapon)
                              .WithDamage(10)
                              .Build();

            var armor = _builder.WithId("same-id")
                              .WithName("Armor")
                              .WithType(ItemType.Armor)
                              .WithDefense(5)
                              .Build();

            _inventory.AddItem(sword);
            var result = _inventory.AddItem(armor);

            Assert.False(result);
            Assert.Single(_inventory.Items);
        }

        [Fact]
        public void AddItem_WhenInventoryFull_ShouldReturnFalse()
        {
            var inventory = new Inventory(1); 
            var sword = _builder.WithName("Sword").WithType(ItemType.Weapon).WithDamage(10).Build();
            var armor = _builder.WithName("Armor").WithType(ItemType.Armor).WithDefense(5).Build();

            inventory.AddItem(sword);
            var result = inventory.AddItem(armor);

            Assert.False(result);
            Assert.Single(inventory.Items);
        }

        [Fact]
        public void RemoveItem_ExistingItem_ShouldRemoveSuccessfully()
        {
            var sword = _builder.WithName("Sword").WithType(ItemType.Weapon).WithDamage(10).Build();
            _inventory.AddItem(sword);

            var result = _inventory.RemoveItem(sword.Id);

            Assert.True(result);
            Assert.Empty(_inventory.Items);
        }

        [Fact]
        public void RemoveItem_NonExistingItem_ShouldReturnFalse()
        {
            var result = _inventory.RemoveItem("non-existing-id");

            Assert.False(result);
        }

        [Fact]
        public void UseItem_UsableItem_ShouldUseSuccessfully()
        {
            var potion = _builder.WithName("Potion")
                               .WithType(ItemType.Potion)
                               .WithHealingPower(20)
                               .Build();
            _inventory.AddItem(potion);

            var result = _inventory.UseItem(potion.Id);

            Assert.True(result);
        }

        [Fact]
        public void UseItem_NonUsableItem_ShouldReturnFalse()
        {
            var questItem = _builder.WithName("Quest")
                                  .WithType(ItemType.QuestItem)
                                  .Build();
            _inventory.AddItem(questItem);
            _inventory.UseItem(questItem.Id);

            var result = _inventory.UseItem(questItem.Id);

            Assert.False(result);
        }

        [Fact]
        public void UpgradeItem_UpgradableItem_ShouldUpgradeSuccessfully()
        {
            var sword = _builder.WithName("Sword")
                              .WithType(ItemType.Weapon)
                              .WithDamage(10)
                              .Build();
            _inventory.AddItem(sword);
            var weapon = _inventory.GetItem(sword.Id) as Weapon;

            var result = _inventory.UpgradeItem(sword.Id);

            Assert.True(result);
            Assert.Equal(2, weapon.Level);
            Assert.Equal(12, weapon.Damage);
        }

        [Fact]
        public void UpgradeItem_NonUpgradableItem_ShouldReturnFalse()
        {
            var potion = _builder.WithName("Potion")
                               .WithType(ItemType.Potion)
                               .WithHealingPower(20)
                               .Build();
            _inventory.AddItem(potion);

            var result = _inventory.UpgradeItem(potion.Id);

            Assert.False(result);
        }

        [Fact]
        public void GetItemsByType_ShouldReturnCorrectItems()
        {
            var sword = _builder.WithName("Sword").WithType(ItemType.Weapon).WithDamage(10).Build();
            var armor = _builder.WithName("Armor").WithType(ItemType.Armor).WithDefense(5).Build();
            _inventory.AddItem(sword);
            _inventory.AddItem(armor);

            var weapons = _inventory.GetItemsByType(ItemType.Weapon);
            var armors = _inventory.GetItemsByType(ItemType.Armor);

            Assert.Single(weapons);
            Assert.Single(armors);
            Assert.Equal(sword.Id, weapons.First().Id);
            Assert.Equal(armor.Id, armors.First().Id);
        }

        [Fact]
        public void GetItemsByRarity_ShouldReturnCorrectItems()
        {
            var commonSword = _builder.WithName("Common Sword")
                                    .WithType(ItemType.Weapon)
                                    .WithDamage(10)
                                    .WithRarity(ItemRarity.Common)
                                    .Build();

            var rareSword = _builder.WithName("Rare Sword")
                                  .WithType(ItemType.Weapon)
                                  .WithDamage(15)
                                  .WithRarity(ItemRarity.Rare)
                                  .Build();
            _inventory.AddItem(commonSword);
            _inventory.AddItem(rareSword);

            var rareItems = _inventory.GetItemsByRarity(ItemRarity.Rare);

            Assert.Single(rareItems);
            Assert.Equal(rareSword.Id, rareItems.First().Id);
        }

        [Fact]
        public void Clear_ShouldRemoveAllItems()
        {
            var sword = _builder.WithName("Sword").WithType(ItemType.Weapon).WithDamage(10).Build();
            var armor = _builder.WithName("Armor").WithType(ItemType.Armor).WithDefense(5).Build();
            _inventory.AddItem(sword);
            _inventory.AddItem(armor);

            _inventory.Clear();

            Assert.Empty(_inventory.Items);
        }

        [Fact]
        public void GetInventoryInfo_ShouldReturnCorrectInformation()
        {
            var sword = _builder.WithName("Sword").WithType(ItemType.Weapon).WithDamage(10).Build();
            _inventory.AddItem(sword);

            var info = _inventory.GetInventoryInfo();

            Assert.Contains("1/5", info);
            Assert.Contains("Weapon: 1", info);
        }
    }
}