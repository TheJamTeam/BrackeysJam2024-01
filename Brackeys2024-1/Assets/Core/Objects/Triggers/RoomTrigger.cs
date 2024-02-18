using System;
using System.Collections;
using Core.Player;
using UnityEngine;

namespace Core.Objects.Triggers
{
    public class RoomTrigger : MonoBehaviour
    {
        public static event Action<int> OnFirstEnter;
        public static event Action<int> OnFirstExit;
        public static event Action<int> OnEnter;
        public static event Action<int> OnExit;

        private int _visitCount;
        [Range(0,4)]
        [SerializeField] private byte roomNumber;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (_visitCount == 0)
            {
                StartCoroutine(WaitToInvoke(OnFirstEnter));
                //OnFirstEnter?.Invoke(roomNumber);
                Debug.Log("Invoked OnFirstEnter");
            }
            else
            {
                OnEnter?.Invoke(roomNumber);
            }
            _visitCount++;
        }

        private void OnTriggerExit(Collider other)
        {
            // If anything that isnt the player left the trigger, return
            if (!other.CompareTag("Player")) return;
            if (_visitCount == 0)
            {
                OnFirstExit?.Invoke(roomNumber);
            }
            else
            {
                OnExit?.Invoke(roomNumber);
            }
        }

        IEnumerator WaitToInvoke(Action<int> actionToInvoke)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            actionToInvoke?.Invoke(roomNumber);
        }
    }
}
