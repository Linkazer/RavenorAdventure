using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to detect all character around the current character
/// </summary>
[System.Obsolete("Fanlk non utilisé")]
public class FlankSetter : MonoBehaviour
{
    public void SetFlank(CPN_Character flankingCharacter)
    {
        List<Node> neighbours = Grid.GetNeighbours(flankingCharacter.CurrentNode);

        foreach(Node n in neighbours)
        {
            List<CPN_Character> chara = n.GetNodeComponent<CPN_Character>();

            foreach(CPN_Character c in chara)
            {
                c.AddMeleeCharacter(flankingCharacter);

                flankingCharacter.AddMeleeCharacter(c);
            }
        }
    }

    public void UnsetFlank(CPN_Character flankingCharacter)
    {
        List<Node> neighbours = Grid.GetNeighbours(flankingCharacter.CurrentNode);

        foreach (Node n in neighbours)
        {
            List<CPN_Character> chara = n.GetNodeComponent<CPN_Character>();

            foreach (CPN_Character c in chara)
            {
                c.RemoveMeleeCharacter(flankingCharacter);

                flankingCharacter.RemoveMeleeCharacter(c);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
