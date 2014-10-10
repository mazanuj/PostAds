using System.Windows.Forms;
using LogManager = NLog.LogManager;

namespace Motorcycle.ViewModels
{
    using Caliburn.Micro;
    using Config;
    using Microsoft.Win32;
    using Motorcycle.Config.Data;
    using Motorcycle.TimerScheduler;
    using NLog;
    using System;
    using System.ComponentModel.Composition;
    using Utils;
    using XmlWorker;
    using LogManager = LogManager;

    [Export(typeof(FrontPanelViewModel))]
    public class FrontPanelViewModel : PropertyChangedBase
    {
        private readonly Logger log = LogManager.GetCurrentClassLogger();
        public LoggingControlViewModel LoggingControl { get; private set; }
        private readonly OpenFileDialog dlg;
        private readonly byte[] flag = new byte[3];

        private bool boxMoto;
        private bool boxUsed;
        private bool boxKol;

        private byte motosaleFrom;
        private byte usedAutoFrom;
        private byte prodayFrom;

        private byte motosaleTo;
        private byte usedAutoTo;
        private byte prodayTo;

        private int motosaleInterval;
        private int usedAutoInterval;
        private int prodayInterval;

        [ImportingConstructor]
        public FrontPanelViewModel(LoggingControlViewModel loggingControlModel)
        {
            LoggingControl = loggingControlModel;

            CanEditFrontPanel = true;
            CanEditMainSettings = true;
            // Create OpenFileDialog
            dlg = new OpenFileDialog
            {
                DefaultExt = ".txt",
                Filter = "Text documents (.txt)|*.txt",
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory
            };

            Informer.OnPostResultChanged += ChangePostResults;
            Informer.OnProxyListFromInternetUpdated += ChangeFrontPanelIsEnabledStatus;
            Informer.OnAllPostsAreCompleted += ResetUiControlsAndCleatFiles;

            MotosaleInterval = 1;
            UsedAutoInterval = 1;
            ProdayInterval = 1;
            NotifyOfPropertyChange(() => MotosaleInterval);
            NotifyOfPropertyChange(() => UsedAutoInterval);
            NotifyOfPropertyChange(() => ProdayInterval);
        }

        public int CountSuccess { get; set; }

        public int CountFailure { get; set; }

        public bool BoxMoto
        {
            get { return boxMoto; }
            set
            {
                boxMoto = value;
                flag[0] = boxMoto ? flag[0] += 1 : flag[0] -= 1;
                NotifyOfPropertyChange(() => BoxMoto);

                CanButtonStart = CheckIfAllFieldsAreFilled();
                NotifyOfPropertyChange(() => CanButtonStart);
            }
        }

        public bool BoxUsed
        {
            get { return boxUsed; }
            set
            {
                boxUsed = value;
                flag[1] = boxUsed ? flag[1] += 1 : flag[1] -= 1;
                NotifyOfPropertyChange(() => BoxUsed);

                CanButtonStart = CheckIfAllFieldsAreFilled();
                NotifyOfPropertyChange(() => CanButtonStart);
            }
        }

        public bool BoxKol
        {
            get { return boxKol; }
            set
            {
                boxKol = value;
                flag[2] = boxKol ? flag[2] += 1 : flag[2] -= 1;
                NotifyOfPropertyChange(() => BoxKol);

                CanButtonStart = CheckIfAllFieldsAreFilled();
                NotifyOfPropertyChange(() => CanButtonStart);
            }
        }

        public byte MotosaleFrom
        {
            get
            {
                return motosaleFrom;
            }
            set
            {
                motosaleFrom = value;
                MotosaleFromLabel = value;
                NotifyOfPropertyChange(() => MotosaleFromLabel);

                CanButtonStart = CheckIfAllFieldsAreFilled();
                NotifyOfPropertyChange(() => CanButtonStart);
            }
        }

        public byte UsedAutoFrom
        {
            get
            {
                return usedAutoFrom;
            }
            set
            {
                usedAutoFrom = value;
                UsedAutoFromLabel = value;
                NotifyOfPropertyChange(() => UsedAutoFromLabel);

                CanButtonStart = CheckIfAllFieldsAreFilled();
                NotifyOfPropertyChange(() => CanButtonStart);
            }
        }

        public byte ProdayFrom
        {
            get
            {
                return prodayFrom;
            }
            set
            {
                prodayFrom = value;
                ProdayFromLabel = value;
                NotifyOfPropertyChange(() => ProdayFromLabel);

                CanButtonStart = CheckIfAllFieldsAreFilled();
                NotifyOfPropertyChange(() => CanButtonStart);
            }
        }

