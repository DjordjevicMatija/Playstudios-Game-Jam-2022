using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startSpawn : MonoBehaviour
{
    public GameObject infoPanel;
    public GameObject blackoutPanel;
    public GameObject player;
    public GameObject spawn1;
    public GameObject spawn2;
    private bool updated;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {        
            StartCoroutine(waitBackground());
        }
    }

    private IEnumerator waitBackground() {
        blackoutPanel.SetActive(true);
        infoPanel.SetActive(false);
        spawn2.SetActive(true);
        FindObjectOfType<AudioManager>().Play("GlassBreak");
        yield return new WaitForSeconds(1);
        player.SetActive(true);
        blackoutPanel.SetActive(false);
        spawn1.SetActive(false);
    }
}
