using UnityEngine;
using TMPro;

namespace FishAndChips
{
	/// <summary>
	/// Handle filtering displayed CraftItems.
	/// </summary>
    [RequireComponent(typeof(TMP_InputField))]
    public class CraftItemSearch : FishScript
    {
		#region -- Private Member Vars --
		private TMP_InputField _inputField;
		#endregion

		#region -- Private Methods --
		private void Awake()
		{
			// Gather UI references.
			_inputField = GetComponent<TMP_InputField>();
		}

		/// <summary>
		/// Callback for when the text input field changes.
		/// </summary>
		/// <param name="searchString">The current text displayed in the input field.</param>
		private void OnSearchStringChanged(string searchString)
		{
			// Trigger event for search change.
			EventManager.TriggerEvent(new CraftItemSearchEvent(searchString));
		}

		/// <summary>
		/// Callback for game reset event.
		/// </summary>
		/// <param name="resetEvent">Game event for reset.</param>
		private void OnGameReset(GameResetEvent resetEvent)
		{
			// Clear the text and the corresponding filtering.
			ClearSearch();
		}
		#endregion

		#region -- Protected Methods --
		/// <summary>
		/// Subscribe to events.
		/// </summary>
		protected override void SubscribeEventListeners()
		{
			base.SubscribeEventListeners();
			if (_inputField != null)
			{
				_inputField.onValueChanged.AddListener(OnSearchStringChanged);
			}
			EventManager.SubscribeEventListener<GameResetEvent>(OnGameReset);
		}

		/// <summary>
		/// Unsubscribe from events.
		/// </summary>
		protected override void UnsubscribeEventListeners()
		{
			base.UnsubscribeEventListeners();
			if (_inputField != null)
			{
				_inputField.onValueChanged.RemoveAllListeners();
			}
			EventManager.UnsubscribeEventListener<GameResetEvent>(OnGameReset);
		}
		#endregion

		#region -- Public Methods --
		/// <summary>
		/// Clear search text.
		/// </summary>
		public void ClearSearch()
		{
			if (_inputField == null)
			{
				return;
			}
			_inputField.text = string.Empty;
		}
		#endregion
	}
}
