using UnityEngine;

public class Utils : MonoBehaviour
{
    [SerializeField] AnimationCurve acelerationUp = null;
    [SerializeField] AnimationCurve acelerationDown = null;

    public static class Curves
    {
        public static class Aceleration
        {
            public static AnimationCurve Up;
            public static AnimationCurve Down;
        }
    }

    private void Awake()
    {
        Curves.Aceleration.Up = acelerationUp;
        Curves.Aceleration.Down = acelerationDown;
    }
}
