using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using TMPro;
using System.Collections.Generic;

namespace FishAndChips
{
    public class OverlayCraftItemUnlock : GameOverlay
    {
		#region -- Inspector --
		public Image CraftItemVisual;
		public TextMeshProUGUI CraftItemNameText;
		public PlayableDirector CraftItemUnlockSequence;
		#endregion

		#region -- Protected Member Vars --
		protected CraftingSystemImageService _imageService;
		#endregion

		#region -- Private Member Vars --
		private List<CraftItemEntity> _queuedEntities = new();
		private bool _waitingForUserInteraction = false;
		#endregion

		#region -- Private Methods --
		private async void DisplayEntity(CraftItemEntity entity)
		{
			if (entity == null)
			{
				return;
			}
			var modelData = entity.CraftItemData.CraftItemModelData;
			if (modelData == null)
			{
				return;
			}
			if (_imageService != null)
			{
				var craftItemSprite = _imageService.GetCraftImage(modelData.VisualKey);
				CraftItemVisual.SetSpriteSafe(craftItemSprite);
			}
			CraftItemNameText.SetTextSafe(modelData.DisplayName);
			// TODO : Check wait conditions.
			while (gameObject != null && gameObject.activeInHierarchy == false)
			{
				await Awaitable.EndOfFrameAsync();
			}

			await CraftItemUnlockSequence.AwaitPlayable();

			_waitingForUserInteraction = true;
			while (_waitingForUserInteraction == true)
			{
				await Awaitable.EndOfFrameAsync();
			}

			if (_queuedEntities.Count > 0)
			{
				entity = _queuedEntities.Pop(0);
				DisplayEntity(entity);
			}
			else
			{
				DismissSelected();
			}
		}
		#endregion

		#region -- Public Methods --
		public override void Initialize()
		{
			base.Initialize();
			_imageService = CraftingSystemImageService.Instance;
		}

		public override void Deactivate()
		{
			base.Deactivate();
			var gameplayView = _uiService.GetView(UIEnumTypes.eViewType.GameplaySceneView.ToString()) as GameplaySceneView;
			if (gameplayView != null && gameplayView.SimpleGameplayBoard != null)
			{
				gameplayView.SimpleGameplayBoard.RecycleUnusableItems();
			}
		}

		public void AddCraftItemToQueue(CraftItemEntity entity)
		{
			if (entity == null)
			{
				return;
			}
			if (_queuedEntities.Contains(entity) == false)
			{
				_queuedEntities.Add(entity);
			}

			if (_queuedEntities.Count == 1)
			{
				var toShow = _queuedEntities.Pop(0);
				DisplayEntity(toShow);
			}
		}

		public void RecieveUserInteraction()
		{
			_waitingForUserInteraction = false;
		}
		#endregion
	}
}
