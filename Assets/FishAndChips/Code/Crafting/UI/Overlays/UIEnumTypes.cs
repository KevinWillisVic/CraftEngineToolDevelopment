namespace FishAndChips
{
	// Enumerations for specific UI concepts such as view types, and overlay types.
    public class UIEnumTypes
    {
		#region -- Supporting --
		public enum eViewType
		{
			None = -1,
			BootView,
			GameplaySceneView,
			GameplayUnlockView,
			EncyclopediaView,
		}

		public enum eOverlayType
		{
			None = -1,
			OverlayLoadingProgressBar,
			OverlayDisableUIInput,
			OverlayHint,
			OverlaySettings,
		}
		#endregion
	}
}
