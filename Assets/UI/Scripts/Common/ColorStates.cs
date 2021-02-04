using UnityEngine;
using UnityEngine.UI;

public class ColorStates : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] Color active = Color.white;
    [SerializeField] Color inactive = Color.gray;
    [SerializeField] Graphic graphicComponent;
#pragma warning restore 0649

    public enum State { Active, Inactive }

    private State _currentState = State.Inactive;

    public void SetState(State newState)
    {
        if (newState == _currentState) return;

        switch (newState)
        {
            case State.Active:
                graphicComponent.color = active;
                break;
            case State.Inactive:
                graphicComponent.color = inactive;
                break;
        }

        _currentState = newState;
    }
}
