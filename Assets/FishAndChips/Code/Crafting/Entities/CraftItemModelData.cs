using System;
using UnityEngine;

namespace FishAndChips
{
	[CreateAssetMenu(menuName = "FishAndChips/Models/CraftItemModelData")]
	[Serializable]
	public class CraftItemModelData : GameObjectModelData
    {
		#region -- Inspector --
		[Tooltip("The ID of its sprite.")]
		public string VisualKey;
		[Tooltip("Small sentence about the item.")]
		public string Blurb;
		#endregion
	}
}
