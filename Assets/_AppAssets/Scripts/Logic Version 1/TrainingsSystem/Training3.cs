using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.TrainingComponent
{
    class Training3 :Training
    {
        protected internal Training3(): base(/*new Training3Logic()*/)
        {
            this.trainingName = TrainingName.FixingTraining;
        }

        public TrainingLogicCore TrainingLogicCore
        {
            get => default;
            set
            {
            }
        }

        public override void startTraining()
        {
        }
    }
}
