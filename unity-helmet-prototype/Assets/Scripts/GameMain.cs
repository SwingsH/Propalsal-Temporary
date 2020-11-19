using System;
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

public enum AttackParts
{
    Head,
    Body,
    Legs
}

public enum RandomPrepareAnim
{
    Figher_Walk_Left = 1,
    Figher_Walk_Right=2,
    Dodge_Left=3,
    Dodge_Right = 4,
    Figher_Walk_Back = 5
}

public class GameMain : MonoBehaviour
{
    public Animator PlayerAnimator;
    public GameObject CommandListUI;
    public GameObject PlayerGameObject;
    public GameObject EnemyGameObject;
    public GameObject MiddleGameObject;
    public CameraMMO cameraMMO;

    public static GameMain _instance;
    protected GameState gameState;
    private bool stateInitialized ;
    private int randomPreAnimCount=0;
    private AttackParts currentAttackCommand;
    private string currentAnimName = string.Empty;
    private bool backwardPlayed = true;

    public static GameMain Instance
    {
        get { 
            return _instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.ChooseCommand;
        stateInitialized = false;
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        switch(gameState)
        {
            case GameState.ChooseCommand:
                if(stateInitialized==false)
                {
                    CommandListUI.SetActive(true);
                }
                break;
            case GameState.PlayCommand:

                if( (!AnimatorIsPlaying() ) && (randomPreAnimCount > 0))
                {
                    if(backwardPlayed) // to next anim
                    {
                        currentAnimName = ((RandomPrepareAnim)UnityEngine.Random.Range(1, 6)).ToString();
                        PlayerAnimator.Play(currentAnimName);
                        backwardPlayed = false;
                    }
                    else
                    {
                        PlayerAnimator.Play(currentAnimName + "_R");
                        backwardPlayed = true;
                        randomPreAnimCount--;
                    }
                    //PlayerAnimator.GetCurrentAnimatorStateInfo(0).
                }
                else if( !AnimatorIsPlaying() && randomPreAnimCount==0)
                {
                    // all random anim play finished
                    PlayerAnimator.Play("Arthur_Dodge_Front_Step_5m");
                    backwardPlayed = true;

                    gameState = GameState.ReadyToNextTurn;
                }

                break;
            case GameState.ReadyToNextTurn:
                PlayerAnimator.SetLayerWeight(1, 0);
                if (!AnimatorIsPlaying())
                {
                    CommandListUI.SetActive(true);
                }
                break;
            case GameState.ShowCommandResault:
                break;
        }

        Vector3 vector = Vector3.Lerp(PlayerGameObject.transform.position, EnemyGameObject.transform.position, 0.5f);
        vector.y = MiddleGameObject.transform.position.y;// never change y
        MiddleGameObject.transform.position = vector;
        cameraMMO.distance = 0.08f;
    }
    bool AnimatorIsPlaying()
    {
        return PlayerAnimator.GetCurrentAnimatorStateInfo(0).length >
               PlayerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    bool AnimatorIsPlayingIdle()
    {
        return PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Battle_Idle") ||
                PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
    }

    public void OnCommandPressed(AttackParts attackParts)
    {
        currentAttackCommand = attackParts;
        CommandListUI.SetActive(false);
        gameState = GameState.PlayCommand;
        randomPreAnimCount = UnityEngine.Random.Range(2,4);
        PlayerAnimator.Rebind();
        PlayerAnimator.Play("Turn_pose");
    }
}
