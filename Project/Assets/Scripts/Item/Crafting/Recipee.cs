using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class Recipee : ScriptableObject
{
	public RecipeeItem[] itemList;

	public Item result;
}
