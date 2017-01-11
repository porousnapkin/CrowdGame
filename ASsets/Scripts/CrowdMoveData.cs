using UnityEngine;

public class CrowdMoveData : ScriptableObject
{
    public float separationStrength = 1.0f;
    public float separationFalloffSpeed = 2.0f;
    public float destinationStrength = 1.0f;
    public float destinationMaxDistanceStrength = 5.0f;
    public float sphereCastSize = 1.0f;
    public float speedDecay = 0.1f;
    public Color color;
}
