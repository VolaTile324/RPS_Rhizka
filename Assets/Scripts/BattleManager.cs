using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    [SerializeField] State state;
    [SerializeField] GameObject gameResult;
    [SerializeField] TMP_Text gameResultText;
    [SerializeField] Player player1;
    [SerializeField] Player player2;

    enum State { 
        Prepare,
        Player1Select,
        Player2Select,
        Attacking,
        Damaging,
        Returning,
        GameOver
    }
    
    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case State.Prepare:
                // player prep
                player1.Prepare();
                player2.Prepare();

                player1.SetPlayable(true);
                player2.SetPlayable(false);
                state = State.Player1Select;
                break;

            case State.Player1Select:
                // select player1
                if(player1.SelectedCharacter != null)
                {
                    player1.SetPlayable(false);
                    player2.SetPlayable(true);
                    state = State.Player2Select;
                }
                break;

            case State.Player2Select:
                // select player2
                if(player2.SelectedCharacter != null)
                {
                    player2.SetPlayable(false);
                    player1.Attack();
                    player2.Attack();
                    state = State.Attacking;
                }
                break;

            case State.Attacking:
                // start attack, when attack done, calculate damage
                if(player1.isAttacking() == false && player2.isAttacking() == false)
                {
                    // who takes damage?
                    CalculateDMG(player1, player2, out Player winner, out Player loser);
                    if (loser == null)
                    {
                        // both take damage
                        player1.TakeDamage(player2.SelectedCharacter.chAttackPower / 2);
                        player2.TakeDamage(player1.SelectedCharacter.chAttackPower / 2);
                    }
                    else
                    {
                        // one takes damage
                        loser.TakeDamage(winner.SelectedCharacter.chAttackPower);
                    }
                    state = State.Damaging;
                }
                break;

            case State.Damaging:
                // check damage, see if char dead
                if(player1.isDamaging() == false && player2.isDamaging() == false)
                {
                    //start damage animation
                    if (player1.SelectedCharacter.chCurrentHealth == 0)
                    {
                        player1.Remove(player1.SelectedCharacter);
                    }

                    if (player2.SelectedCharacter.chCurrentHealth == 0)
                    {
                        player2.Remove(player2.SelectedCharacter);
                    }

                    // return anim
                    if (player1.SelectedCharacter != null)
                    {
                        player1.StartReturn();
                    }
                    
                    if (player2.SelectedCharacter != null)
                    {
                        player2.StartReturn();
                    }

                    state = State.Returning;
                }
                break;

            case State.Returning:
                // return to battle state, or call the GameOver if char dead
                if(player1.isReturning() == false && player2.isReturning() == false)
                {
                    if (player1.CharacterList.Count == 0 && player2.CharacterList.Count == 0)
                    {
                        /* Debug.Log(
                            "Game Over. player1: " + player1.CharacterList.Count 
                            + " player2: " + player2.CharacterList.Count
                            ); */
                        gameResult.SetActive(true);
                        gameResultText.text = "Game Over!\nDRAW";
                        state = State.GameOver;
                    }
                    else if (player1.CharacterList.Count == 0)
                    {
                        gameResult.SetActive(true);
                        gameResultText.text = "Game Over!\nPlayer 2 WINS";
                        state = State.GameOver;
                    }
                    else if (player2.CharacterList.Count == 0)
                    {
                        gameResult.SetActive(true);
                        gameResultText.text = "Game Over!\nPlayer 1 WINS";
                        state = State.GameOver;
                    }
                    else
                    {
                        state = State.Prepare;
                    }
                }
                break;

            case State.GameOver:
                // game will stop
                
                break;
        }
    }

    private void CalculateDMG(Player player1, Player player2, out Player winner, out Player loser)
    {
        var typePL1 = player1.SelectedCharacter.chType;
        var typePL2 = player2.SelectedCharacter.chType;

        if (typePL1 == CharacterType.Rock && typePL2 == CharacterType.Paper)
        {
            winner = player2;
            loser = player1;
        }
        else if (typePL1 == CharacterType.Rock && typePL2 == CharacterType.Scissors)
        {
            winner = player1;
            loser = player2;
        }
        else if (typePL1 == CharacterType.Paper && typePL2 == CharacterType.Rock)
        {
            winner = player1;
            loser = player2;
        }
        else if (typePL1 == CharacterType.Paper && typePL2 == CharacterType.Scissors)
        {
            winner = player2;
            loser = player1;
        }
        else if (typePL1 == CharacterType.Scissors && typePL2 == CharacterType.Rock)
        {
            winner = player2;
            loser = player1;
        }
        else if (typePL1 == CharacterType.Scissors && typePL2 == CharacterType.Paper)
        {
            winner = player1;
            loser = player2;
        }
        else
        {
            winner = null;
            loser = null;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
