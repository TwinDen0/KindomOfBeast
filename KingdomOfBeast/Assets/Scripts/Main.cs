using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public int money = 0;
    public Camera cam;
    [SerializeField] TextMeshProUGUI moneyText;
    public bool music;
    public bool sounds;
    public bool vibration;

    public int rewards;
    [SerializeField] TextMeshProUGUI rewardsTextLose;
    [SerializeField] TextMeshProUGUI rewardsTextWin;
    [SerializeField] GameObject LoseWindow;
    [SerializeField] GameObject WinWindow;

    public List<int> openedTypes = new List<int>() { 1, 1 };
    //0 - Towers
    //1 - Casern
    public List<TypeCollection> typesCollection;
    [SerializeField] TextMeshProUGUI collectionText;
    [SerializeField] TextMeshProUGUI levelText;

    [SerializeField] NavMeshPlus.Components.NavMeshSurface navMeshsurFace;

    public King king;
    [SerializeField] Animator[] kingAnim;
    public float kingPower = 1.1f;
    //Lion = damage
    //Frog = Speed
    //Pig = HP

    public bool battle = false;

    public Color hitColor = new Color(1f, 0.7f, 0.7f, 1f);

    [SerializeField] GameObject musicDisable;
    [SerializeField] GameObject soundsDisable;
    [SerializeField] GameObject vibrationDisable;

    public enum King
    {
        Lion,
        Frog,
        Pig
    }
    enum CollectionType
    {
        Towers,
        Casern
    }

    public static Main Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("Music"))
        {
            if(PlayerPrefs.GetInt("Music") == 0)
            {
                musicDisable.SetActive(true);
                OffMusic();
            }
        }
        if (PlayerPrefs.HasKey("Sounds"))
        {
            if (PlayerPrefs.GetInt("Sounds") == 0)
            {
                soundsDisable.SetActive(true);
                OffSounds();
            }
        }
        if (PlayerPrefs.HasKey("Vibration"))
        {
            if (PlayerPrefs.GetInt("Vibration") == 0)
            {
                vibrationDisable.SetActive(true);
                OffVibration();
            }
        }

        if (Main.Instance.music)
        {
            AudioManager.instance.Play("LobbyMusic");
        }

        if (PlayerPrefs.HasKey("Level"))
        {
            SetLevelText();
        }
        if (PlayerPrefs.HasKey("OpenedTypeTower"))
        {
            openedTypes[0] = PlayerPrefs.GetInt("OpenedTypeTower");
        }
        if (PlayerPrefs.HasKey("OpenedTypeCasern"))
        {
            openedTypes[1] = PlayerPrefs.GetInt("OpenedTypeCasern");
        }
        if (PlayerPrefs.HasKey("Money"))
        {
            money = PlayerPrefs.GetInt("Money");
        }
        if (PlayerPrefs.HasKey("King"))
        {
            int kingInt = PlayerPrefs.GetInt("King");
            switch (kingInt)
            {
                case 0:
                    king = King.Lion;
                    break;
                case 1:
                    king = King.Frog;
                    break;
                case 2:
                    king = King.Pig;
                    break;
            }
        }
        SetTextMoney();
        SetCollection();
        switch (king)
        {
            case King.Lion:
                kingAnim[0].gameObject.SetActive(true);
                kingAnim[0].Play("Wait");
                break;
            case King.Frog:
                kingAnim[1].gameObject.SetActive(true);
                kingAnim[1].Play("Wait");
                break;
            case King.Pig:
                kingAnim[2].gameObject.SetActive(true);
                kingAnim[2].Play("Wait");
                break;
        }
    }
    public void ChooseKing(int king)
    {
        switch (king)
        {
            case 0:
                PlayerPrefs.SetInt("King", 0);
                break;
            case 1:
                PlayerPrefs.SetInt("King", 1);
                break;
            case 2:
                PlayerPrefs.SetInt("King", 2);
                break;
        }
    }
    public void StartBattle()
    {
        if (Main.Instance.music)
        {
            AudioManager.instance.Stop("LobbyMusic");
            AudioManager.instance.Play("BattleMusic");
        }
        OffPause();
        battle = true;
        navMeshsurFace.BuildNavMesh();
        ManagerBuilds.Instance.StartBattle();
        ManagerEnemies.Instance.StartBattle();
    }
    public void OnPause()
    {
        Time.timeScale = 0;
    }
    public void OffPause()
    {
        Time.timeScale = 1;
    }
    public void ReloadScene()
    {
        if (Main.Instance.music)
        {
            AudioManager.instance.Stop("BattleMusic");
        }
        SceneManager.LoadScene("Main");
    }
    public void Win()
    {
        
        if (Main.Instance.music)
        {
            AudioManager.instance.Stop("BattleMusic");
        }
        if (Main.Instance.sounds)
        {
            AudioManager.instance.Play("WinSound");
        }

        OnPause();
        switch (king)
        {
            case King.Lion:
                kingAnim[0].Play("Win");
                break;
            case King.Frog:
                kingAnim[1].Play("Win");
                break;
            case King.Pig:
                kingAnim[2].Play("Win");
                break;
        }
        PlayerPrefs.SetInt("Money", money);
        rewardsTextWin.text = "+" + rewards.ToString();
        int level = PlayerPrefs.GetInt("Level") + 1;
        PlayerPrefs.SetInt("Level", level);
        StartCoroutine(WinWindowUp());
    }
    IEnumerator WinWindowUp()
    {
        yield return new WaitForSecondsRealtime(1f);
        WinWindow.SetActive(true);
    }
    public void Lose()
    {

        if (Main.Instance.music)
        {
            AudioManager.instance.Stop("BattleMusic");
        }
        if (Main.Instance.sounds)
        {
            AudioManager.instance.Play("LoseSound");
        }

        OnPause();
        switch (king)
        {
            case King.Lion:
                kingAnim[0].Play("Lose");
                break;
            case King.Frog:
                kingAnim[1].Play("Lose");
                break;
            case King.Pig:
                kingAnim[2].Play("Lose");
                break;
        }
        PlayerPrefs.SetInt("Money", money);
        rewardsTextLose.text = "+" + rewards.ToString();
        StartCoroutine(LoseWindowUp());
    }
    IEnumerator LoseWindowUp()
    {
        yield return new WaitForSecondsRealtime(1f);
        LoseWindow.SetActive(true);
    }
    public void SetCollection()
    {
        int allNum = 0;
        foreach(var num in openedTypes)
        {
            allNum += num;
        }
        PlayerPrefs.SetInt("OpenedTypeTower", openedTypes[0]);
        PlayerPrefs.SetInt("OpenedTypeCasern", openedTypes[1]);
        collectionText.text = $"{allNum}/{openedTypes.Count * 5} Unlocked";
        for (int i = 0; i < typesCollection.Count; i++)
        {
            typesCollection[i].SetCount(openedTypes[i]);
        }
    }
    public void SetLevelText()
    {
        if (PlayerPrefs.HasKey("Level"))
        {
            levelText.text = $"Level {PlayerPrefs.GetInt("Level") + 1}";
        }
        else
        {
            levelText.text = $"Level 1";
        }
    }
    public void AddMoney(int count)
    {
        money += count;
        SetTextMoney();
    }
    void SetTextMoney()
    {
        moneyText.text = money.ToString();
    }
    public void OnMusic()
    {
        music = true;
        AudioManager.instance.Play("LobbyMusic");
        PlayerPrefs.SetInt("Music", 1);
    }
    public void OffMusic()
    {
        music = false;
        AudioManager.instance.Stop("LobbyMusic");
        PlayerPrefs.SetInt("Music", 0);
    }
    public void OnSounds()
    {
        sounds = true;
        PlayerPrefs.SetInt("Sounds", 1);
    }
    public void OffSounds()
    {
        sounds = false;
        PlayerPrefs.SetInt("Sounds", 0);
    }
    public void OnVibration()
    {
        vibration = true;
        PlayerPrefs.SetInt("Vibration", 1);
    }
    public void OffVibration()
    {
        vibration = false;
        PlayerPrefs.SetInt("Vibration", 0);
    }
}
