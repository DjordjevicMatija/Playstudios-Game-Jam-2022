using Kino;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TerminalSwitch : MonoBehaviour
{

    public GameObject realObjects;
    public GameObject virtualObjects;
    public GameObject playerCamera;
    public GameObject transVerseObject;

    private bool inTerminal;
    private bool inVirtual;
    private bool switched;
    private float currentFadeTime;
    public float fadeTime;
    public float fadeIntensity;

    private void Start() {
        inTerminal = false;
        inVirtual = false;
        switched = false;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (inTerminal) {
                FindObjectOfType<AudioManager>().Play("Terminal");
                currentFadeTime = fadeTime;
            }
        }

        if (currentFadeTime > 0) {

            gameObject.GetComponent<PlayerMovement>().enabled = false;
            gameObject.GetComponent<PlayerSwing>().enabled = false;

            if (currentFadeTime > ((fadeTime / 2.0f) - 0.1) && currentFadeTime < ((fadeTime / 2.0f) + 0.1) && !switched) {

                if (!inVirtual) {
                    realObjects.SetActive(false);
                    virtualObjects.SetActive(true);
                    inVirtual = true;
                    playerCamera.GetComponent<AnalogGlitch>().scanLineJitter = 0.25f;
                } else {
                    if (GetComponent<CollectChip>().chipCount >= 5)
                    {
                        transVerseObject.SetActive(true);
                        GetComponent<CollectChip>().chipCount -= 5; ;
                        GetComponent<CollectChip>().UpdateChips();
                    }
                    realObjects.SetActive(true);
                    virtualObjects.SetActive(false);
                    inVirtual = false;
                    playerCamera.GetComponent<AnalogGlitch>().scanLineJitter = 0;
                }
                switched = true;
            }
            if (currentFadeTime > 1) {
                float intensity = Mathf.Lerp(fadeIntensity, 0, (currentFadeTime - (fadeTime/2.0f) / fadeTime));
                playerCamera.GetComponent<DigitalGlitch>().intensity = intensity;
            } else {
                float intensity = Mathf.Lerp(0, fadeIntensity, currentFadeTime / fadeTime);
                playerCamera.GetComponent<DigitalGlitch>().intensity = intensity;
            }
            currentFadeTime -= Time.deltaTime;
            if (currentFadeTime < 0.05) {
                currentFadeTime = 0;
                playerCamera.GetComponent<DigitalGlitch>().intensity = 0;
                switched = false;
                gameObject.GetComponent<PlayerMovement>().enabled = true;
                gameObject.GetComponent<PlayerSwing>().enabled = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Terminal") {
            other.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            inTerminal = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Terminal") {
            other.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            inTerminal = false;
        }
    }
}
