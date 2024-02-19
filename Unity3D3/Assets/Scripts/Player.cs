using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{

    NavMeshAgent agent;
    Vector3 vecDestination; //����Ʈ 
    Vector3 startPosition;
    [SerializeField] float randomRadiusRange = 30f; //������
    [SerializeField] bool selected = false;

    float waitingTime; //���� �Ŀ� ��� ��ٸ��� �ð�
    [SerializeField] Vector2 vecWaitingMinMax;

    [Header("����������")]
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

        //NavMesh.RemoveAllNavMeshData();//����޽��� ���� �ڵ�ε� ����.
        //NavMeshSurface surface = GetComponent<NavMeshSurface>();
        //surface.BuildNavMesh();
    }

    private void OnDestroy()
    {
        ////1.�������� �÷��� �߿� �˰��� ���ؼ� ����.
        //UnitManager.Instance.RemoveUnit(this);

        //2.� ���ǿ� ���ؼ� �����Ͱ� �����Ǿ���� ��.(��. �����Ϳ��� �÷��̰� ������ ��, �÷��� �� ������� �����ʹ� �����ȴ�.)

        if (UnitManager.Instance != null) //null�� �ƴ϶��.
        {
            UnitManager.Instance.RemoveUnit(this);
        }
    }

    private void Start()
    {
        //setNewPath(); �ѹ��� �ڵ����� �̵��ϵ���.
        //setNewWaitTime(); 

        if (selected == false) return;

        UnitManager.Instance.AddUnit(this);
    }



    void Update()
    {
        //if (isArrive() == true) //���ο� �̵� ��ġ�� ��������,
        //                        //if (checkWaitTime() == true) return; ������ �� isArrive() == true ���� �ִ� ����� ������ 
        //                        //�۾��غ� ��.
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
        //�����ϱ� �� ����
        if (setOffMesh == false) //�¿����޽��� �޽����
        {
            setOffMesh = true;//�¿����޽��� Ʈ��� �ϰ� �Ʒ� �ڵ带 �۵�.

            linkData = agent.currentOffMeshLinkData;

            offMeshStart = transform.position; //���������� �� ��ġ
            offMeshEnd = linkData.endPos + new Vector3(0,agent.height * 0.5f,0); //������ �̵� ��ġ�� ���� ���� + agent.height�� ���ݰ���ŭ ����Ѵ�. 

            agent.isStopped = true; //�� �ڵ� �۵� �߿��� ������Ʈ�� ����.
            JumpSpeed = Vector3.Distance(offMeshStart, offMeshEnd)/ agent.speed; // �����޽���ŸƮ�� �����޽������� �Ÿ� / agent�� �ӵ�.
            //float distance = (offMeshStart - offMeshEnd).magnitude; ���� ���� ��� ���� ���� �ڵ�.

            JumpMaxHeight = (offMeshEnd - offMeshStart).y + JumpHeigt; //������ ���� ���̸� ����

        }

        //3�� �Ŀ� ��Ÿ���� �ȷ�Ǵ� ��ų, �ڵ�.
        //float coolTime = 0.0f;
        //if (��ų��)
        //{
        //    coolTime += Time.deltaTime / 3;
        //    if (coolTime > 1.0f)
        //    { 
                
        //    }
        //}    

        JumpRatio += (Time.deltaTime / JumpSpeed);

        Vector3 movePos = Vector3.Lerp(offMeshStart, offMeshEnd, JumpRatio); //Lerp�� A�� B ������ �߰� ���� ���� ã�� ��.
        movePos.y = offMeshStart.y + JumpMaxHeight * JumpRatio + -JumpHeigt * Mathf.Pow(JumpRatio, 2); // ������ ����� ����� �ڵ�. �߿��ڵ�
        transform.position = movePos;

        if (JumpRatio >= 1.0f)//������ ��
        {
            JumpRatio = 0.0f; //���� �ߴٸ� ratio�� 0.0f �� �ʱ�ȭ.
            agent.CompleteOffMeshLink(); //�����޽� ��� ���Ḧ �˸�.
            agent.isStopped = false; //������Ʈ�� �̵��� �ٽ� ����
            setOffMesh = false; 

        }
    }



    private void setNewWaitTime()
    {
        waitingTime = Random.Range(vecWaitingMinMax.x, vecWaitingMinMax.y);
    }

    /// <summary>
    /// true�� �Ǹ� ��ٷ����Ѵ�, false�� �Ǹ� �̵��ص� �ȴ�.
    /// </summary>
    /// <returns></returns>
    private bool checkWaitTime()
    {
        if (waitingTime >= 0.0f) //��ٷ��� �ϴ� �ð��� 0.0 �� �ƴ϶��
        {
            waitingTime -= Time.deltaTime;//�ð��� ���ҽ�Ų��
            if (waitingTime < 0.0f)//���� ���ҽ�Ų �ð��� 0.0���ϰ� �ȴٸ�
            {
                setNewWaitTime();//���� ��ٸ��� �ð��� ����
                return false;//�̵��϶�� ����
            }

            return true;//���߶�� ����
        }

        return false;//�̵��϶�� ����
    }
    private void setNewPath()
    {
        vecDestination = getRandomPoint();
        agent.SetDestination(vecDestination);
    }


    /// <summary>
    /// NPC�� �����ߴ��� Ȯ���մϴ�.
    /// </summary>
    private bool isArrive()
    {
        if (agent.velocity == Vector3.zero) //������ �ִ� ����, �̵��Ұ����� ����.
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
    /// ������Ʈ�� �̵� ������ ��ġ�� ������ üũ�ؼ� �����մϴ�.
    /// </summary>
    /// <returns></returns>
    private Vector3 getRandomPoint()
    {                       //�÷��̾��� ������ + ����, ������ �߽����� ���� ���� * ���� ����
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
