using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using DG.Tweening;

public class PlayTimeMenu : BaseMenu
{
    public Button PauseButton,ReplayButton;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI GoldText;
    public Image GoldImage;
    public GameObject JoyStick;

    private void Start()
    {
        PauseButton.onClick.AddListener(PauseButtonFunc);
        ReplayButton.onClick.AddListener(ReplayButtonFunc);
        JoyStick.gameObject.SetActive(false);

        GoldShowerText.GetComponent<RectTransform>().anchoredPosition = new Vector2(150, 0);
    }

    private void PauseButtonFunc()
    {
        EventManager.OnPause?.Invoke(true);
    }

    private void OnEnable()
    {
        DataManager.OnSetData += SetDatas;
        DataManager.WhenAddCoin += ShowerGold;
    }

    private void OnDisable()
    {
        DataManager.WhenAddCoin -= ShowerGold;
        DataManager.OnSetData -= SetDatas;
    }

    private void SetDatas(int level, int gold)
    {
        LevelText.text = "LEVEL " + level;
        GoldText.text = gold.ToString();
    }

    public void JoyStickOption(bool o)
    {
        JoyStick.SetActive(o);
    }

    public void ReplayButtonFunc()
    {
        EventManager.RestartLevel?.Invoke();
    }

    private Coroutine goldCoroutine;
    private int gold;
    private int goldTimer;
    public TextMeshProUGUI GoldShowerText;
    private void ShowerGold()
    {
        if (goldCoroutine != null)
        {
            gold++;
            goldTimer = 2;
        }
        else
        {
            gold = 0;
            goldCoroutine = StartCoroutine(GoldShower());
        }
        
        GoldShowerText.text ="+" + gold.ToString();

    }

    IEnumerator GoldShower()
    {
        DOTween.Kill("goldText");
        GoldShowerText.DOFade(1, 0.5f).SetId("goldText");
        goldTimer = 2;
        while (goldTimer > 0)
        {
            yield return new WaitForSeconds(1);
            goldTimer--;
        }
        GoldShowerText.DOFade(0, 0.5f).SetId("goldText");
        goldCoroutine = null;
    }
    
    
}
