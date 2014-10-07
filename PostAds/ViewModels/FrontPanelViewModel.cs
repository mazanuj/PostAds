using System.Windows.Forms;
using LogManager = NLog.LogManager;

namespace Motorcycle.ViewModels
{
    using Caliburn.Micro;
    using Microsoft.Win32;
    using Config;
    using Utils;
    using NLog;
    using System;
    using System.ComponentModel.Composition;
    using XmlWorker;
    using LogManager = LogManager;


    [Export(typeof(FrontPanelViewModel))]
    public class FrontPanelViewModel : PropertyChangedBase
    {
        private readonly Logger log = LogManager.GetCurrentClassLogger();
        public LoggingControlViewModel LoggingControl { get; private set; }
        private readonly OpenFileDialog dlg;
        private readonly FolderBrowserDialog fbd;
        private readonly byte[] flag = new byte[3];

        private bool boxMoto;
        private bool boxUsed;
        private bool boxKol;
        private bool boxPhoto;

        [ImportingConstructor]
        public FrontPanelViewModel(LoggingControlViewModel loggingControlModel)
        {
            LoggingControl = loggingControlModel;

            CanEditFrontPanel = true;
            // Create OpenFileDialog
            dlg = new OpenFileDialog
            {
                DefaultExt = ".txt",
                Filter = "Text documents (.txt)|*.txt",
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory
            };

            fbd = new FolderBrowserDialog
            {
                ShowNewFolderButton = false,
                SelectedPath = AppDomain.CurrentDomain.BaseDirectory
            };

            Informer.OnPostResultChanged += ChangePostResults;
            Informer.OnProxyListFromInternetUpdated += ChangeFrontPanelIsEnabledStatus;
            Informer.OnFilePathsCleared += ResetFileLabels;
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

        public bool BoxPhoto
        {
            get { return boxPhoto; }
            set
            {
                boxPhoto = value;
                NotifyOfPropertyChange(() => BoxPhoto);

                CanButtonStart = CheckIfAllFieldsAreFilled();
                NotifyOfPropertyChange(() => CanButtonStart);
            }
        }

        public bool MotoFileLabel { get; set; }

        public bool SpareFileLabel { get; set; }

        public bool EquipFileLabel { get; set; }

        public bool PhotoDirLabel { get; set; }

        public bool CanButtonStart { get; set; }

        public bool CanEditFrontPanel { get; set; }

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

        private void ResetFileLabels()
        {
            MotoFileLabel = false;
            SpareFileLabel = false;
            EquipFileLabel = false;

            NotifyOfPropertyChange(() => MotoFileLabel);
            NotifyOfPropertyChange(() => SpareFileLabel);
            NotifyOfPropertyChange(() => EquipFileLabel);
        }

        private bool CheckIfAllFieldsAreFilled()
        {
            return (MotoFileLabel || SpareFileLabel || EquipFileLabel) && (flag[0] + flag[1] + flag[2] != 0);
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

            await Advertising.Initialize(flag);

            CanButtonStart = true;
            NotifyOfPropertyChange(() => CanButtonStart);
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
            if (fbd.ShowDialog() != DialogResult.OK && fbd.SelectedPath == string.Empty) return;
            FilePathXmlWorker.SetFilePath("photo", fbd.SelectedPath + @"\");

            PhotoDirLabel = true;
            NotifyOfPropertyChange(() => PhotoDirLabel);

            CanButtonStart = CheckIfAllFieldsAreFilled();
            NotifyOfPropertyChange(() => CanButtonStart);
        }
    }
}