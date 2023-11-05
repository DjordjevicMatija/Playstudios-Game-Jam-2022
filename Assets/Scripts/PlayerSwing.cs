using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerSwing : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject realObjects;
    public GameObject virtualObjects;

    public double swingLimit = 15;

    public GameObject firstBuildings;
    public GameObject secondBuildings;

    public float firstBuildingsScale;
    public float secondBuildingsScale;

    private DistanceJoint2D playerSwingJoint;
    private LineRenderer playerLineRenderer;
    private TrailRenderer playerTrailRender;
    private ParticleSystem playerParticleSystem;
    private Animator anim;
    private Rigidbody2D rb;
    private bool alreadyPlayed = false;

    private bool alreadyConnected = false;
    private GameObject connectedAnchor;

    private bool animated = false;

    private void setRope(GameObject closestAnchor) {
        playerSwingJoint.connectedAnchor = closestAnchor.transform.position;
        playerSwingJoint.enabled = true;
        playerLineRenderer.SetPosition(0, new Vector3(playerObject.transform.position.x, playerObject.transform.position.y, 3));
        playerLineRenderer.SetPosition(1, new Vector3(closestAnchor.transform.position.x, closestAnchor.transform.position.y, 3));
        playerLineRenderer.enabled = true;
    }

    private void Start() {
        playerSwingJoint = playerObject.GetComponent<DistanceJoint2D>();
        playerLineRenderer = playerObject.GetComponent<LineRenderer>();
        playerTrailRender = playerObject.GetComponent<TrailRenderer>();
        anim = playerObject.GetComponent<Animator>();
        rb = playerObject.GetComponent<Rigidbody2D>();
        playerParticleSystem = playerObject.GetComponent<ParticleSystem>();
    }

    void Update()
    {

        firstBuildings.transform.position = new Vector3(-gameObject.transform.position.x * firstBuildingsScale, firstBuildings.transform.position.y, firstBuildings.transform.position.z);
        secondBuildings.transform.position = new Vector3(-gameObject.transform.position.x * secondBuildingsScale, secondBuildings.transform.position.y, secondBuildings.transform.position.z);

        if (Input.GetKey(KeyCode.Space)) { 
            bool notConnected = true;

            if (!alreadyConnected) {
                
                double shortest = swingLimit + 1;
                GameObject shortestAnchor = GameObject.FindGameObjectWithTag("Anchor");
                foreach (GameObject anchor in GameObject.FindGameObjectsWithTag("Anchor")) {
                    double distance = Vector2.Distance(playerObject.transform.position, anchor.transform.position);

                    if (distance < shortest) {
                        shortest = distance;
                        shortestAnchor = anchor;
                    }
                }

                if (shortest < swingLimit) {
                    setRope(shortestAnchor);
                    connectedAnchor = shortestAnchor;
                    alreadyConnected = true;
                    notConnected = false;
                    if (!alreadyPlayed) {
                        FindObjectOfType<AudioManager>().Play("HookShot");
                        alreadyPlayed = true;
                    }
                    

                    if (!animated) {
                        anim.SetTrigger("startSwinging");
                        animated = true;
                    }

                } 
            } else {
                if (Vector2.Distance(playerObject.transform.position, connectedAnchor.transform.position) < swingLimit) {
                    if (!alreadyPlayed) {
                        FindObjectOfType<AudioManager>().Play("HookShot");
                        alreadyPlayed = true;
                    }
                    setRope(connectedAnchor);
                    notConnected = false;

                    if (!animated) {
                        anim.SetTrigger("startSwinging");
                        animated = true;
                    }
                }   
            }

            if (notConnected) {
                playerSwingJoint.enabled = false;
                playerLineRenderer.enabled = false;
                alreadyConnected = false;
                playerObject.GetComponent<PlayerMovement>().enabled = true;
                anim.ResetTrigger("startSwinging");
            } else {
                anim.SetBool("isSwinging", true);
                playerObject.GetComponent<PlayerMovement>().enabled = false;
                if (!playerParticleSystem.isPlaying)
                    playerParticleSystem.Play();
                //playerTrailRender.emitting = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            animated = false;
            anim.ResetTrigger("startSwinging");
            anim.SetTrigger("stopSwinging");
            anim.SetBool("isSwinging", false);
            playerObject.GetComponent<PlayerMovement>().enabled = true;
            playerSwingJoint.enabled = false;
            playerLineRenderer.enabled = false;
            alreadyConnected = false;
            playerParticleSystem.Stop();
            alreadyPlayed = false;
            //playerTrailRender.emitting = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Surface")) {
            playerObject.GetComponent<PlayerMovement>().enabled = true;
            playerSwingJoint.enabled = false;
            playerLineRenderer.enabled = false;
            alreadyConnected = false;
            playerTrailRender.emitting = false;
            rb.velocity = new Vector2(0, 0);
        }
    }
}
