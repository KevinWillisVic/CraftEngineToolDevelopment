using UnityEngine;

namespace FishAndChips
{
	/// <summary>
	/// Core script for PreviouslyMadeIndicatorInstance.
	/// </summary>
    public partial class PreviouslyMadeIndicatorInstance : MonoBehaviour
    {
		#region -- Properties --
		public IEntity Entity { get; set; }
		public CraftItemEntity CraftItemEntity
		{
			get
			{
				if (Entity != null)
				{
					return Entity as CraftItemEntity;
				}
				return null;
			}
		}
		#endregion

		#region -- Protected Member Vars --
		protected CraftingSystemPoolingService _poolingService;
		protected CraftingSystemCraftingService _craftingService;
		#endregion

		#region -- Protected Methods --
		protected virtual string FormatName()
		{
			string instanceId = (CraftItemEntity != null) ? CraftItemEntity.InstanceId : string.Empty;
			return $"PreviouslyMadeIndicator : {instanceId}";
		}
		#endregion

		#region -- Public Methods --
		public void Initialize()
		{
			if (CraftItemEntity == null)
			{
				return;
			}
			Initialize(CraftItemEntity);
		}

		public void Initialize(CraftItemEntity entity)
		{
			Entity = entity;
			if (CraftItemEntity == null)
			{
				return;
			}
			_poolingService = CraftingSystemPoolingService.Instance;
			_craftingService = CraftingSystemCraftingService.Instance;

			gameObject.name = FormatName();
			SetVisual(entity);
			InitializeMovement();
		}
		#endregion
	}
}
