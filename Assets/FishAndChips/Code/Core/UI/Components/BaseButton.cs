using UnityEngine;
using UnityEngine.UI;

namespace FishAndChips
{
    public class BaseButton : Button
    {
		#region -- Protected Member Vars --
		protected AudioService _audioService;
		#endregion

		#region -- Protected Methods --
		protected override void Awake()
		{
			base.Awake();
			_audioService = AudioService.Instance;
			if (Application.isPlaying == true)
			{
				onClick.AddListener(OnButtonClicked);
			}
		}

		protected override void OnDestroy()
		{
			onClick.RemoveListener(OnButtonClicked);
			base.OnDestroy();
		}
		#endregion

		#region -- Public Methods --
		public virtual void OnButtonClicked()
		{
			// TODO : Handle audio.
		}
		#endregion
	}
}
