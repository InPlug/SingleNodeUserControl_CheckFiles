using System;
using Vishnu.Interchange;
using System.Collections.ObjectModel;
using FileChecker;
using System.Linq;
using Vishnu.ViewModel;

namespace UserNodeControls
{
    /// <summary>
    /// Funktion: ViewModel für das Result eines bestimmten Checkers,
    /// hier: FileChecker.Result.
    /// </summary>
    /// <remarks>
    /// File: ResultViewModel
    /// Autor: Erik Nagel
    ///
    /// 04.06.2015 Erik Nagel: erstellt
    /// 21.07.2016 Erik Nagel: In foreach-Schleifen wegen thread-safety linq.ToList implementiert.
    /// </remarks>
    public class ResultViewModel : DynamicUserControlViewModelBase
    {
        /// <summary>
        /// ViewModel für ein Teilergebnis.
        /// </summary>
        public class SubResultViewModel : DynamicUserControlViewModelBase
        {
            /// <summary>
            /// Das logische Einzelergebnis eines Unterergebnisses.
            /// true, false oder null.
            /// </summary>
            public bool? LogicalResult
            {
                get
                {
                    return this._logicalResult;
                }
                set
                {
                    if (value != this._logicalResult)
                    {
                        this._logicalResult = value;
                        this.RaisePropertyChanged("LogicalResult");
                    }
                }
            }

            /// <summary>
            /// Der Name einer gelisteten Datei.
            /// (i.d.R "Anzahl").
            /// </summary>
            public string FileName
            {
                get
                {
                    return this._fileName;
                }
                set
                {
                    if (value != this._fileName)
                    {
                        this._fileName = value;
                        this.RaisePropertyChanged("FileName");
                    }
                }
            }

            /// <summary>
            /// Die Größe einer Detei.
            ///  </summary>
            public long FileSize
            {
                get
                {
                    return this._fileSize;
                }
                set
                {
                    if (value != this._fileSize)
                    {
                        this._fileSize = value;
                        this.RaisePropertyChanged("FileSize");
                    }
                }
            }

            /// <summary>
            /// Die Zeitspanne seit der letzten Änderung einer Detei oder
            /// die Dauer der Überwachung dieser Datei (Trace = true).
            ///  </summary>
            public TimeSpan FileAge
            {
                get
                {
                    return this._fileAge;
                }
                set
                {
                    if (value != this._fileAge)
                    {
                        this._fileAge = value;
                        this.RaisePropertyChanged("FileAge");
                    }
                }
            }

            /// <summary>
            /// Überschriebene ToString()-Methode.
            /// Liefert die Properties als String aufbereitet. 
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return String.Format("{0}: {1}, {2}", this.FileName, this.FileSize.ToString(),
                  this.FileAge.ToString(@"d\d\:h\h\:m\m\:s\s", System.Globalization.CultureInfo.InvariantCulture));
            }

            private bool? _logicalResult;
            private string _fileName;
            private long _fileSize;
            private TimeSpan _fileAge;
        }

        /// <summary>
        /// Das logische Gesamtergebnis eines Prüfprozesses:
        /// true, false oder null.
        /// </summary>
        public bool? LogicalResult
        {
            get
            {
                return this.GetResultProperty<bool?>(typeof(FileCheckerReturnObject), "LogicalResult");
            }
        }

        /// <summary>
        /// Arbeits-Modus:
        /// SIZE=Dateigröße, COUNT=Datei-Anzahl, AGE=Zeit seit letzter Änderung, TRACE=Zeit der Überwachung.
        /// </summary>
        public string Mode
        {
            get
            {
                return this.GetResultProperty<string>(typeof(FileCheckerReturnObject), "Mode");
            }
        }

        /// <summary>
        /// Die Anzahl der Dateien, die das
        /// Prüfkriterium erfüllen.
        /// </summary>
        public int CountFiles
        {
            get
            {
                return this.GetResultProperty<int>(typeof(FileCheckerReturnObject), "CountFiles");
            }
        }

        /// <summary>
        /// Das Verzeichnis aus dem übergebenen Suchpfad. 
        ///  </summary>
        public string SearchDir
        {
            get
            {
                return this.GetResultProperty<string>(typeof(FileCheckerReturnObject), "SearchDir");
            }
        }

        /// <summary>
        /// Der Dateiname aus dem übergebenen Suchpfad. 
        /// Der Dateiname kann ein regulärer Ausdruck sein.
        ///  </summary>
        public string FileMask
        {
            get
            {
                return this.GetResultProperty<string>(typeof(FileCheckerReturnObject), "FileMask");
            }
        }

