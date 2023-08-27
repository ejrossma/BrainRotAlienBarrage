using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : MonoBehaviour
{
    //Object Pools
    private List<GameObject> walkerPool = new List<GameObject>();
    private List<GameObject> turretPool = new List<GameObject>();
    private List<GameObject> jumperPool = new List<GameObject>();
    private List<GameObject> projectilePool = new List<GameObject>();
    private List<GameObject> physicsProjectilePool = new List<GameObject>();

    [Header("Objects To Pool")]
    [SerializeField] GameObject walker;
    [SerializeField] float walkerAmountToPool;
    [SerializeField] GameObject turret;
    [SerializeField] float turretAmountToPool;
    [SerializeField] GameObject jumper;
    [SerializeField] float jumperAmountToPool;
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileAmountToPool;
    [SerializeField] GameObject physicsProjectile;
    [SerializeField] float physicsProjectileAmountToPool;

    [SerializeField] Transform poolContainer;

    private void Awake()
    {
        GameObject tmp;

        //Create walkers
        for (int i = 0; i < walkerAmountToPool; i++)
        {
            tmp = Instantiate(walker);
            tmp.transform.parent = poolContainer;
            tmp.SetActive(false);
            walkerPool.Add(tmp);
        }

        //Create turrets
        for (int i = 0; i < turretAmountToPool; i++)
        {
            tmp = Instantiate(turret);
            tmp.transform.parent = poolContainer;
            tmp.SetActive(false);
            turretPool.Add(tmp);
        }

        //Create jumpers
        for (int i = 0; i < jumperAmountToPool; i++)
        {
            tmp = Instantiate(jumper);
            tmp.transform.parent = poolContainer;
            tmp.SetActive(false);
            jumperPool.Add(tmp);
        }

        //Create projectile
        for (int i = 0; i < projectileAmountToPool; i++)
        {
            tmp = Instantiate(projectile);
            tmp.transform.parent = poolContainer;
            tmp.SetActive(false);
            projectilePool.Add(tmp);
        }

        //Create physics projectile
        for (int i = 0; i < physicsProjectileAmountToPool; i++)
        {
            tmp = Instantiate(physicsProjectile);
            tmp.transform.parent = poolContainer;
            tmp.SetActive(false);
            physicsProjectilePool.Add(tmp);
        }
    }

    public GameObject GetPooledWalker()
    {
        for (int i = 0; i < walkerAmountToPool; i++)
        {
            if (!walkerPool[i].activeInHierarchy)
                return walkerPool[i];
        }
        return null;
    }

    public GameObject GetPooledTurret()
    {
        for (int i = 0; i < turretAmountToPool; i++)
        {
            if (!turretPool[i].activeInHierarchy)
                return turretPool[i];
        }
        return null;
    }

    public GameObject GetPooledJumper()
    {
        for (int i = 0; i < jumperAmountToPool; i++)
        {
            if (!jumperPool[i].activeInHierarchy)
                return jumperPool[i];
        }
        return null;
    }

    public GameObject GetPooledProjectile()
    {
        for (int i = 0; i < projectileAmountToPool; i++)
        {
            if (!projectilePool[i].activeInHierarchy)
                return projectilePool[i];
        }
        return null;
    }

    public GameObject GetPooledPhysicsProjectile()
    {
        for (int i = 0; i < physicsProjectileAmountToPool; i++)
        {
            if (!physicsProjectilePool[i].activeInHierarchy)
                return physicsProjectilePool[i];
        }
        return null;
    }
}
