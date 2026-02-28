using UnityEngine.Playables;
using System;

namespace FishAndChips
{
    /// <summary>
    /// Key Value pairing for playable director.
    /// </summary>
    [Serializable]
    public class AnimationLookupKey
    {
        #region -- Inspector --
        public string Key;
        public PlayableDirector Director;
        #endregion
    }
}
