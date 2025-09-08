using System;
using UnityEngine;

namespace FishAndChips
{
	// TODO : Remove.
	[Serializable]
	public class TestSaveData : SavedData
	{
		#region -- Public Meember Vars --
		public int Counter = 0;
		#endregion

		#region -- Constructor --
		public TestSaveData(string saveId) : base(saveId)
		{
			Load();
		}
		#endregion

		#region -- Public Methods --
		public void IncreaseCounter()
		{
			Counter++;
			Debug.Log($"IncreaseCounter {Counter}");
		}
		#endregion
	}

    public class SaveTester : MonoBehaviour
    {
		#region -- Inspector --
		public string TestFileName;
		public string TestFilePath;
		#endregion

		#region -- Private Member Vars --
		private TestSaveData _testData;
		#endregion

		#region -- Private Methods --
		private void Awake()
		{
			_testData = new TestSaveData(TestFileName);
		}
		#endregion

		#region -- Public Methods --
		public void TestSave()
		{
			_testData.Save();
		}

		public void TestLoad()
		{
			_testData.Load();
		}

		public void TestDelete()
		{
			_testData.Delete();
		}

		public void IncreaseCounter()
		{
			if (_testData == null)
			{
				return;
			}
			_testData.IncreaseCounter();
		}

		public void PrintPersistentDataPath()
		{
			Debug.Log(Application.persistentDataPath);
		}
		#endregion
	}
}
