using System;

namespace FishAndChips
{
	public class SaveEvent : IEvent
	{
		public Type[] DispatchAs { get; internal set; }

		public SaveEvent()
		{
			DispatchAs = new[] { typeof(SaveEvent) };
		}
	}

	public class GeneralPositionSaveEvent : SaveEvent
	{
		public GeneralPositionSaveEvent()
		{
			DispatchAs = new[] { typeof(GeneralPositionSaveEvent), typeof(SaveEvent) };
		}
	}

	public class PositionSaveObjectAddedEvent : SaveEvent
	{
		public CraftItemInstance CraftItemInstance;

		public PositionSaveObjectAddedEvent(CraftItemInstance craftItemInstance)
		{
			CraftItemInstance = craftItemInstance;
			DispatchAs = new[] { typeof(PositionSaveObjectAddedEvent), typeof(SaveEvent) };
		}
	}

	public class PositionSaveObjectRemovedEvent : SaveEvent
	{
		public CraftItemInstance CraftItemInstance;
		public bool RePoolImmediate;
		public float WaitTimeBeforeRepool;

		public PositionSaveObjectRemovedEvent(CraftItemInstance craftItemInstance, bool immediate = false, float waitTimeBeforeRepool = 1)
		{
			CraftItemInstance = craftItemInstance;
			RePoolImmediate = immediate;
			WaitTimeBeforeRepool = waitTimeBeforeRepool;
			DispatchAs = new[] { typeof(PositionSaveObjectRemovedEvent), typeof(SaveEvent) };
		}
	}
}
