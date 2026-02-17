using System;

namespace FishAndChips
{
    [Serializable]
    public class GeneralSaveInfo : SavedData
    {
        #region -- Public Member Vars --
        public bool IsSFXOn;
        public bool IsMusicOn;
		#endregion

		#region -- Constructors --
		public GeneralSaveInfo(string saveId) : base(saveId) 
        {
        }

		public override void Reset()
		{
            base.Reset();
            IsSFXOn = true;
            IsMusicOn = true;
            Save();
		}

        public void SetSFXState(bool state)
        {
            IsSFXOn = state;
            Save();
        }

        public void SetMusicState(bool state)
        {
            IsMusicOn = state;
            Save();
        }
        #endregion
    }
}
