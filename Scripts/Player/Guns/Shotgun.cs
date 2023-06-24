using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Feature idea : Adjust gun image color to give the illusion the gun is effected by shadows when standing in dark areas of the map
// You could also adjust it incrimentally to give the illusion the gun is getting brighter or darker

// BUG : If weapons are switched during animation then event is never reached and the game still thinks the animation is playing.
// Need to prevent switching until after shooting animations are finished

public class Shotgun : MonoBehaviour
{
	[SerializeField]
	private sbyte ammo;

	[SerializeField]
	private AudioSource fireSoundEffect;

	[SerializeField]
	private AudioSource shellFallingSoundEffect;

	[SerializeField]
	private AudioSource gunEmptySoundEffect;


	private Animator gunAnimation;


	private bool isFiring = false;

	private void Start()
	{
		gunAnimation = GetComponent<Animator>();
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && !isFiring && ammo > 0)
		{
			isFiring = true;
			ammo -= 1;
			gunAnimation.SetTrigger("Shoot");
			fireSoundEffect.Play();
			shellFallingSoundEffect.Play();
		}
		else if (Input.GetMouseButtonDown(0) && ammo <= 0)
		{
			gunEmptySoundEffect.Play();
		}
	}

	public bool ShotgunFireAnimationFinished()
	{
		isFiring = false;
		return true;
	}

}
