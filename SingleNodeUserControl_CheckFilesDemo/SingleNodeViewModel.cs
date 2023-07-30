using FileChecker;
using NetEti.MVVMini;
using System;
using System.Windows;
using System.Windows.Input;
using Vishnu.Interchange;
using Vishnu.ViewModel;

namespace SingleNodeUserControl_CheckFilesDemo
{
    /// <summary>
    /// Funktion: Demo-SingleNodeViewModel.
    /// Dient zum Test von UserNodeControls außerhalb von Vishnu.
    /// </summary>
    /// <remarks>
    /// File: SingleNodeViewModel
    /// Autor: Erik Nagel
    ///
    /// 22.05.2015 Erik Nagel: erstellt
    /// </remarks>
    public class SingleNodeViewModel : VishnuViewModelBase
    {
        /// <summary>
        /// Der Name des zugehörigen LogicalTaskTree-Knotens
        /// für die UI verfügbar gemacht.
        /// </summary>
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if (this._name != value)
                {
                    this._name = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }

        /// <summary>
        /// Das logische Ergebnis des zugehörigen LogicalTaskTree-Knotens
        /// für die UI verfügbar gemacht.
        /// </summary>
        public bool? Logical
        {
            get
            {
                return this._logical;
            }
            set
            {
                if (this._logical != value)
                {
                    this._logical = value;
                    this.RaisePropertyChanged("Logical");
                }
            }
        }

        /// <summary>
        /// Merkfeld für den letzten Zustand von Logical, der nicht null war;
        /// Wird benötigt, damit Worker nur dann gestartet werden, und die 
        /// Anzeige wechselt, wenn sich der Zustand von Logical signifikant
        /// geändert hat und nicht jedesmal, wenn der Checker arbeitet (Logical = null).
        /// </summary>
        public bool? LastNotNullLogical
        {
            get
            {
                return this._lastNotNullLogical;
            }
            set
            {
                if (this._lastNotNullLogical != value)
                {
                    this._lastNotNullLogical = value;
                    this.RaisePropertyChanged("LastNotNullLogical");
                }
            }
        }

        /// <summary>
        /// Reicht einen u.U. aus mehreren technischen Quellen kombinierten
        /// Zustand als Aufzählungstyp an die GUI (und den IValueConverter) weiter.
        /// Default: None
        /// </summary>
        public VisualNodeState VisualState
        {
            get
            {
                return this._visualState;
            }
            set
            {
                if (this._visualState != value)
                {
                    this._visualState = value;
                    this.RaisePropertyChanged("VisualState");
                }
            }
        }

        /// <summary>
        /// Kombinierter NodeWorkerState für alle zugeordneten NodeWorker.
        /// </summary>
        public VisualNodeWorkerState WorkersState
        {
            get
            {
                return this._workersState;
            }
            set
            {
                if (this._workersState != value)
                {
                    this._workersState = value;
                    this.RaisePropertyChanged("WorkersState");
                }
            }
        }

        /// <summary>
        /// Ein Text für die Anzahl der beendeten Endknoten dieses Teilbaums
        /// zur Anzeige im ProgressBar (i.d.R. nnn%) für die UI verfügbar gemacht.
        /// </summary>
        public string ProgressText
        {
            get
            {
                return this._progressText;
            }
            set
            {
                if (this._progressText != value)
                {
                    this._progressText = value;
                    this.RaisePropertyChanged("ProgressText");
                }
            }
        }

        /// <summary>
        /// Die Anzahl der beendeten Endknoten dieses Teilbaums
        /// für die UI verfügbar gemacht.
        /// </summary>
        public int SingleNodesFinished
        {
            get
            {
                return this._singleNodesFinished;
            }
            set
            {
                if (this._singleNodesFinished != value)
                {
                    this._singleNodesFinished = value;
                    this.RaisePropertyChanged("SingleNodesFinished");
                    this.RaisePropertyChanged("ProgressText");
                }
            }
        }

        /// <summary>
        /// Das ReturnObject der zugeordneten LogicalNode.
        /// </summary>
        public override Result? Result
        {
            get
            {
                return this._result;
            }
            set
            {
                if (this._result != value)
                {
                    this._result = value;
                    this.RaisePropertyChanged("Result");
                }
            }
        }

        /// <summary>
        /// Zeitpunkt des letzten Starts des Knoten als String.
        /// </summary>
        public string LastRunInfo
        {
            get
            {
                return this._lastRun.ToString();
            }
            set
            {
                this.RaisePropertyChanged("LastRunInfo");
            }
        }

