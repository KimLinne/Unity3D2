using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorManager : MonoBehaviour
{

    [SerializeField] RectTransform rectTrs;


    Rect selectRect;
    Vector2 vecStart; //클릭 시작점
    Vector2 vecEnd; //클릭 마지막 이동점.

    UnitManager unitManager;

    private void Start()
    {
        rectTrs.gameObject.SetActive(false);
        unitManager = UnitManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) //클릭 시작
        {
            unitManager.ClearAllSelectUnit(); //시작될 때 선택된 유닛 초기화.
            vecStart = Input.mousePosition; //클릭시의 마우스 위치.
            rectTrs.gameObject.SetActive(true); //렉트TRS 게임오브젝트 사용가능으로.
            selectRect = new Rect();
        }

        if (Input.GetKey(KeyCode.Mouse0)) // 드래그 중
        {
            vecEnd = Input.mousePosition;
            drawSelector();
        }


        if (Input.GetKeyUp(KeyCode.Mouse0)) //드래그 종료
        {
            checkSelectedUnit(); //유닛 선택
            rectTrs.gameObject.SetActive(false);
        }
    }

    private void drawSelector()
    {
        Vector2 vecCenter = (vecStart + vecEnd) * 0.5f; //스타트와 엔드의 중심점
        rectTrs.position = vecCenter;

        float sizeX = Mathf.Abs(vecStart.x - vecEnd.x); //드래그한 x의 길이
        float sizeY = Mathf.Abs(vecStart.y - vecEnd.y); //드래그한 y의 길이.

        rectTrs.sizeDelta = new Vector2(sizeX, sizeY);

    }

    private void checkSelectedUnit()
    {
        selectRect.xMin = vecEnd.x < vecStart.x ? vecEnd.x : vecStart.x; // ? = vecEnd.x 값이 vecStart.x 값보다 작다면 
        selectRect.xMax = vecEnd.x < vecStart.x ? vecStart.x : vecEnd.x;

        selectRect.yMin = vecEnd.y < vecStart.y ? vecEnd.y : vecStart.y;
        selectRect.yMax = vecEnd.y < vecStart.y ? vecStart.y : vecEnd.y;

        unitManager.SelectUnit(selectRect);
    }
}