        public byte MotosaleTo
        {
            get
            {
                return motosaleTo;
            }
            set
            {
                motosaleTo = value;
                MotosaleToLabel = value;
                NotifyOfPropertyChange(() => MotosaleToLabel);

                CanButtonStart = CheckIfAllFieldsAreFilled();
                NotifyOfPropertyChange(() => CanButtonStart);
            }
        }

        public byte UsedAutoTo
        {
            get
            {
                return usedAutoTo;
            }
            set
            {
                usedAutoTo = value;
                UsedAutoToLabel = value;
                NotifyOfPropertyChange(() => UsedAutoToLabel);

                CanButtonStart = CheckIfAllFieldsAreFilled();
                NotifyOfPropertyChange(() => CanButtonStart);
            }
        }

        public byte ProdayTo
        {
            get
            {
                return prodayTo;
            }
            set
            {
                prodayTo = value;
                ProdayToLabel = value;
                NotifyOfPropertyChange(() => ProdayToLabel);

                CanButtonStart = CheckIfAllFieldsAreFilled();
                NotifyOfPropertyChange(() => CanButtonStart);
            }
        }

        public int MotosaleInterval
        {
            get
            {
                return motosaleInterval;
            }
            set
            {
                motosaleInterval = value;
                MotosaleIntervalLabel = value;
                NotifyOfPropertyChange(() => MotosaleIntervalLabel);
            }
        }

        public int UsedAutoInterval
        {
            get
            {
                return usedAutoInterval;
            }
            set
            {
                usedAutoInterval = value;
                UsedAutoIntervalLabel = value;
                NotifyOfPropertyChange(() => UsedAutoIntervalLabel);
            }
        }

        public int ProdayInterval
        {
            get
            {
                return prodayInterval;
            }
            set
            {
                prodayInterval = value;
                ProdayIntervalLabel = value;
                NotifyOfPropertyChange(() => ProdayIntervalLabel);
            }
        }

        public int MotosaleFromLabel { get; set; }
        public int UsedAutoFromLabel { get; set; }
        public int ProdayFromLabel { get; set; }

        public int MotosaleToLabel { get; set; }
        public int UsedAutoToLabel { get; set; }
        public int ProdayToLabel { get; set; }

        public int MotosaleIntervalLabel { get; set; }
        public int UsedAutoIntervalLabel { get; set; }
        public int ProdayIntervalLabel { get; set; }
        public bool MotoFileLabel { get; set; }

        public bool SpareFileLabel { get; set; }

        public bool EquipFileLabel { get; set; }

        public bool PhotoDirLabel { get; set; }

        public bool CanButtonStart { get; set; }

        public bool CanButtonStop { get; set; }

        public bool CanEditFrontPanel { get; set; }

        public bool CanEditMainSettings { get; set; }

        private void ChangePostResults(bool postResult)
        {
            if (postResult)
            {
                CountSuccess++;
                NotifyOfPropertyChange(() => CountSuccess);
            }
            else
            {
                CountFailure++;
                NotifyOfPropertyChange(() => CountFailure);
            }
        }

        private void ChangeFrontPanelIsEnabledStatus(bool result)
        {
            CanEditFrontPanel = result;
            NotifyOfPropertyChange(() => CanEditFrontPanel);
        }

        private void ResetUiControlsAndCleatFiles()
        {
            FileCleaner.RemoveEmptyLinesFromAllFiles();
            FilePathXmlWorker.ResetFilePaths();

            MotoFileLabel = false;
            SpareFileLabel = false;
            EquipFileLabel = false;
            PhotoDirLabel = false;
            CanButtonStart = false;
            BoxKol = false;
            BoxMoto = false;
            BoxUsed = false;
            CanEditMainSettings = true;
            CanButtonStop = false;

            NotifyOfPropertyChange(() => MotoFileLabel);
            NotifyOfPropertyChange(() => SpareFileLabel);
            NotifyOfPropertyChange(() => EquipFileLabel);
            NotifyOfPropertyChange(() => PhotoDirLabel);
            NotifyOfPropertyChange(() => CanButtonStart);
            NotifyOfPropertyChange(() => BoxKol);
            NotifyOfPropertyChange(() => BoxMoto);
            NotifyOfPropertyChange(() => BoxUsed);
            NotifyOfPropertyChange(() => CanEditMainSettings);
            NotifyOfPropertyChange(() => CanButtonStop);
        }

