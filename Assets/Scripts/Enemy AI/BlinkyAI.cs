using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkyAI : MonoBehaviour
{
    [SerializeField] private GameObject scoreText = null;
    [SerializeField] private int point= 150;
    [SerializeField] private float movementSpeed = 0f;



    private Vector3 _initialPosition;
    private Vector3 _startMovePosition;
    private bool moveFromStart;
    private float reStartTimer;
    private int moveOption;
    private Vector3 moveCheck;
    // Start is called before the first frame update
    void Start()
    {
        //_initialPosition = transform.position;
        _initialPosition = transform.position;
        _startMovePosition = new Vector3(0.03f,1f,5f);
        moveFromStart = true;
        reStartTimer = 0f;
        moveOption=0;
        moveCheck = transform.position;
        movementSpeed=2f;
    }

    // Update is called once per frame
    void Update()
    {
      if(reStartTimer>0) {
        reStartTimer-=Time.deltaTime;
      } else {
        if(reStartTimer<0) reStartTimer=0;
        if(Vector3.Distance(transform.position,moveCheck)<0.001f) moveOption+=1;
        if(moveOption>3) moveOption=0;
        moveCheck=transform.position;
        testMoveGhost();
      }
    }
    public void Reset()
    {
        transform.position = _initialPosition;
        transform.rotation = Quaternion.LookRotation(Vector3.back);
        moveFromStart=true;
        movementSpeed=2f;
    }
    private void OnTriggerEnter(Collider other)
    {
        //TODO You can sometimes keep moving while dead so the if statement will run again
        if (other.CompareTag("Pacman"))
        {
          if(PlayerController.powerUpTimer<=0) {

          } else if(PlayerController.powerUpTimer>0) {
            ScoreManager.scores += point;
            ShowFloatingText();
            reStartTimer=3;
            Reset();

          }

        }
    }
    private void testMoveGhost() {
      if(moveFromStart) {
        if(Vector3.Distance(transform.position,_startMovePosition)>=0.08f) {
          transform.position += Vector3.left * movementSpeed * Time.deltaTime;
          transform.rotation = Quaternion.LookRotation(Vector3.left);
        } else {
          moveFromStart=false;
          movementSpeed=7f;
        }
      } else {
        if(moveOption == 0) {
          transform.position += Vector3.back * movementSpeed * Time.deltaTime;
          transform.rotation = Quaternion.LookRotation(Vector3.back);
        } else if(moveOption==1) {
          transform.position += Vector3.left * movementSpeed * Time.deltaTime;
          transform.rotation = Quaternion.LookRotation(Vector3.left);
        } else if(moveOption==2) {
          transform.position += Vector3.forward * movementSpeed * Time.deltaTime;
          transform.rotation = Quaternion.LookRotation(Vector3.forward);
        } else if(moveOption==3) {
          transform.position += Vector3.right * movementSpeed * Time.deltaTime;
          transform.rotation = Quaternion.LookRotation(Vector3.right);
        }
      }
    }

    void ShowFloatingText()
    {
        var go = Instantiate(scoreText, transform.position, Quaternion.identity, transform);
        go.GetComponent<TextMesh>().text = point.ToString();
    }

}
