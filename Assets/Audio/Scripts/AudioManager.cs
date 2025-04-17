using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] musicClips;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private float[] musicVolumes;

    [Space(10f)]
    [SerializeField] private AudioClip[] windClips;
    [SerializeField] private AudioSource windSource;
    [SerializeField] private float[] windVolumes;

    [Space(10f)]
    [SerializeField] private AudioClip[] thunderClips;
    [SerializeField] private AudioSource thunderSource;
    [SerializeField] private float[] thunderVolumes;
    [SerializeField] private float thunderIntervalMin, thunderIntervalMax;

    [Header("Settings")]
    [SerializeField] private bool disableMusic;
    [SerializeField] private bool disableWind;
    [SerializeField] private bool disableThunder;

    [Header("Others")]
    [SerializeField] private AudioSource start;
    [SerializeField] private AudioSource levelUp, newBestScore, gameOver, perfect;

    private AudioElement _music;
    private AudioElement _wind;
    private AudioElement _thunder;
    private float _lastThunderTime;
    private float _thunderInterval;


    private void Awake()
    {
        _music = disableMusic ? null : new AudioElement(musicSource, musicClips, musicVolumes);
        _wind = disableWind ? null : new AudioElement(windSource, windClips, windVolumes);
        _thunder = disableThunder ? null : new AudioElement(thunderSource, thunderClips, thunderVolumes);
        _thunderInterval = Random.Range(thunderIntervalMin, thunderIntervalMax);

        _music?.PlayRandom();
        _wind?.PlayRandom();
    }

    private void Start()
    {
        Skyscraper.Instance.OnGameStart += start.Play;
        LevelSystem.Instance.OnRankUp += levelUp.Play;
        Combo.Instance.OnComboEnd += () => perfect.pitch = 1;
        Skyscraper.Instance.OnGameOver += (score, bestScore) =>
        {
            if (bestScore) newBestScore.Play();
            else gameOver.Play();
            perfect.pitch = 1;
        };
        Skyscraper.Instance.OnPerfectTap += () =>
        {
            perfect.Play();
            perfect.pitch = Mathf.Clamp(perfect.pitch + .1f, 1f, 3f);
        };
    }

    private void Update()
    {
        UpdateThunder();
    }

    private void UpdateThunder()
    {
        if (_thunder == null) return;
        if (Time.time - _lastThunderTime < _thunderInterval) return;
        _thunder.PlayRandom();
        _lastThunderTime = Time.time;
        _thunderInterval = Random.Range(thunderIntervalMin, thunderIntervalMax);
    }

    private class AudioElement
    {
        private readonly AudioSource source;
        private readonly AudioClip[] clips;
        private readonly float[] volumes;

        public AudioElement(AudioSource source, AudioClip[] clips, float[] volumes)
        {
            this.source = source;
            this.clips = clips;
            this.volumes = volumes;
        }

        public void PlayRandom()
        {
            int index = Random.Range(0, clips.Length);
            source.clip = clips[index];
            source.volume = volumes[index];
            source.Play();
        }

        public void Stop()
        {
            source.Stop();
        }
    }
}
