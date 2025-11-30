using InventorySystem.Core;
using InventorySystem.Core.Enums;
using InventorySystem.Core.Interfaces;

namespace InventorySystem.Items
{
    public class QuestItem : Item, IUsable
    {
        public bool IsQuestCompleted { get; private set; }

        public QuestItem(string id, string name) : base(id, name, ItemType.QuestItem, ItemRarity.Common, 0)
        {
            IsQuestCompleted = false;
        }

        public void CompleteQuest()
        {
            IsQuestCompleted = true;
        }

        public void Use()
        {
            if (!CanUse()) throw new InvalidOperationException("Cannot use quest item");
            IsQuestCompleted = true;
            Console.WriteLine($"Quest item {Name} used");
        }

        public bool CanUse() => !IsQuestCompleted;
    }
}