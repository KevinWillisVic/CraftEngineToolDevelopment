using UnityEngine;
using UnityEngine.Pool;

namespace FishAndChips
{
	public class CraftingSystemPoolingService : Singleton<CraftingSystemPoolingService>, IInitializable
	{
		#region -- Inspector --
		[Header("Craft Item Instance Pool")]
		public int DefaultCraftItemInstancePoolSize = 50;
		public CraftItemInstance CraftItemInstanceTemplate;

		[Header("Previously Made Indicator Instance Pool")]
		public int DefaultPreviouslyMadeIndicatorInstancePoolSize = 50;
		public PreviouslyMadeIndicatorInstance PreviouslyMadeIndicatorInstanceTemplate;
		#endregion

		#region -- Protected Member Vars --
		protected UIService _uiService;
		protected CraftingSystemCraftingService _craftingService;
		#endregion

		#region -- Private Member Vars --
		private ObjectPool<CraftItemInstance> _craftItemInstancePool;
		private ObjectPool<PreviouslyMadeIndicatorInstance> _previouslyMadeIndicatorPool;

		private SimpleGameplayBoard _gameplayBoard;
		#endregion

		#region -- Private Methods --
		private void SubscribeListeners()
		{
			EventManager.SubscribeEventListener<PopulatePools>(OnPopulatePoolEvent);
		}

		private void UnsubscribeListeners()
		{
			EventManager.UnsubscribeEventListener<PopulatePools>(OnPopulatePoolEvent);
		}

		/// <summary>
		/// Callback for event to create pools.
		/// </summary>
		private void OnPopulatePoolEvent(PopulatePools gameEvent)
		{
			var gameplaySceneView = _uiService.GetView<GameplaySceneView>();
			_gameplayBoard = gameplaySceneView.SimpleGameplayBoard;
			CreatePools();
		}
		#endregion

		#region -- Public Methods --
		public override void Initialize()
		{
			base.Initialize();
			SubscribeListeners();
			_uiService = UIService.Instance;
			_craftingService = CraftingSystemCraftingService.Instance;
		}

		public override void Cleanup()
		{
			base.Cleanup();
			UnsubscribeListeners();
		}

		/// <summary>
		/// Create PreviouslyMadeIndicatorInstance pool, and CraftItemInstance pool.
		/// </summary>
		public virtual void CreatePools()
		{
			_craftItemInstancePool = new ObjectPool<CraftItemInstance>(() =>
			{
				var instance = Instantiate(CraftItemInstanceTemplate);
				instance.transform.SetParent(_gameplayBoard.CraftingLayer);
				instance.transform.ResetScale();
				return instance;
			}
			, craftItem =>
			{
			}
			, craftItem => craftItem.gameObject.SetActive(false)
			, craftItem => Destroy(craftItem.gameObject)
			, false
			, DefaultCraftItemInstancePoolSize);

			_previouslyMadeIndicatorPool = new ObjectPool<PreviouslyMadeIndicatorInstance>(() =>
			{
				var instance = Instantiate(PreviouslyMadeIndicatorInstanceTemplate);
				instance.transform.SetParent(_gameplayBoard.PopupLayer);
				instance.transform.ResetScale();
				return instance;
			},
			indicator =>
			{
			}
			, indicator => indicator.gameObject.SetActiveSafe(false)
			, indicator => Destroy(indicator.gameObject)
			, false
			, DefaultPreviouslyMadeIndicatorInstancePoolSize);
		}

		/// <summary>
		/// Return a PreviouslyMadeIndicatorInstance from the pool.
		/// </summary>
		/// <returns>A PreviouslyMadeIndicatorInstance from the pool.</returns>
		public PreviouslyMadeIndicatorInstance GetPreviouslyMadeInstancePoolElement()
		{
			return _previouslyMadeIndicatorPool.Get();
		}

		/// <summary>
		/// Add a PreviouslyMadeIndicatorInstance back to the pool.
		/// </summary>
		/// <param name="instance">Instance being added back to pool.</param>
		public void PoolPreviouslyMadeIndicatorInstance(PreviouslyMadeIndicatorInstance instance)
		{
			_previouslyMadeIndicatorPool.Release(instance);
		}

		/// <summary>
		/// Clear PreviouslyMadeIndicatorInstance pool.
		/// </summary>
		public void ClearPreviouslyMadeIndicatorInstancePool()
		{
			_previouslyMadeIndicatorPool.Clear();
		}

		/// <summary>
		/// Return a CraftItemInstance from the pool.
		/// </summary>
		/// <returns>A CraftItemInstance from the pool.</returns>
		public CraftItemInstance GetCraftItemInstancePoolElement()
		{
			return _craftItemInstancePool.Get();
		}

		/// <summary>
		/// Add a CraftItemInstance back to the pool.
		/// </summary>
		/// <param name="instance">Instance being added back to pool.</param>
		public void PoolCraftItemInstance(CraftItemInstance instance)
		{
			_craftItemInstancePool.Release(instance);
		}

		/// <summary>
		/// Clear CraftItemInstance pool.
		/// </summary>
		public void ClearCraftItemInstancePool()
		{
			_craftItemInstancePool.Clear();
		}
		#endregion
	}
}
