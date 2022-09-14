using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : BaseMenu
{
    private int armorLvl = 1, hungryLvl = 1;

    public Button ArmorButton;
    public Button WeaponButton;
    public Button HungryButton;
    public TextMeshProUGUI armor, weapon, hungry;
    public TextMeshProUGUI armorGold, weaponGold, hungryGold;
    public Color StandarCButton;
    private void Start()
    {
        armor = ArmorButton.transform.parent.GetComponentInChildren<TextMeshProUGUI>();
        weapon = WeaponButton.transform.parent.GetComponentInChildren<TextMeshProUGUI>();
        hungry = HungryButton.transform.parent.GetComponentInChildren<TextMeshProUGUI>();


        weaponGold.text = 10 + "<sprite=0>";
        hungryGold.text = 10 + "<sprite=0>";
        armorGold.text = 10 + "<sprite=0>";
        
        transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -1000);
    }

    private void OnEnable()
    {
        EventManager.FirstTouch += Show;
        EventManager.FinishGame += FinishGame;
        
        ArmorButton.onClick.AddListener(Armor);
        HungryButton.onClick.AddListener(Hungry);
        WeaponButton.onClick.AddListener(Weapon);
    }

    private void FinishGame(GameStat obj)
    {
        transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -1000);
        DOTween.Kill("Armor");
        DOTween.Kill("Hungry");
    }

    private void OnDisable()
    {
        EventManager.FirstTouch -= Show;
        EventManager.FinishGame -= FinishGame;
    }

    private bool titremeArmor, titremeHungry;

    private void Update()
    {
        if (!Base.IsPlaying()) return;
        ArmorButton.image.fillAmount =
            Mathf.Lerp(ArmorButton.image.fillAmount, Player.Instance.Armor / 100f, 5 * Time.deltaTime);

        if (!titremeArmor & Player.Instance.Armor < 30)
        {
            titremeArmor = true;
            ArmorButton.transform.parent.GetComponent<Image>().DOColor(Color.red, 0.25f).SetLoops(-1,LoopType.Yoyo).SetId("Armor");
            ArmorButton.transform.parent.DOShakeScale(0.5f, 0.05f, 10, 90, false).SetLoops(-1,LoopType.Yoyo).SetId("Armor");
        }else if (titremeArmor & Player.Instance.Armor > 30)
        {
            titremeArmor = false;

        }
        
        if (!titremeHungry & Player.Instance.Hungry < 30)
        {
            titremeHungry = true;
            HungryButton.transform.parent.GetComponent<Image>().DOColor(Color.red, 0.25f).SetLoops(-1,LoopType.Yoyo).SetId("Hungry");
            HungryButton.transform.parent.transform.DOShakeScale(0.5f, 0.05f, 10, 90, false).SetLoops(-1,LoopType.Yoyo).SetId("Hungry");
        }else if (titremeHungry & Player.Instance.Hungry > 30)
        {
            titremeHungry = false;

        }
        
        
        HungryButton.image.fillAmount = Mathf.Lerp(HungryButton.image.fillAmount, Player.Instance.Hungry / 100f,
            5 * Time.deltaTime);

        armor.text = "TEMP \n <color=\"green\">%" + (int)(Player.Instance.Armor);
        hungry.text = "HUNGRY \n <color=\"green\">%" + (int)(Player.Instance.Hungry);
        weapon.text = "WEAPON \n <color=\"green\">Lv." + BasicInventory.Instance.Level;
    }

    private void Armor()
    {
        var cost = armorLvl * 10;
        if (cost <= Datas.Coin.GetData())
        {
            DOTween.Kill("Armor");

            ArmorButton.transform.parent.GetComponent<Image>().
                DOColor(Color.white, 0.25f);
            ArmorButton.transform.transform.localScale = Vector3.one;
            // ArmorButton.transform.DOKill();
            // ArmorButton.GetComponent<Image>().DOColor(StandarCButton, 0.25f);
            Datas.Coin.CoinAdd(-cost);
            DOTween.To(()=>Player.Instance.Armor,
                x => Player.Instance.Armor = x, 
                100, 1f);
            Player.Instance.CheckArmorPls();
            armorLvl++;
            armorGold.text = armorLvl * 10 + "<sprite=0>";
        }
    }

    private void Hungry()
    {
        var cost = hungryLvl * 10;
        if (cost <= Datas.Coin.GetData())
        {            DOTween.Kill("Hungry");

            HungryButton.transform.parent.GetComponent<Image>().
                DOColor(Color.white, 0.25f);
            HungryButton.transform.transform.localScale = Vector3.one;
            // HungryButton.GetComponent<Image>().DOColor(StandarCButton, 0.25f);
            // HungryButton.transform.localScale = Vector3.one;
            Datas.Coin.CoinAdd(-cost);
            DOTween.To(()=>Player.Instance.Hungry,
                x => Player.Instance.Hungry = x, 
                100, 1f);
            hungryLvl++;
            hungryGold.text = hungryLvl * 10 + "<sprite=0>";
        }
    }

    private void Weapon()
    {
        var gold = BasicInventory.Instance.Level * 10;
        if (gold <= Datas.Coin.GetData())
        {
            Datas.Coin.CoinAdd(-gold);
            BasicInventory.Instance.LevelUp();
            weaponGold.text = gold + "<sprite=0>";
        }
    }
}