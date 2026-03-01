using UnityEngine;
using static FishAndChips.CraftingEnums;

namespace FishAndChips
{
    public class SpawnableCraftItemComponentListItem : CraftItemComponentListItem
    {
		#region -- Public Methods --
		public override void OnPointerExit()
		{
			if (Entity == null || _isPointerDown == false)
			{
				return;
			}

			Vector3 position = _inputService.GetCurrentInteractionPoint();

			// Spawn a new CraftItem instance.
			var newInstance = _craftingService.SpawnAndReturnCraftItemInstance(Entity,
				position,
				triggerSaveEvent: true,
				spawnAnimation: eCraftItemAnimationKeys.SpawnFromScrollRect.ToString());

			newInstance.transform.localPosition = position;
			newInstance.OnSelected();
			newInstance.SetActiveSafe(true);
			base.OnPointerExit();
		}
		#endregion
	}
}
