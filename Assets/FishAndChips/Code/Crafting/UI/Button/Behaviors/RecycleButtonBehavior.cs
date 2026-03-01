namespace FishAndChips
{
    public class RecycleButtonBehavior : FishScript, IButtonBehavior
    {
		#region -- Public Methods --
		public void OnClick()
		{
			EventManager.TriggerEvent<RecycleTriggerableEvent>(new RecycleTriggerableEvent());

			string toastMessage = "Board Cleared!";
			CraftingSystemCraftingService craftingService = CraftingSystemCraftingService.Instance;
			if (craftingService != null && craftingService.GameplayBoard != null)
			{
				if (craftingService.GameplayBoard.RecycleState == SimpleGameplayBoard.eRecycleState.CleanState)
				{
					toastMessage = "Undone!";
				}
			}
			EventManager.TriggerEvent(new ToastEvent(toastMessage));
		}
		#endregion
	}
}
