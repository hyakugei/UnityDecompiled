using System;
using System.Collections.Generic;

namespace UnityEngine.Experimental.UIElements
{
	internal class TimerEventScheduler : IScheduler
	{
		private class TimerEventSchedulerItem : ScheduledItem
		{
			private readonly Action<TimerState> m_TimerUpdateEvent;

			public TimerEventSchedulerItem(Action<TimerState> updateEvent)
			{
				this.m_TimerUpdateEvent = updateEvent;
			}

			public override void PerformTimerUpdate(TimerState state)
			{
				if (this.m_TimerUpdateEvent != null)
				{
					this.m_TimerUpdateEvent(state);
				}
			}

			public override string ToString()
			{
				return this.m_TimerUpdateEvent.ToString();
			}
		}

		private readonly List<ScheduledItem> m_ScheduledItems = new List<ScheduledItem>();

		private bool m_TransactionMode;

		private readonly List<ScheduledItem> m_ScheduleTransactions = new List<ScheduledItem>();

		private readonly List<ScheduledItem> m_UnscheduleTransactions = new List<ScheduledItem>();

		internal bool disableThrottling = false;

		private int m_LastUpdatedIndex = -1;

		public void Schedule(IScheduledItem item)
		{
			if (item != null)
			{
				ScheduledItem scheduledItem = item as ScheduledItem;
				if (scheduledItem == null)
				{
					throw new NotSupportedException("Scheduled Item type is not supported by this scheduler");
				}
				if (this.m_TransactionMode)
				{
					this.m_ScheduleTransactions.Add(scheduledItem);
				}
				else
				{
					if (this.m_ScheduledItems.Contains(scheduledItem))
					{
						throw new ArgumentException("Cannot schedule function " + scheduledItem + " more than once");
					}
					this.m_ScheduledItems.Add(scheduledItem);
				}
			}
		}

		public IScheduledItem ScheduleOnce(Action<TimerState> timerUpdateEvent, long delayMs)
		{
			TimerEventScheduler.TimerEventSchedulerItem timerEventSchedulerItem = new TimerEventScheduler.TimerEventSchedulerItem(timerUpdateEvent)
			{
				delayMs = delayMs
			};
			this.Schedule(timerEventSchedulerItem);
			return timerEventSchedulerItem;
		}

		public IScheduledItem ScheduleUntil(Action<TimerState> timerUpdateEvent, long delayMs, long intervalMs, Func<bool> stopCondition)
		{
			TimerEventScheduler.TimerEventSchedulerItem timerEventSchedulerItem = new TimerEventScheduler.TimerEventSchedulerItem(timerUpdateEvent)
			{
				delayMs = delayMs,
				intervalMs = intervalMs,
				timerUpdateStopCondition = stopCondition
			};
			this.Schedule(timerEventSchedulerItem);
			return timerEventSchedulerItem;
		}

		public IScheduledItem ScheduleForDuration(Action<TimerState> timerUpdateEvent, long delayMs, long intervalMs, long durationMs)
		{
			TimerEventScheduler.TimerEventSchedulerItem timerEventSchedulerItem = new TimerEventScheduler.TimerEventSchedulerItem(timerUpdateEvent)
			{
				delayMs = delayMs,
				intervalMs = intervalMs,
				timerUpdateStopCondition = null
			};
			timerEventSchedulerItem.SetDuration(durationMs);
			this.Schedule(timerEventSchedulerItem);
			return timerEventSchedulerItem;
		}

		private bool RemovedScheduledItemAt(int index)
		{
			bool result;
			if (index >= 0)
			{
				ScheduledItem scheduledItem = this.m_ScheduledItems[index];
				this.m_ScheduledItems.RemoveAt(index);
				scheduledItem.OnItemUnscheduled();
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		public void Unschedule(IScheduledItem item)
		{
			ScheduledItem scheduledItem = item as ScheduledItem;
			if (scheduledItem != null)
			{
				if (this.m_TransactionMode)
				{
					this.m_UnscheduleTransactions.Add(scheduledItem);
				}
				else if (!this.RemovedScheduledItemAt(this.m_ScheduledItems.IndexOf(scheduledItem)))
				{
					throw new ArgumentException("Cannot unschedule unknown scheduled function " + scheduledItem);
				}
			}
		}

		public void UpdateScheduledEvents()
		{
			try
			{
				this.m_TransactionMode = true;
				long num = Panel.TimeSinceStartupMs();
				int count = this.m_ScheduledItems.Count;
				long num2 = num + 20L;
				int num3 = this.m_LastUpdatedIndex + 1;
				if (num3 >= count)
				{
					num3 = 0;
				}
				for (int i = 0; i < count; i++)
				{
					num = Panel.TimeSinceStartupMs();
					if (!this.disableThrottling && num >= num2)
					{
						break;
					}
					int num4 = num3 + i;
					if (num4 >= count)
					{
						num4 -= count;
					}
					ScheduledItem scheduledItem = this.m_ScheduledItems[num4];
					if (num - scheduledItem.delayMs >= scheduledItem.startMs)
					{
						TimerState state = new TimerState
						{
							start = scheduledItem.startMs,
							now = num
						};
						scheduledItem.PerformTimerUpdate(state);
						scheduledItem.startMs = num;
						scheduledItem.delayMs = scheduledItem.intervalMs;
						if (scheduledItem.ShouldUnschedule())
						{
							this.Unschedule(scheduledItem);
						}
					}
					this.m_LastUpdatedIndex = num4;
				}
			}
			finally
			{
				this.m_TransactionMode = false;
				for (int j = 0; j < this.m_UnscheduleTransactions.Count; j++)
				{
					this.Unschedule(this.m_UnscheduleTransactions[j]);
				}
				this.m_UnscheduleTransactions.Clear();
				for (int k = 0; k < this.m_ScheduleTransactions.Count; k++)
				{
					this.Schedule(this.m_ScheduleTransactions[k]);
				}
				this.m_ScheduleTransactions.Clear();
			}
		}
	}
}
