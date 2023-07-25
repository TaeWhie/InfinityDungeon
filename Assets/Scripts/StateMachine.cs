public class StateMachine 
{
   private IState _mCurrentState;
   private IState _mPrevState;
   public void Update()
   {
      if (_mCurrentState != null)
      {
         _mCurrentState.StateUpdate();
      }
   }
   public void FixedUpdate()
   {
      if (_mCurrentState != null)
      {
         _mCurrentState.StateFixedUpdate();
      }
   }
   public void ChangeState(IState nextState)
   {
      if (nextState != null)
      {
         if (_mCurrentState != null)
         {
            _mPrevState = _mCurrentState;
            _mCurrentState.OnExit();
         }

         _mCurrentState = nextState;
         _mCurrentState.OnEnter();
      }
   }
   public IState ReturnCurrentState()
   {
      return _mCurrentState;
   }
   public IState ReturnPrevState()
   {
      return _mPrevState;
   }
}
