using UnityEngine;
using System.Threading.Tasks;

namespace FishAndChips
{
	/// <summary>
	/// Abstract representation for the game board.
	/// </summary>
    public abstract class GameplayBoard : FishScript
    {
		#region -- Supporting --
		/// <summary>
		/// States the game can be in in regards to recycling.
		/// </summary>
		public enum eRecycleState
		{
			CleanState,
			UndoState
		}
		#endregion

		#region -- Properties --
		public float BoundaryWidth => _boundaryWidth;
		public float BoundaryHeight => _boundaryHeight;
		public eRecycleState RecycleState => _recycleState;
		#endregion

		#region -- Inspector --
		[Header("GameplayBoard Leyers")]
		public Transform CraftingLayer;
		public Transform DragLayer;
		public Transform PopupLayer;
		#endregion

		#region -- Protected Member Vars --
		protected float _boundaryWidth;
		protected float _boundaryHeight;

		protected eRecycleState _recycleState = eRecycleState.CleanState;
		#endregion

		#region -- Private Methods --
		/// <summary>
		/// Callback for when the game is reset.
		/// </summary>
		/// <param name="resetEvent">The event for when the game is reset.</param>
		private void OnResetGame(GameResetEvent resetEvent)
		{
			MassRecycle();
			SetToDefaultState();
		}

		#endregion

		#region -- Protected Methods --
		/// <summary>
		/// Subscribe to events.
		/// </summary>
		protected override void SubscribeEventListeners()
		{
			base.SubscribeEventListeners();
			EventManager.SubscribeEventListener<GameResetEvent>(OnResetGame);
		}

		/// <summary>
		/// Unsubscribe from events.
		/// </summary>
		protected override void UnsubscribeEventListeners()
		{
			EventManager.UnsubscribeEventListener<GameResetEvent>(OnResetGame);
		}

		/// <summary>
		/// Set board to starting state.
		/// </summary>
		protected virtual void SetToDefaultState()
		{
		}
		#endregion

		#region -- Public Methods --
		/// <summary>
		/// Bound a position to within a rectangular game board.
		/// </summary>
		/// <param name="position">Position to be bound.</param>
		/// <param name="buffer"></param>
		/// <returns></returns>
		public virtual Vector2 GetPositionBoundedToCraftingRegionRectangle(Vector2 position, float buffer = 0f)
		{
			float leftBound = -(_boundaryWidth / 2f) + buffer;
			float rightBound = (_boundaryWidth / 2f) - buffer;
			position.x = Mathf.Clamp(position.x, leftBound, rightBound);

			float topBound = (_boundaryHeight / 2f) - buffer;
			float bottomBound = -(_boundaryHeight / 2f) + buffer;
			position.y = Mathf.Clamp(position.y, bottomBound, topBound);

			return position;
		}

		/// <summary>
		/// Recycle all elements on board.
		/// </summary>
		public virtual void MassRecycle()
		{
		}

		/// <summary>
		/// Undo recycle just done.
		/// </summary>
		public virtual void UndoRecycle()
		{
		}

		/// <summary>
		/// Handle any set up.
		/// </summary>
		public virtual async void Setup()
		{
			await Task.CompletedTask;
		}
		#endregion
	}
}
