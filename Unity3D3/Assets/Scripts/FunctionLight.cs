using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionLight : MonoBehaviour
{
    public enum TypeLight
    { 
        Disable,    //비사용
        Allways,    //항상 사용
        OnlyNight,  //밤에만
        OnlyDays,   //낮에만
    }

    public TypeLight typeLight;
    Material matWindow;
    GameObject objLight;

    public void init(bool _isNight)
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        matWindow = Instantiate(mr.material);
        mr.material = matWindow;

        objLight = GetComponentInChildren<Light>().gameObject;
        //objLight = transform.GetChild(0).gameObject;
        //objLight = transform.Find("Point Light").gameObject;

        //matWindow.EnableKeyword("_EMISSION"); //마테리얼의 이미션 기능을 켤 때.
        //if (typeLight == TypeLight.Disable)
        //{
        //    matWindow.DisableKeyword("_EMISSION");
        //    objLight.SetActive(false);
        //}//마테리얼의 이미션 기능을 끌 때.

        if ((_isNight == true && typeLight == TypeLight.OnlyNight) ||
            (_isNight == false && typeLight == TypeLight.OnlyDays)||
            (typeLight == TypeLight.Allways)) //아래 문장을 또는 으로 만들어 코드를 줄임.
        {
            TurnOnLight(true);
        }
        //else if (_isNight == false && typeLight == TypeLight.OnlyDays)
        //{
        //    matWindow.EnableKeyword("_EMISSION");
        //    objLight.SetActive(true);
        //}
        //else if (typeLight == TypeLight.Allways)
        //{
        //    matWindow.EnableKeyword("_EMISSION");
        //    objLight.SetActive(true);
        //}
        else 
        {
            TurnOnLight(false);
        }

    }

    public void TurnOnLight(bool _value)
    {
        if (_value == true)
        {   
            //if(typeLight ==TypeLight.Allways || typeLight ==TypeLight.OnlyNight||)
            matWindow.EnableKeyword("_EMISSION");
           objLight.SetActive(true);
        }
        else
        {
            matWindow.DisableKeyword("_EMISSION");
            objLight.SetActive(false);
        }
    }
   
}
