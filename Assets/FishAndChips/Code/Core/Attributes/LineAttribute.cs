using UnityEngine;

namespace FishAndChips
{
    public class LineAttribute : PropertyAttribute
    {
        public float Height;
        public Color LineColor;

        public LineAttribute(float r = 0,
            float g = 0, 
            float b = 0,
            float height = 3)
        {
            Height = height;
            LineColor = new Color(r/255f, g/255f, b/255f);
        }
    }
}
