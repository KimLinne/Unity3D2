using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{

    NavMeshAgent agent;
    Vector3 vecDestination; //디폴트 
    Vector3 startPosition;
    [SerializeField] float randomRadiusRange = 30f; //반지름
    [SerializeField] bool selected = false;

    float waitingTime; //도착 후에 잠깐 기다리는 시간
    [SerializeField] Vector2 vecWaitingMinMax;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, randomRadiusRange,
            NavMesh.AllAreas))
        {
            startPosition = hit.position;
        }
        else 
        {
            startPosition = transform.position;
        }
    }

    private void OnDestroy()
    {
        ////1.동적으로 플레이 중에 알고리즘에 의해서 삭제.
        //UnitManager.Instance.RemoveUnit(this);

        //2.어떤 조건에 의해서 데이터가 삭제되어야할 때.(예. 에디터에서 플레이가 끝났을 때, 플레이 중 만들어진 데이터는 삭제된다.)

        if (UnitManager.Instance != null) //null이 아니라면.
        {
            UnitManager.Instance.RemoveUnit(this);
        }
    }

    private void Start()
    {
        //setNewPath(); 한번은 자동으로 이동하도록.
        //setNewWaitTime(); 

        if (selected == false) return;

        UnitManager.Instance.AddUnit(this);
    }



    void Update()
    {
        //if (isArrive() == true) //새로운 이동 위치를 잡아줘야함,
        //                        //if (checkWaitTime() == true) return; 문장을 위 isArrive() == true 옆에 넣는 방법도 있으니 
        //                        //작업해볼 것.
        //{
        //    if (checkWaitTime() == true) return;
        //    setNewPath();

        //    //agent.SetDestination(getRandomPoint());
        //}
    }

    private void setNewWaitTime()
    {
        waitingTime = Random.Range(vecWaitingMinMax.x, vecWaitingMinMax.y);
    }

    /// <summary>
    /// true가 되면 기다려야한다, false가 되면 이동해도 된다.
    /// </summary>
    /// <returns></returns>
    private bool checkWaitTime()
    {
        if (waitingTime >= 0.0f) //기다려야 하는 시간이 0.0 이 아니라면
        {
            waitingTime -= Time.deltaTime;//시간을 감소시킨다
            if (waitingTime < 0.0f)//만약 감소시킨 시간이 0.0이하가 된다면
            {
                setNewWaitTime();//새로 기다리는 시간을 정의
                return false;//이동하라고 전달
            }

            return true;//멈추라고 전달
        }

        return false;//이동하라고 전달
    }
    private void setNewPath()
    {
        vecDestination = getRandomPoint();
        agent.SetDestination(vecDestination);
    }


    /// <summary>
    /// NPC가 도착했는지 확인합니다.
    /// </summary>
    private bool isArrive()
    {
        if (agent.velocity == Vector3.zero) //가만히 있는 상태, 이동불가능한 상태.
        {
            
            return true;
        }

        //if (Vector3.Distance(vecDestination, transform.position) == 0.0f)
        //{
        //    return true;
        //}

        return false;
    }

    /// <summary>
    /// 에이전트가 이동 가능한 위치를 스스로 체크해서 전달합니다.
    /// </summary>
    /// <returns></returns>
    private Vector3 getRandomPoint()
    {                       //플레이어의 포지션 + 랜덤, 유닛을 중심으로 구형 범위 * 범위 지정
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * randomRadiusRange;

        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, randomRadiusRange,
            NavMesh.AllAreas))
        {
            return hit.position;
        }

        return startPosition;
    }

    public void SetDestination(Vector3 _pos)
    {
        agent.SetDestination(_pos);
    }

}
