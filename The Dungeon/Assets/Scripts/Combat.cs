using System.Collections;
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

	static string[] enemies = { "Skeleton", "Zombie", "Goblin", "Orc", "Spectre" };
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

	private bool running = true;

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
    }

    private void setButtons()
    {
        Button[] buttons = gameObject.GetComponentsInChildren<Button>();
        foreach (Button b in buttons)
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
                case "BotLeft":
                    midLeft = b;
                    break;
                case "BotRight":
                    midRight = b;
                    break;
            }
        }
    }

    private void setSliders()
    {
        GameObject playerDisplay = GameObject.Find("PlayerDisplay");
        Slider[] sliders = playerDisplay.GetComponentsInChildren<Slider>();
        foreach (Slider s in sliders)
        {
            if (s.name == "HP Bar")
                playerHealth = s;
            else if (s.name == "XP Bar")
                playerExperience = s;
        }
        GameObject enemyDisplay = GameObject.Find("EnemyDisplay");
        enemyHealth = enemyDisplay.GetComponentInChildren<Slider>();
    }
}
