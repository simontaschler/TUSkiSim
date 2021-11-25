namespace TUSkiSim.Lib
{
    public class SkiTow : Lift
    {
        private readonly int numberOfLanes;

        public SkiTow(int number, int velocity, int length, /*double probFailure,*/ int elements, int numberOfLanes) : base(number, velocity, length, elements) =>
            this.numberOfLanes = numberOfLanes;

        public override int CalcFlowRate() =>
            (int)(numberOfLanes * velocity * (((double)elements) / length));
    }
}
