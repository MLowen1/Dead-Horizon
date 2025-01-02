using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;

//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player stats")]
    public int playerHealth = 100;
    private int maxhealth;
    public int resources = 50;
    public int score = 0;
    public int highscore = 0;
    public int collectionSpeed = 1;
    public int MaxResource = 100;
    public int collectionRate = 1;

    [Header ("HUD")]
    public Text playerHealthText;
    public Text resourcesText;
    public Text scoreText;
    public Text highscoreText;

    [Header("Skill Upgrades")]
    [SerializeField] private Button[] UpgradeButton;
    [SerializeField] private int[] UpgradeCost;

    [SerializeField] private int capacityUpgradeAmount = 25;
    [SerializeField] private GameObject ShieldBubble;
    public bool ShieldActive = false;

    [SerializeField] private int[] MaxSkillLevel;
    [SerializeField] private int[] currentSkillLevel;
    [SerializeField] private bool[] HasLevel;


    private GameObject player;
    public bool ismoving = false;

    private BlackHole_Grav BlackHole;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Game manager init");
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject); 
        }

        if (Highscore.instance != null)
        {
            highscore = Highscore.instance.HIGHSCORE;
        }

        else {
            //highscore = 0;
        }

        maxhealth = playerHealth;
        BlackHole = FindObjectOfType<BlackHole_Grav>();
        player = GameObject.FindGameObjectWithTag("Player");
        IsSkillActive();
        StartCoroutine(ScoreOverTime());
        StartCoroutine(ResourceBurn());
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
        IsSkillActive();
        if (instance != null)
        {
            if (playerHealth <= 0)
            {
                if(score > highscore)
                {
                    Highscore.instance.HIGHSCORE = score;
                }
                
                Destroy(player);
                SceneManager.LoadScene(5);
            }
        }
    }

    private void UpdateUI()
    {
        if (scoreText != null && highscoreText != null && playerHealthText != null && resourcesText != null)
        {
            scoreText.text = score.ToString() + " Points";
            highscoreText.text = "HIGHSCORE: " + highscore.ToString();
            playerHealthText.text = "HEALTH: " + playerHealth.ToString();
            resourcesText.text = "RESOURCES: " + resources.ToString();

        }
    }
    public void RestartGame()
    {
        playerHealth = maxhealth;
        resources = 50;
        score = 0;
        collectionRate = 2;
    }

    public void IsSkillActive()
    {
        for (int i = 0; i < UpgradeButton.Length; i++)
        {
            if (resources >= UpgradeCost[i] && (!HasLevel[i] || currentSkillLevel[i] < MaxSkillLevel[i]))
            {
                UpgradeButton[i].interactable = true;
                UpgradeButton[i].image.color = Color.white;
            }
            else
            {
                UpgradeButton[i].interactable = false;
                UpgradeButton[i].image.color = new Color(1, 1, 1, 0.5f);
            }
        }
    }

    public void OnUpgradePressed(int index)
    {
        if (resources >= UpgradeCost[index])
        {
            //Debug.Log("i enter");
            UpgradeEffect(index);            
        }
    }

    private void UpgradeEffect(int index)
    {
        switch (index)
        {
            case 0:
                playerHealth = 100;
                resources -= UpgradeCost[index];
                break;
            case 1:
                if (ShieldActive == false)
                {
                    resources -= UpgradeCost[index];
                    ShieldActive = true;
                    Instantiate(ShieldBubble, player.transform);
                }
                break;
            case 2:
                if (currentSkillLevel[index] < MaxSkillLevel[index])
                {
                    resources -= UpgradeCost[index];
                    MaxResource += capacityUpgradeAmount;
                    currentSkillLevel[index]++;
                }
                break;
            case 3:
                if (currentSkillLevel[index] < MaxSkillLevel[index])
                {
                    resources -= UpgradeCost[index];
                    collectionRate *= 2;
                    currentSkillLevel[index]++;
                }
                break;
            case 4:
                resources -= UpgradeCost[index];
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

                foreach (GameObject enemy in enemies)
                {
                    Destroy(enemy);
                }
                break;
            default:
                Debug.LogError("skill array out of bounds of effects. please add more skills to use within the upgrade effects.");
                break;
        }

        IsSkillActive();
    }

    public IEnumerator ScoreOverTime()
    {
        while (playerHealth > 0)
        {
            score += 100;
            yield return new WaitForSeconds(1);
        }
    }

    public IEnumerator ResourceBurn()
    {
        //Debug.Log("i enter");
        while (resources > 0)
        {
            if (ismoving)
            {
                //Debug.Log("FUEL BURN");
                resources -= 1;  
            }
            yield return new WaitForSeconds(1);
        }
    }
}

