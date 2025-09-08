using System;
using static FishAndChips.SimpleGameplayBoard;

namespace FishAndChips
{
	public class GameEvent : IEvent
	{
		public Type[] DispatchAs { get; internal set; }

		public GameEvent()
		{
			DispatchAs = new[] { typeof(GameEvent) };
		}
	}

	public class GameResetEvent : GameEvent
	{
		public GameResetEvent()
		{
			DispatchAs = new[] { typeof(GameResetEvent), typeof(GameEvent) };
		}
	}

	public class UnlockSequenceFinishedEvent : GameEvent
	{
		public UnlockSequenceFinishedEvent()
		{
			DispatchAs = new[] { typeof(UnlockSequenceFinishedEvent), typeof(GameEvent) };
		}
	}

	public class CraftItemSearchEvent : GameEvent
	{
		public string SearchFilter = string.Empty;

		public CraftItemSearchEvent(string searchFilter)
		{
			SearchFilter = searchFilter;
			DispatchAs = new[] { typeof(CraftItemSearchEvent), typeof(GameEvent) };
		}
	}

	public class CraftItemSelectionEvent : GameEvent
	{
		public CraftItemInstance CraftItemInstance;

		public CraftItemSelectionEvent(CraftItemInstance instance)
		{
			CraftItemInstance = instance;
			DispatchAs = new[] { typeof(CraftItemSelectionEvent), typeof(GameEvent) };
		}
	}

	public class CraftItemReleasedEvent : GameEvent
	{
		public CraftItemInstance CraftItemInstance;

		public CraftItemReleasedEvent(CraftItemInstance instance)
		{
			CraftItemInstance = instance;
			DispatchAs = new[] { typeof(CraftItemReleasedEvent), typeof(GameEvent) };
		}
	}

	public class CraftItemEntityUnlockEvent : GameEvent
	{
		public CraftItemEntity CraftItemEntity;

		public CraftItemEntityUnlockEvent(CraftItemEntity entity)
		{
			CraftItemEntity = entity;
			DispatchAs = new[] { typeof(CraftItemEntityUnlockEvent), typeof(GameEvent) };
		}
	}

	public class CraftRecipeUnlockEvent : GameEvent
	{
		public CraftRecipeEntity CraftRecipeEntity;

		public CraftRecipeUnlockEvent(CraftRecipeEntity entity)
		{
			CraftRecipeEntity = entity;
			DispatchAs = new[] { typeof(CraftRecipeUnlockEvent), typeof(GameEvent) };
		}
	}

	public class PopulatePools : GameEvent
	{
		public PopulatePools()
		{
			DispatchAs = new[] { typeof(PopulatePools), typeof(GameEvent) };
		}
	}

	public class OnGameSceneReady : GameEvent
	{
		public OnGameSceneReady()
		{
			DispatchAs = new[] { typeof(OnGameSceneReady), typeof(GameEvent) };
		}
	}

	/*
	public class CleanBoardEvent : GameEvent
	{
		public CleanBoardEvent()
		{
			DispatchAs = new[] { typeof(CleanBoardEvent) };
		}
	}
	*/

	public class RecycleTriggerableEvent : GameEvent
	{
		public RecycleTriggerableEvent()
		{
			DispatchAs = new[] { typeof(RecycleTriggerableEvent) };
		}
	}

	public class RecycleStateUpdateEvent : GameEvent
	{
		public eRecycleState RecycleState = eRecycleState.CleanState;

		public RecycleStateUpdateEvent(eRecycleState state)
		{
			RecycleState = state;
			DispatchAs = new[] { typeof(RecycleStateUpdateEvent) };
		}
	}

	public class ToastEvent : GameEvent
	{
		public string Message;

		public ToastEvent(string message)
		{
			Message = message;
			DispatchAs = new [] { typeof(ToastEvent) };
		}
	}
}
