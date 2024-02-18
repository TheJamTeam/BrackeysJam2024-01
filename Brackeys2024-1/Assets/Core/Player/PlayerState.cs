using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Core.Objects.Triggers;

namespace Core.Player
{
    /// <summary>
    /// Enumeration of the Puzzles in the Game
    /// Naming Convention:
    /// <list type="bullet">
    /// <item> RoomNumberPuzzleNumber </item>
    /// <item>For Example: Room1Simple=Room1Puzzle0 (10)</item>
    /// </list>
    /// </summary>
    [Serializable]
    public enum Puzzle {
        Room1Simple=10, 
        Room1Complex=11, 
        Room2Simple=20, 
        Room2Complex=21, 
        Room3Simple=30, 
        Room3Complex=31
    }
    
    public class PlayerState : MonoBehaviour
    {
        [NotNull] private List<Puzzle> _completedPuzzles;
        
        [Range(0,4)]
        private byte _currentRoom;

        private Dictionary<byte, int> _roomVisitCounts;
        
        // This is the serialized dictionary
        [Tooltip("Key value pairs of the Name of the Object which ends the Puzzle, and the Puzzle it ends")]
        [SerializeField] private PuzzleDictionary puzzleDictionary;
        
        // This is the true dictionary, made up of the Serialized Dicts Values
        private Dictionary<string, Puzzle> _puzzles;

        /// <summary>
        /// OnEnable is called each time an Object is Enabled
        /// </summary>
        private void OnEnable()
        {
            InteractComponent.OnInteractKeysComplete += OnPuzzleComplete;
            RoomTrigger.OnEnter += OnRoomEnter;
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        private void Start()
        {
            _completedPuzzles = new List<Puzzle>(6);
            _puzzles = puzzleDictionary.ToDictionary();
            
            _roomVisitCounts = new Dictionary<byte, int>();
            for (byte i = 0; i <= 4; i++)
            {
                _roomVisitCounts.Add(i, 0);
            }
        }

        /// <summary>
        /// Adds the Completed puzzle to the List of Completed Puzzles
        /// Triggered by OnInteractKeysComplete
        /// </summary>
        /// <param name="lastPuzzleObject">The name of the last object of the Puzzle</param>
        private void OnPuzzleComplete(string lastPuzzleObject)
        {
            if (_puzzles.TryGetValue(lastPuzzleObject, out Puzzle completedPuzzle))
            {
                _completedPuzzles.Add(completedPuzzle);
            }
        }
        /// <summary>
        /// Sets the current room number and increments the number of times the room has been visited
        /// </summary>
        /// <param name="roomNumber">The RoomNumber (Range 1-3)</param>
        void OnRoomEnter(int roomNumber)
        {
            _currentRoom = (byte)roomNumber;
            _roomVisitCounts[(byte)roomNumber]++;
        }
        /// <summary>
        /// This function is called when the behaviour becomes disabled.
        /// </summary>
        private void OnDisable()
        {
            InteractComponent.OnInteractKeysComplete -= OnPuzzleComplete;
            RoomTrigger.OnEnter -= OnRoomEnter;
        }
    }

    /// <summary>
    /// Class containing the Array of PuzzleDictItems, and a ToDictionary Method
    /// </summary>
    [Serializable]
    public class PuzzleDictionary
    {
        [Tooltip("The items (Key, Value) pairs in the Dictionary")]
        [SerializeField] private PuzzleDictItem[] items;

        /// <summary>
        /// Convert the Array of Dictionary Items stored in this class into a C# Dictionary
        /// </summary>
        /// <returns>A System.Collections.Generic Dictionary containing (string, Puzzle) pairs</returns>
        public Dictionary<string, Puzzle> ToDictionary()
        {
            Dictionary<string, Puzzle> newDictionary = new Dictionary<string, Puzzle>();

            foreach (PuzzleDictItem item in items)
            {
                newDictionary.Add(item.TriggeringObject, item.CompletedPuzzle);
            }

            return newDictionary;
        }
    }
    
    /// <summary>
    /// The Item Data Structure that contains the Key, Value pair for the Dictionary
    /// </summary>
    [Serializable]
    public struct PuzzleDictItem
    {
        [Tooltip("The Name of the GameObject that Ends the Puzzle")]
        public string TriggeringObject;
        [Tooltip("The Puzzle which is completed with this GameObject")]
        public Puzzle CompletedPuzzle;
    }
    
    
}


