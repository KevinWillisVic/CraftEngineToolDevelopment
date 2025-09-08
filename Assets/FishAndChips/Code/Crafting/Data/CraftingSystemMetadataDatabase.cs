using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FishAndChips
{
	[CreateAssetMenu(menuName = "FishAndChips/Metadata/Database")]
	public class CraftingSystemMetadataDatabase : MetadataDatabase
    {
		#region -- Private Member Vars --
		[SerializeField] private ScriptableObject[] _craftItemData;
		[SerializeField] private ScriptableObject[] _craftRecipeData;
		#endregion

		#region -- Properties --
		public override IMetaDataStaticData[] StaticData
		{
			get
			{
				var results = base.StaticData.ToList();
				results.AddRange(FetchStaticData<CraftItemData>(_craftItemData));
				results.AddRange(FetchStaticData<CraftRecipeData>(_craftRecipeData));
				return results.ToArray();
			}
		}
		#endregion

		#region -- Public Methods --
#if UNITY_EDITOR
		public override void Fill()
		{
			base.Fill();
			_craftItemData = ScriptableObjectUtility.GetAssetsWithType<CraftItemScriptableData>();
			_craftRecipeData = ScriptableObjectUtility.GetAssetsWithType<CraftRecipeScriptableData>();
			EditorUtility.SetDirty(this);
		}
#endif
#endregion
	}
}
