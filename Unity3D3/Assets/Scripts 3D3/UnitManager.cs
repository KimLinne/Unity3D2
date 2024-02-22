using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{   //�̱��� ó��.
    public static UnitManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else 
        {
            Destroy(this);
        }
    }
                                        //�����Ҵ�
    private List<Player> listPlayer = new List<Player>();

    public void AddUnit(Player _unit)
    {   
        //_unit�� �̹� ��ϵǾ��ִ� �� Ȯ���ϴ� ���ٽ�.
        //List�� _unit �� ����� �Ǿ����� �ʴٸ� �߰�.
        //if(listPlayer.Exists((x)=> x == _unit)==false)
        //{ 
        listPlayer.Add(_unit); //����Ʈ�� ���� �߰�.
        //}
    }

    public void RemoveUnit(Player _unit)
    {
        listPlayer.Remove(_unit);
    }

    public void MovePosition(Vector3 pos)
    {
        int count = listPlayer.Count;
        for (int iNum = 0; iNum < count; ++iNum)
        {
            Player unit = listPlayer[iNum];
            if (unit.Select == true)
            {
                unit.SetDestination(pos);
            }
        }

        //Ȥ�� foreach �� ���.

        //foreach (Player unit in listPlayer)
        //{
        //    unit.SetDestination(pos);
        //}
    }

    public void ClearAllSelectUnit()
    {
        int count = listPlayer.Count;
        for (int iNum = 0; iNum < count; ++iNum)
        {
            Player sc = listPlayer[iNum];
            sc.Select = false;
        }
    }

    public void SelectUnit(Rect _rect)
    {
        int count = listPlayer.Count;
        for (int iNum = 0; iNum < count; ++iNum)
        {
            Player unit = listPlayer[iNum];
            if (_rect.Contains(Camera.main.WorldToScreenPoint(unit.transform.position)) == true) 
                // Contains // rect���� �� �ȿ� ���ԵǾ� �ִ��� ���´�.
            {
                unit.Select = true; 
            }
        }
    }
}
