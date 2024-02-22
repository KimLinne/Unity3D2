using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputContorller : MonoBehaviour
{

    Animator anim;
    [SerializeField] Transform trsLookAt;

    [SerializeField, Range(0.0f, 1.0f)] float lookAtWeight; // 쳐다보지 않거나, 쳐다보거나. NPC와의 대화 기능에 주요.

    List<string> listDanceStateName = new List<string>();
    private void OnAnimatorIK(int layerIndex) //파란색으로 나오는 건 유니티에서 실행시켜주는 기능이다.
    {
        if(trsLookAt != null)
        {
            anim.SetLookAtWeight(lookAtWeight);
            anim.SetLookAtPosition(trsLookAt.position); //trsLookAt 에 해당되는 특정 개체를 바라보게 한다.
                                                        //다른 애니메이션 중에도 바라보게 된다.

        }
        
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips; // 애니메이션 클립을 꺼낸다.
        
        //다음 시간에는 버튼 사용.
    }

    
    void Update()
    {
        moving();
        doDance();
        
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
