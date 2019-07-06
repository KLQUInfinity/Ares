using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.TrainingComponent
{
    public enum TrainingName
    {
        FireExtinguishing,
        CarryingErgonomics,
        FixingTraining
    }
    public class TrainingFactory
    {

        public static Training createTraining(TrainingName TR_NAme) {
            switch (TR_NAme)
            {
                case TrainingName.FireExtinguishing:
                    return new Training1();
                case TrainingName.CarryingErgonomics:
                    return new Training2();
                case TrainingName.FixingTraining:
                    return new Training3();
                default:
                    return null;
            }
        }
    }
}
