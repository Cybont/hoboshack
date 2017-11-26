using UnityEngine;
using System.Collections;

namespace RainRunner
{
    public class Counter
    {
        private float timeStamp = 0;
        public float CoolDownAmount { get; set; }

        public Counter(float coolDownAmount)
        {
            this.CoolDownAmount = coolDownAmount;
        }
        public bool CoolDown()
        {
            if (Time.time > timeStamp)
            {
                timeStamp = Time.time + CoolDownAmount;
                return true;
            }
            else return false;
        }
        public bool CoolDown(float coolDown)
        {
            if (Time.time > timeStamp)
            {
                timeStamp = Time.time + coolDown;
                return true;
            }
            else return false;
        }
        public static float CountDown(float start, float end)
        {
            if (Time.time > start && Time.time < end) return Time.time - end;
            else return 0;
        }



        public void SetTimeStamp()
        {
            timeStamp = Time.time + CoolDownAmount;
        }
        public bool Wait()
        {
            if (timeStamp > Time.time) return false;
            else return true; 
        }

    }
}
