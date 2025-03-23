using UnityEngine;
using Unity.Cinemachine;

public class CinemachineCameraChange : MonoBehaviour
{
    public CinemachineCamera defaultCam;  // �⺻ ī�޶�
    public CinemachineCamera triggerCam;  // Ʈ���� �� ī�޶�

    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾ Ʈ���ſ� ������ ����
        if (other.CompareTag("Player"))  // �÷��̾� �±� Ȯ��
        {
            defaultCam.Priority = 9;     // �⺻ ī�޶� �켱���� ����
            triggerCam.Priority = 10;    // Ʈ���� ī�޶� �켱���� ����
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // �÷��̾ Ʈ���Ÿ� ����� ����
        if (other.CompareTag("Player"))
        {
            defaultCam.Priority = 10;    // �⺻ ī�޶� �켱���� ����
            triggerCam.Priority = 9;     // Ʈ���� ī�޶� �켱���� ����
        }
    }
}