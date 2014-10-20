using System.Windows.Forms;
using LogManager = NLog.LogManager;

namespace Motorcycle.ViewModels
{
    using Caliburn.Micro;
    using Config;
    using Microsoft.Win32;
    using Config.Data;

    using Motorcycle.TimerScheduler.Old;

    using TimerScheduler;

    using System;
    using System.ComponentModel.Composition;
    using Utils;
    using XmlWorker;

    [Export(typeof(FrontPanelViewModel))]
    public class FrontPanelViewModel : PropertyChangedBase
    {
        public LoggingControlViewModel LoggingControl { get; set; }
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
            CaptchaLabel = true;
            // Create OpenFileDialog
            dlg = new OpenFileDialog
            {
                DefaultExt = ".txt",
                Filter = "Text documents (.txt)|*.txt",
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory
            };

            Informer.OnPostResultChanged += ChangePostResults;
            Informer.OnProxyListFromInternetUpdated += ChangeFrontPanelIsEnabledStatus;
            Informer.OnAllPostsAreCompleted += ResetUiControlsAndClearFiles;
            Informer.OnMotosalePostsAreCompleted += () =>
                {
                    IsMotosaleFinishStatusVisible = true;
                    NotifyOfPropertyChange(() => IsMotosaleFinishStatusVisible);
                };
            Informer.OnProdayPostsAreCompleted += () =>
                {
                    IsProdayFinishStatusVisible = true;
                    NotifyOfPropertyChange(() => IsProdayFinishStatusVisible);
                };
            Informer.OnUsedAutoPostsAreCompleted += () =>
                {
                    IsUsedautoFinishStatusVisible = true;
                    NotifyOfPropertyChange(() => IsUsedautoFinishStatusVisible);
                };
            Informer.OnCaptchaStatusChanged += result =>
                {
                    CaptchaLabel = result;
                    NotifyOfPropertyChange(() => CaptchaLabel);
                };


            LoadTimersValuesFromXml();
        }

