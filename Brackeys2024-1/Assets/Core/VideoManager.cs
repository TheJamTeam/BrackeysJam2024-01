using System;
using System.Collections;
using System.Collections.Generic;
using CustomScripts.Core.Objects;
using CustomScripts.Core.Objects.Triggers;
using CustomScripts.Helpers;
using UnityEngine;

namespace CustomScripts.Core
{
    public class VideoManager : MonoBehaviour
    {
        public Transform greenScreen;
        public List<Transform> positions;
        VideoComponent videoComponent;
    
        Material greenScreenMaterial;
        float lerpValue;
        float fadeInValue;
        float fadeOutValue;
    
        private int activeRoom;
        string neutralPuzzleTrigger;
        string secretPuzzleTrigger;

        private Coroutine hintTimer;
        float roomOneHintTimer = 19f;
        float roomTwoHintTimer = 20f;
        float roomThreeHintTimer = 17f;

        Billboard billboardComponent;
        bool billboardEnabled;

        Animator fadeToWhite;


        private void Awake()
        {
            greenScreen = transform;
            videoComponent = GetComponent<VideoComponent>();
            billboardComponent = GetComponent<Billboard>();
            greenScreenMaterial = greenScreen.GetComponent<Renderer>().material;
            fadeToWhite = GameObject.FindWithTag("Ending").GetComponent<Animator>();
        }


        public void RoomIntro(int roomNumber)
        {
            activeRoom = roomNumber;

            switch (roomNumber)
            {
                case 1:
                    billboardEnabled = true;
                    PlayVideo(0);
                    if (hintTimer == null) { hintTimer = StartCoroutine(HintTimer(roomOneHintTimer)); }
                    break;
                case 2:
                    billboardEnabled = true;
                    PlayVideo(4);
                    if (hintTimer == null) { hintTimer = StartCoroutine(HintTimer(roomTwoHintTimer)); }
                    break;
                case 3:
                    billboardEnabled = true;
                    PlayVideo(8);
                    if (hintTimer == null) { hintTimer = StartCoroutine(HintTimer(roomThreeHintTimer)); }
                    break;
                default:
                    break;
            }
        }


        IEnumerator HintTimer(float introLength)
        {
            yield return new WaitForSeconds(introLength);
            //Debug.Log("Hint Timer Finished.");
            if (hintTimer != null) { PuzzleHint(activeRoom); }
            else { Debug.Log("Hint Cancelled. Room or Secret were completed."); }
        }


        public void PuzzleHint(int roomNumber)
        {
            activeRoom = roomNumber;
            hintTimer = null;

            switch (roomNumber)
            {
                case 1:
                    billboardEnabled = false;
                    PlayVideo(1);
                
                    break;
                case 2:
                    billboardEnabled = false;
                    PlayVideo(5);
                    break;
                case 3:
                    billboardEnabled = false;
                    PlayVideo(9);
                    break;
                default:
                    break;
            }
        }


        public void RoomComplete()
        {
            // Need to stop the hint timers if they're currently ticking down, since hint's no longer needed, and could otherwise iterrupt the room completion.

            switch (activeRoom)
            {
                case 1:
                    if (hintTimer != null) { hintTimer = null; StopCoroutine(HintTimer(roomOneHintTimer)); }
                    billboardEnabled = true;
                    PlayVideo(2);
                    break;
                case 2:
                    if (hintTimer != null) { hintTimer = null; StopCoroutine(HintTimer(roomTwoHintTimer)); }
                    billboardEnabled = false;
                    PlayVideo(6);
                    break;
                case 3:
                    if (hintTimer != null) { hintTimer = null; StopCoroutine(HintTimer(roomThreeHintTimer)); }
                    billboardEnabled = true;
                    PlayVideo(10);
                    break;
                default:
                    break;
            }
        }


        public void SecretComplete()
        {
            // Need to stop the hint timers if they're currently ticking down, since hint's no longer needed, and could otherwise iterrupt the secret.

            switch (activeRoom)
            {
                case 1:
                    if (hintTimer != null) { hintTimer = null; StopCoroutine(HintTimer(roomOneHintTimer)); }
                    billboardEnabled = true;
                    PlayVideo(3);
                    break;
                case 2:
                    if (hintTimer != null) { hintTimer = null; StopCoroutine(HintTimer(roomTwoHintTimer)); }
                    billboardEnabled = true;
                    PlayVideo(7);
                    break;
                case 3:
                    if (hintTimer != null) { hintTimer = null; StopCoroutine(HintTimer(roomThreeHintTimer)); }
                    billboardEnabled = true;
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
            billboardEnabled = true;
            PlayVideo(12);
            yield return new WaitForSeconds(25);
            EndGame();
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
                billboardComponent.enabled = billboardEnabled;
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



        public void CompletionCheck(string interactID)
        {
            switch (activeRoom)
            {
                case 1:
                    neutralPuzzleTrigger = "Blank Canvas";
                    secretPuzzleTrigger = "Proudfoot's Painting";
                    break;
                case 2:
                    neutralPuzzleTrigger = "Weight";
                    secretPuzzleTrigger = "Ornate Book";
                    break;
                case 3:
                    neutralPuzzleTrigger = "Chocolates";
                    secretPuzzleTrigger = "Thoughtful Gift";
                    break;
                default:
                    break;
            }

            // Are the two IDs equal, ignoring case (incase of typo)
            if (String.Equals(interactID, neutralPuzzleTrigger, StringComparison.OrdinalIgnoreCase))
            {
                RoomComplete();
            }
            else if (String.Equals(interactID, secretPuzzleTrigger, StringComparison.OrdinalIgnoreCase))
            {
                SecretComplete();
            }
        }



        void EndGame()
        {
            fadeToWhite.Play("FadeToMenu");
        }





        private void OnEnable()
        {
            RoomTrigger.OnFirstEnter += RoomIntro;
            RoomTrigger.OnEnter += PuzzleHint;
            InteractComponent.OnInteractKeysComplete += CompletionCheck;
            InteractComponent.OnInteractUsed += CompletionCheck;
        }


        private void OnDisable()
        {
            RoomTrigger.OnFirstEnter -= RoomIntro;
            RoomTrigger.OnEnter -= PuzzleHint;
            InteractComponent.OnInteractKeysComplete -= CompletionCheck;
            InteractComponent.OnInteractUsed -= CompletionCheck;
        }
    }
}