        /// <summary>
        /// Kombinierte Ausgabe von NextRunInfo (wann ist der nächsta Durchlauf)
        /// und Result (in voller Länge).
        /// </summary>
        public string NextRunInfoAndResult
        {
            get
            {
                return String.Format("nächster Lauf: {0}\nletztes Ergebnis: {1}", "- Demo, kein nächster Lauf -", (this.Result == null ? "" : this.Result.ToString()));
            }
        }

        /// <summary>
        /// Liefert das Ergebnis von GetToolTipInfo().
        /// Diese Routine zeigt per Default auf NextRunInfoAndResult,
        /// kann aber gegebenenfalls überschrieben werden.
        /// </summary>
        public string ToolTipInfo
        {
            get
            {
                return this.GetToolTipInfo();
            }
        }

        /// <summary>
        /// True zeigt an, dass es sich um einen Knoten innerhalb
        /// eines geladenen Snapshots handelt.
        /// </summary>
        public bool IsSnapshotDummy
        {
            get
            {
                return this._isSnapshotDummy;
            }
            set
            {
                if (this._isSnapshotDummy != value)
                {
                    this._isSnapshotDummy = value;
                    this.RaisePropertyChanged("IsSnapshotDummy");
                }
            }
        }

        /// <summary>
        /// Veröffentlicht einen ButtonText entsprechend this._myLogicalNode.CanTreeStart.
        /// </summary>
        public string ButtonRunText
        {
            get
            {
                return "Run";
            }
            set
            {
                this.RaisePropertyChanged("ButtonRunText");
            }
        }

        /// <summary>
        /// Veröffentlicht einen ButtonText entsprechend this._myLogicalNode.CanTreeStart.
        /// </summary>
        public string ButtonBreakText
        {
            get
            {
                return "Break";
            }
            set
            {
                this.RaisePropertyChanged("ButtonBreakText");
            }
        }

        /// <summary>
        /// Command für den Run-Button im LogicalTaskTreeControl.
        /// </summary>
        public ICommand RunLogicalTaskTree { get { return this._btnRunTaskTreeRelayCommand; } }

        /// <summary>
        /// Command für den Break-Button im LogicalTaskTreeControl.
        /// </summary>
        public ICommand BreakLogicalTaskTree { get { return this._btnBreakTaskTreeRelayCommand; } }

        /// <summary>
        /// Command für den Copy-Button im ToolTip des Controls.
        /// </summary>
        public ICommand CopyToolTipInfoToClipboard { get { return this._btnCopyToolTipInfoToClipboardCommand; } }

        /// <summary>
        /// Liefert das Ergebnis für die Property ToolTipInfo.
        /// Diese Routine zeigt per Default auf NextRunInfoAndResult,
        /// kann aber gegebenenfalls überschrieben werden.
        /// </summary>
        protected virtual string GetToolTipInfo()
        {
            return this.NextRunInfoAndResult;
        }

