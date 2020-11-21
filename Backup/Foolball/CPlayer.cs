using System;
using System.Collections.Generic;
using System.Text;

namespace Foolball
{
    public enum EPosition
    {
        GK = 0,
        LB,
        RB,
        DMF,
        LF,
        RF
    }

    public enum EPriority
    {
        Dribbling = 1,
        Pass,
        Shoot 
    }

    public enum ESpeed
    {
        Low = 1,
        Normal,
        Fast
    }

    public enum EPower
    {
        Low = 1,
        Normal,
        High
    }

    public class CPlayer
    {
        public EPosition Position;
        public EPriority Priority;
        public ESpeed Speed;
        public EPower Power;
        public int Energy;
        public ulong TotalDistance;
        public int nOfGoal;
        public int nOfShoot;
        public int nOfPass;
        public int nOfDribble;
        public bool blnAlways = false;
       
       
    }
}
