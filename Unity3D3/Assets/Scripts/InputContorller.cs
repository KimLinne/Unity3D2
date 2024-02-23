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

    [SerializeField, Range(0.0f, 1.0f)] float lookAtWeight; // �Ĵٺ��� �ʰų�, �Ĵٺ��ų�. NPC���� ��ȭ ��ɿ� �ֿ�.

    List<string> listDanceStateName = new List<string>();

    [SerializeField] GameObject objInven;
    [SerializeField] GameObject objButton;

    Dictionary<string, string> dicNameValue = new Dictionary<string,string>();

    [SerializeField, Range(0.0f, 1.0f)] float distanceToGround;

    private void OnAnimatorIK(int layerIndex) //�Ķ������� ������ �� ����Ƽ���� ��������ִ� ����̴�.
    {
        if (trsLookAt != null)
        {
            anim.SetLookAtWeight(lookAtWeight);         //�� ���´� ���ϰ� ����.
            anim.SetLookAtPosition(trsLookAt.position); //trsLookAt �� �ش�Ǵ� Ư�� ��ü�� �ٶ󺸰� �Ѵ�.
                                                        //�ٸ� �ִϸ��̼� �߿��� �ٶ󺸰� �ȴ�.

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

        dicNameValue.Add("Dance_1", "���");
        dicNameValue.Add("Dance_2", "� ���");
        dicNameValue.Add("Dance_3", "�� �𸣰ڴ���");

    }

    void Start()
    {
        //*AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;*/ // �ִϸ����Ϳ��� ���� �۵�����
                                                                                   // �ִϸ��̼� ��Ʈ�ѷ����� ���´�.


        initDance();
        createDanceUi(); 


        //��ư ��������� �ý��� ���� ��.
    }

    
    void Update()
    {
        moving();
        doDance();
        activeDanceInventory();
        


    }

    private void moving()
    {
        //���� Ʈ�� ��� �� �ٿ��ý��ο�� ������� �� ��. ��ȭ ��ġ -1,0,1
        //���� Ʈ�� ��� �� �� ���ý� ���. ��ȭ ��ġ -1 ~ 1.
        //���� Ʈ�� ������� ���� ��ɵ� �����غ� ��. �߿�.
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
            bool isActive = objInven.activeSelf; //���� �ִ��� �����ִ��� Ȯ���ϸ� �����͸� ������.

            objInven.gameObject.SetActive(!isActive); // �����Ͱ� Ʈ���� false��, false��� true��.
            
        }
    }

    private void initDance()
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips; //Dance_
        int count = clips.Length;
        for (int iNum = 0; iNum < count; ++iNum)
        {
            string animName = clips[iNum].name;
            if (animName.Contains("Dance_")) //Dance_ �� �����ִٸ� 
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
            //��ư �̸�, ��� ����.
             GameObject obj = Instantiate(objButton, parent);

            TMP_Text objText = obj.GetComponentInChildren<TMP_Text>();
            string curName = listDanceStateName[Number];
            objText.text = dicNameValue[curName];

            Button objBtn = obj.GetComponent<Button>();
            objBtn.onClick.AddListener(() =>
            {
                anim.CrossFade(listDanceStateName[Number], 0.1f);
            });
            //���ٽ� ��� �ÿ��� ���� ��ȭ�ϴ� ���������� �־ �ȵȴ�. [iNum]�� �״�� �������� ����
            //int Number = iNum; ���� ���� ����� �Ѵ�.

                
        }

    }
    private void doDance()

    {

        

        if (Input.GetKey(KeyCode.Alpha1))
        {
            //anim.Play("Dance1"); //�ִϸ����� ���� Dance1 �ִϸ��̼��� ���.
            anim.CrossFade("Dance1", 0.2f); // ���밪 �ƴ�,0~1, Dance1 �� ���.
                                            // ��ֶ����� �ð� ���� ����� �ӵ��� �ٸ� �ִϸ��̼����� �̵�.

        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            //anim.Play("Dance2");
            anim.CrossFade("Dance2", 0.2f);
        }
        

        if (Input.GetAxis("Vertical") != 0.0 || Input.GetAxis("Horizontal") != 0.0f) //�� ���� �ϳ��� ||. ������ move ���� Ʈ�� ���.
        {
            anim.Play("move");
        }
    }
}
