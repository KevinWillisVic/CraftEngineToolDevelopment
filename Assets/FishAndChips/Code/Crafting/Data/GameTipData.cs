#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace FishAndChips
{
	/// <summary>
	/// Represents a tip that can be presented to the player on how the game functions.
	/// </summary>
    public class GameTipData : ScriptableObjectData
    {
		#region -- Inspector --
		[Tooltip("Title or display name of tip.")]
		public string TipTitle;
		[Tooltip("Body message of tip.")]
		public string Tip;
		[Tooltip("Is it a tip that would shown on the root of the EncyclopediaView.")]
		public bool IsDefaultTip = false;
		#endregion

#if UNITY_EDITOR
		[MenuItem("Assets/Create/FishAndChips/GameData/GameTipData")]
		public static void CreateAsset()
		{
			var data = ScriptableObjectUtility.CreateAsset<GameTipData>("Assets/FishAndChips/Data/Crafting/ScriptableData/Resources/Tips");
		}
#endif
	}
}
