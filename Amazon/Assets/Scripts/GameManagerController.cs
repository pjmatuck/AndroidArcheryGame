using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerController : MonoBehaviour {

    private GameManager GM;

    void Awake()
    {
        Debug.Log(this.GetType().Name + ".cs GM: " + GM);
    }

    void Start()
    {
        GM = GameManager.Instance;
        //GM.SetGameState(GameState.GAME);
        GM.OnStateChange += HandleOnStateChange;
        Debug.Log(GameManager.gameState);
    }
	
    public void HandleOnStateChange()
    {
        Debug.Log(this.GetType().Name + ".cs State changed:" + GameManager.gameState);
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("FirstLevel");
        }

		if (GameManager.gameState == GameState.GAME && Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Pause");
            GM.SetGameState(GameState.PAUSE);
			return;
        }

		if (GameManager.gameState == GameState.PAUSE && Input.GetKeyDown(KeyCode.P))
		{
			Debug.Log("Game");
			GM.SetGameState(GameState.GAME);
			return;
		}
    }
}
