using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TetrinoDraggable : AttachedDraggable
{
    [SerializeField]
    private Pool<TetrinoDraggable> _pool;
    private bool _placeable;

    private Tetrino _tetrino;
    public Tetrino CurrentTetrino() { return _tetrino; }


//  ~~~~~~~~~~~~~~~~~~~~~~~~ All the rotation ~~~~~~~~~~~~~~~~~~~~~~~~
    public void Rotate() { GetComponent<Animator>().SetTrigger("StartRotating"); }
    public void Rotate(float angle) { _tetrino.Rotate(angle); }

    [SerializeField]
    private float AnglePerSecond;
    private bool _rotating = false;

    void FixedUpdate()
    {
        if (_rotating)
        {
            Rotate(AnglePerSecond * Time.deltaTime);
        }
    }

    public void StartRotation()
    { _rotating = true; }

    public void StopRotation()
    {
        _rotating = false;
        _tetrino.SafeRotate90();
    }
    //  ~~~~~~~~~~~~~~~~~~~~~~~~ All the rotation ~~~~~~~~~~~~~~~~~~~~~~~~


    protected void Awake()
    {
        _pool = FindObjectOfType<Pool<TetrinoDraggable>>();
        if (_pool == null)
        { Debug.LogError("Tetrino " + this.name + " started off not in a pool."); }
    }


    public override void OnBeginDrag(PointerEventData eventData)
    {
        _tetrino.RememberPosition();
        base.OnBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        _placeable = _tetrino.TakeAim();
        base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (_placeable)
        {
            _tetrino.PlaceBlocks();

            base._ownerChanged = true;
            base.SetOwner(_pool.gameObject);

            _pool.Store(this);
        }

        base.OnEndDrag(eventData);
    }

    public void Construct(Tetrino.Type type, char[] letters, Thaum.Type[] thaums)
    {
        switch (type)
        {
            case Tetrino.Type.T1:
                _tetrino = new Tetrino1(letters, thaums, this.transform);
                break;
            case Tetrino.Type.T2:
                _tetrino = new Tetrino2(letters, thaums, this.transform);
                break;
            case Tetrino.Type.T3:
                _tetrino = new Tetrino3(letters, thaums, this.transform);
                break;
            case Tetrino.Type.T4:
                _tetrino = new Tetrino4(letters, thaums, this.transform);
                break;
            case Tetrino.Type.T5:
                _tetrino = new Tetrino5(letters, thaums, this.transform);
                break;

            default:
                Debug.LogError("Use tetrino types 1-5.");
                break;
        }
    }
}
