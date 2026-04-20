using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Beer")]
public class BeerData : ScriptableObject
{
    public BeerSize size;
    public float baseLuck;
    public float price;
}

public enum BeerSize { SMALL, MEDIUM, LARGE }
