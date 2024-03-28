using UnityEngine;


public class Skill : MonoBehaviour
{
    private FSMPlayer player;

    private void Awake()
    {
        player = GetComponent<FSMPlayer>();
    }

    public void playBaseAttack()
    {
        player.SetState(CH_STATE.BASEATTACK);
        
    }

    public void playDoubleAttack()
    {
        if (player.Mana.currentMP >= 30)
        {
            player.Mana.UseMana(30);
            player.SetState(CH_STATE.DOUBLEATTACK);
        }
    }
    
    public void playSpinAttack()
    {
        if (player.Mana.currentMP >= 50)
        {
            player.Mana.UseMana(50);
            player.SetState(CH_STATE.SPINATTACK);
        }
    }

    public void playBlock()
    {
        if (player.Mana.currentMP >= 20)
        {
            player.Mana.UseMana(20);
            player.SetState(CH_STATE.BLOCK);
        }
    }
}