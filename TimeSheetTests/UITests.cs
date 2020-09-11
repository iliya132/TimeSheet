using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;

using Automation = System.Windows.Automation;

namespace TimeSheetTests
{
    [TestClass]
    public class UITests
    {
#if DevAtHome
        private const string timeSheetDebugPath = @"C:\Users\iliya\Source\Repos\iliya132\TimeSheet\TimeSheetApp\bin\Debug\TimeSheetApp.exe";
#else
        private const string  timeSheetDebugPath = @"C:\Users\iliya\Source\Repos\iliya132\TimeSheet\TimeSheetApp\bin\Debug\TimeSheetApp.exe";
#endif
        AutomationElement AEDesktop { get; set; }
        Process tsProc { get; set; }
        AutomationElement AETimeSheetApp { get; set; }

        public UITests()
        {
            tsProc = Process.Start(timeSheetDebugPath);
            WaitWhileCondition(5000, tsProc == null, ()=>
            {
                Process[] processes = Process.GetProcessesByName("TimeSheetApp");
                if (processes.Length > 0)
                {
                    tsProc = processes[0];
                }
            });
            if (tsProc == null)
            {
                throw new Exception("Failed to start TimeSheet process");
            }
            AEDesktop = AutomationElement.RootElement;
            if (AEDesktop == null)
            {
                throw new Exception("Unable to get desktop");
            }
            WaitWhileCondition(5000, AETimeSheetApp == null, () =>
            {
                AETimeSheetApp = AEDesktop.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "TimeSheet"));
            });
            if (AETimeSheetApp == null)
            {
                throw new Exception("Unable to get TimeSheet application");
            }
        }

        [TestMethod]
        public void FindWindow()
        {
            Assert.IsNotNull(AETimeSheetApp);
        }

        [TestMethod]
        public void HelpButton()
        {
            bool success = false;
            AutomationElement helpBtn = null;
            WaitWhileCondition(5000, AETimeSheetApp == null, () =>
            {
                helpBtn = AETimeSheetApp.FindFirst(TreeScope.Children,
                new PropertyCondition(AutomationElement.AutomationIdProperty, "HelpBtn"));
            });
            
            if (helpBtn == null)
                throw new Exception("Failed to found HelpBtn");
            InvokePattern ClickHelpBtn = (InvokePattern)helpBtn.GetCurrentPattern(InvokePattern.Pattern);
            ClickHelpBtn.Invoke();
            Process helpFileProcess = null;
            AutomationElement helpElement = null;
            WaitWhileCondition(1000, helpElement == null, () =>
            {
                Process[] processes = Process.GetProcessesByName("hh");
                if (processes.Length > 0)
                {
                    helpFileProcess = processes[0];
                }
                helpElement = AEDesktop.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "TimeSheetHelp"));
            });
            if (helpFileProcess != null)
            {
                helpFileProcess.CloseMainWindow();
                success = true;
            }
            else
            {
                throw new Exception("Failed to open TSHelp");
            }
            Assert.IsTrue(success);

        }

        [TestInitialize]
        public void Setup()
        {
            
        }

        [TestCleanup]
        public void Cleanup()
        {
            tsProc?.CloseMainWindow();
        }

        private void WaitWhileCondition(int millis, bool condition, Action action = null)
        {
            int iterations = 0;
            int maxIterations = millis / 100;
            do
            {
                action?.Invoke();
                iterations++;
                Thread.Sleep(100);
            } while (iterations < maxIterations && condition);
        }

    }
}
