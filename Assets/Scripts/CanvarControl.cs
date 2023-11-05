using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CanvarControl : MonoBehaviour
{
    public GameObject canvas;
    
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) { 
            canvas.SetActive(true);
        }
    }
}
