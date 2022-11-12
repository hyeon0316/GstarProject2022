using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HpbarController : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI Name;

    private Camera _cam;
    private void Awake()
    {
        _cam = Camera.main;
        gameObject.GetComponent<Canvas>().worldCamera = _cam;
    }

    public void SetEnemyUI(int maxHp, string name)
    {
        slider.maxValue = maxHp;
        slider.value = slider.maxValue;
        Name.text = name;
    }

    public void UpdateHpBar(int amount)
    {
        slider.value = amount;
    }

    public void ShowHpBar()
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
    }

    public void CloseHpBar()
    {
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }
    
    public void SetNpc(string name)
    {
        Name.text = name;
    }
    
    private void Update()
    {
        if (_cam == null)
        {
            _cam = Camera.main;
            gameObject.GetComponent<Canvas>().worldCamera = _cam;
        }
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, _cam.transform.rotation.eulerAngles.y, transform.rotation.z));
    }
}
