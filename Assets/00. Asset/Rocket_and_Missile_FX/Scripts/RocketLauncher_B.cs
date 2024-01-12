using UnityEngine;
using System.Collections;

public class RocketLauncher_B : MonoBehaviour
{
    public bool isSet = false;

    public GameObject engineFlames;
    public GameObject LauncherMesh;
    public GameObject LauncherAnim;
    public GameObject SmokeParticles;
    public GameObject Explosion;
    public GameObject ExplosionAudio;

    //void Start()
    //{
    //    LauncherMesh.SetActive(true);
    //    engineFlames.SetActive(false);
    //    SmokeParticles.SetActive(false);
    //    Explosion.SetActive(false);
    //    ExplosionAudio.SetActive(false);

    //    isSet = false;
    //    StartCoroutine("LaunchRocket");
    //}

    private void OnEnable()
    {
        LauncherMesh.SetActive(true);
        engineFlames.SetActive(false);
        SmokeParticles.SetActive(false);
        Explosion.SetActive(false);
        ExplosionAudio.SetActive(false);

        isSet = false;
        StartCoroutine("LaunchRocket");
    }

    //    void Update (){

    //	if (Input.GetButtonDown("Fire1"))
    //    {
    //			StartCoroutine ("LaunchRocket");
    //    }

    //}

    IEnumerator LaunchRocket()
    {

        engineFlames.SetActive(true);
        SmokeParticles.SetActive(true);

        yield return new WaitForSeconds(0.5f);



        LauncherAnim.GetComponent<Animation>().Play();
        isSet = true;


        yield return new WaitForSeconds(4.2f);
        
        Explosion.SetActive(true);
        ExplosionAudio.SetActive(true);
        engineFlames.SetActive(false);
        LauncherMesh.SetActive(false);

        yield return new WaitForSeconds(2.0f);

        isSet = false;
        gameObject.SetActive(false);
    }
}