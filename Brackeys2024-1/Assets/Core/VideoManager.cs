using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Objects.Triggers;

public class VideoManager : MonoBehaviour
{
    public Transform greenScreen;
    public List<Transform> positions;
    public int currentVideoPosition;
    VideoComponent videoComponent;
    Material greenScreenMaterial;
    float lerpValue;
    float fadeInValue;
    float fadeOutValue;
    private Coroutine hintTimer;
    private int activeRoom;


    private void Awake()
    {
        videoComponent = GetComponent<VideoComponent>();
        greenScreenMaterial = greenScreen.GetComponent<Renderer>().material;
        currentVideoPosition = 0;
    }


    public void RoomIntro(int roomNumber)
    {
        activeRoom = roomNumber;

        switch (roomNumber)
        {
            case 1:
                PlayVideo(0);
                if (hintTimer == null) { hintTimer = StartCoroutine(HintTimer(19)); }
                break;
            case 2:
                PlayVideo(4);
                if (hintTimer == null) { hintTimer = StartCoroutine(HintTimer(20)); }
                break;
            case 3:
                PlayVideo(8);
                if (hintTimer == null) { hintTimer = StartCoroutine(HintTimer(15)); }
                break;
            default:
                break;
        }
    }


    IEnumerator HintTimer(float introLength)
    {
        yield return new WaitForSeconds(introLength);
        Debug.Log("Hint Timer Finished.");
        PuzzleHint(activeRoom);
    }


    public void PuzzleHint(int roomNumber)
    {
        activeRoom = roomNumber;
        hintTimer = null;

        switch (roomNumber)
        {
            case 1:
                PlayVideo(1);
                break;
            case 2:
                PlayVideo(5);
                break;
            case 3:
                PlayVideo(9);
                break;
            default:
                break;
        }
    }


    public void RoomComplete()
    {
        switch (activeRoom)
        {
            case 1:
                PlayVideo(2);
                break;
            case 2:
                PlayVideo(6);
                break;
            case 3:
                PlayVideo(10);
                break;
            default:
                break;
        }
    }


    public void SecretComplete()
    {
        switch (activeRoom)
        {
            case 1:
                PlayVideo(3);
                break;
            case 2:
                PlayVideo(7);
                break;
            case 3:
                PlayVideo(11);
                StartCoroutine(FinalVideo());
                break;
            default:
                break;
        }
    }


    IEnumerator FinalVideo()
    {
        yield return new WaitForSeconds(20);
        PlayVideo(12);
        /*yield return new WaitForSeconds(25);
        // Teleport to the ending screen? Return to the main menu?*/
    }



    public void FadeIn()
    {
        greenScreen.GetComponent<Collider>().enabled = true;
        StopCoroutine(Lerp(false)); 
        greenScreenMaterial.SetFloat("_Blur", 30.0f);
        StartCoroutine(Lerp(true));
    }

    public void FadeOut()
    {
        greenScreen.GetComponent<Collider>().enabled = false;
        StopCoroutine(Lerp(true));
        greenScreenMaterial.SetFloat("_Blur", 0.035f);
        StartCoroutine(Lerp(false));
    }


    IEnumerator Lerp(bool fadingIn)
    {
        lerpValue = 0;

        while (lerpValue < 1)
        {
            lerpValue += 0.4f * Time.deltaTime;
            lerpValue = Mathf.Clamp01(lerpValue);
            fadeInValue = Mathf.Lerp(30.0f, 0.035f, lerpValue);
            fadeOutValue = Mathf.Lerp(0.035f, 30.0f, lerpValue);
            if (fadingIn) { greenScreenMaterial.SetFloat("_Blur", fadeInValue); }
            else { greenScreenMaterial.SetFloat("_Blur", fadeOutValue); }
            yield return new WaitForEndOfFrame();
        }
    }



    public void PlayVideo(int index)
    {
        if (index < positions.Count)
        {
            InterruptVideo();
            greenScreen.position = positions[index].position;
            greenScreen.rotation = positions[index].rotation;
            videoComponent.PlayVideo(index);
            FadeIn();
        }
    }


    public void InterruptVideo()
    {
        videoComponent.StopVideo(true);
    }


    


    private void OnEnable()
    {
        RoomTrigger.OnFirstEnter += RoomIntro;
        RoomTrigger.OnEnter += PuzzleHint;
    }


    private void OnDisable()
    {
        RoomTrigger.OnFirstEnter -= RoomIntro;
        RoomTrigger.OnEnter -= PuzzleHint;
    }








    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayVideo(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayVideo(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayVideo(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlayVideo(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlayVideo(4);
        }
    }*/
}
