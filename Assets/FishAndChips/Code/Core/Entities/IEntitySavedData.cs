namespace FishAndChips
{
    public interface IEntitySavedData : IEntity
    {
        ISavedData SavedData { get; set; }
    }
}
