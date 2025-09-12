using UnityEngine;

namespace FishAndChips
{
    public class CraftItemComponentListItemSelectableElement : CraftItemComponentListItem
    {
		#region -- Public Methods --
		public override void Initialize()
		{
			_craftingService = CraftingSystemCraftingService.Instance;
			base.Initialize();
		}

		public override void OnPointerExit()
		{
			if (Entity == null || _isPointerDown == false)
			{
				return;
			}
			// Spawn a new CraftItem instance.
			var newInstance = _craftingService.SpawnAndReturnCraftItemInstance(Entity,
				Input.mousePosition,
				triggerSaveEvent: true,
				spawnAnimation: CraftItemInstance.eCraftItemAnimationKeys.SpawnFromScrollRect.ToString());

			newInstance.OnSelected();
			newInstance.SetActiveSafe(true);
			base.OnPointerExit();
		}
		#endregion
	}
}
