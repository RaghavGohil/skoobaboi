using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.UI;
using Game.Sound;
using Game.Helpers;

public class GameManagerOld : MonoBehaviour
{


    public static GameManagerOld instance;

    public bool startedDiving;
    public bool isDiving;

    bool canDive;
    
    [SerializeField]
    GameObject character,lobbyCamera,playerCamera,player;
    [SerializeField]
    GameObject bounds;

    public float diveUIStartTime{get;private set;}
    float diveTransitionTime;
    public Vector3 cameraRotation;

    void Start()
    {

        instance = this;
        InitializeDefaults();

        // helpers
        Helpers.canDrawDebugRays = false;
        Helpers.canDrawDebugSpheres = true;
    
    }

    public void InitializeDefaults()
    {
        diveUIStartTime = 4.0f;
        diveTransitionTime = 4.0f;
        canDive = true;
        startedDiving = false;
        isDiving = false;
        character.SetActive(true);
        bounds.SetActive(false);
        lobbyCamera.SetActive(true);
        playerCamera.SetActive(false);
        AudioManager.instance.PlayInGame("abovewater");
    }

    void FixedUpdate()
    {
        StartDiving();
        UpdateMouseState();
    }
    
    void StartDiving()
    {
        if(startedDiving && canDive)
        {
            canDive = false;
            AudioManager.instance.increasingAmbientVolume = true;
            FishGenerator.instance.SpawnFishes();
            character.transform.GetComponent<Animator>().Play("Dive");
            lobbyCamera.GetComponent<Animator>().Play("Dive");
            StartCoroutine(SetDiving());
            Invoke("ActivateBoundsAndFog",0.8f);
        }
    }

    public void UpdateMouseState()
    {
        if(isDiving)
        {
            if(!UIManager.instance.hasPausedGame)
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.None;
        }
    }

    void ActivateBoundsAndFog()
    {
        bounds.SetActive(true);
        RenderSettings.fog = true;
    }

    IEnumerator SetDiving()
    {

        yield return new WaitForSeconds(diveTransitionTime);

        startedDiving = false;
        isDiving = true;

        //char model
        character.SetActive(false);

        //camera desync
        Vector3 playerPos = lobbyCamera.transform.position;
        cameraRotation = lobbyCamera.transform.eulerAngles;
        lobbyCamera.SetActive(false);
        player.transform.position = playerPos;
        playerCamera.SetActive(true);
    }
}
