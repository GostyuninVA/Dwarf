using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Rope : MonoBehaviour
{
    [SerializeField] private GameObject _ropeSegmentPrefub;
    private List<GameObject> _ropeSegments = new List<GameObject>();

    public bool IsIncreasing { get; set; }
    public bool IsDecreasing { get; set; }

    public Rigidbody2D ConnectedObject;
    [SerializeField] private float _maxRopeSegmentLength = 1.0f;
    [SerializeField] private float _ropeSpeed = 4.0f;
    private LineRenderer _lineRender;

    private void Start()
    {
        _lineRender = GetComponent<LineRenderer>();

        ResetLength();
    }

    public void ResetLength()
    {
        foreach(GameObject segment in _ropeSegments)
        {
            Destroy(segment);
        }

        _ropeSegments = new List<GameObject>();

        IsDecreasing = false;
        IsIncreasing = false;

        CreateRopeSegment();
    }

    private void CreateRopeSegment()
    {
        GameObject segment = Instantiate(_ropeSegmentPrefub, transform.position, Quaternion.identity);
        segment.transform.SetParent(transform, true);

        Rigidbody2D segmentBody = segment.GetComponent<Rigidbody2D>();
        SpringJoint2D segmentJoint = segment.GetComponent<SpringJoint2D>();

        if(segmentBody == null || segmentJoint == null)
        {
            Debug.Log("segmentbody or segmentJoint is null");
            return;
        }

        _ropeSegments.Insert(0, segment);

        if(_ropeSegments.Count == 1)
        {
            SpringJoint2D connectedObjectJoint = ConnectedObject.GetComponent<SpringJoint2D>();
            connectedObjectJoint.connectedBody = segmentBody;
            connectedObjectJoint.distance = 0.1f;

            segmentJoint.distance = _maxRopeSegmentLength;
        }
        else
        {
            GameObject nextSegments = _ropeSegments[1];

            SpringJoint2D nextSegmentJoint = nextSegments.GetComponent<SpringJoint2D>();

            nextSegmentJoint.connectedBody = segmentBody;
            segmentJoint.distance = 0.0f;
        }

        segmentJoint.connectedBody = GetComponent<Rigidbody2D>();
    }

    private void RemoveRopeSegment()
    {
        if(_ropeSegments.Count < 2)
        {
            return;
        }

        GameObject topSegment = _ropeSegments[0];
        GameObject nextSegment = _ropeSegments[1];

        SpringJoint2D nextSegmentJoint = nextSegment.GetComponent<SpringJoint2D>();
        nextSegmentJoint.connectedBody = GetComponent<Rigidbody2D>();

        _ropeSegments.RemoveAt(0);
        Destroy(topSegment);
    }

    private void Update()
    {
        GameObject topSegment = _ropeSegments[0];
        SpringJoint2D topSegmentJoint = topSegment.GetComponent<SpringJoint2D>();

        IsIncreasing = InputManager.Instance.KeyRopeDown;
        IsDecreasing = InputManager.Instance.KeyRopeUp;

        if(IsIncreasing)
        {
            if(topSegmentJoint.distance >= _maxRopeSegmentLength)
            {
                CreateRopeSegment();
            }
            else
            {
                topSegmentJoint.distance += _ropeSpeed * Time.deltaTime;
            }
        }

        if(IsDecreasing)
        {
            if(topSegmentJoint.distance <= 0.005f)
            {
                RemoveRopeSegment();
            }
            else
            {
                topSegmentJoint.distance -= _ropeSpeed * Time.deltaTime;
            }
        }

        if(_lineRender != null)
        {
            _lineRender.positionCount = _ropeSegments.Count + 2;

            _lineRender.SetPosition(0, transform.position);

            for(int i = 0; i < _ropeSegments.Count; i++)
            {
                _lineRender.SetPosition(i + 1, _ropeSegments[i].transform.position);
            }

            SpringJoint2D connectedObjectJoint = ConnectedObject.GetComponent<SpringJoint2D>();
            _lineRender.SetPosition(_ropeSegments.Count + 1, 
                                    ConnectedObject.transform.TransformPoint(connectedObjectJoint.anchor));
        }
    }
}
