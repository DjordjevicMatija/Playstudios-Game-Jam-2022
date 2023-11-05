using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    public float speed = 5f;

    public GameObject infoPanel;
    public GameObject infoPanelTxt;

    public float jumpPower = 6.5f;
    public int jumpCnt = 2;

    private bool right = true;
    private bool isDashing = false;
    private bool canDash = true;
    private bool inInfoPanel = false;
    private bool canMove = true;
    private GameObject infoPanelObject;
    public float dashPower = 15f;
    public float dashCooldown = 1f;
    public float dashingTime = 0.2f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        dashPower /= transform.localScale.x;
    }


    // Update is called once per frame
    void Update()
    {
        if (isDashing)
            return;

        if (Input.GetKeyDown(KeyCode.E) && inInfoPanel) {
            if (canMove) {
                infoPanel.gameObject.SetActive(true);
                infoPanelTxt.GetComponent<TextMeshProUGUI>().text = infoPanelObject.gameObject.GetComponent<InfoPanelText>().infoPanelText;
                canMove = false;
            } else {
                infoPanel.gameObject.SetActive(false);
                canMove = true;
            }    
        }

        if (canMove) {
            //move right
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                rb.velocity = new Vector2(speed, rb.velocity.y);
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
                right = true;
                FindObjectOfType<AudioManager>().Play("Walk");

            }

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
                anim.SetBool("isRunning", true);
            }

            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow)) {
                rb.velocity = new Vector2(0, rb.velocity.y);
                if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.LeftArrow))
                    anim.SetBool("isRunning", false);
            }

            //move left
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
                right = false;
                FindObjectOfType<AudioManager>().Play("Walk");
            }

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                anim.SetBool("isRunning", true);
            }

            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow)) {
                rb.velocity = new Vector2(0, rb.velocity.y);
                if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.RightArrow))
                    anim.SetBool("isRunning", false);
            }

            //jump
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
                if (jumpCnt > 0) {
                    rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                    jumpCnt--;
                    if (jumpCnt == 1) {
                        anim.SetTrigger("startJump");
                    }
                }
            }

            //dash
            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash) {
                FindObjectOfType<AudioManager>().Play("Dash");
                anim.SetTrigger("startDash");
                StartCoroutine(Dash());
            }
        }

        if (rb.velocity.y < 0) {
            anim.SetBool("isFalling", true);
        }

        //game over
        if (transform.position.y < -18f)
        {
            FindObjectOfType<AudioManager>().Play("Death");
            FindObjectOfType<GameOver>().EndGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "InfoPanel") {
            //infoPanel.gameObject.SetActive(true);
            //infoPanelTxt.GetComponent<TextMeshProUGUI>().text = other.gameObject.GetComponent<InfoPanelText>().infoPanelText;
            other.transform.GetChild(0).gameObject.SetActive(true);
            inInfoPanel = true;
            infoPanelObject = other.gameObject;
        }

        if (other.name == "ExitDoor") {
            other.GetComponent<Animator>().SetTrigger("openDoor");
        }

        if (other.tag == "LevelFinish") {
            SceneManager.LoadScene("EndOfDemo");
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "InfoPanel") {
            //infoPanel.gameObject.SetActive(false);
            other.transform.GetChild(0).gameObject.SetActive(false);
            inInfoPanel = false;
            infoPanelObject = null;
        }

        if (other.name == "ExitDoor") {
            other.GetComponent<Animator>().SetTrigger("closeDoor");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Surface"))
        {
            jumpCnt = 2;
            anim.SetBool("isFalling", false);
        }

        
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.collider.CompareTag("Surface")) {
            anim.SetBool("isFalling", false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Surface"))
        {
            jumpCnt = 1;
            anim.SetBool("isFalling", false);
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        if (right)
        {
            rb.velocity = new Vector2(transform.localScale.x * dashPower, 0f);
        }
        if (!right)
        {
            rb.velocity = new Vector2(transform.localScale.x * -dashPower, 0f);
        }
        yield return new WaitForSeconds(dashingTime);
        rb.velocity = new Vector2(0, 0);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
