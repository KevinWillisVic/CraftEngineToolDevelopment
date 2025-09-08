using UnityEngine;

namespace FishAndChips
{
    public class CraftingSystemCameraService : Singleton<CraftingSystemCameraService>
    {
		#region -- Inspector --
		public Camera MainCamera;
		#endregion

		#region -- Public Methods --
		public void JumpToPosition(Vector3 position)
		{
			if (MainCamera == null)
			{
				return;
			}
			MainCamera.transform.position = position;
		}
		#endregion
	}
}
