namespace FishAndChips
{
    public static class CraftingEnums
    {
		#region -- Enumerations --
		public enum eCraftItemAnimationKeys
		{
			None,
			InvalidCombo,
			CloneAppear,
			SpawnFromScrollRect
		}

		public enum eEncyclopediaViewMode
		{
			None = -1,
			Home = 0,
			Items = 1,
			Stats = 2,
			Tips = 3
		}

		// Enum for CraftItem keywords in the game.
		public enum eCraftItemKeyword
		{
			Basic,
			// Temporary / On the fly.
			Hint,
			// Temporary / On the fly.
			Depleted
		}
		#endregion
	}
}
