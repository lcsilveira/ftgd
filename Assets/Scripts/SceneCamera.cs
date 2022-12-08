using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCamera : MonoBehaviour
{
    [SerializeField] private Cinemachine.CinemachineVirtualCamera cinemachineVirtualCamera;

    private void Start()
    {
        cinemachineVirtualCamera.Follow = NewPlayer.Instance.transform;
    }
}
