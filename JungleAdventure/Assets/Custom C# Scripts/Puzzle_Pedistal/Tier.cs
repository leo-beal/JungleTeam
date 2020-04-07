using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;

public class Tier : MonoBehaviour
{
    public bool IsSolved = false;
    public bool IsBase = false;

    private Vector3 StartPosition;
    private Vector3 SolvedPosition;

    [Range(0.0f, 360.0f, order = 1)]
    public float AngleDelta = 10.0f;
    public Tier Outer;
    public Tier Inner;
    public Material Unsolved;
    public Material Solved;
    public float TierStep;

    public float LowerTierHeight
    {
        get
        {
            if (Outer == null)
                return 0f;

            return Outer.TierStep + Outer.LowerTierHeight;
        }
    }

    [Range(0.01f, 1f)]
    public float TransitionSpeed = 0.05f;

    private new Renderer renderer;
    private Vector3 TargetPosition;

    private void Awake()
    {
        SolvedPosition = gameObject.transform.parent.position;
        
        if (IsBase)
        {
            IsSolved = true;
        }
        
        Vector3 offset = new Vector3()
        {
            x = 0,
            y = LowerTierHeight,
            z = 0
        };

        StartPosition = TargetPosition = gameObject.transform.position = SolvedPosition + offset;
        
        renderer = GetComponent<Renderer>();

        SetMaterial();
    }

    private void Update()
    {
        if (IsSolved)
        {
            TargetPosition = SolvedPosition;
            
            if (Outer != null)
                SetRotation(Outer.transform.rotation);
        }

        if (gameObject.transform.position != TargetPosition)
            ComputePosition(TargetPosition);
    }

    private void SetRotation(Quaternion rotation)
    {
        gameObject.transform.rotation = rotation;
    }

    private void SetMaterial()
    {
        if (Solved == null)
            return;

        renderer.material = IsSolved ? Solved : Unsolved;
    }

    private void StepTierDown()
    {
        TargetPosition = new Vector3()
        {
            x = gameObject.transform.position.x,
            y = gameObject.transform.position.y - TierStep,
            z = gameObject.transform.position.z
        };

        if (Inner != null)
            Inner.StepTierDown();
    }

    private void StepTierUp()
    {
        TargetPosition = new Vector3()
        {
            x = gameObject.transform.position.x,
            y = gameObject.transform.position.y + TierStep,
            z = gameObject.transform.position.z
        };

        if (Outer != null)
            Outer.StepTierUp();
    }

    public bool CheckSolved()
    {
        if (IsSolved)
            return true;

        if (Outer == null)
            IsSolved = true;

        if ((Outer.transform.rotation.eulerAngles.y + AngleDelta > gameObject.transform.rotation.eulerAngles.y
            && Outer.transform.rotation.eulerAngles.y - AngleDelta < gameObject.transform.rotation.eulerAngles.y)
            || IsSolved)
        {
            IsSolved = true;

            SetMaterial();

            StepTierDown();
            
            gameObject.transform.rotation = Outer.transform.rotation;
        }

        return IsSolved;
    }

    private void ComputePosition(Vector3 target)
    {
        //compute mins and maxes
        float yMin = Mathf.Min(StartPosition.y, SolvedPosition.y);
        float yMax = Mathf.Max(StartPosition.y, SolvedPosition.y);

        //1 or 0 based on if we are altering that component
        var transform = new Vector3()
        {
            x = 0,
            y = gameObject.transform.position.y == target.y ? 0 : TransitionSpeed,
            z = 0
        };

        //apply time difference
        transform *= Time.deltaTime;

        //apply direction
        transform.y *= (gameObject.transform.position.y > target.y ? -1 : 1);
        
        //get proposed position
        var position = transform + gameObject.transform.position;
        
        //validate
        position.y = CheckRange(position.y, yMin, yMax);
        
        //apply position
        gameObject.transform.position = position;
    }

    private float CheckRange(float val, float min, float max)
    {
        if (val > max)
            return max;

        if (val < min)
            return min;

        return val;
    }
}
