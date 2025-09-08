namespace FishAndChips
{
    public interface IMetaData : IMetaDataStaticData
    {
        IEntity CreateEntity(string instanceId);
    }
}
