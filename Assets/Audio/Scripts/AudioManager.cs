using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] AudioClip[] musicClips;
    [SerializeField] AudioSource musicSource;
    [SerializeField] float[] musicVolumes;

    [Space(10f)]

    [SerializeField] AudioClip[] windClips;
    [SerializeField] AudioSource windSource;
    [SerializeField] float[] windVolumes;

    [Space(10f)]

    [SerializeField] AudioClip[] thunderClips;
    [SerializeField] AudioSource thunderSource;
    [SerializeField] float[] thunderVolumes;
    [SerializeField] float thunderIntervalMin, thunderIntervalMax;

    [Header("Others")]

    [SerializeField] AudioSource start;
    [SerializeField] AudioSource levelUp, newBestScore, gameOver, perfect;
#pragma warning restore 0649

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

    private AudioElement _music, _wind, _thunder;
    private Coroutine _thunderCoroutine;

    private void Awake()
    {
        _music = new AudioElement(musicSource, musicClips, musicVolumes);
        _wind = new AudioElement(windSource, windClips, windVolumes);
        _thunder = new AudioElement(thunderSource, thunderClips, thunderVolumes);

        _music.PlayRandom();
        _wind.PlayRandom();

        _thunderCoroutine = StartCoroutine(ThunderCycle());
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

    private IEnumerator ThunderCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(thunderIntervalMin, thunderIntervalMax));
            _thunder.PlayRandom();
        }
    }
}
