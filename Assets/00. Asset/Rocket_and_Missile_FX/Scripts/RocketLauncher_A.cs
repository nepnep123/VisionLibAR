using UnityEngine;
using System.Collections;

public class RocketLauncher_A : MonoBehaviour {


public GameObject engineFlames;
public GameObject LauncherMesh;
public GameObject LauncherAnim;
public GameObject SmokeParticles;


void Start (){

	 engineFlames.SetActive(false);
	 SmokeParticles.SetActive(false);

}

void Update (){

	if (Input.GetButtonDown("Fire1"))
    {

		StartCoroutine ("LaunchRocket");


    }

}

IEnumerator LaunchRocket (){

    engineFlames.SetActive(true);
    SmokeParticles.SetActive(true);

	yield return new WaitForSeconds (0.5f);



	LauncherAnim.GetComponent<Animation>().Play();







}
}