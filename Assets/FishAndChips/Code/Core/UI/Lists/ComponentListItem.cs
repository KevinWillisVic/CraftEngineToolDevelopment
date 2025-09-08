using UnityEngine;
using UnityEngine.Playables;
using System;
using System.Collections.Generic;

namespace FishAndChips
{
    public class ComponentListItem : MonoBehaviour
    {
		#region -- Properties --
		public int Index { get; set; }
		public Action<ComponentListItem> OnItemSelected { get; set; }
		public Action<ComponentListItem> OnItemHeld { get; set; }
		public RectTransform RectTransform { get; private set; }
		public bool IsVisuallySelected { get; set; }

		public object ListObject
		{
			get
			{
				return _listObject;
			}
			set
			{
				_listObject = value;
				OnObjectSet();
			}
		}
		#endregion

		#region -- Inspector --
		public BaseButton SelectionButton;
		public float RequiredHoldTime = 1.0f;

		public PlayableDirector EntryPlayable;

		public List<GameObject> VisuallySelectedObjects = new();

		public bool HandleHoldEvent = false;
		public bool CanBeUnselected = false;
		#endregion

		#region -- Private Member Vars --
		private float _pointerDownTimer = 0f;

		private object _listObject;

		private PlayableDirector _queuedActivationPlayable;

		private bool _handledHoldEvent = false;
		#endregion

		#region -- Protected Member Vars --
		protected bool _isPointerDown = false;
		#endregion

		#region -- Private Methods --
		private void OnDestroy()
		{
			ClearItem();
		}
		#endregion

		#region -- Protected Methods --
		protected virtual void OnObjectSet()
		{
		}

		protected virtual void ListItemHeld()
		{
			OnItemHeld.FireSafe(this);
		}
		#endregion

		#region -- Public Methods --
		public virtual void Initialize()
		{
			RectTransform = GetComponent<RectTransform>();

			if (SelectionButton != null)
			{
				SelectionButton.onClick.RemoveListener(SelectedFromButton);
				SelectionButton.onClick.AddListener(SelectedFromButton);
			}
		}

		public virtual void ClearItem()
		{
			if (SelectionButton != null)
			{
				SelectionButton.onClick.RemoveListener(SelectedFromButton);
			}
		}

		public virtual void UpdateItem()
		{
			if (_isPointerDown == true && HandleHoldEvent == true)
			{
				_pointerDownTimer += Time.deltaTime;

				if (_pointerDownTimer >= RequiredHoldTime)
				{
					_handledHoldEvent = true;
					ListItemHeld();
				}
			}

			if (_queuedActivationPlayable != null && gameObject.activeInHierarchy)
			{
				_queuedActivationPlayable.PlaySafe();
				_queuedActivationPlayable = null;
			}
		}

		public virtual void SelectedFromButton()
		{
			if (_handledHoldEvent == false)
			{
				Selected();
			}
			else
			{
				_handledHoldEvent = false;
			}
		}

		public virtual void VisuallySelected(bool selected)
		{
			if (IsVisuallySelected && selected && CanBeUnselected)
			{
				selected = false;
			}

			IsVisuallySelected = selected;
			foreach (var visual in VisuallySelectedObjects)
			{
				visual.SetActiveSafe(selected);
			}
		}

		public virtual void Selected()
		{
			OnItemSelected.FireSafe(this);
		}

		public virtual async void AnimateIn(float delay)
		{
			if (EntryPlayable == null)
			{
				return;
			}

			if (delay > 0 && Index > 0)
			{
				float time = delay * Index;
				await Awaitable.WaitForSecondsAsync(time);
			}

			if (this == null || gameObject == null)
			{
				return;
			}

			gameObject.SetActiveSafe(true);
			if (gameObject.activeInHierarchy == false)
			{
				_queuedActivationPlayable = EntryPlayable;
			}
			else
			{
				EntryPlayable.PlaySafe();
			}
		}

		public virtual void OnPointerDown()
		{
			_isPointerDown = true;
		}

		public virtual void OnPointerUp()
		{
			_isPointerDown = false;
			_pointerDownTimer = 0;
		}

		public virtual void OnPointerExit()
		{
		}
		#endregion
	}
}
