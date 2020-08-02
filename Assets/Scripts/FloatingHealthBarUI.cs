using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private Transform cam;
    
    void Start()
    {
        cam = GameManager.instance.CameraTransform;
    }
    
    void Update()
    {
        transform.LookAt(cam);
    }
    
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }
}