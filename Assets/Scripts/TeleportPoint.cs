using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour {

    // the position of the teleport point (where to teleport avatar to)
    public Vector3 position;

    // true if can teleport to this point
    public bool canTeleport
    {
        get
        {
            return _canTeleport;
        }
        set
        {
            if (_canTeleport == value) return;
            _canTeleport = value;
            if (_canTeleport)
            {
                positions.Add(id, position);
                beam.Play();
            }
            else
            {
                positions.Remove(id);
                beam.Clear();
                beam.Stop();
            }
        }
    }
    private bool _canTeleport;

    // the particle 'beam' marking the teleport point
    private ParticleSystem beam;

    // records last used ID
    private static int lastID = 0;

    // ID of current TeleportPoint
    private int id;

    // stores all locations of teleport points
    private static Dictionary<int, Vector3> positions = new Dictionary<int, Vector3>();

    // Teleport 'avatar' to the nearest enabled TeleportPoint to 'point'
    // if the distance between the two is below the given threshold
    public static bool TeleportTo(Transform avatar, Vector3 point, float thresh = 0.2f)
    {
        float threshL2sqr = thresh * thresh;
        float bestL2sqr = float.MaxValue;
        int bestID = -1;

        foreach (KeyValuePair<int, Vector3> entry in positions)
        {
            float l2sqr = (point - entry.Value).sqrMagnitude;
            if (l2sqr <= threshL2sqr && l2sqr < bestL2sqr)
            {
                bestL2sqr = l2sqr;
                bestID = entry.Key;
            }
        }

        if (bestID == -1) return false;
        avatar.position = point;
        return true;
    }

    void Start () {
        id = lastID++;
        beam = GetComponent<ParticleSystem>();
        positions.Add(id, position);
    }
}
