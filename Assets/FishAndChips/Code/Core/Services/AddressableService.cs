using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using System;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace FishAndChips
{
    public class AddressableService : Singleton<AddressableService>
    {
		#region -- Private Member Vars --
		private List<object> _levelKeys = new List<object>();
		private List<object> _loadedKeys = new List<object>();
		private Dictionary<object, AsyncOperationHandle> _loadedHandles = new Dictionary<object, AsyncOperationHandle>();
		private Dictionary<string, AsyncOperationHandle<SceneInstance>> _loadedScenes = new Dictionary<string, AsyncOperationHandle<SceneInstance>>();
		private Dictionary<object, int> _loadedCounter = new Dictionary<object, int>();
		#endregion

		#region -- Private Methods --
		private void AddLevelKey(AssetReference assetReference)
		{
			AddLevelKey(assetReference.RuntimeKey);
		}

		private void AddLevelKey(object key)
		{
			if (_levelKeys.Contains(key) == false)
			{
				_levelKeys.Add(key);
			}
		}

		private void IncreaseCounter(object key)
		{
			if (_loadedCounter.ContainsKey(key) == false)
			{
				_loadedCounter.Add(key, 0);
			}
			_loadedCounter[key]++;
		}

		private void ReleaseHandle(object key, AsyncOperationHandle handle)
		{
			try
			{
				if (handle.IsValid())
				{
					Addressables.Release(handle);
				}
				else
				{
					Logger.LogWarning($"[AddressableService][ReleaseHandle] issue handle invalid {key}");
				}
			}
			catch (Exception e)
			{
				Logger.LogWarning($"[AddressableService][ReleaseHandle] issue\n{e}");
			}
		}

		private async Task<T> RunAsyncOperation<T>(object key, AsyncOperationHandle<T> handle, bool throwException = false, bool trackHandle = true) where T : class
		{
			if (trackHandle == true)
			{
				if (!_loadedHandles.ContainsKey(key))
				{
					_loadedHandles.Add(key, handle);
				}
				else
				{
					throw new Exception($"Trying to load key {key} that has already been loaded.");
				}
			}
			T result = null;
			try
			{
				await handle.Task;
				if (handle.Status != AsyncOperationStatus.Succeeded)
				{
					if (trackHandle)
					{
						_loadedHandles.Remove(key);
					}
					Addressables.Release(handle);
					if (throwException)
					{
						throw handle.OperationException;
					}
				}
				else
				{
					result = handle.Result;
					if (trackHandle && !_loadedKeys.Contains(key))
					{
						_loadedKeys.Add(key);
					}
					IncreaseCounter(key);
				}
			}
			catch (Exception e)
			{
				if (trackHandle)
				{
					_loadedHandles.Remove(key);
				}
				Logger.LogWarning($"[AddressableService][RunAsyncOperation] Key {key} issue\n{e}.");
				if (throwException)
				{
					throw;
				}
			}
			return result;
		}
		#endregion

		#region -- Public Methods --
		public override void Initialize()
		{
			_loadedKeys = new List<object>();
			_levelKeys = new List<object>();
			_loadedHandles = new Dictionary<object, AsyncOperationHandle>();
			_loadedCounter = new Dictionary<object, int>();
			_loadedScenes = new Dictionary<string, AsyncOperationHandle<SceneInstance>>();

			InitializeAsync();
		}

		public async void InitializeAsync()
		{
			// Initializing Addressables is a preliminary operation that has the responsibility of setting up the runtime data for Addressables
			var handle = Addressables.InitializeAsync();
			await handle.Task;
		}

		public override void Cleanup()
		{
			try
			{
				ReleaseLevelContext();
				foreach (var kvp in _loadedHandles)
				{
					ReleaseHandle(kvp.Key, kvp.Value);
				}
			}
			catch (Exception e)
			{
				Logger.LogWarning($"[AddressableService][Cleanup] issue\n{e}");
			}
			finally
			{
				_loadedKeys.Clear();
				_loadedHandles.Clear();
				_loadedCounter.Clear();
			}
		}

		public void ReleaseLevelContext()
		{
			foreach (var key in _levelKeys)
			{
				Release(key, removeLevelKeys: false, force: true);
			}
			_levelKeys.Clear();
		}

		public void Release(AssetReference assetReference, bool force = false)
		{
			var key = assetReference.RuntimeKey;
			Release(key, force: force);
		}

		public bool Release(object key, bool removeLevelKeys = true, bool force = false, int unloadCounter = 1)
		{
			if (force == false && _loadedCounter.ContainsKey(key))
			{
				_loadedCounter[key] -= unloadCounter;
				if (_loadedCounter[key] > 0)
				{
					return false;
				}
			}

			if (force && _loadedCounter.ContainsKey(key))
			{
				_loadedCounter[key] = 0;
			}

			try
			{
				if (_loadedHandles.ContainsKey(key))
				{
					ReleaseHandle(key, _loadedHandles[key]);
				}
			}
			catch (Exception e)
			{
				Logger.LogWarning($"[AddressableService][Release] issue. Key {key} release issue.\n{e}");
			}
			finally
			{
				if (_loadedKeys.Contains(key))
				{
					_loadedKeys.Remove(key);
				}

				if (_loadedHandles.ContainsKey(key))
				{
					_loadedHandles.Remove(key);
				}

				if (removeLevelKeys && _levelKeys.Contains(key))
				{
					_levelKeys.Remove(key);
				}
			}
			return true;
		}

		public async Task<IList<T>> LoadAssets<T>(IList<object> keys, bool throwException = false, bool trackHandle = true) where T : Object
		{
			var handle = Addressables.LoadAssetsAsync<T>(keys as IEnumerable, go => { }, Addressables.MergeMode.None);
			return await RunAsyncOperation(keys, handle, throwException, trackHandle);
		}

		public async Task<IList<T>> LoadAssets<T>(object key, bool throwException = false, bool trackHandle = true) where T : Object
		{
			if (_loadedHandles.ContainsKey(key))
			{
				while (_loadedHandles[key].IsDone == false)
				{
					await Awaitable.EndOfFrameAsync();
				}
				IncreaseCounter(key);
				return _loadedHandles[key].Result as IList<T>;
			}
			var handle = Addressables.LoadAssetsAsync<T>(key, go => { });
			return await RunAsyncOperation(key, handle, throwException, trackHandle);
		}

		public async Task<T> LoadAsset<T>(AssetReference assetReference, bool throwException = false, bool trackHandle = true) where T : Object
		{
			if (assetReference.RuntimeKeyIsValid() == false)
			{
				return null;
			}
			return await LoadAsset<T>(assetReference.RuntimeKey, throwException, trackHandle);
		}

		public async Task<T> LoadAsset<T>(object key, bool throwException = false, bool trackHandle = true) where T : Object
		{
			if (_loadedHandles.ContainsKey(key))
			{
				while (_loadedHandles[key].IsDone == false)
				{
					await Awaitable.EndOfFrameAsync();
				}
				IncreaseCounter(key);
				return _loadedHandles[key].Result as T;
			}

			var handle = Addressables.LoadAssetAsync<T>(key);
			return await RunAsyncOperation(key, handle, throwException, trackHandle);
		}

		public async Task<T> LoadAssetInLevelContext<T>(AssetReference assetReference, bool throwException = false, bool trackHandle = true) where T : Object
		{
			var key = assetReference.RuntimeKey;
			var result = await LoadAsset<T>(assetReference, throwException, trackHandle);
			if (result != null)
			{
				AddLevelKey(key);
			}
			return result;
		}

		public async Task<T> LoadAssetInLevelContext<T>(object key, bool throwException = false, bool trackHandle = true) where T : Object
		{
			var result = await LoadAsset<T>(key, throwException, trackHandle);
			if (result != null)
			{
				AddLevelKey(key);
			}
			return result;
		}

		public async Task<GameObject> LoadGameObject(AssetReference assetReference, bool throwException = false, bool trackHandle = true)
		{
			if (!assetReference.RuntimeKeyIsValid())
			{
				return null;
			}

			var key = assetReference.RuntimeKey;
			if (_loadedHandles.ContainsKey(key))
			{
				while (_loadedHandles[key].IsDone == false)
				{
					await Awaitable.EndOfFrameAsync();
				}
				IncreaseCounter(key);
				return _loadedHandles[key].Result as GameObject;
			}

			var handle = assetReference.LoadAssetAsync<GameObject>();
			return await RunAsyncOperation(key, handle, throwException, trackHandle);
		}
		public async Task<T> LoadGameObject<T>(AssetReference assetReference, bool throwException = false, bool trackHandle = true) where T : Object
		{
			var result = await LoadGameObject(assetReference, throwException, trackHandle);
			return result != null ? result.GetComponent<T>() : null;
		}

		public async Task<GameObject> LoadGameObject(object key, bool throwException = false, bool trackHandle = true)
		{
			if (_loadedHandles.ContainsKey(key))
			{
				while (_loadedHandles[key].IsDone == false)
				{
					await Awaitable.EndOfFrameAsync();
				}
				IncreaseCounter(key);
				return _loadedHandles[key].Result as GameObject;
			}

			var handle = Addressables.LoadAssetAsync<GameObject>(key);
			return await RunAsyncOperation(key, handle, throwException, trackHandle);
		}
		public async Task<T> LoadGameObject<T>(object key, bool throwException = false, bool trackHandle = true) where T : Object
		{
			var result = await LoadGameObject(key, throwException, trackHandle);
			return result != null ? result.GetComponent<T>() : null;
		}

		public async Task<GameObject> LoadGameObjectInLevelContext(AssetReference assetReference, bool throwException = false, bool trackHandle = true)
		{
			var result = await LoadGameObject(assetReference, throwException, trackHandle);
			if (result != null)
			{
				AddLevelKey(assetReference);
			}
			return result;
		}

		public async Task<T> LoadGameObjectInLevelContext<T>(AssetReference assetReference, bool throwException = false, bool trackHandle = true) where T : Object
		{
			var result = await LoadGameObjectInLevelContext(assetReference, throwException, trackHandle);
			return result != null ? result.GetComponent<T>() : null;
		}

		public async Task<GameObject> LoadGameObjectInLevelContext(object key, bool throwException = false, bool trackHandle = true)
		{
			var result = await LoadGameObject(key, throwException, trackHandle);
			if (result != null)
			{
				AddLevelKey(key);
			}
			return result;
		}

		public async Task<T> LoadGameObjectInLevelContext<T>(object key, bool throwException = false, bool trackHandle = true) where T : Object
		{
			var result = await LoadGameObjectInLevelContext(key, throwException, trackHandle);
			return result != null ? result.GetComponent<T>() : null;
		}
		#endregion
	}
}
