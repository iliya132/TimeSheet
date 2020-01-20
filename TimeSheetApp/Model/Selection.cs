using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheetApp.Model
{
    public class Selection
    {
        public Selection()
        {

        }
        public Selection(int _processID=1, int _businessBlockSelected = 1, int _supportSelected = 1, int _clientWaySelected = 1, int _escalationSelected = 1, int _formatSelected = 1, int _riskSelected = 1)
        {
            ProcessID = _processID;
            BusinessBlockSelected = _businessBlockSelected;
            SupportSelected = _supportSelected;
            ClientWaySelected = _clientWaySelected;
            EscalationSelected = _escalationSelected;
            FormatSelected = _formatSelected;
            RiskSelected = _riskSelected;
        }
        public void SetValues(Selection selection)
        {
            BusinessBlockSelected = selection.BusinessBlockSelected;
            SupportSelected = selection.SupportSelected;
            ClientWaySelected = selection.ClientWaySelected;
            EscalationSelected = selection.EscalationSelected;
            RiskSelected = selection.RiskSelected;
            FormatSelected = selection.FormatSelected;
        }
        private int _processID;
        public int ProcessID { get => _processID; set => _processID = value; }
        private int _selectedCount = 1;
        public int SelectedCount { get => _selectedCount; set => _selectedCount = value; }
        private int _businessBlockSelected = 0;
        public int BusinessBlockSelected { get => _businessBlockSelected; set => _businessBlockSelected = value; }
        private int _supportSelected = 0;
        public int SupportSelected { get => _supportSelected; set => _supportSelected = value; }
        private int _clientWaySelected = 0;
        public int ClientWaySelected { get => _clientWaySelected; set => _clientWaySelected = value; }
        private int _escalationSelected = 0;
        public int EscalationSelected { get => _escalationSelected; set => _escalationSelected = value; }
        private int _formatSelected = 0;
        public int FormatSelected { get => _formatSelected; set => _formatSelected = value; }
        private int _riskSelected = 0;
        public int RiskSelected { get => _riskSelected; set => _riskSelected = value; }

    }
}
