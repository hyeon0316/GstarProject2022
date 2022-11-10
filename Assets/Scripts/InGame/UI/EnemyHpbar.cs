using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EnemyHpbar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI Name;
    private Camera _cam;
    private void Awake()
    {
        _cam = Camera.main;
        gameObject.GetComponent<Canvas>().worldCamera = _cam;
    }

    public void SetEnemyUI(int _maxHp, int _hp, string _name)
    {
        slider.value = _maxHp / _hp;
        Name.text = _name;
    }
    public void SetNpc(string _name)
    {
        Name.text = _name;
    }
    private void Update()
    {
        if (_cam == null)
        {
            _cam = Camera.main;
            gameObject.GetComponent<Canvas>().worldCamera = _cam;
        }
        transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
    }
}
