using UnityEditor;

namespace FishAndChips
{
    [CustomEditor(typeof(SaveTester))]
    public class SaveTesterEditor : FishAndChipsEditor
    {
		#region -- Public Methods --
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			var tester = (SaveTester)target;

			DrawButton("Test Save", () => tester.TestSave());
			DrawButton("Test Load", () => tester.TestLoad());
			DrawButton("Test Delete", () => tester.TestDelete());
			DrawButton("Increase Counter", () => tester.IncreaseCounter());
			DrawButton("Print", () => tester.PrintPersistentDataPath());
		}
		#endregion
	}
}
