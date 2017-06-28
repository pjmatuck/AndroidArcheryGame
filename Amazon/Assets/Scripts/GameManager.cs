using UnityEngine;
using System.Collections;

// Game States
public enum GameState { MENU, PRESENTLEVEL, PRESENTOBJECTIVE, GAME, PAUSE, GAMEOVER }

public delegate void OnStateChangeHandler();

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    public event OnStateChangeHandler OnStateChange;
    public static GameState gameState { get; private set; }

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }

    }

	void Awake()
	{
		_instance = this;
	}

    public void SetGameState(GameState state)
    {
        gameState = state;
        OnStateChange();
    }
}
