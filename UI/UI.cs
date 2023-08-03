using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour, ISaveManager
{
    [Header("End screen")]
    [SerializeField] private UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject mainMenuButton;
    [Space]

    [SerializeField] private GameObject charcaterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;



    public UI_SkillToolTip skillToolTip;
    public UI_ItemTooltip itemToolTip;
    public UI_StatToolTip statToolTip;
    public UI_CraftWindow craftWindow;

    [SerializeField] private UI_VolumeSlider[] volumeSettings;

    public Player player;


    private void Awake()
    {
        SwitchTo(skillTreeUI); // we need this to assign events on skill tree slots before we asssign events on skill scripts
        fadeScreen.gameObject.SetActive(true);
    }

    void Start()
    {
        SwitchTo(inGameUI);

        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);   
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.GetComponent<PlayerStats>().isDead)
        {
            
            if (Input.GetKeyDown(KeyCode.C))
                SwitchWithKeyTo(charcaterUI);

            if (Input.GetKeyDown(KeyCode.B))
                SwitchWithKeyTo(craftUI);    

            if (Input.GetKeyDown(KeyCode.K))
                SwitchWithKeyTo(skillTreeUI);

            if(Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.Escape))
                SwitchWithKeyTo(optionsUI);
        }

    }

    public void SwitchTo(GameObject _menu)
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null; // we need this to keep fade screen game object active


            if(fadeScreen == false)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_menu != null)
        {
            AudioManager.instance.PlaySFX(7, null);

            _menu.SetActive(true);

        }

        if (GameManager.instance != null)
        {
            if (_menu == inGameUI)
            {
                GameManager.instance.PauseGame(false);
                UnityEngine.Cursor.visible = false;
            }
            else
            {
                GameManager.instance.PauseGame(true);
                UnityEngine.Cursor.visible = true;
            }

        }

    }

    public void SwitchWithKeyTo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }

        SwitchTo(_menu);
    }

    private void CheckForInGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<UI_FadeScreen>() == null)
                return;
        }

        SwitchTo(inGameUI);
    }

    public void SwitchOnEndScreen()
    {
        fadeScreen.FadeOut();
        StartCoroutine(EndScreenCorutione());
    }

    IEnumerator EndScreenCorutione()
    {
        yield return new WaitForSeconds(1);
        endText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        restartButton.SetActive(true);
        mainMenuButton.SetActive(true);

    }

    public void RestartGameButton() => GameManager.instance.RestartScene();
    public void MainMenuButton() => GameManager.instance.MainMenu();

    public void LoadData(GameData _data)
    {
        foreach(KeyValuePair<string, float> pair in _data.volumeSettings)
        {
            foreach(UI_VolumeSlider item in volumeSettings)
            {
                if (item.parametre == pair.Key)
                {
                    item.LoadSlider(pair.Value);
                }

            }

        }

    }

    public void SaveData(ref GameData _data)
    {
        _data.volumeSettings.Clear();

        foreach(UI_VolumeSlider item in volumeSettings)
        {
            _data.volumeSettings.Add(item.parametre, item.slider.value);

        }

    }
}
