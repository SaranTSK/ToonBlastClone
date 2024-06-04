using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToonBlast
{
    public static class Effect
    {
        public static string ExplodeVFX = "ExplodeVFX";
    }

    public class EffectManager : MonoBehaviour
    {
        public static EffectManager Instance;

        [SerializeField] private List<ParticleSystem> effectPref;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
        }

        public void SpawnEffect(string name, Vector2 position, Quaternion rotation)
        {
            ParticleSystem effectPref = GetEffect(name);
            if (effectPref == null)
                return;

            ParticleSystem effect = Instantiate(effectPref, position, rotation);
            effect.Play();
        }

        public ParticleSystem GetEffect(string name)
        {
            return effectPref.Find(e => e.name == name);
        }
    }
}
