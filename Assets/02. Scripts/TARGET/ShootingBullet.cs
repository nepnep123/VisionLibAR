using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBullet : MonoBehaviour
{
    [Header(" [ SHOOT RESOURCE ] ")]
    public Transform firePos; //포탄 발사 위치

    public GameObject fire_Effect; //발사했을 때 포구 이펙트
    public GameObject bullet; //발사할 포탄
    //public GameObject bulletArrow_Effect;

    public TargetSetting target; 

    [Header(" [ VALUE ] ")]
    private float tx;
    private float ty;
    private float tz;
    private float v;
    public float g = 9.8f;
    public float max_height = 10.0f;

    private float elapsed_time;
    private float t;
    private float dat;  //도착점 도달 시간 

    public Transform start_pos;
    public Transform end_pos;

    //[HideInInspector] public bool isFire = false;

    //필요한 컴포넌트 
    //[HideInInspector] Transform tr;
    [SerializeField] public Animator anim;
    [SerializeField] public Main_UIManager UM;

    //임시 사용 
    [SerializeField] public RenderCameraSetting renderCamera_cs;

    //void Start()
    //{
    //    tr = GetComponent<Transform>();
    //}


    //애니메이션 이벤트 등록
    public void AutoShoot()
    {
        StartCoroutine(UM.MSAMShoot_Anim());
    }

    private void OnDisable()
    {
        UM.rocketInit();
    }

    //void Update()
    //{
    //    try
    //    {
    //        if (Input.GetKeyDown(KeyCode.Space))
    //        {
    //            StartCoroutine(Fire());
    //        }
    //    }
    //    catch (System.Exception e)
    //    {
    //        Debug.Log(e.ToString());
    //    }
    //}

    //GetComponent<Rigidbody>().AddForce(transform.forward * 10f, ForceMode.Impulse);
    //IEnumerator Fire_Rigid()
    //{
    //    yield return new WaitForSeconds(0.5f);

    //    GameObject _effect = Instantiate(fire_Effect, firePos.position, firePos.rotation);
    //    Destroy(_effect, 0.5f);

    //    GameObject _bullet = Instantiate(bullet, firePos.position, firePos.rotation);
    //}


    public IEnumerator Fire()
    {
        yield return new WaitForSeconds(0.5f);

        GameObject _effect = Instantiate(fire_Effect, firePos.transform);
        Destroy(_effect, 0.5f);

        Shoot(max_height, start_pos, end_pos);
    }

    GameObject bullet_tr;

    public void Shoot(float _max_height, Transform _startPos, Transform _endPos)
    {
        this.max_height = _max_height;

        bullet_tr = Instantiate(bullet, firePos.transform);
        renderCamera_cs.target = bullet_tr.transform;

        bullet_tr.transform.position = _startPos.localPosition;

        Vector3 startPos = _startPos.localPosition;
        Vector3 endPos = _endPos.localPosition;

        var dh = endPos.y - startPos.y;
        var mh = max_height - startPos.y;
        ty = Mathf.Sqrt(2 * this.g * mh); //float

        float a = this.g;
        float b = -2 * ty;
        float c = 2 * dh;

        dat = (-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a); //float

        tx = -(startPos.x - endPos.x) / dat;
        tz = -(startPos.z - endPos.z) / dat;

        this.elapsed_time = 0;

        StartCoroutine(ShootImpl(_startPos, _endPos));
    }

    //포탄날라가는 궤도를 그리는 로직 
    public IEnumerator ShootImpl(Transform _startPos, Transform _endPos)
    {
        Vector3 startPos = _startPos.localPosition;
        Vector3 endPos = _endPos.localPosition;

        while (true)
        {
            this.elapsed_time += Time.deltaTime;
            var tx = startPos.x + this.tx * elapsed_time;
            var ty = startPos.y + this.ty * elapsed_time - 0.5f * g * elapsed_time * elapsed_time;
            var tz = startPos.z + this.tz * elapsed_time;
            var tpos = new Vector3(tx, ty, tz);

            //Debug.DrawRay(transform.position, tpos);

            //GameObject _explo = Instantiate(bulletArrow_Effect, tpos, Quaternion.identity);
            //Destroy(_explo, 0.5f);

            var pos = target.transform.position - bullet_tr.transform.position;
            var rotation = Quaternion.LookRotation(pos);
            bullet_tr.transform.rotation = Quaternion.Slerp(bullet_tr.transform.rotation, rotation, 5.0f * Time.deltaTime);

            //bullet_tr.transform.LookAt(_endPos);
            bullet_tr.transform.position = tpos;

            if (this.elapsed_time >= this.dat)
                break;

            yield return null;
        }

        if (target.isColl)
        {
            Debug.Log("BULLET DESTROY");

            Destroy(bullet_tr);

            target.isColl = false; //초기화 
        }
    }
}
