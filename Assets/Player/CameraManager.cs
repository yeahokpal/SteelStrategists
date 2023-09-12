using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera vcam1;
    public CinemachineVirtualCamera vcam2;
    public CinemachineVirtualCamera vcam3;
    public CinemachineVirtualCamera vcam4;

    public void SwitchPriority(int room)
    {
        switch (room)
        {
            case 1:
                vcam1.Priority = 1;
                vcam2.Priority = 0;
                vcam3.Priority = 0;
                vcam4.Priority = 0;
                break;
            case 2:
                vcam1.Priority = 0;
                vcam2.Priority = 1;
                vcam3.Priority = 0;
                vcam4.Priority = 0;
                break;
            case 3:
                vcam1.Priority = 0;
                vcam2.Priority = 0;
                vcam3.Priority = 1;
                vcam4.Priority = 0;
                break;
            case 4:
                vcam1.Priority = 0;
                vcam2.Priority = 0;
                vcam3.Priority = 0;
                vcam4.Priority = 1;
                break;
        }
    }
}
