using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

public class InGameUI : MonoBehaviour
{
    public static InGameUI Instance;
    
    [Header("InventoryElements")]
    [SerializeField] private TextMeshProUGUI inventoryText;
    [SerializeField] private TextMeshProUGUI inventoryHeader;
    [SerializeField] private Image inventoryImage;
    [SerializeField] private InGameCollectedObjects collectedObjects;
    [SerializeField] private GameObject inventory;

    [Header("Collectibles")]
    private string brokenLightsText;
    private string teddyText;
    private string newsPaperText;
    private string brokenLightsHeader;
    private string teddyHeader;
    private string newsPaperHeader;
    private Sprite brokenLightsSprite;
    private Sprite teddySprite;
    private Sprite newsPaperSprite;

    [Header("Weapons")]
    private string popcornPistolText;
    private string frenchFriesAssaultRifleText;
    private string magnumMagnumText;
    private string cornDogHuntingRifleText;
    private string lollipopShotgunText;
    private string popcornPistolHeader;
    private string frenchFriesAssaultRifleHeader;
    private string magnumMagnumHeader;
    private string cornDogHuntingRifleHeader;
    private string lollipopShotgunHeader;
    private Sprite popcornPistolSprite;
    private Sprite frenchFriesAssaultRifleSprite;
    private Sprite magnumMagnumSprite;
    private Sprite cornDogHuntingRifleSprite;
    private Sprite lollipopShotgunSprite;

    [Header("PreviewObjects")]
    [SerializeField] private GameObject brokenLights;
    [SerializeField] private GameObject teddy;
    [SerializeField] private GameObject newsPaper;
    [SerializeField] private GameObject popcornPistol;
    [SerializeField] private GameObject frenchFriesAssaultRifle;
    [SerializeField] private GameObject magnumMagnum;
    [SerializeField] private GameObject cornDogHuntingRifle;
    [SerializeField] private GameObject lollipopShotgun;
    
    [Header("Fight")]
    public Animator loadingScreenAnim;
    [SerializeField] public GameObject fightScene;
    [SerializeField] public GameObject pressSpace;
    [SerializeField] private GameObject abilityProgress;
    [SerializeField] public GameObject rideTimeSlider;
    [SerializeField] public GameObject rideHpSlider;
    [SerializeField] public GameObject inventoryWeapon;

    [Header("Components")]
    [SerializeField] private GameObject eIndicator;
    [SerializeField] public GameObject inGameUIScreen;
    [SerializeField] public GameObject firstInventorySelected;
    [SerializeField] private GameObject whiteScreen;
    [SerializeField] private GameObject inventoryButton; 

    [Header("Booleans")]
    private bool isPlayerNotNull;
    [HideInInspector] public bool inventoryIsOpened;
    private bool changeLight;

