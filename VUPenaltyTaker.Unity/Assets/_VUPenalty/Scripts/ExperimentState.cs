namespace VUPenalty
{
    public abstract class ExperimentState
    {
        protected ExperimentState(ExperimentController controller) => _context = controller;

        public abstract void Init();

        public abstract void Finish();

        public abstract void Tick(float deltaTime);

        protected readonly ExperimentController _context;
    }
}