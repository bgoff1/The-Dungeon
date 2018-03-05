﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Combat : MonoBehaviour {

	#region Variables

	#region GameVariables
	private int monsterLevels = 0;
	private int monstersDefeated = 0;
    private int totalGoldCollected= 0;

	private int itemInShop = 0;

	public Sprite[] enemies;
    private GameObject enemy;
    private string enemyName;
    private string[] bosses = { "Giant", "Ogre", "Vampire" };
    private int maxEnemyHealth = 50;
    private int enemyAttackDamage = 25;
    private int maxEnemyExperience = 100;
    private bool enemyIsBoss = false;
        
    private int experienceNeeded;
        
	//options: 1 - weapon, 2-armor, 3-cloak, 4-dagger,
	        // 5-Spellbook, 6-bow, 7-shield, 8-druidic amulet
    private int[] possibleRogueOptions = {1, 2, 3, 4, 8};
    private int[] possibleWarriorOptions = {1, 2, 3, 7, 8};
    private int[] possibleWizardOptions = {1, 2, 3, 5, 8};
    private int[] possibleArcherOptions = {1, 2, 3, 6, 8};
    private int[] possibleDruidOptions = {1, 2, 3, 8, 9};
	#endregion
	#region PlayerVariables
	private int maxHealthPotions =5;
    private int maxHealth=100;
    private int currentHealth=100;
    private int attackDamage = 50;
    private int numHealthPotions = 3;
    private int playerGold = 50;
    private int escapeRopes = 0;
    private int critChance = 5; //percent
    private bool magicWeaponOwned = false; //+10% damage
    private bool magicArmorOwned = false; //-10% damage taken
    private bool magicCloakOwned = false; //+10% dodge chance
    private bool magicDaggerOwned = false; //rogue only - 20% chance for extra 50% attack
    private bool magicSpellbookOwned = false; //wizard only - 10% chance for random spell to be cast
    private bool magicBowOwned = false; //archer only - 10% chance for headshot (2x damage)
    private bool magicShieldOwned = false; //warrior only - +10% block, reflects 20% of all incoming damage
    private bool magicPendantOwned = false; //+5% crit chance
    private bool magicStaffOwned = false; //druid only - gets to choose 2 aspects
	#endregion
	#region DifficultyVariables
	private int healthPotionHealAmount = 30;
    private int healthPotionDropChance = 35; //Percentage
    private int attackMedium = 7;
    private int healthMedium = 20;
    private int potionGainStandard = 10;
    private int enemyDifficultyStandard = 10;
    private int experienceNeededMultiplier = 300;
	#endregion
	#region LevelingVariables
	private int level = 1;
	private int currentExperience = 0;

	private int playerClass = 0;
	private int dodgeBlockChance = 0;
	#endregion
	#region SpellVariables
	private int maxSpellSlots = 0;
	private int currentSpellSlots = 0;
	#endregion
    #region Buttons
    private Button topLeft;
    private Button topRight;
    private Button midLeft;
    private Button midRight;
    private Button bottomLeft;
    private Button bottomRight;
    #endregion
    #region Sliders
    private Slider playerHealth;
    private Text playerHealthText;
    private Slider playerExperience;
    private Slider enemyHealth;
    private Text enemyHealthText;
    #endregion
    public Text gameText;
    public Text potionCount;
    public GameObject loseScreen;
    public GameObject[] buttons;
    public GameObject enemyDisplay;
	#endregion

	void Start() {
		switch (SceneManager.GetActiveScene().name)
		{
			case "SandboxGame":
                attackMedium+=8;
                healthMedium+=10;
                potionGainStandard+=10;
                enemyDifficultyStandard-=5;
                healthPotionHealAmount=50;
                healthPotionDropChance=45;
                experienceNeededMultiplier=250;
                break;
            case "EasyGame":
                attackMedium+=3;
                healthMedium+=5;
                potionGainStandard+=5;
                enemyDifficultyStandard-=3;
                healthPotionHealAmount=40;
                healthPotionDropChance=40;
                experienceNeededMultiplier=275;
                break;
            case "HardGame":
                attackMedium -= 1;
                healthMedium-=5;
                potionGainStandard-=3;
                enemyDifficultyStandard+=3;
                healthPotionHealAmount=25;
                healthPotionDropChance=30;
                experienceNeededMultiplier=325;
                break;
            case "NightmareGame":
                attackMedium -= 2;
                healthMedium -=7;
                potionGainStandard -=5;
                enemyDifficultyStandard+=5;
                healthPotionHealAmount=20;
                healthPotionDropChance=25;
                experienceNeededMultiplier=350;
                break;
		}
	}

    // Awake is called when the GameObject it is attached to
        // becomes active
	void Awake()
    {
        // Do fight logic here:
        setButtons();
        setSliders();
        setText();
        enemy = GameObject.Find("Enemy");
        fight();
    }

    private void setButtons()
    {
        Button[] Buttons = gameObject.GetComponentsInChildren<Button>();
        foreach (Button b in Buttons)
        {
            switch (b.name)
            {
                case "TopLeft":
                    topLeft = b;
                    break;
                case "TopRight":
                    topRight = b;
                    break;
                case "MidLeft":
                    midLeft = b;
                    break;
                case "MidRight":
                    midRight = b;
                    break;
                case "BottomLeft":
                    bottomLeft = b;
                    break;
                case "BottomRight":
                    bottomRight = b;
                    break;
            }
        }
    }

    private void setSliders()
    {
        GameObject playerDisplay = GameObject.Find("Player Display");
        Slider[] sliders = playerDisplay.GetComponentsInChildren<Slider>();
        foreach (Slider s in sliders)
        {
            if (s.name == "HP Bar")
                playerHealth = s;
            else if (s.name == "XP Bar")
                playerExperience = s;
        }
        playerHealth.maxValue = maxHealth;
        playerHealth.value = playerHealth.maxValue;
        enemyHealth = enemyDisplay.GetComponentInChildren<Slider>();
        enemyHealth.maxValue = maxEnemyHealth;
        enemyHealth.value =  enemyHealth.maxValue;
    }

    private void setText()
    {
        Text[] texts = GameObject.Find("Player Display").GetComponentsInChildren<Text>();
        playerHealthText = setHealthText(texts, playerHealthText);
        texts = enemyDisplay.GetComponentsInChildren<Text>();
        enemyHealthText = setHealthText(texts, enemyHealthText);
        playerHealthText.text = playerHealth.value.ToString() + "/" + playerHealth.maxValue.ToString();
    }

    private Text setHealthText(Text[] texts, Text textToSet)
    {
        foreach (Text t in texts)
        {
            if (t.name == "Text")
                textToSet = t;
        }
        return textToSet;
    }

    private void updateSliders()
    {
        playerHealthText.text = playerHealth.value.ToString() + "/" + playerHealth.maxValue.ToString();
        enemyHealthText.text  = enemyHealth.value.ToString() + "/" + enemyHealth.maxValue.ToString();
    }

    private void attack()
    {
        int damageDealt = Random.Range(0, attackDamage);
		int damageTaken = Random.Range(0, enemyAttackDamage);

		enemyHealth.value -= damageDealt;
		playerHealth.value -= damageTaken;
        updateSliders();

        if (playerHealth.value < 1) {
            gameText.text = "You have taken too much damage, you are too weak to go on";
            gameText.text = gameText.text.ToUpper();
            playerDeath();
        }
        if (enemyHealth.value < 1)
            enemyDefeated();
    }

    private void drinkPotion()
    {
        if (numHealthPotions > 0) {
            playerHealth.value += healthPotionHealAmount;
            updateSliders();
            numHealthPotions--;
            updatePotionCount();
        } else {
            gameText.text  = "You have no health potions, defeat enemies for a chance to get one";
            gameText.text = gameText.text.ToUpper();
        }
    }

    private void runAway()
    {
        gameText.text = "You run away from the " + enemyName + ".";
        gameText.text = gameText.text.ToUpper();
    }

    private void enemyDefeated()
    {
        gameText.text = enemyName + " was defeated!";
        if (Random.Range(0,100) < healthPotionDropChance) 
        {
            numHealthPotions++;
            gameText.text += "\nThe " + enemyName + " dropped a health potion!";
            string hpnum = "health potions";;
            if (numHealthPotions == 1)
                hpnum = "health potion";
            gameText.text += "\nYou now have " + numHealthPotions + " " + hpnum + "!";
            updatePotionCount();
        }
        nextEnemy();
    }

    private void playerDeath()
    {
        gameText.text = "You limp out of the dungeon, weak from battle.";
        loseScreen.SetActive(true);
    }

    private void reducePlayerHealth()
    {
        playerHealth.value -= 5;
        if (playerHealth.value < 1)
        {
            playerDeath();
        }
        playerHealthText.text = playerHealth.value.ToString() + "/" + playerHealth.maxValue.ToString();
    }

    private void updatePotionCount()
    {
        potionCount.text = "POTIONS: " + numHealthPotions;
    }

    private void nextEnemy()
    {
        buttons[0].SetActive(false);
        bottomRight.gameObject.SetActive(false);
        bottomLeft.gameObject.SetActive(true);
        bottomLeft.GetComponentInChildren<Text>().text = "CONTINUE";
        bottomLeft.onClick.AddListener(spawnNewEnemy);
    }

    private void spawnNewEnemy()
    {
        enemy.GetComponent<SpriteRenderer>().sprite = enemies[Random.Range(0, enemies.Length)];
        enemyDisplay.GetComponentInChildren<Text>().text = enemy.GetComponent<SpriteRenderer>().sprite.name.ToUpper();
        enemyHealth.value = enemyHealth.maxValue;
        updateSliders(); 
        buttons[0].SetActive(true);
        bottomRight.gameObject.SetActive(true);
        bottomLeft.gameObject.SetActive(false);
    }

    private void fight()
    {
        gameText.text = "";
        updatePotionCount();
        buttons[1].SetActive(false);
        bottomLeft.gameObject.SetActive(false);
        topLeft.GetComponentInChildren<Text>().text = "ATTACK";
        topLeft.onClick.AddListener(attack);
        topRight.GetComponentInChildren<Text>().text = "DRINK POTION";
        topRight.onClick.AddListener(drinkPotion);
        bottomRight.GetComponentInChildren<Text>().text = "RUN AWAY";
        bottomRight.onClick.AddListener(runAway);
        enemy.GetComponent<SpriteRenderer>().sprite = enemies[Random.Range(0, enemies.Length)];
        enemyDisplay.SetActive(true);
        enemyDisplay.GetComponentInChildren<Text>().text = enemy.GetComponent<SpriteRenderer>().sprite.name.ToUpper();
        updateSliders();
        enemyName = enemyDisplay.GetComponentInChildren<Text>().text;
    }
}