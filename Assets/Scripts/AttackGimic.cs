using UniRx;
public abstract class AttackGimic
{
    public int gimicCount;
    
    protected ObjectStat objStat;
    protected int effectID;
    public AttackGimic(int count,int ID,ObjectStat obj)
    {
        gimicCount = count;
        objStat = obj;
        effectID = ID;
        
        objStat.attackCount.Where(x=>x % gimicCount == 0).Subscribe(data=>
        {
            DoGimic();
        });
    }
    public abstract void DoGimic();
}
