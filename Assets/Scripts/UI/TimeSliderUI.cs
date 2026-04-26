using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeSliderUI : MonoBehaviour
{

    //frontTimeSlider is the foreground slider-- the one that will flash visually, exposing the background slider,
    //when the player is previewing an action that will, in the end, cost time units. It will reduce its size when
    //the player takes the action, again exposing the background slider, but the background slider only 'catches up'
    //when those units are actually consumed.
    [SerializeField] private Slider frontTimeSlider;
    [SerializeField] private TMP_Text frontUnitsText;

    [SerializeField] private Slider backTimeSlider;
    [SerializeField] private TMP_Text backUnitsText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
