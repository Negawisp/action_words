using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetrino : MonoBehaviour
{
    public enum Type
    {
        T1,
        T2,
        T3,
        T4,
        T5
    }

    /*
    protected void Construct(int nLetters, char[] letters, Thaum.Type[] types, Transform transform)
    {
        _transform = transform;
        TakeBlocks(nLetters, letters, types);
    }
    */

    public const int __A__ = 2;
    [SerializeField]
    private BlockPool _blockPool;

    protected LetterBlock[][] _blocks;
    private Transform _transform;


    public void Construct(bool[][] blueprint, char[] letters, Thaum.Type[] types)
    {
        _blocks = TakeBlocks(blueprint, letters, types);
        Arrange(_blocks);
    }
    

    public void Arrange(LetterBlock[][] blueprint)
    {
        _transform = transform;
        
        /*
        if (blueprint.Length != blueprint[0].Length)
        {
            Debug.LogError("Blueprint should be squared.");
            return;
        }
        */

        int sz = blueprint.Length;
        float inv = 1 / (float)sz;

        for (int i = 0; i < sz; i++)
        {
            for (int j = 0; j < sz; j++)
            {
                if (blueprint[i][j] != null)
                {
                    _blocks[i][j].transform.SetParent(this.transform);
                    RectTransform rect = blueprint[i][j].GetComponent<RectTransform>();
                    rect.anchorMin = new Vector2(inv *  j     , inv *  i     );
                    rect.anchorMax = new Vector2(inv * (j + 1), inv * (i + 1));

                    rect.offsetMax = Vector2.zero;
                    rect.offsetMin = Vector2.zero;
                }
            }
        }

        _blocks = blueprint;
    }


    protected virtual LetterBlock[][] TakeBlocks (bool[][] blueprint, char[] letters, Thaum.Type[] types)
    {
        _blockPool = FindObjectOfType<BlockPool>();
        if (_blockPool == null)
        { Debug.LogError("Scene didn't have a BlockPool object."); }

        int n = blueprint.Length;
        int m = blueprint[0].Length;
        LetterBlock[][] blocks = new LetterBlock[n][];
        for (int i = 0; i < n; i++)
        {
            blocks[i] = new LetterBlock[m];

            for (int j = 0; j < m; j++)
            {
                if (blueprint[i][j])
                {
                    blocks[i][j] = _blockPool.Get();
                    blocks[i][j].Construct(letters[i], types[i]);
                }
            }
        }

        return blocks;
    }

    
    
    public bool TakeAim()
    {
        bool placeable = true;

        foreach (LetterBlock[] blocks in _blocks)
        {
            foreach (LetterBlock block in blocks)
            {
                if (block)
                {
                    block.FindAimedCell();

                    if (!block.CanBePlaced())
                        placeable = false;
                }
            }
        }

        return placeable;
    }

    public void PlaceBlocks()
    {
        foreach (LetterBlock[] blocks in _blocks)
        {
            foreach (LetterBlock block in blocks)
            {
                if (block)
                    block.StayAtCell();
            }
        }
        FindObjectOfType<TetrinoPlayerSet>().GiveNewTetrino();
    }
    
    public void Rotate(float angle)
    {
        angle = Mathf.Deg2Rad * angle;

        int n = _blocks.Length;
        int m = _blocks[0].Length;

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                if (_blocks[i][j])
                {
                    float x = _blocks[i][j].transform.localPosition.x;
                    float y = _blocks[i][j].transform.localPosition.y;
                    float z = _blocks[i][j].transform.localPosition.z;

                    _blocks[i][j].transform.localPosition = new Vector3(Mathf.Cos(angle) * x - Mathf.Sin(angle) * y,
                                                                      Mathf.Sin(angle) * x + Mathf.Cos(angle) * y,
                                                                      z);
                }
            }
        }
        
    }

    public void SafeRotate90()
    {
        int n = _blocks.Length;
        int m = _blocks[0].Length;
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                if (_blocks[i][j])
                {
                    _blocks[i][j].ResetLocalPosition();
                    float x = _blocks[i][j].transform.localPosition.x;
                    float y = _blocks[i][j].transform.localPosition.y;
                    float z = _blocks[i][j].transform.localPosition.z;

                    _blocks[i][j].transform.localPosition = new Vector3(-y, x, z);
                }
            }
        }
        RememberPosition();
    }

    public void RememberPosition()
    {
        int n = _blocks.Length;
        int m = _blocks[0].Length;
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                if (_blocks[i][j])
                    _blocks[i][j].RememberLocalPosition();
            }
        }
    }

    
}
