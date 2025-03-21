using UnityEngine;
using UnityEngine.UI;

public class SwitchIconController : MonoBehaviour
{
    public SwitchCharacter switchCharacter;
    public Image iconImage;
    public Sprite characterSprite;
    public Sprite carSprite;

    private void Start()
    {
        // Set initial icon based on current state
        UpdateIcon(switchCharacter.character.activeSelf); 
    }

    public void OnIconClick()
    {
        switchCharacter.Switch();
        UpdateIcon(switchCharacter.character.activeSelf);
    }

    private void UpdateIcon(bool isCharacterActive)
    {
        iconImage.sprite = isCharacterActive ? carSprite : characterSprite;
    }
} 