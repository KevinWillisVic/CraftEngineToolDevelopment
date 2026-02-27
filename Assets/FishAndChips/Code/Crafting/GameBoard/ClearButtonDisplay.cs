using TMPro;

namespace FishAndChips
{
	/// <summary>
	/// Handle updating the text of the clear button.
	/// </summary>
    public class ClearButtonDisplay : FishScript
    {
		#region -- Inspector --
		public TextMeshProUGUI ButtonText;
		#endregion

		#region -- Private Methods --
		private void OnEnable()
		{
			SetTextBasedOnState(CraftingSystemCraftingService.Instance.CurrentRecycleState);
		}

		/// <summary>
		/// Event handler for changes to recycle state.
		/// </summary>
		/// <param name="gameEvent">Event for recycle state.</param>
		private void OnButtonRecycleStateChanged(RecycleStateUpdateEvent gameEvent)
		{
			SetTextBasedOnState(gameEvent.RecycleState);
		}

		/// <summary>
		/// Set the text based on the recycle state.
		/// </summary>
		/// <param name="state">The state to base the text on.</param>
		private void SetTextBasedOnState(SimpleGameplayBoard.eRecycleState state)
		{
			switch (state)
			{
				case SimpleGameplayBoard.eRecycleState.CleanState:
					{
						ButtonText.SetTextSafe("Clear");
						break;
					}
				case SimpleGameplayBoard.eRecycleState.UndoState:
					{
						ButtonText.SetTextSafe("Undo Clear");
						break;
					}
			}
		}
		#endregion

		#region -- Protected Methods --
		/// <summary>
		/// Subscribe to events.
		/// </summary>
		protected override void SubscribeEventListeners()
		{
			base.SubscribeEventListeners();
			EventManager.SubscribeEventListener<RecycleStateUpdateEvent>(OnButtonRecycleStateChanged);
		}

		/// <summary>
		/// Unsubscribe from events.
		/// </summary>
		protected override void UnsubscribeEventListeners()
		{
			base.UnsubscribeEventListeners();
			EventManager.UnsubscribeEventListener<RecycleStateUpdateEvent>(OnButtonRecycleStateChanged);
		}
		#endregion
	}
}
