using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InputContorller : MonoBehaviour
{

    Animator anim;
    [SerializeField] Transform trsLookAt;

    [SerializeField, Range(0.0f, 1.0f)] float lookAtWeight; // 쳐다보지 않거나, 쳐다보거나. NPC와의 대화 기능에 주요.

    List<string> listDanceStateName = new List<string>();

    [SerializeField] GameObject objInven;
    [SerializeField] GameObject objButton;

    Dictionary<string, string> dicNameValue = new Dictionary<string,string>();

    [SerializeField, Range(0.0f, 1.0f)] float distanceToGround;

    private void OnAnimatorIK(int layerIndex) //파란색으로 나오는 건 유니티에서 실행시켜주는 기능이다.
    {
        if (trsLookAt != null)
        {
            anim.SetLookAtWeight(lookAtWeight);         //이 형태는 부하가 있음.
            anim.SetLookAtPosition(trsLookAt.position); //trsLookAt 에 해당되는 특정 개체를 바라보게 한다.
                                                        //다른 애니메이션 중에도 바라보게 된다.

        }

        anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
        anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);

        anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);
        anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);

        if (Physics.Raycast(anim.GetIKPosition(AvatarIKGoal.LeftFoot)+ Vector3.up , 
            Vector3.down, 
            out RaycastHit leftHit,
            distanceToGround + 1f, LayerMask.GetMask("Ground")))
        {
            Vector3 footPos = leftHit.point;
            footPos.y += distanceToGround;

            anim.SetIKPosition(AvatarIKGoal.LeftFoot, footPos);

            anim.SetIKRotation(AvatarIKGoal.LeftFoot, 
                Quaternion.LookRotation(
                    Vector3.ProjectOnPlane(transform.forward, leftHit.normal), leftHit.normal
                    ));
        }

        if (Physics.Raycast(anim.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up,
            Vector3.down,
            out RaycastHit rightHit,
            distanceToGround + 1f, LayerMask.GetMask("Ground")))
        {
            Vector3 footPos = rightHit.point;
            footPos.y += distanceToGround;

            anim.SetIKPosition(AvatarIKGoal.RightFoot, footPos);

            anim.SetIKRotation(AvatarIKGoal.RightFoot,
                Quaternion.LookRotation(
                    Vector3.ProjectOnPlane(transform.forward, rightHit.normal), rightHit.normal
                    ));
        }



    }

    private void Awake()
    {
        anim = GetComponent<Animator>();

        dicNameValue.Add("Dance_1", "어떤춤");
        dicNameValue.Add("Dance_2", "어떤 어떤춤");
        dicNameValue.Add("Dance_3", "잘 모르겠는춤");

    }

    void Start()
    {
        //*AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;*/ // 애니메이터에게 실제 작동중인
                                                                                   // 애니메이션 컨트롤러에게 묻는다.


        initDance();
        createDanceUi(); 


        //버튼 만들어지는 시스템 만들 것.
    }

    
    void Update()
    {
        moving();
        doDance();
        activeDanceInventory();
        


    }

    private void moving()
    {
        //블렌드 트리 사용 시 겟엑시스로우는 사용하지 말 것. 변화 수치 -1,0,1
        //블렌드 트리 사용 시 겟 엑시스 사용. 변화 수치 -1 ~ 1.
        //블렌드 트리 사용으로 점프 기능도 재현해볼 것. 중요.
        anim.SetFloat("SpeedVertical", Input.GetAxis("Vertical"));
        anim.SetFloat("SpeedHorizontal", Input.GetAxis("Horizontal"));

        //anim.SetBool("Front", Input.GetKey(KeyCode.W));
        //anim.SetBool("Back", Input.GetKey(KeyCode.S));
        //anim.SetBool("Right", Input.GetKey(KeyCode.D));
        //anim.SetBool("Left", Input.GetKey(KeyCode.A));
    }

    private void activeDanceInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            bool isActive = objInven.activeSelf; //켜져 있는지 꺼져있는지 확인하며 데이터를 보내줌.

            objInven.gameObject.SetActive(!isActive); // 데이터가 트루라면 false로, false라면 true로.
            
        }
    }

    private void initDance()
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips; //Dance_
        int count = clips.Length;
        for (int iNum = 0; iNum < count; ++iNum)
        {
            string animName = clips[iNum].name;
            if (animName.Contains("Dance_")) //Dance_ 가 적혀있다면 
            {
                listDanceStateName.Add(animName); 
            }
        }
    }

    private void createDanceUi()
    {
        Transform parent = objInven.transform;
        int count = listDanceStateName.Count;
        for(int iNum = 0; iNum < count; ++iNum)
        {
            int Number = iNum;
            //버튼 이름, 기능 변경.
             GameObject obj = Instantiate(objButton, parent);

            TMP_Text objText = obj.GetComponentInChildren<TMP_Text>();
            string curName = listDanceStateName[Number];
            objText.text = dicNameValue[curName];

            Button objBtn = obj.GetComponent<Button>();
            objBtn.onClick.AddListener(() =>
            {
                anim.CrossFade(listDanceStateName[Number], 0.1f);
            });
            //람다식 사용 시에는 동적 변화하는 지역변수를 넣어선 안된다. [iNum]을 그대로 전달하지 말고
            //int Number = iNum; 으로 지정 해줘야 한다.

                
        }

    }
    private void doDance()

    {

        

        if (Input.GetKey(KeyCode.Alpha1))
        {
            //anim.Play("Dance1"); //애니메이터 안의 Dance1 애니메이션을 재생.
            anim.CrossFade("Dance1", 0.2f); // 절대값 아님,0~1, Dance1 을 재생.
                                            // 노멀라이즈 시간 동안 상대적 속도로 다른 애니메이션으로 이동.

        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            //anim.Play("Dance2");
            anim.CrossFade("Dance2", 0.2f);
        }
        

        if (Input.GetAxis("Vertical") != 0.0 || Input.GetAxis("Horizontal") != 0.0f) //둘 중의 하나라도 ||. 눌리면 move 블렌드 트리 재생.
        {
            anim.Play("move");
        }
    }
}
