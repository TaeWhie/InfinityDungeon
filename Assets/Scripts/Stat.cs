using System;
using System.Collections.Generic;
using System.Reflection;
[Serializable]
public class Stat 
{
     public float maxHp=0;
     public float attack=0; 
     public float attackSpeed=0;
     public float attackRange=0;
     public float rangeAttackRange=0;
     public Stat(List<string> sentence)
     {
          for (int i = 0; i < sentence.Count; i++)
          {
               var field = this.GetType().GetFields()[i];
               MethodInfo parseMethod = field.FieldType.GetMethod("Parse", new[] { typeof(string) });
               
               field.SetValue(this, parseMethod.Invoke(null, new object[] { sentence[i] }));
          }
     }
}
