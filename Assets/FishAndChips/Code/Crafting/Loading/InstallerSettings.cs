using System;

namespace FishAndChips
{
	/// <summary>
	/// These are a collecting of prefabs that will be spawned during the game initialization.
	/// </summary>
	[Serializable]
	public class InstallerSettings
    {
		#region -- Inspector --
		public UICanvas UICanvasPrefab;
		public CraftingSystemCameraService CameraServicePrefab;
		public CraftingSystemPoolingService PoolingServicePrefab;
		#endregion
	}
}
