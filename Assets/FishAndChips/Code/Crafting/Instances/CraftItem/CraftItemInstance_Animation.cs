using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace FishAndChips
{
	/// <summary>
	/// Handle animations for CraftItemInstance.
	/// </summary>
    public partial class CraftItemInstance
    {
		#region -- Inspector --
		[Header("Animations")]
		public List<AnimationLookupKey> AnimationMap = new();
		#endregion

		#region -- Public Methods --
		/// <summary>
		/// Play animation. Attempt to get PlayableDirector from AnimationMap matching the passed in key.
		/// </summary>
		/// <param name="key">Key to the AnimationMap associated with a PlayableDirector.</param>
		public void PlayAnimation(string key, bool stopCurrent = true)
		{
			if (key.IsNullOrEmpty())
			{
				return;
			}
			if (stopCurrent == true)
			{
				GetCurrentPlayingDirector()?.StopSafe();
			}
			var animation = AnimationMap.FirstOrDefault(a => string.Equals(key, a.Key));
			if (animation != null)
			{
				animation.Director?.PlaySafe();
			}
		}

		/// <summary>
		/// Get the currently playing PlayableDirector.
		/// </summary>
		/// <returns>Return PlayableDirector thats play state is playing.</returns>
		public PlayableDirector GetCurrentPlayingDirector()
		{
			foreach (var animation in AnimationMap)
			{
				if (animation == null || animation.Director == null)
				{
					continue;
				}
				if (animation.Director.state == PlayState.Playing)
				{
					return animation.Director;
				}
			}
			return null;
		}

		/// <summary>
		/// If there is currently a playing PlayableDirector return its duration.
		/// </summary>
		/// <returns>Duration of currently playing PlayableDirector.</returns>
		public float GetCurrentPlayingDirectorLength()
		{
			var director = GetCurrentPlayingDirector();
			return (director != null && director.playableAsset != null) ? (float)director.playableAsset.duration : 0;
		}
		#endregion
	}
}
