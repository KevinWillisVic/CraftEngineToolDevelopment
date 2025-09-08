using UnityEngine;

namespace FishAndChips
{
	public class CraftingSystemSavingService : SavingService, IInitializable
	{
		#region -- Properties --
		public static new CraftingSystemSavingService Instance
		{
			get
			{
				lock (_lock)
				{
					if (_instance == null)
					{
						_instance = (CraftingSystemSavingService)Object.FindAnyObjectByType(typeof(CraftingSystemSavingService));

						if (_instance == null)
						{
							GameObject singletonObject = new GameObject();
							_instance = singletonObject.AddComponent<CraftingSystemSavingService>();
							singletonObject.name = $"Singleton {typeof(CraftingSystemSavingService).ToString()}";
							DontDestroyOnLoad(singletonObject);
						}
					}
					return _instance as CraftingSystemSavingService;
				}
			}
		}

		public BoardSaveInfo BoardSaveState => _boardSaveState;
		#endregion

		#region -- Protected Member Vars --
		protected CraftingSystemCraftingService _craftingService;
		#endregion

		#region -- Private Member Vars --
		private BoardSaveInfo _boardSaveState;
		#endregion

		#region -- Private Methods --
		private void SubscribeEventListeners()
		{
			// Board saving.
			EventManager.SubscribeEventListener<GeneralPositionSaveEvent>(OnGeneralSaveBoard);
			EventManager.SubscribeEventListener<PositionSaveObjectAddedEvent>(OnObjectAddedToBoard);
			EventManager.SubscribeEventListener<PositionSaveObjectRemovedEvent>(OnObjectRemovedFromBoard);

			// Life-cycle.
			EventManager.SubscribeEventListener<GameResetEvent>(OnGameReset);
		}

		private void UnsubscribeEventListeners()
		{
			// Board saving.
			EventManager.UnsubscribeEventListener<GeneralPositionSaveEvent>(OnGeneralSaveBoard);
			EventManager.UnsubscribeEventListener<PositionSaveObjectAddedEvent>(OnObjectAddedToBoard);
			EventManager.UnsubscribeEventListener<PositionSaveObjectRemovedEvent>(OnObjectRemovedFromBoard);

			// Life-cycle.
			EventManager.UnsubscribeEventListener<GameResetEvent>(OnGameReset);
		}

		private void OnObjectAddedToBoard(PositionSaveObjectAddedEvent gameEvent)
		{
			var instance = gameEvent.CraftItemInstance;
			if (instance == null)
			{
				return;
			}
			if (_craftingService.IsFinalItem(instance) == false)
			{
				TrackElementPosition(instance);
			}
		}

		private void OnObjectRemovedFromBoard(PositionSaveObjectRemovedEvent gameEvent)
		{
			UntrackElementPosition(gameEvent.CraftItemInstance);
		}

		private void OnGeneralSaveBoard(GeneralPositionSaveEvent gameEvent)
		{
			_boardSaveState.Save();
		}

		private void LoadSaveData()
		{
			_boardSaveState = new BoardSaveInfo(GameConstants.BoardSaveId);
			_boardSaveState.Load();
		}

		private void OnGameReset(GameResetEvent gameEvent)
		{
			_boardSaveState.Reset();

			// Reset the craft item, and craft recipe save data.
			var craftItemEntites = _craftingService.CraftItemEntities;
			var craftRecipeEntities = _craftingService.CraftRecipeEntities;

			foreach (var entity in craftItemEntites)
			{
				entity.Reset();
				entity.EnsureStartingValues();
			}

			foreach (var entity in craftRecipeEntities)
			{
				entity.Reset();
			}
		}
		#endregion

		#region -- Public Methods --
		public override void Initialize()
		{
			base.Initialize();
			_craftingService = CraftingSystemCraftingService.Instance;
			LoadSaveData();
			SubscribeEventListeners();
		}

		public override void Cleanup()
		{
			base.Cleanup();
			UnsubscribeEventListeners();
		}

		public void TrackElementPosition(CraftItemInstance instance)
		{
			if (instance == null || _boardSaveState == null)
			{
				return;
			}
			_boardSaveState.TrackElement(instance);
		}

		public void UntrackElementPosition(CraftItemInstance instance)
		{
			if (instance == null || _boardSaveState == null)
			{
				return;
			}
			_boardSaveState.UntrackElement(instance);
		}
		#endregion
	}
}
