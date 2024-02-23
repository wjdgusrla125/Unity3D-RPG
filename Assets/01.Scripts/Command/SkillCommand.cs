using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCommand : Command
{
    private int _Index = -1;

    public SkillCommand(int index)
    {
        _Index = index;
    }
    
    public override bool Execute(GameObject target)
    {
        Skill skill = target.GetComponent<Skill>();

        if (skill != null)
        {
            if (_Index == 0)
            {
                skill.playBaseAttack();
                return true;
            }
            
            if (base.Execute(target))
            {
                switch (_Index)
                {
                    case 1:
                        skill.playDoubleAttack();
                        break;
                    case 2:
                        skill.playSpinAttack();
                        break;
                    case 3:
                        skill.playBlock();
                        break;
                }
                
                PlayerIO.SaveData();
                return true;
            }
        }
        return false;
    }
}
