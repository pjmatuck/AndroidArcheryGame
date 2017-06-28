using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehavior : MonoBehaviour {

    //public float arrowSpeed;
    public GameObject arrowHead;

    private Rigidbody2D rgbd2D;
    private Vector3 mousePosition;
    private Vector3 arrowInitialPosition;
    private Quaternion arrowInitialRotation;
    private bool shooted = false;
    private bool hasHit = false;
    private Quaternion previousRotation;
    private GameObject parent;
    private Vector3 movementStartPoint, movementEndPoint;
    private float movementDistance;
    private BowController bowController;

    // Use this for initialization
    void Start () {
        rgbd2D = GetComponent<Rigidbody2D>();

        arrowInitialPosition = transform.position;
        //arrowInitialRotation = transform.rotation;

        parent = GameObject.Find("Bow");
        arrowHead = transform.GetChild(0).gameObject;
        bowController = this.transform.parent.gameObject.GetComponent<BowController>();
	}
	
	// Update is called once per frame
	void Update () {

        if (GameManager.gameState == GameState.GAME)
        {
            Time.timeScale = 1;

            //if (Input.GetMouseButtonUp(0) && shooted == false)
            //{
            //    Shoot(3f);
            //}
        }

        if (GameManager.gameState == GameState.PAUSE)
        {
            Time.timeScale = 0;
        }

        //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        //{
        //    // Get movement of the finger since last frame
        //    movementDistance += Input.GetTouch(0).deltaPosition.magnitude;
        //}

        //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        //{
        //    Debug.Log(movementDistance);
        //    Shoot(movementDistance/1000);
        //}

        //foreach (Touch touch in Input.touches)
        //{
        //    movementStartPoint = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0f));

        //    if (touch.phase == TouchPhase.Moved)
        //    {
        //        Debug.Log("Drag");
        //        movementEndPoint = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));
        //    }

        //    movementDistance = movementEndPoint.magnitude - movementStartPoint.magnitude;
        //    Debug.Log(movementDistance);
        //}

        if (hasHit)
        {
            transform.rotation = previousRotation;
            arrowHead.SetActive(false);
        }
        else
        {
            previousRotation = transform.rotation;
        }

        if (transform.position.x > 45)
        {
            //CameraController.resetCamera = true;
            Debug.Log("Reset Camera");
            FreezeArrow();
        }
    }

    //void FixedUpdate()
    //{
    //    if (shooted)
    //    {
    //        Vector2 v = rgbd2D.velocity;
    //        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    //        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    //    }
    //}

    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log(coll.gameObject.tag);
        string targetTag = coll.gameObject.tag;

        if (targetTag == "Ground")
        {
            FreezeArrow();
        }

        if (targetTag == "Target100")
        {
            Debug.Log("100");
            GUIController.Points += 100;
            FreezeArrow();
        }

        if (targetTag == "Target50")
        {
            Debug.Log("50");
            GUIController.Points += 50;
            FreezeArrow();
        }

        if (targetTag == "Target10")
        {
            Debug.Log("10");
            GUIController.Points += 10;
            FreezeArrow();
        }

        if (targetTag == "Target5")
        {
            Debug.Log("5");
            GUIController.Points += 5;
            FreezeArrow();
        }
    }

    //public void Shoot(float shootPower)
    //{
    //    GUIController.AvailableArrows--;
    //    CameraController.resetCamera = false;

    //    rgbd2D.isKinematic = false;

    //    mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    //Vector2 shootDirection = new Vector2(arrowHead.transform.localPosition.x - transform.position.x,
    //    //    arrowHead.transform.localPosition.x - transform.position.y);
    //    rgbd2D.AddForce(Vector2.right * shootPower, ForceMode2D.Impulse);
    //    shooted = true;
    //    transform.parent = null;
    //}

    //public void UpdateArrowPosOnBow(float pullDistance)
    //{
    //    if (!shooted)
    //    {
    //        transform.localPosition =
    //            new Vector3(transform.position.x - pullDistance, transform.position.y, transform.position.z);
    //    }
    //}

    void FreezeArrow()
    {
        hasHit = true;
        rgbd2D.velocity = Vector2.zero;
        rgbd2D.angularVelocity = 0f;
        rgbd2D.isKinematic = true;
        StartCoroutine(ResetArrow());
        //ResetArrow();
    }

    IEnumerator ResetArrow()
    {
        yield return new WaitForSeconds(1);
        bowController.ResetArrow();
        //CameraController.resetCamera = true;
        //shooted = false;
        hasHit = false;
        //transform.position = arrowInitialPosition;
        //transform.rotation = Quaternion.Euler(new Vector3(0, 0, 45));
        //transform.parent = parent.transform;
        arrowHead.SetActive(true);
        yield return null;
    }
}