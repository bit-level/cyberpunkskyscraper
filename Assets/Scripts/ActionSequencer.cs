using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionList
{
    private List<Action> actions;
    private List<float> delays;
    public Action callback;

    private ActionSequencer engine;
    private Coroutine execution;

    public ActionList(ActionSequencer engine)
    {
        actions  = new List<Action>();
        delays   = new List<float>();
        callback = null;

        this.engine = engine;
        execution = null;
    }

    public void Add(Action action, float delay)
    {
        actions.Add(action);
        delays.Add(delay);
    }

    public void Add(Action action)
    {
        actions.Add(action);
        delays.Add(0f);
    }

    public void AddWaiting(float waitingTime)
    {
        actions.Add(() => { });
        delays.Add(waitingTime);
    }

    public void Execute()
    {
        execution = engine.Execute(actions, delays, callback);
    }

    public void Stop()
    {
        if (execution != null)
            engine.Stop(execution);
    }
}

public class ActionSequencer : MonoBehaviour
{
    public void Stop(Coroutine exec) { StopCoroutine(exec); }

    public Coroutine Execute(List<Action> actions, List<float> delays, Action callback = null)
    {
        if (actions == null || delays == null)  return null;
        if (actions.Count != delays.Count)      return null;

        return StartCoroutine(IExecute(actions, delays, callback));
    }

    private IEnumerator IExecute(List<Action> actions, List<float> delays, Action callback = null)
    {
        for (int i = 0; i < actions.Count; ++i)
        {
            actions[i].Invoke();
            yield return new WaitForSecondsRealtime(delays[i]);
        }

        if (callback != null) callback.Invoke();
    }
}
