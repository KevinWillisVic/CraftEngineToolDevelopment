using UnityEngine.SceneManagement;

namespace FishAndChips
{
    public class SplashView : GameView
    {
		#region -- Inspector --
		public string SceneToTransition = "BootScene";
		#endregion

		#region -- Private Methods --
		private void Start()
		{
			SceneManager.LoadSceneAsync(SceneToTransition, LoadSceneMode.Additive);
		}
		#endregion
	}
}
