using UnityEngine;
using UnityEngine.Playables;
using TMPro;

namespace FishAndChips
{
    public class MessageToast : MonoBehaviour
    {
		#region -- Inspector --
		public TextMeshProUGUI ToastText;
		public PlayableDirector ToastTrack;
		#endregion

		#region -- Private Methods --
		private void Awake()
		{
			EventManager.SubscribeEventListener<ToastEvent>(OnToastEvent);
		}

		private void OnDestroy()
		{
			EventManager.UnsubscribeEventListener<ToastEvent>(OnToastEvent);
		}

		private void OnToastEvent(ToastEvent gameEvent)
		{
			if (gameEvent == null)
			{
				return;
			}
			DisplayToast(gameEvent.Message);
		}
		#endregion

		#region -- Public Methods --
		public void DisplayToast(string message)
		{
			ToastText.SetTextSafe(message);
			ToastTrack.PlaySafe();
		}
		#endregion
	}
}
