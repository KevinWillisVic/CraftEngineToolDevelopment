using UnityEngine;
using UnityEngine.Playables;
using System;
using System.Threading.Tasks;

namespace FishAndChips
{
	[RequireComponent(typeof(PlayableDirector))]
    public class UIPlayableDirector : MonoBehaviour
    {
		#region -- Supporting --
		public enum eDirectorType
		{
			Activation,
			Deactivation,
			None
		}
		#endregion

		#region -- Properties --
		public Action OnComplete { get; set; }
		#endregion

		#region -- Inspector --
		public eDirectorType DirectorType;
		public PlayableDirector Director;
		#endregion

		#region -- Private Methods --
		private void DirectorStopped(PlayableDirector director)
		{
			OnComplete.FireSafe();
		}

		private void FindDirector()
		{
			if (Director != null)
			{
				return;
			}
			Director = GetComponent<PlayableDirector>();
		}
		#endregion

		#region -- Public Methods --
		public void Initialize()
		{
			FindDirector();
			if (Director == null)
			{
				return;
			}
			Director.stopped -= DirectorStopped;
			Director.stopped += DirectorStopped;
		}

		public async Task FindAndPlayAwaitable()
		{
			FindDirector();
			await AwaitPlayableAsync(Director);
		}

		public static void PlayPlayable(PlayableDirector director)
		{
			director.PlaySafe();
		}

		public static async Task AwaitPlayableAsync(PlayableDirector director)
		{
			if (director == null)
			{
				return;
			}

			if (director.state != PlayState.Playing)
			{
				director.Play();
			}
			while (director != null && director.state == PlayState.Playing)
			{
				await Awaitable.EndOfFrameAsync();
			}
		}

		public static async void WaitForActiveObjectAndThenPlay(GameObject obj, PlayableDirector director, float timeoutSeconds)
		{
			await WaitForActiveObjectAndThenPlayAsync(obj, director, timeoutSeconds);
		}

		public static async Task WaitForActiveObjectAndThenPlayAsync(GameObject obj, PlayableDirector director, float timeoutSeconds)
		{
			if (obj == null || director == null)
			{
				return;
			}

			float timeRemaining = timeoutSeconds;

			while (obj.activeInHierarchy == false && timeRemaining > 0)
			{
				if (obj == null)
				{
					return;
				}
				await Awaitable.EndOfFrameAsync();
				timeRemaining -= Time.unscaledDeltaTime;
			}

			if (obj != null && obj.activeInHierarchy == true)
			{
				await AwaitPlayableAsync(director);
			}
		}
		#endregion
	}
}
