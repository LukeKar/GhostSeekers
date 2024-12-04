using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrunScreenON : MonoBehaviour
{
    public GameObject light;
    public Material screen_color;
    private bool ScreenIsOFF = true;
    private bool Interactable = false;


    private void Update() {
        if (Interactable) {
            if (Input.GetKeyDown(KeyCode.E) && ScreenIsOFF) {
                screen_color.color = Color.white;
                light.SetActive(true);
                ScreenIsOFF = false;

            } else if (Input.GetKeyDown(KeyCode.E) && !ScreenIsOFF) {
                screen_color.color = Color.black;
                light.SetActive(false);
                ScreenIsOFF = true;
            }
        }
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Interactable = true;
        }

    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            Interactable = false;
        }
    }

}
