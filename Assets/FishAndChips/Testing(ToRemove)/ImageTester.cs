using UnityEngine;
using UnityEngine.U2D;

namespace FishAndChips
{
    public class ImageTester : MonoBehaviour
    {
		public SpriteAtlas Atlas;
		public SpriteRenderer Renderer;

		private void Start()
		{
			var sprite = Atlas.GetSprite("TestSprite");
			if (sprite != null)
			{
				Renderer.sprite = sprite;
			}
		}
	}
}
