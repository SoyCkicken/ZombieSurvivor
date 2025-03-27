using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : LivingEntity
{
    public NavMeshAgent navMeshAgent;
    public LayerMask targetLayer;
    public LivingEntity target;
    public float FindDelay = 0.5f;
    public float LastFindDelay;
    public float speed;
    public bool isDie = false;
    public int hp = 50000;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        StartCoroutine(Move());
       
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            target = null;
        }

    }
   
    private bool hasTarget
    {
        get
        {
            if (target != null && isDie == false)
            {
                return true;
            }
            return false;
        }
        
    }
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        base.OnDamage(damage, hitPoint, hitNormal);
    }
    public IEnumerator Move()
    {
        while (true)
        {
            if (target != null)
            {
                if (Time.time >= LastFindDelay + FindDelay)
                {
                    LastFindDelay = Time.time;
                    navMeshAgent.SetDestination(target.transform.position);
                }
                if (Time.time >= 10f)
                {
                    speed = navMeshAgent.speed;
                    navMeshAgent.speed += 0.001f;
                }
            }
            else
            {
                //Ÿ���� ���� ��� ���� �ڱ⸦ �߽����� ���� ���� �׸��� �� �ȿ� �ִ� ������Ʈ�� �˻��ؼ� targetLayer�� �ִ� ������� Ȯ��
                Collider[] colliders = Physics.OverlapSphere(transform.position, 5f, targetLayer);
                for (int i = 0; i < colliders.Length; i++)
                {
                    //�������� ������ �װ� ������� ��´�
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();
                    if (livingEntity != null && !livingEntity.dead)
                    {
                        target = livingEntity;
                        break;
                    }
                     
                }
            }
                yield return new WaitForSeconds(0.25f);
        }
        
    }
}
