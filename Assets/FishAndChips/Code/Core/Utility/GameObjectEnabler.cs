using UnityEngine;
using System.Collections.Generic;

namespace FishAndChips
{
    public class GameObjectEnabler : MonoBehaviour
    {
		#region -- Inspector --
		public string Key;

		public List<GameObject> ToEnableOnActive;
		public List<GameObject> ToEnableOnInactive;
		#endregion

		#region -- Public Methods --
		public void SetEnabled(bool value)
		{
			if (this == null || gameObject == null)
			{
				return;
			}

			foreach (var enableOnActive in ToEnableOnActive)
			{
				enableOnActive.SetActiveSafe(value);
			}

			foreach (var enableOnInactive in ToEnableOnInactive)
			{
				enableOnInactive.SetActiveSafe(!value);
			}
		}
		#endregion
	}
}
