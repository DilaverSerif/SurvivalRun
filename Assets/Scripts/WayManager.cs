using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Season
{
    Summer,
    Winter
}

public class WayManager : MonoBehaviour
{
    public static Action SpawnWay;
    public Season WaySeason = Season.Winter;
    protected List<GameObject> SpawnedWays = new List<GameObject>();
    private List<GameObject> SummerWays = new List<GameObject>();
    private List<GameObject> WinterWays = new List<GameObject>();
    public static WayManager Instance;
    public int ChangeSeasonTime = 10;
    public Material SkyBox;
    public Color SummerColor, SummerColor2, WinterColor, WinterColor2;
    public Color WinterFog, SummerFog;

    private void Awake()
    {
        Instance = this;
        SummerWays = Resources.LoadAll<GameObject>("Ways/Summer").ToList();
        WinterWays = Resources.LoadAll<GameObject>("Ways/Winter").ToList();
    }

    private Color netColor;

    private void Start()
    {
        netColor = WinterFog;
        RenderSettings.fogColor = netColor;
        SkyBox.DOColor(WinterColor, "_SkyColor", 0);
        SkyBox.DOColor(WinterColor2, "_EquatorColor", 0);

        for (int i = 0; i < 10; i++)
        {
            SpawnWayFunc();
        }
    }

    private int wayCount;

    private void SpawnWayFunc()
    {
        var pos = Vector3.zero;
        if (SpawnedWays.Count != 0)
            pos = SpawnedWays[0].transform.Find("FinishPoint").position;
        pos.y = 0;
        var spawnedWay = Instantiate(RandomGetWay(), pos, Quaternion.identity);
        spawnedWay.transform.SetParent(Base.GetLevelHolder());
        SpawnedWays.Insert(0, spawnedWay);

        if (SpawnedWays.Count > 17)
        {
            var last = SpawnedWays[SpawnedWays.Count - 1];
            SpawnedWays.Remove(last);
            Destroy(last);
        }

        if (!Base.IsPlaying()) return;
        wayCount++;

        if (wayCount % ChangeSeasonTime == 0)
        {
            if (WaySeason == Season.Summer)
            {
                WaySeason = Season.Winter;
            }
            else
            {
                WaySeason = Season.Summer;
            }
        }
    }


    private Coroutine ChangeToSky;
    private Season lastSeason = Season.Winter;

    public void ChangeSky(Season season)
    {
        if (lastSeason == season) return;
        lastSeason = season;
        if (season == Season.Winter)
        {
            if (ChangeToSky != null)
                StopCoroutine("ChangeToSky");
            ChangeToSky = StartCoroutine(InfectionA(WinterFog, 5));
            ParticleCamera.Instance.Snow.Clear();
            ParticleCamera.Instance.Snow.Play();
            SkyBox.DOColor(WinterColor, "_SkyColor", 5);
            SkyBox.DOColor(WinterColor2, "_EquatorColor", 5);
        }
        else
        {
            if (ChangeToSky != null)
                StopCoroutine("ChangeToSky");
            ChangeToSky = StartCoroutine(InfectionA(SummerFog, 5));
            ParticleCamera.Instance.Snow.Stop();
            SkyBox.DOColor(SummerColor, "_SkyColor", 5);
            SkyBox.DOColor(SummerColor2, "_EquatorColor", 5);
        }
    }

    private int lastWay;

    private GameObject RandomGetWay()
    {
        var way = Random.Range(0, SummerWays.Count);

        while (way == lastWay)
        {
            way = Random.Range(0, SummerWays.Count);
        }

        lastWay = way;
        if (WaySeason == Season.Summer)
            return SummerWays[way];

        return WinterWays[way];
    }

    public IEnumerator InfectionA(Color b, float time)
    {
        Debug.Log("Starting Infestation!");
        float ElapsedTime = 0.0f;
        float TotalTime = time;
        while (ElapsedTime < TotalTime)
        {
            ElapsedTime += Time.deltaTime;
            RenderSettings.fogColor = Color.Lerp(netColor, b, ElapsedTime / TotalTime);
            yield return new WaitForEndOfFrame();
        }

        RenderSettings.fogColor = b;
        yield return new WaitForEndOfFrame();
        Debug.Log("Ending Infestation!");
    }
    // private void SeasonChange()
    // {
    //     StartCoroutine(SeasonChanger());
    // }

    // IEnumerator SeasonChanger()
    // {
    //     int time = 0;
    //     while (Base.IsPlaying())
    //     {
    //         time++;
    //         yield return new WaitForSeconds(1);
    //
    //         if (time % ChangeSeasonTime == 0)
    //         {
    //             - if (WaySeason == Season.Summer)
    //             {
    //                 WaySeason = Season.Winter;
    //             }
    //             else WaySeason = Season.Summer;
    //         }
    //     }
    // }

    private void OnEnable()
    {
        // EventManager.FirstTouch += SeasonChange;
        SpawnWay += SpawnWayFunc;
    }

    private void OnDisable()
    {
        // EventManager.FirstTouch -= SeasonChange;
        SpawnWay -= SpawnWayFunc;
    }
}