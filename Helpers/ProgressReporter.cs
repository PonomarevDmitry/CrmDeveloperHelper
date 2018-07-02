using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    class ProgressReporter
    {
        private IWriteToOutput _writeToOutput;
        private int _total;
        private string _procedureName;

        private int _current;
        public int _level = 0;
        public int _percentStep = 10;

        public ProgressReporter(IWriteToOutput _writeToOutput, int total, int percentStep, string procedureName)
        {
            this._writeToOutput = _writeToOutput;
            this._total = total;
            this._procedureName = procedureName;
            this._percentStep = percentStep;

            if (total > 0)
            {
                _writeToOutput.WriteToOutput("{0}   Total: {1}", this._procedureName, total);
            }
        }

        internal void Increase()
        {
            this._current++;

            WriteProgressMessage();
        }
        private void WriteProgressMessage()
        {
            double proc = _total > 0 ? (double)_current * 100 / (double)_total : 0;

            if (proc >= _level)
            {
                _writeToOutput.WriteToOutput("{0}...   {1} %", this._procedureName, _level.ToString("D2"));

                _level += _percentStep;
            }
        }
    }
}
