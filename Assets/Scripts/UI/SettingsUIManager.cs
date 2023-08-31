using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using Game.Sound;

namespace Game.UI
{

    [System.Serializable]
    public class SaveData
    {
        public bool masterAudio,goodGraphics;
        public float ambientVol,musicVol;

        public SaveData(bool a,bool b,float c,float d)
        {
            masterAudio = a;
            goodGraphics = b;
            ambientVol = c;
            musicVol = d;
        }
    }

    public class SettingsUIManager : MonoBehaviour
    {

        [SerializeField]
        Toggle masterAudio,goodGraphics;
        public Slider ambientVol,musicVol;
        [SerializeField]
        Camera lobbyCamera,playerCamera;
        UniversalAdditionalCameraData playerUAC,lobbyUAC;

        public static SettingsUIManager instance;

        void Start()
        {
            instance = this;

            lobbyUAC = lobbyCamera.GetComponent<UniversalAdditionalCameraData>();
            playerUAC = playerCamera.GetComponent<UniversalAdditionalCameraData>();

            SaveData data = (SaveData)SerializationManager.Load("settings");
            
            if(data == null)
            {
                Init();
                Debug.Log("Unable to load saved data for settings.");
            }
            else
            {
                masterAudio.isOn = data.masterAudio;
                goodGraphics.isOn = data.goodGraphics;
                ambientVol.value = data.ambientVol;
                musicVol.value = data.musicVol;
            }

            SetMasterAudio();
            SetGraphics();
            

        }

        void Init()
        {
            masterAudio.isOn = true;
            goodGraphics.isOn = true;
            ambientVol.value = 1.0f;
            musicVol.value = 1.0f;
        }

        // Update is called once per frame
        public void SaveData()
        {
            SaveData data = new SaveData(masterAudio.isOn,goodGraphics.isOn,ambientVol.value,musicVol.value);
            SerializationManager.Save("settings",data);
        }

        public void PlayToggleAudio()
        {
            AudioManager.instance.PlayInGame("uitoggleclick");
        }

        //Behaviours

        public void SetMasterAudio()
        {
            AudioListener.volume = (masterAudio.isOn)? 1:0;
        }

        public void SetGraphics()
        {
            if(goodGraphics.isOn)
            {
                playerUAC.renderPostProcessing = true;
                lobbyUAC.renderPostProcessing = true;
            }
            else
            {
                playerUAC.renderPostProcessing = false;
                lobbyUAC.renderPostProcessing = false;
            }
        }

    }
}
