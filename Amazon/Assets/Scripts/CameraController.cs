using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public static Transform ObjectToFollow;
    public Transform endCameraPosition;
    public float rightBounderyLimit, leftBounderyLimit, upBounderyLimit, downBounderyLimit;
    public float rightToLeftWaitSec, leftToRightWaitSec;
    private float rightToLeftWaitSecSum, leftToRightWaitSecSum;
    public float rightToLeftMoveSpeed, leftToRightMoveSpeed;
    public static bool resetCamera = false;

    private Vector3 initialCameraPosition, objectStartPosition, cameraOffSet;
    private bool isCameraLeftToRight = true;
    private GameManager GM;

    // Use this for initialization
    void Start () {

        initialCameraPosition = transform.position;
        //initialCameraPosition.y = 0;

        cameraOffSet = transform.position - ObjectToFollow.transform.position;
        cameraOffSet.y = 0f;

        objectStartPosition = ObjectToFollow.transform.position;

        GM = GameManager.Instance;
        GM.SetGameState(GameState.PRESENTLEVEL);
	}

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameState == GameState.PRESENTLEVEL)
        {
            ShowLevel();
        }


        if (GameManager.gameState == GameState.GAME)
        {
            if (!resetCamera)
            {
                transform.position = new Vector3(
                    Mathf.Clamp(ObjectToFollow.position.x + cameraOffSet.x, leftBounderyLimit, rightBounderyLimit),
                    Mathf.Clamp(ObjectToFollow.position.y + cameraOffSet.y, downBounderyLimit, upBounderyLimit),
                    -10.0f);
            }

            if (resetCamera)
            {
                ResetCamera();

                if (transform.position == initialCameraPosition)
                {
                    resetCamera = false;
                    Debug.Log(resetCamera);
                }
            }

        }
    }

    void ResetCamera()
    {
        transform.position = Vector3.Lerp(transform.position, initialCameraPosition, .25f);
    }

    //Move camera towards the end of the level, and back.
    void ShowLevel()
    {
        if (transform.position.x <= endCameraPosition.position.x && isCameraLeftToRight)
        {
            leftToRightWaitSecSum += Time.deltaTime;
            if (leftToRightWaitSecSum >= rightToLeftWaitSec) {
                transform.Translate(Vector3.right * rightToLeftMoveSpeed);
            }
        }
        else
        {
            isCameraLeftToRight = false;
            rightToLeftWaitSecSum += Time.deltaTime;

            if (rightToLeftWaitSecSum >= rightToLeftWaitSec)
            {
                transform.Translate(Vector3.left * leftToRightMoveSpeed);
                if (transform.position.x <= initialCameraPosition.x)
                {
                    GM.SetGameState(GameState.PRESENTOBJECTIVE);
                }
            }
        }

    }
}
