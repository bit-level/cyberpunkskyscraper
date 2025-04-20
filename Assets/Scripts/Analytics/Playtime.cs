using UnityEngine;

namespace BitLevel.Tools
{
    public class Playtime : MonoBehaviour
    {
        private float value;

        public static Playtime Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            float dt = Time.unscaledDeltaTime; 
            if (dt < 30f) value += dt;
        }

        public float Get() => value;
    }
}