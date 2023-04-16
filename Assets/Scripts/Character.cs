using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [SerializeField] string characterName;
    [SerializeField] CharacterType type;
    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;
    [SerializeField] int attackPower;
    [SerializeField] TMP_Text overheadUI;
    [SerializeField] Button charButton;
    [SerializeField] Image charAvatar;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text typeText;
    [SerializeField] Image healthBar;
    [SerializeField] TMP_Text healthText;
    private Vector3 initialPos;

    public Button plButton { get => charButton; }
    public CharacterType chType { get => type; }
    public int chAttackPower { get => attackPower; }
    public int chCurrentHealth { get => currentHealth; }
    public Vector3 chInitialPos { get => initialPos; }
    public int chMaxHealth { get => maxHealth; }

    private void Start()
    {
        overheadUI.text = characterName;
        // charButton.onClick.AddListener(OnCharButtonClicked);
        nameText.text = characterName;
        typeText.text = type.ToString();
        healthText.text = currentHealth + "/" + maxHealth;
        healthBar.fillAmount = (float)currentHealth / maxHealth;
        charButton.interactable = false;
        initialPos = this.transform.position;
    }

    public void ChangeHP(int HPamount)
    {
        currentHealth += HPamount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        HealthUIUpdate();
    }

    private void HealthUIUpdate()
    {
        healthText.text = currentHealth + "/" + maxHealth;
        healthBar.fillAmount = (float)currentHealth / maxHealth;
    }

    public void ShowAttackSprite()
    {
        throw new NotImplementedException();
    }
}
