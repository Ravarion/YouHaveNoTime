using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{

    private List<string> Axi;
    private List<float> AxiVal;
    private List<float> AxiValOld;

    void Start()
    {
        Axi = new List<string>();
        AxiVal = new List<float>();
        AxiValOld = new List<float>();
    }

    void Update()
    {
        for (int i = 0; i < Axi.Count; i++)
        {
            AxiValOld[i] = AxiVal[i];
            AxiVal[i] = Input.GetAxisRaw(Axi[i]);
        }
    }

    // Returns true on the frame that the axis hits -1 but will not return true again until the axis goes up and back down again
    public bool GetOnAxisDown(string axis)
    {
        if (Axi.Contains(axis))
        {
            int i = Axi.FindIndex(x => x == axis);
            if (AxiVal[i] < AxiValOld[i] && AxiVal[i] == -1)
            {
                return true;
            }
        }
        else
        {
            Axi.Add(axis);
            AxiVal.Add(0);
            AxiValOld.Add(0);
        }
        return false;
    }

    // Returns true on the frame that the axis hits 1 but will not return true again until the axis goes down and back up again
    public bool GetOnAxisUp(string axis)
    {
        if (Axi.Contains(axis))
        {
            int i = Axi.FindIndex(x => x == axis);
            if (AxiVal[i] > AxiValOld[i] && AxiVal[i] == 1)
            {
                return true;
            }
        }
        else
        {
            Axi.Add(axis);
            AxiVal.Add(0);
            AxiValOld.Add(0);
        }
        return false;
    }

    // Returns true while the axis is held at -1
    public bool GetAxisDown(string axis)
    {
        if (Axi.Contains(axis))
        {
            int i = Axi.FindIndex(x => x == axis);
            if (AxiVal[i] == -1)
            {
                return true;
            }
        }
        else
        {
            Axi.Add(axis);
            AxiVal.Add(0);
            AxiValOld.Add(0);
        }
        return false;
    }

    // Returns true while the axis is held at 1
    public bool GetAxisUp(string axis)
    {
        if (Axi.Contains(axis))
        {
            int i = Axi.FindIndex(x => x == axis);
            if (AxiVal[i] == 1)
            {
                return true;
            }
        }
        else
        {
            Axi.Add(axis);
            AxiVal.Add(0);
            AxiValOld.Add(0);
        }
        return false;
    }

    // Returns true on the frame that both axi hit 0.8 but will not return true again until the axis goes down and back up again
    // With how it is currently coded, it will likely return true a few frames in a row
    public bool GetOnAxiUpUp(string axis1, string axis2)
    {
        if (!Axi.Contains(axis1))
        {
            Axi.Add(axis1);
            AxiVal.Add(0);
            AxiValOld.Add(0);
        }
        if (!Axi.Contains(axis1))
        {
            Axi.Add(axis2);
            AxiVal.Add(0);
            AxiValOld.Add(0);
        }

        int i = Axi.FindIndex(x => x == axis1);
        int j = Axi.FindIndex(x => x == axis2);
        if (AxiVal[i] > AxiValOld[i] && AxiVal[i] >= .8f && AxiVal[j] > AxiValOld[j] && AxiVal[j] >= .8f)
        {
            return true;

        }
        return false;
    }

    public bool GetOnAxiDownDown(string axis1, string axis2)
    {
        if (!Axi.Contains(axis1))
        {
            Axi.Add(axis1);
            AxiVal.Add(0);
            AxiValOld.Add(0);
        }
        if (!Axi.Contains(axis1))
        {
            Axi.Add(axis2);
            AxiVal.Add(0);
            AxiValOld.Add(0);
        }

        int i = Axi.FindIndex(x => x == axis1);
        int j = Axi.FindIndex(x => x == axis2);
        if (AxiVal[i] < AxiValOld[i] && AxiVal[i] <= -.8f && AxiVal[j] < AxiValOld[j] && AxiVal[j] <= -.8f)
        {
            return true;

        }
        return false;
    }

    public bool GetOnAxiUpDown(string axis1, string axis2)
    {
        if (!Axi.Contains(axis1))
        {
            Axi.Add(axis1);
            AxiVal.Add(0);
            AxiValOld.Add(0);
        }
        if (!Axi.Contains(axis1))
        {
            Axi.Add(axis2);
            AxiVal.Add(0);
            AxiValOld.Add(0);
        }

        int i = Axi.FindIndex(x => x == axis1);
        int j = Axi.FindIndex(x => x == axis2);
        if (AxiVal[i] > AxiValOld[i] && AxiVal[i] >= .8f && AxiVal[j] < AxiValOld[j] && AxiVal[j] <= -.8f)
        {
            return true;

        }
        return false;
    }
}
