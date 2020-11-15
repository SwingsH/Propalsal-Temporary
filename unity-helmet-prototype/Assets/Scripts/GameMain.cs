using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    ChooseCommand,
    PlayCommand,
    ShowCommandResault,
    ReadyToNextTurn
}

public class GameMain : MonoBehaviour
{
    public Animator playerAnimator;
    protected GameState gameState = GameState.ChooseCommand;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator.Play("Battle_Move");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
