namespace FishAndChips
{
    public class ResetGameButtonBehavior : FishScript, IButtonBehavior
    {
		#region -- Protected Member Vars --
		protected UIService _uiService;
		#endregion

		#region -- Protected Methods --
		protected override void Start()
		{
			base.Start();
			_uiService = UIService.Instance;
		}
		#endregion

		#region -- Public Methods --
		public void OnClick()
		{
			var yesNoOverlay = _uiService.ShowOverlay<OverlayYesNo>("OverlayYesNo");

			// Configure yes no overlay.
			yesNoOverlay.Initialize(title: "Are You Sure?",
				description: "This will delete all your saved data and you will be on a fresh game.");

			yesNoOverlay.SetButtonText("Yes", "No");

			// User must select yes in order to reset the game.
			yesNoOverlay.YesSelected += o =>
			{
				EventManager.TriggerEvent(new ToastEvent("Fresh Game!"));
				// Reset Game.
				EventManager.TriggerEvent(new GameResetEvent());
			};

		}
		#endregion
	}
}
