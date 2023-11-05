using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectChip : MonoBehaviour
{

    public int chipCount = 0;

    public GameObject chipText;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Chip") {
            other.gameObject.GetComponent<Animator>().SetTrigger("beingCollected");
            chipCount++;
            UpdateChips();
            FindObjectOfType<AudioManager>().Play("Chips");
        }
    }

    public void UpdateChips()
    {
        chipText.GetComponent<TextMeshProUGUI>().text = chipCount + "/10";
    }
}
