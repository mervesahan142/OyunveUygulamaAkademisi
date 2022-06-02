using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillManagerandUI : MonoBehaviour
{
    int level = 1, levelMaxXp, levelXp = 0, skillPointValue = 0, skillRightValue = 0, attackLevel = 1, enduranceLevel = 1, moveSpeedLevel = 1, dodgeSpeedLevel = 1, attackRangeLevel = 1;
    Image xpImage, skillRightArea, skillMenu;
    Text levelText, skillRightText;
    Button pauseButton;
    Button[] attackLevelButtons = new Button[2];
    Button[] enduranceLevelButtons = new Button[2];
    Button[] moveSpeedLevelButtons = new Button[2];
    Button[] dodgeSpeedLevelButtons = new Button[2];
    Button[] attackRangeLevelButtons = new Button[2];

    void Awake() {
        //reset
        if(false){
            PlayerPrefs.SetInt("spokeWithFreeKnight_1", 0);
            PlayerPrefs.SetInt("spokeWithKnight", 0);
            PlayerPrefs.SetInt("spokeWithWarrior", 0);
            PlayerPrefs.SetInt("spokeWithKing", 0);

            PlayerPrefs.SetInt("level", 1);
            PlayerPrefs.SetInt("levelXp", 0);
            PlayerPrefs.SetInt("skillPointValue", 0);
            PlayerPrefs.SetInt("skillRightValue", 0);

            PlayerPrefs.SetInt("attackLevel", 1);
            PlayerPrefs.SetInt("enduranceLevel", 1);
            PlayerPrefs.SetInt("moveSpeedLevel", 1);
            PlayerPrefs.SetInt("dodgeSpeedLevel", 1);
            PlayerPrefs.SetInt("attackRangeLevel", 1);
        }

        pauseButton = GameObject.Find("PauseButton").GetComponent<Button>();
        //level
        level = PlayerPrefs.GetInt("level");
        levelXp = PlayerPrefs.GetInt("levelXp");
        skillPointValue = PlayerPrefs.GetInt("skillPointValue");
        levelMaxXp = level * 205;
        xpImage = GameObject.Find("XP").GetComponent<Image>();
        xpImage.rectTransform.sizeDelta = new Vector2(levelXp / level, 24.2194f);
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "" + level;

        //Skill Right
        skillRightValue = PlayerPrefs.GetInt("skillRightValue");
        skillRightArea = GameObject.Find("SkillRightArea").GetComponent<Image>();
        skillRightText = GameObject.Find("SkillRightText").GetComponent<Text>();
        if(skillRightValue != 0){
            skillRightText.text = "x" + skillRightValue;
        }else{
            skillRightArea.gameObject.SetActive(false);
        }

        //skill Menu
        attackLevel = PlayerPrefs.GetInt("attackLevel");
        enduranceLevel = PlayerPrefs.GetInt("enduranceLevel");
        moveSpeedLevel = PlayerPrefs.GetInt("moveSpeedLevel");
        dodgeSpeedLevel = PlayerPrefs.GetInt("dodgeSpeedLevel");
        attackRangeLevel = PlayerPrefs.GetInt("attackRangeLevel");

        for(int i=0; i<2; i++){
            attackLevelButtons[i] = GameObject.Find("AttackLevel_" + (i + 1) + "Button").GetComponent<Button>();
            enduranceLevelButtons[i] = GameObject.Find("EnduranceLevel_" + (i + 1) + "Button").GetComponent<Button>();
            moveSpeedLevelButtons[i] = GameObject.Find("MoveSpeedLevel_" + (i + 1) + "Button").GetComponent<Button>();
            dodgeSpeedLevelButtons[i] = GameObject.Find("DodgeSpeedLevel_" + (i + 1) + "Button").GetComponent<Button>();
            attackRangeLevelButtons[i] = GameObject.Find("AttackRangeLevel_" + (i + 1) + "Button").GetComponent<Button>();
        }
        skillMenu = GameObject.Find("SkillMenu").GetComponent<Image>();
        skillMenu.gameObject.SetActive(false);

        

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EarnXp(int enemyXp){
        levelXp = levelXp + enemyXp;
        Debug.Log(levelXp);
        PlayerPrefs.SetInt("levelXp", levelXp);
        xpImage.rectTransform.sizeDelta = new Vector2(levelXp / level, 24.2194f);
        if(levelMaxXp <= levelXp){
            LevelUp();
        }
    }

    void LevelUp(){
        //Level
        level = level + 1;
        levelText.text = "" + level;
        PlayerPrefs.SetInt("level", level);
        levelXp = levelXp - levelMaxXp;
        PlayerPrefs.SetInt("levelXp", levelXp);
        xpImage.rectTransform.sizeDelta = new Vector2(levelXp / level, 24.2194f);
        levelMaxXp = level * 205;
        skillPointValue = skillPointValue + 1;
        PlayerPrefs.SetInt("skillPointValue", skillPointValue);

        //Skill Right
        skillRightValue = skillRightValue + 1;
        PlayerPrefs.SetInt("skillRightValue", skillRightValue);
        skillRightArea.gameObject.SetActive(true);
        skillRightText.text = "x" + skillRightValue;
    }

    public void OpenSkillMenu(){
        Time.timeScale = 0;
        pauseButton.gameObject.SetActive(false);
        skillMenu.gameObject.SetActive(true);
        PrepareSkillMenu();
    }

    public void PrepareSkillMenu(){
        for(int i=0; i<2; i++){
            attackLevelButtons[i].interactable = false;
            enduranceLevelButtons[i].interactable = false;
            moveSpeedLevelButtons[i].interactable = false;
            dodgeSpeedLevelButtons[i].interactable = false;
            attackRangeLevelButtons[i].interactable = false;
            if(skillRightValue != 0){
                if(attackLevel - 1 == i){
                    attackLevelButtons[i].interactable = true;
                }
                if(enduranceLevel - 1 == i){
                    enduranceLevelButtons[i].interactable = true;
                }
                if(moveSpeedLevel - 1 == i){
                    moveSpeedLevelButtons[i].interactable = true;
                }
                if(dodgeSpeedLevel - 1 == i){
                    dodgeSpeedLevelButtons[i].interactable = true;
                }
                if(attackRangeLevel -1 == i){
                    attackRangeLevelButtons[i].interactable = true;
                }
            }
            if(attackLevel - 1 > i){
                attackLevelButtons[i].gameObject.GetComponent<Image>().color = Color.black;
            }
            if(enduranceLevel - 1 > i){
                enduranceLevelButtons[i].gameObject.GetComponent<Image>().color = Color.black;
            }
            if(moveSpeedLevel - 1 > i){
                moveSpeedLevelButtons[i].gameObject.GetComponent<Image>().color = Color.black;
            }
            if(dodgeSpeedLevel - 1 > i){
                dodgeSpeedLevelButtons[i].gameObject.GetComponent<Image>().color = Color.black;
            }
            if(attackRangeLevel - 1 > i){
                attackRangeLevelButtons[i].gameObject.GetComponent<Image>().color = Color.black;
            }
        }
    }

    public void CloseSkillMenu(){
        Time.timeScale = 1;
        pauseButton.gameObject.SetActive(true);
        skillMenu.gameObject.SetActive(false);
    }

    public void UpdateSkill(string buttonName){
        for(int i=0; i<2; i++){
            if(attackLevelButtons[i].gameObject.name == buttonName){
                attackLevel = attackLevel + 1;
                PlayerPrefs.SetInt("attackLevel", attackLevel);
            }else if(enduranceLevelButtons[i].gameObject.name == buttonName){
                enduranceLevel = enduranceLevel + 1;
                PlayerPrefs.SetInt("enduranceLevel", enduranceLevel);
            }else if(moveSpeedLevelButtons[i].gameObject.name == buttonName){
                moveSpeedLevel = moveSpeedLevel + 1;
                PlayerPrefs.SetInt("moveSpeedLevel", moveSpeedLevel);
            }else if(dodgeSpeedLevelButtons[i].gameObject.name == buttonName){
                dodgeSpeedLevel = dodgeSpeedLevel + 1;
                PlayerPrefs.SetInt("dodgeSpeedLevel", dodgeSpeedLevel);
            }else if(attackRangeLevelButtons[i].gameObject.name == buttonName){
                attackRangeLevel = attackRangeLevel + 1;
                PlayerPrefs.SetInt("attackRangeLevel", attackRangeLevel);
            }
        }
        //Skill Right
        skillRightValue = skillRightValue - 1;
        PlayerPrefs.SetInt("skillRightValue", skillRightValue);
        if(skillRightValue <= 0){
            skillRightArea.gameObject.SetActive(false);
        }
        skillRightText.text = "x" + skillRightValue;
        PrepareSkillMenu();
        GameObject.Find("Hero").GetComponent<MainCharacterController>().UpdateSkills();
    }

    public void PlayPause(){
        if(Time.timeScale == 0){
            Time.timeScale = 1;
        }else{
            Time.timeScale = 0;
        }
    }
}
