using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Text lastShotText;
    [SerializeField] private Text currentShotText;
    void Start()
    {
        currentShotText.text = "";
        lastShotText.text = "";
    }

    public void UpdateCurrentShot(Vector2 angles, int power)
    {
        currentShotText.text = GetShotText(angles, power);
    }
    
    public void UpdateLastShot(Vector2 angles, int power)
    {
        string text = GetShotText(angles, power);
        lastShotText.text = $"Last shot\n {text}";
    }

    public void HideCurrentShot()
    {
        currentShotText.text = "";
    }

    private string GetShotText(Vector2 angles, int power)
    {
        return $"X: {(int)angles.x}\nY: {(int) angles.y}\nPower {power}%";
    }
}
