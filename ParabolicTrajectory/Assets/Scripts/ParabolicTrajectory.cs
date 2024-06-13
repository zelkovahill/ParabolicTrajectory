using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngineInternal;

public class ParabolicTrajectory : MonoBehaviour
{

    public LineRenderer lineRenderer;       // Line Renderer 컴포넌트를 할당할 변수
    public int resloution = 30;             // 궤적을 그릴 때 사용할 점의 개수
    public float timeStep = 0.1f;           // 시간 간격

    public Transform launchPoint;           // 발사 위치를 나타내는 트랜스폼
    public float myRoataion;
    public float launchPower;               // 발사 속도
    public float launchAngle;               // 발사 방향
    public float launchDirection;           // 발사 각도
    public float gravity = -9.8f;           // 중력 값
    public GameObject projectilePrefabs;    // 발사할 물체의 프리팹






    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RenderTrajectory();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            LaunchProjectile(projectilePrefabs);
        }
    }

    void RenderTrajectory()                             // 궤적을 계산하고 Line Renderer에 설정하는 함수
    {
        lineRenderer.positionCount = resloution;        // Line Renderer의 점 개수 설정
        Vector3[] points = new Vector3[resloution];     // 궤적 점들을 저장할 배열

        for (int i = 0; i < resloution; i++)               // 각 시간 간격마다 점의 위치를 계산
        {
            float t = i * timeStep;                     // 현재 시간 계산
            points[i] = CalculatePositionAtTime(t);     // 현재 시간에서의 위치 계산

        }

        lineRenderer.SetPositions(points);              // 계산된 점들을 Line Renderer에 설정
    }

    Vector3 CalculatePositionAtTime(float time)         // 주어진 시간에서 물체의 위치를 계산 하는 함수
    {
        float launchAngleRand = Mathf.Deg2Rad * launchAngle;        // 발사 각도를 라디안으로 변환
        float launchDorectionRad = Mathf.Deg2Rad * launchDirection;

        // 시간 t에서의 x,y,z 좌표 계산
        float x = launchPower * time * Mathf.Cos(launchAngleRand) * Mathf.Cos(launchDorectionRad);
        float z = launchPower * time * Mathf.Cos(launchAngleRand) * Mathf.Sin(launchDorectionRad);
        float y = launchPower * time * Mathf.Sin(launchAngleRand) + 0.5f * gravity * time * time;

        // 발사 위치를 기준으로 계산된 위치 반환
        return launchPoint.position + new Vector3(x, y, z);
    }

    // 물체를 발사하는 함수
    public void LaunchProjectile(GameObject _object)
    {
        GameObject temp = Instantiate(_object);

        temp.transform.position = launchPoint.position;
        temp.transform.rotation = launchPoint.rotation;
        //temp.transform.SetParent(null);

        // Rigidboby 컴포넌트를 가져옴
        Rigidbody rb = temp.GetComponent<Rigidbody>();
        if (rb == null) 
        {
            rb = temp.AddComponent<Rigidbody>();
        }

        if (rb != null) 
        {
            rb.isKinematic = false;

            // 발사 각도와 방향을 라디안으로 변환
            float launchAngleRad = Mathf.Deg2Rad * launchAngle;
            float launchDirectionRad = Mathf.Deg2Rad * launchDirection;

            // 초기 속도를 계산
            float initalVelocityX = launchPower  * Mathf.Cos(launchAngleRad) * Mathf.Cos(launchDirectionRad);
            float initalVelocityZ = launchPower  * Mathf.Cos(launchAngleRad) * Mathf.Sin(launchDirectionRad);
            float initalVelocityY = launchPower * Mathf.Sin(launchAngleRad);

            Vector3 initialVelocity = new Vector3(initalVelocityX, initalVelocityY, initalVelocityZ);

            // Rigidbody에 초기 속도를 적용
            rb.velocity = initialVelocity;
        }
    }
}
