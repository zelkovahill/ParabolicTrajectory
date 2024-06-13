using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngineInternal;

public class ParabolicTrajectory : MonoBehaviour
{

    public LineRenderer lineRenderer;       // Line Renderer ������Ʈ�� �Ҵ��� ����
    public int resloution = 30;             // ������ �׸� �� ����� ���� ����
    public float timeStep = 0.1f;           // �ð� ����

    public Transform launchPoint;           // �߻� ��ġ�� ��Ÿ���� Ʈ������
    public float myRoataion;
    public float launchPower;               // �߻� �ӵ�
    public float launchAngle;               // �߻� ����
    public float launchDirection;           // �߻� ����
    public float gravity = -9.8f;           // �߷� ��
    public GameObject projectilePrefabs;    // �߻��� ��ü�� ������






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

    void RenderTrajectory()                             // ������ ����ϰ� Line Renderer�� �����ϴ� �Լ�
    {
        lineRenderer.positionCount = resloution;        // Line Renderer�� �� ���� ����
        Vector3[] points = new Vector3[resloution];     // ���� ������ ������ �迭

        for (int i = 0; i < resloution; i++)               // �� �ð� ���ݸ��� ���� ��ġ�� ���
        {
            float t = i * timeStep;                     // ���� �ð� ���
            points[i] = CalculatePositionAtTime(t);     // ���� �ð������� ��ġ ���

        }

        lineRenderer.SetPositions(points);              // ���� ������ Line Renderer�� ����
    }

    Vector3 CalculatePositionAtTime(float time)         // �־��� �ð����� ��ü�� ��ġ�� ��� �ϴ� �Լ�
    {
        float launchAngleRand = Mathf.Deg2Rad * launchAngle;        // �߻� ������ �������� ��ȯ
        float launchDorectionRad = Mathf.Deg2Rad * launchDirection;

        // �ð� t������ x,y,z ��ǥ ���
        float x = launchPower * time * Mathf.Cos(launchAngleRand) * Mathf.Cos(launchDorectionRad);
        float z = launchPower * time * Mathf.Cos(launchAngleRand) * Mathf.Sin(launchDorectionRad);
        float y = launchPower * time * Mathf.Sin(launchAngleRand) + 0.5f * gravity * time * time;

        // �߻� ��ġ�� �������� ���� ��ġ ��ȯ
        return launchPoint.position + new Vector3(x, y, z);
    }

    // ��ü�� �߻��ϴ� �Լ�
    public void LaunchProjectile(GameObject _object)
    {
        GameObject temp = Instantiate(_object);

        temp.transform.position = launchPoint.position;
        temp.transform.rotation = launchPoint.rotation;
        //temp.transform.SetParent(null);

        // Rigidboby ������Ʈ�� ������
        Rigidbody rb = temp.GetComponent<Rigidbody>();
        if (rb == null) 
        {
            rb = temp.AddComponent<Rigidbody>();
        }

        if (rb != null) 
        {
            rb.isKinematic = false;

            // �߻� ������ ������ �������� ��ȯ
            float launchAngleRad = Mathf.Deg2Rad * launchAngle;
            float launchDirectionRad = Mathf.Deg2Rad * launchDirection;

            // �ʱ� �ӵ��� ���
            float initalVelocityX = launchPower  * Mathf.Cos(launchAngleRad) * Mathf.Cos(launchDirectionRad);
            float initalVelocityZ = launchPower  * Mathf.Cos(launchAngleRad) * Mathf.Sin(launchDirectionRad);
            float initalVelocityY = launchPower * Mathf.Sin(launchAngleRad);

            Vector3 initialVelocity = new Vector3(initalVelocityX, initalVelocityY, initalVelocityZ);

            // Rigidbody�� �ʱ� �ӵ��� ����
            rb.velocity = initialVelocity;
        }
    }
}
