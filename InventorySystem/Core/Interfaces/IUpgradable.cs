namespace InventorySystem.Core.Interfaces
{
    public interface IUpgradable
    {
        int Level { get; }
        int MaxLevel { get; }
        void Upgrade();
        bool CanUpgrade();
    }
}