using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Player : Character
{
    private PlayerData playerData;
    public float Hungry = 100;
    public float Armor = 100;
    public GameObject armor, armor2;
    public static Player Instance;
    public SkinnedMeshRenderer ices;
    public Material playerMat;

    public float HungerSpeed;
    public float ArmorSpeed;
    public SkinnedMeshRenderer skin;
    public Color ColdColor, NormalSkin;
    public float BaseSpeed;
    public Rigidbody rb;
    public Rigidbody[] Ragdoll;
    public Collider[] RagdollCollider;

    protected override void Awake()
    {
        foreach (var rag in Ragdoll)
        {
            rag.isKinematic = true;
        }

        foreach (var b in RagdollCollider)
        {
            b.enabled = false;
        }

        base.Awake();
        Instance = this;
        playerData = PlayerData.Current;
    }

    protected override void Start()
    {
        base.Start();
        Cameras.cam1.CameraSetFull(transform);
        Cameras.cam1.SetOffset(new Vector3(0, 10, -20));
        ColorStart();
    }

    private void OnEnable()
    {
        EventManager.FirstTouch += FirstTouch;
    }

    private void OnDisable()
    {
        EventManager.FirstTouch -= FirstTouch;
    }

    void FirstTouch()
    {
        StartCoroutine(Azalt());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Way"))
        {
            WayManager.SpawnWay.Invoke();
    
            if (other.gameObject.name.Substring(0, 1) == "W")
            {
                    WayManager.Instance.ChangeSky(Season.Winter);
            }
            else
            {
                    WayManager.Instance.ChangeSky(Season.Summer);
            }
        }
    }

    IEnumerator Azalt()
    {
        while (Base.IsPlaying())
        {
            if (Hungry < 0 | Armor < 0)
            {
                Base.FinisGame(GameStat.Lose, 1f);
                Armor = 0;
                Hungry = 0;

                GetComponentInChildren<Animator>().enabled = false;
                rb.isKinematic = true;

                foreach (var rg in Ragdoll)
                {
                    rg.isKinematic = false;
                }

                foreach (var b in RagdollCollider)
                {
                    b.enabled = true;
                }
            }

            skin.material.color = gradient.Evaluate(1 - Armor * 0.01f);

            if (WayManager.Instance.WaySeason == Season.Winter)
            {
                Armor -= Time.fixedDeltaTime * ArmorSpeed;
                Hungry -= Time.fixedDeltaTime;
            }
            else
            {
                Armor -= Time.fixedDeltaTime;
                Hungry -= Time.fixedDeltaTime * HungerSpeed;
            }

            playerData.MoveSpeed = BaseSpeed + Hungry * 0.1f;

            CheckArmorPls();

            for (int i = 0; i < 3; i++)
            {
                var a = (100 - Armor) * 0.45f;
                ices.SetBlendShapeWeight(i, a);
            }

            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForEndOfFrame();
    }

    public void CheckArmorPls()
    {
        if (Armor < 50)
        {
            armor.SetActive(true);
            armor2.SetActive(false);
        }
        else
        {
            armor.SetActive(false);
            armor2.SetActive(true);
        }

        for (int i = 0; i < 3; i++)
        {
            ices.SetBlendShapeWeight(i, 0);
        }
    }


    Gradient gradient;
    GradientColorKey[] colorKey;
    GradientAlphaKey[] alphaKey;

    void ColorStart()
    {
        gradient = new Gradient();

        // Populate the color keys at the relative time 0 and 1 (0 and 100%)
        colorKey = new GradientColorKey[2];
        colorKey[0].color = NormalSkin;
        colorKey[0].time = 0.0f;
        colorKey[1].color = ColdColor;
        colorKey[1].time = 1.0f;

        // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
        alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 0.0f;
        alphaKey[1].time = 1.0f;

        gradient.SetKeys(colorKey, alphaKey);

        // What's the color at the relative time 0.25 (25 %) ?
        // Debug.Log(gradient.Evaluate(0.25f));
    }
}