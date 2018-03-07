using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreation : MonoBehaviour {

	#region variables
	public GameObject characterHUD;
	public GameObject characterDisplay;
	public GameObject input;

	public Sprite maleSprite;
	public Sprite femaleSprite;
    public Sprite dwarfSprite;
    public Sprite elfSprite;
    public Sprite peasantSprite;
    public Sprite ruffianSprite;
    public Sprite khalifateSprite;

    private Button dwarfButton;
	private Button elfButton;
    private Button humanPeasantButton;
    private Button humanRuffianButton;
    private Button khalifateButton;
	#endregion

	void Start () {
		registerButtons();
	}
	
	private void registerButtons()
	{
		Button[] buttons = gameObject.GetComponentsInChildren<Button>();
		foreach (Button b in buttons) {
            switch(b.name)
            {
                case "Dwarf":
                    dwarfButton = b;
                    break;
                case "Elf":
                    elfButton = b;
                    break;
                case "HumanPeasant":
                    humanPeasantButton = b;
                    break;
                case "HumanRuffian":
                    humanRuffianButton = b;
                    break;
                case "Khalifate":
                    khalifateButton = b;
                    break;
            }
		}
		dwarfButton.onClick.AddListener(setDwarfCharacter);
        elfButton.onClick.AddListener(setElfCharacter);
        humanPeasantButton.onClick.AddListener(setHumanPeasantCharacter);
        humanRuffianButton.onClick.AddListener(setHumanRuffianCharacter);
        khalifateButton.onClick.AddListener(setKhalifateCharacter);


    }

    private void setDwarfCharacter()
	{
		GameObject.Find("Character").GetComponent<SpriteRenderer>().sprite = dwarfSprite;
		getCharacterName();
	}

    private void setElfCharacter()
    {
        GameObject.Find("Character").GetComponent<SpriteRenderer>().sprite = elfSprite;
        getCharacterName();
    }

    private void setHumanPeasantCharacter()
    {
        GameObject.Find("Character").GetComponent<SpriteRenderer>().sprite = peasantSprite; 
        getCharacterName();
    }

    private void setHumanRuffianCharacter()
    {
        GameObject.Find("Character").GetComponent<SpriteRenderer>().sprite = ruffianSprite;
        getCharacterName();
    }
    private void setKhalifateCharacter()
    {
        GameObject.Find("Character").GetComponent<SpriteRenderer>().sprite = khalifateSprite;
        getCharacterName();
    }

    private void getCharacterName()
	{
		gameObject.GetComponentInChildren<Text>().text = "ENTER CHARACTER NAME";
		Button[] buttons = gameObject.GetComponentsInChildren<Button>();
		foreach(Button b in buttons) { b.gameObject.SetActive(false); }
		input.SetActive(true);
		input.GetComponentInChildren<Button>().onClick.AddListener(getName);
	}

	private void getName()
	{
		gameObject.SetActive(false);
		characterDisplay.SetActive(true);
		characterDisplay.GetComponentInChildren<Text>().text = input.GetComponent<InputField>().text.ToUpper();
		characterHUD.SetActive(true);
		input.SetActive(false);
	}
}
