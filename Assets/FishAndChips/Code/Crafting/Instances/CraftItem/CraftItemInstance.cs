using UnityEngine;

namespace FishAndChips
{
	/// <summary>
	/// Core script for CraftItemInstance. Representation of a CraftItem.
	/// </summary>
    public partial class CraftItemInstance : MonoBehaviour
    {
		#region -- Properties --
		public IEntity Entity { get; set; }
		public CraftItemEntity CraftItemEntity => Entity != null ? Entity as CraftItemEntity : null;
		public CraftItemData CraftItemData => CraftItemEntity != null ? CraftItemEntity.CraftItemData : null;
		public string InstanceID => CraftItemData != null ? CraftItemData.ID : string.Empty;
		#endregion

		#region -- Public Member Vars --
		public bool IsSelected;
		public bool IsInteractable;
		#endregion

		#region -- Protected Member Vars --
		protected CraftingSystemCraftingService _craftingService;
		#endregion

		#region -- Private Methods --
		private void OnEnable()
		{
			SubscribeEventListeners();
		}

		private void OnDisable()
		{
			UnsubscribeEventListeners();
		}

		private void Update()
		{
			if (CraftItemEntity == null)
			{
				return;
			}

			if (IsSelected == true)
			{
				HandleMovement();
			}
		}

		/// <summary>
		/// Subscribe to game events.
		/// </summary>
		private void SubscribeEventListeners()
		{
			EventManager.SubscribeEventListener<UnlockSequenceFinishedEvent>(OnUnlockSequenceFinished);
		}

		/// <summary>
		/// Unsubscribe from game events.
		/// </summary>
		private void UnsubscribeEventListeners()
		{
			EventManager.UnsubscribeEventListener<UnlockSequenceFinishedEvent>(OnUnlockSequenceFinished);
		}
		#endregion

		#region -- Protected Methods --
		/// <summary>
		/// Format name of GameObject.
		/// </summary>
		/// <returns>Name to display in hiearchy.</returns>
		protected virtual string FormatName()
		{
			string instanceId = (CraftItemEntity != null) ? CraftItemEntity.InstanceId : string.Empty;
			return $"CraftItemInstance : {instanceId}";
		}
		#endregion

		#region -- Public Methods --
		public virtual void Initialize()
		{
			_craftingService = CraftingSystemCraftingService.Instance;

			if (CraftItemEntity == null)
			{
				return;
			}

			IsSelected = false;
			IsInteractable = true;

			gameObject.name = FormatName();
			transform.localScale = Vector3.one;

			SetVisual();
		}

		/// <summary>
		/// Add instance back to pool.
		/// </summary>
		/// <param name="immediate">Should the repooling be immediate.</param>
		/// <param name="waitTime">Time before repooling.</param>
		public void Recycle(bool immediate = false, float waitTime = 1)
		{
			IsSelected = false;
			IsInteractable = false;
			EventManager.TriggerEvent(new PositionSaveObjectRemovedEvent(this, immediate, waitTime));
		}

		/// <summary>
		/// Handle selecting instance. Called from EventTrigger component : Pointer Down.
		/// </summary>
		public void OnSelected()
		{
			if (IsSelected == true)
			{
				return;
			}
			IsSelected = true;
			AttemptResetCloningVariables();
			EventManager.TriggerEvent(new CraftItemSelectionEvent(this));
		}

		/// <summary>
		/// Handle releasing instance. Called from EventTrigger component : Pointer Up, Drop
		/// </summary>
		public void OnRelease()
		{
			if (IsSelected == false)
			{
				return;
			}
			IsSelected = false;
			EventManager.TriggerEvent(new CraftItemReleasedEvent(this));
		}
		#endregion
	}
}
