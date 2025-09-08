using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

namespace FishAndChips
{
	/// <summary>
	/// Wrapper for scriptable objects.
	/// </summary>
    public class ScriptableObjectData : ScriptableObject
    {
		#region -- Public Member Vars --
		public string GUID;
		#endregion

		#region -- Private Methods --
		private void Awake()
		{
			if (GUID.IsNullOrEmpty())
			{
				CreateGUID();
			}
		}
		#endregion

		#region -- Public Methods --
		/// <summary>
		/// Generate unique identifier for the ScriptableObject.
		/// </summary>
		public void CreateGUID()
		{
			GUID = Guid.NewGuid().ToString();
#if UNITY_EDITOR
			EditorUtility.SetDirty(this);
#endif
		}

		/// <summary>
		/// Compare ScriptableObject with object for equality.
		/// </summary>
		/// <param name="other"></param>
		/// <returns>True if the same object, false otherwise.</returns>
		public override bool Equals(object other)
		{
			return Equals(other as ScriptableObjectData);
		}

		public bool Equals(ScriptableObjectData other)
		{
			if (other == null)
			{
				return false;
			}
			if (System.Object.ReferenceEquals(this, other))
			{
				return true;
			}
			return GUID != null && GUID.Equals(other.GUID);
		}

		public static bool operator ==(ScriptableObjectData a, ScriptableObjectData b)
		{
			if (a is null)
			{
				if (b is null)
				{
					return true;
				}
				return false;
			}
			return a.Equals(b);
		}

		public static bool operator !=(ScriptableObjectData a, ScriptableObjectData b)
		{
			return !(a == b);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		#endregion
	}
}
