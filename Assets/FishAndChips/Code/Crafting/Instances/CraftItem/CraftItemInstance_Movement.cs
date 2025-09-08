using UnityEngine;

namespace FishAndChips
{
	/// <summary>
	/// Handle movement logic for CraftItemInstance.
	/// </summary>
    public partial class CraftItemInstance
    {
		#region -- Private Methods --
		/// <summary>
		/// Move the CraftItemInstance.
		/// </summary>
		private void HandleMovement()
		{
			if (ShouldMove() == false)
			{
				return;
			}

			Vector3 newPosition = transform.position;
			if (Input.touchCount > 0)
			{
				newPosition = Input.GetTouch(0).position;
			}
			else if (Input.mousePresent == true && Input.GetMouseButton(0))
			{
				newPosition = Input.mousePosition;
			}
			transform.position = newPosition;
		}
		#endregion

		#region -- Protected Methods --
		/// <summary>
		/// Determine if the instance allows movement.
		/// </summary>
		/// <returns>True if should move, false otherwise.</returns>
		protected virtual bool ShouldMove()
		{
			if (_craftingService.IsFinalItem(CraftItemEntity) == true
				|| _craftingService.IsDepletedItem(CraftItemEntity) == true)
			{
				return false;
			}
			return true;
		}
		#endregion
	}
}
