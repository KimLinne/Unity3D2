using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputContorller : MonoBehaviour
{

    Animator anim;
    [SerializeField] Transform trsLookAt;

    [SerializeField, Range(0.0f, 1.0f)] float lookAtWeight; // �Ĵٺ��� �ʰų�, �Ĵٺ��ų�. NPC���� ��ȭ ��ɿ� �ֿ�.

    List<string> listDanceStateName = new List<string>();
    private void OnAnimatorIK(int layerIndex) //�Ķ������� ������ �� ����Ƽ���� ��������ִ� ����̴�.
    {
        if(trsLookAt != null)
        {
            anim.SetLookAtWeight(lookAtWeight);
            anim.SetLookAtPosition(trsLookAt.position); //trsLookAt �� �ش�Ǵ� Ư�� ��ü�� �ٶ󺸰� �Ѵ�.
                                                        //�ٸ� �ִϸ��̼� �߿��� �ٶ󺸰� �ȴ�.

        }
        
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips; // �ִϸ��̼� Ŭ���� ������.
        
        //���� �ð����� ��ư ���.
    }

    
    void Update()
    {
        moving();
        doDance();
        
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
