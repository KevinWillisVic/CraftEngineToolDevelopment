using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FishAndChips
{
	/// <summary>
	/// Main view for the gameplay.
	/// </summary>
    public class GameplaySceneView : GameView
    {
		#region -- Inspector --
		[Header("Displayed Craft Items")]
		public CraftItemComponentList DisplayedCraftItems;

		[Header("Gameboard")]
		public SimpleGameplayBoard SimpleGameplayBoard;

		[Header("Buttons")]
		public Button HintButton;
		public Button SettingsButton;
		public Button ClearButton;
		public Button EncyclopediaButton;

		public RecycleButtonBehavior RecycleButtonBehavior;
		#endregion

		#region -- Protected Member Vars --
		protected CraftingSystemHintService _hintService;
		protected CraftingSystemCraftingService _craftingService;
		#endregion

		#region -- Private Member Vars --
		private string _lastSearch;
		private List<CraftItemEntity> _craftItemEntities = new();
		#endregion

		#region -- Private Methods --
		/// <summary>
		/// Handle what happens when the game is reset.
		/// </summary>
		private void OnResetGame(GameResetEvent resetEvent)
		{
			// Remove all search filters.
			FilterDisplayedItems(string.Empty);
			UpdateVisibleStateOfHintButton();
		}

		/// <summary>
		/// Handle what happens when a CraftItem has been unlocked.
		/// </summary>
		private void OnCraftItemUnlocked(CraftItemEntityUnlockEvent gameEvent)
		{
			// Re-do last search to potentially get rid of any depleted items.
			FilterDisplayedItems(_lastSearch);
			UpdateVisibleStateOfHintButton();
		}

		/// <summary>
		/// Handler for search raised events.
		/// </summary>
		private void OnSearchRaised(CraftItemSearchEvent gameEvent)
		{
			FilterDisplayedItems(gameEvent.SearchFilter);
		}

		/// <summary>
		/// Handle start up of the game.
		/// </summary>
		private void OnSceneReady(OnGameSceneReady gameEvent)
		{
			FilterDisplayedItems(string.Empty);
			UpdateVisibleStateOfHintButton();
		}

		/// <summary>
		/// Determine if CraftItem should be displayed.
		/// </summary>
		/// <param name="entity">CraftItem being checked.</param>
		/// <returns>True if should be displayed, false otherwise.</returns>
		private bool IsCraftItemValidForDisplaying(CraftItemEntity entity)
		{
			return _craftingService.IsCraftItemValidForDisplaying(entity, _lastSearch);
		}

		/// <summary>
		/// Determine if hint button should be displayed.
		/// </summary>
		private void UpdateVisibleStateOfHintButton()
		{
			HintButton.SetActiveSafe(_hintService.HasHintAvailable());
		}
		#endregion

		#region -- Protected Methods --
		/// <summary>
		/// Handle event subscribtion.
		/// </summary>
		protected override void SubscribeListeners()
		{
			base.SubscribeListeners();
			EventManager.SubscribeEventListener<GameResetEvent>(OnResetGame);
			EventManager.SubscribeEventListener<OnGameSceneReady>(OnSceneReady);
			EventManager.SubscribeEventListener<CraftItemSearchEvent>(OnSearchRaised);
			EventManager.SubscribeEventListener<CraftItemEntityUnlockEvent>(OnCraftItemUnlocked);
		}

		/// <summary>
		/// Handle event unsubscription.
		/// </summary>
		protected override void UnsubsribeListeners()
		{
			base.UnsubsribeListeners();
			EventManager.UnsubscribeEventListener<GameResetEvent>(OnResetGame);
			EventManager.UnsubscribeEventListener<OnGameSceneReady>(OnSceneReady);
			EventManager.UnsubscribeEventListener<CraftItemSearchEvent>(OnSearchRaised);
			EventManager.SubscribeEventListener<CraftItemEntityUnlockEvent>(OnCraftItemUnlocked);
		}

		protected override void SetupButtons()
		{
			if (HintButton != null)
			{
				HintButton.onClick.AddListener(HandleHitHintButton);
			}

			if (SettingsButton != null)
			{
				SettingsButton.onClick.AddListener(HandleHitSettingsButton);
			}

			if (ClearButton != null)
			{
				ClearButton.onClick.AddListener(HandleHitRecycleButton);
			}

			if (EncyclopediaButton != null)
			{
				EncyclopediaButton.onClick.AddListener(HandleHitEncyclopediaButton);
			}
		}
		#endregion

		#region -- Public Methods --
		/// <summary>
		/// Setup view.
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();
			// Services.
			_hintService = CraftingSystemHintService.Instance;
			_craftingService = CraftingSystemCraftingService.Instance;

			SimpleGameplayBoard.Setup();
		}

		/// <summary>
		/// Set up list of CraftItems to display with supplied search filter.
		/// </summary>
		/// <param name="filter">Search filter to limit displayed CraftItems.</param>
		public void FilterDisplayedItems(string filter)
		{
			_lastSearch = filter;
			if (DisplayedCraftItems == null)
			{
				return;
			}

			_craftItemEntities.Clear();
			_craftItemEntities.AddRange(_craftingService.CraftItemEntities);

			// Filter.
			_craftItemEntities.RemoveAll(c => IsCraftItemValidForDisplaying(c) == false);
			DisplayedCraftItems.FillList(_craftItemEntities);
		}

		/// <summary>
		/// Show the settings overlay.
		/// </summary>
		public void HandleHitSettingsButton()
		{
			var settingsOverlay = _uiService.ShowOverlay<OverlaySettings>(UIEnumTypesBase.eOverlayTypeBase.OverlaySettings.ToString(), "Settings");
		}

		/// <summary>
		/// Show the enccylopedia view.
		/// </summary>
		public void HandleHitEncyclopediaButton()
		{
			_uiService.ActivateView(UIEnumTypes.eViewType.EncyclopediaView.ToString());
		}

		/// <summary>
		/// Click callback for hint button.
		/// </summary>
		public void HandleHitHintButton()
		{
			if (_hintService.HasHintAvailable() == true)
			{
				var craftItemEntity = _hintService.GetCraftItemEntityAsHint();
				// Give CraftItem as hint.
				if (craftItemEntity != null)
				{
					// For keyword tracking.
					craftItemEntity.SetHintState(true);

					// Create overlay indicating this item has a hint given.
					var hintOverlay = _uiService.ShowOverlay<OverlayHint>(UIEnumTypes.eOverlayType.OverlayHint.ToString());
					if (hintOverlay != null)
					{
						hintOverlay.Initialize(craftItemEntity);
					}
				}
				else
				{
					// TODO : See about recipe hint.
				}
			}
			else
			{
				UpdateVisibleStateOfHintButton();
			}
		}

		/// <summary>
		/// Handle hitting the recycle button.
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