        /// <summary>
        /// Übergebener Vergleichsoperator (&lt; oder &gt;).
        /// </summary>
        public string Comparer
        {
            get
            {
                return this.GetResultProperty<string>(typeof(FileCheckerReturnObject), "Comparer");
            }
        }

        /// <summary>
        /// Größe einer Datei oder Anzahl Dateien, bei deren Überschreitung
        /// oder Unterschreitung (je nach Comparer) das Ergebnis der Routine
        /// auf false geht.
        /// </summary>
        public long CriticalFileSizeOrCount
        {
            get
            {
                return this.GetResultProperty<long>(typeof(FileCheckerReturnObject), "CriticalFileSizeOrCount");
            }
        }

        /// <summary>
        /// Maximales oder minimales Alter der gefundenen Dateien
        /// (je nach Comparer) bei dem das Ergebnis der Routine auf false geht.
        ///  </summary>
        public TimeSpan CriticalFileAge
        {
            get
            {
                return this.GetResultProperty<TimeSpan>(typeof(FileCheckerReturnObject), "CriticalFileAge");
            }
        }

        /// <summary>
        /// Klartext-Informationen zur Prüfroutine
        /// (was die Routine prüft).
        ///  </summary>
        public string Comment
        {
            get
            {
                return this.GetResultProperty<string>(typeof(FileCheckerReturnObject), "Comment");
            }
        }

        /// <summary>
        /// Bis zu drei KeyValue-Paare mit jeweils DetailName (i.d.R "Anzahl"),
        /// und einem KeyValue-Paar bestehend aus DetailValue (i.d.R eine int-Anzahl)
        /// und einem Detail-Ergebnis (bool?).
        /// </summary>
        public ObservableCollection<SubResultViewModel> SubResults { get; private set; }

        /// <summary>
        /// Konstruktor - übernimmt den DataContext des zugehörigen Vishnu-Knotens.
        /// </summary>
        /// <param name="parentViewModel">DataContext des zugehörigen Vishnu-Knotens.</param>
        public ResultViewModel(IVishnuViewModel parentViewModel)
        {
            this.ParentViewModel = parentViewModel;
            if (parentViewModel != null) // wg. ReferenceNullException im DesignMode
            {
                this.ParentViewModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(parentViewModel_PropertyChanged);
            }
            this.SubResults = new ObservableCollection<SubResultViewModel>();
        }

        /// <summary>
        /// Wird ausgeführt, wenn sich die Result-Property des ViewModels
        /// des zugehörigen Vishnu-Knotens geändert hat.
        /// </summary>
        public void HandleResultPropertyChanged()
        {
            //DispatcherOperation op = this.Dispatcher.BeginInvoke(new Action(()
            this.Dispatcher.Invoke(new Action(()
            =>
            {
                this.SubResults.Clear();
                FileCheckerReturnObject.SubResultListContainer subResultListContainer =
              this.GetResultProperty<FileCheckerReturnObject.SubResultListContainer>(typeof(FileCheckerReturnObject), "SubResults");
                if (subResultListContainer != null)
                {
              // foreach (FileCheckerReturnObject.SubResult subResult in subResultListContainer.SubResults)
              foreach (FileCheckerReturnObject.SubResult subResult in subResultListContainer.SubResults.ToList())
                    {
                        SubResultViewModel subResultViewModel = new SubResultViewModel();
                        this.SubResults.Add(subResultViewModel);
                        subResultViewModel.LogicalResult = subResult.LogicalResult;
                        subResultViewModel.FileName = subResult.FileName;
                        subResultViewModel.FileSize = subResult.FileSize;
                        subResultViewModel.FileAge = subResult.FileAge;
                    }
                    this.RaisePropertyChanged("SubResults");
                }
            }));

            //DispatcherOperationStatus status = op.Status;
            //while (status != DispatcherOperationStatus.Completed)
            //{
            //  status = op.Wait(TimeSpan.FromMilliseconds(1000));
            //  if (status == DispatcherOperationStatus.Aborted)
            //  {
            //    // Alert Someone
            //  }
            //}
            //this.RaisePropertyChanged("SubResults");
            this.RaisePropertyChanged("Mode");
            this.RaisePropertyChanged("CountFiles");
            this.RaisePropertyChanged("SearchDir");
            this.RaisePropertyChanged("FileMask");
            this.RaisePropertyChanged("Comparer");
            this.RaisePropertyChanged("CriticalFileSizeOrCount");
            this.RaisePropertyChanged("CriticalFileAge");
            this.RaisePropertyChanged("Comment");
        }

        private void parentViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Result")
            {
                this.HandleResultPropertyChanged();
            }
        }

    }
}
