using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D;

namespace FishAndChips
{
	//public class CraftingSystemImageService : ImageService<CraftingSystemImageService>
	public class CraftingSystemImageService : ImageService
	{
		
		#region -- Properties --
		
		public static new CraftingSystemImageService Instance
		{
			get
			{
				lock (_lock)
				{
					if (_instance == null)
					{
						_instance = (CraftingSystemImageService)Object.FindAnyObjectByType(typeof(CraftingSystemImageService));
						
						if (_instance == null)
						{
							GameObject singletonObject = new GameObject();
							_instance = singletonObject.AddComponent<CraftingSystemImageService>();
							singletonObject.name = $"Singleton {typeof(CraftingSystemImageService).ToString()}";
							DontDestroyOnLoad(singletonObject);
						}
					}
					return _instance as CraftingSystemImageService;
				}
			}
		}
		#endregion


		#region -- Private Member Vars --
		private SpriteAtlas _craftingAtlas = null;
		#endregion

		#region -- Public Methods --
		public override async Task LoadSpriteAtlases()
		{
			await base.LoadSpriteAtlases();
			await Task.WhenAll(
					LoadSpriteAtlasFromRresources(GameConstants.CraftItemAtlas, false)
				);

			TryGetSpriteAtlases(GameConstants.CraftItemAtlas, out _craftingAtlas);
		}

		/// <summary>
		/// Get visual of CraftItem.
		/// </summary>
		/// <param name="imageId">Id of CraftItem we are getting a visual for.</param>
		/// <returns>A sprite for a CraftItem.</returns>
		public Sprite GetCraftImage(string imageId)
		{
			Sprite sprite = null;
			if (_craftingAtlas != null)
			{
				sprite = _craftingAtlas.GetSprite(imageId);
			}
			return sprite != null ? sprite : GetImageSafe(imageId);
		}
		#endregion
	}
}
