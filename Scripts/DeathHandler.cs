using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using Cinemachine;


public class DeathHandler : MonoBehaviour
{
    PlayerHealth playerH;
    Animator animator;
    [SerializeField] Canvas gameOverCanvas;
    [SerializeField] Canvas displayCanvas;
    [SerializeField] GameObject cameraMoverMain;
    [SerializeField] GameObject cameraMoverDeath;


    public void Awake() 
 {
        playerH = GetComponent<PlayerHealth>();
        cameraMoverDeath.SetActive(false);
    }

    private void Start() 
 {
    
 }

 public void HandleDeath()
 { 
        cameraMoverMain.SetActive(false);
        cameraMoverDeath.SetActive(true);
        displayCanvas.enabled = false;
        Time.timeScale = 0.55f;
        Cursor.lockState = CursorLockMode.None;

        cameraMoverDeath.GetComponent<Animator>().SetBool("isPlayerDeath", true);
    
 }

 public void CameraReset()
 {
    cameraMoverMain.SetActive(true);
 }
    
}
