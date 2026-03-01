using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace FishAndChips
{
    public class OverlaySettings : GameOverlay
    {
		#region -- Inspector --
		[Header("Overlay Settings")]
		[Header("Buttons")]
		public Button ClearButton;
		public Button ResetGameButton;
		public RecycleButtonBehavior RecycleButtonBehavior;
		public ResetGameButtonBehavior ResetGameButtonBehavior;

		[Header("Toggle Group Buttons")]
		public List<Toggle> SFXToggles = new();
		public List<Toggle> MusicToggles = new();
		#endregion

		#region -- Protected Member Vars --
		protected CraftingSystemSavingService _savingService;
		#endregion

		#region -- Private Methods --
		private void SetUpSFXToggles()
		{
			foreach (var toggle in SFXToggles)
			{
				if (toggle.name == "SFXToggleOn")
				{
					toggle.isOn = _savingService.GeneralSaveState.IsSFXOn;
				}
				else
				{
					toggle.isOn = !_savingService.GeneralSaveState.IsSFXOn;
				}
				toggle.onValueChanged.AddListener(state =>
				{
					if (state == true)
					{
						switch (toggle.name)
						{
							case "SFXToggleOn":
								_savingService.ToggleSFX(true);
								break;
							case "SFXToggleOff":
								_savingService.ToggleSFX(false);
								break;
						}
					}
				});
			}
		}

		private void SetUpMusicToggles()
		{
			foreach (var toggle in MusicToggles)
			{
				if (toggle.name == "MusicToggleOn")
				{
					toggle.isOn = _savingService.GeneralSaveState.IsMusicOn;
				}
				else
				{
					toggle.isOn = !_savingService.GeneralSaveState.IsMusicOn;
				}
				toggle.onValueChanged.AddListener(state =>
				{
					if (state == true)
					{
						switch (toggle.name)
						{
							case "MusicToggleOn":
								_savingService.ToggleMusic(true);
								break;
							case "MusicToggleOff":
								_savingService.ToggleMusic(false);
								break;
						}
					}
				});
			}
		}
		#endregion

		#region -- Protected Methods --
		/// <summary>
		/// Handle setting up button on click callbacks.
		/// </summary>
		protected override void SetupButtons()
		{
			base.SetupButtons();
			if (ClearButton != null)
			{
				ClearButton.onClick.AddListener(HandleHitRecycleButton);
			}

			if (ResetGameButton != null)
			{
				ResetGameButton.onClick.AddListener(ResetGame);
			}


			SetUpSFXToggles();
			SetUpMusicToggles();
		}
		#endregion

		#region -- Public Methods --
		public override void Initialize()
		{
			_savingService = CraftingSystemSavingService.Instance;
			base.Initialize();
		}

		/// <summary>
		/// Attempt to reset the game.
		/// </summary>
		public void ResetGame()
		{
			if (ResetGameButtonBehavior != null)
			{
				ResetGameButtonBehavior.OnClick();
			}
			DismissSelected();
		}

		/// <summary>
		/// Handle hitting on the recycle/clear button.
		/// </summary>
		public void HandleHitRecycleButton()
		{
			if (RecycleButtonBehavior != null)
			{
				RecycleButtonBehavior.OnClick();
			}
		}
		#endregion
	}
}
