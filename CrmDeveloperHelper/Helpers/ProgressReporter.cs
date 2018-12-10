using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class ProgressReporter
    {
        private IWriteToOutput _writeToOutput;
        private readonly int _total;
        private readonly string _procedureName;

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
                _writeToOutput.WriteToOutput(Properties.OutputStrings.ProgressReporterHeader, this._procedureName, total);
            }
        }

        internal void Increase()
        {
            this._current++;

            WriteProgressMessage();
        }
        private void WriteProgressMessage()
        {
            double proc = _total > 0 ? (double)_current * 100 / _total : 0;

            if (proc >= _level)
            {
                _writeToOutput.WriteToOutput(Properties.OutputStrings.ProgressReporterStep, this._procedureName, _level.ToString("D2"));
                
                _level += _percentStep;
            }
        }
    }
}
