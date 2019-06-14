using UnityEngine;
using UnityEngine.UI;

public class TxtValue : MonoBehaviour
{
    private Text valueTxt;

    void Awake()
    {
        valueTxt = GetComponent<Text>();

        if (valueTxt == null)
            Debug.LogError("No TextField Found");
        else
        {
            Slider parent = GetComponentInParent<Slider>();
            UpdateSliderValue(parent.value);
        }
    }
    
    public void UpdateSliderValue(float value)
    {
        valueTxt.text = value.ToString();
    }

}