        private bool CheckIfAllFieldsAreFilled()
        {
            var motosaleCorrect = true;
            var usedautoCorrect = true;
            var prodayCorrect = true;

            if (flag[0] > 0) motosaleCorrect = MotosaleFrom != MotosaleTo;
            if (flag[1] > 0) usedautoCorrect = UsedAutoFrom != UsedAutoTo;
            if (flag[2] > 0) prodayCorrect = ProdayFrom != ProdayTo;

            return (MotoFileLabel || SpareFileLabel || EquipFileLabel) && (flag[0] + flag[1] + flag[2] != 0)
                && motosaleCorrect && usedautoCorrect && prodayCorrect;
        }

        private void ResetPostResultStatistic()
        {
            CountSuccess = CountFailure = 0;
            NotifyOfPropertyChange(() => CountSuccess);
            NotifyOfPropertyChange(() => CountFailure);
        }

        public async void ButtonStart()
        {
            ResetPostResultStatistic();

            CanButtonStart = false;
            NotifyOfPropertyChange(() => CanButtonStart);
            CanButtonStop = true;
            NotifyOfPropertyChange(() => CanButtonStop);
            CanEditMainSettings = false;
            NotifyOfPropertyChange(() => CanEditMainSettings);

            var timerParams = new TimerSchedulerParams
            {
                MotosaleFrom = this.MotosaleFrom,
                MotosaleInterval = this.MotosaleInterval,
                MotosaleTo = this.MotosaleTo,
                ProdayFrom = this.ProdayFrom,
                ProdayInterval = this.ProdayInterval,
                ProdayTo = this.ProdayTo,
                UsedAutoFrom = this.UsedAutoFrom,
                UsedAutoInterval = this.UsedAutoInterval,
                UsedAutoTo = this.UsedAutoTo
            };

            await Advertising.Initialize(flag, timerParams);
        }

        public void ButtonStop()
        {
            MotosalePostScheduler.StopPostMsgWithTimer();
            UsedAutoPostScheduler.StopPostMsgWithTimer();
            ProdayPostScheduler.StopPostMsgWithTimer();

            CanButtonStart = true;
            NotifyOfPropertyChange(() => CanButtonStart);
            CanButtonStop = false;
            NotifyOfPropertyChange(() => CanButtonStop);
            CanEditMainSettings = true;
            NotifyOfPropertyChange(() => CanEditMainSettings);

            ResetUiControlsAndCleatFiles();
        }

        public void ButtonMoto()
        {
            if (dlg.ShowDialog() == false) return;
            FilePathXmlWorker.SetFilePath("moto", dlg.FileName);

            MotoFileLabel = true;
            NotifyOfPropertyChange(() => MotoFileLabel);

            CanButtonStart = CheckIfAllFieldsAreFilled();
            NotifyOfPropertyChange(() => CanButtonStart);
        }

        public void ButtonSpare()
        {
            if (dlg.ShowDialog() == false) return;
            FilePathXmlWorker.SetFilePath("spare", dlg.FileName);

            SpareFileLabel = true;
            NotifyOfPropertyChange(() => SpareFileLabel);

            CanButtonStart = CheckIfAllFieldsAreFilled();
            NotifyOfPropertyChange(() => CanButtonStart);
        }

        public void ButtonEquip()
        {
            if (dlg.ShowDialog() == false) return;
            FilePathXmlWorker.SetFilePath("equip", dlg.FileName);

            EquipFileLabel = true;
            NotifyOfPropertyChange(() => EquipFileLabel);

            CanButtonStart = CheckIfAllFieldsAreFilled();
            NotifyOfPropertyChange(() => CanButtonStart);
        }

        public void ButtonPhotoDir()
        {
            var fbd = new FolderBrowserDialog
            {
                ShowNewFolderButton = false,
                SelectedPath = AppDomain.CurrentDomain.BaseDirectory
            };

            if (fbd.ShowDialog() != DialogResult.OK && fbd.SelectedPath == string.Empty) return;
            FilePathXmlWorker.SetFilePath("photo", fbd.SelectedPath + @"\");

            PhotoDirLabel = true;
            NotifyOfPropertyChange(() => PhotoDirLabel);

            CanButtonStart = CheckIfAllFieldsAreFilled();
            NotifyOfPropertyChange(() => CanButtonStart);
        }
    }
}