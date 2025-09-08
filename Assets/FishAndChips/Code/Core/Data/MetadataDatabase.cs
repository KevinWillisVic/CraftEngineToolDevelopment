using UnityEngine;
using System.Collections.Generic;
using System;

namespace FishAndChips
{
    public class MetadataDatabase : ScriptableObject
    {
		#region -- Properties --
        public virtual IMetaDataStaticData[] StaticData
        {
            get
            {
                var results = new List<IMetaDataStaticData>();
                return results.ToArray();
            }
        }
		#endregion

		#region -- Inspector --
		[SerializeField] private string _contentID;
        #endregion

        #region -- Protected Methods --
        protected virtual TStaticData[] FetchStaticData<TStaticData>(ScriptableObject[] scriptableObjects) where TStaticData : IMetaDataStaticData
        {
            if (scriptableObjects == null)
            {
                return Array.Empty<TStaticData>();
            }

            var dataArrary = new List<TStaticData>();
            int length = scriptableObjects.Length;
            for (int i = 0; i < length; i++)
            {
                var obj = scriptableObjects[i];
                if (obj is IMetadataAsset<TStaticData> asset)
                {
                    dataArrary.Add(asset.Data);
                }
            }
            return dataArrary.ToArray();
        }
        #endregion

        #region -- Public Methods --
        public virtual void Fill()
        {
        }
        #endregion
    }
}
