using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseGender : MonoBehaviour {

	public GameObject nextPanel;
	public Sprite maleSprite;
	public Sprite femaleSprite;

	private Button maleButton;
	private Button femaleButton;
	void Start () {
		registerButtons();
	}
	
	private void registerButtons()
	{
		Button[] buttons = gameObject.GetComponentsInChildren<Button>();
		foreach (Button b in buttons) {
			if (b.name == "Male")
			{
				maleButton = b;
			}
			else if (b.name == "Female")
			{
				femaleButton = b;
			}
		}
		maleButton.onClick.AddListener(setMaleCharacter);
		femaleButton.onClick.AddListener(setFemaleCharacter);
	}

	private void setMaleCharacter()
	{
		GameObject.Find("Character").GetComponent<SpriteRenderer>().sprite = maleSprite;
		gameObject.SetActive(false);
		nextPanel.SetActive(true);
	}

	private void setFemaleCharacter()
	{
		GameObject.Find("Character").GetComponent<SpriteRenderer>().sprite = femaleSprite;
		gameObject.SetActive(false);
		nextPanel.SetActive(true);
	}
}
