namespace FishAndChips
{
    public interface IMetadataAsset<out TData> where TData : IMetaDataStaticData
    {
        TData Data { get; }

#if UNITY_EDITOR
        void OnImport();
#endif
    }
}
