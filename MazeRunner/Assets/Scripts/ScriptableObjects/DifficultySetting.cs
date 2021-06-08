using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script to create different difficulty levels
//Enum to improve readability
public enum Difficulty { Easy, Normal, Hard }

//Scriptable object to allow for easy creatinon of different difficulties
[CreateAssetMenu(fileName = "Difficulty", menuName = "ScriptableObjects/Difficulty", order = 1)]
public class DifficultySetting : ScriptableObject
{
    public Difficulty difficulty; //The difficulty
    public float timerModifier; //how fast the score should be lowered
    public float penaltyModifier; //How fast the score should be lowered when the topdown cam is active
    public float coinSpawnRate; //How many coins should spawn relative to the surface of the maze
}
