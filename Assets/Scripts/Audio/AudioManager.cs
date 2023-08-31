using UnityEngine;
using UnityEngine.Audio;
using Game.UI;

namespace Game.Sound
{
    [System.Serializable]
    sealed public class Sound
    {
        public string name;
        public AudioClip clip;
        public float volume;
        public bool mute;
        public bool playOnAwake;
        public bool loop;
        // public float fadeTime; // for ambient sound fading
        
        [HideInInspector]
        public AudioSource source;
    }

    sealed public class AudioManager : MonoBehaviour
    {

        public static AudioManager instance;

        [HideInInspector]
        public AudioSource ambientAudioSource; // must have an audio source already
        [SerializeField]
        Sound ambientSound;
        [SerializeField]
        float leastAmbientSoundVolume;

        public float currentAmbientVolume {get; private set;}

        [SerializeField]
        float ambientTransitionTime;

        public bool increasingAmbientVolume;

        [SerializeField]
        Sound[] gameSounds;

        void Awake()
        {

            instance = this;

            foreach(Sound s in gameSounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.volume = s.volume; 
                s.source.mute = s.mute;
                s.source.loop = s.loop;
                s.source.playOnAwake = s.playOnAwake;
                s.source.clip = s.clip;
            }
        }

        void Start()
        {
            PlayAmbient();
        }

        void Update()
        {
            if(increasingAmbientVolume)
                IncreaseAmbientVolume();
            else    
                DecreaseAmbientVolume();
        }

        void FixedUpdate()
        {
            AudioManager.instance.SetAmbientAudioVolume();
        }

        void PlayAmbient()
        {
            increasingAmbientVolume = false;

            ambientAudioSource = gameObject.AddComponent<AudioSource>();
            ambientAudioSource.volume = leastAmbientSoundVolume; // max volume is in the inspector
            ambientAudioSource.mute = ambientSound.mute;
            ambientAudioSource.loop = ambientSound.loop;
            ambientAudioSource.playOnAwake = ambientSound.playOnAwake;
            ambientAudioSource.clip = ambientSound.clip;

            currentAmbientVolume = ambientSound.volume;
            
            ambientAudioSource.Play();
        }

        public void DecreaseAmbientVolume()
        {

            if(ambientAudioSource.volume > 0)
            {
                float decreaseRate = ambientSound.volume/ambientTransitionTime;

                float decreaseAmount = decreaseRate*Time.deltaTime;

                if(currentAmbientVolume > leastAmbientSoundVolume)
                    currentAmbientVolume -= decreaseAmount;
                
                // ambientAudioSource.volume = Mathf.Lerp(currentAmbientVolume,leastAmbientSoundVolume,currentAmbientVolume/ambientSound.volume);
            }

        }

        public void IncreaseAmbientVolume()
        {
            if(ambientAudioSource.volume < ambientSound.volume)
            {
                float increaseRate = ambientSound.volume/ambientTransitionTime;

                float increaseAmount = increaseRate*Time.deltaTime;

                if(currentAmbientVolume < ambientSound.volume)
                    currentAmbientVolume += increaseAmount;

                // ambientAudioSource.volume = Mathf.Lerp(leastAmbientSoundVolume,currentAmbientVolume,currentAmbientVolume/ambientSound.volume);
            }

        }

        public void SetMusicAudioVolume()
        {
            if(gameSounds != null)
            {
                foreach (Sound sound in gameSounds)
                {
                    sound.source.volume = Mathf.Lerp(0,sound.volume,SettingsUIManager.instance.musicVol.value);
                }
            }
        }

        public void SetAmbientAudioVolume()
        {
            AudioManager.instance.ambientAudioSource.volume = Mathf.Lerp(0,AudioManager.instance.currentAmbientVolume,SettingsUIManager.instance.ambientVol.value);
        }

        public void StopAmbient()
        {
            ambientAudioSource.Stop();
        }

        public void PlayInGame(string sound)
        {

            bool hasSound = false;
            
            if(gameSounds == null)
            {
                Debug.LogError("No sounds found to play");
                return;
            }
            
            foreach(Sound s in gameSounds)
            {
                if(s.name == sound)
                {
                    hasSound = true;
                    s.source.Play();
                    return;
                }
            }

            if(!hasSound)
            {
                Debug.LogError("Requested audio not found");
            }
        }

        public AudioSource GetAudioSource(string sound)
        {
            foreach(Sound s in gameSounds)
            {
                if(s.name == sound)
                {
                    return s.source;
                }
            }
            Debug.LogError("Unable to get audio source of "+sound);
            return null;
        }

        public void StopInGameAudio()
        {
            if(gameSounds != null)
            {
                foreach(Sound s in gameSounds)
                {
                    if(s.source.isPlaying)
                        s.source.Stop();
                }
            }
            else
                Debug.LogWarning("No audio to stop");
        }
    }
}


