using UnityEngine;

namespace SoundMgr {
    [CreateAssetMenu(menuName = "Sound")]
    public class SoundData : ScriptableObject {
        public string soundName;
        public AudioClip source;
        public float volume;
        public bool isLoop;

    }
}
