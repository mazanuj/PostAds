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
        private readonly byte[] flag = new byte[3];

        private bool boxMoto;
        private bool boxUsed;
        private bool boxKol;

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

            Informer.OnPostResultChanged += ChangePostResults;
            Informer.OnProxyListFromInternetUpdated += ChangeFrontPanelIsEnabledStatus;
            Informer.OnFilePathsCleared += this.ResetUiControls;
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

        private void ResetUiControls()
        {
            MotoFileLabel = false;
            SpareFileLabel = false;
            EquipFileLabel = false;
            PhotoDirLabel = false;
            CanButtonStart = false;
            BoxKol = false;
            BoxMoto = false;
            BoxUsed = false;

            NotifyOfPropertyChange(() => MotoFileLabel);
            NotifyOfPropertyChange(() => SpareFileLabel);
            NotifyOfPropertyChange(() => EquipFileLabel);
            NotifyOfPropertyChange(() => PhotoDirLabel);
            NotifyOfPropertyChange(() => CanButtonStart);
            NotifyOfPropertyChange(() => BoxKol);
            NotifyOfPropertyChange(() => BoxMoto);
            NotifyOfPropertyChange(() => BoxUsed);
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