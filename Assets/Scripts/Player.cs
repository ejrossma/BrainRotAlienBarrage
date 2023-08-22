using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] float maxHealth;
    [SerializeField] float health;

    [Header("Player Information")]
    [SerializeField] GameObject currentGun;

    [Header("Gun References")]
    [SerializeField] GameObject assaultObj;
    [SerializeField] GameObject shotgunObj;

    private GunSystem assault;
    private GunSystem shotgun;

    // Start is called before the first frame update
    void Awake()
    {
        assault = assaultObj.GetComponent<GunSystem>();
        shotgun = shotgunObj.GetComponent<GunSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
