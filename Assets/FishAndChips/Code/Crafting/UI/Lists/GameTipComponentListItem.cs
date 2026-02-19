using TMPro;
using UnityEngine;

namespace FishAndChips
{
	/// <summary>
	/// ComponentListItem representing a tip in the game.
	/// </summary>
    public class GameTipComponentListItem : ComponentListItem
    {
		#region -- Properties --
		public GameTipData TipData => _tipData;
		#endregion

		#region -- Inspector --
		[Header("Game Tip UI References")]
		public TextMeshProUGUI TipTitle;
		public TextMeshProUGUI TipDescription;
		#endregion

		#region -- Private Member Vars --
		private GameTipData _tipData;
		#endregion

		#region -- Protected Methods --
		/// <summary>
		/// Set UI text references.
		/// </summary>
		protected override void SetUpTextReferences()
		{
			base.SetUpTextReferences();
			TipTitle.SetTextSafe(_tipData.TipTitle);
			TipDescription.SetTextSafe(_tipData.Tip);
		}
		#endregion

		#region -- Public Methods --
		public override void Initialize()
		{
			_tipData = ListObject as GameTipData;
			if (_tipData == null)
			{
				return;
			}
			base.Initialize();
		}
		#endregion
	}
}
