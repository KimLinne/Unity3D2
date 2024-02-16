using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{   //싱글톤 처리.
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
                                        //동적할당
    private List<Player> listPlayer = new List<Player>();

    public void AddUnit(Player _unit)
    {   
        //_unit이 이미 등록되어있는 지 확인하는 람다식.
        //List에 _unit 이 등록이 되어있지 않다면 추가.
        //if(listPlayer.Exists((x)=> x == _unit)==false)
        //{ 
        listPlayer.Add(_unit); //리스트에 유닛 추가.
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
            unit.SetDestination(pos);
        }

        //혹은 foreach 문 사용.

        //foreach (Player unit in listPlayer)
        //{
        //    unit.SetDestination(pos);
        //}
    }
}