    [Header("Dialogue")]
    public float textDisplaySpeed = 0.04f;
    [SerializeField] private List<string> dialogues;
    [SerializeField] private List<string> dialogues2;
    [SerializeField] private List<string> dialogues3;
    [SerializeField] private List<string> dialogues4;
    [SerializeField] private TextMeshProUGUI dialogueText;
    private int dialogueTextCount;
    public int dialogueCount;
    public bool dialogueLeft;
    public bool canPlayNext;
    public bool textIsPlaying;
    public bool canEndDialogue;
    [SerializeField] public Animator radioAnim;
    [SerializeField] private Animator dialogueBoxAnim;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        dialogueLeft = true;
    }

    private void Update()
    {
        if (Player.Instance != null)
        {
            UpdateAbilityProgress();

            if (Player.Instance.playerCanInteract)
            {
                var componentColor = eIndicator.GetComponent<Image>().color;
                componentColor.a = 1;
                eIndicator.GetComponent<Image>().color = componentColor;
            }
            else
            {
                var componentColor = eIndicator.GetComponent<Image>().color;
                componentColor.a = 0.2156862745098039f;
                eIndicator.GetComponent<Image>().color = componentColor;
            }

            if (changeLight)
            {
                AudioManager.Instance.Stop("InGameMusic");
                Player.Instance.globalLightObject.GetComponent<Light2D>().intensity =
                    Mathf.MoveTowards(Player.Instance.globalLightObject.GetComponent<Light2D>().intensity, 5, Time.deltaTime);
                if (Player.Instance.globalLightObject.GetComponent<Light2D>().intensity >= 3)
                {
                    whiteScreen.SetActive(true);
                    whiteScreen.GetComponent<Image>().color =
                        new Color(1, 1, 1, Mathf.MoveTowards(0, 1, Time.deltaTime));
                    GameSaveStateManager.Instance.GoToMainMenu();
                    whiteScreen.GetComponent<Image>().color =
                        new Color(1, 1, 1, Mathf.MoveTowards(1, 0, Time.deltaTime));
                    whiteScreen.SetActive(false);
                }
            }
        }
    }
    
    public void PressButtonSound()
    {
        AudioManager.Instance.Play("ButtonClick");
    }

    //Sets the current ability that was gained to the according slider
    private void UpdateAbilityProgress()
    {
        abilityProgress.GetComponent<Slider>().value =
            Player.Instance.currentAbilityTime / Player.Instance.maxAbilityTime;
    }

    //Sets everything inactive when in main menu and unpauses game
    public void GoToMainMenu()
    {
        brokenLights.SetActive(false);
        newsPaper.SetActive(false);
        teddy.SetActive(false);
        magnumMagnum.SetActive(false);
        cornDogHuntingRifle.SetActive(false);
        popcornPistol.SetActive(false);
        lollipopShotgun.SetActive(false);
        frenchFriesAssaultRifle.SetActive(false);
        inventoryWeapon.SetActive(false);
        fightScene.SetActive(false);
        
        inventoryText.text = "";
        inventoryHeader.text = "";
        inventoryImage.gameObject.SetActive(false);
        
        GameSaveStateManager.Instance.GoToMainMenu();
        PauseGame();
    }

    //this is called via the "save game" button
    public void SaveGame()
    {
        GameSaveStateManager.Instance.SaveGame();
    }

    //Display nothing first to display something new
    private void DisplayNothing()
    {
        inventoryText.text = "";
        inventoryHeader.text = "";
        inventoryImage.gameObject.SetActive(true);
    }
    
    public void DisplayPopCornPistol()
    {
        DisplayNothing();
        inventoryText.text += popcornPistolText;
        inventoryHeader.text += popcornPistolHeader;
        inventoryImage.sprite = popcornPistolSprite;
    }
    
    public void DisplayMagnumMagnum()
    {
        DisplayNothing();
        inventoryText.text += magnumMagnumText;
        inventoryHeader.text += magnumMagnumHeader;
        inventoryImage.sprite = magnumMagnumSprite;
    }
    
    public void DisplayFrenchFriesAssaultRifle()
    {
        DisplayNothing();
        inventoryText.text += frenchFriesAssaultRifleText;
        inventoryHeader.text += frenchFriesAssaultRifleHeader;
        inventoryImage.sprite = frenchFriesAssaultRifleSprite;
    }
    
    public void DisplayCornDogHuntingRifle()
    {
        DisplayNothing();
        inventoryText.text += cornDogHuntingRifleText;
        inventoryHeader.text += cornDogHuntingRifleHeader;
        inventoryImage.sprite = cornDogHuntingRifleSprite;
    }
    
    public void DisplayLollipopShotgun()
    {
        DisplayNothing();
        inventoryText.text += lollipopShotgunText;
        inventoryHeader.text += lollipopShotgunHeader;
        inventoryImage.sprite = lollipopShotgunSprite;
    }
    
    public void DisplayTeddy()
    {
        DisplayNothing();
        inventoryText.text += teddyText;
        inventoryHeader.text += teddyHeader;
        inventoryImage.sprite = teddySprite;
    }
    
    public void DisplayNewspaper()
    {
        DisplayNothing();
        inventoryText.text += newsPaperText;
        inventoryHeader.text += newsPaperHeader;
        inventoryImage.sprite = newsPaperSprite;
    }
    
    public void DisplayLights()
    {
        DisplayNothing();
        inventoryText.text += brokenLightsText;
        inventoryHeader.text += brokenLightsHeader;
        inventoryImage.sprite = brokenLightsSprite;
    }

    //Goes through every collected collectibles and displays them in the inventory
    private void DisplayCollectedCollectibles()
    {
        var collectedCollectibles = GameSaveStateManager.Instance.saveGameDataManager.collectedCollectiblesIdentifiers;

        for (int index = 0; index < collectedCollectibles.Count; index++)
        {
            var text = "";
        
            var headerText = "";
            
            var collectible = collectedObjects.GetCollectibleDataByIdentifier(collectedCollectibles[index]);
            if (collectible == null)
                return;
            headerText += collectible.header;
            text += collectible.content;
            var spriteCollectible = collectible.icon;
            
            switch (headerText)
            {
                case "Broken Lights" :
                    brokenLights.SetActive(true);
                    brokenLightsSprite = spriteCollectible;
                    brokenLightsText = text;
                    brokenLightsHeader = headerText;
                    break;
                case "News Paper" :
                    newsPaper.SetActive(true);
                    newsPaperSprite = spriteCollectible;
                    newsPaperText = text;
                    newsPaperHeader = headerText;
                    break;
                case "Stuffed Animal" :
                    teddy.SetActive(true);
                    teddySprite = spriteCollectible;
                    teddyText = text;
                    teddyHeader = headerText;
                    break;
            }
            
        }
    }
    
    //Goes through every collected weapons and displays them in the inventory
    private void DisplayCollectedWeapons()
    {
        var collectedCollectibles = GameSaveStateManager.Instance.saveGameDataManager.collectedWeaponsIdentifiers;

        for (int index = 0; index < collectedCollectibles.Count; index++)
        {
            var headerText = "";
            
            var text = "";

            var weapon = collectedObjects.GetWeaponDataByIdentifier(collectedCollectibles[index]);
            if (weapon == null)
                return;
            headerText += weapon.weaponName;
            text += weapon.weaponDescription;
            text += "\n" + "\n" + "Special Ability:" + "\n" + weapon.weaponAbilityDescription;
            var spriteWeapon = weapon.inGameWeaponVisual;
            
            var itemIdentifier = headerText;
            
            switch (itemIdentifier)
            {
                case "Magnum magnum" :
                    magnumMagnum.SetActive(true);
                    magnumMagnumSprite = spriteWeapon;
                    magnumMagnumText = text;
                    magnumMagnumHeader = headerText;
                    break;
                case "French Fries AR" :
                    frenchFriesAssaultRifle.SetActive(true);
                    frenchFriesAssaultRifleSprite = spriteWeapon;
                    frenchFriesAssaultRifleText = text;
                    frenchFriesAssaultRifleHeader = headerText;
                    break;
                case "Lollipop Shotgun" :
                    lollipopShotgun.SetActive(true);
                    lollipopShotgunSprite = spriteWeapon;
                    lollipopShotgunText = text;
                    lollipopShotgunHeader = headerText;
                    break;
                case "Corn Dog Hunting Rifle" :
                    cornDogHuntingRifle.SetActive(true);
                    cornDogHuntingRifleSprite = spriteWeapon;
                    cornDogHuntingRifleText = text;
                    cornDogHuntingRifleHeader = headerText;
                    break;
                case "Popcorn Pistol" :
                    popcornPistol.SetActive(true);
                    popcornPistolSprite = spriteWeapon;
                    popcornPistolText = text;
                    popcornPistolHeader = headerText;
                    break;
            }
        }
    }

    private void DisplayFirstDialogue()
    {
        if (dialogueTextCount == dialogues.Count - 1)
        {
            dialogueLeft = false;
        }
        
        StartCoroutine(LetterByLetterTextCoroutine(dialogueText, dialogues[dialogueTextCount]));
        
        dialogueTextCount++;
    }
    
    private void DisplaySecondDialogue()
    {
        if (dialogueTextCount == dialogues2.Count - 1)
        {
            dialogueLeft = false;
        }
        
        StartCoroutine(LetterByLetterTextCoroutine(dialogueText, dialogues2[dialogueTextCount]));
        
        dialogueTextCount++;
    }
    
    private void DisplayThirdDialogue()
    {
        if (dialogueTextCount == dialogues3.Count - 1)
        {
            dialogueLeft = false;
        }
        
        StartCoroutine(LetterByLetterTextCoroutine(dialogueText, dialogues3[dialogueTextCount]));
        
        dialogueTextCount++;
    }
    
    private void DisplayFourthDialogue()
    {
        if (dialogueTextCount == dialogues4.Count - 1)
        {
            dialogueLeft = false;
        }
        
        StartCoroutine(LetterByLetterTextCoroutine(dialogueText, dialogues4[dialogueTextCount]));
        
        dialogueTextCount++;
    }
    
    //an implementation for displaying a text letter by letter.
    private IEnumerator LetterByLetterTextCoroutine(TextMeshProUGUI textField, string text)
    {
        var currentText = "";

        textIsPlaying = true;
        
        for (int index = 0; index < text.Length; index++)
        {
            currentText += text[index];
            textField.text = currentText;
            yield return new WaitForSeconds(textDisplaySpeed);
        }

        textIsPlaying = false;

        if (!dialogueLeft)
        {
            canEndDialogue = true;
        }
        else
        {
            canPlayNext = true;
        }
    }
    
    //Activates everything needed to display a dialogue and starts the dialogue that is next in the story
    public IEnumerator DisplayDialogueElements()
    {
        Player.Instance.isPlayingDialogue = true;
        
        yield return new WaitForSeconds(1);

        radioAnim.SetTrigger("RadioOn");

        yield return new WaitForSeconds(1);
        
        dialogueBoxAnim.SetTrigger("DialogueBoxOn");
        
        yield return new WaitForSeconds(0.24f);

        Player.Instance.isInteracting = true;

        GetDialogue();
    }
    
    //Puts away everything that was displayed before and resets values 
    public void EndDialogue()
    {
        canEndDialogue = false;
        Player.Instance.isPlayingDialogue = false;
        dialogueTextCount = 0;
        dialogueCount++;
        canPlayNext = false;
        dialogueText.text = "";
        dialogueLeft = true;
        Player.Instance.isInteracting = false;
        radioAnim.SetTrigger("RadioOff");
        dialogueBoxAnim.SetTrigger("DialogueBoxOff");
        
        if (dialogueCount == 4)
        {
            Player.Instance.isPlayingDialogue = true;
            Player.Instance.isInteracting = true;
            AudioManager.Instance.Play("WalkieTalkie");
            changeLight = true;
        }
    }

    //Plays next dialogue when screen is clicked
    public void PlayNext()
    {
        canPlayNext = false;

        dialogueText.text = "";
        
        textDisplaySpeed = 0.04f;

        GetDialogue();
    }

    //Gets dialogue that is next in the story
    private void GetDialogue()
    {
        if (dialogueCount == 0)
        {
            DisplayFirstDialogue();
        }
        else if (dialogueCount == 1)
        {
            DisplaySecondDialogue();
        }
        else if (dialogueCount == 2)
        {
            DisplayThirdDialogue();
        }
        else if (dialogueCount == 3)
        {
            DisplayFourthDialogue();
        }
    }
    
    //Opens Inventory
    public void PauseGame()
    {
        if (inventoryIsOpened)
        {
            inventoryButton.SetActive(true);
            inventoryIsOpened = false;
            inventory.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            inventoryButton.SetActive(false);
            inventoryIsOpened = true;
            inventory.SetActive(true);
            DisplayCollectedCollectibles();
            DisplayCollectedWeapons();
            EventSystem.current.SetSelectedGameObject(firstInventorySelected);
        }
    }

    //Activates inGame ui and starts off with the first dialogue when a new was started
    public void ActivateInGameUI()
    {
        inGameUIScreen.SetActive(true);
        if (GameSaveStateManager.Instance.startedNewGame)
        {
            StartCoroutine(DisplayDialogueElements());
        }
        else
        {
            dialogueCount = 1;
        }

        GameSaveStateManager.Instance.startedNewGame = false;
    }
}
