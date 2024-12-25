using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace SoundMgr {
    public class SoundPlayer : MonoBehaviour {
        [SerializeField] private int audioSouceNum;
        [SerializeField] private List<SoundData> sounds;

        private Dictionary<string, SoundData> _soundDict;

        public static SoundPlayer I;

        private AudioSource[] _audioSources;
        [SerializeField] private float fadeDuration;

        void Awake() {
            DontDestroyOnLoad(gameObject);
            if (I == null) I = this;
            else Destroy(gameObject);
            
            _soundDict = new();
            foreach (var sound in sounds) {
                _soundDict[sound.soundName] = sound;
            }

            for (int i = 0; i < audioSouceNum; i++) {
                gameObject.AddComponent<AudioSource>();
            }
            _audioSources = GetComponents<AudioSource>();
        }

        public void Play(string soundName) {
            SoundData data = _soundDict[soundName];

            if (data.isLoop) {
                bool isPlaying = _audioSources
                    .Where(x => x.isPlaying)
                    .Any(x => x.clip == data.source);
                if (isPlaying) return;
            }

            AudioSource targetSource = null;
            foreach (var source in _audioSources) {
                if (!source.isPlaying) {
                    targetSource = source;
                    break;
                }
            }

            if (targetSource == null) return;
            targetSource.clip = data.source;
            targetSource.volume = data.volume;
            targetSource.loop = data.isLoop;
            targetSource.Play();
        }
        
        public async UniTask FadeOutSound(string soundName) {
            AudioSource targetSource =
                _audioSources.FirstOrDefault(x => x.isPlaying && x.clip == _soundDict[soundName].source);
            if(targetSource == null) return;
            targetSource.DOFade(0f, fadeDuration);
            await UniTask.Delay(TimeSpan.FromSeconds(fadeDuration));
            targetSource.Stop();
        }
    }
    

}
