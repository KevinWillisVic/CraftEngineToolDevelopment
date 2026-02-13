using UnityEngine;
using UnityEngine.InputSystem;

namespace FishAndChips
{
    public class InputService : Singleton<InputService>, IInitializable, ICleanable
    {
		#region -- Public Member Vars --
		public bool EnableEnhancedTouch = true;
		#endregion

		#region -- Public Methods --
		public override void Initialize()
		{
			base.Initialize();

			if (EnableEnhancedTouch == true)
			{
				SetEnhancedTouch(true);
			}
		}

		public override void Cleanup()
		{
			base.Cleanup();

			if (EnableEnhancedTouch == true)
			{
				SetEnhancedTouch(false);
			}
		}

		public void SetEnhancedTouch(bool state)
		{
			if (state == true)
			{
				UnityEngine.InputSystem.EnhancedTouch.EnhancedTouchSupport.Enable();
			}
			else
			{
				UnityEngine.InputSystem.EnhancedTouch.EnhancedTouchSupport.Disable();
			}
		}

		public Vector3 GetCurrentInteractionPoint()
		{
			// TODO : Check if using the old input system.
			/*
			if (Input.touchCount > 0)
			{
				_pointerEventData.position = Input.GetTouch(0).position;
				EventSystem.current.RaycastAll(_pointerEventData, _collisionResults);
			}
			else if (Input.mousePresent == true)
			{
				_pointerEventData.position = Input.mousePosition;
				EventSystem.current.RaycastAll(_pointerEventData, _collisionResults);
			}
			*/

			Vector3 position = Vector3.zero;
			if (Touchscreen.current.primaryTouch.IsPressed())
			{
				position = Touchscreen.current.primaryTouch.position.value;
			}
			else
			{
				Mouse mouse = Mouse.current;
				if (mouse != null)
				{
					position = mouse.position.value;
				}
			}

			if (EnableEnhancedTouch == true)
			{
				foreach (var touch in UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches)
				{
					position = touch.screenPosition;
				}
			}
			return position;
		}
		#endregion
	}
}
