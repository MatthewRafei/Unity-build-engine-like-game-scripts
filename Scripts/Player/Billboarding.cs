using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboarding : MonoBehaviour
{
    public static Billboarding instance;

	private void Awake()
	{
		instance = this;
	}
}
