using System;

namespace FishAndChips
{
    /// <summary>
    /// Save structure for common saveables.
    /// </summary>
    [Serializable]
    public class GeneralSaveInfo : SavedData
    {
        #region -- Public Member Vars --
        // Should SFX play or be muted.
        public bool IsSFXOn;
        // Should Music play or be muted.
        public bool IsMusicOn;
		#endregion

		#region -- Constructors --
		public GeneralSaveInfo(string saveId) : base(saveId) 
        {
        }
		#endregion

		#region -- Public Methods --
        /// <summary>
        /// Reset save state to default.
        /// </summary>
		public override void Reset()
		{
            base.Reset();
            IsSFXOn = true;
            IsMusicOn = true;
            Save();
		}

        /// <summary>
        /// Set save state of sfx setting.
        /// </summary>
        /// <param name="state">State to set sfx setting.</param>
        public void SetSFXState(bool state)
        {
            IsSFXOn = state;
            Save();
        }

        /// <summary>
        /// Set save state of music setting.
        /// </summary>
        /// <param name="state">State to set music setting.</param>
        public void SetMusicState(bool state)
        {
            IsMusicOn = state;
            Save();
        }
        #endregion
    }
}
