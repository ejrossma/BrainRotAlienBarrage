using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperArmorUICooldown : MonoBehaviour
{
    [SerializeField] SuperArmor superArmor;
    [SerializeField] RectTransform progressBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float updatedProgressValue = 1 - superArmor.GetSuperArmorCooldown() / superArmor.superArmorCooldown;
        //if (updatedProgressValue > 0.99) progressBar.gameObject.SetActive(false);
        //else progressBar.gameObject.SetActive(true);

        progressBar.localScale = new Vector3(updatedProgressValue * 10, 1, 1);
    }
}
