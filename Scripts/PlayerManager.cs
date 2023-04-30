using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance; // static instance of the script
    public GameObject player; // reference to the player game object

    void Awake()
    {
        if (instance == null) // if instance doesn't exist
        {
            instance = this; // set the static instance to this script
        }
    }
}
