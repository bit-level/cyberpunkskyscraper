using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour
{
    [SerializeField] private List<Language> languages;
    [SerializeField] private List<string> texts;
    [SerializeField] private Text text;

    private void Start()
    {
        LocalizationManager.Instance.Register(this);
    }

    public void SetLanguage(Language language)
    {
        for (int i = 0; i < languages.Count; i++)
        {
            if (language == languages[i])
            {
                text.text = texts[i];
                break;
            }
        }
    }
}