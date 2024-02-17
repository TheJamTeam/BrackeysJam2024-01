using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreativityProgression : MonoBehaviour
{
    public Transform greenScreen;
    public List<Transform> positions;
    public int currentPosition;
    VideoComponent videoComponent;
    bool roomEntered;
    Material greenScreenMaterial;
    float lerpValue;
    float fadeInValue;
    float fadeOutValue;


    private void Start()
    {
        videoComponent = greenScreen.GetComponent<VideoComponent>();
        greenScreenMaterial = greenScreen.GetComponent<Renderer>().material;
        currentPosition = 0;
    }


    public void RoomEntrance()
    {
        FadeIn();
        greenScreen.position = positions[0].position;
        greenScreen.rotation = positions[0].rotation;
        videoComponent.PlayVideo(0);
    }


    public void NextPosition()
    {
        if ((currentPosition + 1) < (positions.Count - 2))
        {
            FadeIn();
            currentPosition++;
            greenScreen.position = positions[currentPosition].position;
            greenScreen.rotation = positions[currentPosition].rotation;
            PlayNextVideo();
        }
    }

    
    public void RoomComplete()
    {
        FadeIn();
        greenScreen.position = positions[positions.Count - 2].position;
        greenScreen.rotation = positions[positions.Count - 2].rotation;
        currentPosition = positions.Count - 2;
        PlayNextVideo();
    }


    public void SecretComplete()
    {
        FadeIn();
        greenScreen.position = positions[positions.Count - 1].position;
        greenScreen.rotation = positions[positions.Count - 1].rotation;
        currentPosition = positions.Count - 1;
        PlayNextVideo();
    }



    public void FadeIn()
    {
        greenScreen.GetComponent<Collider>().enabled = true;
        greenScreenMaterial.SetFloat("_Blur", 30.0f);
        StopAllCoroutines();
        StartCoroutine(Lerp(true));
    }

    public void FadeOut()
    {
        greenScreen.GetComponent<Collider>().enabled = false;
        greenScreenMaterial.SetFloat("_Blur", 0.035f);
        StopAllCoroutines();
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

    public void PlayNextVideo()
    {
        InterruptVideo();
        videoComponent.PlayVideo(currentPosition);
    }

    public void RepeatVideo()
    {
        PlayNextVideo();
    }

    public void InterruptVideo()
    {
        videoComponent.StopVideo(true);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            RoomEntrance();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            NextPosition();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            RoomComplete();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SecretComplete();
        }
    }
}
