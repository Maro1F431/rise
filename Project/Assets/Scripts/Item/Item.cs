using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public string Name;
	public Sprite Image;
	public GameObject Prefab;
	public string FunctionToCall;
	public string FlavorText;

    [Header("Item Config")]
	
    public bool IsResource;
    public bool IsBuff;
    public bool IsWeapon;

    [Header("Weapon Stats")] 
	
    public int Damage;
    public float TimeBeforeShoot;
	public float BulletSpeed;
	public List<BulletPattern> BulletPatterns;
	public GameObject BulletPrefab;

    [Header("Buff Stats")] 
	
    public int HpBuff;
    public int DodgeCDR;
    public float speedBuff;
	public int armorBuff;
	public int damageBuff;

}
