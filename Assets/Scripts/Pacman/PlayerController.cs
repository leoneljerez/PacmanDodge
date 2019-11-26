using System.Collections;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private AudioSource audioReset = null;
    [SerializeField] private AudioSource audioStart = null;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject ghost1 = null;
    [SerializeField] private GameObject ghost2 = null;
    [SerializeField] private GameObject ghost3 = null;
    [SerializeField] private GameObject ghost4 = null;

    public static bool isPowerUp;
    public static float powerUpTimer;
    public static int lives = 3;
    private float movementSpeed = 14f;

    private Animator animator;
    private Vector3 _initialPosition;
    private Vector3 _ghost1InitialPosition;
    private Vector3 _ghost2InitialPosition;
    private Vector3 _ghost3InitialPosition;
    private Vector3 _ghost4InitialPosition;
    public static bool performed;

    // Start is called before the first frame update
    private void Start()
    {
        _initialPosition = transform.position;
        _ghost1InitialPosition = ghost1.gameObject.GetComponent<Transform>().position;
        _ghost2InitialPosition = ghost2.gameObject.GetComponent<Transform>().position;
        _ghost3InitialPosition = ghost3.gameObject.GetComponent<Transform>().position;
        _ghost4InitialPosition = ghost4.gameObject.GetComponent<Transform>().position;
        animator = GetComponent<Animator>();
        performed = false;

        isPowerUp = false;
        powerUpTimer=0;

        Reset();
        if (performed == false)
        {
            StartCoroutine(StartGame());
        }
    }

    // Update is called once per frame
    private void Update()
    {
        MovePlayer();
        if(powerUpTimer>0) {
          powerUpTimer-= Time.deltaTime;
        }
        if(isPowerUp) {
          powerUpTimer += 5;
          isPowerUp=false;
        }
        PlayerWins();
    }

    private float getMovementSpeed()
    {
        return movementSpeed;
    }

    private void setMovementSpeeed(float x)
    {
        movementSpeed = x;
    }

    //Moves the player whenever they press a Keyboard (W,A,S,D or up,down,left,right) or Gamepad (dpad up,down,left,right or left stick)
    private void MovePlayer()
    {
        Gamepad gamepad = Gamepad.current;
        Keyboard keyboard = Keyboard.current;

        bool isMoving = true;
        bool isDead = animator.GetBool("isDead");



        if (performed == false)
        {
            return;
        }
        else if (isDead)
        {
            isMoving = false;
            animator.SetBool("isMoving", isMoving);
        }
        else if (keyboard != null && (keyboard.upArrowKey.isPressed || keyboard.wKey.isPressed) || gamepad != null && (gamepad.dpad.up.isPressed || gamepad.leftStick.y.ReadValue() == 1))
        {
            animator.SetBool("isMoving", isMoving);
            transform.position += Vector3.forward * getMovementSpeed() * Time.deltaTime;
           // print("up move speed" + getMovementSpeed());
            transform.rotation = Quaternion.LookRotation(Vector3.forward);
        }
        else if (keyboard != null && (keyboard.downArrowKey.isPressed || keyboard.sKey.isPressed) || gamepad != null && (gamepad.dpad.down.isPressed || gamepad.leftStick.y.ReadValue() == -1))
        {
            animator.SetBool("isMoving", isMoving);
            transform.position += Vector3.back * getMovementSpeed() * Time.deltaTime;
           // print("down move speed" + getMovementSpeed());
            transform.rotation = Quaternion.LookRotation(Vector3.back);
        }
        else if (keyboard != null && (keyboard.leftArrowKey.isPressed || keyboard.aKey.isPressed) || gamepad != null && (gamepad.dpad.left.isPressed || gamepad.leftStick.x.ReadValue() == -1))
        {
            animator.SetBool("isMoving", isMoving);
            transform.position += Vector3.left * getMovementSpeed() * Time.deltaTime;
            //print(" left move speed" + getMovementSpeed());
            transform.rotation = Quaternion.LookRotation(Vector3.left);
        }
        else if (keyboard != null && (keyboard.rightArrowKey.isPressed || keyboard.dKey.isPressed) || gamepad != null && (gamepad.dpad.right.isPressed || gamepad.leftStick.x.ReadValue() == 1))
        {
            animator.SetBool("isMoving", isMoving);
            transform.position += Vector3.right * getMovementSpeed() * Time.deltaTime;
            //print("right move speed" + getMovementSpeed());
            transform.rotation = Quaternion.LookRotation(Vector3.right);
        }
        else
        {
            isMoving = false;
            animator.SetBool("isMoving", isMoving);
        }

    }

    //Die whenever a ghost hits us
    private void OnTriggerEnter(Collider other)
    {
        //TODO You can sometimes keep moving while dead so the if statement will run again
        if (other.CompareTag("Enemy"))
        {
            if (powerUpTimer <= 0)
            {
                animator.SetBool("isDead", true);
                audioReset.Play();
                if (lives >= 1)
                {
                    GameObject.Find("Lives " + lives.ToString()).GetComponent<Image>().enabled = false;
                    lives--;
                }
                else
                {
                    SceneManager.LoadScene(2);
                }
            }
            else if(powerUpTimer>0) {

            }
        }
    }

    //Start game or reset position when Pac-Man dies
    public void Reset()
    {
        transform.position = _initialPosition;
        animator.SetBool("isDead", false);
        animator.SetBool("isMoving", false);
        transform.rotation = Quaternion.LookRotation(Vector3.back);
    }

    //Wait until the music plays to start the game
    private IEnumerator StartGame()
    {
        audioStart.Play();
        yield return new WaitForSeconds(audioStart.clip.length - 0.5f);
        performed = true;
    }

    private IEnumerator WaitFor3Seconds()
    {
        yield return new WaitForSeconds(3f);
        setMovementSpeeed(14f);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "RandomEvent")
        {
            float num = 0.00f;
            num = Random.Range(0.0f, 30.0f); //random number generator from 0-30 in float

            if (num > 20.0) // random event 1 (reset position)
            {
                print("Random Event: Player Reset Position");
                collision.transform.position = new Vector3(collision.transform.position.x, -5, collision.transform.position.z);
                transform.position = _initialPosition;
            }
            else if (num > 10.0) // random event 2 (speed up player for 3 seconds)
            {
                print("Random Event: Player MoveSpeed++ for 3 sec");
                collision.transform.position = new Vector3(collision.transform.position.x, -5, collision.transform.position.z);
                setMovementSpeeed(16f);
                StartCoroutine(WaitFor3Seconds());
            }
            else if (num > 0.0) // random event 3 (increase life by one if they need it)
            {
                print("Random Event: Lives++");
                collision.transform.position = new Vector3(collision.transform.position.x, -5, collision.transform.position.z);
                if (lives < 3)
                {
                    lives++;
                    GameObject.Find("Lives " + lives.ToString()).GetComponent<Image>().enabled = true;
                }
            }
            /**else if (num > 0.0) // random event 3 (reset ghosts position) NOT WORKING RIGHT
            {
                collision.transform.position = new Vector3(collision.transform.position.x, -5, collision.transform.position.z);
            
                ghost1.GetComponent<Rigidbody>().velocity = Vector3.zero;
                ghost1.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                ghost1.GetComponent<Rigidbody>().Sleep();
                ghost1.GetComponent<Transform>().position = _ghost1InitialPosition;

                
                ghost2.GetComponent<Rigidbody>().velocity = Vector3.zero;
                ghost2.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                ghost1.GetComponent<Rigidbody>().Sleep();
                ghost2.GetComponent<Transform>().position = _ghost2InitialPosition;

                
                ghost3.GetComponent<Rigidbody>().velocity = Vector3.zero;
                ghost3.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                ghost1.GetComponent<Rigidbody>().Sleep();
                ghost3.GetComponent<Transform>().position = _ghost3InitialPosition;

                
                ghost4.GetComponent<Rigidbody>().velocity = Vector3.zero;
                ghost4.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                ghost1.GetComponent<Rigidbody>().Sleep();
                ghost4.GetComponent<Transform>().position = _ghost4InitialPosition;
            }**/
        }
    }

    public void PlayerWins()
    {
        if (GameObject.Find("PacDot(Clone)") == null)
        {
            SceneManager.LoadScene(2);
        }
    }

}
