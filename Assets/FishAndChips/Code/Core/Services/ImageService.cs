using UnityEngine;
using UnityEngine.U2D;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FishAndChips
{
	//public class ImageService<T> : Singleton<T> where T : Singleton<T>
	public class ImageService : Singleton<ImageService>
	{
		#region -- Protected Member Vars --
		protected Sprite _defaultSprite;
		protected SpriteAtlas _iconSpriteAtlas;

		protected List<string> _levelAtlases = new();

		protected Dictionary<string, SpriteAtlas> _spriteAtlases = new();
		protected Dictionary<string, SpriteAtlas> _permanentSpriteAtlases = new();

		protected Dictionary<string, List<Action<SpriteAtlas>>> _requestAtlasCallbackDict = new();

#if FISH_ADDRESSABLE
		protected List<object> _levelAddressables = new();
		protected Dictionary<object, List<string>> _spriteAtlasAddressableDictByKey = new();

		// Services.
		protected AddressableService _addressableService;
#endif

		#endregion

		#region -- Private Methods --
		private SpriteAtlas LoadSpriteAtlas(string atlasName)
		{
			if (_permanentSpriteAtlases.ContainsKey(atlasName))
			{
				return _permanentSpriteAtlases[atlasName];
			}
			if (_spriteAtlases.ContainsKey(atlasName))
			{
				return _spriteAtlases[atlasName];
			}
			return null;
		}

		private void RequestAtlas(string tag, Action<SpriteAtlas> callback)
		{
			if (_permanentSpriteAtlases.ContainsKey(tag))
			{
				callback(_permanentSpriteAtlases[tag]);
			}
			else if (_spriteAtlases.ContainsKey(tag))
			{
				callback(_spriteAtlases[tag]);
			}
			else
			{
				if (_requestAtlasCallbackDict.ContainsKey(tag))
				{
					if (_requestAtlasCallbackDict[tag] == null)
					{
						_requestAtlasCallbackDict[tag] = new();
					}
					_requestAtlasCallbackDict[tag].Add(callback);
				}
				else
				{
					_requestAtlasCallbackDict.Add(tag, new());
					_requestAtlasCallbackDict[tag].Add(callback);
				}
			}
		}
		#endregion

		#region -- Protected Methods --
		protected void LoadDefaultSprite()
		{
			_defaultSprite = GetImage(CoreConstants.DefaultImageName);
		}

		protected Sprite FetchFromDictionary(Dictionary<string, Sprite> spriteDictionary, string imageName)
		{
			Sprite returnSprite = null;
			if (imageName.IsNullOrEmpty() == false 
				&& spriteDictionary.TryGetValue(imageName, out returnSprite))
			{
				return returnSprite;
			}

			Logger.LogError($"[ImageService] Attempt to find image {imageName} failed");
			return null;
		}
		#endregion

		#region -- Public Methods --
		public override void Initialize()
		{
			base.Initialize();
			_levelAtlases = new();
			_spriteAtlases = new();
			_permanentSpriteAtlases = new();

			_requestAtlasCallbackDict = new();
			SpriteAtlasManager.atlasRequested += RequestAtlas;
		}

		public override void Cleanup()
		{
			UnloadLevelContext();

			_defaultSprite = null;
			_spriteAtlases.Clear();
			_requestAtlasCallbackDict.Clear();
		}

		public virtual async Task Load()
		{
			await LoadSpriteAtlases();
		}

		public virtual async Task LoadSpriteAtlases()
		{
			await Task.WhenAll(
					LoadSpriteAtlasFromRresources(CoreConstants.DefaultUIIconAtlasKey, true)
				);

			TryGetSpriteAtlases(CoreConstants.DefaultUIIconAtlasKey, out _iconSpriteAtlas);
			LoadDefaultSprite();
		}

		public void UnloadLevelContext()
		{
			foreach (var spriteAtlasName in _levelAtlases)
			{
				UnloadSpriteAtlas(spriteAtlasName);
			}
			_levelAtlases.Clear();
		}

		public void TryGetSpriteAtlases(string atlasName, out SpriteAtlas spriteAtlas)
		{
			_permanentSpriteAtlases.TryGetValue(atlasName, out spriteAtlas);
			if (spriteAtlas == null)
			{
				_spriteAtlases.TryGetValue(atlasName, out spriteAtlas);
			}
		}

		public void LoadSpriteAtlas(SpriteAtlas atlas, bool permanent = false)
		{
			if (permanent == true)
			{
				if (_permanentSpriteAtlases.ContainsKey(atlas.name) == true)
				{
					_permanentSpriteAtlases[atlas.name] = atlas;
				}
				else
				{
					_permanentSpriteAtlases.Add(atlas.name, atlas);
				}
			}
			else
			{
				if (_spriteAtlases.ContainsKey(atlas.name) == true)
				{
					_spriteAtlases[atlas.name] = atlas;
				}
				else
				{
					_spriteAtlases.Add(atlas.name, atlas);
				}
			}

			var tag = atlas.name;
			if (_requestAtlasCallbackDict.ContainsKey(tag))
			{
				foreach (var callback in _requestAtlasCallbackDict[tag])
				{
					callback(atlas);
				}
				_requestAtlasCallbackDict.Remove(tag);
			}
		}

		public void UnloadSpriteAtlas(string atlasName)
		{
			if (_permanentSpriteAtlases.ContainsKey(atlasName))
			{
				_permanentSpriteAtlases.Remove(atlasName);
			}
			if (_spriteAtlases.ContainsKey(atlasName))
			{
				_spriteAtlases.Remove(atlasName);
			}
		}

		public bool IsDefaultSprite(Sprite sprite)
		{
			return sprite == GetDefaultSprite();
		}

		public Sprite GetDefaultSprite()
		{
			if (_defaultSprite == null)
			{
				LoadDefaultSprite();
			}
			return (_defaultSprite != null) ? _defaultSprite : default;
		}

		public virtual Sprite GetImage(string imageName)
		{
			// Try permanent atlases first.
			var image = _permanentSpriteAtlases.Values.Select(atlas => atlas == null ? null : atlas.GetSprite(imageName)).FirstOrDefault(sprite => sprite != null);

			// If contained in permanent return it, otherwise check non permanent sprite atlases.
			return (image != null) ? image : _spriteAtlases.Values.Select(atlas => atlas == null ? null : atlas.GetSprite(imageName)).FirstOrDefault(sprite => sprite != null);
		}

		public virtual Sprite GetImageSafe(string imageName)
		{
			return CheckNullAndReturnDefaultSprite(GetImage(imageName));
		}

		public Sprite CheckNullAndReturnDefaultSprite(Sprite sprite)
		{
			return sprite == null ? GetDefaultSprite() : sprite;
		}

		public async Task<string> LoadSpriteAtlasFromRresources(string path, bool permanent)
		{
			var atlasRequest = Resources.LoadAsync<SpriteAtlas>(path);
			while (atlasRequest.isDone == false)
			{
				await Awaitable.EndOfFrameAsync();
			}
			var atlas = atlasRequest.asset as SpriteAtlas;
			if (atlas == null)
			{
				return String.Empty;
			}
			LoadSpriteAtlas(atlas, permanent);
			return atlas.name;
		}
		#endregion
	}
}

