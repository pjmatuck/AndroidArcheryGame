using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BowController : MonoBehaviour
{

    //Game Manager
    GameManager GM;

    //Bow Attributes
    public GameObject bowString;
    private LineRenderer bowStringRenderer;
    private Vector2 bowToMFVectorDistance; //MF stands for Mouse or Finger
    private float angleDeg;
    private float stringPullDistance;
    private Vector3 stringStartPosition;

    //Arrow Attributes
    ArrowBehavior arrowBehavior;
    public GameObject arrow;
    public float shootPower;
    private Rigidbody2D arrowRigidBody;
    private Vector3 arrowStartPosition;
    private Vector3 arrowStartLocalPosition, arrowPreviousLocalPosition;
    private bool arrowShooted = false;
    private bool arrowHit = false;

    //Input Attributes
    private Vector3 mouseOrFingerPosition;

    void Start()
    {
        GM = GameManager.Instance;
        //Bow Start Config
        bowStringRenderer = bowString.GetComponent<LineRenderer>();
        //bowStringRenderer.material.color = Color.gray;
        bowStringRenderer.sortingLayerName = "Weapons";
        bowStringRenderer.sortingOrder = 1;
        stringStartPosition = bowStringRenderer.GetPosition(1);

        //Arrow Start Config
        arrowBehavior = transform.GetChild(0).GetComponent<ArrowBehavior>();
        arrowStartPosition = new Vector3(
            this.transform.position.x - 0.5f,
            this.transform.position.y + 0.08f,
            this.transform.position.z);
        arrow = Instantiate(arrow, arrowStartPosition, Quaternion.identity);
        arrowRigidBody = arrow.GetComponent<Rigidbody2D>();
        arrow.transform.parent = transform;
        arrowStartLocalPosition = arrow.transform.localPosition;

        CameraController.ObjectToFollow = arrow.transform;
    }

    // Update is called once per frame
    void Update()
    {

        //ONLY FOR TESTS
        //if (GameManager.gameState == GameState.GAME) {
        //	mouseOrFingerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //}
        if (GameManager.gameState == GameState.GAME)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButton(0) && !arrowShooted)
                {
                    mouseOrFingerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    bowToMFVectorDistance = new Vector2(transform.position.x - mouseOrFingerPosition.x, transform.position.y - mouseOrFingerPosition.y);
                    Debug.Log(bowToMFVectorDistance.magnitude);
                    stringPullDistance = Mathf.Clamp(bowToMFVectorDistance.magnitude / 4f, 0, 1);
                    bowStringRenderer.SetPosition(1, new Vector3(-stringPullDistance, 0, 0));
                    //arrowBehavior.UpdateArrowPosOnBow(stringPullDistance);

                    arrow.transform.localPosition =
                        new Vector3(
                            arrowStartLocalPosition.x - stringPullDistance,
                            arrowStartLocalPosition.y,
                            arrowStartLocalPosition.z);
                }

                if (Input.GetMouseButtonUp(0) && !arrowShooted)
                {
                    bowStringRenderer.SetPosition(1, new Vector3(-stringStartPosition.x, 0, 0));
                    arrowPreviousLocalPosition = arrow.transform.localPosition;
                    ShootArrow(stringPullDistance * shootPower, transform.rotation * Vector2.right);
                    //arrowBehavior.Shoot(stringPullDistance * 3f);
                }
            }
        }

        //Define the distance between the click/tap and the bow's reaction
        if (bowToMFVectorDistance.magnitude > 1f)
        {
            angleDeg = Mathf.Atan2(bowToMFVectorDistance.y, bowToMFVectorDistance.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, Mathf.Clamp(angleDeg, -45f, 90f));
        }
    }

    void FixedUpdate()
    {
        if (arrowShooted)
        {
            UpdateArrowFlight();
        }
    }

    //Correct the arrow rotation to make an arc/parabolic movement
    private void UpdateArrowFlight()
    {
        Vector2 v = arrowRigidBody.velocity;
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void ShootArrow(float shootPower, Vector2 direction)
    {
        GUIController.AvailableArrows--;
        CameraController.resetCamera = false;

        arrowRigidBody.isKinematic = false;
        arrowRigidBody.AddForce(direction * shootPower, ForceMode2D.Impulse);
        arrowShooted = true;
        //transform.parent = null;
    }

    public void ResetArrow()
    {
        CameraController.resetCamera = true;
        arrowHit = false;
        arrowShooted = false;
        arrow.transform.position = arrowStartPosition;
        arrow.transform.localPosition = new Vector3(
            arrowPreviousLocalPosition.x + stringPullDistance,
            arrowPreviousLocalPosition.y,
            arrowPreviousLocalPosition.z);
        arrow.transform.rotation = this.transform.rotation;
        Debug.Log("Reset Arrow");
    }
}
