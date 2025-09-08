#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.Events;

namespace FishAndChips
{
    public class DebugKeyPressResponse : MonoBehaviour
    {
		#region -- Inspector --
		public KeyCode TriggerKey = KeyCode.A;
		public UnityEvent PressAction;
		#endregion

		#region -- Private Methods --
		private void Update()
		{
			if (Input.GetKeyDown(TriggerKey) == true)
			{
				PressAction.FireSafe();
			}
		}
		#endregion
	}
}
#endif
