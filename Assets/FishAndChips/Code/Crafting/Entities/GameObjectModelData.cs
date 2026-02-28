using System;

namespace FishAndChips
{
	/// <summary>
	/// Base visual data related to a asset.
	/// </summary>
    [Serializable]
    public class GameObjectModelData : ScriptableObjectData
    {
		#region -- Inspector --
		public string DisplayName;
		#endregion
	}
}
