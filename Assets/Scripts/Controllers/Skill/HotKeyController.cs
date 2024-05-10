using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class HotKeyController : MonoBehaviour
{
    private TextMeshProUGUI text;
    private KeyCode hotKey;
    private GameObject blackHole;
    private Transform enemy;
    
    public void Setup(KeyCode _hotKey, GameObject _blackHole, Transform _enemy)
    {
        blackHole = _blackHole;
        enemy = _enemy;
        
        text = GetComponentInChildren<TextMeshProUGUI>();
        hotKey = _hotKey;
        
        text.text = hotKey.ToString();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(hotKey))
        {
            blackHole.GetComponent<BlackHoleController>().AddEnemy(enemy);
            gameObject.SetActive(false);
        }
    }
}
