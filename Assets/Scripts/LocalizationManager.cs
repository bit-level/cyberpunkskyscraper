using System.Collections.Generic;
using UnityEngine;
using YG;

public class LocalizationManager : MonoBehaviour
{
    private static readonly List<LocalizedText> texts = new();

    private void Awake()
    {
        YG2.onCorrectLang += OnCorrectLang;
    }

    private void Start()
    {
        SetLanguage(YG2.lang == "ru" ? Language.Russian : Language.English);
    }

    public static void Register(LocalizedText text)
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

    private void OnCorrectLang(string lang)
    {
        if (lang == "ru" ||
            lang == "be" ||
            lang == "kk" ||
            lang == "uk" ||
            lang == "uz")
        {
            YG2.lang = "ru";
        }
        else
        {
            YG2.lang = "en";
        }
    }
}