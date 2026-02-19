using System;
using UnityEngine;

namespace FishAndChips
{
	/// <summary>
	/// Save structure for element that will appear on the board.
	/// </summary>
    [Serializable]
    public class BoardElementSaveInfo
    {
		#region -- Public Member Vars --
		// Id of the element on the board.
		public string ID;
		// Position of the element on the board.
		public Vector2 Position;
		// Live object to fetch its position from.
		[NonSerialized] public Transform RuntimeInstance;
		#endregion

		#region -- Constructors --
		public BoardElementSaveInfo(string id, Vector2 position)
		{
			ID = id;
			Position = position;
		}

		public BoardElementSaveInfo(string id, Transform instance)
		{
			ID = id;
			Position = instance.transform.localPosition;
			RuntimeInstance = instance;
		}
		#endregion

		#region -- Public Methods --
		public void RefreshPosition()
		{
			if (RuntimeInstance == null)
			{
				return;
			}
			Position = RuntimeInstance.localPosition;
		}
		#endregion
	}
}
