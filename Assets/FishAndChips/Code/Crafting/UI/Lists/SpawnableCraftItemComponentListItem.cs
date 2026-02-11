using UnityEngine;
using UnityEngine.InputSystem;

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

			Vector3 position = Vector3.zero;
			position = Mouse.current.position.value;

			// Spawn a new CraftItem instance.
			var newInstance = _craftingService.SpawnAndReturnCraftItemInstance(Entity,
				position,
				triggerSaveEvent: true,
				spawnAnimation: CraftItemInstance.eCraftItemAnimationKeys.SpawnFromScrollRect.ToString());

			newInstance.OnSelected();
			newInstance.SetActiveSafe(true);
			base.OnPointerExit();
		}
		#endregion
	}
}
