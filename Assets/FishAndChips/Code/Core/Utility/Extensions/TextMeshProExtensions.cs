using UnityEngine;
using TMPro;
using System;

namespace FishAndChips
{
    public static class TextMeshProExtensions
    {
        public static void SetTextSafe(this TMP_Text text, string message)
        {
            if (text != null)
            {
                text.SetText(message);
            }
        }

        public static void SetTextColorSafe(this TMP_Text text, Color color)
        {
            if (text != null)
            {
                text.color = color;
            }
        }

        public static void SetInputFieldTextSafe(this TMP_InputField inputField, string message)
        {
            if (inputField != null)
            {
                inputField.text = message;
            }
        }

        public static void RemoveInputFieldListenersSafe(this TMP_InputField inputField)
        {
            if (inputField != null)
            {
                inputField.onValueChanged.RemoveAllListeners();
            }
        }

        public static void AddInputFieldListenerSafe(this TMP_InputField inputField, UnityEngine.Events.UnityAction<string> listener)
        {
            if (inputField != null)
            {
                inputField.onValueChanged.AddListener(listener);
            }
        }
    }
}
