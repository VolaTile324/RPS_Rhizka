using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] Character selectedCharacter;
    [SerializeField] Transform attackRef;
    [SerializeField] bool isCPU;
    [SerializeField] List<Character> characterList;
    [SerializeField] UnityEvent onTakeDamageRock;
    [SerializeField] UnityEvent onTakeDamagePaper;
    [SerializeField] UnityEvent onTakeDamageScissors;
    [SerializeField] UnityEvent onEliminateRock;
    [SerializeField] UnityEvent onEliminatePaper;
    [SerializeField] UnityEvent onEliminateSci;

    public Character SelectedCharacter { get => selectedCharacter; }
    public List<Character> CharacterList { get => characterList; }

    private void Start()
    {
        if (isCPU)
        {
            foreach (var character in characterList)
            {
                character.plButton.interactable = false;
            }
        }
    }

    public void Prepare()
    {
        selectedCharacter = null;
    }

    public void SelectCharacter(Character character)
    {
        selectedCharacter = character;
    }

    public void SetPlayable(bool value)
    {
        if (isCPU)
        {
            // as computer, i want to select the char randomly
            // but tries strategize by prioritizing the character with more health
            // to-do: and looks at player's health too
            List<Character> lotteryList = new List<Character>();
            foreach (var character in characterList)
            {
                int ticket = Mathf.CeilToInt(((float)character.chCurrentHealth / (float)character.chMaxHealth) * 10);
                for (int i = 0; i < ticket; i++)
                {
                    lotteryList.Add(character);
                }
            }

            int index = UnityEngine.Random.Range(0, lotteryList.Count);
            selectedCharacter = lotteryList[index];
        }

        
        foreach (var character in characterList)
        {
            character.plButton.interactable = value;
        }
    }

    public void Attack()
    {
        selectedCharacter.transform
            .DOMove(attackRef.position, 0.3f);
    }

    public bool isAttacking()
    {
        return DOTween.IsTweening(selectedCharacter.transform);
    }

    public void TakeDamage(int dmgValue)
    {
        selectedCharacter.ChangeHP(-dmgValue);
        var spriteRend = selectedCharacter.GetComponent<SpriteRenderer>();
        // spriteRend.DOColor(Color.red, 0.5f)
        //    .OnComplete(() => spriteRend.DOColor(Color.white, 0.5f));
        spriteRend.DOColor(Color.red, 0.2f).SetLoops(6, LoopType.Yoyo);
        // play hurt sound depending on character type
        TakeDamageCharType();
    }

    public void TakeDamageCharType()
    {
        switch (selectedCharacter.chType)
        {
            case CharacterType.Rock:
                onTakeDamageRock.Invoke();
                break;
            case CharacterType.Paper:
                onTakeDamagePaper.Invoke();
                break;
            case CharacterType.Scissors:
                onTakeDamageScissors.Invoke();
                break;
            default:
                break;
        }
    }

    public bool isDamaging()
    {
        var spriteRend = selectedCharacter.GetComponent<SpriteRenderer>();
        return DOTween.IsTweening(spriteRend);
    }

    public void Remove(Character targetCharacter)
    {
        if (characterList.Contains(targetCharacter) == false)
        {
            Debug.LogError("Character not found");
            return;
        }

        if (selectedCharacter == targetCharacter)
        {
            // find character type and play it's death sound
            EliminateSoundCharType();
            selectedCharacter = null;
        }
        
        targetCharacter.plButton.interactable = false;
        targetCharacter.gameObject.SetActive(false);
        characterList.Remove(targetCharacter);
    }

    public void EliminateSoundCharType()
    {
        switch (selectedCharacter.chType)
        {
            case CharacterType.Rock:
                onEliminateRock.Invoke();
                break;
            case CharacterType.Paper:
                onEliminatePaper.Invoke();
                break;
            case CharacterType.Scissors:
                onEliminateSci.Invoke();
                break;
            default:
                break;
        }
    }

    public void StartReturn()
    {
        selectedCharacter.transform.DOMove(selectedCharacter.chInitialPos, 0.6f);
    }

    public bool isReturning()
    {
        if (selectedCharacter == null)
        {
            return false;
        }
        return DOTween.IsTweening(selectedCharacter.transform);
    }
}