        /// <summary>
        /// Standard Konstruktor - setzt alle Demo-Properties.
        /// </summary>
        public SingleNodeViewModel()
        {
            this._name = "Demo-View";
            this.Logical = false;
            this.LastNotNullLogical = false;
            this.VisualState = VisualNodeState.Done;
            this.WorkersState = VisualNodeWorkerState.Valid;
            this.SingleNodesFinished = 100;
            this._progressText = "100 %";
            this.IsSnapshotDummy = false;
            this._lastRun = DateTime.Now;
            this._btnRunTaskTreeRelayCommand = new RelayCommand(runTaskTreeExecute, canRunTaskTreeExecute);
            this._btnBreakTaskTreeRelayCommand = new RelayCommand(breakTaskTreeExecute, canBreakTaskTreeExecute);
            this._btnCopyToolTipInfoToClipboardCommand = new RelayCommand(this.CmdBtnCopy_Executed, this.CmdBtnCopy_CanExecute);
            FileCheckerReturnObject.SubResultListContainer subResultListContainer = new FileCheckerReturnObject.SubResultListContainer();
            subResultListContainer.SubResults?.Add(new FileCheckerReturnObject.SubResult() { FileName = "TestFile_1", FileAge = TimeSpan.FromMinutes(101), FileSize = 1001, LogicalResult = false });
            subResultListContainer.SubResults?.Add(new FileCheckerReturnObject.SubResult() { FileName = "TestFile_2 this is a testfile with an extraordinary wide name. It should trigger the Horizontal ScrollBar of the containing control.", FileAge = TimeSpan.FromMinutes(102), FileSize = 1002, LogicalResult = false });
            subResultListContainer.SubResults?.Add(new FileCheckerReturnObject.SubResult() { FileName = "TestFile_3", FileAge = TimeSpan.FromMinutes(103), FileSize = 1003, LogicalResult = false });
            subResultListContainer.SubResults?.Add(new FileCheckerReturnObject.SubResult() { FileName = "TestFile_4", FileAge = TimeSpan.FromMinutes(104), FileSize = 1004, LogicalResult = false });
            subResultListContainer.SubResults?.Add(new FileCheckerReturnObject.SubResult() { FileName = "TestFile_5", FileAge = TimeSpan.FromMinutes(105), FileSize = 1005, LogicalResult = false });
            subResultListContainer.SubResults?.Add(new FileCheckerReturnObject.SubResult() { FileName = "TestFile_6", FileAge = TimeSpan.FromMinutes(106), FileSize = 1006, LogicalResult = false });
            subResultListContainer.SubResults?.Add(new FileCheckerReturnObject.SubResult() { FileName = "TestFile_7", FileAge = TimeSpan.FromMinutes(107), FileSize = 1007, LogicalResult = false });
            subResultListContainer.SubResults?.Add(new FileCheckerReturnObject.SubResult() { FileName = "TestFile_8", FileAge = TimeSpan.FromMinutes(108), FileSize = 1008, LogicalResult = false });
            subResultListContainer.SubResults?.Add(new FileCheckerReturnObject.SubResult() { FileName = "TestFile_9", FileAge = TimeSpan.FromMinutes(109), FileSize = 1009, LogicalResult = false });
            this._returnObject = new FileCheckerReturnObject()
            {
                LogicalResult = false,
                Mode = "TRACE",
                //SearchDir = @"..\..\TestFiles xxxxxxxTest der Parameter-Scrollbarxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
                SearchDir = @"..\..\TestFiles",
                FileMask = @".*\.pdx",
                Comparer = ">",
                CountFiles = 3,
                CriticalFileSizeOrCount = 2,
                CriticalFileAge = TimeSpan.FromSeconds(20),
                SubResults = subResultListContainer,
                Comment = "Dies ist ein Test für FileChecker und sein UserNodeControl."
            };
            this.Result = new Result("TestResult", true, NodeState.Finished, NodeLogicalState.Done, this._returnObject);
        }

        public void RaiseAllProperties()
        {
            this.RaisePropertyChanged("Name");
            this.RaisePropertyChanged("Logical");
            this.RaisePropertyChanged("LastNotNullLogical");
            this.RaisePropertyChanged("VisualState");
            this.RaisePropertyChanged("WorkersState");
            this.RaisePropertyChanged("SingleNodesFinished");
            this.RaisePropertyChanged("ProgressText");
            this.RaisePropertyChanged("IsSnapshotDummy");
            this.RaisePropertyChanged("Result");
            this.RaisePropertyChanged("LastRunInfo");
            this.RaisePropertyChanged("NextRunInfoAndResult");
            this.RaisePropertyChanged("ButtonRunText");
            this.RaisePropertyChanged("ButtonBreakText");
        }

        private Vishnu.Interchange.Result? _result;
        private FileCheckerReturnObject _returnObject;
        private bool? _logical;
        private bool? _lastNotNullLogical;
        private VisualNodeState _visualState;
        private string _progressText;
        private int _singleNodesFinished;
        private string _name;
        private VisualNodeWorkerState _workersState;
        private RelayCommand _btnRunTaskTreeRelayCommand;
        private RelayCommand _btnBreakTaskTreeRelayCommand;
        private RelayCommand _btnCopyToolTipInfoToClipboardCommand;
        private DateTime _lastRun;
        private bool _isSnapshotDummy;

        private void runTaskTreeExecute(object? parameter)
        {
            this._lastRun = DateTime.Now;
            this.Logical = true;
            this.LastNotNullLogical = true;
            this.VisualState = VisualNodeState.Done;
            this.WorkersState = VisualNodeWorkerState.Valid;
            this.SingleNodesFinished = 100;
            this.ProgressText = "100 %";
            this.RaiseAllProperties();
        }

        private bool canRunTaskTreeExecute()
        {
            return true;
        }

        private void breakTaskTreeExecute(object? parameter)
        {
            this.Logical = null;
            this.LastNotNullLogical = null;
            this.VisualState = VisualNodeState.Aborted;
            this.WorkersState = VisualNodeWorkerState.Valid;
            this.SingleNodesFinished = 0;
            this.ProgressText = "0 %";
            this.RaiseAllProperties();
        }

        private bool canBreakTaskTreeExecute()
        {
            return true;
        }

        private void CmdBtnCopy_Executed(object? parameter)
        {
            Clipboard.SetText(this.NextRunInfoAndResult);
        }

        private bool CmdBtnCopy_CanExecute()
        {
            return true;
        }

    }
}
