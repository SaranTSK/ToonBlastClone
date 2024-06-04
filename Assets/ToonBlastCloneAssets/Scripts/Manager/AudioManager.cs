using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToonBlast
{
    [System.Serializable]
    public class AudioClass
    {
        public string Name;
        public AudioClip clip;
        public bool IsPlayOnAwake;
        public bool IsLoop;
        [Range(0f, 1f)] public float Vol;
        [Range(.1f, 3f)] public float Pitch;
        [Range(0f, 1f)] public float SpatialBlend;

        [HideInInspector] public AudioSource Source;
    }

    public static class Audio
    {
        public static string BGM_Gameplay = "BGM_Gameplay";
        public static string SFX_Pop = "SFX_Pop";
        public static string SFX_Drop = "SFX_Drop";
        public static string SFX_Bomb = "SFX_Bomb";
    }

    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        [SerializeField] private List<AudioClass> sounds;
        [SerializeField] private float masterVolume = 1.0f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            foreach (AudioClass s in sounds)
            {
                if (s.clip != null)
                {
                    s.Source = gameObject.AddComponent<AudioSource>();
                    s.Source.clip = s.clip;
                    s.Source.volume = s.Vol;
                    s.Source.pitch = s.Pitch;
                    s.Source.loop = s.IsLoop;
                    s.Source.playOnAwake = s.IsPlayOnAwake;
                    s.Source.spatialBlend = s.SpatialBlend;
                }
            }
        }

        private void Start()
        {
            AudioListener.volume = masterVolume;
        }

        public void Play(string name)
        {
            AudioClass s = sounds.Find(s => s.Name == name);

            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found...");
                return;
            }

            s.Source.Play();
        }

        public void Stop(string name)
        {
            AudioClass s = sounds.Find(s => s.Name == name);

            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found...");
                return;
            }

            s.Source.Stop();
            s.Source.loop = false;
        }

        public void PlayOneShot(string name)
        {
            AudioClass s = sounds.Find(s => s.Name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found...");
                return;
            }

            s.Source.PlayOneShot(s.Source.clip);
        }
    }
}
