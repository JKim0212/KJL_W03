using UnityEngine;
using Unity.Cinemachine;

public class CinemachineCameraChange : MonoBehaviour
{
    public CinemachineCamera defaultCam;  // 기본 카메라
    public CinemachineCamera triggerCam;  // 트리거 존 카메라

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 트리거에 들어오면 실행
        if (other.CompareTag("Player"))  // 플레이어 태그 확인
        {
            defaultCam.Priority = 9;     // 기본 카메라 우선순위 낮춤
            triggerCam.Priority = 10;    // 트리거 카메라 우선순위 높임
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 플레이어가 트리거를 벗어나면 실행
        if (other.CompareTag("Player"))
        {
            defaultCam.Priority = 10;    // 기본 카메라 우선순위 복구
            triggerCam.Priority = 9;     // 트리거 카메라 우선순위 낮춤
        }
    }
}