using Unity.Cinemachine;
using UnityEngine;

public class CameraDeadzoneSwitcher : MonoBehaviour
{
    public CinemachineCamera virtualCam;

    [Header("Deadzone if IsPlayerOrbiting")]
    public Vector2 deadzoneInOrbit = new Vector2(0.2f, 0.2f);

    [Header("Deadzone if !IsPlayerOrbiting")]
    public Vector2 deadzoneOutOrbit = new Vector2(0.5f, 0.5f);

    private PlayerOrbit orbit;

    void Start()
    {
        orbit = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerOrbit>();
        virtualCam = GetComponent<CinemachineCamera>();
    }

    public void NewFollowTarget(Transform obj, float lens)
    {
        virtualCam.Follow = obj;
        virtualCam.LookAt = obj;
    }

    void Update()
    {
        
        var transposer = virtualCam.GetComponent<CinemachinePositionComposer>();
        if (transposer == null) return;

        if (orbit.IsPlayerOrbiting)
        {
            transposer.DeadZoneDepth = deadzoneInOrbit.x;
            transposer.DeadZoneDepth = deadzoneInOrbit.y;
        }
        else
        {
            transposer.DeadZoneDepth = deadzoneOutOrbit.x;
            transposer.DeadZoneDepth = deadzoneOutOrbit.y;
        }
    }
}
