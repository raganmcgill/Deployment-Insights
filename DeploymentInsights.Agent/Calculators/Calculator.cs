using DeploymentInsights.Models;
using System.Collections.Generic;
using System.Text;

namespace DeploymentInsights.Agent.Calculators
{
    public class Calculator
    {
        public List<Deployment> Calculate(List<Deployment> deployments)
        {
            var _myLocalList = new List<Deployment>(deployments);

            var dfc = new DeploymentFrequencyCalculator();
            dfc.Calculate(_myLocalList);

            var ltc = new LeadTimeCalculator();
            ltc.Calculate(_myLocalList);

            var cfrc = new ChangeFailRateCalculator();
            cfrc.Calculate(_myLocalList);

            var mttrc = new TimeToRestoreCalculator();
            mttrc.Calculate(_myLocalList);

            return _myLocalList;
        }

    }
}
