using UnityEngine;
namespace Assets.Scripts.MissionComponent
{
    class Mission2 : Mission
    {
        protected internal Mission2(GameObject Room) : base(Room/*new Training2Logic()*/)
        {
            this.missionName = MissionName.FuelLeaking;
        }

        public override void startMission()
        {
        }

    }
}