        public bool IsMotosaleFinishStatusVisible { get; set; }
        public bool IsUsedautoFinishStatusVisible { get; set; }
        public bool IsProdayFinishStatusVisible { get; set; }

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
            get { return motosaleFrom; }
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
            get { return usedAutoFrom; }
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
            get { return prodayFrom; }
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
            get { return motosaleTo; }
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
            get { return usedAutoTo; }
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
            get { return prodayTo; }
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
            get { return motosaleInterval; }
            set
            {
                motosaleInterval = value;
                MotosaleIntervalLabel = value;
                NotifyOfPropertyChange(() => MotosaleIntervalLabel);
            }
        }

        public int UsedAutoInterval
        {
            get { return usedAutoInterval; }
            set
            {
                usedAutoInterval = value;
                UsedAutoIntervalLabel = value;
                NotifyOfPropertyChange(() => UsedAutoIntervalLabel);
            }
        }

        public int ProdayInterval
        {
            get { return prodayInterval; }
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

        public bool CaptchaLabel { get; set; }

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

        private void ResetUiControlsAndClearFiles()
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
            flag[0] = flag[1] = flag[2] = 0;

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

        private void ResetFinishStatusValues()
        {
            IsProdayFinishStatusVisible = false;
            IsMotosaleFinishStatusVisible = false;
            IsUsedautoFinishStatusVisible = false;
            NotifyOfPropertyChange(() => IsProdayFinishStatusVisible);
            NotifyOfPropertyChange(() => IsMotosaleFinishStatusVisible);
            NotifyOfPropertyChange(() => IsUsedautoFinishStatusVisible);
        }

        private bool CheckIfAllFieldsAreFilled()
        {
            var tempResult = (MotoFileLabel || SpareFileLabel || EquipFileLabel) && (flag[0] + flag[1] + flag[2] != 0) && PhotoDirLabel;
            if (flag[0] > 0) tempResult = tempResult && MotosaleFrom != MotosaleTo;
            if (flag[1] > 0) tempResult = tempResult && UsedAutoFrom != UsedAutoTo;
            if (flag[2] > 0) tempResult = tempResult && ProdayFrom != ProdayTo;

            return tempResult;
        }

        private void ResetPostResultStatistic()
        {
            CountSuccess = CountFailure = 0;
            NotifyOfPropertyChange(() => CountSuccess);
            NotifyOfPropertyChange(() => CountFailure);
        }

        private void LoadTimersValuesFromXml()
        {
            MotosaleInterval = TimerXmlWorker.GetTimerValue("motosale", "interval");
            UsedAutoInterval = TimerXmlWorker.GetTimerValue("usedauto", "interval");
            ProdayInterval = TimerXmlWorker.GetTimerValue("proday", "interval");

            MotosaleFrom = (byte)TimerXmlWorker.GetTimerValue("motosale", "from");
            UsedAutoFrom = (byte)TimerXmlWorker.GetTimerValue("usedauto", "from");
            ProdayFrom = (byte)TimerXmlWorker.GetTimerValue("proday", "from");

            MotosaleTo = (byte)TimerXmlWorker.GetTimerValue("motosale", "to");
            UsedAutoTo = (byte)TimerXmlWorker.GetTimerValue("usedauto", "to");
            ProdayTo = (byte)TimerXmlWorker.GetTimerValue("proday", "to");

            NotifyOfPropertyChange(() => MotosaleInterval);
            NotifyOfPropertyChange(() => UsedAutoInterval);
            NotifyOfPropertyChange(() => ProdayInterval);

            NotifyOfPropertyChange(() => MotosaleFrom);
            NotifyOfPropertyChange(() => UsedAutoFrom);
            NotifyOfPropertyChange(() => ProdayFrom);

            NotifyOfPropertyChange(() => MotosaleTo);
            NotifyOfPropertyChange(() => UsedAutoTo);
            NotifyOfPropertyChange(() => ProdayTo);
        }

        private void SaveTimersValuesToXml()
        {
            TimerXmlWorker.SetTimerValue("motosale", "interval", (byte)MotosaleInterval);
            TimerXmlWorker.SetTimerValue("motosale", "from", MotosaleFrom);
            TimerXmlWorker.SetTimerValue("motosale", "to", MotosaleTo);

            TimerXmlWorker.SetTimerValue("usedauto", "interval", (byte)UsedAutoInterval);
            TimerXmlWorker.SetTimerValue("usedauto", "from", UsedAutoFrom);
            TimerXmlWorker.SetTimerValue("usedauto", "to", UsedAutoTo);

            TimerXmlWorker.SetTimerValue("proday", "interval", (byte)ProdayInterval);
            TimerXmlWorker.SetTimerValue("proday", "from", ProdayFrom);
            TimerXmlWorker.SetTimerValue("proday", "to", ProdayTo);
        }

        public async void ButtonStart()
        {
            ResetPostResultStatistic();
            ResetFinishStatusValues();
            SaveTimersValuesToXml();

            CanButtonStart = false;
            NotifyOfPropertyChange(() => CanButtonStart);
            CanButtonStop = true;
            NotifyOfPropertyChange(() => CanButtonStop);
            CanEditMainSettings = false;
            NotifyOfPropertyChange(() => CanEditMainSettings);

            var timerParams = new TimerSchedulerParams
            {
                MotosaleFrom = MotosaleFrom,
                MotosaleInterval = MotosaleInterval,
                MotosaleTo = MotosaleTo,
                ProdayFrom = ProdayFrom,
                ProdayInterval = ProdayInterval,
                ProdayTo = ProdayTo,
                UsedAutoFrom = UsedAutoFrom,
                UsedAutoInterval = UsedAutoInterval,
                UsedAutoTo = UsedAutoTo
            };

            await Advertising.Initialize(flag, timerParams);
        }

        public void ButtonStop()
        {
            Informer.RaiseStopTimerClickedEvent();

            CanButtonStart = true;
            NotifyOfPropertyChange(() => CanButtonStart);
            CanButtonStop = false;
            NotifyOfPropertyChange(() => CanButtonStop);
            CanEditMainSettings = true;
            NotifyOfPropertyChange(() => CanEditMainSettings);

            ResetUiControlsAndClearFiles();
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