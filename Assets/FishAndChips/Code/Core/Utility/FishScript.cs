using UnityEngine;

namespace FishAndChips
{
	/// <summary>
	/// Base monobheviour for project.
	/// </summary>
    public class FishScript : MonoBehaviour
    {
		#region -- Protected Methods --
		protected virtual void Start()
		{
			SubscribeEventListeners();
		}

		protected virtual void OnDestroy()
		{
			UnsubscribeEventListeners();
		}

		protected virtual void SubscribeEventListeners()
		{
		}

		protected virtual void UnsubscribeEventListeners()
		{
		}
		#endregion
	}
}
