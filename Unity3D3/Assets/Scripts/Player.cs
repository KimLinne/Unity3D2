using Unity.AI.Navigation;
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

    [Header("점프데이터")]
    OffMeshLinkData linkData;
    [SerializeField] float JumpSpeed = 0.0f;
    float JumpRatio = 0.0f;
    float JumpMaxHeight = 0.0f;
    [SerializeField] float JumpHeigt = 5f;
    bool setOffMesh = false;
    Vector3 offMeshStart;
    Vector3 offMeshEnd;

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

        //NavMesh.RemoveAllNavMeshData();//나브메쉬를 삭제 코드로도 가능.
        //NavMeshSurface surface = GetComponent<NavMeshSurface>();
        //surface.BuildNavMesh();
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

        if (agent.isOnOffMeshLink == true)
        {
            doOffMesh();
        }
    }

    private void doOffMesh()
    {
        //점프하기 전 설정
        if (setOffMesh == false) //셋오프메쉬가 펄스라면
        {
            setOffMesh = true;//셋오프메쉬를 트루로 하고 아래 코드를 작동.

            linkData = agent.currentOffMeshLinkData;

            offMeshStart = transform.position; //시작지점은 내 위치
            offMeshEnd = linkData.endPos + new Vector3(0,agent.height * 0.5f,0); //마지막 이동 위치는 엔드 포스 + agent.height의 절반값만큼 상승한다. 

            agent.isStopped = true; //이 코드 작동 중에는 에이전트가 멈춤.
            JumpSpeed = Vector3.Distance(offMeshStart, offMeshEnd)/ agent.speed; // 오프메쉬스타트와 오프메쉬엔드의 거리 / agent의 속도.
            //float distance = (offMeshStart - offMeshEnd).magnitude; 위와 같은 결과 값을 갖는 코드.

            JumpMaxHeight = (offMeshEnd - offMeshStart).y + JumpHeigt; //적절한 점프 높이를 설정

        }

        //3초 후에 쿨타임이 안료되는 스킬, 코드.
        //float coolTime = 0.0f;
        //if (스킬온)
        //{
        //    coolTime += Time.deltaTime / 3;
        //    if (coolTime > 1.0f)
        //    { 
                
        //    }
        //}    

        JumpRatio += (Time.deltaTime / JumpSpeed);

        Vector3 movePos = Vector3.Lerp(offMeshStart, offMeshEnd, JumpRatio); //Lerp란 A와 B 사이의 중간 지점 값을 찾는 것.
        movePos.y = offMeshStart.y + JumpMaxHeight * JumpRatio + -JumpHeigt * Mathf.Pow(JumpRatio, 2); // 포물선 운동으로 만드는 코드. 중요코드
        transform.position = movePos;

        if (JumpRatio >= 1.0f)//도착한 것
        {
            JumpRatio = 0.0f; //도착 했다면 ratio를 0.0f 로 초기화.
            agent.CompleteOffMeshLink(); //오프메쉬 사용 종료를 알림.
            agent.isStopped = false; //에이전트의 이동을 다시 동작
            setOffMesh = false; 

        }
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
