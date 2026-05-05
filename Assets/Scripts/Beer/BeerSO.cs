using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Beer")]
public class BeerData : ScriptableObject
{
    public BeerSize size;

    [Header("base bars this drink gives")]

    // look in TavernManager to see how this factors in, but basically, a roll is done from 1-100.
    // After rapport modifiers, 1-50 gives baseBars. 51-85 gives midBars. 86-100 gives highBars.
    public int baseBars;
    public int midBars;
    public int highBars;
    public float price;
}

public enum BeerSize { SMALL, MEDIUM, LARGE }
