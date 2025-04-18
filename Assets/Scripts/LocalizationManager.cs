using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    private readonly List<LocalizedText> texts = new();

    public static LocalizationManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Register(LocalizedText text)
    {
        texts.Add(text);
    }

    public void SetLanguage(Language language)
    {
        for (int i = 0; i < texts.Count; i++)
        {
            texts[i].SetLanguage(language);
        }
    }
}