using UnityEditor;

namespace FishAndChips
{
    [CustomEditor(typeof(BaseButton), editorForChildClasses: true)]
    public class BaseButtonEditor : Editor
    {
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
		}
	}
}
