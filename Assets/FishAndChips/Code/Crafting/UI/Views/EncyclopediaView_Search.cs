using UnityEngine;

namespace FishAndChips
{
    public partial class EncyclopediaView
    {
		#region -- Private Member Vars --
		private string _lastSearch;
		#endregion

		#region -- Private Methods --
		private void ClearActiveSearches()
		{
			var searchComponents = FindObjectsByType<CraftItemSearch>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			foreach (var component in searchComponents)
			{
				component.ClearSearch();
			}
		}

		private void OnSearchRaised(CraftItemSearchEvent gameEvent)
		{
			if (gameObject.activeInHierarchy == false)
			{
				return;
			}
			SetupCraftItemScrollRect(gameEvent.SearchFilter);
		}

		private bool IsValidForDisplaying(CraftItemEntity entity)
		{
			return _craftingService.IsCraftItemValidForDisplaying(entity, _lastSearch, _selectedKeywords, true);
		}
		#endregion
	}
}
