public abstract class IState 
{
    protected ObjectStat currentGameObject;
    public int animStateID;
    public IState(ObjectStat obj,int animStateID)
    {
        currentGameObject = obj;
        animStateID = animStateID;
    }
    public abstract void StateFixedUpdate();
    public abstract void StateUpdate();
    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void StartAni();
    public abstract void EndAni();
}