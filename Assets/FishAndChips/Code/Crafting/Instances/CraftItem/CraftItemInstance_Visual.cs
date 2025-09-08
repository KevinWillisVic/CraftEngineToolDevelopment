using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace FishAndChips
{
	/// <summary>
	/// Handle visuals for CraftItemInstaance.
	/// </summary>
    public partial class CraftItemInstance
    {
		#region -- Enumerations --
		public enum eCraftItemAnimationKeys
		{
			None,
			InvalidCombo,
			CloneAppear,
		}
		#endregion

		#region -- Inspector --
		[Header("Visuals")]
		public SpriteRenderer SpriteRendererVisual;
		public Image ImageVisual;

		[Header("Animations")]
		public List<AnimationLookupKey> AnimationMap = new();
		#endregion

		#region -- Protected Member Vars --
		protected CraftingSystemImageService _imageService;
		#endregion

		#region -- Private Methods --
		/// <summary>
		/// Callback for unlock sequence finishing. Some things should be recycled.
		/// </summary>
		/// <param name="gameEvent">Event for unlock sequence finishing.</param>
		private void OnUnlockSequenceFinished(UnlockSequenceFinishedEvent gameEvent)
		{
			if (gameEvent == null)
			{
				return;
			}

			if (_craftingService.IsFinalItem(CraftItemEntity) == true)
			{
				// TODO : Trigger special animation?
				Recycle(true);
			}
		}

		// TODO : Check on recipe unlocking to see if depleted items should be recycled.
		#endregion

		#region -- Public Methods --
		/// <summary>
		/// Trigger animation for invalid combination.
		/// </summary>
		public void HandleInvalidCombination()
		{
			PlayAnimation(GameConstants.InvalidCombinationAnimKey);
		}

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
				var currentPlayable = GetCurrentPlayingDirector();
				currentPlayable.StopSafe();
			}
			var animation = AnimationMap.FirstOrDefault(a => a.Key == key);
			if (animation != null)
			{
				animation.Director.PlaySafe();
			}
		}

		/// <summary>
		/// Get the currently playing PlayableDirector.
		/// </summary>
		/// <returns>Return PlayableDirector thats play state is playing.</returns>
		public PlayableDirector GetCurrentPlayingDirector()
		{
			PlayableDirector director = null;
			foreach (var a in AnimationMap)
			{
				if (a.Director == null)
				{
					continue;
				}
				if (a.Director.state == PlayState.Playing)
				{
					return a.Director;
				}
			}
			return director;
		}

		/// <summary>
		/// If there is currently a playing PlayableDirector return its duration
		/// </summary>
		/// <returns>Duration of currently playing PlayableDirector.</returns>
		public float GetCurrentPlayingDirectorLength()
		{
			var director = GetCurrentPlayingDirector();
			if (director!= null && director.playableAsset != null)
			{
				return (float)director.playableAsset.duration;
			}
			return 0f;
		}

		/// <summary>
		/// Set visuals of the CraftItemInstance.
		/// </summary>
		/// <param name="key">Id of visual.</param>
		public void SetVisual(string key)
		{
			if (_imageService == null)
			{
				_imageService = CraftingSystemImageService.Instance;
			}

			var sprite = _imageService.GetCraftImage(key);
			if (sprite == null)
			{
				return;
			}

			SpriteRendererVisual.SetSpriteSafe(sprite);
			ImageVisual.SetSpriteSafe(sprite);
		}

		/// <summary>
		/// Set visuals of the CraftItemInstance.
		/// </summary>
		/// <param name="entity">CraftItemEntity to base the visual off of.</param>
		public void SetVisual(CraftItemEntity entity)
		{
			if (entity == null)
			{
				Logger.LogError("CraftItemInstance_Visual.SetVisual : Entity was null.");
				return;
			}
			SetVisual(entity.CraftItemData.CraftItemModelData.VisualKey);
		}

		/// <summary>
		/// Set visuals of the CraftItemInstance using the CraftItemEntity property.
		/// </summary>
		public void SetVisual()
		{
			if (CraftItemEntity == null)
			{
				Logger.LogError("CraftItemInstance_Visual.SetVisual : CraftItemEntity was null.");
				return;
			}
			SetVisual(CraftItemEntity);
		}
		#endregion
	}
}
