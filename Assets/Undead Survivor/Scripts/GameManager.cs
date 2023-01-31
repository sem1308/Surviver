using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float gameTime;
    public float maxGameTime;
    public int[] maxExp;
    public int level = 0;
    public int maxLevel; // 플레이어가 가질 수 있는 최대 레벨
    public int maxWeaponCnt; // 플레이어가 가질 수 있는 최대 무기 개수

    public Player player;
    public PoolManager pool;
    public Weapon[] weapons;
    public WeaponData[] weaponDatas;
    public Sprite[] weaponSelectSprites;

    public Image imageExp;
    public Image imageHP;
    public GameObject HPBox;
    public TextMeshProUGUI textTime;
    public TextMeshProUGUI textLV;
    public Image imageClear;
    public Image imageDie;

    public GameObject skillSet;
    public Image[] skillimgs;
    Skill[] skills;
    TextMeshProUGUI[][] skillTexts;
    Image[] skillSelectImgs;

    bool[] hasWeapon;
    bool isMenuOpen;
    bool isCharSelected;

    // UI
    public GameObject UIDead;
    public GameObject UIWin;
    public GameObject UIChar;
    public GameObject UIEsc;
    public GameObject UISetting;

    // 오디오
    public AudioClip AudioSelect;
    public AudioClip AudioLevelUp;
    public AudioClip AudioDead;
    public AudioClip AudioWin;
    AudioSource audioSource;

    // 시간 
    int min;
    int sec;

    /* 초기화 */
    private void Awake()
    {
        instance = this;
        maxLevel = maxExp.Length;
        maxWeaponCnt = 6;

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        InitPlayer();
        InitWeapon();

        hasWeapon = new bool[weapons.Length];

        skills = skillSet.GetComponentsInChildren<Skill>();
        skillTexts = new TextMeshProUGUI[skills.Length][];
        skillSelectImgs = new Image[skills.Length];
        for (int i = 0; i < skills.Length; i++)
        {
            Skill skill = skills[i];
            skillTexts[i] = skill.GetComponentsInChildren<TextMeshProUGUI>();
            skillSelectImgs[i] = skill.GetComponentsInChildren<Image>()[1];
        }
        Pause();
    }

    private void InitPlayer()
    {
        player.maxHealth = 100;
        player.health = player.maxHealth;
        player.speed = 5f;
        player.curExp = 0f;
        player.maxExp = maxExp[0];
        player.curWeaponCnt = 0;

        textLV.text = player.level.ToString();
    }

    private void InitWeapon()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].Init(weaponDatas[i]);
        }
    }

    /* 프레임 업데이트 */
    private void Update()
    {
        gameTime += Time.deltaTime;
        min = (int)(gameTime / 60);
        sec = (int)(gameTime - 60f * min);
        textTime.text = $"{min:D2}:{sec:D2}";
        if (gameTime > maxGameTime)
        {
            Invoke("Win", 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Esc();
        }
    }

    /* 기타 API */
    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }

    public void ShowSkills()
    {
        // 오디오 효과
        PlayAudio(AudioLevelUp);

        skillSet.SetActive(true);
        Pause();

        // 랜덤 스킬 뽑기
        List<int> idxs = new List<int>();

        int currentNumber;

        int cnt = 0;
        do
        {
            currentNumber = UnityEngine.Random.Range(0, weapons.Length);

            if (!hasWeapon[currentNumber] && idxs.Contains(currentNumber))
                continue;

            idxs.Add(currentNumber);
            skills[cnt].weapon = weapons[currentNumber];
            skills[cnt].property = RandomEnum<Property>();
            skills[cnt].amount = UnityEngine.Random.Range(Skill.amountRange[(int)skills[cnt].property].min, Skill.amountRange[(int)skills[cnt].property].max);
            cnt++;
        } while (cnt < skills.Length);

        // 스킬 표시
        for (int i = 0; i < skills.Length; i++)
        {
            Weapon weapon = skills[i].weapon;
            skillTexts[i][0].text = weapon.weaponName;
            skillTexts[i][1].text = hasWeapon[weapon.weaponIdx] ? skills[i].explainToString() : weapon.explainment;
            skillSelectImgs[i].sprite = weaponSelectSprites[idxs[i]];
        }
    }

    public void SelectSkill(int which)
    {
        // 오디오 효과
        PlayAudio(AudioSelect);

        // 스킬 선택
        Skill skill = skills[which];
        if (!hasWeapon[skill.weapon.weaponIdx])
        {
            AddWeapon(skill.weapon);
            hasWeapon[skill.weapon.weaponIdx] = true;
        }
        else
        {
            skill.SkillUp();
        }

        skillSet.SetActive(false);

        Resume();
    }

    public void SelectChar(Weapon weapon)
    {
        // 오디오 효과
        PlayAudio(AudioSelect);

        // 캐릭터의 초기 무기 설정
        hasWeapon[weapon.weaponIdx] = true;
        player.SetAnimCont(weapon.initCharIdx);
        AddWeapon(weapon);

        UIChar.SetActive(false);
        isCharSelected = true;
        Resume();
    }

    public void AddWeapon(Weapon weapon)
    {
        player.weapons.Add(weapon);
        skillimgs[player.curWeaponCnt++].sprite = weapon.gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    public static T RandomEnum<T>()
    {
        Array values = Enum.GetValues(typeof(T));

        return (T)values.GetValue(new System.Random().Next(0, values.Length));
    }

    public void PlayAudio(AudioClip clip, float vol = 0.8f, float pitch = 1f)
    {
        audioSource.clip = clip;
        audioSource.volume = vol;
        audioSource.pitch = pitch;
        audioSource.Play();
    }

    public void GoHome()
    {
        PlayAudio(AudioSelect);
        SceneManager.LoadScene("HomeScene");
    }

    public void GameStart()
    {
        PlayAudio(AudioSelect);
        SceneManager.LoadScene("GameScene");
    }

    public void GameOver()
    {
        Pause();
        PlayAudio(AudioDead);
        UIDead.SetActive(true);
    }

    public void Win()
    {
        Pause();
        PlayAudio(AudioWin);
        UIWin.SetActive(true);
    }

    public void Esc()
    {
        PlayAudio(AudioSelect);
        if (!isCharSelected) return;
        if (isMenuOpen)
        {
            Resume();
            isMenuOpen = false;
        }
        else
        {
            Pause();
            isMenuOpen = true;
        }
        UIEsc.SetActive(isMenuOpen);
    }

    public void OCSetting(bool isOpen)
    {
        PlayAudio(AudioSelect);
        UISetting.SetActive(isOpen);
    }
}

[System.Serializable]
public class WeaponData
{
    public string name; // 이름
    public int weaponIdx;
    public int initCharIdx;
    public string explainment; // 설명
    public float coolTime; // 쿨타임
    public int projectileCnt; // 발사체 개수
    public float damage; // 대미지
    public float range; // 범위
    public BulletData bulletData;
}

