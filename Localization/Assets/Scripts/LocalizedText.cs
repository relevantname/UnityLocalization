using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizedText : MonoBehaviour
{
    private void Start()
    {
        Text text = GetComponent<Text>();
        text.text = LocalizationManager.Instance.GetLocalizedText(text.text);
    }
}
