using System.Collections.Generic;
using UnityEngine;

public class RapportManager : MonoBehaviour
{

    public static RapportManager Ins => _instance;
    private static RapportManager _instance;
    Dictionary<string, int> rapportMap = new Dictionary<string, int>();
    List<string> rapportCooldownList = new List<string>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogError($"Multiple instances of UIManager in scene, destroying component on {gameObject.name}");
            Destroy(this);
            return;
        }
        else
        {
            _instance = this;
        }

        DayManager.Ins.OnDayChanged += ClearRapportCooldown;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddRapport(string name, int amt)
    {
        if (!RapportEligible(name))
            return;

        if (rapportMap.ContainsKey(name))
        {
            rapportMap[name] += amt;
        }
        else
        {
            rapportMap.Add(name, amt);
        }

        rapportCooldownList.Add(name);
    }

    bool RapportEligible(string name)
    {
        return rapportCooldownList.Contains(name);
    }

    int GetRapportLevel(string name)
    {
        if (!rapportMap.ContainsKey(name))
        {
            return 0;
        }

        int rapport = rapportMap[name];

        if (rapport <= 2)
            return 1;

        if (rapport <= 5)
            return 2;

        if (rapport <= 10)
            return 3;

        if (rapport <= 20)
            return 4;

        if (rapport <= 50)
            return 5;

        return 0;
    }

    void ClearRapportCooldown()
    {
        rapportCooldownList = new List<string>();
    }
}
