using UnityEngine;

public class AStarPathfinding : MonoBehaviour
{
    [SerializeField] private DistanceFormulaTypes distanceFormula;
    [SerializeField] private ushort SearchFrequencyDelay;

    private Square[,] searchGrid;
}
