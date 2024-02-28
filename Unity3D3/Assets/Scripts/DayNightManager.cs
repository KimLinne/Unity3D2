using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DayNightManager : MonoBehaviour
{

    private Light directionalLight;
    [SerializeField,Range(0f,24f)] private float timeOfDay;

    public bool isNight = false;
    private string keyIsNight = "isNight";
    public bool AutoChange = false; // �ڵ� ���� ����.

    [SerializeField,Range(0f,24f)] float dayTime = 14;
    [SerializeField, Range(0f, 24f)] float nightTime = 23;

    List<FunctionLight> listLights = new List<FunctionLight>();

    private void OnApplicationQuit() //������ ���� �� ��
    {
        string value = isNight == true ? "t" : "f";
        PlayerPrefs.SetString(keyIsNight, value);
    }

    private void Awake()
    {
        string value = PlayerPrefs.GetString(keyIsNight, "f");
        isNight = value == "t" ? true : false; // t��� true�� �ƴ϶�� false��.

        if (directionalLight == null)
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            int count = lights.Length;
            for (int iNum = 0; iNum < count; ++iNum)
            {
                Light light = lights[iNum];
                if (light.type == LightType.Directional)
                {
                    directionalLight = light;
                }
                else 
                {
                    listLights.Add(light.transform.parent.GetComponent<FunctionLight>());

                }
            }
            count = listLights.Count;
            for (int iNum = 0; iNum < count; ++iNum)
            {
                FunctionLight light = listLights[iNum];
                light.init(isNight);
            }

        }

    }

    void Start()
    {
        
    }

   
    void Update()
    {
        timeOfDay %= 24; //0~24 ������ ���� ����
        if (AutoChange == true)
        {
            timeOfDay += Time.deltaTime; // �ð��� �ڵ����� ����

            if (timeOfDay > dayTime)
            {
                isNight = true;
            }
            else if (timeOfDay > nightTime)
            {
                isNight = false;
            }
                       
        }

        else //�ð��� ���� ����
        {
            if (isNight == true )
            {
                timeOfDay += Time.deltaTime;
                if (timeOfDay > nightTime)
                {
                    timeOfDay = nightTime;
                } // ������ ����
                               
            }
            else
            {
                timeOfDay += Time.deltaTime;
                if (timeOfDay < nightTime && timeOfDay > dayTime) //�ΰ��� ���� ���� ���� �ÿ�.
                {
                    timeOfDay = dayTime;
                }// ��
            }

            

        }

        if (timeOfDay > 23.1f)
        {
            timeOfDay = 4;
        }

        updateLighting();
    }

    private void updateLighting()
    {
        if (directionalLight == null) 
        {
            Debug.LogError("���ӿ� �𷺼ų� ����Ʈ�� �������� �ʽ��ϴ�.");
            return;
        }
        
        float timePercent = timeOfDay / 24f; //0~1
        directionalLight.transform.localRotation = Quaternion.Euler(new Vector3(timePercent * 360f -90f, 150f,0f));

        if (directionalLight.transform.eulerAngles.x >= 180f && directionalLight.color.r > 0.0f) //night
        {
            
                directionalLight.color -= new Color(1, 1, 1) * Time.deltaTime * 10.0f;
                if (directionalLight.color.r < 0.0f)
                {
                    directionalLight.color = new Color(0, 0, 0);

                    foreach (FunctionLight light in listLights)
                    {
                        light.TurnOnLight(true);
                    }
                }
                      

        }
        else if (directionalLight.transform.localRotation.x >= 0.0f && directionalLight.color.r < 1.0f) //day
        {
            
                directionalLight.color += new Color(1, 1, 1) * Time.deltaTime * 10f;
                if (directionalLight.color.r > 1.0f)
                {
                    directionalLight.color = new Color(1, 1, 1);

                    foreach (FunctionLight light in listLights)
                    {
                        light.TurnOnLight(false);
                    }
                }
            

        }
    }
}
